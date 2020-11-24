using System.Toolkit.Interfaces;
namespace Motion.Interfaces
{
    /// <summary>
    ///     回转体
    /// </summary>
    public interface IWheel : IAutomatic
    {
        /// <summary>
        ///     正向
        /// </summary>
        void Direct();

        /// <summary>
        ///     停止
        /// </summary>
        void Stop();

        /// <summary>
        ///     反向
        /// </summary>
        void Reverse();
    }
}