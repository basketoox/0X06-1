using System.ComponentModel.Composition;
using System.Globalization;
using System.Platform.Applications;
using System.Platform.Applications.Services;
using System.Windows;

namespace System.Platform.Presentation.Services
{
    /// <summary>
    /// 这是<see cref="IMessageService"/>接口的默认实现。它通过MessageBox向用户显示消息.
    /// </summary>
    /// <remarks>
    /// 如果这个服务的默认实现不能满足您的需求，那么您可以提供自己的实现.
    /// </remarks>
    [Export(typeof(IMessageService))]
    public class MessageService : IMessageService
    {
        private static MessageBoxResult MessageBoxResult { get { return MessageBoxResult.None; } }

        private static MessageBoxOptions MessageBoxOptions
        {
            get
            {
                return (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft) ? MessageBoxOptions.RtlReading : MessageBoxOptions.None;
            }
        }

        /// <summary>
        /// 显示消息.
        /// </summary>
        /// <param name="owner">拥有此消息窗口的窗口.</param>
        /// <param name="message">The message.</param>
        public void ShowMessage(object owner, string message)
        {
            var ownerWindow = owner as Window;
            if (ownerWindow != null)
            {
                MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.None,
                    MessageBoxResult, MessageBoxOptions);
            }
            else
            {
                MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.None,
                    MessageBoxResult, MessageBoxOptions);
            }
        }

        /// <summary>
        /// 显示警告信息.
        /// </summary>
        /// <param name="owner">拥有此消息窗口的窗口.</param>
        /// <param name="message">消息.</param>
        public void ShowWarning(object owner, string message)
        {
            var ownerWindow = owner as Window;
            if (ownerWindow != null)
            {
                MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Warning,
                    MessageBoxResult, MessageBoxOptions);
            }
            else
            {
                MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Warning,
                    MessageBoxResult, MessageBoxOptions);
            }
        }

        /// <summary>
        /// 将消息显示为错误.
        /// </summary>
        /// <param name="owner">拥有此消息窗口的窗口.</param>
        /// <param name="message">消息.</param>
        public void ShowError(object owner, string message)
        {
            var ownerWindow = owner as Window;
            if (ownerWindow != null)
            {
                MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult, MessageBoxOptions);
            }
            else
            {
                MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult, MessageBoxOptions);
            }
        }

        /// <summary>
        /// 显示指定的问题.
        /// </summary>
        /// <param name="owner">拥有此消息窗口的窗口.</param>
        /// <param name="message">问题.</param>
        /// <returns><c>true</c> for yes, <c>false</c> for no and <c>null</c> for cancel.</returns>
        public bool? ShowQuestion(object owner, string message)
        {
            var ownerWindow = owner as Window;
            MessageBoxResult result;
            if (ownerWindow != null)
            {
                result = MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question, MessageBoxResult.Cancel, MessageBoxOptions);
            }
            else
            {
                result = MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question, MessageBoxResult.Cancel, MessageBoxOptions);
            }

            if (result == MessageBoxResult.Yes) { return true; }
            if (result == MessageBoxResult.No) { return false; }

            return null;
        }

        /// <summary>
        /// 显示指定的是/否问题.
        /// </summary>
        /// <param name="owner">拥有此消息窗口的窗口.</param>
        /// <param name="message">问题.</param>
        /// <returns><c>true</c> for yes and <c>false</c> for no.</returns>
        public bool ShowYesNoQuestion(object owner, string message)
        {
            var ownerWindow = owner as Window;
            MessageBoxResult result;
            if (ownerWindow != null)
            {
                result = MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.YesNo,
                    MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions);
            }
            else
            {
                result = MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.YesNo,
                    MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions);
            }

            return result == MessageBoxResult.Yes;
        }
    }
}
