using System;

namespace Motion.AdlinkAps
{
    /// <summary>
    ///     传动参数
    /// </summary>
    public struct TransmissionParams
    {
        #region 属性
        /// <summary>
        ///     导程(mm/周，度/周)
        /// </summary>
        public double Lead { get; set; }

        /// <summary>
        ///     细分数(几倍细分)
        /// </summary>
        public int SubDivisionNum { get; set; }

        /// <summary>
        ///     当量脉冲(如：p/mm，p/度)
        /// </summary>
        /// <remarks>一圈的脉冲数/导程</remarks>
        public double EquivalentPulse
        {
            get
            {
                if (Lead == 0)
                    throw new Exception("导程为0");
                return SubDivisionNum / Lead;
            }
        }
        /// <summary>
        ///     脉冲当量(如：mm/p，度/p)
        /// </summary>
        public double PulseEquivalent
        {
            get
            {
                if (SubDivisionNum == 0)
                    throw new Exception("细分数为0");
                return Lead / SubDivisionNum;
            }
        }

        #endregion
        //public override string ToString()
        //{
        //    return Lead.ToString() + "," + SubDivisionNum.ToString();
        //}
        //public static TransmissionParams Parse(string str)
        //{
        //    string[] strValue = str.Split(',');
        //    var transParams = new TransmissionParams();
        //    transParams.Lead = double.Parse(strValue[0]);
        //    transParams.SubDivisionNum = int.Parse(strValue[1]);
        //    return transParams;
        //}
    }
}