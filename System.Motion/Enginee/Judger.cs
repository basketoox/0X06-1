using System;
using System.Threading;

namespace Motion.Enginee
{
    /// <summary>
    ///     表示一个结果判读。
    /// </summary>
    public class Judger
    {
        private readonly Func<bool> _condition;
        private int _interval = 50;

        /// <summary>
        ///     构造函数。
        /// </summary>
        /// <param name="condition">判读条件。</param>
        public Judger(Func<bool> condition)
        {
            _condition = condition;
        }

        /// <summary>
        ///     结果判读名字。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     当前判读状态值。
        /// </summary>
        public bool? Value
        {
            get
            {
                try
                {
                    return _condition();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        ///     判读间隔时间。
        /// </summary>
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        /// <summary>
        ///     等待条件判读成立。
        /// </summary>
        /// <param name="cancelToken">取消操作关联的令牌。</param>
        /// <param name="timeout">等待的毫秒数，或为 <see cref="F:System.Threading.Timeout.Infinite" /> (-1)，表示无限期等待。</param>
        public bool Sure(CancellationToken cancelToken, int timeout = -1)
        {
            return SpinWait.SpinUntil(() =>
            {
                if (cancelToken != CancellationToken.None)
                    cancelToken.ThrowIfCancellationRequested();

                var result = Value == true;
                if ((result != true) && (Interval > 0))
                    Thread.Sleep(Interval);
                return result;
            }, timeout);
        }
    }
}