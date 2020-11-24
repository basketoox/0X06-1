namespace Motion.AdlinkAps
{
    /// <summary>
    /// 限位停止方式
    /// </summary>
    public enum ELStopType : byte
    {
        /// <summary>
        /// 立即停止
        /// </summary>
        ImmediateStop = 0,
        /// <summary>
        /// 减速停止
        /// </summary>
        DecelStop = 1,
    }
}