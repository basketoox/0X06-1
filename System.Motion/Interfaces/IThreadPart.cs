namespace Motion.Interfaces
{
    /// <summary>
    ///     线程部件
    /// </summary>
    public interface IThreadPart
    {
        /// <summary>
        ///     运行任务线程
        /// </summary>
        /// <param name="runningMode">运行模式</param>
        void Run(RunningModes runningMode);

        /// <summary>
        ///     暂停任务线程
        /// </summary>
        void Pause();

        /// <summary>
        ///     唤醒任务线程
        /// </summary>
        void Resume();

        /// <summary>
        ///     停止任务线程
        /// </summary>
        void Stop();
    }
}