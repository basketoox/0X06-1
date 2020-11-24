using System;
using System.Collections.Generic;
using System.Diagnostics;
using Motion.Interfaces;
namespace Motion.Enginee
{
    /// <summary>
    /// 吸破真空电磁阀
    /// </summary>
    public class VacuoBrokenCylinder : Cylinder, ICylinderStatusJugger
    {
        #region "字段"

        private bool _noSensorAlarm;//无动点信号报警
        private bool _offSensorAlarm;//无原点信号报警
        private bool vacuoNoSensor;
        private bool vacuoOffSensor;
        private bool _OutOriginStatus;
        private bool _OutMoveStatus;
        private IoPoint _InSensor, _OutVacuo,_OutBroken;
        private bool cylinderManual;
        private bool cylinderEnable;
        private bool _autoExecute;
        private readonly Stopwatch _watchInhale = new Stopwatch();
        private readonly Stopwatch _watchBroken = new Stopwatch();
        private readonly Stopwatch[] _watchAlarm = new Stopwatch[] { new Stopwatch(), new Stopwatch() };

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="InSensor">感应器信号</param>
        /// <param name="OutVacuo">吸真空输出</param>
        /// <param name="OutBroken">破真空输出</param>
        public VacuoBrokenCylinder(IoPoint InSensor, IoPoint OutVacuo, IoPoint OutBroken)
        {
            _InSensor = InSensor;
            _OutVacuo = OutVacuo;
            _OutBroken = OutBroken;
            _watchInhale.Restart();
            _watchBroken.Restart();
        }

        #region "属性"

        /// <summary>
        /// 吸真空条件
        /// </summary>
        public CylinderCondition Condition { get; set; }
        /// <summary>
        /// 延时
        /// </summary>
        public VacuoDelay Delay { get; set; }
        /// <summary>
        /// 气缸报警集合
        /// </summary>
        public IList<Alarm> Alarms
        {
            get
            {
                var list = new List<Alarm>();
                list.Add(new Alarm(() => NoSensorAlarm()) { AlarmLevel = AlarmLevels.Error, Name = string.Format("{0}无感应信号报警！", Name) });
                list.Add(new Alarm(() => OffSensorAlarm()) { AlarmLevel = AlarmLevels.Error, Name = string.Format("{0}感应信号常量报警！", Name) });
                list.Add(new Alarm(() => AutoOnPrompt()) { AlarmLevel = AlarmLevels.None, Name = string.Format("{0}自动时，手动为ON提示！", Name) });
                return list;
            }
        }

        #region 异常报警及提示
        /// <summary>
        /// 无感应信号报警
        /// </summary>
        /// <returns></returns>
        private bool NoSensorAlarm()
        {
            bool alarmDelayDone = false;

            if (Condition.External.AirSignal && _OutVacuo.Value && (!_InSensor.Value)) vacuoNoSensor = true;
            else vacuoNoSensor = false;

            if (vacuoNoSensor && !Condition.NoOriginShield)
                if (Delay.AlarmTime <= _watchAlarm[0].ElapsedMilliseconds) alarmDelayDone = true;
                else alarmDelayDone = false;
            else _watchAlarm[0].Restart();
            //无感应信号报警
            if ((vacuoNoSensor && alarmDelayDone) || (_noSensorAlarm && (!Condition.External.AlarmReset))) _noSensorAlarm = true;
            else _noSensorAlarm = false;

            return _noSensorAlarm;
        }
        /// <summary>
        /// 感应信号常量报警
        /// </summary>
        /// <returns></returns>
        private bool OffSensorAlarm()
        {

            bool alarmDelayDone = false;
            if (Condition.External.AirSignal && !_OutVacuo.Value && _InSensor.Value) vacuoOffSensor = true;
            else vacuoOffSensor = false;
            if (vacuoOffSensor && !Condition.NoOriginShield)
                if (Delay.AlarmTime <= _watchAlarm[1].ElapsedMilliseconds)
                    alarmDelayDone = true;
                else alarmDelayDone = false;
            else _watchAlarm[1].Restart();
            //感应信号常量报警
            if ((vacuoOffSensor && alarmDelayDone) || (_offSensorAlarm && !Condition.External.AlarmReset)) _offSensorAlarm = true;
            else _offSensorAlarm = false;
            return _offSensorAlarm;
        }
        /// <summary>
        /// 自动时，手动模式为ON提示
        /// </summary>
        private bool AutoOnPrompt()
        {
            //自动操作时，手动为ON提示
            if (!Condition.External.ManualAutoMode && Condition.External.InitializingDone && cylinderManual)
                return true;
            else return false;

        }
        #endregion

