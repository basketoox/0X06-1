using System.ComponentModel;

namespace System.Platform.Foundation
{
    /// <summary>
    /// 使用新的验证方法扩展<see cref="IDataErrorInfo"/>接口.
    /// </summary>
    public static class DataErrorInfoExtensions
    {
        /// <summary>
        /// 验证指定的对象.
        /// </summary>
        /// <param name="instance">要验证的对象.</param>
        /// <returns>指示此对象的错误信息。默认为空字符串("").</returns>
        /// <exception cref="ArgumentNullException">参数实例不能为空.</exception>
        public static string Validate(this IDataErrorInfo instance)
        {
            if (instance == null) { throw new ArgumentNullException("instance"); }

            return instance.Error ?? "";
        }

        /// <summary>
        /// 验证对象的指定成员.
        /// </summary>
        /// <param name="instance">要验证的对象.</param>
        /// <param name="memberName">要验证的成员的名称.</param>
        /// <returns>成员的错误消息。默认为空字符串("").</returns>
        /// <exception cref="ArgumentNullException">参数实例不能为空.</exception>
        public static string Validate(this IDataErrorInfo instance, string memberName)
        {
            if (instance == null) { throw new ArgumentNullException("instance"); }

            return instance[memberName] ?? "";
        }
    }
}
