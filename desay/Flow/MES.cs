using Motion.Enginee;
using System;
using System.Collections.Generic;
using Motion.Interfaces;
using System.Toolkit;
using System.Device;
using System.Threading;
using desay.ProductData;
using System.IO;
using System.Windows;

namespace desay.Flow
{
    public class MES : StationPart
    {
        private PlateformAlarm m_Alarm;
        /// <summary>AA通讯服务</summary>
        public AsynTcpServer aaServer { get; set; }
        /// <summary>自动扫码枪</summary>
        public DM50S AutoScanner { get; set; }
        /// <summary>手动扫码枪</summary>
        public HoneyWell1900 SnScanner { get; set; }
        /// <summary>测高模块</summary>
        public Panasonic HeightDectector { get; set; }
        /// <summary>Mes的AA数据</summary>
        public MesData.AAData MesAAData = new MesData.AAData();
        /// <summary>frmMES对象</summary>
        public frmMES FrmMES;


        public Thread threadDealMsg = null;
        IAsyncResult SNResult;
        IAsyncResult FNResult;
        public string StartTime = null;
        public string EndTime = null;
        bool RequestLocation = false;
        bool RequestResult = false;
        public int FNScanTimes = 0;

        #region 通讯处理
        private void DealMsg()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(100);

                    #region 扫码                
                    if (FNResult != null && FNResult.IsCompleted && Marking.BeginTriggerFN)
                    {
                        Marking.FN = AutoScanner.ReceiveString;
                        //if (Marking.FN == null || Marking.FN == "" || Marking.FN.Length <= 5 || Marking.FN.Contains("Error") /*|| !Marking.FN.Substring(0, 4).Equals("0X06")*/ )
                        if (!((IList<string>)Config.Instance.FNLibrary).Contains(Marking.FN.Substring(0, 9)))
                        {
                            if (FNScanTimes > 10)
                            {
                                AppendText("治具码异常:" + Marking.FN);
                                Marking.BeginTriggerFN = false;
                            }
                            else
                            {
                                Marking.BeginTriggerFN = true;
                                SendRequestMsg(2);
                                FNScanTimes++;
                            }
                        }
                        else
                        {
                            FNScanTimes = 0;
                            Marking.BeginTriggerFN = false;
                            AppendText("读到的治具码为:" + Marking.FN);
                            StartTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff");
                            lock (MesData.CarrierDataLock)
                            {
                                MesData.carrierData.SN = Marking.SN;
                                MesData.carrierData.FN = Marking.FN;
                                MesData.carrierData.StartTime = StartTime;
                            }
                            StartTime = null;
                            Marking.GetFNFlg = true;
                        }
                        AutoScanner.ReceiveString = null;
                    }
                    if (SnScanner.StringReceived && !Marking.SnScannerShield)
                    {
                        Marking.BeginTriggerSN = false;
                        SnScanner.StringReceived = false;
                        Marking.SN = SnScanner.ReceiveString;
                        AppendText("读到的产品码为:" + Marking.SN);
                        Marking.GetSNFlg = true;
                    }
                    #endregion

                    #region 测高
                    if (HeightDectector.StringReceived)
                    {
                        HeightDectector.StringReceived = false;
                        Marking.RequestHeightError = false;
                        if (HeightDectector.DealRecvData(HeightDectector.ReceiveString, ref Marking.DetectHeight) != "数据接收正常!")
                        {
                            Marking.RequestHeightError = true;
                        }
                        AppendText("解析到当前测高高度为:" + Marking.DetectHeight);
                        //这里后续需要判断是否测高模块反馈的数据是否正常，先打印log显示
                        Position.Instance.DetectHeight2BaseHeight = Marking.DetectHeight - Position.Instance.GlueBaseHeight;
                        Marking.GetHeightFlg = true;
                    }
                    #endregion