        #region "状态"
        /// <summary>
        /// 报警可复位
        /// </summary>
        public override bool HaveAlarmReset
        {
            get
            {
                //报警取消时，报警可复位提示
                if ((_noSensorAlarm && (!vacuoNoSensor))
                || (_offSensorAlarm && (!vacuoOffSensor)))
                    return true;
                else return false;
            }
        }
        /// <summary>
        /// 原点信号状态
        /// </summary>
        public override bool OutOriginStatus
        {
            get
            {
                return _OutOriginStatus;
            }
        }
        /// <summary>
        /// 动点信号状态
        /// </summary>
        public override bool OutMoveStatus
        {
            get
            {
                return _OutMoveStatus;
            }
        }
        /// <summary>
        /// 输出监控
        /// </summary>
        public override bool IsOutMove
        {
            get
            {
                return _OutVacuo.Value;
            }
        }
        #endregion

        #endregion

        #region 方法

        /// <summary>
        /// 模块初始化操作
        /// </summary>
        public override void InitExecute()
        {
            cylinderManual = false;
            _autoExecute = false;
            cylinderEnable = true;
            CylinderActive();
        }

        /// <summary>
        /// 手动操作
        /// </summary>
        public override void ManualExecute()
        {
            if (!Condition.External.ManualAutoMode)
            {
                if (!cylinderManual)
                {
                    if ((!_autoExecute && Condition.IsOnCondition)
                        || (_autoExecute && Condition.IsOffCondition))
                        cylinderManual = true;
                }
                else
                {
                    if ((!_autoExecute && Condition.IsOffCondition)
                        || (_autoExecute && Condition.IsOnCondition))
                        cylinderManual = false;
                }
            }
            cylinderEnable = true;
            CylinderActive();
        }

        /// <summary>
        /// 自动置位操作
        /// </summary>
        public override void Set()
        {
            if (cylinderManual) return;
            _autoExecute = true;
            CylinderActive();
        }

        /// <summary>
        /// 自动复位操作
        /// </summary>
        public override void Reset()
        {
            if (cylinderManual) return;
            _autoExecute = false;
            CylinderActive();
        }

        private void CylinderActive()
        {
            if (((cylinderManual && (!_autoExecute))
                || ((!cylinderManual) && _autoExecute)) && cylinderEnable)
            {
                _OutVacuo.Value = true;
            }
            else
            {
                _OutVacuo.Value = false;
                _OutBroken.Value = true;
            }
        }

        public void StatusJugger()
        {
            //原点状态输出
            if ((!_OutVacuo.Value) && ((!_InSensor.Value && !Condition.NoOriginShield)
            || Condition.NoOriginShield))
            {
                _watchBroken.Stop();
                if (Delay.BrokenTime <= _watchBroken.ElapsedMilliseconds)
                {
                    _OutBroken.Value = false;
                    _OutOriginStatus = true;
                }
                else _OutOriginStatus = false;
                _watchBroken.Start();
            }
            else
            {
                _watchBroken.Restart();
                _OutOriginStatus = false;
            }

            //动点状态输出
            if (_OutVacuo.Value && ((_InSensor.Value && !Condition.NoOriginShield)
            || Condition.NoOriginShield))
            {
                _watchInhale.Stop();
                if (Delay.InhaleTime <= _watchInhale.ElapsedMilliseconds) _OutMoveStatus = true;
                else _OutMoveStatus = false;
                _watchInhale.Start();
            }
            else
            {
                _watchInhale.Restart();
                _OutMoveStatus = false;
            }
        }

        #endregion
    }
}