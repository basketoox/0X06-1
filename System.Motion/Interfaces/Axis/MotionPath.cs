namespace Motion.Interfaces
{
    /// <summary>
    ///     表示轴的一个运动。
    /// </summary>
    /// <typeparam name="TKey">路径 Key。</typeparam>
    /// <typeparam name="TVelocityCurve">运动曲线。</typeparam>
    public class MotionPath<TKey, TVelocityCurve>
    {
        /// <summary>
        ///     路径名称。
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        ///     要到达的目的。
        /// </summary>
        public double Target { get; set; }

        /// <summary>
        ///     运动曲线。
        /// </summary>
        public TVelocityCurve VelocityCurve { get; set; }
    }
}