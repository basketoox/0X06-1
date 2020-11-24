using System.ComponentModel;

namespace System.Patterns
{
    /// <summary>
    /// 提供用于创建属性描述符的内部方法。这个类不应该直接使用.
    /// </summary>
    internal class InternalPropertyDescriptorFactory : TypeConverter
    {
        public static PropertyDescriptor CreatePropertyDescriptor<TComponent, TProperty>(string name,
            Func<TComponent, TProperty> getter, Action<TComponent, TProperty> setter)
        {
            return new GenericPropertyDescriptor<TComponent, TProperty>(name, getter, setter);
        }

        public static PropertyDescriptor CreatePropertyDescriptor<TComponent, TProperty>(string name,
            Func<TComponent, TProperty> getter)
        {
            return new GenericPropertyDescriptor<TComponent, TProperty>(name, getter);
        }

        public static PropertyDescriptor CreatePropertyDescriptor(string name, Type componentType, Type propertyType,
            Func<object, object> getter, Action<object, object> setter)
        {
            return new GenericPropertyDescriptor(name, componentType, propertyType, getter, setter);
        }

        public static PropertyDescriptor CreatePropertyDescriptor(string name, Type componentType, Type propertyType,
            Func<object, object> getter)
        {
            return new GenericPropertyDescriptor(name, componentType, propertyType, getter);
        }

        protected class GenericPropertyDescriptor<TComponent, TProperty> : SimplePropertyDescriptor
        {
            private readonly Func<TComponent, TProperty> getter;
            private readonly Action<TComponent, TProperty> setter;

            public GenericPropertyDescriptor(string name, Func<TComponent, TProperty> getter,
                Action<TComponent, TProperty> setter)
                : base(typeof(TComponent), name, typeof(TProperty))
            {
                if (getter == null)
                {
                    throw new ArgumentNullException("getter");
                }
                if (setter == null)
                {
                    throw new ArgumentNullException("setter");
                }

                this.getter = getter;
                this.setter = setter;
            }

            public GenericPropertyDescriptor(string name, Func<TComponent, TProperty> getter)
                : base(typeof(TComponent), name, typeof(TProperty))
            {
                if (getter == null)
                {
                    throw new ArgumentNullException("getter");
                }

                this.getter = getter;
            }

            public override bool IsReadOnly
            {
                get { return setter == null; }
            }

            public override object GetValue(object target)
            {
                var component = (TComponent)target;
                var value = getter(component);
                return value;
            }

            public override void SetValue(object target, object value)
            {
                if (!IsReadOnly)
                {
                    var component = (TComponent)target;
                    var newValue = (TProperty)value;
                    setter(component, newValue);
                }
            }
        }

        protected class GenericPropertyDescriptor : SimplePropertyDescriptor
        {
            private readonly Func<object, object> getter;
            private readonly Action<object, object> setter;

            public GenericPropertyDescriptor(string name, Type componentType, Type propertyType,
                Func<object, object> getter, Action<object, object> setter)
                : base(componentType, name, propertyType)
            {
                if (getter == null)
                {
                    throw new ArgumentNullException("getter");
                }
                if (setter == null)
                {
                    throw new ArgumentNullException("setter");
                }

                this.getter = getter;
                this.setter = setter;
            }

            public GenericPropertyDescriptor(string name, Type componentType, Type propertyType,
                Func<object, object> getter)
                : base(componentType, name, propertyType)
            {
                if (getter == null)
                {
                    throw new ArgumentNullException("getter");
                }

                this.getter = getter;
            }

            public override bool IsReadOnly
            {
                get { return setter == null; }
            }

            public override object GetValue(object target)
            {
                var value = getter(target);
                return value;
            }

            public override void SetValue(object target, object value)
            {
                if (!IsReadOnly)
                {
                    var newValue = value;
                    setter(target, newValue);
                }
            }
        }
    }
}