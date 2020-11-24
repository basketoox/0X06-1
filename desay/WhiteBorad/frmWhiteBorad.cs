using System;
using System.Windows.Forms;
using DTCCM2_DLL;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using Ini;
using System.Diagnostics;
using DxDemoDtccm2.DesayView;
using log4net;
using desay.ProductData;

namespace desay
{
    public partial class frmWhiteBorad : Form
    {
        //该模块为独立模块，暂不与主线程代码融合（但功能已集成）,该模块可以单独调试（在program中启动）
        static ILog log = LogManager.GetLogger(typeof(frmWhiteBorad));

        public static frmWhiteBorad wb;

        #region 变量
        private string _deviceName;
        public int _deviceID = -1;

        public static int m_iDevID = 0;
        public static ushort m_uWidth = 56;
        public static ushort m_uHeight = 192;
        public static uint m_nMemSize = 0;
        public static uint m_um_FrameCnt = 0;
        public static uint m_uErrm_FrameCnt = 0;
        public static double m_fFramefps = 0;
        public static double m_Disaplayfps = 0;

        static ManualResetEvent m_ThreadEvent = new ManualResetEvent(false);
        Thread m_pThread;
        static ManualResetEvent m_DisplayThreadEvent = new ManualResetEvent(false);
        Thread m_pDisplayThread;

        public static bool m_b_thread_start = false;
        public static bool m_b_thread_error = false;
        public static FrameInfo m_FrameInfo = new FrameInfo();

        public static Boolean m_bTriple = false;
        public static uint m_FrameCnt = 0;
        public static uint m_DeviceErrCnt = 0;
        public static byte[] m_TripleBuffer = null;
        public static IntPtr m_DisplyBuffer = new IntPtr();

        public uint grabSize = 0;

        public UInt16[] paraList;
        public ushort paraListSize = 0;

        public SensorTab CurrentSensor = new SensorTab();
        public WhiteBoardDetection wbDetection = new WhiteBoardDetection();

        bool ShowStain = false;
        bool ShowDefect = false;
        bool SavePhtoto = false;

