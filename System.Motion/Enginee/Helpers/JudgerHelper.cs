using System.Collections.ObjectModel;
using System.Threading;

namespace Motion.Enginee
{
    public static class JudgerHelper
    {
        private static readonly object SyncRoot = new object();

        /// <summary>
        ///     超时判读集合。
        /// </summary>
        public static ObservableCollection<Judger> TimeoutJudgers = new ObservableCollection<Judger>();

        /// <summary>
        ///     等待条件判读成立。
        /// </summary>
        /// <param name="judger">条件判读。</param>
        /// <param name="cancelToken">取消判读操作关联的令牌。</param>
        /// <param name="toContinue">要继续的 <see cref="T:System.Threading.WaitHandle" />。</param>
        /// <param name="toStop">要停止的 <see cref="T:System.Threading.WaitHandle" />。</param>
        /// <param name="timeout">等待的毫秒数，或为 <see cref="F:System.Threading.Timeout.Infinite" /> (-1)，表示无限期等待。</param>
        /// <returns></returns>
        public static bool Sure(this Judger judger, CancellationToken cancelToken, WaitHandle toContinue,
            WaitHandle toStop, int timeout = -1)
        {
            while (true)
            {
                var result = judger.Sure(cancelToken, timeout);
                if (result)
                {
                    lock (SyncRoot)
                    {
                        if (TimeoutJudgers.Contains(judger))
                        {
                            TimeoutJudgers.Remove(judger);
                        }
                    }
                    break;
                }

                lock (SyncRoot)
                {
                    if (!TimeoutJudgers.Contains(judger))
                    {
                        TimeoutJudgers.Add(judger);
                    }
                }
            }

            if (toStop != null && toContinue != null)
            {
                var waitHandles = new[] { toStop, toContinue };

                var index = WaitHandle.WaitAny(waitHandles);
                if (index == 0)
                {
                    cancelToken.ThrowIfCancellationRequested();
                }
            }
            return true;
        }
    }
}