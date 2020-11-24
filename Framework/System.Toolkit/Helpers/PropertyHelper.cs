using System;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Toolkit.Helpers
{
    public class PropertyHelper
    {
        /// <summary>
        /// 获取指定对象的属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="property">指定的属性(可以多级，如A.B.C)</param>
        /// <returns></returns>
        public static object GetPropertyValue(object obj, string property)
        {
            return GetPropertyValue(obj, property, null);
        }

        /// <summary>
        /// 获取指定对象的属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="property">指定的属性(可以多级，如A.B.C)</param>
        /// <param name="nullValue">如果指定的属性为空时的返回值</param>
        /// <returns></returns>
        public static object GetPropertyValue(object obj, string property, object nullValue)
        {
            if (property.Contains("."))
            {
                return GetPropertyValue(obj.GetType().GetProperty(property.Substring(0, property.IndexOf('.'))).GetValue(obj, null),
                    property.Remove(0, property.IndexOf('.') + 1), nullValue);
            }
            else
            {
                var result = obj.GetType().GetProperty(property).GetValue(obj, null);
                if (result == null)
                    return nullValue;
                else
                    return result;
            }
        }

        /// <summary>
        /// 通过属性的lambda表达式获取属性的名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("propertyExpression");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("propertyExpression");
            }

            var getMethod = property.GetGetMethod(true);
            if (getMethod.IsStatic)
            {
                throw new ArgumentException("propertyExpression");
            }

            return memberExpression.Member.Name;
        }

        /// <summary>
        /// 对象同名属性的浅表复制
        /// </summary>
        /// <param name="from">源对象</param>
        /// <param name="to">目标对象</param>
        public static void CopyProperties(object from, object to)
        {
            if (from == null || to == null)
                return;
            foreach (var item in from.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!item.CanRead)
                    continue;
                var p = to.GetType().GetProperty(item.Name, BindingFlags.Instance | BindingFlags.Public);
                if (p == null || !p.CanWrite || !p.PropertyType.IsAssignableFrom(item.PropertyType))
                    continue;
                p.SetValue(to, item.GetValue(from, null), null);
            }
        }
    }
}
