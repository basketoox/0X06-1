using System;

namespace System.Toolkit.Extensions
{
    /// <summary>
    /// 表示字符串扩展。
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 判断字符串是否是null，string.Empty或者是空白字符组成。
        /// </summary>
        /// <param name="source">源字符串。</param>
        /// <param name="ignoreWhiteSpace">是否忽略空白字符。</param>
        /// <returns>如果源字符串是null、string.Empty则返回true，否则返回false。</returns>
        public static bool IsNullOrEmpty(this string source, bool ignoreWhiteSpace = true)
        {
            if (ignoreWhiteSpace)
            {
                return string.IsNullOrWhiteSpace(source);
            }
            else
            {
                return string.IsNullOrEmpty(source);
            }
        }

        /// <summary>
        /// 比较字符串忽略字符串的大小写。
        /// </summary>
        /// <param name="source">源字符串。</param>
        /// <param name="target">目标字符串。</param>
        /// <returns>如果源字符串和目标字符串忽略大小写比较相等，返回true。否则返回false。</returns>
        public static bool EqualNoCase(this string source, string target)
        {
            return source.Equals(target, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 字符串转UInt32。
        /// </summary>
        /// <param name="source">源字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>如果转换成功，返回转换后的值。否则返回默认值。</returns>
        public static UInt32 ToUInt32(this string source, UInt32 defaultValue = 0)
        {
            var value = 0u;
            if (UInt32.TryParse(source, out value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// 字符串转Int32。
        /// </summary>
        /// <param name="source">源字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>如果转换成功，返回转换后的值。否则返回默认值。</returns>
        public static Int32 ToInt32(this string source, Int32 defaultValue = 0)
        {
            var value = 0;
            if (Int32.TryParse(source, out value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// 字符串转UInt64。
        /// </summary>
        /// <param name="source">源字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>如果转换成功，返回转换后的值。否则返回默认值。</returns>
        public static UInt64 ToUInt64(this string source, UInt64 defaultValue = 0)
        {
            var value = 0ul;
            if (UInt64.TryParse(source, out value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// 字符串转Int64。
        /// </summary>
        /// <param name="source">源字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>如果转换成功，返回转换后的值。否则返回默认值。</returns>
        public static Int64 ToInt64(this string source, Int64 defaultValue = 0)
        {
            var value = 0L;
            if (Int64.TryParse(source, out value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// 字符串转Double。
        /// </summary>
        /// <param name="source">源字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>如果转换成功，返回转换后的值。否则返回默认值。</returns>
        public static Double ToDouble(this string source, Double defaultValue = 0d)
        {
            var value = 0d;
            if (Double.TryParse(source, out value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// 字符串转Single。
        /// </summary>
        /// <param name="source">源字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>如果转换成功，返回转换后的值。否则返回默认值。</returns>
        public static Single ToSingle(this string source, Single defaultValue = 0f)
        {
            var value = 0f;
            if (Single.TryParse(source, out value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// 字符串转Boolean。
        /// </summary>
        /// <param name="source">源字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>如果转换成功，返回转换后的值。否则返回默认值。</returns>
        public static Boolean ToBoolean(this string source, Boolean defaultValue = false)
        {
            var value = false;
            if (Boolean.TryParse(source, out value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// 字符串转日期。
        /// </summary>
        /// <param name="source">源字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>如果转换成功，返回转换后的值。否则返回默认值。</returns>
        public static DateTime ToDateTime(this string source, DateTime defaultValue)
        {
            var value = DateTime.Now;
            if (DateTime.TryParse(source, out value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// 字符串转小数。
        /// </summary>
        /// <param name="source">源字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>如果转换成功，返回转换后的值。否则返回默认值。</returns>
        public static Decimal ToDecimal(this string source, Decimal defaultValue)
        {
            var value = defaultValue;
            if (Decimal.TryParse(source, out value))
            {
                return value;
            }
            return defaultValue;
        }
    }
}
