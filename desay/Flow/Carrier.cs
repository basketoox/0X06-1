using System.Collections.Generic;
using Motion.Enginee;
using Motion.Interfaces;
using System.Crosscutting.Logging;
using System.Threading;
using System.Diagnostics;
using System;
using desay.ProductData;
using System.Collections;
using System.Threading.Tasks;
using System.Drawing;
using System.Toolkit;
using System.IO;
using log4net;
using System.Device;
using System.Windows.Forms;

namespace desay.Flow
{
    /// <summary>
    /// Control flow of carrier transportation
    /// </summary>
    public class Carrier : StationPart
    {
        private PlateformAlarm m_Alarm;
        public Carrier(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe)
            : base(ExternalSign, stationIni, stationOpe, typeof(Carrier)) { }
        #region 部件
        /// <summary>
        /// 平移伸缩气缸
        /// </summary>
        public DoubleCylinder MoveCylinder { get; set; }
        /// <summary>
        /// 接驳台顶升气缸
        /// </summary>
        public SingleCylinder CarrierUpCylinder { get; set; }
        /// <summary>
        /// 接驳台开夹气缸
        /// </summary>
        public SingleCylinder CarrierClampCylinder { get; set; }
        /// <summary>
        /// 产品下压气缸
        /// </summary>
        public SingleCylinder CarrierPressCylinder { get; set; }   //20201110  XiaoW 增
        /// <summary>
        /// 回流线阻挡气缸
        /// </summary>
        public SingleCylinder CarrierStopCylinder { get; set; }
        #endregion

        public event Action<int> SendRequest;
        public event Action AlarmClean;

        private int FailCount;
        public bool CarrierHomeBit;
        bool needAlarm;