                    #region AA
                    if (aaServer.IsResultTCP)
                    {
                        aaServer.IsResultTCP = false;
                        //aaServer.strResultTCP = aaServer.strResultTCP.Replace("\r", "");
                        //aaServer.strResultTCP = aaServer.strResultTCP.Replace("\n", "");
                        if (aaServer.strResultTCP == null)
                            m_Alarm = PlateformAlarm.接收到AA字符为空;
                        else
                        {
                            AppendText("收到AA字符:" + aaServer.strResultTCP);
                            if (aaServer.strResultTCP.Contains("$HAP01")) //HAP01是点胶工位这边通知AA工位可以把治具流过来
                            {
                                Marking.AACallIn = true;
                                aaServer.strResultTCP = null;
                            }
                            else if (aaServer.strResultTCP.Contains("$HAA02")) //HAA02是TCP连接上的信号
                            {
                                aaServer.strResultTCP = null;
                                Marking.AaClientOpenFlg = true;
                                Marking.AaClientCloseFlg = false;
                            }
                            else if (aaServer.strResultTCP.Contains("$HAA99"))//HAA99是TCP断开的信号
                            {
                                aaServer.strResultTCP = null;
                                Marking.AaClientOpenFlg = false;
                                Marking.AaClientCloseFlg = true;

                            }
                            else if (aaServer.strResultTCP.Contains("$HAR")) //HAR是AA结果
                            {
                                MesAAData.aaResult = true;
                                MesAAData.haveLensRst = true;
                                MesAAData.whiteLightRst = true;
                                MesAAData.whiteBoardRst = true;
                                MesAAData.gluePosRst = true;
                                MesAAData.glueCheckRst = true;
                                MesAAData.lightCameraRst = true;
                                MesAAData.preAAPosRst = true;
                                MesAAData.searchPosRst = true;
                                MesAAData.ocAdjustRst = true;
                                MesAAData.tiltAdjustRst = true;
                                MesAAData.uvAfterRst = true;
                                MesAAData.uvBeforeRst = true;
                                if (aaServer.strResultTCP.Contains("$HAR01")) //HAR01代表AA OK
                                {
                                    MesAAData.aaResult = true;
                                    MesAAData.ResultCode = "00";
                                    Config.Instance.AAProductOkTotal++;
                                }
                                if (aaServer.strResultTCP.Contains("$HAR10")) //HAR10代表AA NG
                                {
                                    MesAAData.aaResult = false;
                                    Config.Instance.AAProductNgTotal++;
                                    if (aaServer.strResultTCP.Contains("NG10"))
                                    {
                                        MesAAData.uvAfterRst = false;
                                        MesAAData.ResultCode = "12";
                                    }
                                    else if (aaServer.strResultTCP.Contains("NG1"))
                                    {
                                        MesAAData.haveLensRst = false;
                                        MesAAData.ResultCode = "01";
                                    }
                                    else if (aaServer.strResultTCP.Contains("NG2_2"))
                                    {
                                        MesAAData.whiteBoardRst = false;
                                        MesAAData.ResultCode = "03";
                                    }
                                    else if (aaServer.strResultTCP.Contains("NG2"))
                                    {
                                        MesAAData.whiteLightRst = false;
                                        MesAAData.ResultCode = "02";
                                    }
                                    else if (aaServer.strResultTCP.Contains("NG3_2"))
                                    {
                                        MesAAData.glueCheckRst = false;
                                        MesAAData.ResultCode = "05";
                                    }
                                    else if (aaServer.strResultTCP.Contains("NG3"))
                                    {
                                        MesAAData.gluePosRst = false;
                                        MesAAData.ResultCode = "04";
                                    }
                                   
                                    else if (aaServer.strResultTCP.Contains("NG4"))
                                    {
                                        MesAAData.lightCameraRst = false;
                                        MesAAData.ResultCode = "06";
                                    }
                                    else if (aaServer.strResultTCP.Contains("NG5"))
                                    {
                                        MesAAData.preAAPosRst = false;
                                        MesAAData.ResultCode = "07";
                                    }
                                    else if (aaServer.strResultTCP.Contains("NG6"))
                                    {
                                        MesAAData.searchPosRst = false;
                                        MesAAData.ResultCode = "08";
                                    }
                                    else if (aaServer.strResultTCP.Contains("NG7"))
                                    {
                                        MesAAData.ocAdjustRst = false;
                                        MesAAData.ResultCode = "09";
                                    }
                                    else if (aaServer.strResultTCP.Contains("NG8"))
                                    {
                                        MesAAData.tiltAdjustRst = false;
                                        MesAAData.ResultCode = "10";
                                    }
                                    else if (aaServer.strResultTCP.Contains("NG9"))
                                    {
                                        MesAAData.uvBeforeRst = false;
                                        MesAAData.ResultCode = "11";
                                    }
                                }

                                string fn = aaServer.strResultTCP.Substring(aaServer.strResultTCP.LastIndexOf('*') + 1);
                                lock (MesData.AADataLock)
                                {
                                    MesData.NeedShowFN = fn;
                                    if (MesData.ResultList.ContainsKey(fn))
                                    {
                                        MesData.ResultList.Remove(fn);
                                    }
                                    MesData.ResultList.Add(fn, MesAAData);
                                    MesData.AADataList.Add(MesAAData);
                                }
                                Marking.AaGetResultFlg = true;
                                aaServer.strResultTCP = null;
                            }
                            if (aaServer.strResultTCP != null && aaServer.strResultTCP.Contains("$HAD"))
                            {
                                string[] temp = aaServer.strResultTCP.Split('$');
                                Marking.AAData = temp[temp.Length - 1].Substring(6);
                                //整理并上传
                                Marking.AaGetDataFlg = true;
                                aaServer.strResultTCP = null;
                                EndTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff");
                            }
                        }
                    }
                    #endregion
                }
                catch
                {
                    AppendText("通讯处理异常！");
                }
            }
        }
        #endregion

        public MES(External ExternalSign, StationInitialize stationIni, StationOperate stationOpe)
                        : base(ExternalSign, stationIni, stationOpe, typeof(MES))
        {
            threadDealMsg = new Thread(DealMsg);
            threadDealMsg.IsBackground = true;
        }
        public override void Running(RunningModes runningMode)
        {
            int count = 0;
            while (true)
            {
                Thread.Sleep(10);
                #region 自动流程
                if (stationOperate.Running)
                {
                    switch (step)
                    {
                        case 0://控制流程结束，接收控制流程数据信息
                            if (Marking.AaGetDataFlg)
                            {
                                Marking.AaGetDataFlg = false;
                                WriteFile();
                                step = 100;
                            }
                            break;
                        case 10:
                            break;
                        case 20://
                            break;
                        case 30://
                            break;
                        case 100://复位各标志位
                            step = 0;
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
                    Thread.Sleep(1000);
                    switch (stationInitialize.Flow)
                    {

                        case 0://清除所有标志位
                            stationInitialize.InitializeDone = false;
                            stationOperate.RunningSign = false;
                            step = 0;
                            stationInitialize.Flow = 10;
                            break;
                        case 10://检查MES连接
                            Thread.Sleep(200);
                            stationInitialize.Flow = 20;
                            break;
                        case 20://检查CCD连接
                            //if (Marking.CCDShield)
                            {
                                Marking.GluePosResult = false;
                                Marking.GlueCheckResult = false;
                                stationInitialize.Flow = 30;
                            }
                            break;
                        case 30://检查白板连接
                            //if (Marking.WhiteShield || Marking.CleanShield)
                            {
                                Marking.WbData = null;
                                Marking.WhiteBoardResult = false;
                                Marking.WhiteLightResult = false;
                                Marking.WbRequestResultFlg = false;
                                Marking.WbGetResultFlg = false;
                                AppendText("白板复位完成！");
                                stationInitialize.Flow = 40;
                            }
                            break;
                        case 40://检查AA连接
                            if (aaServer._status || Marking.AAShield)
                            {
                                Marking.AaAllowPassFlg = false;
                                Marking.AACallIn = false;
                                Marking.AAData = null;
                                Marking.AaGetDataFlg = false;
                                Marking.AaGetResultFlg = false;
                                Marking.AAResult = false;
                                Marking.AaSendCodeFlg = false;
                                AppendText("AA复位完成！");
                                stationInitialize.Flow = 50;
                            }
                            break;
                        case 50://检查自动扫码枪连接
                            if (AutoScanner.IsOpen || !Marking.ScannerEnable)
                            {
                                Marking.BeginTriggerSN = false;
                                Marking.GetSNFlg = false;
                                Marking.BeginTriggerFN = false;
                                Marking.GetFNFlg = false;
                                AppendText("治具码扫码枪复位完成！");

                                stationInitialize.Flow = 60;
                            }
                            break;
                        case 60://检查手动扫码枪连接
                                //if (SnScanner.IsOpen)
                                //{
                            AppendText("产品码扫码枪复位完成！");
                            stationInitialize.Flow = 70;
                            //}
                            break;
                        case 70:
                            if (HeightDectector.IsOpen)
                            {
                                Marking.RequestHeightFlg = false;
                                Marking.GetHeightFlg = false;
                                AppendText("测高模块复位完成！");
                                stationInitialize.Flow = 90;
                            }
                            break;
                        case 90://复位完成，置位初始化标志
                            lock (MesData.MesDataLock)
                            {
                                MesData.MesDataList.Clear();
                            }
                            lock (MesData.AADataLock)
                            {
                                if (MesData.ResultList.Count > 0)
                                    MesData.ResultList.Clear();
                                if (MesData.AADataList.Count > 0)
                                    MesData.AADataList.Clear();
                            }
                            Thread.Sleep(200);
                            stationInitialize.InitializeDone = true;
                            AppendText($"{Name}初始化完成！");
                            stationInitialize.Flow = 100;
                            break;
                        default:
                            break;
                    }
                }
                #endregion
                if (AlarmReset.AlarmReset)
                {
                    m_Alarm = PlateformAlarm.无消息;
                }
            }
        }

        protected override IList<Alarm> alarms()
        {
            var list = new List<Alarm>();
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.初始化故障)
            {
                AlarmLevel = AlarmLevels.None,
                Name = PlateformAlarm.初始化故障.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.MES通讯超时)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.MES通讯超时.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.接收到AA字符为空)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.接收到AA字符为空.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.入料请求发送失败)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.入料请求发送失败.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.写入MES数据文件失败)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.写入MES数据文件失败.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.结果发送失败)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.结果发送失败.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.未找到当前治具码数据)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.未找到当前治具码数据.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.AA返回的结果数据缺失)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.AA返回的结果数据缺失.ToString()
            });
            list.Add(new Alarm(() => m_Alarm == PlateformAlarm.AA返回结果码数据缺失)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = PlateformAlarm.AA返回结果码数据缺失.ToString()
            });

            return list;
        }

        protected override IList<ICylinderStatusJugger> cylinderStatus()
        {
            var list = new List<ICylinderStatusJugger>();
            return list;
        }

        public enum PlateformAlarm : int
        {
            无消息,
            初始化故障,
            MES通讯超时,
            MES通讯失败次数超限度,
            MES通讯连接失败,
            入料请求发送失败,
            白板发送失败,
            接收到错误字符,
            CCD发送失败,
            结果发送失败,
            接收到AA字符为空,
            AA返回的结果数据缺失,
            AA返回结果码数据缺失,
            未找到当前治具码数据,
            写入MES数据文件失败,
        }

        public void SendRequestMsg(int type)
        {
            #region 测高
            if (type == 1)
            {
                if (Marking.RequestHeightFlg)
                {
                    AppendText("请求测高数据！");
                    Marking.RequestHeightFlg = false;
                    Marking.GetHeightFlg = false;
                    HeightDectector.WriteDetectHeightCommand();
                }
            }
            #endregion

            #region 扫码
            if (type == 2)
            {
                if (Marking.BeginTriggerSN)//后台持续读取
                {
                    AppendText("触发扫产品码！");
                }
                if (Marking.BeginTriggerFN)//指令触发读取
                {
                    AppendText("触发扫治具码！");
                    FNResult = AutoScanner.BeginTrigger(new TriggerArgs() { sender = this, tryTimes = 3, message = "scanFN\r\n" });
                }
            }
            #endregion

            #region AA
            if (type == 5)
            {
                if (Marking.AaAllowPassFlg && Marking.AaClientOpenFlg)
                {
                    AppendText("允许AA放行治具！");
                    Marking.AaAllowPassFlg = false;
                    try
                    {
                        lock (MesData.AALock)
                        {
                            aaServer.AsynSend(string.Format("$AHB01\r\n"));
                            Thread.Sleep(300);
                        }
                    }
                    catch (Exception ex)
                    {
                        m_Alarm = PlateformAlarm.入料请求发送失败;
                    }
                }
            }
            if (type == 6)
            {
                if (Marking.AaSendCodeFlg && Marking.AaClientOpenFlg)
                {
                    Marking.AaSendCodeFlg = false;
                    try
                    {
                        string strMsg = null;
                        //Marking.FN = "1111";
                        //aaServer.AsynSend(string.Format("$AHVSN{0}#\r\n", Marking.FN));
                        lock (MesData.GlueDataLock)
                        {
                            if (MesData.glueData.cleanData.carrierData.FN.Length > 5)
                            {
                                //if (Marking.GlueShield) Marking.GlueResult = true;//这里在同致的基础上增加  陶工 NG1 NG2问题
                                //strMsg = (!MesData.glueData.cleanData.CleanResult ||(!Marking.GlueResult && Marking.GlueShield && !Marking.CCDShield)) ? "10" : "01";                         
                                //if (MesData.glueData.cleanData.HaveLens.Equals("NG") && !Marking.HaveLensShield && !Marking.CleanShield)//暂时不启用监测镜头
                                //    strMsg += "NG1";
                                //else if (MesData.glueData.cleanData.WbResult.Equals("NG") && !Marking.WhiteShield && !Marking.CleanShield)
                                //    strMsg += "NG2";
                                //else if (!Marking.GlueResult && !Marking.GlueShield && !Marking.CCDShield)
                                //    strMsg += "NG3";
                                //xmz  全OK 01  其余10   无镜头NG1  白板点亮测试NG NG2   点胶NG  NG3
                                
                                //辉创点检增加
                                //白板点检OK：AHR01_CheckWB
                                //白板点检NG：AHR10_CheckWB
                                //胶水检测点检OK：AHR01_CheckGlue
                                //胶水检测点检NG：AHR10_CheckGlue
                                //MTF点检OK：AHR01_CheckMTF
                                //MTF点检NG：AHR10_CheckMTF
                                //胶重点检OK：AHR01_CheckWeight
                                //胶重点检NG：AHR10_CheckWeight
                                

                                if (Marking.GlueShield)
                                {
                                    if (!Marking.DryRun)
                                    {
                                        Marking.GluePosResult = true;    //不考虑相机
                                        Marking.GlueCheckResult = true;
                                    }
                                    else
                                    {
                                        Marking.GluePosResult = true;
                                        Marking.GlueCheckResult = false;
                                    }

                                }
                                strMsg = (MesData.glueData.cleanData.HaveLens.Equals("OK")        //有无镜头
                                            && MesData.glueData.cleanData.CleanResult             //清洗结果
                                            && MesData.glueData.cleanData.WbLResult.Equals("OK")  //白板点亮结果
                                            && MesData.glueData.cleanData.WbResult.Equals("OK")   //白板检测结果 
                                            && Marking.GluePosResult                                 //点胶定位结果
                                            && Marking.GlueCheckResult) ? "01" : "10";                 //点胶识别结果

                                log.Debug($"有无镜头{MesData.glueData.cleanData.HaveLens}");
                                log.Debug($"清洗工位结果{MesData.glueData.cleanData.CleanResult.ToString()}");
                                log.Debug($"白板点亮测试结果{MesData.glueData.cleanData.WbLResult}");
                                log.Debug($"白板检测测试结果{MesData.glueData.cleanData.WbResult}");
                                log.Debug($"点胶定位结果{Marking.GluePosResult.ToString()}");
                                log.Debug($"点胶识别结果{Marking.GlueCheckResult.ToString()}");
                                if (Marking.GlueShield) log.Debug($"点胶工位屏蔽");
                                if (Marking.CleanShield) log.Debug($"清洗工位屏蔽");
                                if (Marking.WhiteShield) log.Debug($"白板检测屏蔽");

                                if (MesData.glueData.cleanData.HaveLens.Equals("NG"))
                                {
                                    strMsg += "NG1";      //镜头清洗NG
                                }
                                else if (MesData.glueData.cleanData.WbLResult.Equals("NG"))
                                {
                                    strMsg += "NG2";      //白板点亮NG
                                }
                                else if (MesData.glueData.cleanData.WbResult.Equals("NG"))
                                {
                                    strMsg += "NG2_2";    //白板检测NG
                                }
                                else if (!Marking.GluePosResult)
                                {
                                    strMsg += "NG3";      //点胶定位NG
                                }
                                else if (!Marking.GlueCheckResult)
                                {
                                    strMsg += "NG3_2";    //点胶识别NG
                                }

                                if (Marking.WhiteMode)     
                                {
                                   strMsg += "_CheckWB";      //白板点检
                                   Marking.WhiteMode = false;
                                }
                                else if (Marking.GlueMode) 
                                {
                                    strMsg += "_CheckGlue";   //胶水点检
                                    Marking.GlueMode = false;
                                }
                                else if (Marking.AAMode)   
                                {
                                    strMsg += "_CheckMTF";    //AA点检
                                    Marking.AAMode = false;
                                }
                                else if (Marking.WeighMode)                      
                                {
                                    strMsg += "_CheckWeight"; //胶重点检
                                    Marking.WeighMode = false;
                                }

                                log.Debug($"发送给AA字符串{strMsg}");

                                if (Marking.GlueCheckResult)
                                    Config.Instance.GlueProductOkTotal++;
                                else
                                    Config.Instance.GlueProductNgTotal++;

                                lock (MesData.AALock)
                                {
                                    //发送产品码SN,治具码FN
                                    aaServer.AsynSend(string.Format("$AHVSN{0},{1}#\r\n", MesData.glueData.cleanData.carrierData.FN, MesData.glueData.cleanData.carrierData.SN));
                                    Thread.Sleep(300);
                                    //发送前段结果
                                    aaServer.AsynSend(string.Format("$AHR{0}\r\n", strMsg));
                                    Thread.Sleep(300);
                                    strMsg += "*" + MesData.glueData.cleanData.carrierData.FN;
                                }
                                AppendText("发送AA治具码及结果数据！" + strMsg);

                            }
                            else
                            {
                                log.Debug($"治具号异常：" + MesData.glueData.cleanData.carrierData.FN);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        m_Alarm = PlateformAlarm.结果发送失败;
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 保存生产数据，上传MES
        /// </summary>
        public void WriteFile()
        {
            if (Marking.SnScannerShield)    //无产品码
            {
                log.Debug(FrmMES.HelloWord());
                return;
            }
            else                            //有产品码
            {
                try
                {
                    log.Debug("开始解析AA数据");
                    string[] temp1 = Marking.AAData.Split(':');
                    if (temp1.Length < 2)
                    {
                        m_Alarm = PlateformAlarm.AA返回的结果数据缺失;
                        return;
                    }
                    string[] code = temp1[0].Split(',', ';');
                    if (code.Length < 2)
                    {
                        m_Alarm = PlateformAlarm.AA返回结果码数据缺失;
                        return;
                    }
                    //治具码
                    string fn = code[0];
                    log.Debug("从AA数据中获取治具码:" + fn);
                    MesData.GlueData data = new MesData.GlueData();
                    lock (MesData.MesDataLock)
                    {
                        if (!MesData.MesDataList.ContainsKey(fn))
                        {
                            m_Alarm = PlateformAlarm.未找到当前治具码数据;
                            return;
                        }
                        data = MesData.MesDataList[fn];
                    }

                    //产品码
                    string sn = data.cleanData.carrierData.SN;
                    Position.Instance.pchTestIdK = sn;
                    log.Debug("从AA数据中获取产品码:" + sn);

                    //整合数据
                    string ResultCode = MesData.ResultList[fn].ResultCode; //AA结果代码
                    Position.Instance.pchErrcdk = ResultCode;
                    log.Debug("从AA数据中获取结果:" + ResultCode);

                    string ResultItems = string.Empty;                     //AA数据内容


                    log.Debug("MES数据组合完成");
                    //检查数据
                    string result = FrmMES.fCanIGoTest(sn);
                    if (result.Trim(' ') == "0")
                    {
                        //数据上传
                        string sendresult = FrmMES.fSendData(ResultCode, ResultItems);
                        if (sendresult.Trim(' ') == "0")
                        {
                            log.Debug("数据上传成功！");
                        }
                        else
                        {
                            log.Debug("数据上传失败！");
                            MessageBox.Show("数据上传失败！", "提示");
                        }
                    }
                }
                catch
                {

                }
            }


            #region 注释代码

            //try
            //{
            //    log.Debug("开始解析AA数据");
            //    string[] temp1 = Marking.AAData.Split(':');
            //    if (temp1.Length < 2)
            //    {
            //        m_Alarm = PlateformAlarm.AA返回的结果数据缺失;
            //        return;
            //    }
            //    string[] code = temp1[0].Split(',', ';');
            //    if (code.Length < 2)
            //    {
            //        m_Alarm = PlateformAlarm.AA返回结果码数据缺失;
            //        return;
            //    }
            //    string fn = code[0];
            //    log.Debug("从AA数据中获取治具码");
            //    MesData.GlueData data = new MesData.GlueData();
            //    lock (MesData.MesDataLock)
            //    {
            //        if (!MesData.MesDataList.ContainsKey(fn))
            //        {
            //            m_Alarm = PlateformAlarm.未找到当前治具码数据;
            //            return;
            //        }
            //        data = MesData.MesDataList[fn];
            //    }
            //    log.Debug("根据治具码获取对应的数据信息");
            //    if (data.cleanData.carrierData.SN == null)
            //    {

            //        data.cleanData.carrierData.SN = "12345678";
            //    }
            //    string FileName = AppConfig.MesShareFileFolderName + data.cleanData.carrierData.SN.Trim() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            //    string Header = "\"" + data.cleanData.carrierData.SN.Trim() + "\";\"" + data.cleanData.carrierData.StartTime.Trim() + "\";\"" + EndTime.Trim() + "\";\""
            //        + (MesAAData.aaResult ? "OK" : "NG") + "\""/* + "\r\n"*/;
            //    string Body = $"\"{Config.Instance.MesWorkNum}\";\"HaveLens\";;;\"" + data.cleanData.HaveLens.Trim() + "\";;\"T\";\"" + data.cleanData.HaveLens.Trim() + "\";\r\n";

            //    if (!data.cleanData.HaveLens.Trim().Contains("NG") && !data.cleanData.WbData.Trim().Contains("NG"))
            //    {
            //        Body += $"\"{Config.Instance.MesWorkNum}\";\"GlueCheck\";;;\"" + data.GlueResult.Trim() + "\";;\"T\";\"" + data.GlueResult.Trim() + "\";\r\n"
            //                + $"\"{Config.Instance.MesWorkNum}\";\"LightCamera\";;;\"" + (MesAAData.lightCameraRst ? "OK" : "NG") + "\";;\"T\";\"" + (MesAAData.lightCameraRst ? "OK" : "NG") + "\";\r\n"
            //                + $"\"{Config.Instance.MesWorkNum}\";\"PreAAPos\";;;\"" + (MesAAData.preAAPosRst ? "OK" : "NG") + "\";;\"T\";\"" + (MesAAData.preAAPosRst ? "OK" : "NG") + "\";\r\n"
            //                + $"\"{Config.Instance.MesWorkNum}\";\"SearchPos\";;;\"" + (MesAAData.searchPosRst ? "OK" : "NG") + "\";;\"T\";\"" + (MesAAData.searchPosRst ? "OK" : "NG") + "\";\r\n"
            //                + $"\"{Config.Instance.MesWorkNum}\";\"OCAdjust\";;;\"" + (MesAAData.ocAdjustRst ? "OK" : "NG") + "\";;\"T\";\"" + (MesAAData.ocAdjustRst ? "OK" : "NG") + "\";\r\n"
            //                + $"\"{Config.Instance.MesWorkNum}\";\"Tiltadjust\";;;\"" + (MesAAData.tiltAdjustRst ? "OK" : "NG") + "\";;\"T\";\"" + (MesAAData.tiltAdjustRst ? "OK" : "NG") + "\";\r\n"
            //                + $"\"{Config.Instance.MesWorkNum}\";\"FN\";;;\"" + (data.cleanData.carrierData.FN) + "\";;\"T\";\"" + ("OK") + "\";\r\n";
            //    }
            //    for (int i = 1; i < temp1.Length - 1; i++)
            //    {
            //        Body += temp1[i];
            //    }
            //    string Footer = "##\r\n";
            //    log.Debug("MES数据组合完成");
            //    lock (MesData.MesDataLock)
            //    {
            //        MesData.MesDataList.Remove(fn);
            //    }
            //    if (!Marking.SnScannerShield)
            //    {
            //        FileStream fs = new FileStream(FileName.Trim(), FileMode.Create);
            //        StreamWriter sw = new StreamWriter(fs);
            //        //写入数据
            //        sw.WriteLine(Header);
            //        sw.WriteLine(Body + Footer);
            //        //sw.WriteLine(Footer);
            //        //清空缓冲区
            //        sw.Flush();
            //        //关闭流
            //        sw.Close();
            //        fs.Close();
            //        log.Debug("MES数据写入完成！");
            //    }
            //}
            //catch (Exception e)
            //{
            //    log.Error(e.Message);
            //    log.Error(e.StackTrace);
            //    m_Alarm = PlateformAlarm.写入MES数据文件失败;
            //}

            #endregion

        }
    }
}
