using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using log4net;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using License;
using Motion.Enginee;
using Motion.Interfaces;
using desay.ProductData;
using desay.Flow;
using System.Toolkit;
using System.Toolkit.Helpers;
using System.Text;
using Motion.AdlinkAps;
using desay.View;

namespace desay
{
    public partial class frmMain : Form
    {
        #region field  
        public static frmMain main;
        private AlarmType LplateformIsAlarm, RplateformIsAlarm, CarrierIsAlarm, MachineIsAlarm, MesIsAlarm;
        private External m_External = new External();
        private MachineOperate MachineOperation;
        private GluePlateform m_GluePlateform;
        private CleanPlateform m_CleanPlateform;
        private Carrier m_Carrier;
        public frmWb Wb;
        public frmTeach frmt;
        private frmAAVision m_AAVision;
        //private frmWhiteBorad m_WB;
        public MES m_Mes;

        Encryption hasp;
        Registered haspRegistered;

        private EventButton StartButton1, StartButton2, EstopButton, EstopButton2, StopButton, PauseButton, ResetButton;
        private LayerLight layerLight;
        private bool ManualAutoMode;
        Thread threadMachineRun = null;
        Thread threadAlarmCheck = null;
        Thread threadStatusCheck = null;
        Thread threadLicenseCheck = null;

        static ILog log = LogManager.GetLogger(typeof(frmMain));

        public bool uploadEnable = false;
        event Action<string> LoadingMessage;
        event Action<UserLevel> UserLevelChangeEvent;
        event Action StopEvent;

        AsynTcpServer scannerServer;
        AsynTcpServer aaServer;

        int faultcount;
        Global.Fault fault = new Global.Fault();

        public bool AutoNeedleStatus;//自动对针状态记忆
        public bool AutoNeedleStatusRun;//自动对针状态忆
        public int NeedleStep;
        IntPtr DlgHandle_wb;
        public frmAAVision aa;
        //public frmWhiteBorad wb1;

        #endregion
        
        #region Constructor

        public frmMain()
        {
            InitializeComponent();
            main = this;
            hasp = new License.Encryption();
            hasp.ShowWindows += ShowWindows;
            hasp.MachineName = "0X06";
            haspRegistered = new Registered(hasp);
            //LogInfor = new frmLog()
        }
        private void ShowWindows(object sender, System.EventArgs e)
        {
            if (!haspRegistered.Created)
            {
                haspRegistered.ShowDialog();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        #endregion

        #region 用户权限

        void UserLevelChange(UserLevel level)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<UserLevel>(UserLevelChange), level);
            }
            else
            {
                switch (level)
                {
                    case UserLevel.操作员:
                        btnTeach.Enabled = false;
                        btnSetting.Enabled = false;
                        break;
                    case UserLevel.工程师:
                        btnTeach.Enabled = true;
                        btnSetting.Enabled = true;
                        break;
                    default:
                        btnTeach.Enabled = false;
                        btnSetting.Enabled = false;
                        break;
                }
            }
        }
        public void OnUserLevelChange(UserLevel level)
        {
            UserLevelChangeEvent?.Invoke(level);
        }

        #endregion

