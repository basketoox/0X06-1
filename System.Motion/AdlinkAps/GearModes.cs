namespace Motion.AdlinkAps
{
    /// <summary>
    ///     插补状态
    /// </summary>
    public enum GearModes
    {
        /// <summary>
        /// In disabling status.
        /// </summary>
        Disable = 0,
        /// <summary>
        /// In enabling status of standard mode.
        /// </summary>
        Gear = 1,
        /// <summary>
        /// In enabling status of gantry mode.
        /// </summary>
        Gantry = 2
    }
}