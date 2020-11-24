namespace Motion.Interfaces
{
    /// <summary>
    /// 速度曲线类型
    /// </summary>
    public enum CurveTypes : byte
    {
        /// <summary>
        /// 梯形速度曲线
        /// </summary>
        T,
        /// <summary>
        /// S型速度曲线
        /// </summary>
        S,
        /// <summary>
        /// 未指定速度曲线类型
        /// </summary>
        None = 255,
    }
}