namespace Motion.AdlinkAps
{
    /// <summary>
    /// 轴运行状态（非0的都是停止运行）
    /// </summary>
    public enum RunState
    {
        /// <summary>
        /// 正在运行
        /// </summary>
        Running = 0,
        /// <summary>
        /// 已停止运行
        /// </summary>
        Stopped = 1,

        /***********部分板卡无以下功能***********/
        /// <summary>
        /// 脉冲输入完毕
        /// </summary>
        PulseOver = 1,
        /// <summary>
        /// 命令停止（如调用了DecelStop函数）
        /// </summary>
        CommandStop = 2,
        /// <summary>
        /// 限位停止
        /// </summary>
        ELStop = 3,
        /// <summary>
        /// 遇原点停止
        /// </summary>
        ORGStop = 4,
    }
}