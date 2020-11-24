using System;
using System.Linq;
using System.Toolkit.Attributes;

namespace System.Toolkit.Helpers
{
    public static class EnumHelper
    {
        /// <summary>
        ///     字符串转枚举。
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">字符串</param>
        /// <returns>字符串转换后的枚举实例</returns>
        public static T ToEnum<T>(this string value) where T : struct
        {
            var type = typeof (T);
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("类型{0}不是枚举类型。", type.Name));
            }
            var result = default(T);
            if (!Enum.TryParse(value, out result))
            {
                throw new ArgumentException(string.Format("{0}不能转换成枚举类型{1}。", value, type.Name));
            }
            return result;
        }

        /// <summary>
        ///     获取枚举类型的所有值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns></returns>
        public static T[] GetEnumValues<T>() where T : struct
        {
            return Enum.GetValues(typeof (T)).Cast<T>().ToArray();
        }

       /// <summary>
        ///     获取枚举类型的所有值
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        public static Enum[] GetEnumValues(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException(string.Format("类型{0}不是枚举类型", enumType));
            return Enum.GetValues(enumType).Cast<Enum>().ToArray();
        }
    }
}