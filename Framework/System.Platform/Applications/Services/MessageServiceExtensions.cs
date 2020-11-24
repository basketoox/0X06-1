namespace System.Platform.Applications.Services
{
    /// <summary>
    /// 为<see cref="IMessageService"/>提供方法重载，以简化其使用.
    /// </summary>
    public static class MessageServiceExtensions
    {
        /// <summary>
        /// 显示消息.
        /// </summary>
        /// <param name="service">消息服务.</param>
        /// <param name="message">消息.</param>
        /// <exception cref="ArgumentNullException">参数服务不能为空.</exception>
        public static void ShowMessage(this IMessageService service, string message)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            service.ShowMessage(null, message);
        }

        /// <summary>
        /// 显示警告信息.
        /// </summary>
        /// <param name="service">消息服务.</param>
        /// <param name="message">消息.</param>
        /// <exception cref="ArgumentNullException">参数服务不能为空.</exception>
        public static void ShowWarning(this IMessageService service, string message)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            service.ShowWarning(null, message);
        }

        /// <summary>
        /// 将消息显示为错误.
        /// </summary>
        /// <param name="service">消息服务.</param>
        /// <param name="message">消息.</param>
        /// <exception cref="ArgumentNullException">参数服务不能为空.</exception>
        public static void ShowError(this IMessageService service, string message)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            service.ShowError(null, message);
        }

        /// <summary>
        /// 显示指定的问题.
        /// </summary>
        /// <param name="service">消息服务.</param>
        /// <param name="message">消息.</param>
        /// <returns><c>true</c> for yes, <c>false</c> for no and <c>null</c> for cancel.</returns>
        /// <exception cref="ArgumentNullException">参数服务不能为空.</exception>
        public static bool? ShowQuestion(this IMessageService service, string message)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowQuestion(null, message);
        }

        /// <summary>
        /// 显示指定的是/否问题.
        /// </summary>
        /// <param name="service">消息服务.</param>
        /// <param name="message">消息</param>
        /// <returns><c>true</c> for yes and <c>false</c> for no.</returns>
        /// <exception cref="ArgumentNullException">参数服务不能为空.</exception>
        public static bool ShowYesNoQuestion(this IMessageService service, string message)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            return service.ShowYesNoQuestion(null, message);
        }
    }
}