        #region 停止事件
        void StopStatus()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(StopStatus));
            }
            else
            {
                ManualAutoMode = false;//cdm加1010
                btnManualAuto.Text = "手动模式";
                btnManualAuto.ForeColor = Color.Red;
            }
        }
        public void OnStop()
        {
            StopEvent?.Invoke();
        }
        #endregion

        #region  窗体加载
        public void LoadagainWb()
        {
            m_CleanPlateform.WbINItrans(Config.Instance.CurrentProductType);
            Wb.Visible = false;
            CallWb.ExitAAImageDlg();
            Wb.frmWb_Load(this, null);
            Wb.Visible = true;
            Wb.ShowWb(this, null);
            MessageBox.Show($"已重新加载白板请注意白板界面是否显示正常,若新增型号请配置白板路径，若不正常请关闭软件并重开");
        }
       
        private void frmMain_Load(object sender, EventArgs e)
        {
            UserLevelChangeEvent += UserLevelChange;
            StopEvent += new Action(StopStatus);
            Config.Instance.userLevel = UserLevel.操作员;


            //调试方便暂时开放权限以及设置屏蔽
            Config.Instance.userLevel = UserLevel.工程师;
            Marking.AAShield = true;
            Marking.CurtainShield = true;

            OnUserLevelChange(Config.Instance.userLevel);
            new Thread(new ThreadStart(() =>
            {
                frmStarting loading = new frmStarting(8);
                //利用消息控制loading进度条
                LoadingMessage += new Action<string>(loading.ShowMessage);
                loading.ShowDialog();
            })).Start();
            Thread.Sleep(500);

            #region  加载板卡

            LoadingMessage("加载板卡信息");
            try
            {
                IoPoints.m_ApsController.Initialize();
                IoPoints.m_ApsController.LoadParamFromFile(AppConfig.ConfigIniCardfile);
                IoPoints.m_DaskController.Initialize();
            }
            catch (Exception ex) { log.Error($"{ex}"); }
            #endregion

            #region 伺服轴信息加载

            LoadingMessage("加载轴控制资源");
            var Lxaxis = new ServoAxis(IoPoints.m_ApsController)
            {
                NoId = 0,
                Name = "清洗X轴",
                Transmission = AxisParameter.Instance.LXTransParams
            };
            var Lyaxis = new ServoAxis(IoPoints.m_ApsController)
            {
                NoId = 1,
                Name = "清洗Y轴",
                Transmission = AxisParameter.Instance.LYTransParams
            };
            var Lzaxis = new ServoAxis(IoPoints.m_ApsController)
            {
                NoId = 2,
                Name = "清洗Z轴",
                Transmission = AxisParameter.Instance.LZTransParams
            };

            var Rxaxis = new ServoAxis(IoPoints.m_ApsController)
            {
                NoId = 4,
                Name = "点胶X轴",
                Transmission = AxisParameter.Instance.RXTransParams
            };
            var Ryaxis = new ServoAxis(IoPoints.m_ApsController)
            {
                NoId = 5,
                Name = "点胶Y轴",
                Transmission = AxisParameter.Instance.RYTransParams
            };
            var Rzaxis = new ServoAxis(IoPoints.m_ApsController)
            {
                NoId = 6,
                Name = "点胶Z轴",
                Transmission = AxisParameter.Instance.RZTransParams
            };

            #endregion

            #region 设置轴回零参数
            try
            {
                Lxaxis.SetAxisHomeParam(new HomeParams(0, 1, 0, 10000, 5000, 0));
                Lyaxis.SetAxisHomeParam(new HomeParams(0, 1, 0, 10000, 5000, 0));
                Lzaxis.SetAxisHomeParam(new HomeParams(1, 1, 1, 10000, 5000, 0));
                Rxaxis.SetAxisHomeParam(new HomeParams(0, 1, 0, 10000, 5000, 0));
                Ryaxis.SetAxisHomeParam(new HomeParams(0, 1, 0, 10000, 5000, 0));
                Rzaxis.SetAxisHomeParam(new HomeParams(0, 1, 0, 10000, 5000, 0));
            }
            catch (Exception ex) { log.Error($"{ex}"); }
            #endregion

            #region 通讯初始化操作
            OpenScanner();     //扫码枪
            OpenTcpServer();   //创建TCP 服务器，连接 AA，CCD,白板
            OpenDetectHeight(); //测高模块
            OpenPower();//白板电源

            #endregion

            #region 工站模组操作
            LoadingMessage("加载模组操作资源");
            //Loading module operating resources
            var MesInitialize = new StationInitialize(
                () => { return !ManualAutoMode & hasp.LicenseIsOK; },
                () => { return MesIsAlarm.IsAlarm; });
            var MesOperate = new StationOperate(
                () => { return MesInitialize.InitializeDone & hasp.LicenseIsOK; },
                () => { return MesIsAlarm.IsAlarm; });
            var CarrierInitialize = new StationInitialize(
                () => { return !ManualAutoMode & hasp.LicenseIsOK; },
                () => { return CarrierIsAlarm.IsAlarm; });
            var CarrierOperate = new StationOperate(
                () => { return CarrierInitialize.InitializeDone & hasp.LicenseIsOK; },
                () => { return CarrierIsAlarm.IsAlarm; });
            var LplateformInitialize = new StationInitialize(
                () => { return !ManualAutoMode & hasp.LicenseIsOK; },
                () => { return LplateformIsAlarm.IsAlarm; });
            var LplateformOperate = new StationOperate(
                () => { return LplateformInitialize.InitializeDone & hasp.LicenseIsOK; },
                () => { return LplateformIsAlarm.IsAlarm; });
            var RplateformInitialize = new StationInitialize(
                () => { return !ManualAutoMode & hasp.LicenseIsOK; },
                () => { return RplateformIsAlarm.IsAlarm; });
            var RplateformOperate = new StationOperate(
                () => { return RplateformInitialize.InitializeDone & hasp.LicenseIsOK; },
                () => { return RplateformIsAlarm.IsAlarm; });

            #endregion

            #region 气缸信息 
            #region      清洗   
            var cleanStopCylinder = new SingleCylinder(IoPoints.IDI24, IoPoints.IDI24, IoPoints.IDO10)
            {
                Name = "清洗阻挡气缸",
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                    NoMoveShield = true,
                    NoOriginShield = true
                },
                Delay = Delay.Instance.cleanStopCylinderDelay
            };
            var cleanUpCylinder = new SingleCylinder(IoPoints.IDI2, IoPoints.IDI1, IoPoints.IDO11)
            {
                Name = "清洗顶升气缸",
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                    //xmz8.25
                    //NoMoveShield = true,
                    //NoOriginShield = true
                },
                Delay = Delay.Instance.cleanUpCylinderDelay
            };
            var cleanClampCylinder = new SingleCylinder(IoPoints.IDI4, IoPoints.IDI5, IoPoints.IDO12)
            {
                Name = "清洗夹紧气缸",
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                },
                Delay = Delay.Instance.cleanClampCylinderDelay
            };
            var cleanUpDownCylinder = new SingleCylinder(IoPoints.IDI8, IoPoints.IDI9, IoPoints.IDO14)
            {
                Name = "清洗上下气缸",
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                },
                Delay = Delay.Instance.cleanUpDownCylinderDelay
            };
            var cleanRotateCylinder = new SingleCylinder(IoPoints.IDI6, IoPoints.IDI7, IoPoints.IDO13)
            {
                Name = "清洗旋转气缸",
                Condition = new CylinderCondition(() => { return true; }, () => { return cleanUpDownCylinder.OutOriginStatus && cleanClampCylinder.OutMoveStatus; })
                {
                    External = m_External,
                },
                Delay = Delay.Instance.cleanRotateCylinderDelay
            };

            var lightUpDownCylinder = new SingleCylinder(IoPoints.IDI10, IoPoints.IDI11, IoPoints.IDO15)
            {
                Name = "光源上下气缸",
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                },
                Delay = Delay.Instance.lightUpDownCylinderDelay
            };

            #endregion

            #region 点胶
            var glueStopCylinder = new SingleCylinder(IoPoints.IDI24, IoPoints.IDI24, IoPoints.IDO17)
            {
                Name = "点胶阻挡气缸",
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                    NoOriginShield = true,
                    NoMoveShield = true
                },
                Delay = Delay.Instance.glueStopCylinderDelay
            };
            var glueUpCylinder = new SingleCylinder(IoPoints.IDI17, IoPoints.IDI16, IoPoints.IDO18)
            {
                Name = "点胶顶升气缸",
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                    NoOriginShield = true,
                    NoMoveShield = true
                },
                Delay = Delay.Instance.glueUpCylinderDelay
            };

            var moveCylinder = new DoubleCylinder(IoPoints.TDI9, IoPoints.TDI10, IoPoints.IDO2, IoPoints.IDO3)
            {
                Name = "接驳台平移气缸",
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                    //NoOriginShield = true,
                    //NoMoveShield = true
                },
                Delay = Delay.Instance.moveCylinderDelay
            };
            var carrierUpCylinder = new SingleCylinder(IoPoints.TDI12, IoPoints.TDI11, IoPoints.IDO4)
            {
                Name = "接驳台顶升气缸",
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                    NoOriginShield = true,
                    NoMoveShield = true
                },
                Delay = Delay.Instance.carrierUpCylinderDelay
            };
            var carrierClampCylinder = new SingleCylinder(IoPoints.TDI13, IoPoints.TDI14, IoPoints.IDO5)
            {
                Name = "接驳台开夹气缸",
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                    NoOriginShield = true,
                    NoMoveShield = true
                },
                Delay = Delay.Instance.carrierClampCylinderDelay
            };
            //20201110  XiaoW 增
            var carrierPressCylinder = new SingleCylinder(IoPoints.TDI7, IoPoints.TDI15, IoPoints.IDO23)
            {
                Name = "产品下压气缸",
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                    NoOriginShield = true,
                    NoMoveShield = true
                },
                Delay = Delay.Instance.carrierPressCylinderDelay
            };
            var carrierStopCylinder = new SingleCylinder(IoPoints.IDI24, IoPoints.IDI24, IoPoints.IDO21)
            {
                Name = "回流线阻挡气缸",
                Condition = new CylinderCondition(() => { return true; }, () => { return true; })
                {
                    External = m_External,
                    NoOriginShield = true,
                    NoMoveShield = true
                },
                Delay = Delay.Instance.carrierStopCylinderDelay
            };
            #endregion

            #endregion

            #region 模组信息加载、启动
            LoadingMessage("模组信息加载、启动");
            //loading module information
            m_Mes = new MES(m_External, MesInitialize, MesOperate)
            {
                Name = "通讯模块",
                AutoScanner = autoScanner,
                SnScanner = snScanner,
                aaServer = aaServer,
                HeightDectector = heightDetector
            };
            m_Mes.AddPart();
            m_Mes.StationAppendTextReceive += new System.Toolkit.Interfaces.DataReceiveCompleteEventHandler(DealWithReceiveData);
            m_Mes.Run(RunningModes.Online);

            m_Carrier = new Carrier(m_External, CarrierInitialize, CarrierOperate)
            {
                Name = "组装平台",
                MoveCylinder = moveCylinder,
                CarrierUpCylinder = carrierUpCylinder,
                CarrierClampCylinder = carrierClampCylinder,
                CarrierPressCylinder= carrierPressCylinder,   //20201110  XiaoW 增
                CarrierStopCylinder = carrierStopCylinder
            };
            m_Carrier.AddPart();
            m_Carrier.StationAppendTextReceive += new System.Toolkit.Interfaces.DataReceiveCompleteEventHandler(DealWithReceiveData);
            m_Carrier.Run(RunningModes.Online);

            m_CleanPlateform = new CleanPlateform(m_External, LplateformInitialize, LplateformOperate)
            {
                Name = "左平台",
                Xaxis = Lxaxis,
                Yaxis = Lyaxis,
                Zaxis = Lzaxis,
                CleanStopCylinder = cleanStopCylinder,
                CleanUpCylinder = cleanUpCylinder,

                CleanClampCylinder = cleanClampCylinder,
                CleanRotateCylinder = cleanRotateCylinder,
                CleanUpDownCylinder = cleanUpDownCylinder,

                LightUpDownCylinder = lightUpDownCylinder
            };
            m_CleanPlateform.AddPart();
            m_CleanPlateform.StationAppendTextReceive += new System.Toolkit.Interfaces.DataReceiveCompleteEventHandler(DealWithReceiveData);
            m_CleanPlateform.Run(RunningModes.Online);

            m_GluePlateform = new GluePlateform(m_External, RplateformInitialize, RplateformOperate)
            {
                Name = "右平台",
                Xaxis = Rxaxis,
                Yaxis = Ryaxis,
                Zaxis = Rzaxis,
                GlueStopCylinder = glueStopCylinder,
                GlueUpCylinder = glueUpCylinder
            };
            m_GluePlateform.AddPart();
            m_GluePlateform.StationAppendTextReceive += new System.Toolkit.Interfaces.DataReceiveCompleteEventHandler(DealWithReceiveData);
            m_GluePlateform.Run(RunningModes.Online);

            MachineOperation = new MachineOperate(() =>
            {
                return CarrierInitialize.InitializeDone & LplateformInitialize.InitializeDone & RplateformInitialize.InitializeDone
                & MesInitialize.InitializeDone & !Global.IsLocating /*& hasp.LicenseIsOK*/;
            }, () =>
            {
                return MachineIsAlarm.IsAlarm | CarrierIsAlarm.IsAlarm | LplateformIsAlarm.IsAlarm | RplateformIsAlarm.IsAlarm | MesIsAlarm.IsAlarm;
            });
            #endregion
           
            #region 故障代码建立
            AddAlarms();
            int faultCode = 0;
            foreach (var arm in m_Mes.Alarms)
            {
                ConstructErrorCode(arm, ref faultCode);
            }
            foreach (var arm in m_Carrier.Alarms)
            {
                ConstructErrorCode(arm, ref faultCode);
            }
            foreach (var arm in m_CleanPlateform.Alarms)
            {
                ConstructErrorCode(arm, ref faultCode);
            }
            foreach (var arm in m_GluePlateform.Alarms)
            {
                ConstructErrorCode(arm, ref faultCode);
            }
            foreach (var arm in MachineAlarms)
            {
                ConstructErrorCode(arm, ref faultCode);
            }
            #endregion

            #region 加载信号灯资源
            LoadingMessage("加载信号灯资源");
            //StartButton1 = new EventButton(IoPoints.TDI6);
            StartButton2 = new EventButton(IoPoints.TDI0);
            ResetButton = new LightButton(IoPoints.TDI3, IoPoints.IDO27);
            PauseButton = new LightButton(IoPoints.TDI2, IoPoints.IDO26);
            StopButton = new LightButton(IoPoints.TDI1, IoPoints.IDO25);

            EstopButton = new EventButton(IoPoints.TDI4);
            EstopButton2 = new EventButton(IoPoints.TDI5);
            layerLight = new LayerLight(IoPoints.IDO30, IoPoints.IDO29, IoPoints.IDO28, IoPoints.IDO31);

            StartButton2.Pressed += btnStart_MouseDown;
            StartButton2.Released += btnStart_MouseUp;
            PauseButton.Pressed += btnPause_MouseDown;
            PauseButton.Released += btnPause_MouseUp;
            ResetButton.Pressed += btnReset_MouseDown;
            ResetButton.Released += btnReset_MouseUp;
            StopButton.Pressed += btnStop_MouseDown;
            StopButton.Released += btnStop_MouseUp;
            //StopButton.Pressed += btnAlarmClean_MouseDown;
            //StopButton.Released += btnAlarmClean_MouseUp;

            MachineOperation.StartButton = StartButton2;
            MachineOperation.PauseButton = PauseButton;
            MachineOperation.StopButton = StopButton;
            MachineOperation.ResetButton = ResetButton;
            MachineOperation.EstopButton = EstopButton;
            MachineOperation.EstopButton2 = EstopButton2;
            #endregion

            #region 加载白板模板
            m_CleanPlateform.WbINItrans(Config.Instance.CurrentProductType);
            AppendText($"写入白板参数类型名:{Config.Instance.CurrentProductType}");
            #endregion

            #region 加载子窗体
            LoadingMessage("加载子窗体资源");
            AddSubForm();
            #endregion

            #region 加载线程资源
            LoadingMessage("加载线程资源");
            //m_Carrier.AlarmClean += new Action(AlarmClean);
            m_Carrier.SendRequest += new Action<int>(m_Mes.SendRequestMsg);
            m_CleanPlateform.SendRequest += new Action<int>(m_Mes.SendRequestMsg);
            m_GluePlateform.SendRequest += new Action<int>(m_Mes.SendRequestMsg);
            SerialStart();
            #endregion

            IoPoints.IDO19.Value = false;
            timer1.Enabled = true;
            AppendText("数据加载完成");
            //timer2.Enabled = true;
            //Thread.Sleep(500);
        }
        private void ShowWb(object sender, EventArgs e)
        {
            CallWb.ShowAAImageDlg();
            DlgHandle_wb = CallWb.GetAAImageDlgHwnd();
            // IntPtr 
            CallWb.SetWindowPos(DlgHandle_wb, 0, 20, 50, 500, 700, 0);
            CallWb.ShowWindow(DlgHandle_wb, 1);//1为显示，0为隐藏
        }
        public void AlarmClean()
        {
            m_External.AlarmReset = true;
            Thread.Sleep(100);
            m_External.AlarmReset = false;
        }

        private void OpenTcpServer()
        {
            try
            {
                aaServer = new AsynTcpServer(Position.Instance.AAPort, Position.Instance.ServerIP);
                Marking.TcpServerOpenSuccess = true;
            }
            catch (Exception ex)
            {
                Marking.TcpServerOpenSuccess = false;
            }
        }

        private void ConstructErrorCode(Alarm arm, ref int faultCode)
        {
            if (!Global.FaultDictionary.ContainsKey(arm.Name))
            {
                Global.Fault ft = new Global.Fault();
                faultCode++;
                ft.FaultCode = faultCode;
                ft.FaultMessage = arm.Name;
                ft.FaultCount = 0;
                Global.FaultDictionary.Add(arm.Name, ft);
            }
        }

        private List<Alarm> MachineAlarms;
        private void AddAlarms()
        {
            MachineAlarms = new List<Alarm>();

            MachineAlarms.Add(new Alarm(() => !IoPoints.TDI4.Value || !IoPoints.TDI5.Value)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "急停按钮已按下，注意安全！"
            });

            MachineAlarms.Add(new Alarm(() => !IoPoints.TDI5.Value & !Marking.DoorShield)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "安全门已打开,请注意安全！"
            });

            //MachineAlarms.Add(new Alarm(() => !IoPoints.TDI8.Value & !Marking.CurtainShield)
            //{
            //    AlarmLevel = AlarmLevels.Warrning,
            //    Name = "安全光幕已感应！"
            //});

            MachineAlarms.Add(new Alarm(() => Marking.UVAfterAlarm & !Marking.UVAfterRst)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "UV后检测NG！"
            });

            MachineAlarms.Add(new Alarm(() => !Marking.TcpServerOpenSuccess)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "TCP服务器打开失败，请检查IP地址或网线连接！"
            });

            MachineAlarms.Add(new Alarm(() => !Marking.HeightDetectorOpenSuccess)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "测高串口打开失败，请检查通讯设置或线路连接！"
            });

            MachineAlarms.Add(new Alarm(() => !IoPoints.IDI13.Value)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "胶水液位报警，请检查！"
            });

            //MachineAlarms.Add(new Alarm(() => !IoPoints.IDI24.Value)
            //{
            //    AlarmLevel = AlarmLevels.Error,
            //    Name = "未检测到气压信号！"
            //});

            //MachineAlarms.Add(new Alarm(() => !m_Mes.stationInitialize.InitializeDone)
            //{
            //    AlarmLevel = AlarmLevels.Warrning,
            //    Name = "通讯模块未复位！"
            //});

            MachineAlarms.Add(new Alarm(() => !m_Carrier.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "回流线未回原点！"
            });

            MachineAlarms.Add(new Alarm(() => !m_CleanPlateform.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "清洗模组未回原点！"
            });

            MachineAlarms.Add(new Alarm(() => !m_GluePlateform.stationInitialize.InitializeDone)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "点胶模组未回原点！"
            });

            MachineAlarms.Add(new Alarm(() => m_CleanPlateform.Xaxis.IsPEL || m_CleanPlateform.Xaxis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "清洗X轴感应到限位！"
            });

            MachineAlarms.Add(new Alarm(() => m_CleanPlateform.Yaxis.IsPEL || m_CleanPlateform.Yaxis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "清洗Y轴感应到限位！"
            });

            MachineAlarms.Add(new Alarm(() => m_CleanPlateform.Zaxis.IsPEL /*|| m_CleanPlateform.Zaxis.IsMEL*/)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "清洗Z轴感应到限位！"
            });

            MachineAlarms.Add(new Alarm(() => m_GluePlateform.Xaxis.IsPEL || m_GluePlateform.Xaxis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "点胶X轴感应到限位！"
            });

            MachineAlarms.Add(new Alarm(() => m_GluePlateform.Yaxis.IsPEL || m_GluePlateform.Yaxis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "点胶Y轴感应到限位！"
            });

            MachineAlarms.Add(new Alarm(() => m_GluePlateform.Zaxis.IsPEL || m_GluePlateform.Zaxis.IsMEL)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "点胶Z轴感应到限位！"
            });

            //MachineAlarms.Add(new Alarm(() => !Marking.WbClientOpenFlg && !Marking.WhiteShield && !Marking.CleanShield)
            //{
            //    AlarmLevel = AlarmLevels.Warrning,
            //    Name = "白板检测软件未开启，请检查！"
            //});

            MachineAlarms.Add(new Alarm(() => !Marking.AaClientOpenFlg && !Marking.CleanRecycleRun && !Marking.GlueRecycleRun && !Marking.AAShield)
            {
                AlarmLevel = AlarmLevels.Warrning,
                Name = "AA软件未开启，请检查！"
            });

            MachineAlarms.Add(new Alarm(() => !hasp.LicenseIsOK && hasp.Duetime <= 0)
            {
                AlarmLevel = AlarmLevels.Error,
                Name = "该软件为试用软件，现已到期，或加密狗已拔出，请尽快联系厂商！"
            });

            //MachineAlarms.Add(new Alarm(() => !hasp.LicenseIsOK && hasp.Duetime > 0)
            //{
            //    AlarmLevel = AlarmLevels.Error,
            //    Name = "加密狗无法授权，请检查加密狗或联系厂商！"
            //});
        }
   
        #endregion


        /// <summary>
        /// receive flow information displayed to the mainview
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="result">return result</param>
        private void DealWithReceiveData(object sender, string result) => AppendText(result);

        #region 窗体关闭
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ManualAutoMode)
            {
                AppendText("软件无法退出，必须在手动模式才能操作！");
                e.Cancel = true;
            }
            else
            {
                DialogResult result = MessageBox.Show("是否保存配置文件再退出？", "退出", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName, Config.Instance);
                    SerializerManager<AxisParameter>.Instance.Save(AppConfig.ConfigAxisName, AxisParameter.Instance);
                    SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
                    SerializerManager<Delay>.Instance.Save(AppConfig.ConfigDelayName, Delay.Instance);
                    Thread.Sleep(200);
                    log.Debug("配置文件已保存");

                    threadStatusCheck?.Abort();
                    threadMachineRun?.Abort();
                    threadAlarmCheck?.Abort();
                    m_Mes?.threadDealMsg?.Abort();
                    m_GluePlateform.Xaxis.Stop();
                    m_GluePlateform.Yaxis.Stop();
                    m_GluePlateform.Zaxis.Stop();
                    m_CleanPlateform.Xaxis.Stop();
                    m_CleanPlateform.Yaxis.Stop();
                    m_CleanPlateform.Zaxis.Stop();

                    CloseScanner();
                    CloseDetectHeight();
                    ClosePower();

                    MessageBox.Show("软件即将关闭，请注意清洗点胶装置！");

                    Environment.Exit(0);
                }
                else if (result == DialogResult.No)
                {
                    threadStatusCheck?.Abort();
                    threadMachineRun?.Abort();
                    threadAlarmCheck?.Abort();
                    m_Mes?.threadDealMsg?.Abort();
                    //m_GluePlateform.threadRun.Abort();
                    //m_AAVision.m_pThread?.Abort();
                    
                    m_GluePlateform.Xaxis.Stop();
                    m_GluePlateform.Yaxis.Stop();
                    m_GluePlateform.Zaxis.Stop();

                    m_CleanPlateform.Xaxis.Stop();
                    m_CleanPlateform.Yaxis.Stop();
                    m_CleanPlateform.Zaxis.Stop();

                    CloseScanner();
                    CloseDetectHeight();
                    ClosePower();

                    SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName, Config.Instance);
                    log.Debug("配置文件不保存");
                    MessageBox.Show("软件即将关闭，请注意清洗点胶装置！");

                    Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region 窗体切换

        private void btnMain_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tpgMain;
        }

        private void btnTeach_Click(object sender, EventArgs e)
        {
            if (frmt == null || frmt.IsDisposed)
            {
                frmt = new frmTeach(m_GluePlateform, m_CleanPlateform, m_Carrier);
                frmt.SendRequest += new Action<int>(m_Mes.SendRequestMsg);
                frmt.Show();
            }
            if (frmt.WindowState != FormWindowState.Normal)
            {
                frmt.WindowState = FormWindowState.Normal;
            }            
        }

        private void btnShowWindows_Click(object sender, EventArgs e)
        {
            if (!haspRegistered.Created)
            {
                haspRegistered.ShowDialog();
            }
        }
        private void btnParameter_Click(object sender, EventArgs e)
        {
            new frmParameter().ShowDialog();
        }
        private void btnCommSetting_Click(object sender, EventArgs e)
        {
            new frmCommSetting().ShowDialog();
        }
        private void btnCylinderDelay_Click(object sender, EventArgs e)
        {
            new frmCylinderDelay().ShowDialog();
        }


        private void btnIOMonitor_Click(object sender, EventArgs e)
        {
            tbcMain.SelectedTab = tpgIOmonitor;
        }
        private void btnProduct_Click(object sender, EventArgs e)
        {
            new frmRecipe(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\"),
                Config.Instance.CurrentProductType,
                () =>
                {
                    try
                    {
                        Config.Instance.CurrentProductType = frmRecipe.CurrentProductType;
                        Position.Instance = SerializerManager<Position>.Instance.Load(AppConfig.ConfigPositionName);
                    }
                    catch (Exception ex)
                    {
                        AppendText($"加载数据失败！");
                    }
                },
                () =>
                {
                    try
                    {
                        Config.Instance.CurrentProductType = frmRecipe.CurrentProductType;
                        SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
                        Marking.IsProductThrans = true;
                    }
                    catch (Exception ex)
                    {
                        AppendText($"保存数据失败！");
                    }

                }).ShowDialog();
            if (Marking.IsProductThrans)
            {
                Marking.IsProductThrans = false;

                LoadagainWb();
                frmAAVision.ReadParamFromFile();
            }

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmLogin fm = new frmLogin();
            fm.ShowDialog();
            OnUserLevelChange(Config.Instance.userLevel);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddSubForm()
        {
            GenerateForm(new frmIOmonitor(), tpgIOmonitor);
            Wb = new frmWb();
            Wb.frmWb_Load(this, null);
            Wb.Visible = true;
            Wb.ShowWb(this, null);

            aa = new frmAAVision();
            aa.frmAAVision_Load(this, null);
            aa.Visible = true;
            aa.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// 在选项卡中生成窗体
        /// </summary>
        private void GenerateForm(Form frm, TabPage sender)
        {
            //设置窗体没有边框 加入到选项卡中  
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.TopLevel = false;
            frm.Parent = sender;
            frm.ControlBox = false;
            frm.Dock = DockStyle.Fill;
            frm.Show();
        }
        private void GenerateForm2(Form frm)
        {
            //设置窗体没有边框 加入到选项卡中  
            //frm.FormBorderStyle = FormBorderStyle.None;
            //frm.TopLevel = false;
            //frm.Parent = sender;
            //frm.ControlBox = false;
            //frm.Dock = DockStyle.Fill;
            frm.Show();
        }
        #endregion

        #region 线程处理
        private void SerialStart()
        {
            try
            {
                //xumz
                threadMachineRun = new Thread(MachineRun);
                threadMachineRun.IsBackground = true;
                threadMachineRun.Start();
                if (threadMachineRun.IsAlive) AppendText("设备操作线程运行中...");

                threadAlarmCheck = new Thread(AlarmCheck);
                threadAlarmCheck.IsBackground = true;
                threadAlarmCheck.Start();
                if (threadAlarmCheck.IsAlive) AppendText("故障检查线程运行中...");

                threadStatusCheck = new Thread(StatusCheck);
                threadStatusCheck.IsBackground = true;
                threadStatusCheck.Start();
                if (threadStatusCheck.IsAlive) AppendText("状态检查线程运行中...");

                threadLicenseCheck = new Thread(LicenseCheck);
                threadLicenseCheck.IsBackground = true;
                threadLicenseCheck.Start();

                m_Mes?.threadDealMsg.Start();
                //m_GluePlateform.threadRun.Start();
            }
            catch (Exception ex)
            {
                AppendText("Server start Error: " + ex.Message);
            }
        }

        private void MachineRun()
        {
            var watchCT = new Stopwatch();
            watchCT.Start();
            while (true)
            {
                Thread.Sleep(50);
                m_External.AirSignal = true;
                m_External.ManualAutoMode = ManualAutoMode;

                layerLight.VoiceClosed = Marking.VoiceClosed;
                layerLight.Status = MachineOperation.Status;
                layerLight.Refreshing();

                m_GluePlateform.stationOperate.ManualAutoMode = ManualAutoMode;
                m_GluePlateform.stationOperate.AutoRun = MachineOperation.Running;
                m_GluePlateform.stationInitialize.Run();
                m_GluePlateform.stationOperate.Run();

                m_CleanPlateform.stationOperate.ManualAutoMode = ManualAutoMode;
                m_CleanPlateform.stationOperate.AutoRun = MachineOperation.Running;
                m_CleanPlateform.stationInitialize.Run();
                m_CleanPlateform.stationOperate.Run();

                m_Carrier.stationOperate.ManualAutoMode = ManualAutoMode;
                m_Carrier.stationOperate.AutoRun = MachineOperation.Running;
                m_Carrier.stationInitialize.Run();
                m_Carrier.stationOperate.Run();

                m_Mes.stationOperate.ManualAutoMode = ManualAutoMode;
                m_Mes.stationOperate.AutoRun = MachineOperation.Running;
                m_Mes.stationInitialize.Run();
                m_Mes.stationOperate.Run();

                MachineOperation.ManualAutoModel = ManualAutoMode;
                MachineOperation.CleanProductDone = Global.CleanProductDone;
                MachineOperation.Run();
                MachineOperation.Stop = !IoPoints.TDI4.Value && !IoPoints.TDI5.Value;
                MachineOperation.Pause = !IoPoints.TDI2.Value && !Marking.DoorShield;
                if (!IoPoints.TDI4.Value || !IoPoints.TDI5.Value /*|| !hasp.LicenseIsOK*/)
                {
                    m_GluePlateform.Xaxis.Stop();
                    m_GluePlateform.Yaxis.Stop();
                    m_GluePlateform.Zaxis.Stop();
                    m_CleanPlateform.Xaxis.Stop();
                    m_CleanPlateform.Yaxis.Stop();
                    m_CleanPlateform.Zaxis.Stop();

                    m_GluePlateform.stationInitialize.InitializeDone = false;
                    m_Carrier.stationInitialize.InitializeDone = false;
                    m_CleanPlateform.stationInitialize.InitializeDone = false;
                    m_Mes.stationInitialize.InitializeDone = false;
                    MachineOperation.IniliazieDone = false;
                }

                #region 设备运行中
                //if (/*IoPoints.TDI6.Value && */IoPoints.TDI7.Value)
                //{
                //    if (!ManualAutoMode)
                //    {
                //        //AppendText("设备无法启动，必须在自动模式才能操作！");
                //    }
                //    else
                //    {
                //        MachineOperation.Stop = false;
                //        MachineOperation.Reset = false;
                //        MachineOperation.Pause = false;
                //        MachineOperation.Start = true;
                //        //cdm 10.26改
                //        IoPoints.IDO0.Value = true;
                //        IoPoints.IDO16.Value = true;
                //    }
                //}
                if (ManualAutoMode && MachineOperation.Running)
                {
                    IoPoints.IDO25.Value = false;
                    IoPoints.IDO26.Value = false;

                    TimeSpan ts = watchCT.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                    Marking.CycleRunTime = elapsedTime;

                    AutoNeedleStatus = false;
                    AutoNeedleStatusRun = false;
                }
                else
                {
                    //IoPoints.TDO8.Value = false;

                }

                if (MachineOperation.Pausing)
                {
                    //cdm 10.26改
                    IoPoints.IDO9.Value = false ;
                    IoPoints.IDO8.Value = false;
                    IoPoints.IDO26.Value = true;
                }
                
                if (!ManualAutoMode && AutoNeedleStatus
                    && m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueAdjustPinPosition.X)
                     && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.GlueAdjustPinPosition.Y)
                     && m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueAdjustPinPosition.Z))
                {
                    AutoNeedleStatusRun = true;
                }

                if (!ManualAutoMode && !MachineOperation.Stopping && !MachineOperation.Resetting && AutoNeedleStatus && AutoNeedleStatusRun)
                {
                    MoveToNeedlePointP();//执行自动对针                  
                }

                #endregion

                #region 设备复位中
                if (MachineOperation.Resetting)
                {
                    switch (MachineOperation.Flow)
                    {
                        case 0:
                            MachineOperation.IniliazieDone = false;
                            MachineOperation.Stop = false;
                            MachineOperation.Reset = false;
                            IoPoints.IDO25.Value = false;
                            IoPoints.IDO26.Value = false;
                            IoPoints.IDO27.Value = true;
                            m_External.InitializingDone = false;
                            m_GluePlateform.stationInitialize.InitializeDone = false;
                            m_Carrier.stationInitialize.InitializeDone = false;
                            m_Mes.stationInitialize.InitializeDone = false;
                            m_CleanPlateform.stationInitialize.InitializeDone = false;
                            m_GluePlateform.stationInitialize.Start = false;
                            m_Carrier.stationInitialize.Start = false;
                            m_Mes.stationInitialize.Start = false;
                            m_CleanPlateform.stationInitialize.Start = false;
                            if (true) MachineOperation.Flow = 10;
                            NeedleStep = 0;
                            AutoNeedleStatus = false;
                            AutoNeedleStatusRun = false;
                            break;
                        case 10:
                            m_Carrier.stationInitialize.Start = true;
                            m_CleanPlateform.stationInitialize.Start = true;
                            m_GluePlateform.stationInitialize.Start = true;
                            m_Mes.stationInitialize.Start = true;
                            if (m_GluePlateform.stationInitialize.Running &&
                                m_CleanPlateform.stationInitialize.Running &&
                                m_Carrier.stationInitialize.Running &&
                                m_Mes.stationInitialize.Running)
                            {
                                MachineOperation.Flow = 20;
                            }
                            break;
                        case 20:
                            if (m_GluePlateform.stationInitialize.Flow == -1 ||
                                m_CleanPlateform.stationInitialize.Flow == -1 ||
                                m_Carrier.stationInitialize.Flow == -1 ||
                                m_Mes.stationInitialize.Flow == -1)
                            {
                                MachineOperation.IniliazieDone = false;
                                IoPoints.IDO27.Value = false;
                                MachineOperation.Flow = -1;
                            }
                            else
                            {
                                if (m_GluePlateform.stationInitialize.InitializeDone &&
                                    m_CleanPlateform.stationInitialize.InitializeDone &&
                                    m_Carrier.stationInitialize.InitializeDone &&
                                    m_Mes.stationInitialize.InitializeDone)
                                {
                                    MachineOperation.IniliazieDone = true;
                                    IoPoints.IDO27.Value = false;
                                    MachineOperation.Flow = 50;
                                }                   
                            }
                            break;
                        default:
                            m_GluePlateform.stationInitialize.Start = false;
                            m_Carrier.stationInitialize.Start = false;
                            m_CleanPlateform.stationInitialize.Start = false;
                            m_Mes.stationInitialize.Start = false;
                            break;
                    }
                }
                #endregion

                #region 设备停止中
                if (MachineOperation.Stopping)
                {
                    OnStop();

                    m_GluePlateform.stationInitialize.Estop = true;
                    m_Carrier.stationInitialize.Estop = true;
                    m_CleanPlateform.stationInitialize.Estop = true;
                    m_Mes.stationInitialize.Estop = true;

                    MachineOperation.IniliazieDone = false;
                    m_Carrier.stationInitialize.InitializeDone = false;
                    m_CleanPlateform.stationInitialize.InitializeDone = false;
                    m_GluePlateform.stationInitialize.InitializeDone = false;
                    m_Mes.stationInitialize.InitializeDone = false;
                    IoPoints.IDO16.Value = false;
                    IoPoints.IDO19.Value = false;

                    IoPoints.IDO9.Value = false;
                    IoPoints.IDO8.Value = false;
                    IoPoints.IDO0.Value = false;
                    IoPoints.IDO1.Value = false;

                    IoPoints.IDO25.Value = true;
                    IoPoints.IDO26.Value = false;
                    IoPoints.IDO27.Value = false;

                    AutoNeedleStatus = false;
                    AutoNeedleStatusRun = false;

                    if (!m_GluePlateform.stationInitialize.Running &&
                        !m_Carrier.stationInitialize.Running &&
                        !m_CleanPlateform.stationInitialize.Running &&
                        !m_Mes.stationInitialize.Running)
                    {
                        MachineOperation.IniliazieDone = false;
                        MachineOperation.Stopping = false;
                        m_GluePlateform.stationInitialize.Estop = false;
                        m_Carrier.stationInitialize.Estop = false;
                        m_CleanPlateform.stationInitialize.Estop = false;
                        m_Mes.stationInitialize.Estop = false;
                    }
                }
                #endregion

                #region 设备急停中
                if (!IoPoints.TDI4.Value || !IoPoints.TDI5.Value)
                {
                    MachineOperation.IniliazieDone = false;
                    m_Carrier.stationInitialize.InitializeDone = false;
                    m_CleanPlateform.stationInitialize.InitializeDone = false;
                    m_GluePlateform.stationInitialize.InitializeDone = false;
                    m_Mes.stationInitialize.InitializeDone = false;

                    m_Carrier.stationOperate.Stop = true;
                    m_CleanPlateform.stationOperate.Stop = true;
                    m_GluePlateform.stationOperate.Stop = true;
                    m_Mes.stationOperate.Stop = true;

                    m_CleanPlateform.Xaxis.IsServon = false;
                    m_CleanPlateform.Yaxis.IsServon = false;
                    m_CleanPlateform.Zaxis.IsServon = false;
                    m_GluePlateform.Xaxis.IsServon = false;
                    m_GluePlateform.Yaxis.IsServon = false;
                    m_GluePlateform.Zaxis.IsServon = false;
                    m_CleanPlateform.Xaxis.Stop();
                    m_CleanPlateform.Yaxis.Stop();
                    m_CleanPlateform.Zaxis.Stop();
                    m_GluePlateform.Xaxis.Stop();
                    m_GluePlateform.Yaxis.Stop();
                    m_GluePlateform.Zaxis.Stop();

                    IoPoints.IDO9.Value = false;
                    IoPoints.IDO8.Value = false;
                    IoPoints.IDO16.Value = false;//清洗关闭
                    IoPoints.IDO19.Value = false;//点胶关闭
                    IoPoints.IDO27.Value = false;//复位状态清除

                    AutoNeedleStatus = false;
                    AutoNeedleStatusRun = false;
                }
                #endregion
            }
        }
        private void AlarmCheck()
        {
            while (true)
            {
                Thread.Sleep(100);
                LplateformIsAlarm = AlarmCheck(m_CleanPlateform.Alarms);
                CarrierIsAlarm = AlarmCheck(m_Carrier.Alarms);
                RplateformIsAlarm = AlarmCheck(m_GluePlateform.Alarms);
                MesIsAlarm = AlarmCheck(m_Mes.Alarms);
                MachineIsAlarm = AlarmCheck(MachineAlarms);
            }
        }
        private void StatusCheck()
        {
            var list = new List<ICylinderStatusJugger>();
            m_Mes.stationInitialize.Estop = false;
            m_Carrier.stationInitialize.Estop = false;
            m_CleanPlateform.stationInitialize.Estop = false;
            m_GluePlateform.stationInitialize.Estop = false;
            list.AddRange(m_GluePlateform.CylinderStatus);
            list.AddRange(m_CleanPlateform.CylinderStatus);
            list.AddRange(m_Carrier.CylinderStatus);
            while (true)
            {
                Thread.Sleep(50);
                foreach (var lst in list)
                    lst.StatusJugger();
            }
        }
        private void LicenseCheck()
        {
            while (true)
            {
                try
                {
                    hasp.UnauthorizedDetection();
                    Thread.Sleep(600000);
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
        }

        private double Okpercent;

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            lblneed.Text = $"X={Position.Instance.CCD2NeedleOffset.X.ToString("0.00")}Y={Position.Instance.CCD2NeedleOffset.Y.ToString("0.00")}";
            //this.Text = hasp.Nature ? "AA点胶设备控制系统（永久版）" : $"AA点胶设备控制系统（试用版）到期还剩{hasp.Duetime}天";
            //btnShowWindows.Visible = !hasp.Nature;
            this.LAB_VER.Text = "v20200804";
            lblGlueShape.Text = Position.Instance.UseRectGlue?"点胶形状：矩形": "点胶形状：圆形";
            #region 文本显示
            lblMachineStatus.Text = MachineOperation.Status.ToString();
            lblMachineStatus.ForeColor = MachineStatusColor(MachineOperation.Status);
            lblSwPath.Text = Config.Instance.GlueSwPath;

            lblProductType.Text = Config.Instance.CurrentProductType;
            lblTotalCycleTime.Text = $"{(Marking.CleanCycleTime + Marking.GlueCycleTime).ToString("f1")}s";
            lblTotalNum.Text = $"{Config.Instance.ProductTotal.ToString("f0")}pcs";
            lblOkNum.Text = $"{Config.Instance.AAProductOkTotal.ToString("f0")}pcs";
            lblNgNum.Text = $"{Config.Instance.AAProductNgTotal.ToString("f0")}pcs";
            //lblPressure.Text = $"压力：{temp2.ToString("f2")}kgf";

            lblCleanCycleTime.Text = $"{Marking.CleanCycleTime.ToString("f1")}s";
            lblGlueCycleTime.Text = $"{Marking.GlueCycleTime.ToString("f1")}s";
            lblCleanOKTotal.Text = $"{Config.Instance.CleanProductOkTotal.ToString("f0")}pcs";
            lblCleanNGTotal.Text = $"{Config.Instance.CleanProductNgTotal.ToString("f0")}pcs";
            lblGlueOKTotal.Text = $"{Config.Instance.GlueProductOkTotal.ToString("f0")}pcs";
            lblGlueNGTotal.Text = $"{Config.Instance.GlueProductNgTotal.ToString("f0")}pcs";
            lblAAOKTotal.Text = $"{Config.Instance.AAProductOkTotal.ToString("f0")}pcs";
            lblAANGTotal.Text = $"{Config.Instance.AAProductNgTotal.ToString("f0")}pcs";

            lblCarrierReady.Text = m_Carrier.stationOperate.Status.ToString();
            lblCarrierReady.ForeColor = StationStatusColor(m_Carrier.stationOperate.Status);
            lblCleanReady.Text = m_CleanPlateform.stationOperate.Status.ToString();
            lblCleanReady.ForeColor = StationStatusColor(m_CleanPlateform.stationOperate.Status);
            lblGlueReady.Text = m_GluePlateform.stationOperate.Status.ToString();
            lblGlueReady.ForeColor = StationStatusColor(m_GluePlateform.stationOperate.Status);

            lblFN.Text = Marking.FN;
            lblSN.Text = Marking.SN;
            lblAAResult.Text = Marking.AAResult ? "OK" : "NG";
            lblAAResult.BackColor = Marking.AAResult ? Color.LimeGreen : Color.Red;

            //lblCurrentFN.Text = MesData.NeedShowFN;
            #endregion

            #region 图像显示
            //输送装置信号显示
            picCarrierCallOut.Image = Marking.CarrierCallOut ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picCarrierCallOutFinish.Image = Marking.CarrierCallOutFinish ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            //picCarrierCallIn.Image = Marking.CarrierCallIn ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            //picCarrierCallInFinish.Image = Marking.CarrierCallInFinish ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            //清洗工位信号显示
            picCleanCallIn.Image = Marking.CleanCallIn ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picCleanCallInFinish.Image = Marking.CleanHaveProduct ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picCleanWorking.Image = Marking.CleanWorking ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picCleanHoming.Image = Marking.CleanHoming ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picCleanWorkFinish.Image = Marking.CleanWorkFinish ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picCleanCallOut.Image = Marking.CleanCallOut ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picCleanCallOutFinish.Image = Marking.CleanCallOutFinish ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            //点胶工位信号显示
            picGlueCallIn.Image = Marking.GlueCallIn ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picGlueCallInFinish.Image = Marking.GlueHaveProduct ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picGlueWorking.Image = Marking.GlueWorking ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picGlueHoming.Image = Marking.GlueHoming ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picGlueWorkFinish.Image = Marking.GlueWorkFinish ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picGlueCallOut.Image = Marking.GlueCallOut ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picGlueCallOutFinish.Image = Marking.GlueCallOutFinish ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            //AA工位信号显示
            picAACallIn.Image = Marking.AACallIn ? Properties.Resources.LedGreen : Properties.Resources.LedNone;

            picCleanResult.Image = Marking.CleanResult ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            picGlueResult.Image = Marking.GlueResult ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            //picAAResult.Image = Marking.AAResult ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            picHaveLens.Image = Marking.HaveLensRst ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            picWhiteBoard.Image = Marking.WhiteBoardRst ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            picGlueCheck.Image = Marking.GlueCheckRst ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            picLightCamera.Image = Marking.LightCameraRst ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            picPreAAPos.Image = Marking.PreAAPosRst ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            picSearchPos.Image = Marking.SearchPosRst ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            picOCAdjust.Image = Marking.OCAdjustRst ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            picTiltAdjust.Image = Marking.TiltAdjustRst ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            picUVBefore.Image = Marking.UVBeforeRst ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            picUVAfter.Image = Marking.UVAfterRst ? Properties.Resources.LedGreen : Properties.Resources.LedRed;
            #endregion

            #region 操作权限变更
            btnStopVoice.BackColor = layerLight.VoiceClosed ? Color.LightPink : Color.Transparent;
            layerLight.Status = MachineOperation.Status;
            layerLight.Refreshing();

            btnManualAuto.Enabled = !MachineOperation.Running;
            btnReset.Enabled = !MachineOperation.Running;

            if (ManualAutoMode || Global.IsLocating)
            {
                btnTeach.Enabled = false;//测试，原程序是false；
                btnSetting.Enabled = false;
            }
            else
            {
                if (Config.Instance.userLevel == UserLevel.操作员 || Config.Instance.userLevel == UserLevel.工程师)
                {
                    if (Config.Instance.userLevel == UserLevel.工程师)
                    {
                        btnTeach.Enabled = true;
                        btnSetting.Enabled = true;
                    }
                }
            }
            //if (lstAlarm.Count > 0) lblAlarmMsg.Text = lstAlarm[0];
            //else
            //    lblAlarmMsg.Text = "";

            Marking.ScannerEnable = chkScannerEnable.Checked;
            Marking.CleanShield = chkCleanShiled.Checked;
            Marking.CleanRun = chkCleanRun.Checked;
            Marking.GlueShield = chkGlueShiled.Checked;
            Marking.GlueRun = chkGlueRun.Checked;
            Marking.HaveLensShield = chkLensShield.Checked;
            Marking.PlasmaShield = chkPlasmaShield.Checked;
            Marking.WhiteShield = chkWhiteShiled.Checked;
            Marking.CCDShield = chkCCDShiled.Checked;
            Marking.CleanRecycleRun = chkCleanRecycleRun.Checked;
            Marking.GlueRecycleRun = chkGlueRecycleRun.Checked;
            if (MachineOperation.Running)
            {
                gbxCleanSetting.Enabled = false;
                gbxGlueSetting.Enabled = false;
                gbxCarrierSetting.Enabled = false;
                gbxCarrierButton.Enabled = false;
            }
            else
            {
                gbxCleanSetting.Enabled = true;
                gbxGlueSetting.Enabled = true;
                gbxCarrierSetting.Enabled = true;
                gbxCarrierButton.Enabled = true;
            }
            #endregion

            #region 通讯测高信息显示
            RefreshCommMsg();
            #endregion

            if (Marking.CarrierWorking && IoPoints.IDI18.Value)
            {
                IoPoints.IDO8.Value = false;
            }
            SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName, Config.Instance);

            timer1.Enabled = true;
        }

        /// <summary>
        /// 显示通讯字符
        /// </summary>
        /// <param name="name">通讯模块标识符</param>
        /// <param name="flag">接收和发送标志，true接收，false发送</param>
        /// <param name="show">是否有字符需显示标志</param>
        /// <param name="msg">待显示的字符</param>
        public void ShowCommMsg(string name, bool flag, ref bool show, string msg)
        {
            if ( msg != null && show)
            {
                show = false;
                if (flag)
                    AppendText(string.Format("接收到{0}字符：{1}", name, msg));
                else
                    AppendText(string.Format("发送给{0}字符：{1}", name, msg));
            }
        }

        /// <summary>
        /// 刷新所有的通讯信息
        /// </summary>
        public void RefreshCommMsg()
        {
            //if (aaServer != null)
            //{
            //    ShowCommMsg("AA", aaServer.Flag, ref aaServer.ShowMsg, aaServer.MsgShow);
            //    aaServer.MsgShow = null;
            //}
            //if (ccdServer != null)
            //    ShowCommMsg("视觉", ccdServer.Flag, ref ccdServer.ShowMsg, ccdServer.MsgShow);
            //if (wbServer != null)
            //{
            //    ShowCommMsg("白板", wbServer.Flag, ref wbServer.ShowMsg, wbServer.MsgShow);
            //    //if (wbServer.MsgShow != null && (wbServer.MsgShow.Contains("$HWA02") || wbServer.MsgShow.Contains("$HWA99")))
            //    //    wbServer.IsResultTCP = false;
            //}
            if (heightDetector != null && heightDetector.ShowMsg)
            {
                heightDetector.ShowMsg = false;
                if (heightDetector.Flag)
                    AppendText(string.Format("接收到测高模块字符：{0}", heightDetector.MsgShow));
                else
                    AppendText(string.Format("发送给测高模块字符：{0}", heightDetector.MsgShow));
            }
        }

        private Color StationStatusColor(StationStatus status)
        {
            switch (status)
            {
                case StationStatus.模组报警:
                    return Color.Red;
                case StationStatus.模组未准备好:
                    return Color.Orange;
                case StationStatus.模组准备好:
                    return Color.Blue;
                case StationStatus.模组运行中:
                    return Color.Green;
                case StationStatus.模组暂停中:
                    return Color.Purple;
                default:
                    return Color.Red;
            }
        }
        #endregion

        #region 消息显示
        /// <summary>
        /// 使用委托方式更新AppendText显示
        /// </summary>
        /// <param name="txt">消息</param>
        public void AppendText(string txt)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendText), txt);
            }
            else
            {
                if (txt.Equals("ShowFN"))
                {
                    lblCurrentFN.Text = MesData.NeedShowFN;
                    return;
                }
                if (lstInfo.Items.Count > 1000) lstInfo.Items.Clear();
                lstInfo.Items.Insert(0, string.Format("{0}-{1}" + Environment.NewLine, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), txt));
                log.Debug(txt);
            }
        }

        public AlarmType AlarmCheck(IList<Alarm> Alarms)
        {
            var Alarm = new AlarmType();
            foreach (Alarm alarm in Alarms)
            {
                var btemp = alarm.IsAlarm;
                if (alarm.AlarmLevel == AlarmLevels.Error)
                {
                    Alarm.IsAlarm |= btemp;
                    this.Invoke(new Action(() =>
                    {
                        Msg(string.Format("{0},{1}", alarm.AlarmLevel.ToString(), alarm.Name), btemp);
                        if (btemp)
                        {
                            if (Global.FaultDictionary.ContainsKey(alarm.Name))
                            {
                                var Fau = Global.FaultDictionary[alarm.Name];
                                Fau.FaultCount++;
                            }
                            if (Global.FaultDictionary1.ContainsKey(alarm.Name))
                            {
                                var Fau = Global.FaultDictionary1[alarm.Name];
                                Fau.FaultCount++;
                            }
                            else
                            {
                                fault.FaultCode = Global.FaultDictionary[alarm.Name].FaultCode;
                                fault.FaultCount = Global.FaultDictionary[alarm.Name].FaultCount;
                                fault.FaultMessage = Global.FaultDictionary[alarm.Name].FaultMessage;
                                Global.FaultDictionary1.Add(alarm.Name, fault);
                                faultcount++;
                            }

                        }
                    }));
                }
                else if (alarm.AlarmLevel == AlarmLevels.None)
                {
                    Alarm.IsPrompt |= btemp;
                    this.Invoke(new Action(() =>
                    {
                        Msg(string.Format("{0},{1}", alarm.AlarmLevel.ToString(), alarm.Name), btemp);
                    }));
                }
                else
                {
                    Alarm.IsWarning |= btemp;
                    this.Invoke(new Action(() =>
                    {
                        Msg(string.Format("{0},{1}", alarm.AlarmLevel.ToString(), alarm.Name), btemp);
                    }));
                }
            }
            return Alarm;
        }

        private void Msg(string str, bool value)
        {
            string tempstr = null;
            bool sign = false;
            try
            {
                var arrRight = new List<object>();
                foreach (var tmpist in lstAlarm.Items) arrRight.Add(tmpist);
                if (value)
                {
                    foreach (string tmplist in arrRight)
                    {
                        if (tmplist.IndexOf("-") > -1)
                        {
                            tempstr = tmplist.Substring(tmplist.IndexOf("-") + 1, tmplist.Length - tmplist.IndexOf("-") - 1);
                        }
                        if (tempstr == (str + "\r\n"))
                        {
                            sign = true;
                            break;
                        }
                    }
                    if (!sign)
                    {
                        lstAlarm.Items.Insert(0, (string.Format("{0}-{1}" + Environment.NewLine, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), str)));
                        log.Error(str);
                    }
                }
                else
                {
                    foreach (string tmplist in arrRight)
                    {
                        if (tmplist.IndexOf("-") > -1)
                        {
                            tempstr = tmplist.Substring(tmplist.IndexOf("-") + 1, tmplist.Length - tmplist.IndexOf("-") - 1);
                            if (tempstr == (str + "\r\n")) lstAlarm.Items.Remove(tmplist);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private Color MachineStatusColor(MachineStatus status)
        {
            switch (status)
            {
                case MachineStatus.设备未准备好:
                    return Color.Orange;
                case MachineStatus.设备准备好:
                    return Color.Blue;
                case MachineStatus.设备运行中:
                    return Color.Green;
                case MachineStatus.设备停止中:
                    return Color.Red;
                case MachineStatus.设备暂停中:
                    return Color.Purple;
                case MachineStatus.设备复位中:
                    return Color.OrangeRed;
                case MachineStatus.设备报警中:
                    return Color.Red;
                case MachineStatus.设备急停已按下:
                    return Color.Red;
                default:
                    return Color.Red;
            }
        }
        #endregion

        #region 设备操作按钮

        private void btnStart_MouseDown(object sender, EventArgs e)
        {
            if (!ManualAutoMode)
            {
                AppendText("设备无法启动，必须在自动模式,启动白板 才能操作！");
                return;
            }
            MachineOperation.Start = true;
            MachineOperation.Stop = false;
            MachineOperation.Reset = false;
            MachineOperation.Pause = false;
            //cdm 10.26改
            IoPoints.IDO9.Value = true;
            IoPoints.IDO8.Value = true;
        }

        private void btnStart_MouseUp(object sender, EventArgs e)
        {
            MachineOperation.Start = false;
        }

        private void btnPause_MouseDown(object sender, EventArgs e)
        {
            MachineOperation.Pause = true;
            MachineOperation.Stop = false;
            MachineOperation.Reset = false;
            MachineOperation.Start = false;
        }

        private void btnPause_MouseUp(object sender, EventArgs e)
        {
            MachineOperation.Pause = false;
        }

        private void btnStop_MouseDown(object sender, EventArgs e)
        {
            MachineOperation.Stop = true;
            MachineOperation.Reset = false;
            MachineOperation.Pause = false;
            MachineOperation.Start = false;
        }

        private void btnStop_MouseUp(object sender, EventArgs e)
        {
            MachineOperation.Stop = false;
        }
        private void btnReset_MouseDown(object sender, EventArgs e)
        {
            if (Position.Instance.UseRectGlue) MessageBox.Show("当前模式为矩形点胶请注意治具点位匹配");
            else MessageBox.Show("当前模式为圆弧点胶请注意治具点位匹配");
            if (ManualAutoMode)
            {
                if (!MachineIsAlarm.IsAlarm && !LplateformIsAlarm.IsAlarm
                     && !CarrierIsAlarm.IsAlarm && !RplateformIsAlarm.IsAlarm)
                    AppendText("设备手动状态时，才能复位。自动状态只能清除报警！");
                m_External.AlarmReset = true;
            }
            else
            {
                if (MachineOperation != null)
                {
                    MachineOperation.IniliazieDone = false;
                    MachineOperation.Flow = 0;
                    MachineOperation.Reset = true;
                    MachineOperation.Stop = false;
                    MachineOperation.Pause = false;
                    MachineOperation.Start = false;
                }
            }
        }

        private void btnReset_MouseUp(object sender, EventArgs e)
        {
            MachineOperation.Reset = false;
            m_External.AlarmReset = false;
        }
        private void btnManualAuto_Click(object sender, EventArgs e)
        {
            if (ManualAutoMode && MachineOperation.Running) //自动模式不能直接切换为手动，需要先停止运行再切换模式
            {
                AppendText("设备运行中，不能切换至手动模式!");
                return;
            }
            ManualAutoMode = ManualAutoMode ? false : true;
            btnManualAuto.Text = ManualAutoMode ? "自动模式" : "手动模式";
            btnManualAuto.ForeColor = ManualAutoMode ? Color.Green : Color.Red;
            if (ManualAutoMode) tbcMain.SelectedTab = tpgMain;
        }

        private void btnProductSetting_Click(object sender, EventArgs e)
        {
            if (MachineOperation.Running)
            {
                AppendText("设备停止或暂停时，才能操作！");
                return;
            }
            new frmRunSetting().ShowDialog();
        }
        
        private void btnTapping_Click(object sender, EventArgs e)
        {
            //if (MachineOperation.Running)
            //{
            //    AppendText("设备停止或暂停时，才能操作！");
            //    return;
            //}
            //else
            //{
            //    bool itrue = true;
            //    int step = 0;
            //    while (itrue)
            //    {
            //        switch (step)
            //        {
            //            case 0:
            //                MoveToPointP(Position.Instance.CutGlueStartPosition);
            //                step = 10;
            //                break;
            //            case 10:
            //                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.CutGlueStartPosition.X)
            //                    && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.CutGlueStartPosition.Y)
            //                    && m_GluePlateform.Zaxis.IsInPosition(Position.Instance.CutGlueStartPosition.Z))
            //                {
            //                    m_GluePlateform.Xaxis.MoveTo(Position.Instance.CutGlueEndPosition.X, Global.RXmanualSpeed);
            //                    m_GluePlateform.Yaxis.MoveTo(Position.Instance.CutGlueEndPosition.Y, Global.RYmanualSpeed);
            //                    step = 20;
            //                }
            //                break;
            //            case 20:
            //                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.CutGlueEndPosition.X)
            //                    && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.CutGlueEndPosition.Y))
            //                {
            //                    MoveToPointP(Position.Instance.GlueSafePosition);
            //                    step = 30;
            //                }
            //                break;
            //            case 30:
            //                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueSafePosition.X)
            //                    && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.GlueSafePosition.Y)
            //                    && m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
            //                {
            //                    itrue = false;
            //                    step = 40;
            //                }
            //                break;
            //        }
            //    }
            //    //MoveToPointP(Position.Instance.CutGlueStartPosition);
            //    //if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.CutGlueStartPosition.X)
            //    //    && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.CutGlueStartPosition.Y)
            //    //    && m_GluePlateform.Zaxis.IsInPosition(Position.Instance.CutGlueStartPosition.Z))
            //    //{
            //    //    m_GluePlateform.Xaxis.MoveTo(Position.Instance.CutGlueEndPosition.X, Global.RXmanualSpeed);
            //    //    m_GluePlateform.Yaxis.MoveTo(Position.Instance.CutGlueEndPosition.Y, Global.RYmanualSpeed);
            //    //}
            //}
        }
        
        private void btnAlarmClean_MouseUp(object sender, MouseEventArgs e)
        {
            m_External.AlarmReset = false;
        }


        private void btnAlarmClean_MouseDown(object sender, MouseEventArgs e)
        {
            m_External.AlarmReset = true;
            m_GluePlateform.Xaxis.Clean();
            m_GluePlateform.Yaxis.Clean();
            m_GluePlateform.Zaxis.Clean();
            m_CleanPlateform.Xaxis.Clean();
            m_CleanPlateform.Yaxis.Clean();
            m_CleanPlateform.Zaxis.Clean();
        }

        private void chkScannerEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkScannerEnable.Checked)
            {
                OpenScanner();
            }
        }

        private void btnCountClean_Click(object sender, EventArgs e)
        {
            if (MachineOperation.Running)
            {
                AppendText("设备停止或暂停时，才能操作！");
                return;
            }
            DialogResult result = MessageBox.Show("是否清除计数？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Config.Instance.CleanProductOkTotal = 0;
                Config.Instance.CleanProductNgTotal = 0;
                Config.Instance.GlueProductOkTotal = 0;
                Config.Instance.GlueProductNgTotal = 0;
                Config.Instance.AAProductOkTotal = 0;
                Config.Instance.AAProductNgTotal = 0;

                SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName, Config.Instance);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            new frmStationDebug(m_CleanPlateform, m_GluePlateform, m_Carrier).ShowDialog();
        }

        private void btnCarrier1_Click(object sender, EventArgs e)
        {
            if (IoPoints.IDO9.Value)
            {
                IoPoints.IDO9.Value = false;
            }
            else
            {
                IoPoints.IDO9.Value = true;
            }
        }

        private void btnCarrier2_Click(object sender, EventArgs e)
        {
            if (IoPoints.IDO8.Value)
            {
                IoPoints.IDO8.Value = false;
            }
            else
            {
                IoPoints.IDO8.Value = true;
            }
        }

        private void btnCarrier3_Click(object sender, EventArgs e)
        {
            if (IoPoints.IDO0.Value)
            {
                IoPoints.IDO0.Value = false;
            }
            else
            {
                IoPoints.IDO1.Value = false;
                IoPoints.IDO0.Value = true;
            }
        }

        private void btnCarrier4_Click(object sender, EventArgs e)
        {
            if (IoPoints.IDO1.Value)
            {
                IoPoints.IDO1.Value = false;
            }
            else
            {
                IoPoints.IDO0.Value = false;
                IoPoints.IDO1.Value = true;
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.M:
                    if (ManualAutoMode && MachineOperation.Running) //自动模式不能直接切换为手动，需要先停止运行再切换模式
                    {
                        AppendText("设备运行中，不能切换至手动模式!");
                        return;
                    }
                    ManualAutoMode = ManualAutoMode ? false : true;
                    btnManualAuto.Text = ManualAutoMode ? "自动模式" : "手动模式";
                    btnManualAuto.ForeColor = ManualAutoMode ? Color.Green : Color.Red;
                    if (ManualAutoMode) tbcMain.SelectedTab = tpgMain;
                    break;
            }
        }

        bool IsAllSetShield;

        private void btnNeedleCalib_Click(object sender, EventArgs e)
        {
            if (MachineOperation.Running)
            {
                AppendText("设备停止或暂停时，才能操作！");
                return;
            }
            else
            {
                AppendText("自动寻针执行中。。。");
                AutoNeedleStatus = true;
                MoveToPointP(Position.Instance.GlueAdjustPinPosition);//移动到对针初位置                
            }
        }

        /// <summary>
        /// 自动对针功能不好使，屏蔽
        /// </summary>
        private void MoveToNeedlePointP()
        {
            //var Speed = 0;
            //var Value = 0;
            //double X0 = 0;
            //double Y0 = 0;
            //double Z0 = 0;
            //double X1 = 0;
            //double Y1 = 0;

            //while (!ManualAutoMode && !MachineOperation.Resetting && AutoNeedleStatus && AutoNeedleStatusRun)
            //{
            //    Thread.Sleep(10);
            //    if (m_GluePlateform.Xaxis.IsAlarmed || m_GluePlateform.Xaxis.IsEmg || !m_GluePlateform.Xaxis.IsServon
            //        || (m_GluePlateform.Xaxis.CurrentPos > (Position.Instance.GlueAdjustPinPosition.X + 4 * Position.Instance.NeedleCalibOffset.X)
            //        && m_GluePlateform.Xaxis.CurrentPos < (Position.Instance.GlueAdjustPinPosition.X - 4 * Position.Instance.NeedleCalibOffset.X)))
            //    {
            //        m_GluePlateform.Xaxis.Stop();
            //        AppendText("点胶X轴异常停止，请复位！");
            //        AutoNeedleStatus = false;
            //        AutoNeedleStatusRun = false;
            //        return;
            //    }
            //    if (m_GluePlateform.Yaxis.IsAlarmed || m_GluePlateform.Yaxis.IsEmg || !m_GluePlateform.Yaxis.IsServon
            //   || (m_GluePlateform.Yaxis.CurrentPos > (Position.Instance.GlueAdjustPinPosition.Y + 4 * Position.Instance.NeedleCalibOffset.Y)
            //   && m_GluePlateform.Yaxis.CurrentPos < (Position.Instance.GlueAdjustPinPosition.Y - 4 * Position.Instance.NeedleCalibOffset.Y)))
            //    {
            //        m_GluePlateform.Yaxis.Stop();
            //        AppendText("点胶Y轴异常停止，请复位！");
            //        AutoNeedleStatus = false;
            //        AutoNeedleStatusRun = false;
            //        return;

            //    }
            //    if (AutoNeedleStatus)
            //    {
            //        switch (NeedleStep)
            //        {
            //            case 0: //X轴正方向对针
            //                Thread.Sleep(200);
            //                if (!IoPoints.IDI27.Value && m_GluePlateform.Xaxis.CurrentPos < (Position.Instance.GlueAdjustPinPosition.X + Position.Instance.NeedleCalibOffset.X * 2))
            //                {
            //                    Speed = 5000;
            //                    Value = 1000;
            //                    Value *= 1;
            //                    Thread.Sleep(200);
            //                    APS168.APS_relative_move(0, Value, Speed);
            //                    //cdm1031
            //                    Position.Instance.NeedleCalibOffset.X = -1;
            //                    Position.Instance.NeedleCalibOffset.Y = 1;
            //                    Position.Instance.NeedleCalibOffset.Z = 1;
            //                }
            //                else
            //                {
            //                    if (IoPoints.IDI27.Value)
            //                    {
            //                        // X0 = m_Pointform.Xaxis.CurrentPos;
            //                        NeedleStep = 1;
            //                    }

            //                    else
            //                    {
            //                        NeedleStep = -1;
            //                        AppendText("点胶X轴左侧寻针失败，请重新执行对针确认！");
            //                    }
            //                }
            //                break;
            //            case 1:  //X轴小步左侧寻中心
            //                Speed = 1000;
            //                Value = 100;
            //                Value *= 1;
            //                Thread.Sleep(200);
            //                APS168.APS_relative_move(0, Value, Speed);
            //                Thread.Sleep(200);
            //                if (!IoPoints.IDI27.Value || m_GluePlateform.Xaxis.CurrentPos > (Position.Instance.GlueAdjustPinPosition.X + Position.Instance.NeedleCalibOffset.X))
            //                {
            //                    if (!IoPoints.IDI27.Value && m_GluePlateform.Xaxis.CurrentPos <= (Position.Instance.GlueAdjustPinPosition.X + Position.Instance.NeedleCalibOffset.X))
            //                    {
            //                        X0 = m_GluePlateform.Xaxis.CurrentPos;
            //                        AppendText("点胶X轴向左寻针成功！");
            //                        NeedleStep = 20;
            //                    }
            //                    else
            //                    {
            //                        NeedleStep = -1;
            //                        AppendText("点胶X轴向左寻针失败，请重新执行对针确认！");
            //                    }

            //                }
            //                break;
            //            case 10:  //移至第二相限
            //                if (!IoPoints.IDI27.Value)
            //                {
            //                    Thread.Sleep(200);
            //                    m_GluePlateform.Xaxis.MoveTo(Position.Instance.GlueAdjustPinPosition.X + Position.Instance.NeedleCalibOffset.X, AxisParameter.Instance.RXspeed);
            //                    NeedleStep = 11;
            //                }
            //                break;
            //            case 11://X轴移到位
            //                Thread.Sleep(200);
            //                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueAdjustPinPosition.X + Position.Instance.NeedleCalibOffset.X))
            //                    NeedleStep = 20;
            //                break;
            //            case 20: //X轴负方向对针
            //                if (!IoPoints.IDI27.Value && m_GluePlateform.Xaxis.CurrentPos >= Position.Instance.GlueAdjustPinPosition.X - Position.Instance.NeedleCalibOffset.X)
            //                {
            //                    Speed = 5000;
            //                    Value = 1000;
            //                    Value *= -1;
            //                    Thread.Sleep(200);
            //                    APS168.APS_relative_move(0, Value, Speed);
            //                }
            //                else
            //                {
            //                    if (IoPoints.IDI27.Value)
            //                    {
            //                        NeedleStep = 21;
            //                    }
            //                    else
            //                    {
            //                        NeedleStep = -1;
            //                        AppendText("点胶X轴向右寻针失败，请重新执行对针确认！");
            //                    }
            //                }
            //                break;
            //            case 21:  //X轴小步右侧寻中心
            //                Speed = 1000;
            //                Value = 100;
            //                Value *= -1;
            //                Thread.Sleep(200);
            //                APS168.APS_relative_move(0, Value, Speed);
            //                Thread.Sleep(200);
            //                if (!IoPoints.IDI27.Value || m_GluePlateform.Xaxis.CurrentPos < Position.Instance.GlueAdjustPinPosition.X - Position.Instance.NeedleCalibOffset.X)
            //                {
            //                    if (!IoPoints.IDI27.Value && m_GluePlateform.Xaxis.CurrentPos >= Position.Instance.GlueAdjustPinPosition.X - Position.Instance.NeedleCalibOffset.X)
            //                    {
            //                        X1 = m_GluePlateform.Xaxis.CurrentPos;
            //                        AppendText("点胶X轴向右寻针成功！");
            //                        NeedleStep = 40;
            //                    }
            //                    else
            //                    {
            //                        NeedleStep = -1;
            //                        AppendText("点胶X轴向右寻针失败，请重新执行对针确认！");
            //                    }

            //                }
            //                break;

            //            case 30:  //移回第一相限
            //                Thread.Sleep(200);
            //                m_GluePlateform.Xaxis.MoveTo(Position.Instance.GlueAdjustPinPosition.X, AxisParameter.Instance.RYspeed);
            //                Thread.Sleep(200);
            //                NeedleStep = 31;
            //                break;
            //            case 31://X轴移到位
            //                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueAdjustPinPosition.X))
            //                    NeedleStep = 40;
            //                break;
            //            case 40: //Y轴正方向对针
            //                if (!IoPoints.IDI26.Value && m_GluePlateform.Yaxis.CurrentPos < (Position.Instance.GlueAdjustPinPosition.Y + Position.Instance.NeedleCalibOffset.Y))
            //                {
            //                    Speed = 5000;
            //                    Value = 1000;
            //                    Value *= 1;
            //                    Thread.Sleep(200);
            //                    APS168.APS_relative_move(0, Value, Speed);
            //                }
            //                else
            //                {
            //                    if (IoPoints.IDI26.Value)
            //                    {
            //                        NeedleStep = 41;
            //                    }
            //                    else
            //                    {
            //                        NeedleStep = -1;
            //                        AppendText("点胶Y轴向前寻针失败，请重新执行对针确认！");
            //                    }
            //                }
            //                break;
            //            case 41:  //Y轴小步向前寻中心
            //                Speed = 1000;
            //                Value = 500;
            //                Value *= 1;
            //                Thread.Sleep(200);
            //                APS168.APS_relative_move(1, Value, Speed);
            //                Thread.Sleep(200);
            //                if (!IoPoints.IDI26.Value || m_GluePlateform.Yaxis.CurrentPos > (Position.Instance.GlueAdjustPinPosition.Y + Position.Instance.NeedleCalibOffset.Y))
            //                {
            //                    if (!IoPoints.IDI26.Value && m_GluePlateform.Yaxis.CurrentPos <= (Position.Instance.GlueAdjustPinPosition.Y + Position.Instance.NeedleCalibOffset.Y))
            //                    {
            //                        Y0 = m_GluePlateform.Yaxis.CurrentPos;
            //                        AppendText("点胶Y轴向前寻针成功！");
            //                        NeedleStep = 60;
            //                    }
            //                    else
            //                    {
            //                        NeedleStep = -1;
            //                        AppendText("点胶Y轴向前寻针失败，请重新执行对针确认！");
            //                    }

            //                }
            //                break;
            //            case 50:  //移至第四相限  
            //                Thread.Sleep(200);
            //                m_GluePlateform.Yaxis.MoveTo(Position.Instance.GlueAdjustPinPosition.Y + Position.Instance.NeedleCalibOffset.Y * 2, AxisParameter.Instance.RYspeed);
            //                Thread.Sleep(200);
            //                NeedleStep = 51;
            //                break;
            //            case 51://Y轴移到位                      
            //                if (m_GluePlateform.Yaxis.IsInPosition(Position.Instance.GlueAdjustPinPosition.Y + Position.Instance.NeedleCalibOffset.Y * 2))
            //                    NeedleStep = 60;
            //                break;
            //            case 60: //Y轴负方向对针
            //                if (!IoPoints.IDI26.Value && m_GluePlateform.Yaxis.CurrentPos > Position.Instance.GlueAdjustPinPosition.Y - Position.Instance.NeedleCalibOffset.Y)
            //                {
            //                    Speed = 5000;
            //                    Value = 1000;
            //                    Value *= -1;
            //                    Thread.Sleep(200);
            //                    APS168.APS_relative_move(1, Value, Speed);
            //                }
            //                else
            //                {
            //                    if (IoPoints.IDI26.Value)
            //                    {
            //                        NeedleStep = 61;
            //                    }
            //                    else
            //                    {
            //                        NeedleStep = -1;
            //                        AppendText("点胶Y轴向后寻针失败，请重新执行对针确认！");
            //                    }

            //                }
            //                break;
            //            case 61:  //Y轴小步向前寻中心
            //                Speed = 1000;
            //                Value = 500;
            //                Value *= -1;
            //                Thread.Sleep(200);
            //                APS168.APS_relative_move(1, Value, Speed);
            //                Thread.Sleep(200);
            //                if (!IoPoints.IDI26.Value || m_GluePlateform.Yaxis.CurrentPos < Position.Instance.GlueAdjustPinPosition.Y - Position.Instance.NeedleCalibOffset.Y)
            //                {
            //                    if (!IoPoints.IDI26.Value && m_GluePlateform.Yaxis.CurrentPos >= Position.Instance.GlueAdjustPinPosition.Y - Position.Instance.NeedleCalibOffset.Y)
            //                    {
            //                        Y1 = m_GluePlateform.Yaxis.CurrentPos;
            //                        AppendText("点胶Y轴向后寻针成功！");
            //                        NeedleStep = 70;
            //                    }
            //                    else
            //                    {
            //                        NeedleStep = -1;
            //                        AppendText("点胶Y轴向后寻针失败，请重新执行对针确认！");
            //                    }

            //                }
            //                break;
            //            case 70: //移动到对针坐标位置
            //                if (X0 > 0 && X1 > 0 & Y0 > 0 && Y1 > 0)
            //                {
            //                    Position.Instance.NeedleCalibCenter.X = ((X0 + X1) / 2);
            //                    Position.Instance.NeedleCalibCenter.Y = (Y0 + Y1) / 2;
            //                    m_GluePlateform.Xaxis.MoveTo(Position.Instance.NeedleCalibCenter.X, AxisParameter.Instance.RXspeed);
            //                    m_GluePlateform.Yaxis.MoveTo(Position.Instance.NeedleCalibCenter.Y, AxisParameter.Instance.RYspeed);
            //                    Thread.Sleep(200);
            //                    NeedleStep = 71;
            //                }
            //                else
            //                {
            //                    NeedleStep = -1;
            //                    AppendText("点胶XY轴寻针失败，请重新执行对针确认！");
            //                }

            //                break;
            //            case 71://XY轴移到位
            //                if ((m_GluePlateform.Xaxis.IsInPosition(Position.Instance.NeedleCalibCenter.X) || IoPoints.IDI27.Value)
            //                    && (m_GluePlateform.Yaxis.IsInPosition(Position.Instance.NeedleCalibCenter.Y) || IoPoints.IDI26.Value))
            //                    NeedleStep = 80;
            //                break;
            //            case 80: //Z上移离开对针坐标位置                           
            //                if (IoPoints.IDI27.Value || IoPoints.IDI26.Value || m_GluePlateform.Zaxis.CurrentPos > (Position.Instance.GlueAdjustPinPosition.Z - Position.Instance.NeedleCalibOffset.Z))
            //                {
            //                    Speed = 5000;
            //                    Value = 1000;
            //                    Value *= -1;
            //                    Thread.Sleep(200);
            //                    APS168.APS_relative_move(2, Value, Speed);
            //                }
            //                else
            //                {
            //                    if (!IoPoints.IDI27.Value && !IoPoints.IDI26.Value && m_GluePlateform.Zaxis.CurrentPos <= (Position.Instance.GlueAdjustPinPosition.Z - Position.Instance.NeedleCalibOffset.Z))
            //                    {
            //                        NeedleStep = 81;
            //                    }
            //                    else
            //                    {
            //                        NeedleStep = -1;
            //                        AppendText("点胶Y轴向后寻针失败，请重新执行对针确认！");
            //                    }

            //                }
            //                break;
            //            case 81: //Z上移小步对针坐标位置  
            //                Speed = 1000;
            //                Value = 100;
            //                Value *= 1;
            //                Thread.Sleep(200);
            //                APS168.APS_relative_move(2, Value, Speed);
            //                Thread.Sleep(200);
            //                if ((IoPoints.IDI27.Value && IoPoints.IDI26.Value) || m_GluePlateform.Zaxis.CurrentPos > (Position.Instance.GlueAdjustPinPosition.Z + Position.Instance.NeedleCalibOffset.Z))
            //                {
            //                    if (IoPoints.IDI27.Value && IoPoints.IDI26.Value && m_GluePlateform.Zaxis.CurrentPos <= (Position.Instance.GlueAdjustPinPosition.Z + Position.Instance.NeedleCalibOffset.Z))
            //                    {
            //                        Z0 = m_GluePlateform.Zaxis.CurrentPos;
            //                        Position.Instance.NeedleCalibCenter.Z = Z0;
            //                        AppendText("点胶Z轴寻针成功！");
            //                        NeedleStep = 100;
            //                    }
            //                    else
            //                    {
            //                        NeedleStep = -1;
            //                        AppendText("点胶Z轴寻针失败，请重新执行对针确认！");
            //                    }

            //                }
            //                break;
            //            case 100: //Z上移离开对针坐标位置    
            //                Thread.Sleep(200);
            //                if (X0 > 0 && X1 > 0 & Y0 > 0 && Y1 > 0 && Z0 > 0)//IO10Points.DI24.Value && IO10Points.DI25.Value &&
            //                {
            //                    double GlueOffsetX = Position.Instance.NeedleCalibCenter.X - Position.Instance.GlueAdjustPinPosition.X + Position.Instance.NeedleCalibOffset.X;
            //                    double GlueOffsetY = Position.Instance.NeedleCalibCenter.Y - Position.Instance.GlueAdjustPinPosition.Y + Position.Instance.NeedleCalibOffset.Y;
            //                    double GlueOffsetZ = Position.Instance.NeedleCalibCenter.Z - Position.Instance.GlueAdjustPinPosition.Z + Position.Instance.NeedleCalibOffset.Z;
            //                    if (GlueOffsetX > 0)
            //                    {
            //                        Position.Instance.GlueOffsetX = Position.Instance.GlueOffsetX + GlueOffsetX;
            //                    }
            //                    else
            //                    {
            //                        Position.Instance.GlueOffsetX = Position.Instance.GlueOffsetX - GlueOffsetX;
            //                    }
            //                    if (GlueOffsetY > 0)
            //                    {
            //                        Position.Instance.GlueOffsetY = Position.Instance.GlueOffsetY - GlueOffsetY;
            //                    }
            //                    else
            //                    {
            //                        Position.Instance.GlueOffsetY = Position.Instance.GlueOffsetY + GlueOffsetY;
            //                    }
            //                    if (GlueOffsetZ > 0)
            //                        Position.Instance.GlueHeight = Position.Instance.GlueHeight + GlueOffsetZ;
            //                    else
            //                        Position.Instance.GlueHeight = Position.Instance.GlueHeight - GlueOffsetZ;
            //                    AppendText("自动对针成功！");
            //                    NeedleStep = 90;
            //                }
            //                else
            //                {
            //                    NeedleStep = 90;
            //                    AppendText("自动对针失败！");
            //                }
            //                break;
            //            case 90: //Z返回原位       
            //                Thread.Sleep(3000);
            //                IoPoints.m_ApsController.BackHome(m_GluePlateform.Zaxis.NoId);
            //                NeedleStep = 91;
            //                break;
            //            case 91:  //XY返回原位 
            //                if (IoPoints.m_ApsController.CheckHomeDone(m_GluePlateform.Zaxis.NoId, 10.0) == 0)
            //                {
            //                    IoPoints.m_ApsController.BackHome(m_GluePlateform.Xaxis.NoId);
            //                    IoPoints.m_ApsController.BackHome(m_GluePlateform.Yaxis.NoId);
            //                    NeedleStep = 92;
            //                }
            //                else
            //                {
            //                    m_GluePlateform.Zaxis.Stop();
            //                }
            //                break;
            //            case 92://判断Z轴是否动作完成
            //                if (IoPoints.m_ApsController.CheckHomeDone(m_GluePlateform.Xaxis.NoId, 10.0) == 0 &&
            //                    IoPoints.m_ApsController.CheckHomeDone(m_GluePlateform.Yaxis.NoId, 10.0) == 0)
            //                {
            //                    AutoNeedleStatus = false;
            //                    AutoNeedleStatusRun = false;
            //                    NeedleStep = 0;
            //                }
            //                break;
            //            default:
            //                NeedleStep = 0;
            //                AutoNeedleStatus = false;
            //                AutoNeedleStatusRun = false;
            //                break;
            //        }
            //    }
            //}
        }

        private void btnAAImage_Click(object sender, EventArgs e)
        {
            if (Wb == null || Wb.IsDisposed)
            {
                Wb = new frmWb();
                Wb.frmWb_Load(this, null);
                Wb.Visible = true;
                Wb.ShowWb(this, null);
            }
            else
            {
                Wb.WindowState = FormWindowState.Normal;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (tbcMain.SelectedTab == tpgMain)
            {
                tbcMain.SelectedTab = tpgAAImage;
            }

            if (tbcMain.SelectedTab == tpgAAImage)
            {
                //切回主页面不再切换
                tbcMain.SelectedTab = tpgMain;
                timer2.Enabled = false;
            }
            
            Thread.Sleep(100);
            
        }

        private void btnAAVision_Click(object sender, EventArgs e)
        {
            if (aa == null || aa.IsDisposed)
            {
                aa = new frmAAVision();
                aa.frmAAVision_Load(this, null);
                aa.Show();
            }
            else
            {
                aa.WindowState = FormWindowState.Normal;
            }
        }

        private void btnIsExitWorking_Click(object sender, EventArgs e)
        {
            if (MachineOperation.Running)
            {
                AppendText("设备暂停时，才能操作！");
                return;
            }
            if (MachineOperation.Alarming) return;
            if ((MachineOperation.Status != MachineStatus.设备准备好) 
                && (MachineOperation.Status != MachineStatus.设备暂停中))return;
            if (Global.IsLocating) return;
        }

        private void 设置电源ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool success = false;
            desay.View.WhiteBoardPower frmPower = new View.WhiteBoardPower(this.serialPort1, out success);
            if (success)
            {
                frmPower.Show();
            }
        }

        private void cb_WhiteBoardPower_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_WhiteBoardPower.Checked)
            {
                OpenPower();
                log.Debug("白板电源控制重启");
            }
        }

        private void gbxCarrierSetting_Enter(object sender, EventArgs e)
        {
            
        }


        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void btnWb_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show("是否需要退出白板软件并重新加载", "", MessageBoxButtons.YesNo)) return;

            if (MachineOperation.Running)
            {
                AppendText("设备暂停时，才能操作！");
                return;
            }
            LoadagainWb();
        }

        private void btnStopVoice_Click(object sender, EventArgs e)
        {
            Marking.VoiceClosed = !Marking.VoiceClosed;
            //layerLight.VoiceClosed = !layerLight.VoiceClosed;
        }

        #endregion

        private bool ServoAxisIsReady(ApsAxis axis)
        {
            return !axis.IsServon || axis.IsAlarmed || axis.IsEmg || axis.IsMEL || axis.IsPEL;
        }

        /// <summary>
        /// 割胶使用函数，用于移动位置
        /// </summary>
        /// <param name="pos">待移动的位置坐标</param>
        /// <returns></returns>
        private int MoveToPointP(Point3D<double> pos)
        {
            if (!m_GluePlateform.stationInitialize.InitializeDone) return -4;
            //判断Z轴是否在零点
            if (!m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, Global.RZmanualSpeed);
            while (true)
            {
                Thread.Sleep(10);
                if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z)) break;
                if (m_GluePlateform.Zaxis.IsAlarmed || m_GluePlateform.Zaxis.IsEmg || !m_GluePlateform.Zaxis.IsServon
                    || m_GluePlateform.Zaxis.IsPositiveLimit || m_GluePlateform.Zaxis.IsNegativeLimit)
                {
                    m_GluePlateform.Zaxis.Stop();
                    return -4;
                }
            }
            //将X、Y移动到指定位置
            if (!m_GluePlateform.Xaxis.IsInPosition(pos.X)) m_GluePlateform.Xaxis.MoveTo(pos.X, Global.RXmanualSpeed);
            if (!m_GluePlateform.Yaxis.IsInPosition(pos.Y)) m_GluePlateform.Yaxis.MoveTo(pos.Y, Global.RYmanualSpeed);
            while (true)
            {
                Thread.Sleep(10);
                if (m_GluePlateform.Xaxis.IsInPosition(pos.X) && m_GluePlateform.Yaxis.IsInPosition(pos.Y)) break;
                if (m_GluePlateform.Xaxis.IsAlarmed || m_GluePlateform.Xaxis.IsEmg || !m_GluePlateform.Xaxis.IsServon
                    || m_GluePlateform.Xaxis.IsPositiveLimit || m_GluePlateform.Xaxis.IsNegativeLimit
                    || m_GluePlateform.Yaxis.IsAlarmed || m_GluePlateform.Yaxis.IsEmg || !m_GluePlateform.Yaxis.IsServon
                    || m_GluePlateform.Yaxis.IsPositiveLimit || m_GluePlateform.Yaxis.IsNegativeLimit)
                {
                    m_GluePlateform.Xaxis.Stop();
                    m_GluePlateform.Yaxis.Stop();
                    return -4;
                }
            }
            //将Z轴移动到指定位置
            m_GluePlateform.Zaxis.MoveTo(pos.Z, Global.RZmanualSpeed);
            while (true)
            {
                Thread.Sleep(10);
                if (m_GluePlateform.Zaxis.IsInPosition(pos.Z)) break;
                if (m_GluePlateform.Zaxis.IsAlarmed || m_GluePlateform.Zaxis.IsEmg || !m_GluePlateform.Zaxis.IsServon
                    || m_GluePlateform.Zaxis.IsPositiveLimit || m_GluePlateform.Zaxis.IsNegativeLimit)
                {
                    m_GluePlateform.Zaxis.Stop();
                    return -4;
                }
            }
            return 0;
        }

        #region 串口通讯
        public void OpenDetectHeight()
        {
            try
            {
                heightDetector = new System.Device.Panasonic();
                heightDetector.SetConnectionParam(Position.Instance.HeightConnectionString);
                heightDetector.DeviceOpen();
      
                //heightDetector._OnReadComBuff += RecvFromHeightDector;
                Marking.HeightDetectorOpenSuccess = true;
                LoadingMessage("测高模块打开成功");
            }
            catch
            {
                Marking.HeightDetectorOpenSuccess = false;
                LoadingMessage("测高模块打开失败");
            }
        }
        public void CloseDetectHeight()
        {
            try
            {
                heightDetector.DeviceClose();
                snScanner.DeviceClose();
            }
            catch (Exception e)
            {

            }
        }

        private void RecvFromHeightDector(byte[] readbuffer)
        {
            string strMsg = Encoding.Default.GetString(readbuffer);
            try
            {
                heightDetector.DealRecvData(strMsg, ref Marking.DetectHeight);
            }
            catch (Exception e)
            {
                LogHelper.Error(e.Message);
            }
        }

        public void OpenScanner()
        {
            try
            {
                if (!autoScanner.IsOpen)
                {
                    autoScanner = new System.Device.DM50S();
                    autoScanner.SetConnectionParam(Position.Instance.AutoConnectionString);
                    autoScanner.Open();
                    //manualScanner = new System.Device.DM50S();
                    //manualScanner.SetConnectionParam(Position.Instance.ManualConnectionString);
                    //manualScanner.Open();
                    AppendText("治具码扫码枪端口打开成功");
                }
                if (!snScanner.IsOpen)
                {
                    snScanner = new System.Device.HoneyWell1900();
                    snScanner.SetConnectionParam(Position.Instance.ManualConnectionString);
                    snScanner.DeviceOpen();
                    AppendText("产品码扫码枪端口打开成功");
                }
            }
            catch (Exception e)
            {
                AppendText("扫码枪端口打开失败");
            }
        }

        public void CloseScanner()
        {
            try
            {
                if (autoScanner.IsOpen)
                    autoScanner.Close();
                if (manualScanner.IsOpen)
                    manualScanner.Close();
                LoadingMessage("扫码枪端口打开成功");
            }
            catch (Exception e)
            {
                LoadingMessage("扫码枪端口关闭失败");
            }
        }
        public void OpenPower()
        {

            try
            {
                if(this.serialPort1.IsOpen)
                {
                    this.serialPort1.Close();
                    Thread.Sleep(200);
                }
                var ConnectionParam = Position.Instance.WhiteBoardPowerConnectionString;
                string[] param = ConnectionParam.Split(',');
                this.serialPort1.PortName = param[0];
                this.serialPort1.BaudRate = int.Parse(param[1]);
                this.serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), param[2]);
                this.serialPort1.DataBits = int.Parse(param[3]);
                this.serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), param[4]);
                this.serialPort1.ReadTimeout = int.Parse(param[5]);
                this.serialPort1.WriteTimeout = int.Parse(param[6]);
                this.serialPort1.RtsEnable = true;
                this.serialPort1.DtrEnable = true;

                if (!this.serialPort1.IsOpen)
                {
                    this.serialPort1.Open();
                    bool success = this.serialPort1.IsOpen;

                    if (success)
                    {
                        AppendText("电源端口打开成功");
                        serialPort1.Write("SYST:REM" + Environment.NewLine);//远程PC操作指令
                        Thread.Sleep(50);
                        serialPort1.Write("INST CH1" + Environment.NewLine);//选择频道1
                        serialPort1.Write("VOLT " + Position.Instance.Voltage[0].ToString() + Environment.NewLine);
                        Thread.Sleep(50);//电压
                        serialPort1.Write("CURR " + Position.Instance.Current[0].ToString() + Environment.NewLine);
                        Thread.Sleep(50);//电流

                        serialPort1.Write("INST CH2" + Environment.NewLine);
                        Thread.Sleep(50);
                        serialPort1.Write("VOLT " + Position.Instance.Voltage[1].ToString() + Environment.NewLine);
                        Thread.Sleep(50);
                        serialPort1.Write("CURR " + Position.Instance.Current[1].ToString() + Environment.NewLine);
                        Thread.Sleep(50);


                        serialPort1.Write("INST CH3" + Environment.NewLine);
                        Thread.Sleep(50);
                        serialPort1.Write("VOLT " + Position.Instance.Voltage[2].ToString() + Environment.NewLine);
                        Thread.Sleep(50);
                        serialPort1.Write("CURR " + Position.Instance.Current[2].ToString() + Environment.NewLine);
                        Thread.Sleep(50);

                        this.serialPort1.Write("OUTP 1" + Environment.NewLine);//用于开启通道
                        AppendText("白板电源端口打开成功");
                    }
                    else
                    {
                        AppendText("电源端口打开失败");
                    }

                  

                    /*
                    //设置电源为PC控制模式
                    this.serialPort1.Write("SYST:REM\r\n");
                    //设置通道1电压电流值
                    this.serialPort1.Write("APPL CH1," + Position.Instance.Voltage[0].ToString() + "," + Position.Instance.Current[0].ToString() + "\r\n");
                    //设置通道2电压电流值
                    this.serialPort1.Write("APPL CH2," + Position.Instance.Voltage[1].ToString() + "," + Position.Instance.Current[1].ToString() + "\r\n");
                    //设置通道3电压电流值
                    this.serialPort1.Write("APPL CH3," + Position.Instance.Voltage[2].ToString() + "," + Position.Instance.Current[2].ToString() + "\r\n");
                   */

                    //输出打开


               
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("电源端口打开异常:" + ex.ToString());
           //     LoadingMessage("白板电源端口打开失败");

            }
        }
        public void ClosePower()
        {
            try
            {
                if (this.serialPort1.IsOpen)
                    this.serialPort1.Close();
                MessageBox.Show("电源端口关闭成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("电源端口关闭异常:" + ex.ToString());
            }
        }
        #endregion
    }
}

