using System.Collections.Generic;
using Motion.Enginee;
using Motion.Interfaces;
using System.Threading;
using System.Diagnostics;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.IO;
using System.Toolkit;
using desay.ProductData;
using Motion.AdlinkAps;
using System.Windows.Forms;
using System.Text;
using System.Toolkit.Helpers;
namespace desay.Flow
{
    /// <summary>
    /// 清洗平台控制流程
    /// </summary>
    public class CleanPlateform : StationPart
    {
        /// <summary>
        /// 报警信息枚举变量
        /// </summary>        
        public CleanPlateform(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe)
                        : base(ExternalSign, stationIni, stationOpe, typeof(CleanPlateform))
        { }
        #region 部件
        /// <summary>
        /// X-轴
        /// </summary>
        public ServoAxis Xaxis { get; set; }
        /// <summary>
        /// Y-轴
        /// </summary>
        public ServoAxis Yaxis { get; set; }
        /// <summary>
        /// Z-轴
        /// </summary>
        public ServoAxis Zaxis { get; set; }
        /// <summary>
        /// 清洗阻挡气缸
        /// </summary>
        public SingleCylinder CleanStopCylinder { get; set; }
        /// <summary>
        /// 清洗顶升气缸
        /// </summary>
        public SingleCylinder CleanUpCylinder { get; set; }
        /// <summary>
        /// 清洗旋转气缸
        /// </summary>
        public SingleCylinder CleanRotateCylinder { get; set; }
        /// <summary>
        /// 清洗夹紧气缸
        /// </summary>
        public SingleCylinder CleanClampCylinder { get; set; }
        /// <summary>
        /// 清洗上下气缸
        /// </summary>
        public SingleCylinder CleanUpDownCylinder { get; set; }
        /// <summary>
        /// 光源上下气缸
        /// </summary>
        public SingleCylinder LightUpDownCylinder { get; set; }

        //public SingleCylinder LightingTestCylinder { get; set; }

        #endregion

        private PlateformAlarm m_Alarm;

        public event Action<int> SendRequest;

        /// <summary>
        /// Cone清洗次数
        /// </summary>
        public int CleanNum;
        /// <summary>
        /// Lens清洗次数
        /// </summary>
        public int CleanNum2;
        public bool CleanHomeBit;
        /// <summary>
        /// 单个产品点亮次数
        /// </summary>
        int wbCheckCount = 0;
        //指定的buf大小必须大于传入的字符长度
        StringBuilder buf = new StringBuilder(3072);

