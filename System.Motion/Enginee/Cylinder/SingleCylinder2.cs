using System;
using System.Collections.Generic;
using System.Diagnostics;
using Motion.Interfaces;
namespace Motion.Enginee
{
    /// <summary>
    /// 双气缸共用电磁阀控制块
    /// </summary>
    public class SingleCylinder2 : Cylinder, ICylinderStatusJugger
    {
        #region "字段"
        private bool _noMoveAlarm;//无动点信号报警
        private bool _noOriginAlarm;//无原点信号报警
        private bool _offMoveAlarm;//气缸为Off时，动点信号报警
        private bool _onOriginAlarm;//气缸为On时，原点信号报警
        private bool cylinderNoOrigin;
        private bool cylinderNoMove;
        private bool cylinderOffMove;
        private bool cylinderOnOrigin;
        private bool _OutOriginStatus;
        private bool _OutMoveStatus;
        private IoPoint _InOrigin1, _InOrigin2, _InMove1, _InMove2,_OutMove;
        private bool cylinderManual;
        private bool cylinderEnable;
        private bool _autoExecute;
        private readonly Stopwatch _watchOrigin = new Stopwatch();
        private readonly Stopwatch _watchMove = new Stopwatch();
        private readonly Stopwatch[] _watchAlarm = new Stopwatch[] { new Stopwatch(), new Stopwatch(), new Stopwatch(), new Stopwatch() };
        #endregion
        public SingleCylinder2(IoPoint InOrigin1, IoPoint InOrigin2, IoPoint InMove1, IoPoint InMove2, IoPoint OutMove)
        {
            _InOrigin1 = InOrigin1;
            _InOrigin2 = InOrigin2;
            _InMove1 = InMove1;
            _InMove2 = InMove2;
            _OutMove = OutMove;
            _watchOrigin.Restart();
            _watchMove.Restart();
        }

        #region "属性"
        /// <summary>
        /// 气缸条件
        /// </summary>
        public CylinderCondition Condition { get; set; }
        /// <summary>
        /// 气缸延时
        /// </summary>
        public CylinderDelay Delay { get; set; }
        /// <summary>
        /// 气缸报警集合
        /// </summary>
        public IList<Alarm> Alarms
        {
            get
            {
                var list = new List<Alarm>();
                list.Add(new Alarm(() => NoOriginAlarm()) { AlarmLevel = AlarmLevels.Error, Name = string.Format("{0}无原点报警！", Name) });
                list.Add(new Alarm(() => NoMoveAlarm()) { AlarmLevel = AlarmLevels.Error, Name = string.Format("{0}无动点报警！", Name) });
                list.Add(new Alarm(() => OnOriginAlarm()) { AlarmLevel = AlarmLevels.Error, Name = string.Format("{0}ON时，原点信号报警！", Name) });
                list.Add(new Alarm(() => OffMoveAlarm()) { AlarmLevel = AlarmLevels.Error, Name = string.Format("{0}OFF时，动点信号报警！", Name) });
                list.Add(new Alarm(() => AutoOnPrompt()) { AlarmLevel = AlarmLevels.None, Name = string.Format("{0}自动时，手动为ON提示！", Name) });
                return list;
            }
        }

