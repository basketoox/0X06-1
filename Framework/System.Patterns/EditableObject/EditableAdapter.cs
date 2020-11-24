using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace System.Patterns
{
    public class EditableAdapter<T> : IEditableObject, ICustomTypeDescriptor, INotifyPropertyChanged
    {
        private Memento<T> _memento;

        public EditableAdapter(T target)
        {
            Target = target;
        }

        #region IEditableObject Members

        public void BeginEdit()
        {
            if (_memento == null)
            {
                _memento = new Memento<T>(Target);
            }
        }

        public void CancelEdit()
        {
            if (_memento != null)
            {
                _memento.Restore(Target);
                _memento = null;
            }
        }

        public void EndEdit()
        {
            _memento = null;
        }

        #endregion

        #region ICustomTypeDescriptor Members

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            IList<PropertyDescriptor> propertyDescriptors = new List<PropertyDescriptor>();

            var readonlyPropertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && !p.CanWrite);

            var writablePropertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);

            foreach (var property in readonlyPropertyInfos)
            {
                var propertyCopy = property; // 需要此属性副本用于闭包

                var propertyDescriptor = PropertyDescriptorFactory.CreatePropertyDescriptor(
                    property.Name,
                    typeof(T),
                    property.PropertyType,
                    component => propertyCopy.GetValue(((EditableAdapter<T>)component).Target, null));

                propertyDescriptors.Add(propertyDescriptor);
            }

            foreach (var property in writablePropertyInfos)
            {
                var propertyCopy = property; // 需要此属性副本用于闭包

                var propertyDescriptor = PropertyDescriptorFactory.CreatePropertyDescriptor(
                    property.Name,
                    typeof(T),
                    property.PropertyType,
                    component => propertyCopy.GetValue(((EditableAdapter<T>)component).Target, null),
                    (component, value) => propertyCopy.SetValue(((EditableAdapter<T>)component).Target, value, null));

                propertyDescriptors.Add(propertyDescriptor);
            }

            return new PropertyDescriptorCollection(propertyDescriptors.ToArray());
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            throw new NotImplementedException();
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            throw new NotImplementedException();
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            throw new NotImplementedException();
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            throw new NotImplementedException();
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            throw new NotImplementedException();
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            throw new NotImplementedException();
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// 包装对象.
        /// </summary>
        public T Target { get; set; }

        private void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #region INotifyPropertyChanged Members

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                var target = Target as INotifyPropertyChanged;
                if (target != null)
                {
                    PropertyChanged += value;
                    target.PropertyChanged += NotifyPropertyChanged;
                }
            }

            remove
            {
                var target = Target as INotifyPropertyChanged;
                if (target != null)
                {
                    PropertyChanged -= value;
                    target.PropertyChanged -= NotifyPropertyChanged;
                }
            }
        }

        private event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}