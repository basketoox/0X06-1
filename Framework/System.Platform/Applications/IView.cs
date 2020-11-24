namespace System.Platform.Applications
{
    /// <summary>
    /// 代表一个视图
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// 获取或设置视图的数据上下文.
        /// </summary>
        object DataContext { get; set; }
    }
}
