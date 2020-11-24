namespace System.Platform.Applications.Services
{
    /// <summary>
    /// 此服务向用户显示消息。当消息是问题时，它返回答案.
    /// </summary>
    /// <remarks>
    /// 这个界面是为了简单而设计的。如果您必须完成更高级的场景，
    /// 那么我们建议实现您自己的特定消息服务.
    /// </remarks>
    public interface IMessageService
    {
        /// <summary>
        /// 显示消息.
        /// </summary>
        /// <param name="owner">拥有此消息窗口的窗口.</param>
        /// <param name="message">消息.</param>
        void ShowMessage(object owner, string message);

        /// <summary>
        /// 显示警告信息.
        /// </summary>
        /// <param name="owner">拥有此消息窗口的窗口.</param>
        /// <param name="message">消息.</param>
        void ShowWarning(object owner, string message);

        /// <summary>
        /// 将消息显示为错误.
        /// </summary>
        /// <param name="owner">拥有此消息窗口的窗口.</param>
        /// <param name="message">消息.</param>
        void ShowError(object owner, string message);

        /// <summary>
        /// 显示指定的问题.
        /// </summary>
        /// <param name="owner">拥有此消息窗口的窗口.</param>
        /// <param name="message">问题.</param>
        /// <returns><c>true</c> for yes, <c>false</c> for no and <c>null</c> for cancel.</returns>
        bool? ShowQuestion(object owner, string message);

        /// <summary>
        /// 显示指定的是/否问题.
        /// </summary>
        /// <param name="owner">拥有此消息窗口的窗口.</param>
        /// <param name="message">问题.</param>
        /// <returns><c>true</c> for yes and <c>false</c> for no.</returns>
        bool ShowYesNoQuestion(object owner, string message);
    }
}
