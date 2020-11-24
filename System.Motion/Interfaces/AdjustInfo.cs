namespace Motion.Interfaces
{
    /// <summary>
    ///     检测位置调整信息
    /// </summary>
    public class AdjustInfo
    {
        /// <summary>
        ///     X 轴偏移量
        /// </summary>
        public double Xoffset { get; set; }

        /// <summary>
        ///     Y 轴偏移量
        /// </summary>
        public double Yoffset { get; set; }

        /// <summary>
        ///     R 轴偏移量
        /// </summary>
        public double Rangle { get; set; }
    }
}