        #region 异常报警及提示
        /// <summary>
        /// 无原点信号报警
        /// </summary>
        /// <returns></returns>
        private bool NoOriginAlarm()
        {
            bool alarmDelayDone = false;
            if (Condition.External.AirSignal && !_OutMove.Value && (!_InOrigin1.Value || !_InOrigin2.Value)) cylinderNoOrigin = true;
            else cylinderNoOrigin = false;
            if (cylinderNoOrigin && !Condition.NoOriginShield)
                if (Delay.AlarmTime <= _watchAlarm[0].ElapsedMilliseconds) alarmDelayDone = true;
                else alarmDelayDone = false;
            else _watchAlarm[0].Restart();
            //无原点报警
            if ((cylinderNoOrigin && alarmDelayDone) || (_noOriginAlarm && !Condition.External.AlarmReset)) _noOriginAlarm = true;
            else _noOriginAlarm = false;
            return _noOriginAlarm;
        }
        /// <summary>
        /// 无动点信号报警
        /// </summary>
        /// <returns></returns>
        private bool NoMoveAlarm()
        {

            bool alarmDelayDone = false;
            if (Condition.External.AirSignal && _OutMove.Value && (!_InMove1.Value || !_InMove2.Value)) cylinderNoMove = true;
            else cylinderNoMove = false;

            if (cylinderNoMove && !Condition.NoMoveShield)
                if (Delay.AlarmTime <= _watchAlarm[1].ElapsedMilliseconds) alarmDelayDone = true;
                else alarmDelayDone = false;
            else _watchAlarm[1].Restart();
            //无动点报警
            if ((cylinderNoMove && alarmDelayDone) || (_noMoveAlarm && !Condition.External.AlarmReset)) _noMoveAlarm = true;
            else _noMoveAlarm = false;
            return _noMoveAlarm;
        }
        /// <summary>
        /// 气缸为Off时，动点信号报警
        /// </summary>
        /// <returns></returns>
        private bool OffMoveAlarm()
        {

            bool alarmDelayDone = false;
            if (Condition.External.AirSignal && !_OutMove.Value && _InMove1.Value && _InMove2.Value) cylinderOffMove = true;
            else cylinderOffMove = false;

            if (cylinderOffMove && !Condition.NoMoveShield)
                if (Delay.AlarmTime <= _watchAlarm[2].ElapsedMilliseconds) alarmDelayDone = true;
                else alarmDelayDone = false;
            else _watchAlarm[2].Restart();
            //气缸OFF时，动点信号报警
            if ((cylinderOffMove && alarmDelayDone) || (_offMoveAlarm && !Condition.External.AlarmReset)) _offMoveAlarm = true;
            else _offMoveAlarm = false;
            return _offMoveAlarm;

        }
        /// <summary>
        /// 气缸为On时，原点信号报警
        /// </summary>
        /// <returns></returns>
        private bool OnOriginAlarm()
        {

            bool alarmDelayDone = false;
            if (Condition.External.AirSignal && _OutMove.Value && _InOrigin1.Value && _InOrigin2.Value) cylinderOnOrigin = true;
            else cylinderOnOrigin = false;

            if (cylinderOnOrigin && !Condition.NoOriginShield)
                if (Delay.AlarmTime <= _watchAlarm[3].ElapsedMilliseconds) alarmDelayDone = true;
                else alarmDelayDone = false;
            else _watchAlarm[3].Restart();
            //气缸ON时，原点信号报警
            if ((cylinderOnOrigin && alarmDelayDone) || (_onOriginAlarm && !Condition.External.AlarmReset)) _onOriginAlarm = true;
            else _onOriginAlarm = false;
            return _onOriginAlarm;

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
                if ((_noOriginAlarm && (!cylinderNoOrigin))
                || (_noMoveAlarm && (!cylinderNoMove))
                || (_offMoveAlarm && (!cylinderOffMove))
                || (_onOriginAlarm && (!cylinderOnOrigin)))
                    return true;
                else return false;

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
        /// 输出监控
        /// </summary>
        public override bool IsOutMove
        {
            get
            {
                return _OutMove.Value;
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
                    if (((!_autoExecute) && Condition.IsOnCondition)
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
            if (((cylinderManual && !_autoExecute)
                || (!cylinderManual && _autoExecute)) && cylinderEnable)
            {
                _OutMove.Value = true;
            }
            else
            {
                _OutMove.Value = false;
            }
        }

        public void StatusJugger()
        {
            //原点状态输出
            if (!_OutMove.Value && ((_InOrigin1.Value && _InOrigin2.Value && !Condition.NoOriginShield)
            || Condition.NoOriginShield))
            {
                _watchOrigin.Stop();
                if (Delay.OriginTime <= _watchOrigin.ElapsedMilliseconds) _OutOriginStatus = true;
                else _OutOriginStatus = false;
                _watchOrigin.Start();
            }
            else
            {
                _watchOrigin.Restart();
                _OutOriginStatus = false;
            }
            //动点状态输出
            if (_OutMove.Value && ((_InMove1.Value && _InMove2.Value && !Condition.NoMoveShield)
            || Condition.NoMoveShield))
            {
                _watchMove.Stop();
                if (Delay.MoveTime <= _watchMove.ElapsedMilliseconds) _OutMoveStatus = true;
                else _OutMoveStatus = false;
                _watchMove.Start();
            }
            else
            {
                _watchMove.Restart();
                _OutMoveStatus = false;
            }
        }
        #endregion
    }
}
