namespace Motion.Interfaces
{
    /// <summary>
    ///     速度曲线参数
    /// </summary>
    public class VelocityCurve
    {
        #region 方法

        /// <summary>
        ///     复制个完整的副本
        /// </summary>
        /// <returns></returns>
        public VelocityCurve Clone()
        {
            return new VelocityCurve(Strvel, Maxvel, Tacc, Tdec, Svacc, Svdec, VelocityCurveType);
        }

        #endregion

        #region 属性

        /// <summary>
        ///     开始速度pps
        /// </summary>
        /// <remarks>Starting velocity  (mm/s)</remarks>
        public double Strvel { get; set; }

        /// <summary>
        ///     最大速度pps（即稳定运行的速度）
        /// </summary>
        /// <remarks>max velocity (mm/s)</remarks>
        public double Maxvel { get; set; }

        /// <summary>
        ///     加速时间s
        /// </summary>
        /// <remarks>acceleration time  (m/s2)</remarks>
        public double Tacc { get; set; }

        /// <summary>
        ///     减速时间s
        /// </summary>
        /// <remarks>deceleration time (m/s2)</remarks>
        public double Tdec { get; set; }

        /// <summary>
        ///     加速度圆角（S型运动曲线专用，其他的类型时为零）
        /// </summary>
        /// <remarks>S-curve acc disition (mm)</remarks>
        public double Svacc { get; set; }

        /// <summary>
        ///     减速度圆角（S型运动曲线专用，其他的类型时为零）
        /// </summary>
        /// <remarks>S-curve dec disition (mm)</remarks>
        public double Svdec { get; set; }

        /// <summary>
        ///     S曲线因子
        /// </summary>
        /// <remarks>S-Factor(0表示T形曲线)</remarks>
        public double Sfac { get; set; }

        /// <summary>
        ///     速度曲线类型
        /// </summary>
        public CurveTypes VelocityCurveType { get; set; }

        #endregion

        #region 构造器

        /// <summary>
        ///     构造器
        /// </summary>
        public VelocityCurve()
        {
        }

        /// <summary>
        ///     等腰梯形的梯形速度曲线
        /// </summary>
        /// <param name="strvel">起始速度，pps</param>
        /// <param name="maxvel">正常速度，pps</param>
        /// <param name="tacc">加速时间，单位：s</param>
        public VelocityCurve(double strvel, double maxvel, double tacc)
            : this(strvel, maxvel, tacc, tacc, 0, 0, CurveTypes.T)
        {
        }

        /// <summary>
        ///     加减速不对称的梯形速度曲线
        /// </summary>
        /// <param name="strvel"></param>
        /// <param name="maxvel"></param>
        /// <param name="tacc"></param>
        /// <param name="tdec">减速时间</param>
        public VelocityCurve(double strvel, double maxvel, double tacc, double tdec)
            : this(strvel, maxvel, tacc, tdec, 0, 0, CurveTypes.T)
        {
        }

        /// <summary>
        ///     加减速不对称的S形速度曲线
        /// </summary>
        /// <param name="strvel">pps</param>
        /// <param name="maxvel"></param>
        /// <param name="tacc"></param>
        /// <param name="tdec"></param>
        /// <param name="svacc">加速圆角半径</param>
        /// <param name="svdec">减速圆角半径</param>
        public VelocityCurve(double strvel, double maxvel, double tacc, double tdec, double svacc, double svdec)
            : this(strvel, maxvel, tacc, tdec, svacc, svdec, CurveTypes.S)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="strvel">开始速度pps</param>
        /// <param name="maxvel">最大速度pps（即稳定运行的速度）</param>
        /// <param name="tacc"> 加速时间s</param>
        /// <param name="tdec">减速时间s</param>
        /// <param name="svacc">加速度圆角（S型运动曲线专用，其他的类型时为零）</param>
        /// <param name="svdec">减速度圆角（S型运动曲线专用，其他的类型时为零）</param>
        /// <param name="velocityCurveType">速度曲线类型</param>
        public VelocityCurve(double strvel, double maxvel, double tacc, double tdec, double svacc, double svdec,
            CurveTypes velocityCurveType)
        {
            Strvel = strvel;
            Maxvel = maxvel;
            Tacc = tacc;
            Tdec = tdec;
            Svacc = svacc;
            Svdec = svdec;

            VelocityCurveType = velocityCurveType;
        }
        #endregion
        public override string ToString()
        {
            return Strvel.ToString() + ","
                + Maxvel.ToString() + ","
                + Tacc.ToString() + ","
                + Tdec.ToString() + ","
                + Svacc.ToString() + ","
                + Svdec.ToString() + ","
                + Sfac.ToString() + ","
                + (VelocityCurveType == CurveTypes.T ? "0" : VelocityCurveType==CurveTypes.S ? "1":"255");

        }
        public static VelocityCurve Parse(string str)
        {
            string[] strValue = str.Split(',');
            var velocityCurve = new VelocityCurve();
            velocityCurve.Strvel = double.Parse(strValue[0]);
            velocityCurve.Maxvel = double.Parse(strValue[1]);
            velocityCurve.Tacc = double.Parse(strValue[2]);
            velocityCurve.Tdec = double.Parse(strValue[3]);
            velocityCurve.Svacc = double.Parse(strValue[4]);
            velocityCurve.Svdec = double.Parse(strValue[5]);
            velocityCurve.Sfac = double.Parse(strValue[6]);
            velocityCurve.VelocityCurveType = (CurveTypes)(byte.Parse(strValue[7]));
            return velocityCurve;
        }
    }
}