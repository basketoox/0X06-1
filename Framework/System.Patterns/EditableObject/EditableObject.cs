using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace System.Patterns
{
    //对于IEditableObject的实现，应该满足一下要求：
    //1. 具有NonEditableAttribute标记的属性不参与编辑
    //2. 如果某个属性类型也实现了IEditableObject， 那么将递归调用相应编辑方法。
    //3. 对于集合对象，如果集合对象实现了IEditableObject，将会对集合的每个项调用相应编辑方法。
    //4. 可以查询对象是否改变，包括任何标量属性的变化，关联的IEditableObject类型的属性的变化，集合属性的变化。
    //下面是具体实现：
    //首先要定义NonEditableAttribute类：
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class NonEditableAttribute : Attribute
    {
    }

    //其次是一个辅助类，用于找到一个类型内的标量属性，可编辑对象属性和集合属性，因为这三种属性需要不同的处理方式：
    internal class EditableProperty
    {
        public EditableProperty(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            Scalars = new List<PropertyInfo>();
            Editables = new List<PropertyInfo>();
            Collections = new List<PropertyInfo>();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                //忽略定义了NonEditableAttribute的属性。
                if (property.IsDefined(typeof (NonEditableAttribute), false))
                {
                    continue;
                }
                //不能读的属性不参与编辑
                if (!property.CanRead)
                {
                    continue;
                }
                var propertyType = property.PropertyType;
                if (propertyType.IsValueType || propertyType == typeof (string))
                {
                    //标量属性需要是值类型或者string类型，并且可写。
                    if (property.CanWrite)
                    {
                        Scalars.Add(property);
                    }
                }
                    //可编辑对象属性是递归参与编辑流程的。
                else if ((typeof (IEditableObject).IsAssignableFrom(propertyType)))
                {
                    Editables.Add(property);
                }
                    //集合属性也是参与编辑流程的。
                else if (typeof (IList).IsAssignableFrom(propertyType))
                {
                    Collections.Add(property);
                }
            }
        }

        public List<PropertyInfo> Scalars { get; private set; }
        public List<PropertyInfo> Editables { get; private set; }
        public List<PropertyInfo> Collections { get; private set; }
    }

    /// <summary>
    ///     通用可编辑对象
    /// </summary>
    /// <see cref="http://blog.csdn.net/gentle_wolf" />
    [Serializable]
    public abstract class EditableObject : NotifiableObject, IEditableObject
    {
        //缓存可编辑属性，不用每次重新获取这些元数据
        private static readonly ConcurrentDictionary<Type, EditableProperty> _cachedEditableProperties;

        private bool _isEditing;
        private object _stub;

        static EditableObject()
        {
            _cachedEditableProperties = new ConcurrentDictionary<Type, EditableProperty>();
        }

        //对象是不是处于编辑状态。
        [NonEditable]
        public bool IsEditing
        {
            get { return _isEditing; }
            protected set
            {
                if (_isEditing != value)
                {
                    _isEditing = value;
                    OnPropertyChanged("IsEditing");
                }
            }
        }

        //获取对象是不是改变了，比如说，调用了BeginEdit但是并没有修改任何属性，对象就没有改变，
        //此时不需要保存,检查修改的时候内部做了对象相互引用造成的无穷递归情况。所以即使对象有相互应用
        //也能正确检测。
        [NonEditable]
        public bool IsChanged
        {
            get { return GetIsChanged(new HashSet<EditableObject>()); }
        }

        /// <summary>
        ///     开始编辑，创建当前对象副本
        /// </summary>
        public void BeginEdit()
        {
            //如果已经处于编辑状态，那么什么也不做。
            if (IsEditing)
            {
                return;
            }
            IsEditing = true;
            //创建对象副本。
            if (this is ICloneable)
            {
                var cloneable = this as ICloneable;
                _stub = cloneable.Clone();
            }
            else
            {
                _stub = MemberwiseClone();
            }
            var editableProp = GetEditableProperty();
            //对于每个管理的IEditableObject，递归调用BeginEdit
            foreach (var item in editableProp.Editables)
            {
                var editableObject = item.GetValue(this, null) as IEditableObject;
                if (editableObject != null)
                {
                    editableObject.BeginEdit();
                }
            }
            //对于集合属性中，如果任何项是IEditableObject，那么递归调用BeginEdit。
            foreach (var collProperty in editableProp.Collections)
            {
                var coll = collProperty.GetValue(this, null) as IList;
                if (coll != null)
                {
                    foreach (var editableObject in coll.OfType<IEditableObject>())
                    {
                        editableObject.BeginEdit();
                    }
                }
            }
        }

        /// <summary>
        ///     取消编辑，将副本恢复到当前对象，并清除副本
        /// </summary>
        public void CancelEdit()
        {
            //如果没有处于编辑状态，就什么也不做。
            if (!IsEditing)
            {
                return;
            }
            IsEditing = false;
            var editableProp = GetEditableProperty();
            //还原标量属性的值。
            foreach (var scalarProperty in editableProp.Scalars)
            {
                scalarProperty.SetValue(this, scalarProperty.GetValue(_stub, null), null);
            }
            //对于IEditableObject属性，递归调用CancelEdit
            foreach (var editableProperty in editableProp.Editables)
            {
                var editableObject = editableProperty.GetValue(this, null) as IEditableObject;
                if (editableObject != null)
                {
                    editableObject.CancelEdit();
                }
            }
            foreach (var collProperty in editableProp.Collections)
            {
                var collOld = collProperty.GetValue(_stub, null) as IList;
                var collNew = collProperty.GetValue(this, null) as IList;
                //如果两个集合不相同，那么就恢复原始集合的引用。
                if (!ReferenceEquals(collOld, collNew))
                {
                    collProperty.SetValue(this, collOld, null);
                }
                //对原始集合中每个IEditableObject，递归调用CancelEdit
                if (collOld != null)
                {
                    foreach (var editableObject in collOld.OfType<IEditableObject>())
                    {
                        editableObject.CancelEdit();
                    }
                }
            }
            //清除副本
            _stub = null;
        }

        /// <summary>
        ///     接受编辑结果，并清除副本
        /// </summary>
        public void EndEdit()
        {
            //如果没有处于编辑状态，就什么也不做。
            if (!IsEditing)
            {
                return;
            }
            IsEditing = false;
            var editableProp = GetEditableProperty();
            //对于每个IEditableObject属性，递归调用EndEdit
            foreach (var editableProperty in editableProp.Editables)
            {
                var editableObject = editableProperty.GetValue(this, null) as IEditableObject;
                if (editableObject != null)
                {
                    editableObject.EndEdit();
                }
            }
            //对于集合属性中每个项，如果其是IEditableObject，则递归调用EndEdit
            foreach (var collProperty in editableProp.Collections)
            {
                var collNew = collProperty.GetValue(this, null) as IList;
                if (collNew != null)
                {
                    foreach (var editableObject in collNew.OfType<IEditableObject>())
                    {
                        editableObject.EndEdit();
                    }
                }
            }
            //清除副本
            _stub = null;
        }

        private bool GetIsChanged(HashSet<EditableObject> markedObjects)
        {
            //如果没有在编辑状态，那么表示对象没有改变
            if (!IsEditing)
            {
                return false;
            }
            //如果对象已经被检查过了，说明出现循环引用，并且被检查过的对象没有改变。
            if (markedObjects.Contains(this))
            {
                return false;
            }
            var editableProp = GetEditableProperty();
            //检测标量属性有没有变化。
            foreach (var scalarProperty in editableProp.Scalars)
            {
                var newValue = scalarProperty.GetValue(this, null);
                var oldValue = scalarProperty.GetValue(_stub, null);
                var changed = false;
                if (newValue != null)
                {
                    changed = !newValue.Equals(oldValue);
                }
                else if (oldValue != null)
                {
                    changed = true;
                }
                if (changed)
                {
                    return true;
                }
            }
            //标记此对象已经被检查过
            markedObjects.Add(this);
            //对于每一个IEditableObject属性，进行递归检查
            foreach (var editableProperty in editableProp.Editables)
            {
                var editableObject = editableProperty.GetValue(this, null) as EditableObject;
                if (editableObject != null)
                {
                    if (editableObject.GetIsChanged(markedObjects))
                    {
                        return true;
                    }
                }
            }
            //检查集合对象的想等性
            foreach (var collectionProperty in editableProp.Collections)
            {
                IList empty = new object[0];
                var collOld = (collectionProperty.GetValue(_stub, null) as IList) ?? empty;
                var collNew = (collectionProperty.GetValue(this, null) as IList) ?? empty;
                if (!ReferenceEquals(collOld, collNew))
                {
                    //Detectif elements are added or deleted in Collection. 
                    if (!collOld.Cast<object>().SequenceEqual(collNew.Cast<object>()))
                    {
                        return true;
                    }
                }
                //Detectif any element is changed in collection.
                foreach (var item in collNew)
                {
                    var editableObject = item as EditableObject;
                    if (editableObject != null)
                    {
                        if (editableObject.GetIsChanged(markedObjects))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private EditableProperty GetEditableProperty()
        {
            return _cachedEditableProperties.GetOrAdd(GetType(), t => new EditableProperty(t));
        }
    }
}