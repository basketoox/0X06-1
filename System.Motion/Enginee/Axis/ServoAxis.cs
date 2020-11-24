using Motion.AdlinkAps;
namespace Motion.Enginee
{
    /// <summary>
    ///     凌华 Adlink 伺服马达驱动轴
    /// </summary>
    public class ServoAxis : ApsAxis
    {
        public ServoAxis(ApsController apsController) : base(apsController)
        {
        }
        public override double CurrentPos
        {
            get
            {
                return ApsController.GetCurrentFeedbackPosition(NoId) * Transmission.PulseEquivalent;
            }
        }
        /// <summary>
        ///     是否原点
        /// </summary>
        public bool IsOrigin
        {
            get { return ApsController.IsOrg(NoId); }
        }

        /// <summary>
        ///     是否到达正限位。
        /// </summary>
        public override bool IsPositiveLimit
        {
            get { return ApsController.IsMel(NoId); }
        }

        /// <summary>
        ///     是否到达负限位。
        /// </summary>
        public override bool IsNegativeLimit
        {
            get { return ApsController.IsPel(NoId); }
        }
        /// <summary>
        ///     是否报警
        /// </summary>
        public override bool IsAlarmed
        {
            get { return ApsController.IsAlm(NoId); }
        }

        /// <summary>
        ///     是否急停
        /// </summary>
        public override bool IsEmg
        {
            get { return ApsController.IsEmg(NoId); }
        }
        public override void APS_set_command(double pos)
        {
            //base.APS_set_command(pos);
            ApsController.SetCommandPosition(NoId, pos);
            ApsController.SetFeedbackPosition(NoId, pos);
        }
        /// <summary>
        ///     是否到位。
        /// </summary>
        public override bool IsInPosition(double pos)
        {
            var val = ApsController.IsDown(NoId);
            return ApsController.IsDown(NoId) & (CurrentPos + 0.10 >= pos & CurrentPos - 0.10 <= pos);
        }
    }
}