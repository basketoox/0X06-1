using System;
using System.Reflection;

namespace System.Toolkit.Helpers
{
    public class AttributeHelper
    {
        /// <summary>
        /// 查找成员的特性
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="memberInfo">目标成员</param>
        /// <returns>特性</returns>
        public static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
        {
            var attrs = memberInfo.GetCustomAttributes(typeof(T), true);
            if (attrs != null && attrs.Length > 0)
            {
                var attribute = attrs[0] as T;
                if (attribute != null)
                {
                    return attribute;
                }
            }
            return null;
        }


        /// <summary>
        /// 查找枚举值的特性
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="memberInfo">枚举值</param>
        /// <returns>特性</returns>
        public static T GetAttribute<T>(Enum @enum) where T : Attribute
        {
            return GetAttribute<T>(@enum.GetType().GetField(@enum.ToString()));
        }

    }
}
