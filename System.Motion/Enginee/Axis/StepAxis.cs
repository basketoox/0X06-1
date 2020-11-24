using Motion.AdlinkAps;
namespace Motion.Enginee
{
    /// <summary>
    ///     凌华 Adlink 轴
    /// </summary>
    public class StepAxis : ApsAxis
    {
        public StepAxis(ApsController apsController) : base(apsController)
        {
        }
        public override double CurrentPos
        {
            get
            {
                return ApsController.GetCurrentCommandPosition(NoId) * Transmission.PulseEquivalent;
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
        public override void APS_set_command(double pos)
        {
            //base.APS_set_command(pos);
            ApsController.SetCommandPosition(NoId, pos);
            //ApsController.SetFeedbackPosition(NoId, pos);
        }
        /// <summary>
        ///     是否到位。
        /// </summary>
        public override bool IsInPosition(double pos)
        {
            return ApsController.IsDown(NoId) & (CurrentPos - 0.010 < pos&& CurrentPos + 0.01 > pos );
        }
    }
}