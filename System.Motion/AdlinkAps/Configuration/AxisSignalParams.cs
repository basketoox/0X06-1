namespace Motion.AdlinkAps
{
    /// <summary>
    ///     轴卡有效电平配置
    /// </summary>
    public class AxisSignalParams
    {
        #region 属性

        /// <summary>
        ///     限位信号电平 0-not inverse, 1-inverse
        /// </summary>
        public int ElLogic { get; set; }

        /// <summary>
        ///     ORG信号 0-not inverse, 1-inverse
        /// </summary>
        public int OrgLogic { get; set; }

        /// <summary>
        ///     ALM信号 0-low active,1-high active
        /// </summary>
        public int AlmLogic { get; set; }

        /// <summary>
        ///     EZ信号 0-low active,1-high active
        /// </summary>
        public int EzLogic { get; set; }

        /// <summary>
        ///     INP信号 0-low active,1-high active
        /// </summary>
        public int InpLogic { get; set; }

        /// <summary>
        ///     SERVO信号 0-low active,1-high active
        /// </summary>
        public int ServoLogic { get; set; }

        /// <summary>
        ///     脉冲输出模式
        /// </summary>
        public int PlsOutMode { get; set; }

        /// <summary>
        ///     脉冲输入模式
        /// </summary>
        public int PlsInMode { get; set; }

        /// <summary>
        ///     脉冲方向 0-positive, 1-negative
        /// </summary>
        public int EncodeDir { get; set; }

        #endregion

        #region 构造器

        public AxisSignalParams()
        {
        }

        public AxisSignalParams(int elLogic, int orgLogic, int almLogic, int ezLogic, int inpLogic, int servoLogic,
            int plsOutMode, int plsInMode, int encodeDir)
        {
            ElLogic = elLogic; //限位信号电平 0-not inverse, 1-inverse
            OrgLogic = orgLogic; //ORG信号 0-not inverse, 1-inverse
            AlmLogic = almLogic; //ALM信号 0-low active,1-high active
            EzLogic = ezLogic; //EZ信号 0-low active,1-high active
            InpLogic = inpLogic; //INP信号 0-low active,1-high active
            ServoLogic = servoLogic; //SERVO信号 0-low active,1-high active
            PlsOutMode = plsOutMode; //脉冲输出模式：
            PlsInMode = plsInMode; //脉冲输入模式：
            EncodeDir = encodeDir; //脉冲方向
        }

        #endregion
    }
}