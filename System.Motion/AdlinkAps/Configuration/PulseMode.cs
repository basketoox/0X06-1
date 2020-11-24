namespace Motion.AdlinkAps
{
    /// <summary>
    ///     脉冲模式
    /// </summary>
    public enum PulseMode
    {
        /// <summary>
        ///     pulse/dir模式，脉冲上升沿有效
        /// </summary>
        PulseDirAsc = 0,

        /// <summary>
        ///     pulse/dir模式，脉冲下降沿有效
        /// </summary>
        PulseDirDes = 1,

        /// <summary>
        ///     pulse/dir模式，脉冲上升沿有效 低电平正向
        /// </summary>
        PulseDirAscLow = 0,

        /// <summary>
        ///     pulse/dir模式，脉冲下降沿有效 低电平正向
        /// </summary>
        PulseDirDesLow = 1,

        /// <summary>
        ///     pulse/dir模式，脉冲上升沿有效 高电平正向
        /// </summary>
        PulseDirAscHigh = 2,

        /// <summary>
        ///     pulse/dir模式，脉冲下降沿有效 高电平正向
        /// </summary>
        PulseDirDesHigh = 3,

        /// <summary>
        ///     CW/CCW模式，脉冲上升沿有效
        /// </summary>
        CWCCWAsc = 4,

        /// <summary>
        ///     CW/CCW模式，脉冲下降沿有效
        /// </summary>
        CWCCWDes = 5
    }
}