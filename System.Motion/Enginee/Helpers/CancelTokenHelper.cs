using System.Threading;
using Motion.Interfaces;

namespace Motion.Enginee
{
    public static class CancelTokenHelper
    {
        /// <summary>
        ///     监控停止信号量状态并可发出取消操作通知。
        /// </summary>
        /// <param name="cancelToken">取消判读操作关联的令牌。</param>
        /// <param name="timeout">等待的毫秒数，或为 <see cref="F:System.Threading.Timeout.Infinite" /> (-1)，表示无限期等待。</param>
        /// <param name="waitHandles">一个 WaitHandle 数组，包含当前实例将等待的对象。</param>
        /// <returns>满足等待的对象的数组索引。</returns>
        /// <remarks>waitHandles[0]:要监控停止信号的 <see cref="T:System.Threading.WaitHandle" />。</remarks>
        public static int Notify(this CancellationToken cancelToken, int timeout, params WaitHandle[] waitHandles)
        {
            var index = WaitHandle.WaitAny(waitHandles, timeout);

            if (index == 0)
                cancelToken.ThrowIfCancellationRequested();

            return index;
        }
    }
}