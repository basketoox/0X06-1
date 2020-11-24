using System.Platform.Foundation;
using System.Threading;
using System.Windows.Threading;

namespace System.Platform.Applications
{
    /// <summary>
    /// ViewModel实现的抽象基类.
    /// </summary>
#pragma warning disable 618
    public abstract class ViewModel : Model
#pragma warning restore 618
    {
        private readonly IView _view;

        /// <summary>
        /// 初始化<see cref="ViewModel"/>类的一个新实例，并将其自身附加为<c>DataContext</c>到视图
        /// </summary>
        /// <param name="view">视图.</param>
        protected ViewModel(IView view)
        {
            if (view == null) { throw new ArgumentNullException("view"); }
            _view = view;

            // 检查代码是否在WPF应用程序模型中运行
            if (SynchronizationContext.Current is DispatcherSynchronizationContext)
            {
                //在视图以数据绑定开始之前,
                //设置视图的DataContext必须延迟，以便视图模型可以初始化内部数据(例如命令)
                Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate
                {
                    _view.DataContext = this;
                });
            }
            else
            {
                // 当代码在WPF应用程序模型之外运行时，我们立即设置DataContext.
                view.DataContext = this;
            }
        }


        /// <summary>
        /// 获取关联视图.
        /// </summary>
        public IView View { get { return _view; } }
    }
}
