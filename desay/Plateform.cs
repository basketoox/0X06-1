using System.Collections.Generic;
using Motion.Enginee;
using Motion.Interfaces;
using System.Threading;
using System.ToolKit;
using System.Diagnostics;
using System.Framework;
namespace desay
{
    public class Plateform : ThreadPart
    {
        private PlateformAlarm m_Alarm;
        public Plateform(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe)
        {
            externalSign = ExternalSign;
            stationInitialize = stationIni;
            stationOperate = stationOpe;
        }
        public StationInitialize stationInitialize { get; set; }
        public StationOperate stationOperate { get; set; }
        public External externalSign { get; set; }
        /// <summary>
        /// X轴
        /// </summary>
        public ServoAxis Xaxis { get; set; }
        /// <summary>
        /// Y轴
        /// </summary>
        public ServoAxis Yaxis { get; set; }
        /// <summary>
        /// Z轴
        /// </summary>
        public ServoAxis Zaxis { get; set; }
        /// <summary>
        /// M轴
        /// </summary>
        public ServoAxis Maxis { get; set; }
        /// <summary>
        /// 吸笔真空吸
        /// </summary>
        public VacuoBrokenCylinder InhaleCylinder { get; set; }
        /// <summary>
        /// 读码器
        /// </summary>
        public ISerialPortTriggerModel ReadCodePort { get; set; }
        public override void Running(RunningModes runningMode)
        {
            var step = 0;
            var ScrewIsDone = 0;
            var CannotTorIn = false;
            var GetScrewPos = new Point3D<int>();
            var ScrewHolePos = new Point3D<int>();
            var ZaxisScrewpos = 0;
            var watchCameratimeout = new Stopwatch();
            //var watchScrewtimeout = new Stopwatch();
            var tempPut1 = false;
            var tempPut2 = false;
            var GetScrew1 = false;
            var GetScrew2 = false;
            var pressPulse = false;
            var startForbid = false;
            var _watch = new Stopwatch();
            _watch.Start();

            PlateformAlarm alarm=0;
            while (true)
            {
                Thread.Sleep(10);
                InhaleCylinder.Condition.External = externalSign;
                #region 判断后端设备交互信号
                if (tempPut1 && !GetScrew2)
                {
                    tempPut1 = false;
                    GetScrew1 = true;
                }
                if (tempPut2 && !GetScrew1)
                {
                    tempPut2 = false;
                    GetScrew2 = true;
                }
                if (!IoPoints.TDI7.Value && !Global.GetScrew1SignSheild) tempPut1 = false;
                if (!IoPoints.TDI8.Value && !Global.GetScrew2SignSheild) tempPut2 = false;
                if ((IoPoints.TDI7.Value || Global.GetScrew1SignSheild) && !GetScrew1 && !GetScrew2) tempPut1 = true;
                if ((IoPoints.TDI8.Value || Global.GetScrew2SignSheild) && !GetScrew1 && !GetScrew2) tempPut2 = true;
                #endregion
                #region 自动流程
                if (stationOperate.Running)
                {
                    switch (step)
                    {
                        case 0:
                            Marking.CoderResult.SN = "";
                            Marking.CoderResult.Result = "";
                            step = 10;
                            Marking.watchCT.Restart();
                            break;
                        case 10://触发读码器读码
                            ReadCodePort.Trigger(new TriggerArgs()
                            {
                                tryTimes = 1
                            });
                            step = 20;
                            break;
                        case 20://判断读码器是否读到SN
                            Thread.Sleep(300);
                            if (Marking.CoderResult.SN == "") step = 10;
                            else step = 30;
                            break;
                        case 30://双按钮启动，双按钮按下的时间间隔必须小于500ms才正常启动设备
                            if (!pressPulse && (IoPoints.TDI15.Value || IoPoints.TDI16.Value))
                            {
                                pressPulse = true;
                                startForbid = true;
                                _watch.Restart();
                            }
                            if (startForbid && IoPoints.TDI15.Value && IoPoints.TDI16.Value)
                            {
                                step = 40;
                                startForbid = false;
                                pressPulse = false;
                            }
                            else
                            {
                                if (500 <= _watch.ElapsedMilliseconds) startForbid = false;
                            }
                            if (!IoPoints.TDI15.Value && !IoPoints.TDI16.Value)
                            {
                                pressPulse = false;
                                startForbid = false;
                            }
                            break;
                        case 40://判断哪个螺丝整列机优先准备好
                            if(GetScrew1&&!GetScrew2)
                            {
                                GetScrewPos = Position.Put1ScrewPosition;
                                step = 50;
                            }
                            else if(!GetScrew1 && GetScrew2)
                            {
                                GetScrewPos = Position.Put2ScrewPosition;
                                step = 50;
                            }
                            break;
                        case 50://判断Z轴是否在0位置，X轴移动到取螺丝位置
                            if(Zaxis.IsInPosition(0))
                            {
                                Xaxis.MoveTo(GetScrewPos.X, AxisParameter.XvelocityCurve);
                                step = 60;
                            }
                            break;
                        case 60://X轴到达取螺丝位置，Z轴下降
                            if (Xaxis.IsInPosition(GetScrewPos.X))
                            {
                                Zaxis.MoveTo(GetScrewPos.Z, AxisParameter.ZvelocityCurve);
                                step = 70;
                            }
                            break;
                        case 70://Z轴到达取螺丝位置，吸真空
                            if (Zaxis.IsInPosition(GetScrewPos.Z))
                            {
                                InhaleCylinder.Set();
                                step = 80;
                            }
                            break;
                        case 80://Z轴上升0位置
                            if(InhaleCylinder.OutMoveStatus)
                            {
                                Zaxis.MoveTo(0, AxisParameter.ZvelocityCurve);
                                ScrewHolePos = Position.ScrewHolePosition[Marking.WorkIndex - 1];
                                step = 90;
                            }
                            break;
                        case 90://Z轴到达0位置，XY轴移动拧螺丝位置
                            if (Zaxis.IsInPosition(0))
                            {
                                Xaxis.MoveTo(ScrewHolePos.X, AxisParameter.XvelocityCurve);
                                Yaxis.MoveTo(ScrewHolePos.Y, AxisParameter.YvelocityCurve);
                                step = 100;
                            }
                            break;
                        case 100://XY轴到达拧螺丝位置，Z轴下降
                            if (Xaxis.IsInPosition(ScrewHolePos.X) && Yaxis.IsInPosition(ScrewHolePos.Y))
                            {
                                Zaxis.MoveTo(ScrewHolePos.Z, AxisParameter.ZvelocityCurve);
                                step = 110;
                            }
                            break;
                        case 110://Z轴到达拧螺丝位置，启动电批,并Z轴慢速下行
                            if(Zaxis.IsInPosition(ScrewHolePos.Z))
                            {
                                IoPoints.TDO11.Value = true;
                                ZaxisScrewpos = ScrewHolePos.Z + Position.ScrewDepth;
                                if(IoPoints.TDI9.Value)
                                {
                                    Zaxis.MoveTo(ZaxisScrewpos, new VelocityCurve(0,(double)Position.ZScrewSpeed,0));
                                    step = 120;
                                }
                            }
                            break;
                        case 120://Z轴到达位置，判断螺丝是否拧紧
                            if (Zaxis.IsInPosition(ZaxisScrewpos))
                            {
                                if (alarm == PlateformAlarm.收不到电批的螺丝拧紧正常或拧紧异常信号)
                                {
                                    CannotTorIn = true;
                                    alarm = PlateformAlarm.无消息;
                                    IoPoints.TDO11.Value = false;
                                    IoPoints.TDO12.Value = true;
                                    Thread.Sleep(200);
                                    IoPoints.TDO12.Value = false;
                                    step = 130;
                                }
                                if (IoPoints.TDI10.Value || IoPoints.TDI11.Value) step = 130;
                                else
                                {
                                    if (ScrewIsDone > 2)
                                    {
                                        alarm = PlateformAlarm.收不到电批的螺丝拧紧正常或拧紧异常信号;
                                        m_Alarm = PlateformAlarm.收不到电批的螺丝拧紧正常或拧紧异常信号;
                                    }
                                    Thread.Sleep(100);
                                    ScrewIsDone++;
                                }
                            }
                            break;
                        case 130://触发读取数据
                            ScrewPort.Trigger(new TriggerArgs()
                            {
                                tryTimes = 1,
                                message = "0,TR"
                            });
                            HeightGaugePort.Trigger(new TriggerArgs()
                            {
                                tryTimes = 1
                            });
                            Thread.Sleep(200);
                            step = 140;
                            break;
                        case 140://判断检测结果
                            if(CannotTorIn)
                            {
                                Marking.ResultScrew[Marking.WorkIndex].Result = "NG";
                                step = 150;
                            }
                            else
                            {
                                if (Marking.ResultScrew[Marking.WorkIndex].Result == "OK")
                                {
                                    var data = Marking.ResultScrew[Marking.WorkIndex].HeightValue - Global.BaseHeight;
                                    if (data > Position.MinHeight && data <= Position.MaxHeight) Marking.ResultScrew[Marking.WorkIndex].Result = "OK";
                                    else Marking.ResultScrew[Marking.WorkIndex].Result = "NG";
                                    step = 150;
                                }
                                if(Marking.ResultScrew[Marking.WorkIndex].Result == "NG") step = 150;
                            }
                            break;
                        case 150://电批停止，真空吸气OFF,Z轴回到0位置
                            IoPoints.TDO11.Value = false;
                            InhaleCylinder.Reset();
                            Zaxis.MoveTo(0, AxisParameter.ZvelocityCurve);
                            step = 160;
                            break;
                        case 160://真空吸气达到,Z轴到达0位置，计数加+1
                            if(InhaleCylinder.OutOriginStatus && Zaxis.IsInPosition(0))
                            {
                                Marking.WorkIndex++;
                                if (Marking.WorkIndex < Position.HoleNum) step = 40;
                                else step = 170;
                            }
                            break;
                        case 170://判断产品拧螺丝结果
                            for(var i=0;i<Position.HoleNum;i++)
                            {
                                if (Marking.ResultScrew[i].Result == "OK")
                                    Marking.ProductResult &= true;
                                else
                                    Marking.ProductResult &= false;
                            }
                            if (Marking.ProductResult) Config.ProductOkTotal++;
                            else Config.ProductNgTotal++;
                            step = 180;
                            break;
                        case 180://XY轴回到0位置
                            Xaxis.MoveTo(0, AxisParameter.XvelocityCurve);
                            Yaxis.MoveTo(0, AxisParameter.YvelocityCurve);
                            step = 190;
                            break;
                        case 190://XY轴到达0位置
                            if (Xaxis.IsInPosition(0) && Yaxis.IsInPosition(0))
                            {
                                step = 200;
                            }
                            break;
                        default:
                            Marking.ProductResult = true;
                            stationOperate.RunningSign = false;
                            Marking.WorkIndex = 0;
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
                        case 0:   //清除所有标志位的状态
                            stationInitialize.InitializeDone = false;
                            stationOperate.RunningSign = false;
                            alarm = PlateformAlarm.无消息;
                            Marking.ProductResult = true;
                            Marking.WorkIndex = 0;
                            step = 0;
                            Xaxis.Stop();
                            Yaxis.Stop();
                            Zaxis.Stop();
                            if (!Xaxis.IsAlarmed && !Yaxis.IsAlarmed)
                            {
                                Xaxis.IsServon = true;
                                Yaxis.IsServon = true;
                                Zaxis.IsServon = true;
                                stationInitialize.Flow = 10;
                            }
                            break;
                        case 10:  //复位Z轴，真空
                            IoPoints.ApsController.BackHome(Zaxis.NoId);
                            InhaleCylinder.InitExecute();
                            InhaleCylinder.Reset();
                            stationInitialize.Flow = 20;
                            break;
                        case 20:    //判断所有气缸到位，启动Z轴回原点
                            if (InhaleCylinder.OutOriginStatus)
                            {
                                if (IoPoints.ApsController.CheckHomeDone(Zaxis.NoId, 10.0) == 0)
                                {
                                    IoPoints.ApsController.BackHome(Xaxis.NoId);
                                    IoPoints.ApsController.BackHome(Yaxis.NoId);
                                    stationInitialize.Flow = 30;
                                }
                                else
                                {
                                    Zaxis.Stop();
                                    stationInitialize.InitializeDone = false; ;
                                    stationInitialize.Flow = -1;
                                }
                            }
                            break;
                        case 30://判断XY轴是否异常，为0,正常，为1：原点异常，为<0：故障
                            var resultX = IoPoints.ApsController.CheckHomeDone(Xaxis.NoId, 10.0);
                            var resultY = IoPoints.ApsController.CheckHomeDone(Yaxis.NoId, 10.0);
                            if (IoPoints.ApsController.CheckHomeDone(Xaxis.NoId, 10.0) == 0 
                                && IoPoints.ApsController.CheckHomeDone(Yaxis.NoId, 10.0) == 0)
                            {
                                stationInitialize.InitializeDone = true;
                                stationInitialize.Flow = 40;
                            }
                            else//异常处理
                            {
                                stationInitialize.InitializeDone = false; ;
                                stationInitialize.Flow = -1;
                            }
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                //故障清除
                if (externalSign.AlarmReset)
                    m_Alarm = PlateformAlarm.无消息;
            }
        }
        /// <summary>
        /// 流程报警集合
        /// </summary>
        public IList<Alarm> Alarms
        {
            get
            {
                var list = new List<Alarm>();
                list.Add(new Alarm(() => m_Alarm == PlateformAlarm.初始化故障)
                {
                    AlarmLevel = AlarmLevels.None,
                    Name = PlateformAlarm.初始化故障.ToString()
                });
                list.Add(new Alarm(() => m_Alarm == PlateformAlarm.PLC端放料1信号交互异常)
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = PlateformAlarm.PLC端放料1信号交互异常.ToString()
                });
                list.Add(new Alarm(() => m_Alarm == PlateformAlarm.PLC端放料2信号交互异常)
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = PlateformAlarm.PLC端放料2信号交互异常.ToString()
                });
                list.Add(new Alarm(() => m_Alarm == PlateformAlarm.上托盘入料异常或感应不良)
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = PlateformAlarm.上托盘入料异常或感应不良.ToString()
                });
                list.Add(new Alarm(() => m_Alarm == PlateformAlarm.下托盘入料异常或感应不良)
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = PlateformAlarm.下托盘入料异常或感应不良.ToString()
                });
                list.Add(new Alarm(() => m_Alarm == PlateformAlarm.相机定位超时)
                {
                    AlarmLevel = AlarmLevels.Error,
                    Name = PlateformAlarm.相机定位超时.ToString()
                });
                return list;
            }
        }
    }
}