        public static string DeviceConfigFilePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OpenDevice.ini");
            }
        }
        public static string DefectConfigFilePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OpenDeviceConfig.ini");
            }
        }

        Graphics g;
        Bitmap myImage;
        int PBwidth, PBheight;

        byte[] bmp;

        #endregion

        #region 引用方法
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "GetTickCount")]
        public static extern uint GetTickCount();

        #endregion

        #region 方法
        private UInt16 Convert2UInt16(string str)
        {
            if (str == null || str == "")
                return 0;
            if ((str.Length > 2) && (str[0] == '0') && (str[1] == 'x'))
            {
                return Convert.ToUInt16(str, 16);
            }
            else
            {
                return Convert.ToUInt16(str);
            }
        }
        private Byte Convert2Byte(string str)
        {
            if (str == null || str == "")
                return 0;
            if ((str.Length > 2) && (str[0] == '0') && (str[1] == 'x'))
            {
                return Convert.ToByte(str, 16);
            }
            else
            {
                return Convert.ToByte(str);
            }
        }
        public bool LoadWhiteBoardDetectionPra(string filename)
        {
            try
            {
                string dirPath = Directory.GetCurrentDirectory();
                IniFile iReader = new IniFile(filename);
                //PIN定义根据原白板软件编写
                wbDetection.pin.m_pin1 = Convert2UInt16(iReader.ReadKey("PIN00", "PIN"));
                wbDetection.pin.m_pin2 = Convert2UInt16(iReader.ReadKey("PIN01", "PIN"));
                wbDetection.pin.m_pin3 = Convert2UInt16(iReader.ReadKey("PIN02", "PIN"));
                wbDetection.pin.m_pin4 = Convert2UInt16(iReader.ReadKey("PIN03", "PIN"));
                wbDetection.pin.m_pin5 = Convert2UInt16(iReader.ReadKey("PIN04", "PIN"));
                wbDetection.pin.m_pin6 = Convert2UInt16(iReader.ReadKey("PIN05", "PIN"));
                wbDetection.pin.m_pin7 = Convert2UInt16(iReader.ReadKey("PIN06", "PIN"));
                wbDetection.pin.m_pin8 = Convert2UInt16(iReader.ReadKey("PIN07", "PIN"));
                wbDetection.pin.m_pin9 = Convert2UInt16(iReader.ReadKey("PIN08", "PIN"));
                wbDetection.pin.m_pin10 = Convert2UInt16(iReader.ReadKey("PIN09", "PIN"));
                wbDetection.pin.m_pin11 = Convert2UInt16(iReader.ReadKey("PIN10", "PIN"));
                wbDetection.pin.m_pin12 = Convert2UInt16(iReader.ReadKey("PIN11", "PIN"));
                wbDetection.pin.m_pin13 = Convert2UInt16(iReader.ReadKey("PIN12", "PIN"));
                wbDetection.pin.m_pin14 = Convert2UInt16(iReader.ReadKey("PIN13", "PIN"));
                wbDetection.pin.m_pin15 = Convert2UInt16(iReader.ReadKey("PIN14", "PIN"));
                wbDetection.pin.m_pin16 = Convert2UInt16(iReader.ReadKey("PIN15", "PIN"));
                wbDetection.pin.m_pin17 = Convert2UInt16(iReader.ReadKey("PIN16", "PIN"));
                wbDetection.pin.m_pin18 = Convert2UInt16(iReader.ReadKey("PIN17", "PIN"));
                wbDetection.pin.m_pin19 = Convert2UInt16(iReader.ReadKey("PIN18", "PIN"));
                wbDetection.pin.m_pin20 = Convert2UInt16(iReader.ReadKey("PIN19", "PIN"));
                wbDetection.pin.m_pin21 = Convert2UInt16(iReader.ReadKey("PIN20", "PIN"));
                wbDetection.pin.m_pin22 = Convert2UInt16(iReader.ReadKey("PIN21", "PIN"));
                wbDetection.pin.m_pin23 = Convert2UInt16(iReader.ReadKey("PIN22", "PIN"));
                wbDetection.pin.m_pin24 = Convert2UInt16(iReader.ReadKey("PIN23", "PIN"));
                wbDetection.pin.m_pin25 = Convert2UInt16(iReader.ReadKey("PIN24", "PIN"));
                wbDetection.pin.m_pin26 = Convert2UInt16(iReader.ReadKey("PIN25", "PIN"));


                wbDetection.m_sStainDefectConfig.nBadPixel_Spec = Convert2UInt16(iReader.ReadKey("nBadPixel_Spec", "StainDefect")); 
                wbDetection.m_sStainDefectConfig.nHotPixel_Spec = Convert2UInt16(iReader.ReadKey("nHotPixel_Spec", "StainDefect")); 
                wbDetection.m_sStainDefectConfig.nDeepBlemish_Spec = Convert2UInt16(iReader.ReadKey("nDeepBlemish_Spec", "StainDefect")); 
                wbDetection.m_sStainDefectConfig.nLowBlemish_Spec = Convert2UInt16(iReader.ReadKey("nLowBlemish_Spec", "StainDefect")); 

                wbDetection.m_sStainDefectConfig.m_sDefectCriterion.BinaryThreshold = Convert2UInt16(iReader.ReadKey("BinaryThreshold", "StainDefect"));
                wbDetection.m_sStainDefectConfig.m_sDefectCriterion.dCircleOffset = Convert2UInt16(iReader.ReadKey("dCircleOffset", "StainDefect"));
                wbDetection.m_sStainDefectConfig.m_sDefectCriterion.dDebugSavepicture = (Convert2UInt16(iReader.ReadKey("dDebugSavepicture", "StainDefect")) == 1 )? true : false ; 

                wbDetection.m_sStainDefectConfig.m_sDefectCriterion.fDefect = float.Parse((iReader.ReadKey("fDefect", "StainDefect")));
                wbDetection.m_sStainDefectConfig.m_sDefectCriterion.fDefectMaxNumInDusty = Convert2UInt16(iReader.ReadKey("fDefectMaxNumInDusty", "StainDefect")); 
                wbDetection.m_sStainDefectConfig.m_sDefectCriterion.iWDefect = Convert2UInt16(iReader.ReadKey("iWDefect", "StainDefect"));
                wbDetection.m_sStainDefectConfig.m_sDefectCriterion.iWDefectMaxNumInDusty = Convert2UInt16(iReader.ReadKey("iWDefectMaxNumInDusty", "StainDefect")); 

                wbDetection.m_sStainDefectConfig.m_sStrainCriterion.BinaryThreshold = wbDetection.m_sStainDefectConfig.m_sDefectCriterion.BinaryThreshold;
                wbDetection.m_sStainDefectConfig.m_sStrainCriterion.sCircleOffset = wbDetection.m_sStainDefectConfig.m_sDefectCriterion.dCircleOffset;
                wbDetection.m_sStainDefectConfig.m_sStrainCriterion.sDebugSavepicture = wbDetection.m_sStainDefectConfig.m_sDefectCriterion.dDebugSavepicture;

                wbDetection.m_sStainDefectConfig.m_sStrainCriterion.fLuxLower = Convert2UInt16(iReader.ReadKey("fLuxLower", "StainDefect")); 
                wbDetection.m_sStainDefectConfig.m_sStrainCriterion.fLuxUpper = Convert2UInt16(iReader.ReadKey("fLuxUpper", "StainDefect"));
                wbDetection.m_sStainDefectConfig.m_sStrainCriterion.highStainAreaNum = Convert2UInt16(iReader.ReadKey("highStainAreaNum", "StainDefect"));
                wbDetection.m_sStainDefectConfig.m_sStrainCriterion.highStainNumInArea = Convert2UInt16(iReader.ReadKey("highStainNumInArea", "StainDefect")); 
                wbDetection.m_sStainDefectConfig.m_sStrainCriterion.iJump = Convert2UInt16(iReader.ReadKey("iJump", "StainDefect")); 
                wbDetection.m_sStainDefectConfig.m_sStrainCriterion.lowStainAreaNum = Convert2UInt16(iReader.ReadKey("lowStainAreaNum", "StainDefect"));
                wbDetection.m_sStainDefectConfig.m_sStrainCriterion.lowStainNumInArea = Convert2UInt16(iReader.ReadKey("lowStainNumInArea", "StainDefect"));

                ShowStain = iReader.ReadKey("ShowStain", "wbGloble")=="True"? true:false;
                ShowDefect = iReader.ReadKey("ShowDefect", "wbGloble") == "True" ? true : false;
                SavePhtoto = iReader.ReadKey("SavePhtoto", "wbGloble") == "True" ? true : false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
                return false;
            }
            return true;
        }
        public bool LoadConfig(string filename)
        {
            try
            {
                string dirPath = Directory.GetCurrentDirectory();
                IniFile iReader = new IniFile(filename);
                CurrentSensor.width = Convert2UInt16(iReader.ReadKey("width", "Sensor"));
                CurrentSensor.height = Convert2UInt16(iReader.ReadKey("height", "Sensor"));
                CurrentSensor.type = Convert2Byte(iReader.ReadKey("type", "Sensor"));
                CurrentSensor.port = Convert2Byte(iReader.ReadKey("port", "Sensor"));
                CurrentSensor.pin = Convert2Byte(iReader.ReadKey("pin", "Sensor"));
                CurrentSensor.slaveID = Convert2Byte(iReader.ReadKey("SlaveID", "Sensor"));
                CurrentSensor.mode = Convert2Byte(iReader.ReadKey("mode", "Sensor"));
                CurrentSensor.flagReg = Convert2UInt16(iReader.ReadKey("FlagReg", "Sensor"));
                CurrentSensor.flagMask = Convert2UInt16(iReader.ReadKey("FlagMask", "Sensor"));
                CurrentSensor.flagData = Convert2UInt16(iReader.ReadKey("FlagData", "Sensor"));
                CurrentSensor.flagReg1 = Convert2UInt16(iReader.ReadKey("FlagReg1", "Sensor"));
                CurrentSensor.flagMask1 = Convert2UInt16(iReader.ReadKey("FlagMask1", "Sensor"));
                CurrentSensor.flagData1 = Convert2UInt16(iReader.ReadKey("FlagData1", "Sensor"));
                CurrentSensor.outformat = Convert2Byte(iReader.ReadKey("outformat", "Sensor"));
                CurrentSensor.mclk = Convert2UInt16(iReader.ReadKey("mclk", "Sensor"));
                CurrentSensor.avdd = Convert2UInt16(iReader.ReadKey("avdd", "Sensor"));
                CurrentSensor.dovdd = Convert2UInt16(iReader.ReadKey("dovdd", "Sensor"));
                CurrentSensor.dvdd = Convert2UInt16(iReader.ReadKey("dvdd", "Sensor"));

                CurrentSensor.pclk_pol = Convert.ToInt16(iReader.ReadKey("pclk", "Sensor"));
                CurrentSensor.hsync_pol = Convert.ToInt16(iReader.ReadKey("hsync_pol", "Sensor"));
                CurrentSensor.vsync_pol = Convert.ToInt16(iReader.ReadKey("vsync_pol", "Sensor"));
                CurrentSensor.sync_pol_auto = Convert.ToInt16(iReader.ReadKey("sync_pol_auto", "Sensor"));


                string regData = iReader.ReadSectionData("ParaList").Trim();
                string[] lines = regData.Split('\0');
                ushort offset = 0;
                this.CurrentSensor.paraList = new UInt16[8192 * 4];
                for (int idx = 0; idx < lines.Length; idx++)
                {
                    string[] values = lines[idx].Split(',');
                    this.CurrentSensor.paraList[offset++] = Convert2UInt16(values[0]);
                    this.CurrentSensor.paraList[offset++] = Convert2UInt16(values[1]);
                    this.CurrentSensor.paraList[offset++] = Convert2UInt16(values[2]);
                    this.CurrentSensor.paraList[offset++] = Convert2UInt16(values[3]);
                }
                this.CurrentSensor.paraListSize = offset;
                paraList = this.CurrentSensor.paraList;
                paraListSize = this.CurrentSensor.paraListSize;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 保存Raw图片
        /// </summary>
        /// <param name="Filename"></param>
        /// <param name="pBuffer"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool bSaveRawFile(string Filename, byte[] pBuffer, uint width, uint height, uint type)
        {
            UInt32 dwSizeImage = width * height * type;
            FileStream fs = new FileStream(Filename, FileMode.OpenOrCreate);
            if (null == fs)
            {
                MessageBox.Show("bSaveRawFile fs is null");
            }
            fs.Write(pBuffer, 1, (Int32)dwSizeImage);
            fs.Close();
            return true;
        }
        public Bitmap BGR24ToBitmap(byte[] imgBGR)
        {

            int p = 0;
            Bitmap bmp = new Bitmap(CurrentSensor.width, CurrentSensor.height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            if (imgBGR != null)
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.BitmapData bmpdata = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
                IntPtr ptr = bmpdata.Scan0;
                System.Runtime.InteropServices.Marshal.Copy(imgBGR, 0, ptr, imgBGR.Length);
                bmp.UnlockBits(bmpdata);
            }
            return bmp;
        }
        #endregion

        #region 测试线程

        public  void StartOneTest()
        {
            int res = 0;
            try
            {
                res = OneTest();
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                Marking.WbRequestResultFlg = true;
            }

            if (res == 0)
            {
                lblCapImage.Text = "失败";
                lblBad.Text = "NG";
                lblStain.Text = "NG";
            }
            else
            {
                lblCapImage.Text = "成功";
            }


            if (res == 0)
            {
                Marking.WbCheckAgainFlg = true;
                Marking.WhiteBoardResult = false;

            }
            else if (res == 1)
            {
                Marking.WbCheckAgainFlg = false;
                Marking.WhiteBoardResult = false;
            }
            else if (res == 2)
            {
                Marking.WbCheckAgainFlg = false;
                Marking.WhiteBoardResult = true;
            }
        }


        #endregion

        #region 采集与显示线程
        static void ImageThread(object obj)
        {
            frmWhiteBorad ia = (frmWhiteBorad)obj;
            ia.ImageThread();
            return;
        }
        int white1 = 0;
        bool ifinit = false;
        IntPtr pBmp2;
        IntPtr GrabBuffer;
        public void ImageThread()
        {
            uint timeStart = 0;
            uint timeStop = 0;
            uint FrameNum = 0;
            uint DeviceErrCnt = 0;
            int bRet = 0;
            byte[] data = new byte[1];
            m_um_FrameCnt = 0;
            m_uErrm_FrameCnt = 0;
            m_fFramefps = 0;
            try
            {



                m_nMemSize = grabSize;
                int width = CurrentSensor.width;
                int height = CurrentSensor.height;
                int Size = width * height * 3 + 1024;
                if (!ifinit)
                {
                    ifinit = true;
                    m_TripleBuffer = new byte[width * height * 3 + 1024];
                  
                    GrabBuffer = Marshal.AllocHGlobal((int)Size);
                    pBmp2 = Marshal.AllocHGlobal(width * height * 4);
                    m_DisplyBuffer = Marshal.AllocHGlobal((int)Size);

                }
               
                m_bTriple = false;
                uint uGrabSize = 0;
                m_b_thread_error = true;
                while (true)
                {
                    m_DeviceErrCnt = DeviceErrCnt;
                    try
                    {
                        white1++;
                        bRet = DTCCM2_API.GrabFrame(GrabBuffer, (uint)Size, ref uGrabSize, ref m_FrameInfo, (int)m_iDevID);
                        if (bRet == WhiteBoardDetection.DT_ERROR_OK)
                        {
                            Thread.Sleep(200);
                            try
                            {
                                m_b_thread_error = false;
                                if (!m_bTriple)
                                {
                                    Marshal.Copy(GrabBuffer, m_TripleBuffer, 0, (int)grabSize);
                                    m_bTriple = true;

                                }
                                m_um_FrameCnt++;
                                m_FrameCnt++;
                                FrameNum++;

                                if (FrameNum % 10 == 0)
                                {
                                    timeStop = GetTickCount() - timeStart;
                                    timeStart = GetTickCount();
                                    m_fFramefps = (float)(10000.0 / timeStop);
                                }
                                DeviceErrCnt = 0;
                            }
                            catch
                            {

                            }

                        }
                        else if (bRet == WhiteBoardDetection.DT_ERROR_FAILED)
                        {
                            if (m_uErrm_FrameCnt > 10)
                            {
                                log.Debug("Err image too many!");
                                break;
                            }
                            DeviceErrCnt = 0;
                            Thread.Sleep(1);
                        }
                        else
                        {
                            DeviceErrCnt++;
                            if (DeviceErrCnt > 10)
                            {

                                log.Debug("Can't grab image!");
                                log.Debug(DateTime.Now.ToString());
                                break;
                            }
                        }
                        if(white1>6)
                        {
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        log.Debug(e.Message + e.StackTrace);
                    }
                }
                m_b_thread_start = true;
                m_pThread.Abort();
            }
            catch(Exception ex)
            {

            }
            return;
        }
        static void ImageDisplayThread(object obj)
        {
            frmWhiteBorad ia = (frmWhiteBorad)obj;
            ia.ImageDisplayThread();
            return;
        }
        IntPtr pBmp;
        public void ImageDisplayThread()
        {
            int width = CurrentSensor.width;
            int height = CurrentSensor.height;
            pBmp = Marshal.AllocHGlobal(width * height * 3);
            byte[] bmp = new byte[width * height];
            uint timeStart = GetTickCount();
            uint timeStop = 0;
            uint DisplayFrmCnt = 0;
            m_DisplayThreadEvent.Set();

            while (true)
            {
                m_DisplayThreadEvent.WaitOne();
                if (!m_bTriple)
                {
                    Thread.Sleep(1);
                    timeStop += GetTickCount() - timeStart;
                    timeStart = GetTickCount();
                    continue;
                }
                else if (m_FrameCnt > 0 && m_bTriple)
                {
                    timeStart = GetTickCount();
                    try
                    {
                        Marshal.Copy(m_TripleBuffer, 0, m_DisplyBuffer, (int)grabSize);
                        DTCCM2_API.ImageProcess(m_DisplyBuffer, pBmp, width, height, ref m_FrameInfo, m_iDevID);
                        DTCCM2_API.DisplayRGB24(pBmp, ref m_FrameInfo, m_iDevID);
                        m_bTriple = false;
                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show(e.Message);
                        //MessageBox.Show(e.StackTrace);
                    }

                    DisplayFrmCnt++;
                    timeStop += GetTickCount() - timeStart;
                    timeStart = GetTickCount();

                    if (DisplayFrmCnt % 10 == 0)
                    {
                        m_Disaplayfps = (float)(10000.0 / timeStop);
                        timeStop = 0;
                    }
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }
        #endregion

        #region 与（度信测试盒）相机相关的操作
        /// <summary>
        /// 原C#demo自带的CloseCamera
        /// </summary>
        /// <returns></returns>
        public int bCloseCamera()
        {
            if (_deviceID != -1)
            {
                DTCCM2_API.CloseVideo(_deviceID);
                DTCCM2_API.ResetSensorI2cBus(_deviceID);
                DTCCM2_API.SensorEnable(Convert.ToByte(CurrentSensor.pin ^ 0x03), true, _deviceID);
                Thread.Sleep(50);
                DTCCM2_API.SetSensorClock(false, 24 * 10, _deviceID);
                SENSOR_POWER[] power = new SENSOR_POWER[10] { SENSOR_POWER.POWER_AVDD, SENSOR_POWER.POWER_DOVDD, SENSOR_POWER.POWER_DVDD, SENSOR_POWER.POWER_AFVCC, SENSOR_POWER.POWER_VPP, 0, 0, 0, 0, 0 };
                int[] volt = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] current = new int[10] { 500, 500, 500, 500, 500, 0, 0, 0, 0, 0 };
                bool[] onoff = new bool[10] { false, false, false, false, false, false, false, false, false, false };
                CURRENT_RANGE[] range = new CURRENT_RANGE[10];
                //使能柔性接口
                DTCCM2_API.EnableSoftPin(false, _deviceID);
                DTCCM2_API.EnableGpio(false, _deviceID);
                DTCCM2_API.SetSoftPinPullUp(false, _deviceID);
                DTCCM2_API.SensorEnable(0, true, _deviceID);

                //设置5路电压值
                DTCCM2_API.PmuSetVoltage(power, volt, 5, _deviceID);
                Thread.Sleep(150);
                //设置电压开关
                DTCCM2_API.PmuSetOnOff(power, onoff, 5, _deviceID);
            }

            return 1;
        }

        /// <summary>
        /// OpenDevice
        /// </summary>
        /// <returns></returns>
        public bool OpenDevice()
        {
            int iDev = 0;
            if (DevCombox.Items.Count<0)
            {
                IntPtr[] devName = new IntPtr[8];
                int rect = -1;
                rect = DTCCM2_DLL.DTCCM2_API.EnumerateDevice(devName, 8, ref iDev);
                this._deviceName = Marshal.PtrToStringAnsi(devName[0]);
            }         
            else if (_deviceID>=0)
            {
               if( DTCCM2_API.IsDevConnect(_deviceID) == WhiteBoardDetection.DT_ERROR_OK)
                {
                    DTCCM2_API.CloseDevice(_deviceID);
                }
            }
            this._deviceName = DevCombox.SelectedItem.ToString();
            if (DTCCM2_API.OpenDevice(this._deviceName, ref _deviceID, iDev) != WhiteBoardDetection.DT_ERROR_OK)
            {
                return false;
            }
            else
            {
                byte[] pSN = new byte[32];
                int pRetLen=0;
                DTCCM2_API.GetDeviceSN(pSN, 32, ref pRetLen, _deviceID);
                string str = System.Text.Encoding.Default.GetString(pSN);
            }
            return true;
        }

        /// <summary>
        /// 保存图片到文件
        /// </summary>
        /// <param name="TripleBuffer"></param>
        private void CaptureImageToFile(byte[] TripleBuffer)
        {
            int width = CurrentSensor.width;
            int height = CurrentSensor.height;
            int Size = CurrentSensor.width * CurrentSensor.height * 3 + 1024;
            byte[] pIn = TripleBuffer;
            byte[] pOut = new byte[CurrentSensor.width * CurrentSensor.height * 3];
            string filename = null;

            filename = DateTime.Now.Year.ToString() + "_"
                    + DateTime.Now.Month.ToString() + "_"
                    + DateTime.Now.Day.ToString() + "_"
                    + DateTime.Now.DayOfWeek.ToString() + "_"
                    + DateTime.Now.Hour.ToString() + "_"
                    + DateTime.Now.Minute.ToString() + "_"
                    + DateTime.Now.Second.ToString() + "_";

            SaveFileDialog sf = new SaveFileDialog();

            sf.Title = "保存导出图片";
            sf.Filter = "Jpeg File(*.jpg)|*.jpg|Bitmap File(*.bmp)|*.bmp|Raw File(*.raw)|*.raw||*.*";           //删选、设定文件显示类型
            sf.FileName = filename;
            if (sf.ShowDialog() == DialogResult.OK)
            {
                string CaptureTpye = null;
                switch (sf.FilterIndex)
                {
                    case 1:
                        CaptureTpye = ".jpg";
                        break;
                    case 2:
                        CaptureTpye = ".bmp";
                        break;
                    case 3:
                        CaptureTpye = ".raw";
                        break;
                    case 4:
                        CaptureTpye = ".raw";
                        break;
                    default:
                        break;
                }

                if (CaptureTpye == ".raw")
                {
                    if (CurrentSensor.type == WhiteBoardDetection.D_YUV)
                    {
                        bSaveRawFile(sf.FileName, pIn, (uint)width, (uint)height, 2);
                    }
                }

                DTCCM2_API.DataToRGB24(pIn, pOut, (ushort)width, (ushort)height, CurrentSensor.type, this._deviceID);

                if (CaptureTpye == ".jpg")
                {

                    int nType = CurrentSensor.type;
                    if (nType == WhiteBoardDetection.D_YUV || nType == WhiteBoardDetection.D_YUV_SPI || nType == WhiteBoardDetection.D_YUV_MTK_S)
                    {
                        Bitmap bmp = BGR24ToBitmap(pOut);
                        bmp.Save(sf.FileName, ImageFormat.Jpeg);
                    }
                    else
                    {
                        Bitmap bmp = BGR24ToBitmap(pOut);
                        bmp.Save(sf.FileName, ImageFormat.Jpeg);
                    }
                }
                else if (CaptureTpye == ".bmp")
                {
                    try
                    {
                        Bitmap bmp = BGR24ToBitmap(pOut);
                        bmp.Save(sf.FileName, ImageFormat.Bmp);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                }
                return;
            }
        }
        /// <summary>
        /// 保存bmp到文件
        /// </summary>
        /// <param name="bmp"></param>
        private void CaptureBmpToFile(Bitmap bmp,string catalogue)
        {
            int width = CurrentSensor.width;
            int height = CurrentSensor.height;
            int Size = CurrentSensor.width * CurrentSensor.height * 3 + 1024;
            byte[] pOut = new byte[CurrentSensor.width * CurrentSensor.height * 3];

            string filename = DateTime.Now.Year.ToString() + "_"
                    + DateTime.Now.Month.ToString() + "_"
                    + DateTime.Now.Day.ToString() + "_"
                    + DateTime.Now.DayOfWeek.ToString() + "_"
                    + DateTime.Now.Hour.ToString() + "_"
                    + DateTime.Now.Minute.ToString() + "_"
                    + DateTime.Now.Second.ToString();
            try
            {
                bmp.Save(catalogue + filename, ImageFormat.Bmp);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }  
            return;
        }
        /// <summary>
        /// 获取一张BMP图片
        /// </summary>
        /// <param name="TripleBuffer"></param>
        private Bitmap CaptureImage(byte[] TripleBuffer)
        {
            int width = CurrentSensor.width;
            int height = CurrentSensor.height;
            int Size = CurrentSensor.width * CurrentSensor.height * 3 + 1024;
            byte[] pIn = TripleBuffer;
            byte[] pOut = new byte[CurrentSensor.width * CurrentSensor.height * 3];

            DTCCM2_API.DataToRGB24(pIn, pOut, (ushort)width, (ushort)height, CurrentSensor.type, this._deviceID);
            try
            {
                return BGR24ToBitmap(pOut);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
            
        }
        /// <summary>
        /// 枚举Device
        /// </summary>
        /// <returns></returns>
        public int EnumerateDev()
        {
            string[] m_DevName = new string[8];
            string[] m_curDevName = new string[8];

            int DevNum = 0;
            IntPtr[] RcvArray = new IntPtr[m_DevName.Length];//Here can't use string
            int ret = DTCCM2_API.EnumerateDevice(RcvArray, 8, ref DevNum);
            DevCombox.Items.Clear();

            for (int i = 0; i < DevNum; i++)
            {
                m_DevName[i] = Marshal.PtrToStringAnsi(RcvArray[i]);
                if (m_DevName[i] != null)
                {
                    DevCombox.Items.Insert(i, m_DevName[i]);
                    Marshal.FreeCoTaskMem(RcvArray[i]);
                }
            }
            if (DevNum > 0)
            {
                DevCombox.SelectedIndex = 0;
            }

            return ret;
        }
        /// <summary>
        /// 根据原AA_Image软件写的关闭设备程序
        /// </summary>
        private void CloseAllPlay()
        {
            if (m_pThread != null)
            {
                m_pThread.Abort();
                m_pThread = null;
            }
            if (m_pDisplayThread != null)
            {
                m_pDisplayThread.Abort();
                m_pDisplayThread = null;
            }
            m_b_thread_start = false;

            DTCCM2_API.CloseVideo(this._deviceID);
            DTCCM2_API.ResetSensorI2cBus(this._deviceID);
            DTCCM2_API.SensorEnable((byte)(CurrentSensor.pin ^ 2), true, this._deviceID);
            Thread.Sleep(20);
            DTCCM2_API.SensorEnable((byte)(CurrentSensor.pin ^ 1), true, this._deviceID);
            Thread.Sleep(400);

            SENSOR_POWER[] Power = new SENSOR_POWER[10] { SENSOR_POWER.POWER_AVDD, SENSOR_POWER.POWER_DOVDD, SENSOR_POWER.POWER_DVDD, SENSOR_POWER.POWER_AFVCC, SENSOR_POWER.POWER_VPP, 0, 0, 0, 0, 0 };
            bool[] OnOff = { false };
            int[] Volt = { 0 };
            DTCCM2_API.PmuSetVoltage(Power, Volt, 5, this._deviceID);
            if (DTCCM2_API.PmuSetOnOff(Power, OnOff, 5, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK)
            {
                //这里会下电失败，但是也可以关闭相机
                //MessageBox.Show("下电失败");
                m_TripleBuffer = null;
                m_DisplyBuffer = IntPtr.Zero;
                //MessageBox.Show("已关闭");
                return;
            }
            //MessageBox.Show("下电成功");
            m_TripleBuffer = null;
            m_DisplyBuffer = IntPtr.Zero;
            //MessageBox.Show("关闭成功");
            return;
        }
        #endregion

        #region 界面操作
        public frmWhiteBorad()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;
            wb = this;
            try
            {
                EnumerateDev();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
            LoadConfig(DeviceConfigFilePath);
            LoadWhiteBoardDetectionPra(DefectConfigFilePath);
            ShowPinConfig();
            ShowParameterConfig();
        }
        private void frmWhiteBorad_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                //if (m_pThread != null)
                //{
                //    m_pThread.Abort();
                //    m_pThread = null;
                //}
                //if (m_pDisplayThread != null)
                //{
                //    m_pDisplayThread.Abort();
                //    m_pDisplayThread = null;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Application.Exit();
            }
        }
        private void ShowPinConfig()
        {
            comboxPin_01.SelectedIndex = wbDetection.pin.m_pin1;
            comboxPin_02.SelectedIndex = wbDetection.pin.m_pin2;
            comboxPin_03.SelectedIndex = wbDetection.pin.m_pin3;
            comboxPin_04.SelectedIndex = wbDetection.pin.m_pin4;
            comboxPin_05.SelectedIndex = wbDetection.pin.m_pin5;
            comboxPin_06.SelectedIndex = wbDetection.pin.m_pin6;
            comboxPin_07.SelectedIndex = wbDetection.pin.m_pin7;
            comboxPin_08.SelectedIndex = wbDetection.pin.m_pin8;
            comboxPin_09.SelectedIndex = wbDetection.pin.m_pin9;
            comboxPin_10.SelectedIndex = wbDetection.pin.m_pin10;
            comboxPin_11.SelectedIndex = wbDetection.pin.m_pin11;
            comboxPin_12.SelectedIndex = wbDetection.pin.m_pin12;
            comboxPin_13.SelectedIndex = wbDetection.pin.m_pin13;
            comboxPin_14.SelectedIndex = wbDetection.pin.m_pin14;
            comboxPin_15.SelectedIndex = wbDetection.pin.m_pin15;
            comboxPin_16.SelectedIndex = wbDetection.pin.m_pin16;
            comboxPin_17.SelectedIndex = wbDetection.pin.m_pin17;
            comboxPin_18.SelectedIndex = wbDetection.pin.m_pin18;
            comboxPin_19.SelectedIndex = wbDetection.pin.m_pin19;
            comboxPin_20.SelectedIndex = wbDetection.pin.m_pin20;
            comboxPin_21.SelectedIndex = wbDetection.pin.m_pin21;
            comboxPin_22.SelectedIndex = wbDetection.pin.m_pin22;
            comboxPin_23.SelectedIndex = wbDetection.pin.m_pin23;
            comboxPin_24.SelectedIndex = wbDetection.pin.m_pin24;
            comboxPin_25.SelectedIndex = wbDetection.pin.m_pin25;
            comboxPin_26.SelectedIndex = wbDetection.pin.m_pin26;
        }
        private void SavePinConfig()
        {
           string dirPath = Directory.GetCurrentDirectory();
           IniFile iReader = new IniFile(DefectConfigFilePath);
           wbDetection.pin.m_pin1 = comboxPin_01.SelectedIndex;
           wbDetection.pin.m_pin2 = comboxPin_02.SelectedIndex;
           wbDetection.pin.m_pin3 = comboxPin_03.SelectedIndex;
           wbDetection.pin.m_pin4 = comboxPin_04.SelectedIndex;
           wbDetection.pin.m_pin5 = comboxPin_05.SelectedIndex;
           wbDetection.pin.m_pin6 = comboxPin_06.SelectedIndex;
           wbDetection.pin.m_pin7 = comboxPin_07.SelectedIndex;
           wbDetection.pin.m_pin8 = comboxPin_08.SelectedIndex;
           wbDetection.pin.m_pin9 = comboxPin_09.SelectedIndex;
           wbDetection.pin.m_pin10= comboxPin_10.SelectedIndex ;
           wbDetection.pin.m_pin11= comboxPin_11.SelectedIndex ;
           wbDetection.pin.m_pin12= comboxPin_12.SelectedIndex ;
           wbDetection.pin.m_pin13= comboxPin_13.SelectedIndex ;
           wbDetection.pin.m_pin14= comboxPin_14.SelectedIndex ;
           wbDetection.pin.m_pin15= comboxPin_15.SelectedIndex ;
           wbDetection.pin.m_pin16= comboxPin_16.SelectedIndex ;
           wbDetection.pin.m_pin17= comboxPin_17.SelectedIndex ;
           wbDetection.pin.m_pin18= comboxPin_18.SelectedIndex ;
           wbDetection.pin.m_pin19= comboxPin_19.SelectedIndex ;
           wbDetection.pin.m_pin20= comboxPin_20.SelectedIndex ;
           wbDetection.pin.m_pin21= comboxPin_21.SelectedIndex ;
           wbDetection.pin.m_pin22= comboxPin_22.SelectedIndex ;
           wbDetection.pin.m_pin23= comboxPin_23.SelectedIndex ;
           wbDetection.pin.m_pin24= comboxPin_24.SelectedIndex ;
           wbDetection.pin.m_pin25= comboxPin_25.SelectedIndex ;
           wbDetection.pin.m_pin26 = comboxPin_26.SelectedIndex ;

           iReader.IniWriteValue("PIN", "PIN00", wbDetection.pin.m_pin1.ToString());
           iReader.IniWriteValue("PIN", "PIN01", wbDetection.pin.m_pin2.ToString());
           iReader.IniWriteValue("PIN", "PIN02", wbDetection.pin.m_pin3.ToString());
           iReader.IniWriteValue("PIN", "PIN03", wbDetection.pin.m_pin4.ToString());
           iReader.IniWriteValue("PIN", "PIN04", wbDetection.pin.m_pin5.ToString());
           iReader.IniWriteValue("PIN", "PIN05", wbDetection.pin.m_pin6.ToString());
           iReader.IniWriteValue("PIN", "PIN06", wbDetection.pin.m_pin7.ToString());
           iReader.IniWriteValue("PIN", "PIN07", wbDetection.pin.m_pin8.ToString());
           iReader.IniWriteValue("PIN", "PIN08", wbDetection.pin.m_pin9.ToString());
           iReader.IniWriteValue("PIN", "PIN09", wbDetection.pin.m_pin10.ToString());
           iReader.IniWriteValue("PIN", "PIN10", wbDetection.pin.m_pin11.ToString());
           iReader.IniWriteValue("PIN", "PIN11", wbDetection.pin.m_pin12.ToString());
           iReader.IniWriteValue("PIN", "PIN12", wbDetection.pin.m_pin13.ToString());
           iReader.IniWriteValue("PIN", "PIN13", wbDetection.pin.m_pin14.ToString());
           iReader.IniWriteValue("PIN", "PIN14", wbDetection.pin.m_pin15.ToString());
           iReader.IniWriteValue("PIN", "PIN15", wbDetection.pin.m_pin16.ToString());
           iReader.IniWriteValue("PIN", "PIN16", wbDetection.pin.m_pin17.ToString());
           iReader.IniWriteValue("PIN", "PIN17", wbDetection.pin.m_pin18.ToString());
           iReader.IniWriteValue("PIN", "PIN18", wbDetection.pin.m_pin19.ToString());
           iReader.IniWriteValue("PIN", "PIN19", wbDetection.pin.m_pin20.ToString());
           iReader.IniWriteValue("PIN", "PIN20", wbDetection.pin.m_pin21.ToString());
           iReader.IniWriteValue("PIN", "PIN21", wbDetection.pin.m_pin22.ToString());
           iReader.IniWriteValue("PIN", "PIN22", wbDetection.pin.m_pin23.ToString());
           iReader.IniWriteValue("PIN", "PIN23", wbDetection.pin.m_pin24.ToString());
           iReader.IniWriteValue("PIN", "PIN24", wbDetection.pin.m_pin25.ToString());
           iReader.IniWriteValue("PIN", "PIN25", wbDetection.pin.m_pin26.ToString());

            MessageBox.Show("保存PIN配置成功");

        }

        private void ShowParameterConfig()
        {
            txbHOTPIXEL_THRESHOLD.Text = wbDetection.m_sStainDefectConfig.m_sDefectCriterion.fDefect.ToString();
            txbHOTPIXEL_SORTNUM.Text = wbDetection.m_sStainDefectConfig.m_sDefectCriterion.fDefectMaxNumInDusty.ToString();
            txbHOTPIXEL_SPEC.Text = wbDetection.m_sStainDefectConfig.nHotPixel_Spec.ToString();

            txbBADPIXEL_THRESHOLD.Text = wbDetection.m_sStainDefectConfig.m_sDefectCriterion.iWDefect.ToString();
            txbBADPIXEL_SORTNUM.Text = wbDetection.m_sStainDefectConfig.m_sDefectCriterion.iWDefectMaxNumInDusty.ToString();
            txbBADPIXEL_SPEC.Text = wbDetection.m_sStainDefectConfig.nBadPixel_Spec.ToString();

            txbBinaryThreshold.Text = wbDetection.m_sStainDefectConfig.m_sDefectCriterion.BinaryThreshold.ToString();
            txbBinaryThreshold.Text = wbDetection.m_sStainDefectConfig.m_sStrainCriterion.BinaryThreshold.ToString();
            txbCIRCLE_R.Text = wbDetection.m_sStainDefectConfig.m_sDefectCriterion.dCircleOffset.ToString();
            txbCIRCLE_R.Text = wbDetection.m_sStainDefectConfig.m_sStrainCriterion.sCircleOffset.ToString();


            //污点
            txbLUX_DEEPBLEMISH.Text = wbDetection.m_sStainDefectConfig.m_sStrainCriterion.fLuxUpper.ToString();
            txbLUX_NoDEEPBLEMISH.Text = wbDetection.m_sStainDefectConfig.m_sStrainCriterion.fLuxLower.ToString();
            txbROISIZE.Text = wbDetection.m_sStainDefectConfig.m_sStrainCriterion.iJump.ToString();
            txbSIZENODEEP.Text = wbDetection.m_sStainDefectConfig.m_sStrainCriterion.lowStainNumInArea.ToString();
            txbSIZEDEEP.Text = wbDetection.m_sStainDefectConfig.m_sStrainCriterion.highStainNumInArea.ToString();

            //两个参数一样
            txbSPEC_NumDEEP.Text = wbDetection.m_sStainDefectConfig.m_sStrainCriterion.highStainAreaNum.ToString();
            txbSPEC_NumDEEP.Text = wbDetection.m_sStainDefectConfig.nDeepBlemish_Spec.ToString();
            txbSPEC_NumNODEEP.Text = wbDetection.m_sStainDefectConfig.m_sStrainCriterion.lowStainAreaNum.ToString();
            txbSPEC_NumNODEEP.Text = wbDetection.m_sStainDefectConfig.nLowBlemish_Spec.ToString();

            //全局变量
            ckbSavePhotho.Checked = SavePhtoto;
            ckbShowDefect.Checked = ShowDefect;
            ckbShowStain.Checked = ShowStain;
        }

        private void SaveParameterConfig()
        {
            wbDetection.m_sStainDefectConfig.m_sDefectCriterion.fDefect = float.Parse(this.txbHOTPIXEL_THRESHOLD.Text);
            wbDetection.m_sStainDefectConfig.m_sDefectCriterion.fDefectMaxNumInDusty = int.Parse(this.txbHOTPIXEL_SORTNUM.Text);
            wbDetection.m_sStainDefectConfig.nHotPixel_Spec = int.Parse(this.txbHOTPIXEL_SPEC.Text);

            wbDetection.m_sStainDefectConfig.m_sDefectCriterion.iWDefect = int.Parse(txbBADPIXEL_THRESHOLD.Text);
            wbDetection.m_sStainDefectConfig.m_sDefectCriterion.iWDefectMaxNumInDusty = int.Parse(txbBADPIXEL_SORTNUM.Text);
            wbDetection.m_sStainDefectConfig.nBadPixel_Spec = int.Parse(txbBADPIXEL_SPEC.Text);

            wbDetection.m_sStainDefectConfig.m_sDefectCriterion.BinaryThreshold = int.Parse(txbBinaryThreshold.Text);
            wbDetection.m_sStainDefectConfig.m_sStrainCriterion.BinaryThreshold = int.Parse(txbBinaryThreshold.Text);
            wbDetection.m_sStainDefectConfig.m_sDefectCriterion.dCircleOffset = int.Parse(txbCIRCLE_R.Text);
            wbDetection.m_sStainDefectConfig.m_sStrainCriterion.sCircleOffset = int.Parse(txbCIRCLE_R.Text);

            //污点
            wbDetection.m_sStainDefectConfig.m_sStrainCriterion.fLuxUpper = int.Parse(txbLUX_DEEPBLEMISH.Text);
            wbDetection.m_sStainDefectConfig.m_sStrainCriterion.fLuxLower = int.Parse(txbLUX_NoDEEPBLEMISH.Text);
            wbDetection.m_sStainDefectConfig.m_sStrainCriterion.iJump = int.Parse(txbROISIZE.Text);
            wbDetection.m_sStainDefectConfig.m_sStrainCriterion.lowStainNumInArea = int.Parse(txbSIZENODEEP.Text);
            wbDetection.m_sStainDefectConfig.m_sStrainCriterion.highStainNumInArea = int.Parse(txbSIZEDEEP.Text);

            //两个参数一样
            wbDetection.m_sStainDefectConfig.m_sStrainCriterion.highStainAreaNum = int.Parse(txbSPEC_NumDEEP.Text);
            wbDetection.m_sStainDefectConfig.nDeepBlemish_Spec = int.Parse(txbSPEC_NumDEEP.Text);
            wbDetection.m_sStainDefectConfig.m_sStrainCriterion.lowStainAreaNum = int.Parse(txbSPEC_NumNODEEP.Text);
            wbDetection.m_sStainDefectConfig.nLowBlemish_Spec = int.Parse(txbSPEC_NumNODEEP.Text);

            //全局参数
            ShowStain = ckbShowStain.Checked;
            ShowDefect = ckbShowDefect.Checked;
            SavePhtoto = ckbSavePhotho.Checked;

            string dirPath = Directory.GetCurrentDirectory();
            IniFile iReader = new IniFile(DefectConfigFilePath);

            iReader.IniWriteValue("StainDefect", "nBadPixel_Spec", wbDetection.m_sStainDefectConfig.nBadPixel_Spec.ToString());
            iReader.IniWriteValue("StainDefect", "nHotPixel_Spec", wbDetection.m_sStainDefectConfig.nHotPixel_Spec.ToString());
            iReader.IniWriteValue("StainDefect", "nDeepBlemish_Spec", wbDetection.m_sStainDefectConfig.nDeepBlemish_Spec.ToString());
            iReader.IniWriteValue("StainDefect", "nLowBlemish_Spec", wbDetection.m_sStainDefectConfig.nLowBlemish_Spec.ToString());

            iReader.IniWriteValue("StainDefect", "BinaryThreshold", wbDetection.m_sStainDefectConfig.m_sDefectCriterion.BinaryThreshold.ToString());
            iReader.IniWriteValue("StainDefect", "dCircleOffset", wbDetection.m_sStainDefectConfig.m_sDefectCriterion.dCircleOffset.ToString());
            iReader.IniWriteValue("StainDefect", "fDefect", wbDetection.m_sStainDefectConfig.m_sDefectCriterion.fDefect.ToString());
            iReader.IniWriteValue("StainDefect", "fDefectMaxNumInDusty", wbDetection.m_sStainDefectConfig.m_sDefectCriterion.fDefectMaxNumInDusty.ToString());
            iReader.IniWriteValue("StainDefect", "iWDefect", wbDetection.m_sStainDefectConfig.m_sDefectCriterion.iWDefect.ToString());
            iReader.IniWriteValue("StainDefect", "iWDefectMaxNumInDusty", wbDetection.m_sStainDefectConfig.m_sDefectCriterion.iWDefectMaxNumInDusty.ToString());

            iReader.IniWriteValue("StainDefect", "fLuxLower", wbDetection.m_sStainDefectConfig.m_sStrainCriterion.fLuxLower.ToString());
            iReader.IniWriteValue("StainDefect", "fLuxUpper", wbDetection.m_sStainDefectConfig.m_sStrainCriterion.fLuxUpper.ToString());
            iReader.IniWriteValue("StainDefect", "highStainAreaNum", wbDetection.m_sStainDefectConfig.m_sStrainCriterion.highStainAreaNum.ToString());
            iReader.IniWriteValue("StainDefect", "highStainNumInArea", wbDetection.m_sStainDefectConfig.m_sStrainCriterion.highStainNumInArea.ToString());
            iReader.IniWriteValue("StainDefect", "iJump", wbDetection.m_sStainDefectConfig.m_sStrainCriterion.iJump.ToString());
            iReader.IniWriteValue("StainDefect", "lowStainAreaNum", wbDetection.m_sStainDefectConfig.m_sStrainCriterion.lowStainAreaNum.ToString());
            iReader.IniWriteValue("StainDefect", "lowStainNumInArea", wbDetection.m_sStainDefectConfig.m_sStrainCriterion.lowStainNumInArea.ToString());

            iReader.IniWriteValue("wbGloble", "ShowStain", ShowStain.ToString());
            iReader.IniWriteValue("wbGloble", "ShowDefect", ShowDefect.ToString());
            iReader.IniWriteValue("wbGloble", "SavePhtoto", SavePhtoto.ToString());
        }


        private void btnPlay_Click(object sender, EventArgs e)
        {
            StartAllPlay();
        }
        public void StartAllPlay()
        {
            #region 初始化
            if (!this.OpenDevice()) { log.Debug("OpenDevice"); return; }
            if (DTCCM2_API.SetSoftPinPullUp(false, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("SetSoftPinPullUp  to down"); return; }
            byte[] pinDef = new byte[40];
            pinDef[0] = (byte)wbDetection.pin.m_pin1;
            pinDef[1] = (byte)wbDetection.pin.m_pin2;
            pinDef[2] = (byte)wbDetection.pin.m_pin3;
            pinDef[3] = (byte)wbDetection.pin.m_pin4;
            pinDef[4] = (byte)wbDetection.pin.m_pin5;
            pinDef[5] = (byte)wbDetection.pin.m_pin6;
            pinDef[6] = (byte)wbDetection.pin.m_pin7;
            pinDef[7] = (byte)wbDetection.pin.m_pin8;
            pinDef[8] = (byte)wbDetection.pin.m_pin9;
            pinDef[9] = (byte)wbDetection.pin.m_pin10;
            pinDef[10] = (byte)wbDetection.pin.m_pin11;
            pinDef[11] = (byte)wbDetection.pin.m_pin12;
            pinDef[12] = (byte)wbDetection.pin.m_pin13;
            pinDef[13] = (byte)wbDetection.pin.m_pin14;
            pinDef[14] = (byte)wbDetection.pin.m_pin15;
            pinDef[15] = (byte)wbDetection.pin.m_pin16;
            pinDef[16] = (byte)wbDetection.pin.m_pin17;
            pinDef[17] = (byte)wbDetection.pin.m_pin18;
            pinDef[18] = (byte)wbDetection.pin.m_pin19;
            pinDef[19] = (byte)wbDetection.pin.m_pin20;
            pinDef[20] = (byte)wbDetection.pin.m_pin21;
            pinDef[21] = (byte)wbDetection.pin.m_pin22;
            pinDef[22] = (byte)wbDetection.pin.m_pin23;
            pinDef[23] = (byte)wbDetection.pin.m_pin24;
            pinDef[24] = (byte)wbDetection.pin.m_pin25;
            pinDef[25] = (byte)wbDetection.pin.m_pin26;

            if (DTCCM2_API.SetSoftPin(pinDef, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("SetSoftPin to on"); return; }
            if (DTCCM2_API.EnableSoftPin(true, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("EnableSoftPin to on"); return; }
            if (DTCCM2_API.EnableGpio(true, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("EnableGpio to on"); return; }

            SENSOR_POWER[] Power = new SENSOR_POWER[5] { SENSOR_POWER.POWER_AVDD, SENSOR_POWER.POWER_DOVDD, SENSOR_POWER.POWER_DVDD, SENSOR_POWER.POWER_AFVCC, SENSOR_POWER.POWER_VPP };
            int[] Volt = new int[5] { 0, 0, 0, 0, 0 };
            bool[] OnOff = new bool[5] { true, true, true, true, true };
            CURRENT_RANGE[] range = new CURRENT_RANGE[10];
            int[] Current = new int[5] { 500, 500, 500, 500, 500 };

            int m_AVDD = 0;
            int m_DOVDD = 0;
            int m_CORE = 0;
            int m_AFVcc = 0;
            int m_Vpp = 0;

            m_AVDD = (int)CurrentSensor.avdd;
            m_DOVDD = (int)CurrentSensor.dovdd;
            m_CORE = (int)CurrentSensor.dvdd;
            m_AFVcc = (int)3300;
            m_Vpp = (int)7000;

            Power[0] = SENSOR_POWER.POWER_AVDD;
            Volt[0] = (int)m_AVDD; ; // 2.8V
            Current[0] = 500; // 300mA
            OnOff[0] = true;
            range[0] = CURRENT_RANGE.CURRENT_RANGE_MA;

            Power[1] = SENSOR_POWER.POWER_DOVDD;
            Volt[1] = (int)m_DOVDD; // 1.8V
            Current[1] = 500; // 300mA
            OnOff[1] = true;
            range[1] = CURRENT_RANGE.CURRENT_RANGE_MA;

            Power[2] = SENSOR_POWER.POWER_DVDD;
            Volt[2] = (int)m_CORE;// 1.2V
            Current[2] = 500;// 300mA
            OnOff[2] = true;
            range[2] = CURRENT_RANGE.CURRENT_RANGE_MA;

            Power[3] = SENSOR_POWER.POWER_AFVCC;
            Volt[3] = (int)m_AFVcc; // 2.8V
            Current[3] = 500; // 130mA
            OnOff[3] = true;
            range[3] = CURRENT_RANGE.CURRENT_RANGE_MA;

            Power[4] = SENSOR_POWER.POWER_VPP;
            Volt[4] = (int)(m_Vpp);
            Current[4] = 500; // 300mA
            OnOff[4] = true; //m_Vpp_On;
            range[4] = CURRENT_RANGE.CURRENT_RANGE_MA;

            if (DTCCM2_API.PmuSetVoltage(Power, Volt, 5, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("PmuSetVoltage to on"); return; }
            if (DTCCM2_API.PmuSetOnOff(Power, OnOff, 5, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("Error:电压开关设置失败"); return; }
            if (DTCCM2_API.PmuSetCurrentRange(Power, range, 5, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("PmuSetCurrentRange"); return; }
            if (DTCCM2_API.PmuSetOcpCurrentLimit(Power, Current, 5, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("PmuSetOcpCurrentLimit"); return; }

            if (CurrentSensor.port == 1)
            {
                uint paraCtrl = WhiteBoardDetection.PARA_AUTO_POL;
                if (CurrentSensor.pclk_pol == 1)
                {
                    paraCtrl = paraCtrl | WhiteBoardDetection.PARA_PCLK_RVS;
                }
                DTCCM2_API.SetParaCtrl(paraCtrl, this._deviceID);
            }

            if (DTCCM2_API.SetSensorClock(true, (ushort)(CurrentSensor.mclk * 10), this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("SetSensorClock"); }
            if (DTCCM2_API.SetSoftPinPullUp(true, _deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("SetSoftPinPullUp"); }
            if (DTCCM2_API.InitDevice(this.pictureBox1.Handle, CurrentSensor.width, CurrentSensor.height, CurrentSensor.port, CurrentSensor.type, 1, null, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("InitDevice"); }

            DTCCM2_API.SetSensorI2cRate(false, this._deviceID);
            DTCCM2_API.SensorEnable(Convert.ToByte(CurrentSensor.pin ^ 0x02), true, this._deviceID);
            Thread.Sleep(20);
            DTCCM2_API.SensorEnable(CurrentSensor.pin, true, this._deviceID);
            Thread.Sleep(50);
            int nLenght = paraListSize / 4;
            for (int i = 0; i < nLenght; i++)
            {
                ushort _slave = paraList[4 * i + 0];
                ushort _sReg = paraList[4 * i + 1];
                ushort _sValue = paraList[4 * i + 2];
                ushort _sIIMode = paraList[4 * i + 3];

                if (_sIIMode == 0xff) { Thread.Sleep(_sValue); }
                else
                {
                    if (DTCCM2_API.WriteSensorReg((byte)_slave, _sReg, _sValue, (byte)_sIIMode, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK)
                    {
                        if (DTCCM2_API.WriteSensorReg((byte)_slave, _sReg, _sValue, (byte)_sIIMode, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK)
                        {
                            log.Debug("WriteSensorReg");return;
                        }
                    }
                }
            }

            if (DTCCM2_API.ResetSensorI2cBus(this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("ResetSensorI2cBus"); return; }

            if (DTCCM2_API.SetSensorPort(CurrentSensor.port, CurrentSensor.width, CurrentSensor.height, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("SetSensorPort"); return; }

            if (CurrentSensor.type == WhiteBoardDetection.D_YUV || CurrentSensor.type == WhiteBoardDetection.D_YUV_SPI || CurrentSensor.type == WhiteBoardDetection.D_YUV_MTK_S)
            {
                DTCCM2_API.SetYUV422Format(CurrentSensor.outformat, this._deviceID);
            }
            else { DTCCM2_API.SetRawFormat(CurrentSensor.outformat, this._deviceID); }

            if (DTCCM2_API.InitRoi(0, 0, CurrentSensor.width, (ushort)(false ? CurrentSensor.height >> 1 : CurrentSensor.height), 0, 0, 1, 1, CurrentSensor.type, true, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("InitRoi"); return; }

            if (DTCCM2_API.CalculateGrabSize(ref this.grabSize, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("CalculateGrabSize"); return; }

            if (DTCCM2_API.OpenVideo(this.grabSize, this._deviceID) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("OpenVideo"); return; }

            if (DTCCM2_API.InitDisplay((uint)pictureBox1.Handle, this.CurrentSensor.width, this.CurrentSensor.height, this.CurrentSensor.type, 0x01, IntPtr.Zero) != WhiteBoardDetection.DT_ERROR_OK) { log.Debug("InitDisplay"); return; }

            #endregion
            #region 创建图像采集、图像显示线程，整个项目只会创建一个采集线程和一个显示线程


            ifinit1 = false;
            if (!ifinit1)
            {
               ifinit1 = true;

                if (m_pThread == null)
                {
                    m_b_thread_error = false;
                    ParameterizedThreadStart ParStart = new ParameterizedThreadStart(ImageThread);
                    m_pThread = new Thread(ParStart);
                    m_pThread.Start(this);
                    m_b_thread_start = true;
                }
                if (m_pDisplayThread == null)
                {
                    ParameterizedThreadStart ParDisplayStart = new ParameterizedThreadStart(ImageDisplayThread);
                    m_pDisplayThread = new Thread(ParDisplayStart);
                    m_DisplayThreadEvent.Reset();
                    m_pDisplayThread.Start(this);
                }
            }
            #endregion
        }

        bool ifinit1 = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            lblFram_OK.Text = m_FrameCnt.ToString();
            lblFram_ERROR.Text = m_DeviceErrCnt.ToString();
            lblFram_fps.Text = m_fFramefps.ToString("0.00");
            lblDisplay_fps.Text = m_Disaplayfps.ToString("0.00");
            timer1.Enabled = true;
        }

        private void btnTakePicture_Click(object sender, EventArgs e)
        {
            if (!m_b_thread_start) { MessageBox.Show("Camera未启动采集，请打开再保存图片"); return; }
            try
            {
                if (m_pDisplayThread != null){ m_DisplayThreadEvent.Reset(); }
                CaptureImageToFile(m_TripleBuffer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                if (m_pDisplayThread != null)
                    m_DisplayThreadEvent.Set();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            CloseAllPlay();
            this.pictureBox1.Image = null;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SavePinConfig();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //MIPI standard
            comboxPin_01.SelectedIndex = 20;
            comboxPin_02.SelectedIndex = 0;
            comboxPin_03.SelectedIndex = 2;
            comboxPin_04.SelectedIndex = 1;
            comboxPin_05.SelectedIndex = 3;
            comboxPin_06.SelectedIndex = 4;
            comboxPin_07.SelectedIndex = 5;
            comboxPin_08.SelectedIndex = 6;
            comboxPin_09.SelectedIndex = 7;
            comboxPin_10.SelectedIndex = 8;
            comboxPin_11.SelectedIndex = 9;
            comboxPin_12.SelectedIndex = 20;
            comboxPin_13.SelectedIndex = 10;
            comboxPin_14.SelectedIndex = 11;
            comboxPin_15.SelectedIndex = 12;
            comboxPin_16.SelectedIndex = 20;
            comboxPin_17.SelectedIndex = 20;
            comboxPin_18.SelectedIndex = 13;
            comboxPin_19.SelectedIndex = 15;
            comboxPin_20.SelectedIndex = 14;
            comboxPin_21.SelectedIndex = 19;
            comboxPin_22.SelectedIndex = 18;
            comboxPin_23.SelectedIndex = 21;
            comboxPin_24.SelectedIndex = 16;
            comboxPin_25.SelectedIndex = 20;
            comboxPin_26.SelectedIndex = 20;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Parallel standard
            comboxPin_01.SelectedIndex = 16;
            comboxPin_02.SelectedIndex = 0;
            comboxPin_03.SelectedIndex = 2;
            comboxPin_04.SelectedIndex = 1;
            comboxPin_05.SelectedIndex = 3;
            comboxPin_06.SelectedIndex = 4;
            comboxPin_07.SelectedIndex = 5;
            comboxPin_08.SelectedIndex = 6;
            comboxPin_09.SelectedIndex = 7;
            comboxPin_10.SelectedIndex = 8;
            comboxPin_11.SelectedIndex = 9;
            comboxPin_12.SelectedIndex = 20;
            comboxPin_13.SelectedIndex = 10;
            comboxPin_14.SelectedIndex = 11;
            comboxPin_15.SelectedIndex = 12;
            comboxPin_16.SelectedIndex = 20;
            comboxPin_17.SelectedIndex = 20;
            comboxPin_18.SelectedIndex = 20;
            comboxPin_19.SelectedIndex = 20;
            comboxPin_20.SelectedIndex = 20;
            comboxPin_21.SelectedIndex = 13;
            comboxPin_22.SelectedIndex = 20;
            comboxPin_23.SelectedIndex = 14;
            comboxPin_24.SelectedIndex = 15;
            comboxPin_25.SelectedIndex = 18;
            comboxPin_26.SelectedIndex = 19;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            ShowPinConfig();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                SaveParameterConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!m_b_thread_start) { MessageBox.Show("Camera未启动采集，请打开再保存图片"); return; }
            if (m_pDisplayThread != null)m_DisplayThreadEvent.Reset();

            Bitmap bmp =  CaptureImage(m_TripleBuffer);
            Desay_DefectTest(bmp);

            if (m_pDisplayThread != null) m_DisplayThreadEvent.Set();
        }

        private int Desay_DefectTest(Bitmap Bmp)
        {
            try
            {
                BitmapData bmdat = Bmp.LockBits(new Rectangle(Point.Empty, Bmp.Size), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                byte[] buffer = new byte[bmdat.Stride * bmdat.Height];

                Marshal.Copy(bmdat.Scan0, buffer, 0, buffer.Length);
                Bmp.UnlockBits(bmdat);

                byte[] YBuffer = new byte[CurrentSensor.width * CurrentSensor.height];

                int _B, _G, _R = 0;
                for (int i = 0; i < CurrentSensor.height; i++)
                {
                    for (int j = 0; j < CurrentSensor.width; j++)
                    {
                        int index = i * CurrentSensor.width + j;

                        _B = buffer[index * 3 + 0];
                        _G = buffer[index * 3 + 1];
                        _R = buffer[index * 3 + 2];
                        YBuffer[index] = (byte)(((_R * 299) + (_G * 587) + (_B * 114)) / 1000);
                    }
                }

                int size = Marshal.SizeOf(wbDetection.m_tagDefectAndStainInfo);//获取结构体占用空间大小
                IntPtr intptr_tagDefectAndStainInfo = Marshal.AllocHGlobal(size);//声明一个同样大小的空间
                Marshal.StructureToPtr(wbDetection.m_tagDefectAndStainInfo, intptr_tagDefectAndStainInfo, true);//将结构体放到这个空间

                size = Marshal.SizeOf(wbDetection.m_tagDefectResult);//获取结构体占用空间大小
                IntPtr intptr_tagDefectResult = Marshal.AllocHGlobal(size);//声明一个同样大小的空间
                Marshal.StructureToPtr(wbDetection.m_tagDefectResult, intptr_tagDefectResult, true);//将结构体放到这个空间中

                int res = WhiteBoardDetection.Desay_DefectTest(YBuffer, CurrentSensor.width, CurrentSensor.height, wbDetection.m_sStainDefectConfig.m_sDefectCriterion, intptr_tagDefectAndStainInfo, intptr_tagDefectResult);

                WhiteBoardDetection._tagDefectResult DefectResult = (WhiteBoardDetection._tagDefectResult)Marshal.PtrToStructure(intptr_tagDefectResult, typeof(WhiteBoardDetection._tagDefectResult));

                WhiteBoardDetection._tagDefectAndStainInfo DefectAndStainInfo = (WhiteBoardDetection._tagDefectAndStainInfo)Marshal.PtrToStructure(intptr_tagDefectAndStainInfo, typeof(WhiteBoardDetection._tagDefectAndStainInfo));
                Marking.WbData = $"\"{Config.Instance.MesWorkNum}\";\"WHITEBOARD_LOWBLEMISHNUM\";\"0\";\"0\";\"{DefectResult.DarkCount}\"" + ";\"COUNT\"" + ";\"N\"" + ";\"OK\"" + ";\r\n"
                                + $"\"{Config.Instance.MesWorkNum}\";\"WHITEBOARD_DEEPBLEMISHNUM\";\"0\";\"0\";\"{DefectResult.BrightCount}\"" + ";\"COUNT\"" + ";\"N\"" + ";\"OK\"" + ";\r\n";
                if (ShowDefect)
                {
                    ShowRectangle(DefectAndStainInfo.Center_x, DefectAndStainInfo.Center_y, DefectAndStainInfo.HorizontalSize, Pens.Blue, SavePhtoto, Bmp);
                }
                return res;
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message + ex.StackTrace);
                return 0;
            }
        }


        private void DefectTest_1()
        {
            try
            {
                if (m_pDisplayThread != null)
                    m_DisplayThreadEvent.Reset();

                byte[] YBuffer = new byte[CurrentSensor.width * CurrentSensor.height];
                //byte[] Bmp = new byte[CurrentSensor.width * CurrentSensor.height * 3];
                //Marshal.Copy(pBmp, Bmp, 0, Bmp.Length);

                //Bmp = SaveImage("‪2020_6_10_Wednesday_18_0_36_.bmp");

                int _B, _G, _R = 0;
                for (int i = 0; i < CurrentSensor.height; i++)
                {
                    for (int j = 0; j < CurrentSensor.width; j++)
                    {
                        int index = i * CurrentSensor.width + j;

                        _B = bmp[index * 3 + 0];
                        _G = bmp[index * 3 + 1];
                        _R = bmp[index * 3 + 2];
                        YBuffer[index] = (byte)(((_R * 299) + (_G * 587) + (_B * 114)) / 1000);
                    }
                }

                int size = Marshal.SizeOf(wbDetection.m_tagDefectAndStainInfo);//获取结构体占用空间大小
                IntPtr intptr_tagDefectAndStainInfo = Marshal.AllocHGlobal(size);//声明一个同样大小的空间
                Marshal.StructureToPtr(wbDetection.m_tagDefectAndStainInfo, intptr_tagDefectAndStainInfo, true);//将结构体放到这个空间

                size = Marshal.SizeOf(wbDetection.m_tagDefectResult);//获取结构体占用空间大小
                IntPtr intptr_tagDefectResult = Marshal.AllocHGlobal(size);//声明一个同样大小的空间
                Marshal.StructureToPtr(wbDetection.m_tagDefectResult, intptr_tagDefectResult, true);//将结构体放到这个空间中

                int res = WhiteBoardDetection.Desay_DefectTest(YBuffer, CurrentSensor.width, CurrentSensor.height, wbDetection.m_sStainDefectConfig.m_sDefectCriterion, intptr_tagDefectAndStainInfo, intptr_tagDefectResult);

                WhiteBoardDetection._tagDefectResult re = (WhiteBoardDetection._tagDefectResult)Marshal.PtrToStructure(intptr_tagDefectResult, typeof(WhiteBoardDetection._tagDefectResult));

                WhiteBoardDetection._tagDefectAndStainInfo DefectAndStainInfo = (WhiteBoardDetection._tagDefectAndStainInfo)Marshal.PtrToStructure(intptr_tagDefectAndStainInfo, typeof(WhiteBoardDetection._tagDefectAndStainInfo));


                //ShowRectangle(DefectAndStainInfo.Center_x, DefectAndStainInfo.Center_y, DefectAndStainInfo.HorizontalSize, Pens.Blue, SavePhtoto);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void Desay_StainTest_1()
        {
            try
            {
                if (m_pDisplayThread != null)
                    m_DisplayThreadEvent.Reset();

                byte[] YBuffer = new byte[CurrentSensor.width * CurrentSensor.height];
                int _B, _G, _R = 0;
                for (int i = 0; i < CurrentSensor.height; i++)
                {
                    for (int j = 0; j < CurrentSensor.width; j++)
                    {
                        int index = i * CurrentSensor.width + j;

                        _B = bmp[index * 3 + 0];
                        _G = bmp[index * 3 + 1];
                        _R = bmp[index * 3 + 2];
                        YBuffer[index] = (byte)(((_R * 299) + (_G * 587) + (_B * 114)) / 1000);
                    }
                }

                int size = Marshal.SizeOf(wbDetection.m_tagDefectAndStainInfo);//获取结构体占用空间大小
                IntPtr intptr_tagDefectAndStainInfo = Marshal.AllocHGlobal(size);//声明一个同样大小的空间
                Marshal.StructureToPtr(wbDetection.m_tagDefectAndStainInfo, intptr_tagDefectAndStainInfo, true);//将结构体放到这个空间

                size = Marshal.SizeOf(wbDetection.m_tagDefectResult);//获取结构体占用空间大小
                IntPtr intptr_tagDefectResult = Marshal.AllocHGlobal(size);//声明一个同样大小的空间
                Marshal.StructureToPtr(wbDetection.m_tagDefectResult, intptr_tagDefectResult, true);//将结构体放到这个空间中

                int res = WhiteBoardDetection.Desay_StainTest(YBuffer, CurrentSensor.width, CurrentSensor.height, wbDetection.m_sStainDefectConfig.m_sStrainCriterion, intptr_tagDefectAndStainInfo, intptr_tagDefectResult);

                WhiteBoardDetection._tagDefectResult re = (WhiteBoardDetection._tagDefectResult)Marshal.PtrToStructure(intptr_tagDefectResult, typeof(WhiteBoardDetection._tagDefectResult));

                WhiteBoardDetection._tagDefectAndStainInfo DefectAndStainInfo = (WhiteBoardDetection._tagDefectAndStainInfo)Marshal.PtrToStructure(intptr_tagDefectAndStainInfo, typeof(WhiteBoardDetection._tagDefectAndStainInfo));

                MessageBox.Show("污点总数：" + re.CountSum.ToString());
                

                //ShowRectangle(DefectAndStainInfo.Center_x, DefectAndStainInfo.Center_y,DefectAndStainInfo.HorizontalSize, Pens.Black, SavePhtoto);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private int Desay_StainTest(Bitmap Bmp)
        {
            try
            {
                BitmapData bmdat = Bmp.LockBits(new Rectangle(Point.Empty, Bmp.Size), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                byte[] buffer = new byte[bmdat.Stride * bmdat.Height];

                Marshal.Copy(bmdat.Scan0, buffer, 0, buffer.Length);
                Bmp.UnlockBits(bmdat);

                byte[] YBuffer = new byte[CurrentSensor.width * CurrentSensor.height];

                int _B, _G, _R = 0;
                for (int i = 0; i < CurrentSensor.height; i++)
                {
                    for (int j = 0; j < CurrentSensor.width; j++)
                    {
                        int index = i * CurrentSensor.width + j;

                        _B = buffer[index * 3 + 0];
                        _G = buffer[index * 3 + 1];
                        _R = buffer[index * 3 + 2];
                        YBuffer[index] = (byte)(((_R * 299) + (_G * 587) + (_B * 114)) / 1000);
                    }
                }

                int size = Marshal.SizeOf(wbDetection.m_tagDefectAndStainInfo);//获取结构体占用空间大小
                IntPtr intptr_tagDefectAndStainInfo = Marshal.AllocHGlobal(size);//声明一个同样大小的空间
                Marshal.StructureToPtr(wbDetection.m_tagDefectAndStainInfo, intptr_tagDefectAndStainInfo, true);//将结构体放到这个空间

                size = Marshal.SizeOf(wbDetection.m_tagDefectResult);//获取结构体占用空间大小
                IntPtr intptr_tagDefectResult = Marshal.AllocHGlobal(size);//声明一个同样大小的空间
                Marshal.StructureToPtr(wbDetection.m_tagDefectResult, intptr_tagDefectResult, true);//将结构体放到这个空间中

                int res = WhiteBoardDetection.Desay_StainTest(YBuffer, CurrentSensor.width, CurrentSensor.height, wbDetection.m_sStainDefectConfig.m_sStrainCriterion, intptr_tagDefectAndStainInfo, intptr_tagDefectResult);

                WhiteBoardDetection._tagDefectResult DefectResult = (WhiteBoardDetection._tagDefectResult)Marshal.PtrToStructure(intptr_tagDefectResult, typeof(WhiteBoardDetection._tagDefectResult));

                WhiteBoardDetection._tagDefectAndStainInfo DefectAndStainInfo = (WhiteBoardDetection._tagDefectAndStainInfo)Marshal.PtrToStructure(intptr_tagDefectAndStainInfo, typeof(WhiteBoardDetection._tagDefectAndStainInfo));
                Marking.WbData += $"\"{Config.Instance.MesWorkNum}\";\"WHITEBOARD_HOTPIXEL\";\"0\";\"2\";\"{DefectResult.DarkCount}\"" + ";\"COUNT\"" + ";\"N\"" + ";\"OK\"" + ";\r\n"
                + $"\"{Config.Instance.MesWorkNum}\";\"WHITEBOARD_BADPIXEL\";\"0\";\"2\";\"{DefectResult.BrightCount}\"" + ";\"COUNT\"" + ";\"N\"" + ";\"OK\"" + ";";
                if (ShowStain)
                {
                    ShowRectangle(DefectAndStainInfo.Center_x, DefectAndStainInfo.Center_y, DefectAndStainInfo.HorizontalSize, Pens.Red, SavePhtoto, Bmp);
                }

                return res;
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message + ex.StackTrace);
                return 0;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenBMP();
            if (bmp != null)
            {
                DefectTest_1();
                bmp = null;
            }
        }


        Graphics vGraphics;
        Point LeftTop = new Point(0, 0);
        Point RightTop = new Point(0, 0);
        Point LeftDown = new Point(0, 0);
        Point RightDowm = new Point(0, 0);
        int l = 0;

        private void ShowRectangle(int[] x,int[] y ,int[] range,Pen pen,bool save,Bitmap bmp)
        {
            pictureBox1.Image = bmp;
            vGraphics = Graphics.FromImage(pictureBox1.Image);

            for (int i = 0; i < x.Length; i++)
            {
                l = range[i] / 2;
                LeftTop.X = x[i] - l;
                LeftTop.Y = y[i] - l;

                RightTop.X = x[i] + l;
                RightTop.Y = y[i] - l;

                RightDowm.X = x[i] + l;
                RightDowm.Y = y[i] + l;

                LeftDown.X = x[i] - l;
                LeftDown.Y = y[i] + l;

                vGraphics.DrawLine(pen, LeftTop, RightTop);
                vGraphics.DrawLine(pen, RightTop, RightDowm);
                vGraphics.DrawLine(pen, RightDowm, LeftDown);
                vGraphics.DrawLine(pen, LeftDown, LeftTop);
                log.Debug("Center_x:" + x[i].ToString() + "," + "Center_y:" + y[i].ToString() + " 范围：" +  range[i].ToString());
            }



            pictureBox1.Invalidate();
            if (save)
            {
                string filename = DateTime.Now.Year.ToString() + "_"
                                + DateTime.Now.Month.ToString() + "_"
                                + DateTime.Now.Day.ToString() + "_"
                                + DateTime.Now.DayOfWeek.ToString() + "_"
                                + DateTime.Now.Hour.ToString() + "_"
                                + DateTime.Now.Minute.ToString() + "_"
                                + DateTime.Now.Second.ToString();
                pictureBox1.Image.Save(AppDomain.CurrentDomain.BaseDirectory + "//wb//" + filename+".bmp");
            }
        }

        private void OpenBMP()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "bmp图片(*.bmp)|*.*";
            bmp = null;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap Bmp = new Bitmap(Image.FromFile(dialog.FileName));  // 加载图像
                BitmapData bmdat = Bmp.LockBits(new Rectangle(Point.Empty, Bmp.Size), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);  // 锁定位图
                byte[] buffer = new byte[bmdat.Stride * bmdat.Height];  // 缓冲区，用来装载位图数据

                Marshal.Copy(bmdat.Scan0, buffer, 0, buffer.Length);  // 复制位图数据
                this.pictureBox1.Image = Bmp;//主界面显示
                Bmp.UnlockBits(bmdat);  // 解除锁定
                bmp = buffer;
            }
        }
        public byte[] SaveImage(String path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中
            return imgBytesIn;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenBMP();
            if (bmp != null)
            {
                Desay_StainTest_1();
                bmp = null;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (!m_b_thread_start) { MessageBox.Show("Camera未启动采集，请打开再保存图片"); return; }
            if (m_pDisplayThread != null)m_DisplayThreadEvent.Reset();
            Bitmap bmp = CaptureImage(m_TripleBuffer);
            Desay_StainTest(bmp);
            if (m_pDisplayThread != null)m_DisplayThreadEvent.Set();
        }

        private void button12_Click(object sender, EventArgs e)
        {
           int res = OneTest();
            if (res == 0)
            {
                lblCapImage.Text = "失败";
                lblBad.Text = "NG";
                lblStain.Text = "NG";
            }
            else
            {
                lblCapImage.Text = "成功";
            }
        }

        /// <summary>
        /// 每次点亮的方法，返回值：0（代表需要重新点亮）；1（代表NG）；2（代表OK）
        /// </summary>
        /// <returns></returns>
        public int OneTest()
        {
            m_b_thread_error = false;

            StartAllPlay();//启动点亮以及线程
            Thread.Sleep(2000);
            if ( m_b_thread_error ) { return 0; }

            if (m_pThread == null) { return 0; }

            if(m_pDisplayThread ==null) { return 0; }

            if (m_pDisplayThread != null) m_DisplayThreadEvent.Reset();

            Bitmap bmp = CaptureImage(m_TripleBuffer);
            if (bmp == null) return 0;

            int resDefect = Desay_DefectTest(bmp);
            int resStain = Desay_StainTest(bmp);
            if (m_pDisplayThread != null) m_DisplayThreadEvent.Set();
            CloseAllPlay();

            if (resDefect == 1) lblBad.Text = "OK";
            else lblBad.Text = "NG";

            if (resStain == 1) lblStain.Text = "OK";
            else lblStain.Text = "NG";

            return ((resDefect == 1) && (resStain == 1)) ? 2 : 1;
            
        }

        public void frmWhiteBorad_Load(object sender, EventArgs e)
        {

        }

        private void frmWhiteBorad_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible=false;
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Title = "请选择文件夹";
                dialog.Filter = "所有文件(*.*)|*.ini";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string file = dialog.FileName;
                    LoadWhiteBoardDetectionPra(file);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }
        #endregion

    }

}
