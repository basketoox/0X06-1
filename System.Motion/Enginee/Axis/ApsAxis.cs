using System;
using Motion.AdlinkAps;
using Motion.Interfaces;
using System.Collections.Generic;
namespace Motion.Enginee
{
    /// <summary>
    ///     凌华Adlink轴,修改于2018.7.10 finley jiang
    /// </summary>
    public class ApsAxis : Axis, INeedClean
    {
        protected readonly ApsController ApsController;

        public ApsAxis(ApsController apsController)
        {
            ApsController = apsController;
        }

        #region Overrides of Axis

        /// <summary>
        ///     当前 Absolute 位置。
        /// </summary>
        public override double CurrentPos { get; }
        public override double CurrentSpeed
        {
            get
            {
                return Convert.ToDouble(ApsController.GetCurrentCommandSpeed(NoId)) / Transmission.EquivalentPulse;
            }
        }
        /// <summary>
        /// 轴传动参数
        /// </summary>
        public TransmissionParams Transmission { get; set; }

        #region 获取当前轴的 IO 信号
        /// <summary>
        ///     是否已励磁。
        /// </summary>
        public bool IsServon
        {
            get { return ApsController.GetServo(NoId); }
            set
            {
                if (value)
                {
                    ApsController.ServoOn(NoId);
                }
                else
                {
                    ApsController.ServoOff(NoId);
                }
            }
        }

        /// <summary>
        ///     是否到达正限位
        /// </summary>
        /// <returns></returns>
        public bool IsPEL
        {
            get { return ApsController.IsPel(NoId); }
        }
        /// <summary>
        ///     是否到达正负位
        /// </summary>
        /// <returns></returns>
        public bool IsMEL
        {
            get { return ApsController.IsMel(NoId); }
        }

        /// <summary>
        ///     是否在轴原点
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool IsOrign
        {
            get { return ApsController.IsOrg(NoId); }
        }
        /// <summary>
        /// 是否在轴Z相
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool IsSZ
        {
            get { return ApsController.IsSZ(NoId); }
        }
        /// <summary>
        /// 是否在轴Z相
        /// </summary>
        /// <param name="axisNo"></param>
        /// <returns></returns>
        public bool IsINP
        {
            get { return ApsController.IsINP(NoId); }
        }
        #endregion
        /// <summary>
        ///     是否已完成最后运动指令。
        /// </summary>
        /// <code>? + var isReach = Math.Abs(commandPosition - currentPosition) &lt; Precision;</code>
        public override bool IsDone
        {
            get { return ApsController.IsDown(NoId); }
        }

        /// <summary>
        /// 运动轴轴移动到指定的位置。
        /// </summary>
        /// <param name="value">将要移动到的位置。</param>
        /// <param name="velocityCurve">移动时的运行参数。</param>
        public override void MoveTo(double value, VelocityCurve velocityCurve = null)
        {
            var Data = value * Transmission.EquivalentPulse;
            var velocity = velocityCurve;
            velocity.Maxvel = velocityCurve.Maxvel * Transmission.EquivalentPulse;
            ApsController.MoveAbsPulse(NoId, (int)Data, velocity);
            velocity.Maxvel /= Transmission.EquivalentPulse;
        }

        /// <summary>
        ///     运动轴相对移动到指定位置。
        /// </summary>
        /// <param name="value">要移动到的距离。</param>
        /// <param name="velocityCurve"></param>
        public override void MoveDelta(double value, VelocityCurve velocityCurve = null)
        {
            var Data = value * Transmission.EquivalentPulse;
            var velocity = velocityCurve;
            velocity.Maxvel = velocityCurve.Maxvel * Transmission.EquivalentPulse;
            ApsController.MoveRelPulse(NoId, (int)Data, velocity);
            velocity.Maxvel /= Transmission.EquivalentPulse;
        }

        /// <summary>
        ///     正向移动。
        /// </summary>
        public override void Postive()
        {
            var velocityCurve = new VelocityCurve { Maxvel = (Speed ?? 0) * Transmission.EquivalentPulse };
            ApsController.ContinuousMove(NoId, MoveDirection.Postive, velocityCurve);
        }

        /// <summary>
        ///     反向移动。
        /// </summary>
        public override void Negative()
        {
            var velocityCurve = new VelocityCurve { Maxvel = (Speed ?? 0) * Transmission.EquivalentPulse };
            ApsController.ContinuousMove(NoId, MoveDirection.Negative, velocityCurve);
        }

        /// <summary>
        ///     轴停止运动。
        /// </summary>
        /// <param name="velocityCurve"></param>
        public override void Stop(VelocityCurve velocityCurve = null)
        {
            ApsController.DecelStop(NoId);
        }

        public override void Initialize()
        {
            ApsController.MoveOrigin(NoId);
        }

        #endregion

        #region Implementation of INeedInitialization

        #endregion

        #region Implementation of INeedClean

        /// <summary>
        ///      清除
        /// </summary>
        public void Clean()
        {
            Stop();
        }

        public override bool IsInPosition(double pos)
        {
            throw new NotImplementedException();
        }
        public void SetAxisHomeParam(HomeParams homeParam) => ApsController.SetAxisHomeConfig(NoId, homeParam);
        public override void BackHome() => ApsController.BackHome(NoId);

        public override int CheckHomeDone(double timeoutLimit) => ApsController.CheckHomeDone(NoId, timeoutLimit);

        public override void APS_set_command(double pos)
        {
            throw new NotImplementedException();
        }

        public override StopReasons GetStopReasons
        {
            get
            {
                return ApsController.GetStopReason(NoId);
            }
        }
        /// <summary>
        /// 轴报警集合
        /// </summary>
        public IList<Alarm> Alarms
        {
            get
            {
                var list = new List<Alarm>();
                list.Add(new Alarm(() => ApsController.IsAlm(NoId)) { AlarmLevel = AlarmLevels.Error, Name = Name + "故障报警" });
                return list;
            }
        }

        public override bool IsPositiveLimit { get; }

        public override bool IsNegativeLimit { get; }

        public override bool IsAlarmed { get; }

        public override bool IsEmg { get; }
        #endregion
    }
}