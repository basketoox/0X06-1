namespace Motion.AdlinkAps
{
    /// <summary>
    ///     axis stop reason.
    /// </summary>
    public enum StopReasons
    {
        /// <summary>
        ///     Stop normally
        /// </summary>
        NORMAL = 0,

        /// <summary>
        ///     Stop when EMG is turn ON
        /// </summary>
        EMG = 1,

        /// <summary>
        ///     Stop when ALM is turn ON
        /// </summary>
        ALM = 2,

        /// <summary>
        ///     Stop when servo is turn-OFF
        /// </summary>
        SVNO = 3,

        /// <summary>
        ///     Stop by PEL signal turn ON
        /// </summary>
        PEL = 4,

        /// <summary>
        ///     Stop by MEL signal turn ON
        /// </summary>
        MEL = 5,

        /// <summary>
        ///     Stop by soft-limit condition – positive end limit
        /// </summary>
        SPEL = 6,

        /// <summary>
        ///     Stop by soft-limit condition – minus end limit
        /// </summary>
        SMEL = 7,

        /// <summary>
        ///     EMG stop by user
        /// </summary>
        USER_EMG = 8,

        /// <summary>
        ///     Stop by user
        /// </summary>
        USER = 9,

        /// <summary>
        ///     Stop by E-Gear gantry protect level 2 condition is met
        /// </summary>
        GAN_L1 = 10,

        /// <summary>
        ///     Stop by E-Gear gantry protect level 2 condition is met
        /// </summary>
        GAN_L2 = 11,

        /// <summary>
        ///     Stop because gear slave axis
        /// </summary>
        GEAR_SLAVE = 12,

        /// <summary>
        ///     Error position check level
        /// </summary>
        ERROR_LEVEL = 13,

        /// <summary>
        ///     DI
        /// </summary>
        STOP_DI = 14
    }
}