        #region abstract class ThreadPart
        public override void Running(RunningModes runningMode)
        {
            var _watch = new Stopwatch();
            _watch.Start();
            try
            {


                while (true)
                {
                    Thread.Sleep(10);
                    MoveCylinder.Condition.External = AlarmReset;
                    CarrierClampCylinder.Condition.External = AlarmReset;
                    CarrierUpCylinder.Condition.External = AlarmReset;

                    CarrierHomeBit = MoveCylinder.OutOriginStatus && CarrierUpCylinder.OutOriginStatus && CarrierClampCylinder.OutOriginStatus && CarrierPressCylinder.OutOriginStatus
                        && ((IoPoints.IDI28.Value && IoPoints.IDI29.Value && IoPoints.IDI30.Value && IoPoints.IDI31.Value) || Marking.DoorShield) && (IoPoints.IDI15.Value || Marking.CurtainShield);//门禁暂时屏蔽

                    #region 自动流程
                    if (stationOperate.Running)
                    {
                        Marking.CarrierHaveProduct = IoPoints.TDI8.Value && CarrierUpCylinder.OutOriginStatus
                            && CarrierClampCylinder.OutOriginStatus && MoveCylinder.OutOriginStatus;
                        switch (step)
                        {
                            case 0://判断接驳台是否在原点，顶升是否在原位
                                   //IoPoints.IDO0.Value = true;
                                   //IoPoints.IDO106.Value = true;
                                if (Marking.CarrierHaveProduct && !Marking.CarrierShield)
                                {
                                    IoPoints.IDO0.Value = false;
                                    IoPoints.IDO1.Value = false;
                                    Marking.SN = null;
                                    Marking.FN = null;
                                    if ((!IoPoints.IDI15.Value && !Marking.CurtainShield) || !Marking.DoorShield)
                                        break;

                                    if (CarrierHomeBit)
                                        step = 30;
                                    else
                                        step = 10;
                                }
                                break;
                            case 10://复位平移伸缩气缸
                                if (MoveCylinder.OutMoveStatus)
                                {
                                    MoveCylinder.Reset();
                                }
                                step = 11;
                                break;
                            case 11://复位接驳台开夹气缸
                                if (CarrierClampCylinder.OutMoveStatus)
                                {
                                    CarrierClampCylinder.Reset();
                                }
                                step = 12;
                                break;
                            case 12://复位接驳台顶升气缸
                                if (CarrierUpCylinder.OutMoveStatus)
                                {
                                    CarrierUpCylinder.Reset();
                                }
                                step = 13;
                                break;
                            case 13://复位产品下压气缸   20201110  XiaoW 增
                                if (CarrierPressCylinder.OutOriginStatus)
                                {
                                    CarrierPressCylinder.Reset();
                                }
                                step = 20;
                                break;
                            case 20://判断所有气缸到位
                                if (MoveCylinder.OutOriginStatus && CarrierClampCylinder.OutOriginStatus && CarrierUpCylinder.OutOriginStatus)
                                {
                                    step = 30;
                                }
                                break;
                            //case 25://回流线阻挡气缸放下
                            //    if (CarrierHomeBit && /*IoPoints.IDI20.Value*/ && !IoPoints.TDI8.Value && !IoPoints.IDO0.Value)
                            //    {
                            //        CarrierStopCylinder.Set();
                            //        Marking.CarrierCallInFinish = true;
                            //        Marking.SN = null;
                            //        Marking.FN = null;
                            //        Thread.Sleep(100);
                            //       /* IoPoints.IDO106.Value*/ = true;
                            //        IoPoints.IDO0.Value = true;
                            //        lock (MesData.AADataLock)
                            //        {
                            //            if (MesData.ResultList.ContainsKey(MesData.NeedShowFN))
                            //            {
                            //                AppendText("ShowFN");
                            //                Marking.AAResult = MesData.ResultList[MesData.NeedShowFN].aaResult;
                            //                Marking.LightCameraRst = MesData.ResultList[MesData.NeedShowFN].lightCameraRst;
                            //                Marking.PreAAPosRst = MesData.ResultList[MesData.NeedShowFN].preAAPosRst;
                            //                Marking.SearchPosRst = MesData.ResultList[MesData.NeedShowFN].searchPosRst;
                            //                Marking.OCAdjustRst = MesData.ResultList[MesData.NeedShowFN].ocAdjustRst;
                            //                Marking.TiltAdjustRst = MesData.ResultList[MesData.NeedShowFN].tiltAdjustRst;
                            //                Marking.UVBeforeRst = MesData.ResultList[MesData.NeedShowFN].uvBeforeRst;
                            //                Marking.UVAfterRst = MesData.ResultList[MesData.NeedShowFN].uvAfterRst;
                            //                Marking.HaveLensRst = MesData.ResultList[MesData.NeedShowFN].haveLensRst;
                            //                Marking.WhiteBoardRst = MesData.ResultList[MesData.NeedShowFN].whiteBoardRst;
                            //                Marking.GlueCheckRst = MesData.ResultList[MesData.NeedShowFN].glueCheckRst;
                            //                MesData.ResultList.Remove(MesData.NeedShowFN);
                            //            }
                            //        }
                            //        step = 26;
                            //    }
                            //    else if (/*IoPoints.IDI20.Value*/)
                            //    {
                            //       /* IoPoints.IDO106.Value*/ = false;
                            //    }
                            //    break;
                            //case 26://接驳台治具到位
                            //    if (IoPoints.TDI8.Value)
                            //    {
                            //        Thread.Sleep(10);
                            //        CarrierStopCylinder.Reset();
                            //        IoPoints.IDO0.Value = false;
                            //        if (!/*IoPoints.IDI20.Value*/)
                            //        {
                            //            //允许放料
                            //            Marking.AaAllowPassFlg = true;
                            //            SendRequest();
                            //        }
                            //        step = 30;
                            //    }
                            //    break;
                            case 30://顶升气缸顶起
                                if (IoPoints.TDI8.Value)
                                {
                                    Thread.Sleep(100);
                                    CarrierUpCylinder.Set();
                                    Marking.CarrierWorking = true;
                                    step = 40;
                                }
                                break;
                            case 40://开夹气缸置位
                                if (CarrierUpCylinder.OutMoveStatus && CarrierClampCylinder.OutOriginStatus)
                                {
                                    CarrierClampCylinder.Set();
                                    Thread.Sleep(100);
                                    if (!IoPoints.IDI18.Value)
                                    {
                                        Marking.AaAllowPassFlg = true;
                                        //请求AA放行治具
                                        SendRequest(5);
                                    }
                                    step = 60;
                                }
                                break;

                            case 60://判断是否有扫码功能
                                if (Marking.ScannerEnable && !Marking.SnScannerShield)
                                {
                                    AppendText("等待人工扫产品码！");
                                    step = 70;
                                }
                                else
                                    step = 100;

                                break;

                            case 70://等待人工扫产品码SN
                                Marking.BeginTriggerSN = true;
                                //SendRequest(2);//霍尼韦尔1900
                                _watch.Restart();
                                step = 80;
                                break;
                            case 80:
                                if (Marking.GetSNFlg || Marking.SnScannerShield)
                                {
                                    Marking.GetSNFlg = false;
                                    step = 100;
                                }
                                else
                                {
                                    if (_watch.ElapsedMilliseconds > Position.Instance.SocketTimeout)
                                    {
                                        m_Alarm = PlateformAlarm.扫产品码超时;
                                        step = 70;//扫产品码启动
                                    }                                
                                }                                
                                break;
                            case 100://等待启动信号
                                if (IoPoints.TDI0.Value && ((IoPoints.IDI28.Value && IoPoints.IDI29.Value && IoPoints.IDI30.Value && IoPoints.IDI31.Value) || Marking.DoorShield)
                                        && (IoPoints.IDI15.Value || Marking.CurtainShield))
                                {
                                    Marking.CarrierCallInFinish = false;                   //20201019   清除入料状态    zw
                                    if (Marking.HaveLensShield || IoPoints.IDI19.Value)    
                                    {
                                        log.Debug("启动信号");
                                        step = 102;
                                    }
                                    else
                                    {
                                        MessageBox.Show("请放好镜头及产品");
                                    }
                                }
                                break;

                            case 102://产品下压气缸动作   20201110  XiaoW 改
                                if (CarrierPressCylinder.OutOriginStatus && CarrierClampCylinder.OutMoveStatus)
                                {
                                    log.Debug("产品下压气缸置位");
                                    CarrierPressCylinder.Set();
                                    step = 103;
                                }
                                break;

                            case 103: //产品到位后，开夹气缸复位   20201110 XiaoW 改
                                if (CarrierClampCylinder.OutMoveStatus && CarrierPressCylinder.OutMoveStatus)
                                {
                                    Thread.Sleep(CarrierPressCylinder.Delay.MoveTime);
                                    log.Debug("开夹气缸到位并复位");
                                    CarrierClampCylinder.Reset();
                                    step = 104;
                                }
                                break;

                            case 104://开夹气缸复位后，产品下压气缸复位   20201110  XiaoW 改
                                if (CarrierPressCylinder.OutMoveStatus && CarrierClampCylinder.OutOriginStatus)
                                {
                                    log.Debug("产品下压气缸复位");
                                    CarrierPressCylinder.Reset();                                    
                                    step = 110;
                                }
                                break;

                            case 105://判断是否有回流到位信号,无则发送入料请求
                                //w无用代码
                                if (!IoPoints.IDI18.Value)
                                {
                                    //允许放料
                                    Marking.AaAllowPassFlg = true;
                                    SendRequest(5);                  
                                }
                                step = 110;
                                break;
                            case 110://平移气缸伸出
                                if (CarrierClampCylinder.OutOriginStatus && MoveCylinder.OutOriginStatus && CarrierPressCylinder.OutOriginStatus
                                    && (IoPoints.IDI15.Value || Marking.CurtainShield))
                                {
                                    log.Debug("平移气缸置位");
                                    MoveCylinder.Set();
                                    step = 120;
                                }
                                break;
                            case 120://判断是否有扫码功能
                                if (Marking.ScannerEnable)
                                    step = 130;
                                else
                                    step = 160;
                                break;
                            case 130://触发扫治具码FN
                                if (MoveCylinder.OutMoveStatus)
                                {
                                    Thread.Sleep(100);
                                    Marking.BeginTriggerFN = true;
                                    SendRequest(2);
                                    _watch.Restart();
                                    step = 140;
                                }
                                break;
                            case 140://获取扫描FN码结果
                                if (Marking.GetFNFlg)
                                {
                                    Marking.GetFNFlg = false;
                                    step = 160;
                                }
                                else
                                {
                                    if (_watch.ElapsedMilliseconds > Position.Instance.SocketTimeout)
                                    {                                        
                                        m_Alarm = PlateformAlarm.扫治具码超时;
                                        step = 120;//扫治具码启动
                                    }
                                }
                                break;
                            case 160://顶升气缸复位
                                if (CarrierClampCylinder.OutOriginStatus && CarrierUpCylinder.OutMoveStatus && CarrierPressCylinder.OutOriginStatus)
                                {
                                    CarrierUpCylinder.Reset();
                                    Marking.CarrierCallOut = true;
                                    step = 170;
                                }
                                break;
                            case 170://前接驳台电机反转
                                if (CarrierClampCylinder.OutOriginStatus && CarrierUpCylinder.OutOriginStatus
                                    && !IoPoints.IDI0.Value && IoPoints.IDI2.Value)//判断Plasma工位是否有料
                                {
                                    IoPoints.IDO1.Value = true;
                                    step = 180;
                                }
                                break;
                            case 180://治具抵达清洗位，接驳台收回
                                if ((IoPoints.IDI0.Value || IoPoints.IDI1.Value) && !IoPoints.IDO10.Value
                                    && (IoPoints.IDI15.Value || Marking.CurtainShield))
                                {
                                    IoPoints.IDO1.Value = false;
                                    lock (MesData.CarrierDataLock)
                                    {
                                        lock (MesData.CleanDataLock)
                                        {
                                            MesData.cleanData.carrierData.FN = MesData.carrierData.FN;
                                            MesData.cleanData.carrierData.SN = MesData.carrierData.SN;
                                            MesData.cleanData.carrierData.StartTime = MesData.carrierData.StartTime;
                                        }
                                        MesData.carrierData.SN = "";
                                        MesData.carrierData.FN = "";
                                        MesData.carrierData.StartTime = "";
                                    }
                                    MoveCylinder.Reset();
                                    Thread.Sleep(10);
                                    Marking.CarrierCallOut = false;
                                    Marking.CarrierCallOutFinish = true;
                                    step = 190;
                                }
                                break;
                            case 190://接驳台到待料位，等待入料
                                if (MoveCylinder.OutOriginStatus && Marking.CarrierCallOutFinish)
                                {
                                    Marking.CarrierCallIn = true;
                                    Marking.CarrierWorking = false;
                                    Marking.CarrierCallOutFinish = false;
                                    step = 210;
                                }
                                break;
                            case 200:
                                if (!IoPoints.IDI18.Value)
                                {
                                    //允许放料
                                    Marking.AaAllowPassFlg = true;
                                    SendRequest(5);
                                }
                                step = 210;
                                break;
                            case 210://回流线阻挡气缸放下
                                if (IoPoints.IDI18.Value && !IoPoints.TDI8.Value)//回流阻挡到位
                                {
                                    CarrierStopCylinder.Set();
                                    Marking.CarrierCallInFinish = true;
                                    Thread.Sleep(10);
                                    IoPoints.IDO8.Value = true;
                                    IoPoints.IDO0.Value = true;
                                    lock (MesData.AADataLock)
                                    {
                                        if (MesData.ResultList.ContainsKey(MesData.NeedShowFN))
                                        {
                                            AppendText("ShowFN");
                                            Marking.AAResult = MesData.ResultList[MesData.NeedShowFN].aaResult;
                                            Marking.LightCameraRst = MesData.ResultList[MesData.NeedShowFN].lightCameraRst;
                                            Marking.PreAAPosRst = MesData.ResultList[MesData.NeedShowFN].preAAPosRst;
                                            Marking.SearchPosRst = MesData.ResultList[MesData.NeedShowFN].searchPosRst;
                                            Marking.OCAdjustRst = MesData.ResultList[MesData.NeedShowFN].ocAdjustRst;
                                            Marking.TiltAdjustRst = MesData.ResultList[MesData.NeedShowFN].tiltAdjustRst;
                                            Marking.UVBeforeRst = MesData.ResultList[MesData.NeedShowFN].uvBeforeRst;
                                            Marking.UVAfterRst = MesData.ResultList[MesData.NeedShowFN].uvAfterRst;
                                            Marking.HaveLensRst = MesData.ResultList[MesData.NeedShowFN].haveLensRst;
                                            Marking.WhiteBoardRst = MesData.ResultList[MesData.NeedShowFN].whiteBoardRst;
                                            Marking.GlueCheckRst = MesData.ResultList[MesData.NeedShowFN].glueCheckRst;
                                            MesData.ResultList.Remove(MesData.NeedShowFN);
                                            if (!Marking.UVAfterRst)
                                            {
                                                needAlarm = true;
                                            }
                                        }
                                    }
                                    step = 220;
                                }
                                else if (IoPoints.IDI18.Value)//回流阻挡到位
                                {
                                    //回流电机停止
                                    IoPoints.IDO8.Value = false;
                                }
                                else//回流阻挡未到位
                                {
                                    if (_watch.ElapsedMilliseconds > 60000)
                                    {
                                        step = 200;
                                        _watch.Restart();
                                    }
                                }   
                                break;
                            case 220://接驳台治具到位
                                if (IoPoints.TDI8.Value)
                                {
                                    Thread.Sleep(10);
                                    CarrierStopCylinder.Reset();
                                    //IoPoints.IDO8.Value = false;
                                    IoPoints.IDO0.Value = false;
                                    if (needAlarm)
                                    {
                                        needAlarm = false;
                                        Marking.UVAfterAlarm = true;
                                        Thread.Sleep(Position.Instance.UVAfterAlarmTime);
                                        Marking.UVAfterAlarm = false;
                                        //AlarmClean();
                                    }
                                    step = 0;
                                }
                                break;
                            case 300://通知AA允许放料
                                     //w代码无用
                                if (Marking.CarrierCallIn)
                                {
                                    Marking.AaAllowPassFlg = true;
                                    SendRequest(5);
                                    step = 0;
                                }
                                break;
                            default:
                                stationOperate.RunningSign = false;
                                step = 0;
                                break;
                        }
                    }
                    #endregion

                    #region 初始化流程
                    if (stationInitialize.Running)
                    {
                        switch (stationInitialize.Flow)
                        {
                            case 0://清除所有标志位
                                stationInitialize.InitializeDone = false;
                                stationOperate.RunningSign = false;
                                step = 0;
                                Thread.Sleep(200);
                                Marking.CarrierHaveProduct = false;
                                Marking.CarrierCallOut = false;
                                Marking.CarrierCallOutFinish = false;
                                Marking.CarrierCallIn = false;
                                Marking.CarrierCallInFinish = false;
                                Marking.CarrierWorking = false;

                                stationInitialize.Flow = 10;
                                break;
                            case 10://复位平移伸缩气缸
                                Thread.Sleep(200);
                                if (MoveCylinder.Condition.IsOffCondition)
                                {
                                    MoveCylinder.InitExecute();
                                    MoveCylinder.Reset();
                                    stationInitialize.Flow = 11;
                                }
                                break;
                            case 11://复位接驳台开夹气缸
                                Thread.Sleep(200);
                                if (CarrierClampCylinder.Condition.IsOffCondition)
                                {
                                    CarrierClampCylinder.InitExecute();
                                    CarrierClampCylinder.Reset();
                                    stationInitialize.Flow = 12;
                                }
                                break;
                            case 12://复位接驳台顶升气缸
                                Thread.Sleep(200);
                                if (CarrierUpCylinder.Condition.IsOffCondition)
                                {
                                    CarrierUpCylinder.InitExecute();
                                    CarrierUpCylinder.Reset();
                                    stationInitialize.Flow = 13;
                                }
                                break;
                            case 13://复位回流线阻挡气缸
                                Thread.Sleep(200);
                                if (CarrierStopCylinder.Condition.IsOffCondition)
                                {
                                    CarrierStopCylinder.InitExecute();
                                    CarrierStopCylinder.Reset();
                                    stationInitialize.Flow = 14;
                                }
                                break;
                            case 14://产品下压气缸复位   20201101  XiaoW 增
                                Thread.Sleep(200);
                                if (CarrierPressCylinder.Condition.IsOffCondition)
                                {
                                    CarrierPressCylinder.InitExecute();
                                    CarrierPressCylinder.Reset();
                                    stationInitialize.Flow = 20;
                                }
                                break;
                            case 20://判断所有气缸到位,伸缩气缸收回
                                if (MoveCylinder.OutOriginStatus && CarrierClampCylinder.OutOriginStatus
                                    && CarrierUpCylinder.OutOriginStatus && CarrierPressCylinder.OutOriginStatus)   //20201101  XiaoW 改
                                {
                                    //MoveCylinder.Set();
                                    stationInitialize.Flow = 30;
                                }
                                break;
                            case 30://流水线停止转动
                                if (IoPoints.IDO0.Value) IoPoints.IDO0.Value = false;
                                if (IoPoints.IDO1.Value) IoPoints.IDO1.Value = false;
                                if (IoPoints.IDO9.Value) IoPoints.IDO9.Value = false;
                                if (IoPoints.IDO8.Value) IoPoints.IDO8.Value = false;
                                stationInitialize.Flow = 50;
                                break;
                            case 50://复位完成，置位初始化标志
                                Thread.Sleep(200);
                                stationInitialize.InitializeDone = true;
                                AppendText($"{Name}初始化完成！");
                                stationInitialize.Flow = 60;
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion
                    //clean all the enum of alarm information
                    if (AlarmReset.AlarmReset)
                    {
                        m_Alarm = PlateformAlarm.无消息;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("软件异常003,请关闭重启" + ex.ToString());

            }
        }
        /// <summary>
        /// 流程报警集合
        /// </summary>
        protected override IList<Alarm> alarms()
        {
            var list = new List<Alarm>();
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.初始化故障)
            {
                AlarmLevel = AlarmLevels.None,
                Name = PlateformAlarm.初始化故障.ToString()
            });
            //list.Add(new Alarm(() => FailCount >= Marking.CarrierFailCount)
            //{
            //    AlarmLevel = AlarmLevels.Error,
            //    Name = string.Format("进料光纤有{0}次以上产品丢失或感应不良！", Marking.CarrierFailCount)
            //});
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.等待扫码结果)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.等待扫码结果.ToString()
            });
            list.AddRange(MoveCylinder.Alarms);
            list.AddRange(CarrierUpCylinder.Alarms);
            list.AddRange(CarrierClampCylinder.Alarms);
            list.AddRange(CarrierStopCylinder.Alarms);
            list.AddRange(CarrierPressCylinder.Alarms);   //20201101  XiaoW 增 
            return list;
        }
        /// <summary>
        /// 气缸状态集合
        /// </summary>
        protected override IList<ICylinderStatusJugger> cylinderStatus()
        {
            var list = new List<ICylinderStatusJugger>();

            list.Add(MoveCylinder);
            list.Add(CarrierUpCylinder);
            list.Add(CarrierClampCylinder);
            list.Add(CarrierStopCylinder);
            list.Add(CarrierPressCylinder);   //20201110 XiaoW 增
            return list;
        }
        #endregion

        private enum PlateformAlarm : int
        {
            无消息,
            初始化故障,
            等待扫码结果,
            气缸不在状态位,
            入料请求发送失败,
            扫产品码超时,
            扫治具码超时
        }
    }
}
