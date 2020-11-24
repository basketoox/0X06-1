namespace Motion.Interfaces
{
    /// <summary>
    ///     运动轴插补控制器。
    /// </summary>
    public interface IAxisController : IAxisController<VelocityCurve>
    {
    }

    /// <summary>
    ///     运动轴插补控制器。
    /// </summary>
    /// <typeparam name="T">速度曲线参数</typeparam>
    public interface IAxisController<in T>
    {
        /// <summary>
        ///     线性差补方式运动。
        /// </summary>
        /// <typeparam name="T">速度曲线参数</typeparam>
        /// <param name="axisNo1"></param>
        /// <param name="axisNo2"></param>
        /// <param name="pulseNum1"></param>
        /// <param name="pulseNum2"></param>
        /// <param name="velocityCurve"></param>
        void MoveLine(int axisNo1, int axisNo2, int pulseNum1, int pulseNum2, T velocityCurve);
    }
}