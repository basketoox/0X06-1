using System;

namespace System.Toolkit.Extensions
{
    /// <summary>
    /// 表示浮点型扩展。
    /// </summary>
    public static class FloatPointExtension
    {
        /// <summary>
        /// 比较两个float值是否相等。
        /// </summary>
        /// <param name="source">源值。</param>
        /// <param name="target">目标值。</param>
        /// <returns>如果两个值相等返回true，否则返回false。</returns>
        public static bool EqualsWithThreshold(this float source, float target)
        {
            return Math.Abs(source - target) < float.Epsilon;
        }

        /// <summary>
        /// 比较两个float值是否相等。
        /// </summary>
        /// <param name="source">源值。</param>
        /// <param name="target">目标值。</param>
        /// <param name="threshold">比较阈值。</param>
        /// <returns>如果两个值相等返回true，否则返回false。</returns>
        /// <remarks>
        ///   <![CDATA[
        ///   算法为 Math.Abs(source - target) < threshold ? true : false;
        ///  ]]> 
        /// </remarks>
        public static bool EqualsWithThreshold(this float source, float target, float threshold)
        {
            return Math.Abs(source - target) < threshold ? true : false;
        }

        /// <summary>
        /// 比较两个double值是否相等。
        /// </summary>
        /// <param name="source">源值。</param>
        /// <param name="target">目标值。</param>
        /// <returns>如果两个值相等返回true，否则返回false。</returns>
        public static bool EqualsWithThreshold(this double source, double target)
        {
            return Math.Abs(source - target) < double.Epsilon ? true : false;
        }

        /// <summary>
        /// 比较两个double值是否相等。
        /// </summary>
        /// <param name="source">源值。</param>
        /// <param name="target">目标值。</param>
        /// <param name="threshold">比较阈值。</param>
        /// <returns>如果两个值相等返回true，否则返回false。</returns>
        /// <remarks>
        ///   <![CDATA[
        ///   算法为 Math.Abs(source - target) < threshold ? true : false;
        ///  ]]> 
        /// </remarks>
        public static bool EqualsWithThreshold(this double source, double target, double threshold)
        {
            return Math.Abs(source - target) < threshold ? true : false;
        }
    }
}
