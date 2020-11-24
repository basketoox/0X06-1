using System.Threading;
using System.Windows.Threading;

namespace System.Platform.Applications
{
    /// <summary>
    /// 提供助手方法，用于执行涉及视图的常见任务.
    /// </summary>
    public static class ViewHelper
    {
        /// <summary>
        /// 获取与指定视图关联的视图模型.
        /// </summary>
        /// <param name="view">视图.</param>
        /// <returns>当没有找到ViewModel时关联的ViewModel，或<c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">视图不能<c>null</c>.</exception>
        public static ViewModel GetViewModel(this IView view)
        {
            if (view == null) { throw new ArgumentNullException("view"); }

            var dataContext = view.DataContext;
            //当DataContext为null时，ViewModel可能还没有设置它。
            //通过执行Dispatcher的事件队列来执行它。
            if (dataContext == null && SynchronizationContext.Current is DispatcherSynchronizationContext)
            {
                DispatcherHelper.DoEvents();
                dataContext = view.DataContext;
            }
            return dataContext as ViewModel;
        }

        /// <summary>
        /// 获取与指定视图关联的视图模型.
        /// </summary>
        /// <typeparam name="T">视图模型的类型</typeparam>
        /// <param name="view">The view.</param>
        /// <returns>当没有找到ViewModel时,关联的ViewModel，或<c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">视图不能是<c>null</c>.</exception>
        public static T GetViewModel<T>(this IView view) where T : ViewModel
        {
            return GetViewModel(view) as T;
        }
    }
}
