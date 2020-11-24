
namespace System.Platform.Applications
{
    /// <summary>
    /// 模块控制器的接口，它负责模块的生命周期.
    /// </summary>
    public interface IModuleController
    {
        /// <summary>
        /// 初始化模块控制器.
        /// </summary>
        void Initialize();

        /// <summary>
        /// 运行模块控制器.
        /// </summary>
        void Run();

        /// <summary>
        /// 关闭模块控制器.
        /// </summary>
        void Shutdown();
    }
}
