using System.Threading;
using System.Threading.Tasks;

namespace System.Platform.Foundation
{
    /// <summary>
    /// 定义节流模式.
    /// </summary>
    public enum ThrottledActionMode
    {
        /// <summary>
        /// 为延迟时间最多调用一次方法.
        /// </summary>
        InvokeMaxEveryDelayTime,

        /// <summary>
        /// 只有在延迟时间内没有调用调用请求时才调用该方法.
        /// </summary>
        InvokeOnlyIfIdleForDelayTime
    }


    /// <summary>
    /// 该类支持对多个方法调用进行节流，以提高应用程序的响应能力。
    /// 它延迟方法调用，并在延迟期间跳过此方法的所有其他调用。
    /// 动作的调用是同步的。它使用在创建该类时处于活动状态的当前同步上下文.
    /// </summary>
    /// <remarks>这个类是线程安全的.</remarks>
    public class ThrottledAction
    {
        private readonly TaskScheduler taskScheduler;
        private readonly object timerLock = new object();
        private readonly Timer timer;
        private readonly Action action;
        private readonly ThrottledActionMode mode;
        private readonly TimeSpan delayTime;
        private volatile bool isRunning;


        /// <summary>
        /// 初始化<see cref="ThrottledAction"/>类的新实例.
        /// </summary>
        /// <param name="action">应该控制的操作.</param>
        /// <exception cref="ArgumentNullException">参数操作不能为空.</exception>
        public ThrottledAction(Action action)
            : this(action, ThrottledActionMode.InvokeMaxEveryDelayTime, TimeSpan.FromMilliseconds(10))
        {
        }

        /// <summary>
        /// 初始化<see cref="ThrottledAction"/>类的新实例.
        /// </summary>
        /// <param name="action">应该控制的操作.</param>
        /// <param name="mode">定义节流模式.</param>
        /// <param name="delayTime">延迟时间.</param>
        /// <exception cref="ArgumentNullException">参数操作不能为空.</exception>
        public ThrottledAction(Action action, ThrottledActionMode mode, TimeSpan delayTime)
        {
            if (action == null) { throw new ArgumentNullException("action"); }
            this.taskScheduler = SynchronizationContext.Current != null ? TaskScheduler.FromCurrentSynchronizationContext() : TaskScheduler.Default;
            this.timer = new Timer(TimerCallback);
            this.action = action;
            this.mode = mode;
            this.delayTime = delayTime;
        }


        /// <summary>
        /// 指示请求执行操作委托.
        /// </summary>
        public bool IsRunning { get { return isRunning; } }


        /// <summary>
        /// 请求动作委托的执行.
        /// </summary>
        public void InvokeAccumulated()
        {
            lock (timerLock)
            {
                if (mode == ThrottledActionMode.InvokeOnlyIfIdleForDelayTime || !isRunning)
                {
                    isRunning = true;
                    timer.Change(delayTime, Timeout.InfiniteTimeSpan);
                }
            }
        }

        /// <summary>
        /// 取消所请求的操作委托的执行.
        /// </summary>
        public void Cancel()
        {
            lock (timerLock)
            {
                isRunning = false;
                timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            }
        }

        private void TimerCallback(object state)
        {
            lock (timerLock)
            {
                isRunning = false;
            }

            Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.DenyChildAttach, taskScheduler);
        }
    }
}
