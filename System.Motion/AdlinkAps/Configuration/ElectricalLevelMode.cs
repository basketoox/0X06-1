namespace Motion.AdlinkAps
{
    /// <summary>
    ///     电平模式枚举
    /// </summary>
    /// <remarks>
    ///     一般规定低电平为0~0.25V，高电平为3.5~5V，
    ///     一般规定低电平为0~0.25V，高电平为3.5~5V。
    ///     一般低电平表示0，高电平表示1。
    /// </remarks>
    public enum ElectricalLevelMode : byte
    {
        /// <summary>
        ///     正逻辑高电平
        /// </summary>
        HighPos = ElectricalLevels.High,

        /// <summary>
        ///     正逻辑低电平
        /// </summary>
        LowPos = ElectricalLevels.Low,

        /// <summary>
        ///     负逻辑高电平
        /// </summary>
        HighNeg = ElectricalLevels.Low,

        /// <summary>
        ///     负逻辑低电平
        /// </summary>
        LowNeg = ElectricalLevels.High
    }
}