namespace Motion.AdlinkAps
{
    /// <summary>
    ///     软限位参数
    /// </summary>
    public class SoftLimitParams
    {
        #region 属性

        /// <summary>
        ///     使能软件位
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        ///     正限位位置
        /// </summary>
        public double SPelPosition { get; set; }

        /// <summary>
        ///     负限位位置
        /// </summary>
        public double SMelPosition { get; set; }

        #endregion

        #region 构造器

        public SoftLimitParams()
        {
        }

        public SoftLimitParams(bool enable, double pPosition, double mPosition)
        {
            Enable = enable;
            SPelPosition = pPosition;
            SMelPosition = mPosition;
        }
        public override string ToString()
        {
            return (Enable ? "1" : "0") + ","+ SMelPosition.ToString() + ","+ SPelPosition.ToString();
        }
        public static SoftLimitParams Parse(string str)
        {
            string[] strValue = str.Split(',');
            var softLimitParams = new SoftLimitParams();
            softLimitParams.Enable = strValue[0] == "1" ? true : false;
            softLimitParams.SMelPosition = double.Parse(strValue[1]);
            softLimitParams.SPelPosition = double.Parse(strValue[2]);
            return softLimitParams;
        }
        #endregion
    }
}