        public override void Running(RunningModes runningMode)
        {
            Stopwatch _repositoryWatch = new Stopwatch();
            _repositoryWatch.Start();
            var _watch = new Stopwatch();
            _watch.Start();
            try
            {
                while (true)
                {
                    Thread.Sleep(10);
                    CleanStopCylinder.Condition.External = AlarmReset;
                    CleanUpCylinder.Condition.External = AlarmReset;
                    CleanClampCylinder.Condition.External = AlarmReset;
                    CleanRotateCylinder.Condition.External = AlarmReset;
                    CleanUpDownCylinder.Condition.External = AlarmReset;
                    LightUpDownCylinder.Condition.External = AlarmReset;
                    CleanHomeBit = IoPoints.IDI2.Value && IoPoints.IDI6.Value && IoPoints.IDI8.Value && IoPoints.IDI4.Value
                                && IoPoints.IDI10.Value && Zaxis.IsInPosition(Position.Instance.CleanSafePosition.Z);

                    #region 自动运行控制流程
                    if (stationOperate.Running)
                    {
                        Marking.CleanHaveProduct = (IoPoints.IDI0.Value || Marking.CleanHaveProductSheild) && CleanUpCylinder.OutOriginStatus;
                        switch (step)
                        {
                            case 0: //判断平台是否在原点，顶升是否在原位
                                Marking.HaveLens = false;
                                Marking.WhiteBoardResult = false;
                                Marking.WhiteLightResult = false;
                                Marking.OnceCleanFinish = false;
                                Marking.TwiceCleanFinish = false;
                                Marking.CleanWorking = true;
                                Marking.CleanWorkFinish = false;
                                Marking.WbCheckAgainFlg = false;
                                IoPoints.IDO16.Value = false;
                                if (Marking.CleanHaveProduct && !Marking.CleanFinishBit)
                                {
                                    Marking.CleanCallIn = false;
                                    if (CleanHomeBit)
                                    { step = 50; }
                                    else 
                                    { step = 10; }
                                }
                                else
                                {
                                    if (Marking.CleanHaveProduct && Marking.CleanFinishBit && Marking.CarrierCallOut)
                                    {
                                        Config.Instance.CleanProductOkTotal++;
                                        Marking.CleanResult = true;
                                        Marking.HaveLens = true;
                                        Marking.WhiteBoardResult = true;
                                        Marking.WhiteLightResult = true;
                                        Marking.CleanCallIn = false;
                                        Marking.CleanCallOut = true;
                                        step = 410;//直接跳过清洗
                                    }
                                    else
                                    {
                                        Marking.CleanCallIn = true;
                                        Marking.CleanFinishBit = false;
                                    }
                                }
                                break;

                            case 10:  //复位所有气缸的动作                          
                                CleanStopCylinder.Reset();
                                CleanClampCylinder.Reset();
                                LightUpDownCylinder.Reset();
                                Thread.Sleep(100);
                                if (CleanClampCylinder.OutOriginStatus)
                                {
                                    CleanUpDownCylinder.Reset();
                                    Thread.Sleep(100);
                                }
                                if (CleanUpDownCylinder.OutOriginStatus)
                                {
                                    CleanRotateCylinder.Reset();
                                    Thread.Sleep(100);
                                    CleanUpCylinder.Reset();
                                    Thread.Sleep(100);
                                    step = 20;
                                }
                                break;
                            case 20:    //判断所有气缸到位，启动Z轴回安全位
                                if (Zaxis.IsServon && CleanUpCylinder.OutOriginStatus && CleanUpDownCylinder.OutOriginStatus)
                                {
                                    Zaxis.MoveTo(Position.Instance.CleanSafePosition.Z, AxisParameter.Instance.LZspeed);
                                    step = 30;
                                }
                                break;
                            case 30://判断Z轴是否在安全位置
                                if (Zaxis.IsInPosition(Position.Instance.CleanSafePosition.Z))
                                {
                                    //Xaxis.MoveTo(Position.Instance.CleanSafePosition.X, AxisParameter.Instance.LXspeed);
                                    //Yaxis.MoveTo(Position.Instance.CleanSafePosition.Y, AxisParameter.Instance.LYspeed);
                                    MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.CleanSafePosition, AxisParameter.Instance.LXspeed);

                                    step = 50;
                                }
                                break;


                            case 50: //顶升气缸顶起
                                if (CleanHomeBit)
                                {
                                    //移动到位，稳定再顶升
                                    Thread.Sleep(500);
                                    Marking.CleanStart = true;
                                    Marking.CleanFinish = false;
                                    _repositoryWatch.Restart();
                                    Marking.CleanCycleTime = 0;
                                    CleanUpCylinder.Set();
                                    step = 60;
                                }
                                else
                                    step = 0;
                                break;

                            case 60: //判断有无料检测是否启用  默认被屏蔽了 
                                if (Marking.HaveLensShield)
                                {   //step = 70;
                                    log.Debug("镜头感应屏蔽");
                                    Marking.HaveLens = true;
                                    step = 105;
                                }
                                else
                                {

                                    step = 105;
                                }
                                break;
                            //case 70://XY模组移至有无料检测位
                            //    Xaxis.MoveTo(Position.Instance.LensDetectPosition.X, AxisParameter.Instance.LXspeed);
                            //    Yaxis.MoveTo(Position.Instance.LensDetectPosition.Y, AxisParameter.Instance.LYspeed);
                            //    step = 80;
                            //    break;
                            //case 80://Z模组移至有无料检测位
                            //    if (Yaxis.IsInPosition(Position.Instance.LensDetectPosition.Y)
                            //        && Xaxis.IsInPosition(Position.Instance.LensDetectPosition.X))
                            //    {
                            //        Zaxis.MoveTo(Position.Instance.LensDetectPosition.Z, AxisParameter.Instance.LZspeed);
                            //        step = 100;

                            //    }
                            //    break;
                            //case 100: //判断是否有镜头
                            //    if (Zaxis.IsInPosition(Position.Instance.LensDetectPosition.Z) && CleanUpCylinder.OutMoveStatus
                            //        && IoPoints.TDI10.Value)
                            //    {
                            //        Thread.Sleep(100);
                            //        if (IoPoints.IDI3.Value)
                            //        {
                            //            Marking.HaveLens = true;
                            //            step = 105;//判断是否启用Plasma
                            //        }
                            //        else
                            //        {
                            //            Config.Instance.CleanProductNgTotal++;
                            //            Marking.CleanResult = false;
                            //            Marking.HaveLens = false;
                            //            Marking.CleanHoming = true;
                            //            Marking.CleanFinishBit = true;
                            //            step = 410;//清洗结束
                            //        }
                            //    }
                            //    break;


                            case 105://判断Plasma是否启用
                                if (Marking.CleanRun || Marking.CleanShield)
                                {
                                    Thread.Sleep(500);
                                    step = 310;//判断是否进行白板检测
                                }

                                else
                                    step = 106;//执行Plasma流程
                                break;

                            case 106://Z轴移至清洗安全位
                                Zaxis.MoveTo(Position.Instance.CleanSafePosition.Z, AxisParameter.Instance.LZspeed);
                                step = 110;//执行Plasma流程
                                break;

                            case 110://XY轴前往镜筒清洗轨迹起点
                                if (Zaxis.IsInPosition(Position.Instance.CleanSafePosition.Z))
                                {
                                    Thread.Sleep(500);
                                    //Xaxis.MoveTo(Position.Instance.CleanConeFirstPosition.X, AxisParameter.Instance.LXspeed);
                                    //Yaxis.MoveTo(Position.Instance.CleanConeFirstPosition.Y, AxisParameter.Instance.LYspeed);
                                    MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.CleanConeFirstPosition, AxisParameter.Instance.LXspeed);

                                    CleanNum = 0;
                                    step = 120;
                                }
                                break;

                            case 120://Z轴移动到镜筒清洗位
                                if (Xaxis.IsInPosition(Position.Instance.CleanConeFirstPosition.X) && Yaxis.IsInPosition(Position.Instance.CleanConeFirstPosition.Y))
                                {
                                    Zaxis.MoveTo(Position.Instance.CleanConeFirstPosition.Z, AxisParameter.Instance.LZspeed);
                                    step = 130;
                                }
                                break;
                            case 130://判断圆形/矩形 Cone清洗
                                if (Zaxis.IsInPosition(Position.Instance.CleanConeFirstPosition.Z))
                                {
                                    if (Position.Instance.UseRectGlue)
                                    {
                                        #region 矩形清洗
                                        int rectstep = 0;
                                        bool stepstatus = true;
                                        while (stepstatus)
                                        {
                                            switch (rectstep)
                                            {
                                                case 0:
                                                    if (Marking.PlasmaShield)
                                                    { IoPoints.IDO16.Value = false; }
                                                    else
                                                    { IoPoints.IDO16.Value = true; }

                                                    //第二个点
                                                    MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.CleanConeSecondPosition, AxisParameter.Instance.CleanPathSpeed);
                                                    rectstep = 10;
                                                    break;
                                                case 10:
                                                    if (Xaxis.IsInPosition(Position.Instance.CleanConeSecondPosition.X) && Yaxis.IsInPosition(Position.Instance.CleanConeSecondPosition.Y))
                                                    {
                                                        //第三个点
                                                        MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.CleanConeThirdPositon, AxisParameter.Instance.CleanPathSpeed);
                                                        rectstep = 20;
                                                    }
                                                    break;
                                                case 20:
                                                    if (Xaxis.IsInPosition(Position.Instance.CleanConeThirdPositon.X) && Yaxis.IsInPosition(Position.Instance.CleanConeThirdPositon.Y))
                                                    {
                                                        //第四个点
                                                        MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.CleanConeForthPosition, AxisParameter.Instance.CleanPathSpeed);
                                                        rectstep = 30;
                                                    }
                                                    break;
                                                case 30:
                                                    if (Xaxis.IsInPosition(Position.Instance.CleanConeForthPosition.X) && Yaxis.IsInPosition(Position.Instance.CleanConeForthPosition.Y))
                                                    {
                                                        //第一个点
                                                        MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.CleanConeFirstPosition, AxisParameter.Instance.CleanPathSpeed);
                                                        rectstep = 40;
                                                    }
                                                    break;
                                                case 40:
                                                    if (Xaxis.IsInPosition(Position.Instance.CleanConeFirstPosition.X) && Yaxis.IsInPosition(Position.Instance.CleanConeFirstPosition.Y))
                                                    {
                                                        stepstatus = false;
                                                        rectstep = 0;
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                        //InitBufferMode(3, (int)Position.Instance.CleanPathSpeed);
                                        //DoRect(3, Position.Instance.CleanConeFirstPosition.X, Position.Instance.CleanConeFirstPosition.Y, Position.Instance.CleanConeFirstPosition.Z, (int)Position.Instance.CleanPathSpeed, (int)Position.Instance.CleanPathSpeed);
                                        //DoRect(3, Position.Instance.CleanConeSecondPosition.X, Position.Instance.CleanConeSecondPosition.Y, Position.Instance.CleanConeSecondPosition.Z, (int)Position.Instance.CleanPathSpeed, (int)Position.Instance.CleanPathSpeed);
                                        //DoRect(3, Position.Instance.CleanConeThirdPositon.X, Position.Instance.CleanConeThirdPositon.Y, Position.Instance.CleanConeThirdPositon.Z, (int)Position.Instance.CleanPathSpeed, (int)Position.Instance.CleanPathSpeed);
                                        //DoRect(3, Position.Instance.CleanConeForthPosition.X, Position.Instance.CleanConeForthPosition.Y, Position.Instance.CleanConeForthPosition.Z, (int)Position.Instance.CleanPathSpeed, (int)Position.Instance.CleanPathSpeed);
                                        //DoRect(3, Position.Instance.CleanConeFirstPosition.X, Position.Instance.CleanConeFirstPosition.Y, Position.Instance.CleanConeFirstPosition.Z, (int)Position.Instance.CleanPathSpeed, (int)Position.Instance.CleanPathSpeed);
                                        //if (Marking.CleanRun)
                                        //    IoPoints.IDO16.Value = false;
                                        //else
                                        //    IoPoints.IDO16.Value = true;
                                        ////清洗启动
                                        //APSptStart();

                                        #endregion
                                    }
                                    else
                                    {
                                        #region 圆形清洗
                                        Int32 Dimension = 2;
                                        Int32[] Axis_ID_Array_For_2Axes_ArcMove = new Int32[2] { Xaxis.NoId, Yaxis.NoId };
                                        Int32[] Center_Pos_Array = new Int32[2] { Convert.ToInt32(Position.Instance.CleanConeCenterPositionReal.X / AxisParameter.Instance.LXTransParams.PulseEquivalent),
                                        Convert.ToInt32(Position.Instance.CleanConeCenterPositionReal.Y/ AxisParameter.Instance.LYTransParams.PulseEquivalent) };
                                        //Int32 Max_Arc_Speed = 20000;
                                        Int32 Angle = 360;
                                        if (Marking.PlasmaShield)
                                            IoPoints.IDO16.Value = false;
                                        else
                                            IoPoints.IDO16.Value = true;
                                        APS168.APS_absolute_arc_move(Dimension, Axis_ID_Array_For_2Axes_ArcMove, Center_Pos_Array, (int)Position.Instance.CleanPathSpeed * 1000, Angle);
                                        #endregion
                                    }
                                    step = 140;
                                    Thread.Sleep(10);
                                }
                                break;
                            case 140://清洗次数判断
                                if (Xaxis.IsInPosition(Position.Instance.CleanConeFirstPosition.X) && Yaxis.IsInPosition(Position.Instance.CleanConeFirstPosition.Y))   //&& Xaxis.IsAcrStop && Yaxis.IsAcrStop
                                {
                                    CleanNum++;
                                    // if (CleanNum < 3) step = 151;
                                    if (CleanNum < Position.Instance.ConeCleanNum)
                                        step = 130;
                                    else
                                    {
                                        Marking.OnceCleanFinish = true;
                                        IoPoints.IDO16.Value = false;
                                        step = 150;
                                    }
                                    Thread.Sleep(10);
                                }
                                break;
                            case 150://Z轴移到安全位置  
                                CleanNum = 0;
                                Zaxis.MoveTo(Position.Instance.CleanSafePosition.Z, AxisParameter.Instance.LZspeed);
                                step = 160;
                                break;
                            case 160://清洗上下气缸下降
                                if (Zaxis.IsInPosition(Position.Instance.CleanSafePosition.Z) && CleanUpDownCylinder.OutOriginStatus)
                                {
                                    //Xaxis.MoveTo(Position.Instance.CleanLensFirstPosition.X, AxisParameter.Instance.LXspeed);
                                    //Yaxis.MoveTo(Position.Instance.CleanLensFirstPosition.Y, AxisParameter.Instance.LYspeed);
                                    MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.CleanLensFirstPosition, AxisParameter.Instance.LXspeed);

                                    Thread.Sleep(200);
                                    CleanUpDownCylinder.Set();
                                    step = 170;
                                }
                                break;
                            case 170://清洗夹紧气缸夹紧
                                if (CleanUpDownCylinder.OutMoveStatus && CleanClampCylinder.OutOriginStatus)
                                {
                                    Thread.Sleep(100);
                                    CleanClampCylinder.Set();
                                    Thread.Sleep(100);
                                    step = 180;
                                }
                                break;
                            case 180://清洗上下气缸上升
                                if (CleanClampCylinder.OutMoveStatus && CleanClampCylinder.OutMoveStatus)
                                {
                                    Thread.Sleep(100);
                                    CleanUpDownCylinder.Reset();
                                    step = 190;
                                }
                                break;
                            case 190://清洗翻转气缸翻转180度
                                if (CleanRotateCylinder.OutOriginStatus && CleanUpDownCylinder.OutOriginStatus)
                                {
                                    Thread.Sleep(100);
                                    CleanRotateCylinder.Set();//翻转
                                    if (Position.Instance.UseRectGlue)
                                    {
                                        step = 200;//20.5M 矩形 清洗时气缸不下降
                                    }
                                    else
                                    {
                                        step = 191;//Freetech 圆心 清洗时气缸需下降
                                    }
                                }
                                break;
                            case 191://清洗上下气缸下降                                     
                                if (CleanUpDownCylinder.OutOriginStatus && CleanRotateCylinder.OutMoveStatus)
                                {
                                    if (Position.Instance.CleanUpDownCylinder == 1)
                                    {
                                        CleanUpDownCylinder.Set();//下降
                                        Thread.Sleep(200);
                                        step = 192;
                                    }
                                    else
                                    {
                                        step = 200;
                                    }
                                }
                                break;
                            case 192://清洗上下气缸到位
                                if (CleanUpDownCylinder.OutMoveStatus)
                                {
                                    Zaxis.MoveTo(Position.Instance.CleanSafePosition.Z, AxisParameter.Instance.LZspeed);
                                    step = 210;
                                }
                                break;
                            case 200://Z 2次前往清洗安全位置
                                if (CleanRotateCylinder.OutMoveStatus)
                                {
                                    Zaxis.MoveTo(Position.Instance.CleanSafePosition.Z, AxisParameter.Instance.LZspeed);
                                    step = 210;
                                }
                                break;
                            case 210://XY 2次前往清洗圆形轨迹起点
                                if (Zaxis.IsInPosition(Position.Instance.CleanSafePosition.Z) && Yaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.Y) && Xaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.X))
                                {
                                    CleanNum2 = 0;
                                    step = 220;
                                }
                                break;
                            case 220://Z 2次前往清洗圆形轨迹起点
                                if (Xaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.X) && Yaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.Y))
                                {
                                    Zaxis.MoveTo(Position.Instance.CleanLensFirstPosition.Z, AxisParameter.Instance.LZspeed);
                                    step = 240;
                                }
                                break;
                            case 240://判断圆形/矩形 Lens清洗
                                if (Zaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.Z))
                                {
                                    if (Position.Instance.UseRectGlue)
                                    {
                                        #region 矩形清洗
                                        int rectstep = 0;
                                        bool stepstatus = true;
                                        while (stepstatus)
                                        {
                                            switch (rectstep)
                                            {
                                                case 0:
                                                    if (Marking.PlasmaShield)
                                                    { IoPoints.IDO16.Value = false; }
                                                    else
                                                    { IoPoints.IDO16.Value = true; }

                                                    //第二个点
                                                    MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.CleanLensSecondPosition, AxisParameter.Instance.CleanPathSpeed);
                                                    rectstep = 10;
                                                    break;
                                                case 10:
                                                    if (Xaxis.IsInPosition(Position.Instance.CleanLensSecondPosition.X) && Yaxis.IsInPosition(Position.Instance.CleanLensSecondPosition.Y))
                                                    {
                                                        //第三个点
                                                        MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.CleanLensThirdPositon, AxisParameter.Instance.CleanPathSpeed);
                                                        rectstep = 20;
                                                    }
                                                    break;
                                                case 20:
                                                    if (Xaxis.IsInPosition(Position.Instance.CleanLensThirdPositon.X) && Yaxis.IsInPosition(Position.Instance.CleanLensThirdPositon.Y))
                                                    {
                                                        //第四个点
                                                        MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.CleanLensForthPosition, AxisParameter.Instance.CleanPathSpeed);
                                                        rectstep = 30;
                                                    }
                                                    break;
                                                case 30:
                                                    if (Xaxis.IsInPosition(Position.Instance.CleanLensForthPosition.X) && Yaxis.IsInPosition(Position.Instance.CleanLensForthPosition.Y))
                                                    {
                                                        //第一个点
                                                        MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.CleanLensFirstPosition, AxisParameter.Instance.CleanPathSpeed);
                                                        rectstep = 40;
                                                    }
                                                    break;
                                                case 40:
                                                    if (Xaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.X) && Yaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.Y))
                                                    {
                                                        stepstatus = false;
                                                        rectstep = 0;
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                        //InitBufferMode(3, (int)Position.Instance.CleanPathSpeed);
                                        //DoRect(3, Position.Instance.CleanLensFirstPosition.X, Position.Instance.CleanLensFirstPosition.Y, Position.Instance.CleanLensFirstPosition.Z, (int)Position.Instance.CleanPathSpeed, (int)Position.Instance.CleanPathSpeed);
                                        //DoRect(3, Position.Instance.CleanLensSecondPosition.X, Position.Instance.CleanLensSecondPosition.Y, Position.Instance.CleanLensSecondPosition.Z, (int)Position.Instance.CleanPathSpeed, (int)Position.Instance.CleanPathSpeed);
                                        //DoRect(3, Position.Instance.CleanLensThirdPositon.X, Position.Instance.CleanLensThirdPositon.Y, Position.Instance.CleanLensThirdPositon.Z, (int)Position.Instance.CleanPathSpeed, (int)Position.Instance.CleanPathSpeed);
                                        //DoRect(3, Position.Instance.CleanLensForthPosition.X, Position.Instance.CleanLensForthPosition.Y, Position.Instance.CleanLensForthPosition.Z, (int)Position.Instance.CleanPathSpeed, (int)Position.Instance.CleanPathSpeed);
                                        //DoRect(3, Position.Instance.CleanLensFirstPosition.X, Position.Instance.CleanLensFirstPosition.Y, Position.Instance.CleanLensFirstPosition.Z, (int)Position.Instance.CleanPathSpeed, (int)Position.Instance.CleanPathSpeed);
                                        //if (Marking.CleanRun)
                                        //    IoPoints.IDO16.Value = false;
                                        //else
                                        //    IoPoints.IDO16.Value = true;
                                        ////清洗启动
                                        //APSptStart();

                                        #endregion
                                    }
                                    else
                                    {
                                        #region 圆形清洗
                                        Int32 Dimension2 = 2;
                                        Int32[] Axis_ID_Array_For_2Axes_ArcMove2 = new Int32[2] { Xaxis.NoId, Yaxis.NoId };
                                        Int32[] Center_Pos_Array2 = new Int32[2] { Convert.ToInt32(Position.Instance.CleanLensCenterPositionReal.X/ AxisParameter.Instance.LXTransParams.PulseEquivalent),
                                        Convert.ToInt32(Position.Instance.CleanLensCenterPositionReal.Y/ AxisParameter.Instance.LYTransParams.PulseEquivalent) };
                                        Int32 Max_Arc_Speed2 = 20000;
                                        Int32 Angle2 = 360;
                                        if (Marking.PlasmaShield)
                                            IoPoints.IDO16.Value = false;
                                        else
                                            IoPoints.IDO16.Value = true;
                                        APS168.APS_absolute_arc_move(Dimension2, Axis_ID_Array_For_2Axes_ArcMove2, Center_Pos_Array2, (int)Position.Instance.CleanPathSpeed * 1000, Angle2);
                                        #endregion
                                    }
                                    step = 250;
                                    Thread.Sleep(10);
                                }
                                break;
                            case 250://清洗次数判断   
                                if (Xaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.X) && Yaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.Y))   //&& Xaxis.IsAcrStop && Yaxis.IsAcrStop
                                {
                                    CleanNum2++;
                                    if (CleanNum2 < Position.Instance.LensCleanNum)
                                        step = 240;
                                    else
                                    {
                                        Marking.TwiceCleanFinish = true;
                                        IoPoints.IDO16.Value = false;
                                        step = 260;
                                    }
                                    Thread.Sleep(10);
                                }
                                break;
                            case 260://Z轴回清洗安全位置  
                                Zaxis.MoveTo(Position.Instance.CleanSafePosition.Z, AxisParameter.Instance.LZspeed);
                                CleanNum2 = 0;
                                step = 261;
                                break;

                            case 261://清洗上下气缸复位
                                if (Zaxis.IsInPosition(Position.Instance.CleanSafePosition.Z))
                                {
                                    if (!Marking.WhiteShield)
                                    {
                                        log.Debug("XY模组移至白板测试位置");
                                        //Xaxis.MoveTo(Position.Instance.AdjustLightPosition.X, AxisParameter.Instance.LXspeed);
                                        //Yaxis.MoveTo(Position.Instance.AdjustLightPosition.Y, AxisParameter.Instance.LYspeed);
                                        MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.AdjustLightPosition, AxisParameter.Instance.LXspeed);
                                    }
                                    if (CleanUpDownCylinder.OutMoveStatus)
                                    {
                                        CleanUpDownCylinder.Reset();
                                    }
                                    step = 270;
                                }
                                break;
                            case 270://清洗翻转气缸复位
                                if (CleanUpDownCylinder.OutOriginStatus)
                                {
                                    Thread.Sleep(100);
                                    CleanRotateCylinder.Reset();
                                    step = 280;
                                }
                                break;
                            case 280://清洗上下气缸下降
                                if (CleanRotateCylinder.OutOriginStatus && CleanUpDownCylinder.OutOriginStatus)
                                {
                                    Thread.Sleep(100);
                                    CleanUpDownCylinder.Set();
                                    step = 290;
                                }
                                break;
                            case 290://清洗夹紧气缸松开
                                if (CleanUpDownCylinder.OutMoveStatus /*&& CleanClampCylinder.OutMoveStatus*/)
                                {
                                    Thread.Sleep(100);
                                    CleanClampCylinder.Reset();
                                    step = 300;
                                }
                                break;
                            case 300://清洗上下气缸上升
                                if (CleanClampCylinder.OutOriginStatus /*&& CleanUpDownCylinder.OutMoveStatus*/)
                                {
                                    Thread.Sleep(100);
                                    CleanUpDownCylinder.Reset();
                                    Thread.Sleep(300);
                                    step = 310;
                                }
                                break;
                            case 310: //判断白板检测是否屏蔽
                                if (!Marking.WhiteShield && !Marking.CleanShield)
                                {
                                    step = 320;//移至白板检测位
                                }
                                else
                                {
                                    Config.Instance.CleanProductOkTotal++;
                                    Marking.CleanResult = true;
                                    Marking.CleanHoming = true;
                                    Marking.CleanFinishBit = true;
                                    Marking.WhiteBoardResult = true;
                                    Marking.WhiteLightResult = true;
                                    step = 410;//清洗结束
                                }
                                break;
                            case 320://XY轴移至白板测试位置
                                if (Xaxis.IsInPosition(Position.Instance.AdjustLightPosition.X) && Yaxis.IsInPosition(Position.Instance.AdjustLightPosition.Y))
                                { }
                                else
                                {
                                    //Xaxis.MoveTo(Position.Instance.AdjustLightPosition.X, AxisParameter.Instance.LXspeed);
                                    //Yaxis.MoveTo(Position.Instance.AdjustLightPosition.Y, AxisParameter.Instance.LYspeed);
                                    MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.AdjustLightPosition, AxisParameter.Instance.LXspeed);
                                }
                                step = 330;
                                break;
                            case 330://Z轴移至白板测试位置
                                if (Xaxis.IsInPosition(Position.Instance.AdjustLightPosition.X) && Yaxis.IsInPosition(Position.Instance.AdjustLightPosition.Y))
                                {
                                    log.Debug("XY模组移至白板测试位置");
                                    Zaxis.MoveTo(Position.Instance.AdjustLightPosition.Z, AxisParameter.Instance.LZspeed);
                                    step = 340;
                                }
                                break;
                            case 340://光源上下气缸下降
                                if (Zaxis.IsInPosition(Position.Instance.AdjustLightPosition.Z) && LightUpDownCylinder.OutOriginStatus)
                                {
                                    if (Position.Instance.WbLightCylinder == 1)
                                    {
                                        LightUpDownCylinder.Set();
                                    }
                                    Thread.Sleep(500);
                                    wbCheckCount = 0;//点亮次数归零
                                    step = 350;//白板测试启动
                                }
                                break;
                            case 350://白板测试启动
                                if (Zaxis.IsInPosition(Position.Instance.AdjustLightPosition.Z) && CleanUpCylinder.OutMoveStatus)
                                {
                                    AppendText("白板测试启动");
                                    try
                                    {
                                        if (!Marking.DryRun)
                                        {
                                            //调用DLL
                                            CallWb.StartAAImage(MesData.cleanData.carrierData.SN, MesData.cleanData.carrierData.FN);
                                            Thread.Sleep(500);
                                            _watch.Restart();
                                            step = 845;
                                        }
                                        else
                                        {
                                            _watch.Restart();
                                            Config.Instance.CleanProductOkTotal++;
                                            Marking.CleanResult = true;
                                            Marking.WhiteBoardResult = true;
                                            Marking.WhiteLightResult = true;
                                            Marking.WbGetResultFlg = true;
                                            step = 370;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        AppendText($"白板程序异常！{ex}");
                                        Config.Instance.CleanProductNgTotal++;
                                        Marking.WhiteBoardResult = false;
                                        Marking.WhiteLightResult = false;
                                        Marking.WbGetResultFlg = true;
                                        step = 360;
                                    }
                                    if (_watch.ElapsedMilliseconds / 1000 > 20)
                                    {
                                        AppendText("AA工位超时");
                                        _watch.Restart();
                                        step = 360;
                                        Marking.WhiteBoardResult = false;
                                        Marking.WhiteLightResult = false;
                                        Marking.WbGetResultFlg = true;
                                    }
                                }
                                break;

                            case 845://获取测试状态

                                if (CallWb.GetAAImageStatus() == (int)AAImageSTATUS.AAImage_READY)//测试完成
                                {
                                    step = 846;
                                }
                                else if (CallWb.GetAAImageStatus() == (int)AAImageSTATUS.AAImage_TESETING)//测试中
                                {

                                }
                                else if (CallWb.GetAAImageStatus() == (int)AAImageSTATUS.AAImage_WARMING)//报警
                                {
                                    AppendText("AA工位报警");
                                    step = 360;//报警
                                    Marking.WhiteBoardResult = false;
                                    Marking.WhiteLightResult = false;
                                    Marking.WbGetResultFlg = true;
                                }
                                if (_watch.ElapsedMilliseconds / 1000 > 20)
                                {
                                    AppendText("AA工位超时");
                                    _watch.Restart();
                                    step = 360;
                                    Marking.WhiteBoardResult = false;
                                    Marking.WhiteLightResult = false;
                                    Marking.WbGetResultFlg = true;
                                }
                                break;

                            case 846://获取测试结果
                                if (CallWb.GetAAImageTestResult(buf) == (int)AAImageResult.AAIamge_PASS)
                                {
                                    Marking.WhiteLightResult = true;
                                    Marking.WhiteBoardResult = true;
                                }
                                else if (CallWb.GetAAImageTestResult(buf) == (int)AAImageResult.AAImage_LightON_NG)
                                {
                                    Marking.WhiteLightResult = false;//改动
                                    Marking.WhiteBoardResult = false;
                                }
                                else if (CallWb.GetAAImageTestResult(buf) == (int)AAImageResult.AAImage_Particle_NG)
                                {
                                    Marking.WhiteLightResult = true;
                                    Marking.WhiteBoardResult = false;
                                }
                                else if (CallWb.GetAAImageTestResult(buf) == (int)AAImageResult.AA_SN_NG)
                                {
                                    Marking.WhiteLightResult = true;
                                    Marking.WhiteBoardResult = false;
                                }
                                log.Info(buf);
                                step = 360;
                                break;

                            case 360://白板测试结果
                                if (Marking.WhiteLightResult && Marking.WhiteBoardResult)
                                {
                                    log.Debug("点亮成功OK");
                                    AppendText("点亮成功，识别结果为：OK");
                                    Config.Instance.CleanProductOkTotal++;
                                    Marking.CleanResult = true;
                                    Marking.WbGetResultFlg = true;
                                    step = 370;
                                }
                                else if(false)   //(!Marking.WhiteLightResult)  //屏蔽二次点亮
                                {
                                    if (!Marking.WbCheckAgainFlg && wbCheckCount < 1)
                                    {
                                        AppendText("点亮失败，重新顶升气缸");
                                        step = 361;
                                    }
                                    else if (Marking.WbCheckAgainFlg && wbCheckCount >= 1)
                                    {
                                        Marking.WbCheckAgainFlg = false;
                                        wbCheckCount = 0;
                                        AppendText("再次点亮失败");
                                        log.Debug("再次点亮失败");
                                        Config.Instance.CleanProductNgTotal++;
                                        Marking.CleanResult = false;
                                        Marking.WbGetResultFlg = true;
                                        step = 370;
                                    }                              
                                }
                                else
                                {
                                    wbCheckCount = 0;
                                    AppendText("点亮失败，结果为：NG");
                                    log.Debug("点亮失败,结果NG");
                                    Config.Instance.CleanProductNgTotal++;
                                    Marking.CleanResult = false;
                                    Marking.WbGetResultFlg = true;
                                    step = 370;
                                }
                                break;
                            case 361://点亮失败重新顶升
                                IoPoints.IDO9.Value = false;
                                Thread.Sleep(500);
                                CleanUpCylinder.Reset();

                                Thread.Sleep(1000);
                                log.Debug("点亮失败，重新顶升气缸");
                                CleanUpCylinder.Set();                                
                                Thread.Sleep(500);
                                IoPoints.IDO9.Value = true;
                                step = 362;
                                break;
                            case 362://气缸顶升后重新开始白板检测
                                if (CleanUpCylinder.OutMoveStatus)
                                {
                                    Marking.WbCheckAgainFlg = true;
                                    wbCheckCount++;//单个产品点亮次数+1
                                    step = 350;
                                }
                                break;
                            case 370://光源上下气缸复位
                                if (LightUpDownCylinder.OutMoveStatus)
                                {
                                    LightUpDownCylinder.Reset();
                                    Thread.Sleep(200);
                                }
                                step = 380;
                                break;
                            case 380:// Z轴返回安全位
                                Zaxis.MoveTo(Position.Instance.CleanSafePosition.Z, AxisParameter.Instance.RZspeed);
                                Marking.CleanHoming = true;
                                Marking.CleanFinishBit = true;
                                step = 390;
                                break;
                            case 390:
                                if (Zaxis.IsInPosition(Position.Instance.CleanSafePosition.Z))
                                {
                                    //Xaxis.MoveTo(Position.Instance.CleanConeFirstPosition.X, AxisParameter.Instance.LXspeed);
                                    //Yaxis.MoveTo(Position.Instance.CleanConeFirstPosition.Y, AxisParameter.Instance.LYspeed);
                                    MoveLine2Absolute(Xaxis, Yaxis, Position.Instance.CleanConeFirstPosition, AxisParameter.Instance.LXspeed);
                                    step = 410;//流程结束
                                }
                                break;
                            case 410://阻挡气缸下降
                                if ((Marking.CleanFinishBit || Marking.CleanShield) && !Marking.CleanRecycleRun)
                                {
                                    Marking.CleanWorkFinish = true;
                                    if (Marking.GlueCallIn && !Marking.CleanRecycleRun)
                                    {
                                        CleanStopCylinder.Set();
                                        lock (MesData.CleanDataLock)
                                        {
                                            MesData.cleanData.HaveLens = Marking.HaveLens ? "OK" : "NG";
                                            MesData.cleanData.WbData = Marking.WbData;
                                            Marking.WbData = "";
                                            MesData.cleanData.WbLResult = Marking.WhiteLightResult ? "OK" : "NG";
                                            MesData.cleanData.WbResult = Marking.WhiteBoardResult ? "OK" : "NG";
                                            MesData.cleanData.CleanResult = Marking.CleanResult;
                                            lock (MesData.GlueDataLock)
                                            {
                                                MesData.glueData.cleanData.carrierData.SN = MesData.cleanData.carrierData.SN;
                                                MesData.glueData.cleanData.carrierData.FN = MesData.cleanData.carrierData.FN;
                                                MesData.glueData.cleanData.carrierData.StartTime = MesData.cleanData.carrierData.StartTime;
                                                MesData.glueData.cleanData.HaveLens = MesData.cleanData.HaveLens;
                                                MesData.glueData.cleanData.WbData = MesData.cleanData.WbData;
                                                MesData.glueData.cleanData.WbLResult = MesData.cleanData.WbLResult;
                                                MesData.glueData.cleanData.WbResult = MesData.cleanData.WbResult;                                                
                                                MesData.glueData.cleanData.CleanResult = MesData.cleanData.CleanResult;
                                            }
                                            MesData.cleanData.carrierData.SN = "";
                                            MesData.cleanData.carrierData.FN = "";
                                            MesData.cleanData.carrierData.StartTime = "";
                                            MesData.cleanData.HaveLens = "";
                                            MesData.cleanData.WbData = "";
                                            MesData.cleanData.WbLResult = "";
                                            MesData.cleanData.WbResult = "";                                            
                                            MesData.cleanData.CleanResult = false;
                                        }
                                        step = 420;
                                    }
                                }
                                else if (Marking.CleanRecycleRun)

                                    step = 420;
                                break;
                            case 420://顶升气缸下降(清洗结束)                                   
                                if (IoPoints.IDO9.Value == true)
                                {
                                    IoPoints.IDO9.Value = false;
                                    Thread.Sleep(1000);
                                }
                                CleanUpCylinder.Reset();
                                IoPoints.IDO9.Value = true;
                                Thread.Sleep(1500);
                                Marking.CleanCallOut = true;
                                Marking.CleanCallOutFinish = false;
                                _repositoryWatch.Stop();
                                Marking.CleanCycleTime = _repositoryWatch.ElapsedMilliseconds / 1000.0;
                                if (Marking.CleanRecycleRun)
                                {
                                    CleanStopCylinder.Reset();
                                    Marking.CleanStart = false;
                                    Marking.CleanFinish = true;
                                    Marking.CleanFinishBit = false;
                                    Marking.CleanHoming = false;
                                    Marking.CleanWorking = false;
                                    Marking.CleanCallOut = false;
                                    Marking.CleanCallOutFinish = true;
                                    Thread.Sleep(100);
                                    step = 0;
                                }
                                else
                                    step = 430;
                                break;
                            case 430://阻挡气缸复位                           
                                if (IoPoints.IDO9.Value)//默认在传送带正转2s后，复位阻挡汽缸
                                {
                                    Thread.Sleep(500);
                                    CleanStopCylinder.Reset();
                                    Marking.CleanStart = false;
                                    Marking.CleanFinish = true;
                                    Marking.CleanFinishBit = false;
                                    Marking.CleanHoming = false;
                                    Marking.CleanWorking = false;
                                    Marking.CleanCallOut = false;
                                    Marking.CleanCallOutFinish = true;
                                    Thread.Sleep(100);
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

                    #region 初始化控制流程
                    if (stationInitialize.Running)
                    {
                        switch (stationInitialize.Flow)
                        {

                            case 0://清除所有标志位
                                stationInitialize.InitializeDone = false;
                                stationOperate.RunningSign = false;
                                wbCheckCount = 0;
                                Marking.CleanCallIn = false;
                                Marking.CleanWorking = false;
                                Marking.CleanWorkFinish = false;
                                Marking.CleanHoming = false;
                                Marking.CleanCallOut = false;
                                Marking.CleanCallOutFinish = false;
                                IoPoints.IDO16.Value = false;
                                //Marking.WbCheckAgainFlg = false;
                                Marking.WbRequestResultFlg = false;
                                Marking.WhiteBoardResult = false;
                                Marking.WhiteLightResult = false;
                                step = 0;
                                Xaxis.Stop();
                                Yaxis.Stop();
                                Zaxis.Stop();
                                Thread.Sleep(200);
                                if (!Xaxis.IsAlarmed && !Yaxis.IsAlarmed && !Zaxis.IsAlarmed)
                                {
                                    Xaxis.IsServon = false;
                                    Yaxis.IsServon = false;
                                    Zaxis.IsServon = false;
                                    Xaxis.Clean();
                                    Yaxis.Clean();
                                    Zaxis.Clean();
                                    Thread.Sleep(200);
                                    stationInitialize.Flow = 10;
                                    Thread.Sleep(200);
                                }
                                break;
                            case 10://气缸复位
                                Zaxis.IsServon = true;
                                CleanStopCylinder.InitExecute();
                                CleanStopCylinder.Reset();
                                CleanClampCylinder.InitExecute();
                                CleanClampCylinder.Reset();
                                LightUpDownCylinder.InitExecute();
                                LightUpDownCylinder.Reset();
                                Thread.Sleep(100);
                                if (CleanClampCylinder.OutOriginStatus)
                                {
                                    CleanRotateCylinder.InitExecute();
                                    CleanRotateCylinder.Reset();
                                    Thread.Sleep(100);
                                }
                                if (CleanRotateCylinder.OutOriginStatus)
                                {
                                    CleanUpDownCylinder.InitExecute();
                                    CleanUpDownCylinder.Reset();
                                    Thread.Sleep(100);
                                    //stationInitialize.Flow = 13;
                                }
                                if (CleanUpDownCylinder.OutOriginStatus)
                                {
                                    CleanUpCylinder.InitExecute();
                                    CleanUpCylinder.Reset();
                                    Thread.Sleep(100);
                                    stationInitialize.Flow = 20;
                                }
                                break;
                            case 20://启动IZ轴回原点
                                if (Zaxis.IsServon && IoPoints.IDI8.Value && IoPoints.IDI2.Value)
                                {
                                    Zaxis.BackHome();
                                    stationInitialize.Flow = 30;
                                }
                                break;
                            case 30://判断IZ是否回原点
                                if (IoPoints.m_ApsController.CheckHomeDone(Zaxis.NoId, 30.0) == 0)
                                {
                                    AppendText($"{Zaxis.Name}回原点完成！");
                                    Xaxis.IsServon = true;
                                    Yaxis.IsServon = true;
                                    Thread.Sleep(200);
                                    stationInitialize.Flow = 40;
                                }
                                else
                                {
                                    Zaxis.Stop();
                                    stationInitialize.InitializeDone = false;
                                    m_Alarm = PlateformAlarm.初始化故障;
                                    stationInitialize.Flow = -1;
                                }
                                break;
                            case 40://启动XY轴回原点
                                if (Xaxis.IsServon && Yaxis.IsServon)
                                {
                                    Xaxis.BackHome();
                                    Yaxis.BackHome();
                                    stationInitialize.Flow = 50;
                                }
                                break;
                            case 50://判断X是否回原点
                                if (IoPoints.m_ApsController.CheckHomeDone(Xaxis.NoId, 60.0) == 0)
                                {
                                    AppendText($"{Xaxis.Name}回原点完成！");

                                    stationInitialize.Flow = 60;
                                }
                                else
                                {
                                    Xaxis.Stop();
                                    stationInitialize.InitializeDone = false;
                                    m_Alarm = PlateformAlarm.初始化故障;
                                    stationInitialize.Flow = -1;
                                }
                                break;
                            case 55:
                                if (Yaxis.IsServon)
                                {
                                    Yaxis.BackHome();
                                    stationInitialize.Flow = 60;
                                }
                                break;
                            case 60://判断Y轴是否回原点,启动C0、C1轴回原点
                                if (IoPoints.m_ApsController.CheckHomeDone(Yaxis.NoId, 60.0) == 0)
                                {
                                    AppendText($"{Yaxis.Name}回原点完成！");
                                    stationInitialize.Flow = 70;
                                }
                                else
                                {
                                    Yaxis.Stop();
                                    stationInitialize.InitializeDone = false;
                                    m_Alarm = PlateformAlarm.初始化故障;
                                    stationInitialize.Flow = -1;
                                }
                                break;
                            case 70://复位完成
                                Xaxis.Stop();
                                Yaxis.Stop();
                                Zaxis.Stop();
                                Thread.Sleep(500);
                                Xaxis.APS_set_command(0.0);
                                Yaxis.APS_set_command(0.0);
                                Zaxis.APS_set_command(0.0);
                                Thread.Sleep(200);
                                stationInitialize.InitializeDone = true;
                                AppendText($"{Name}初始化完成！");
                                stationInitialize.Flow = 80;
                                break;
                            case 80://Z轴移至安全位
                                Zaxis.MoveTo(Position.Instance.CleanSafePosition.Z, AxisParameter.Instance.LZspeed);
                                Thread.Sleep(100);
                                stationInitialize.Flow = 90;
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion

                    //清除所有报警信息
                    if (AlarmReset.AlarmReset)
                    {
                        m_Alarm = PlateformAlarm.无消息;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("软件异常001,请关闭重启" + ex.ToString());

            }
        }
        /// <summary>
        /// 报警信息集合
        /// </summary>
        protected override IList<Alarm> alarms()
        {
            var list = new List<Alarm>();
            list.AddRange(Xaxis.Alarms);
            list.AddRange(Yaxis.Alarms);
            list.AddRange(Zaxis.Alarms);
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.初始化故障)
            {
                AlarmLevel = AlarmLevels.None,
                Name = PlateformAlarm.初始化故障.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.顶升气缸未复位或平台不在原位)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.顶升气缸未复位或平台不在原位.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.清洗工位料盘检测有误)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.清洗工位料盘检测有误.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.白板发送失败)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.白板发送失败.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.白板接收异常)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.白板接收异常.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.白板通讯超时)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.白板通讯超时.ToString()
            });
            list.AddRange(CleanStopCylinder.Alarms);
            list.AddRange(CleanUpCylinder.Alarms);
            list.AddRange(CleanClampCylinder.Alarms);
            list.AddRange(CleanRotateCylinder.Alarms);
            list.AddRange(CleanUpDownCylinder.Alarms);
            list.AddRange(LightUpDownCylinder.Alarms);
            //list.AddRange(LightingTestCylinder.Alarms);
            return list;
        }

        /// <summary>
        /// 气缸状态集合
        /// </summary>
        protected override IList<ICylinderStatusJugger> cylinderStatus()
        {
            var list = new List<ICylinderStatusJugger>();
            //要添加气缸
            list.Add(CleanStopCylinder);
            list.Add(CleanUpCylinder);
            list.Add(CleanRotateCylinder);
            list.Add(CleanClampCylinder);
            list.Add(CleanUpDownCylinder);
            list.Add(LightUpDownCylinder);

            return list;
        }

        public enum PlateformAlarm : int
        {
            无消息,
            初始化故障,
            顶升气缸未复位或平台不在原位,
            清洗工位料盘检测有误,
            等待扫码结果,
            执行条件不满足,
            气缸不在状态位,
            白板通讯超时,
            白板发送失败,
            白板接收异常
        }

        public enum AAImageSTATUS : int
        {
            AAImage_READY = 0,
            AAImage_TESETING,
            AAImage_WARMING,
        };

        public enum AAImageResult : int
        {
            AAIamge_PASS = 0,
            AAImage_LightON_NG,
            AAImage_Particle_NG,
            AA_SN_NG,
        };

        public bool WbINItrans(string ModelName)
        {

            if (File.Exists(AppConfig.WbIniPath) == false)
            {
                MessageBox.Show($"白板参数文件不存在:{AppConfig.WbIniPath}");
                return false;
            }

            IniHelper.WriteValue("ProjectConfig", "ModelName", ModelName, AppConfig.WbIniPath);
            return true;
        }

        #region 清洗连续直线插补
        int iBord = 0;
        int iTarbleID = 0;
        int iPt = 0;

        /// <summary>
        /// buffer空间状态
        /// </summary>
        PTSTS ptStatus = new PTSTS { };
        PTLINE Line = new PTLINE { };
        /// <summary>
        /// 初始化列表 table
        /// </summary>
        /// <returns></returns>
        public bool InitBufferMode(int iAxisNum, int VerX)
        {
            try
            {

                int[] arr = new int[] { Xaxis.NoId, Yaxis.NoId, Zaxis.NoId };
                //禁能 点表功能
                APS168.APS_pt_disable(iBord, iTarbleID);
                //致能 点表功能
                APS168.APS_pt_enable(iBord, iTarbleID, iAxisNum, arr);
                //设置 点表功能 参数
                APS168.APS_pt_set_absolute(iBord, iTarbleID);
                APS168.APS_pt_set_trans_buffered(iBord, iTarbleID);
                APS168.APS_pt_set_acc(iBord, iTarbleID, VerX * AxisParameter.Instance.LXTransParams.EquivalentPulse);
                APS168.APS_pt_set_dec(iBord, iTarbleID, VerX * AxisParameter.Instance.LXTransParams.EquivalentPulse);

            }
            catch { return false; }
            return true;
        }

        /// <summary>
        /// 监控获取Buffer空间状态
        /// </summary>
        /// <param name="Status">buffer状态</param>
        public void APSgetPtStatus(ref PTSTS Status)
        {
            try { APS168.APS_get_pt_status(iBord, iPt, ref Status); }
            catch { }
        }

        public void DoRect(int iAxisNum, double x, double y, double z = 0.0, int Verx = 10, int Very = 10, int VerZ = 10)
        {
            try
            {

                APSgetPtStatus(ref ptStatus);
                if ((ptStatus.BitSts & 0X02) == 0)
                {
                    //最大速度
                    APS168.APS_pt_set_vm(iBord, iPt, Verx * AxisParameter.Instance.LXTransParams.EquivalentPulse);
                    //结束速度
                    APS168.APS_pt_set_ve(iBord, iPt, Verx * AxisParameter.Instance.LXTransParams.EquivalentPulse);
                    Line.Dim = iAxisNum;
                    //设置直线数据
                    Line.Pos = new double[6];
                    Line.Pos[0] = (Double)x * AxisParameter.Instance.LXTransParams.EquivalentPulse;
                    Line.Pos[1] = (Double)y * AxisParameter.Instance.LYTransParams.EquivalentPulse;
                    Line.Pos[2] = (Double)z * AxisParameter.Instance.LZTransParams.EquivalentPulse;
                    Line.Pos[3] = 0.0;
                    Line.Pos[4] = 0.0;
                    Line.Pos[5] = 0.0;
                    //点表中插入2个点，进行直线插补
                    APS168.APS_pt_line(iBord, iPt, ref Line, ref ptStatus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void APSptStart()
        {
            try
            {
                APS168.APS_pt_start(iBord, iPt);
                while (true)
                {
                    APSgetPtStatus(ref ptStatus);
                    if ((ptStatus.BitSts & 0x01) == 0)//Buffer is done ，缓存运动结束
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }
            }
            catch { }
        }
        #endregion

        /// <summary>
        /// 两轴直线插补（绝对运动）
        /// </summary>
        public void MoveLine2Absolute(ServoAxis axis_a, ServoAxis axis_b, Point3D<double> point, VelocityCurve velParams)
        {
            int pulseNum1 = Convert.ToInt32(point.X / AxisParameter.Instance.LXTransParams.PulseEquivalent);
            int pulseNum2 = Convert.ToInt32(point.Y / AxisParameter.Instance.LYTransParams.PulseEquivalent);
            var velocity = velParams;
            velocity.Maxvel = velParams.Maxvel * AxisParameter.Instance.LXTransParams.EquivalentPulse;
            IoPoints.m_ApsController.MoveLine2Absolute(axis_a.NoId, axis_b.NoId, pulseNum1, pulseNum2, velocity);
            velocity.Maxvel /= AxisParameter.Instance.LXTransParams.EquivalentPulse;
        }

    }

}
