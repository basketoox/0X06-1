 using System.Collections.Generic;
using Motion.Enginee;
using Motion.Interfaces;
using System.Threading;
using System.Diagnostics;
using System;
using System.Toolkit;
using System.Collections;
using System.Threading.Tasks;
using desay.ProductData;
using System.IO;
using Motion.AdlinkAps;
using System.Device;
using System.Windows.Forms;

namespace desay.Flow
{
    /// <summary>
    /// 点胶平台控制流程
    /// </summary>
    public class GluePlateform : StationPart
    {
        private PlateformAlarm m_Alarm;
        public Thread threadRun = null;
        public int count;//点胶NG计数

        public GluePlateform(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe)
                        : base(ExternalSign, stationIni, stationOpe, typeof(GluePlateform))
        {
            //threadRun = new Thread(run);
            //threadRun.IsBackground = true;
        }     
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
        /// 点胶阻挡气缸
        /// </summary>
        public SingleCylinder GlueStopCylinder { get; set; }
        /// <summary>
        /// 点胶顶升气缸
        /// </summary>
        public SingleCylinder GlueUpCylinder { get; set; }
        #endregion

        public event Action<int> SendRequest;
        int failCount = 0;

        public bool GlueHomeBit;
        public Point3D<int> ccdOffset;
        public override void Running(RunningModes runningMode)
        {
            Stopwatch watchGlueCT = new Stopwatch();
            watchGlueCT.Start();
            var _watch = new Stopwatch();
            _watch.Start();
            uint GlueCheckCount = 0;
            double glueHeightOffset = 0.0;//new add
            double glueHeightTotal = 0.0;//new add
            int glueHeightTotalTimes = 0;
            try
            {


           
            while (true)
            {
                Thread.Sleep(10);
                GlueStopCylinder.Condition.External = AlarmReset;
                GlueUpCylinder.Condition.External = AlarmReset;

                GlueHomeBit = /*IoPoints.IDI17.Value &&*/ IoPoints.IDI12.Value && Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z);

                #region 自动运行流程
                if (stationOperate.Running)
                {

                    Marking.GlueHaveProduct = (IoPoints.IDI12.Value || Marking.GlueHaveProductSheild) && GlueUpCylinder.OutOriginStatus;
                    switch (step)
                    {
                        case 0: //判断平台是否在原点，顶升是否在原位
                            Marking.GlueCallOutFinish = false;
                            Marking.GlueWorking = true;
                            IoPoints.IDO19.Value = false;
                            if (Marking.GlueHaveProduct/* && !Marking.GlueShield && !Marking.GlueFinishBit && (MesData.glueData.cleanData.CleanResult || Marking.CleanShield)*/)
                            {
                                Marking.GlueCallIn = false;
                                if (GlueHomeBit)
                                    step = 50;
                                else
                                    step = 10;
                            }
                            else
                            {
                                Marking.GlueCallIn = true;
                                Marking.GlueFinishBit = false;
                            }

                            break;
                        case 10:  //复位所有气缸的动作

                            GlueStopCylinder.Reset();
                            GlueUpCylinder.Reset();
                            step = 20;
                            break;
                        case 20:    //判断所有气缸到位，启动Z轴回安全位
                            if (GlueUpCylinder.OutOriginStatus)
                            {
                                Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                                step = 30;
                            }
                            break;
                        case 30:  //判断Z轴是否在安全位
                            if (Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                            {
                                step = 50;
                            }
                            break;
                        case 50: //顶升气缸顶起
                            if (GlueHomeBit)
                            {
                                Thread.Sleep(50);
                                watchGlueCT.Restart();
                                Marking.GlueCycleTime = 0;

                                GlueUpCylinder.Set();
                                lock (MesData.GlueDataLock)
                                {
                                    if (Marking.GlueHaveProduct && !Marking.GlueShield
                                        && (MesData.glueData.cleanData.CleanResult || Marking.CleanShield))
                                    {
                                        //step = 60;//相机定位拍照
                                        //step = 120;//Z轴返回安全位
                                        step = 51;//Z轴返回安全位
                                    }
                                    else
                                    {
                                        //  Marking.GlueResult = true;
                                        step = 210;//结束流程
                                    }
                                }
                            }
                            else
                                step = 0;
                            break;
                        case 51://判断Z轴是否在回原位  
                            Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                            step = 52;
                            break;
                        case 52://检测Z轴是否在安全位，XY轴移至测高位置
                            if (Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                            {
                                Xaxis.MoveTo(Position.Instance.GlueHeightPosition.X, AxisParameter.Instance.RXspeed);
                                Yaxis.MoveTo(Position.Instance.GlueHeightPosition.Y, AxisParameter.Instance.RYspeed);
                                step = 53;
                            }
                            break;
                        case 53://Z轴移至测高位置
                            if (Xaxis.IsInPosition(Position.Instance.GlueHeightPosition.X)
                            && Yaxis.IsInPosition(Position.Instance.GlueHeightPosition.Y))
                            {
                                Zaxis.MoveTo(Position.Instance.GlueHeightPosition.Z, new VelocityCurve()
                                { Strvel = 5.00, Maxvel = 10.0, Tacc = 0.1, Tdec = 0.1, VelocityCurveType = CurveTypes.T });
                                step = 54;
                            }
                            break;
                        case 54://发送测高指令
                            if (Zaxis.IsInPosition(Position.Instance.GlueHeightPosition.Z))
                            {
                                Marking.RequestHeightFlg = true;
                                Thread.Sleep(1000);//测高模块需要等待一会数据稳定
                                SendRequest(1);
                                _watch.Restart();
                                step = 55;
                            }
                            break;
                        case 55://读取测高结果
                            if (Marking.GetHeightFlg)
                            {
                                if (Marking.RequestHeightError)
                                {
                                    AppendText("测高识别异常，重新获取测高数据");
                                    step = 54;
                                }
                                else
                                {
                                    glueHeightOffset = Position.Instance.DetectHeight2BaseHeight;//new add
                                    Marking.GetHeightFlg = false;
                                    step = 60;

                                }
                            }
                            //_watch.Stop();
                            if (_watch.ElapsedMilliseconds/1000.0>30)
                            {
                                _watch.Restart();
                                m_Alarm = PlateformAlarm.测高模块通讯超时;
                                step = 54;//测高启动
                            }

                            break;
                        case 60:// Z轴返回安全位
                            Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                            step = 65;
                            if (glueHeightOffset > Position.Instance.DetectHeightOffsetUp || glueHeightOffset < -Position.Instance.DetectHeightOffsetDown)
                            {
                                AppendText("测高偏差过大异常");
                                log.Debug("测高偏差过大异常");
                                m_Alarm = PlateformAlarm.测高偏差值过大异常;
                                step = 175;//当点胶异常  //测高异常偏差大
                            }

                            break;
                        case 65://XY模组移至相机拍照位置
                            if (GlueUpCylinder.OutMoveStatus)
                            {
                                Xaxis.MoveTo(Position.Instance.GlueCameraPosition.X, AxisParameter.Instance.RXspeed);
                                Yaxis.MoveTo(Position.Instance.GlueCameraPosition.Y, AxisParameter.Instance.RYspeed);
                                step = 70;
                            }
                            break;
                        case 70://Z模组移至相机拍照位置
                            if (Yaxis.IsInPosition(Position.Instance.GlueCameraPosition.Y)
                                && Xaxis.IsInPosition(Position.Instance.GlueCameraPosition.X))
                            {
                                Zaxis.MoveTo(Position.Instance.GlueCameraPosition.Z, AxisParameter.Instance.RZspeed);
                                step = 80;
                            }
                            log.Debug("70");
                            break;
                        case 80://CCD定位抓图
                               try
                                {


                              
                            if (Zaxis.IsInPosition(Position.Instance.GlueCameraPosition.Z))
                            {
                                Marking.GlueResult = false;//结果复位
                                if (!Marking.CCDShield)
                                {
                                    AppendText("点胶定位识别");
                                    Marking.CenterLocateTestFinish = false;
                                    Marking.CenterLocateTestSucceed = false;
                                    frmAAVision.acq.CenterLocateTestAcquire();
                                    _watch.Restart();
                                    step = 90;
                                }
                                else { step = 120; if (Position.Instance.UseRectGlue) step = 93; }//Z轴返回安全位
                            }
                                }
                                catch (Exception ex)
                                {
                                    AppendText("点胶定位识别异常，拍照异常01");
                                    //step = 80;
                                    Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                                    step = 175;//////////


                                }
                                log.Debug("80");

                            break;
                        case 90://点胶定位结果
                            if (Marking.CenterLocateTestFinish)
                            {
                                Marking.CenterLocateTestFinish = false;
                                if (Marking.CenterLocateTestSucceed)
                                {
                                    AppendText("点胶定位识别完成");
                                    step = 92;
                                }
                                else
                                {

                                    AppendText("点胶定位识别异常，认为点胶NG发送给AA");
                                    //step = 80;
                                    Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                                    step = 175;//////////
                                }
                            }
                            log.Debug("90");
                            break;
                            //***********************************20201121***************************************//
                            //新增矩形点胶  目前没有加入测高和视觉 先固定位置点
                         case 92:
                                if (Position.Instance.UseRectGlue) step = 93;
                                else step = 100;
                            break;
                        case 93:
                                Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                                step = 94;
                                break;
                       case 94:
                                if (Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                                {

                                    Xaxis.MoveTo(Config.Instance.RectX[0], AxisParameter.Instance.RXspeed);
                                    Yaxis.MoveTo(Config.Instance.RectY[0], AxisParameter.Instance.RYspeed);
                                   Zaxis.MoveTo(Config.Instance.RectZ, AxisParameter.Instance.RZspeed);
                                    step = 95;
                                }
                             break;
                       
                            case 95:
                                if (Xaxis.IsInPosition(Config.Instance.RectX[0]) && Yaxis.IsInPosition(Config.Instance.RectY[0]) &&Zaxis.IsInPosition(Config.Instance.RectZ))
                                {

                                    InitBufferMode(3, (int)AxisParameter.Instance.GluePathSpeed.Maxvel);
                                    DoRect(3, Config.Instance.RectX[0], Config.Instance.RectY[0], Config.Instance.RectZ);
                                    DoRect(3, Config.Instance.RectX[1], Config.Instance.RectY[1], Config.Instance.RectZ);
                                    DoRect(3, Config.Instance.RectX[2], Config.Instance.RectY[2], Config.Instance.RectZ);
                                    DoRect(3, Config.Instance.RectX[3], Config.Instance.RectY[3], Config.Instance.RectZ);
                                    DoRect(3, Config.Instance.RectX[0]+1, Config.Instance.RectY[0], Config.Instance.RectZ);



                                    APSptStart(Config.Instance.GlueRectNOoneDelayTime, !Marking.GlueRun);
                                    Xaxis.MoveTo(Config.Instance.RectX[3], AxisParameter.Instance.GluePathSpeed);
                                    Yaxis.MoveTo(Config.Instance.RectY[3], AxisParameter.Instance.GluePathSpeed);
                 
                                    step = 96;
                                }
                                break;
                            case 96:
                                if (Xaxis.IsInPosition(Config.Instance.RectX[3])&& Yaxis.IsInPosition(Config.Instance.RectY[3]))
                                {

                                    Marking.GlueResult = true;
                                    Marking.CcdGetResultFlg = true;
                                    Marking.CcdGetResultFailFlg = false;

                                    step = 180;//结束 忽略  点胶识别
                                }
                                break;

                            case 100://计算点胶位置
                            Position.Instance.GlueCenterPosition.X = Position.Instance.GlueCameraPosition.X - Position.Instance.CCD2NeedleOffset.X + Position.Instance.PCB2CCDOffset.X + Position.Instance.GlueOffsetX;
                            Position.Instance.GlueCenterPosition.Y = Position.Instance.GlueCameraPosition.Y - Position.Instance.CCD2NeedleOffset.Y - Position.Instance.PCB2CCDOffset.Y + Position.Instance.GlueOffsetY;
                            Position.Instance.GlueStartPosition.X = Position.Instance.GlueCenterPosition.X - Position.Instance.GlueRadius;
                            Position.Instance.GlueStartPosition.Y = Position.Instance.GlueCenterPosition.Y;
                            Position.Instance.GlueSecondPosition.X = Position.Instance.GlueCenterPosition.X;
                            Position.Instance.GlueSecondPosition.Y = Position.Instance.GlueCenterPosition.Y - Position.Instance.GlueRadius;
                            Position.Instance.GlueThirdPositon.X = Position.Instance.GlueCenterPosition.X + Position.Instance.GlueRadius;
                            Position.Instance.GlueThirdPositon.Y = Position.Instance.GlueCenterPosition.Y;
                            log.Debug(string.Format("Center-X:{0},Y:{1};Start-X:{2},Y:{3};Second-X:{4},Y:{5};End-X:{6},Y:{7}",
                                Position.Instance.GlueCenterPosition.X, Position.Instance.GlueCenterPosition.Y,
                                Position.Instance.GlueStartPosition.X, Position.Instance.GlueStartPosition.Y,
                                Position.Instance.GlueSecondPosition.X, Position.Instance.GlueSecondPosition.Y,
                                Position.Instance.GlueThirdPositon.X, Position.Instance.GlueThirdPositon.Y));
                            step = 120;
                            log.Debug("100");
                            break;

                        case 120:// Z轴返回安全位
                            Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                            step = 121;

                            break;
                        case 121:// XY轴点胶圆形轨迹起点
                            if (Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                            {
                                Xaxis.MoveTo(Position.Instance.GlueStartPosition.X, AxisParameter.Instance.RXspeed);
                                Yaxis.MoveTo(Position.Instance.GlueStartPosition.Y, AxisParameter.Instance.RYspeed);

                                step = 122;
                            }

                            break;
                        case 122://Z 点胶圆形轨迹起点
                            if (Xaxis.IsInPosition(Position.Instance.GlueStartPosition.X)
                                && Yaxis.IsInPosition(Position.Instance.GlueStartPosition.Y))
                            {

                                Zaxis.MoveTo(glueHeightOffset + Position.Instance.GlueHeight, AxisParameter.Instance.RZspeed);
                                //Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z + Position.Instance.DetectHeight2BaseHeight + Position.Instance.GlueHeight, AxisParameter.Instance.RZspeed);
                                Marking.GlueHeight_value = glueHeightOffset + Position.Instance.GlueHeight;
                                //取平均值
                                if (glueHeightTotalTimes < 10) glueHeightTotal += Marking.GlueHeight_value;
                                else Marking.GlueHeight_value = glueHeightTotal / 10;
                                 glueHeightTotalTimes++;
                                AppendText($"平均点胶Z值参照值:{Marking.GlueHeight_value}");
                                AppendText($"当前点胶Z值{ glueHeightOffset + Position.Instance.GlueHeight},测高偏差值{Position.Instance.DetectHeight2BaseHeight}");
                                step = 130;
                            }
                            break;
                        case 130://XYZ前往点胶圆形轨迹起点
                            if (Zaxis.IsInPosition(glueHeightOffset + Position.Instance.GlueHeight))
                            {
                                //增加Z轴判断  悬空点胶 应该是这个变量出现了问题 Position.Instance.DetectHeight2BaseHeight
                                Thread.Sleep(100);
                                //位置异常处理
                                if ((Zaxis.CurrentPos > (Marking.GlueHeight_value + Position.Instance.DetectHeightOffsetUp))|| (Zaxis.CurrentPos < (Marking.GlueHeight_value - Position.Instance.DetectHeightOffsetDown)))
                                {
                                    AppendText($"点胶高度位置异常超出平均值限位,上限{Marking.GlueHeight_value + Position.Instance.DetectHeightOffsetUp},下限{Marking.GlueHeight_value - Position.Instance.DetectHeightOffsetDown}");
                                    Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                                    step = 175;//当点胶NG处理
                                    break;
                                }

                                int step1 = 0;
                                bool istrue = true;
                                while (istrue)
                                {

                                    switch (step1)
                                    {
                                        case 0://起始空胶
                                            APS168.APS_absolute_arc_move(2, new Int32[2] { Xaxis.NoId, Yaxis.NoId }, new Int32[2]
                                            { (int)((Position.Instance.GlueCenterPosition.X) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                                            (int)((Position.Instance.GlueCenterPosition.Y) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                                            (int)Position.Instance.GluePathSpeed * 1000, Position.Instance.StartGlueAngle);
                                            Thread.Sleep(1);
                                            step1 = 10;
                                            break;
                                        case 10://点胶第一圈
                                            if (Xaxis.IsDone && Yaxis.IsDone && Zaxis.IsDone && Xaxis.CurrentSpeed == 0 && Yaxis.CurrentSpeed == 0 && Zaxis.CurrentSpeed == 0)
                                            {
                                                APS168.APS_absolute_arc_move(2, new Int32[2] { Xaxis.NoId, Yaxis.NoId }, new Int32[2]
                                                {  (int)((Position.Instance.GlueCenterPosition.X) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                                                (int)((Position.Instance.GlueCenterPosition.Y) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                                                (int)Position.Instance.GluePathSpeed * 1000, 360);
                                                if (Marking.GlueRun)
                                                    IoPoints.IDO19.Value = false;
                                                else
                                                    IoPoints.IDO19.Value = true;
                                                Thread.Sleep(1);
                                                step1 = 20;
                                            }
                                            break;
                                        case 20://点胶第二圈
                                            if (Xaxis.IsDone && Yaxis.IsDone && Zaxis.IsDone && Xaxis.CurrentSpeed == 0 && Yaxis.CurrentSpeed == 0 && Zaxis.CurrentSpeed == 0)
                                            {
                                                APS168.APS_absolute_arc_move(2, new Int32[2] { Xaxis.NoId, Yaxis.NoId }, new Int32[2]
                                                {  (int)((Position.Instance.GlueCenterPosition.X) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                                                (int)((Position.Instance.GlueCenterPosition.Y) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                                                (int)Position.Instance.GluePathSpeed * 1000, Position.Instance.SecondGlueAngle);
                                                Thread.Sleep((int)Position.Instance.StopGlueDelay);
                                                step1 = 30;
                                            }
                                            break;
                                        case 30://点胶拖胶
                                            if (Xaxis.IsDone && Yaxis.IsDone && Zaxis.IsDone && Xaxis.CurrentSpeed == 0 && Yaxis.CurrentSpeed == 0 && Zaxis.CurrentSpeed == 0)
                                            {
                                                IoPoints.IDO19.Value = false;
                                                APS168.APS_absolute_move(Zaxis.NoId, (int)((Zaxis.CurrentPos - Position.Instance.DragGlueHeight) / AxisParameter.Instance.RYTransParams.PulseEquivalent),
                                                    (int)Position.Instance.DragGlueSpeed * 1000);
                                                APS168.APS_absolute_arc_move(2, new Int32[2] { Xaxis.NoId, Yaxis.NoId }, new Int32[2]
                                                {  (int)((Position.Instance.GlueCenterPosition.X) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                                                (int)((Position.Instance.GlueCenterPosition.Y) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                                                (int)Position.Instance.DragGlueSpeed * 1000, Position.Instance.DragGlueAngle);
                                                Thread.Sleep(1);
                                                step1 = 40;
                                            }
                                            break;
                                        case 40://点胶结束
                                            if (Xaxis.IsDone && Yaxis.IsDone && Zaxis.IsDone && Xaxis.CurrentSpeed == 0
                                                && Yaxis.CurrentSpeed == 0 && Zaxis.CurrentSpeed == 0)
                                            {
                                                IoPoints.IDO19.Value = false;
                                                istrue = false;
                                                step1 = 0;
                                            }
                                            break;

                                    }
                                }

                                step = 133;
                            }

                            break;
                        case 133: //点胶次数判断
                            if (Xaxis.IsDone && Yaxis.IsDone && Zaxis.IsDone && Xaxis.CurrentSpeed == 0 && Yaxis.CurrentSpeed == 0)
                            {
                                    Marking.GlueFinish = true;
                                    IoPoints.IDO19.Value = false;
                                    step = 140;
                            }

                            break;
                        case 140:// 判断是否CCD检测
                            if (Marking.GlueFinish && !Marking.CCDShield)
                                step = 141;//进行点胶检测
                            else if (Marking.GlueFinish && Marking.CCDShield)
                            {
                                Marking.GlueResult = true;
                                //Config.Instance.GlueProductOkTotal++;
                                step = 180;//流程结束
                            }

                            break;
                        case 141:// Z轴返回安全位
                            Marking.GlueFinish = false;
                            Thread.Sleep(10);
                            Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                            step = 142;
                                _watch.Restart();

                                break;
                           
                        case 142:// XY轴前往拍照位置 
                            if (Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                            {
                                Xaxis.MoveTo(Position.Instance.GlueCameraPosition.X, AxisParameter.Instance.RXspeed);
                                Yaxis.MoveTo(Position.Instance.GlueCameraPosition.Y, AxisParameter.Instance.RYspeed);
                                step = 143;
                                    _watch.Restart();
                                }
                                else//new add
                                {
                                    _watch.Stop();
                                    if (_watch.ElapsedMilliseconds > 12000)
                                    {
                                        _watch.Restart();
                                        AppendText("点胶检测结果异常170");
                                        Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                                        step = 175;//点胶检测结果异常170
                                    }
                                }

                                break;
                        case 143:// Z轴前往拍照位置
                            if (Xaxis.IsInPosition(Position.Instance.GlueCameraPosition.X)
                            && Yaxis.IsInPosition(Position.Instance.GlueCameraPosition.Y))
                            {
                                Zaxis.MoveTo(Position.Instance.GlueCameraPosition.Z, AxisParameter.Instance.RZspeed);
                                step = 144;
                                _watch.Restart();
                            }
                                else//new add
                                {
                                    _watch.Stop();
                                    if (_watch.ElapsedMilliseconds > 12000)
                                    {
                                        _watch.Restart();
                                        AppendText("点胶检测结果异常170");
                                        Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                                        step = 175;//点胶检测结果异常170
                                    }
                                }


                                break;
                        ///2020.8.27 临时对策  144 到175 加对策 else 超时
                        case 144://CCD拍照检测
                            if (Xaxis.IsInPosition(Position.Instance.GlueCameraPosition.X)
                                && Yaxis.IsInPosition(Position.Instance.GlueCameraPosition.Y)
                                 && Zaxis.IsInPosition(Position.Instance.GlueCameraPosition.Z))
                            {
                                //触发拍照信号                              
                                step = Marking.CCDShield ? 180 : 160;
                            }
                            else//new add
                            {
                                _watch.Stop();
                                if (_watch.ElapsedMilliseconds > 12000)
                                {
                                    _watch.Restart();
                                    AppendText("CCD拍照位到位异常144");
                                    Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                                    step = 175;//CCD拍照位到位异常
                                }
                            }
                            break;
                        case 160:
                            Marking.GlueCheckTestFinish = false;
                            Marking.GlueResult = false;
                            frmAAVision.acq.GlueCheckTestAcquire();
                            step = 170;
                            log.Debug("160");
                            _watch.Restart();
                            break;
                        case 170://点胶检测结果
                            if (Marking.GlueCheckTestFinish)
                            {
                                Marking.GlueCheckTestFinish = false;
                                if (Marking.GlueResult)
                                {
                                    AppendText("点胶识别:" + "OK");

                                    Marking.GlueResult = true;
                                    Marking.CcdGetResultFlg = true;
                                    Marking.CcdGetResultFailFlg = false;

                                    step = 180;
                                }
                                else
                                {
                                    AppendText("点胶识别:" + "NG " + "重新识别...");
                                    GlueCheckCount++;
                                    if (GlueCheckCount > 1)
                                    {
                                        Marking.GlueResult = false;
                                        Marking.CcdGetResultFlg = true;
                                        Marking.CcdGetResultFailFlg = true;
                                        step = 180;
                                    }
                                    else
                                    {
                                        step = 160;
                                    }
                                }
                            }
                            else//new add
                                {
                               //     _watch.Restart();

                                    if (_watch.ElapsedMilliseconds > 7000)
                                {
                                    _watch.Restart();
                                    AppendText("点胶检测结果异常170");
                                    Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                                    step = 175;//点胶检测结果异常170
                                }
                            }
                            log.Debug("170");
                            break;
                        case 175://定位NG 直接发点胶NG
                            if (Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                            {
                                Marking.GlueResult = false;
                                Marking.CcdGetResultFlg = true;
                                Marking.CcdGetResultFailFlg = true;
                                step = 180;
                            }
                        
                            break;
                        case 180://判断Z轴是否在回原位  
                            Marking.GlueHoming = true;
                            Marking.GlueFinishBit = true;
                            Thread.Sleep(10);
                            Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                            step = 190;
                            break;
                        case 190://检测Z轴回原位，判断XY轴是否回原位置
                            if (Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                            {
                                Marking.GlueCallOut = true;
                                Marking.GlueCallOutFinish = false;
                                watchGlueCT.Stop();
                                Marking.GlueCycleTime = watchGlueCT.ElapsedMilliseconds / 1000.0;
                                step = 210;
                            }
                            break;
                        case 210://与AA通讯
                            if (!Marking.GlueRecycleRun)
                            {
                                Marking.AaSendCodeFlg = true;
                                SendRequest(6);
                                _watch.Restart();
                                step = 220;
                            }
                            else
                            {
                                GlueStopCylinder.Reset();
                                step = 230;//顶升气缸下降
                            }
                            break;
                        case 220://等待AA入料请求
                            if (Marking.AACallIn)
                            {
                                IoPoints.IDO9.Value = true;
                                step = 230;
                            }
                            else
                            {
                                _watch.Stop();
                                if (_watch.ElapsedMilliseconds > Position.Instance.SocketTimeout)
                                {
                                    _watch.Start();
                                    m_Alarm = PlateformAlarm.AA通讯超时;
                                    step = 230;//重新发送结果
                                }
                            }
                            
                            break;
                        case 230://阻挡气缸下降
                            if (!Marking.GlueRecycleRun)
                            {
                                try
                                {
                                    lock (MesData.GlueDataLock)
                                    {

                                        MesData.glueData.GlueResult = Marking.GlueResult ? "OK" : "NG";
                                        MesData.glueData.glueResult = Marking.GlueResult;
                                        lock (MesData.MesDataLock)
                                        {
                                            if (MesData.MesDataList.ContainsKey(MesData.glueData.cleanData.carrierData.FN))
                                                MesData.MesDataList.Remove(MesData.glueData.cleanData.carrierData.FN);
                                            MesData.MesDataList.Add(MesData.glueData.cleanData.carrierData.FN, MesData.glueData);
                                        }
                                        MesData.glueData.GlueResult = "";
                                        MesData.glueData.glueResult = false;
                                        MesData.glueData.cleanData.carrierData.SN = "";
                                        MesData.glueData.cleanData.carrierData.FN = "";
                                        MesData.glueData.cleanData.carrierData.StartTime = "";
                                        MesData.glueData.cleanData.HaveLens = "";
                                        MesData.glueData.cleanData.WbData = "";
                                        MesData.glueData.cleanData.WbResult = "";
                                        MesData.glueData.cleanData.CleanResult = false;
                                    }
                                }
                                catch
                                {
                                    m_Alarm = PlateformAlarm.数据放入链表失败;
                                }
                                GlueStopCylinder.Set();
                                Thread.Sleep(500);
                            }
                            step = 240;
                            break;
                        case 240://顶升气缸下降
                            if (GlueUpCylinder.OutMoveStatus/* && (Marking.AACallIn || Marking.GlueRecycleRun)*/)
                            {
                                IoPoints.IDO9.Value = false;
                                Thread.Sleep(10);
                                GlueUpCylinder.Reset();
                                //Thread.Sleep(2000);
                                if (Marking.GlueRecycleRun)
                                {
                                    Marking.GlueFinishBit = false;
                                    Marking.GlueWorking = false;
                                    Marking.GlueHoming = false;
                                    Marking.GlueCallOut = false;
                                    Marking.GlueCallOutFinish = true;
                                    step = 0;
                                }
                                else
                                {
                                    Thread.Sleep(100);
                                    Marking.GlueFinishBit = true;
                                    IoPoints.IDO9.Value = true;
                                    step = 260;
                                }
                            }
                            break;
                        case 260://阻挡气缸复位                         
                            if (IoPoints.IDO9.Value && ! IoPoints.IDI12.Value && GlueUpCylinder.OutOriginStatus)
                            {
                                Thread.Sleep(1500);
                                GlueStopCylinder.Reset();
                                Marking.CcdGetLocationFailFlg = false;
                                Marking.CcdGetLocationFlg = false;
                                Marking.CcdGetResultFailFlg = false;
                                Marking.CcdGetResultFlg = false;
                                Marking.GlueFinishBit = false;
                                Marking.GlueWorking = false;
                                Marking.GlueHoming = false;
                                Marking.GlueCallOut = false;
                                Marking.GlueCallOutFinish = true;
                                Marking.AACallIn = false;
                                    step = 0;
                            }
                            break;
                        default:
                            stationOperate.RunningSign = false;
                            step = 0;
                            IoPoints.IDO19.Value = false;
                            IoPoints.IDO19.Value = false;
                            break;
                    }
                }
                #endregion
                #region 初始化运行流程
                if (stationInitialize.Running)
                {
                    switch (stationInitialize.Flow)
                    {
                        case 0://加载视觉参数
                            stationInitialize.Flow = 1;
                            break;
                        case 1://清除所有标志位
                            stationInitialize.InitializeDone = false;
                            stationOperate.RunningSign = false;
                            Marking.GlueCallIn = false;
                            Marking.GlueWorking = false;
                            Marking.GlueWorkFinish = false;
                            Marking.GlueHoming = false;
                            Marking.GlueCallOut = false;
                            Marking.GlueCallOutFinish = false;
                            Marking.GlueFinishBit = false;
                            IoPoints.IDO19.Value = false;
                            Marking.NeedleLocateTest = false;
                            Marking.CenterLocateTest = false;
                            Marking.GlueCheckTest = false;
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
                            }
                            break;
                        case 10://气缸复位
                            Zaxis.IsServon = true;
                            Thread.Sleep(200);
                            GlueStopCylinder.InitExecute();
                            GlueStopCylinder.Reset();
                            GlueUpCylinder.InitExecute();
                            GlueUpCylinder.Reset();
                            stationInitialize.Flow = 20;
                            break;
                        case 20://启动IZ轴回原点
                            if (Zaxis.IsServon && GlueUpCylinder.OutOriginStatus)
                            {
                                Zaxis.BackHome();
                                //if (IoGlues.m_ApsController.IsHoming(IZaxis.NoId))
                                stationInitialize.Flow = 30;
                            }
                            break;
                        case 30://判断IZ是否回原点
                            if (Zaxis.CheckHomeDone(40.0) == 0)
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
                        case 40://启动X\Y\IX轴回原点，
                            if (Xaxis.IsServon && Yaxis.IsServon)
                            {
                                Xaxis.BackHome();
                                Yaxis.BackHome();
                                stationInitialize.Flow = 50;
                            }
                            break;
                        case 50://判断X是否回原点
                            if (Xaxis.CheckHomeDone(40.0) == 0)
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
                        case 60://判断Y轴是否回原点
                            if (Yaxis.CheckHomeDone(60.0) == 0)
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
                            Thread.Sleep(200);
                            Xaxis.APS_set_command(0.0);
                            Yaxis.APS_set_command(0.0);
                            Zaxis.APS_set_command(0.0);
                            Thread.Sleep(500);

                            stationInitialize.Flow = 80;
                            stationInitialize.InitializeDone = true;
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
             //   MessageBox.Show("软件异常002,请关闭重启");
                MessageBox.Show("软件异常002,请关闭重启" + ex.ToString());
            }
        }

        private void run ()
        {
        }
        /// <summary>
        /// 流程报警集合
        /// </summary>
        protected override IList<Alarm> alarms()
        {
            var list = new List<Alarm>();
            list.AddRange(Xaxis.Alarms);
            list.AddRange(Yaxis.Alarms);
            list.AddRange(Zaxis.Alarms);
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.初始化故障) { AlarmLevel = AlarmLevels.None, Name = PlateformAlarm.初始化故障.ToString() });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.顶升气缸未复位或平台不在原位启动)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.顶升气缸未复位或平台不在原位启动.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.点胶工位料盘检测有误)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = PlateformAlarm.点胶工位料盘检测有误.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.CCD通讯超时)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.CCD通讯超时.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.CCD发送失败)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.CCD发送失败.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.CCD接收异常)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.CCD接收异常.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.结果发送失败)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.结果发送失败.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.AA通讯超时)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.AA通讯超时.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.接收到错误字符)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.接收到错误字符.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.胶水检测软件重新打开失败)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.胶水检测软件重新打开失败.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.测高偏差值过大异常)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.测高偏差值过大异常.ToString()
            });
            list.AddRange(GlueStopCylinder.Alarms);
            list.AddRange(GlueUpCylinder.Alarms);
            return list;
        }
        /// <summary>
        /// Collections of cylinder's statuses
        /// </summary>
        protected override IList<ICylinderStatusJugger> cylinderStatus()
        {
            var list = new List<ICylinderStatusJugger>();
            list.Add(GlueStopCylinder);
            list.Add(GlueUpCylinder);
            return list;
        }
        public enum PlateformAlarm : int
        {
            无消息,
            初始化故障,
            顶升气缸未复位或平台不在原位启动,
            点胶工位料盘检测有误,
            执行条件不满足,
            气缸不在状态位,
            CCD通讯超时,
            CCD发送失败,
            CCD接收异常,
            结果发送失败,
            AA通讯超时,
            测高模块通讯超时,
            测高偏差值过大异常,
            接收到错误字符,
            数据放入链表失败,
            胶水检测软件重新打开失败
        }


        int iBord = 0;
        int iTarbleID = 0;
        int iPt = 0;

        /// <summary>
        /// buffer空间状态
        /// </summary>
        PTSTS ptStatus = new PTSTS { };
        PTLINE Line=new PTLINE { };
        /// <summary>
        /// 初始化列表 table
        /// </summary>
        /// <returns></returns>
        public bool InitBufferMode(int iAxisNum,int VerX)
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
                APS168.APS_pt_set_acc(iBord, iTarbleID, VerX*AxisParameter.Instance.RXTransParams.EquivalentPulse);
                APS168.APS_pt_set_dec(iBord, iTarbleID, VerX * AxisParameter.Instance.RXTransParams.EquivalentPulse);
               
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

        public void DoRect(int iAxisNum, double x,double y,double z=0.0,int Verx=10,int Very=10,int VerZ=10)
        {
            try
            {

                APSgetPtStatus(ref ptStatus);
                if ((ptStatus.BitSts & 0X02) == 0)
                {   
                    //最大速度
                    APS168.APS_pt_set_vm(iBord, iPt, Verx * AxisParameter.Instance.RXTransParams.EquivalentPulse);
                    //结束速度
                    APS168.APS_pt_set_ve(iBord, iPt, Verx * AxisParameter.Instance.RXTransParams.EquivalentPulse);
                    Line.Dim = iAxisNum;
                    //设置直线数据
                    Line.Pos = new double[6];
                    Line.Pos[0] = (Double)x * AxisParameter.Instance.RXTransParams.EquivalentPulse;
                    Line.Pos[1] = (Double)y * AxisParameter.Instance.RYTransParams.EquivalentPulse;
                    Line.Pos[2] = (Double)z * AxisParameter.Instance.RYTransParams.EquivalentPulse;
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


        public void APSptStart(int iTime,bool bGlue)
        {
            try {
                APS168.APS_pt_start(iBord, iPt);
              
                if (bGlue) IoPoints.IDO19.Value = true;
                Thread.Sleep(iTime);
                while (true)
                {
                   
                    APSgetPtStatus(ref ptStatus);
                    if ((ptStatus.BitSts & 0x01) == 0)//Buffer is done ，缓存运动结束
                    {
                        IoPoints.IDO19.Value = false;
                        break;
                       
                    }
                    Thread.Sleep(1);
                }

            } catch { }
        }


        /// <summary>
        /// 打开自己开发的程序
        /// </summary>
        /// <param name="fileName">文件名称（比如C-MES.exe）</param>
        /// <param name="filePath">文件所在路径（比如G:\SoftWare\DMMES）</param>
        private int OpenOtherEXEMethod(string fileName, string filePath)
        {
            if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(filePath))
            {
                if (!Directory.Exists(filePath)) return -1;
                //开启一个新process
                System.Diagnostics.ProcessStartInfo p = null;
                System.Diagnostics.Process proc;

                p = new System.Diagnostics.ProcessStartInfo(fileName);
                p.WorkingDirectory = filePath;//设置此外部程序所在windows目录
                proc = System.Diagnostics.Process.Start(p);//调用外部程序

            }

            return 0;

        }

        /// <summary>
        /// 关闭软件
        /// </summary>
        private void CloseSw()
        {
            //这个是判断，关闭
            //获得任务管理器中的所有进程
            Process[] p = Process.GetProcesses();
            foreach (Process p1 in p)
            {
                try
                {
                    string processName = p1.ProcessName.ToLower().Trim();
                    //判断是否包含阻碍更新的进程
                    if (processName == "胶水检测")
                        p1.Kill();
                }
                catch { }
            }
        }

    }

}
