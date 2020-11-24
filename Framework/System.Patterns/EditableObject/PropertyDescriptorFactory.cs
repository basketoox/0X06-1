using System.ComponentModel;

namespace System.Patterns
{
    /// <summary>
    /// 提供易于创建属性描述符的方法.
    /// </summary>
    public static class PropertyDescriptorFactory
    {
        /// <summary>
        /// 创建自定义属性描述符.
        /// </summary>
        /// <typeparam name="TComponent">组件类型.</typeparam>
        /// <typeparam name="TProperty">参数类型.</typeparam>
        /// <param name="name">属性的名称.</param>
        /// <param name="getter">获取组件并获取此属性值的函数.</param>
        /// <param name="setter">获取组件并设置此属性值的操作.</param>
        /// <returns>客户属性描述符.</returns>
        public static PropertyDescriptor CreatePropertyDescriptor<TComponent, TProperty>(string name, Func<TComponent, TProperty> getter, Action<TComponent, TProperty> setter)
        {
            return InternalPropertyDescriptorFactory.CreatePropertyDescriptor<TComponent, TProperty>(name, getter, setter);
        }

        /// <summary>
        /// 创建一个自定义只读属性描述符.
        /// </summary>
        /// <typeparam name="TComponent">组件类型.</typeparam>
        /// <typeparam name="TProperty">参数类型.</typeparam>
        /// <param name="name">只读属性的名称.</param>
        /// <param name="getter">获取组件并获取此属性值的函数.</param>
        /// <returns>客户属性描述符.</returns>
        public static PropertyDescriptor CreatePropertyDescriptor<TComponent, TProperty>(string name, Func<TComponent, TProperty> getter)
        {
            return InternalPropertyDescriptorFactory.CreatePropertyDescriptor<TComponent, TProperty>(name, getter);
        }

        /// <summary>
        /// 创建自定义属性描述符.
        /// </summary>
        /// <param name="name">属性的名称.</param>
        /// <param name="componentType">一个系统.表示此属性描述符绑定到的组件类型的类型.</param>
        /// <param name="propertyType">一个系统.表示此属性的数据类型的类型.</param>
        /// <param name="getter">获取组件并获取此属性值的函数.</param>
        /// <param name="setter">获取组件并设置此属性值的操作.</param>
        /// <returns>客户属性描述符.</returns>
        public static PropertyDescriptor CreatePropertyDescriptor(string name, Type componentType, Type propertyType, Func<object, object> getter, Action<object, object> setter)
        {
            return InternalPropertyDescriptorFactory.CreatePropertyDescriptor(name, componentType, propertyType, getter, setter);
        }

        /// <summary>
        /// 创建一个自定义只读属性描述符.
        /// </summary>
        /// <param name="name">只读属性的名称.</param>
        /// <param name="componentType">一个系统。表示此属性描述符绑定到的组件类型的类型.</param>
        /// <param name="propertyType">一个系统。表示此属性的数据类型的类型.</param>
        /// <param name="getter">获取组件并获取此属性值的函数.</param>
        /// <returns>客户属性描述符.</returns>
        public static PropertyDescriptor CreatePropertyDescriptor(string name, Type componentType, Type propertyType, Func<object, object> getter)
        {
            return InternalPropertyDescriptorFactory.CreatePropertyDescriptor(name, componentType, propertyType, getter);
        }
    }
}
