namespace System.Platform.Applications
{
    /// <summary>
    /// ViewModel实现的抽象基类.
    /// </summary>
    /// <typeparam name="TView">视图的类型。是否提供了作为类型而不是具体类型本身的接口.</typeparam>
    public abstract class ViewModel<TView> : ViewModel where TView : IView
    {
        private readonly TView _view;


        /// <summary>
        /// 初始化<see cref="ViewModel&lt;TView&gt;"/>类，并将其自身附加为<c>DataContext</c>到视图
        /// </summary>
        /// <param name="view">The view.</param>
        protected ViewModel(TView view) : base(view)
        {
            _view = view;
        }


        /// <summary>
        /// 获取关联视图作为指定的视图类型.
        /// </summary>
        /// <remarks>
        /// 在ViewModel类中使用此属性可以避免强制转换.
        /// </remarks>
        protected TView ViewCore { get { return _view; } }
    }
}
