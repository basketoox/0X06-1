using BGAPI2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace desay
{
    public class BaumerCamera : ICamera
    {
        #region Interface
        public void Acquire()
        {
            GrabOneImage();
        }

        public ImageData OutputImageData
        {
            get
            {
                return new ImageData() { Data = this.m_pImgData, Width = m_nImageWidth, Height = m_nImageHeight, ImageFormat = m_sPixelFormat };
            }
        }

        private string serialNumber;
        public string SerialNumber
        {
            get { return serialNumber; }
        }

        private int timeOut = 2000;
        public int TimeOut
        {
            get
            {
                return timeOut;
            }
            set
            {
                timeOut = value;
            }
        }

        private string describe;
        public string Describe
        {
            get { return describe; }
        }
        public TriggerSource TriggerSource
        {
            get { return GetTriggerSource(); }
            set
            {
                SetTriggerSource(value);
            }
        }

        public string TriggerMode
        {
            get {return GetTriggerMode(TriggerMode); }
        }
        public TriggerActivation TriggerAction
        {
            get { return GetTriggerActivation(); }
            set
            {
                SetTriggerActivation(value);
            }
        }

        private double currentFrameRate;
        public double FrameRate
        {
            get { return currentFrameRate; }
        }
        private double currentFrameRateLoss;
        public double FrameRateLoss
        {
            get { return currentFrameRateLoss; }
        }

        public double ExposureTime
        {
            get
            {
                double exposure = 10;
                GetExposureTime(ref exposure);
                return exposure;
            }
            set
            {
                SetExposureTime(value);
            }
        }

        public double Gain
        {
            get
            {
                double gain = 1;
                GetGain(ref gain);
                return gain;
            }
            set
            {
                SetGain(value);
            }
        }

        public bool IsOpen
        {
            get { return bOpened; }
        }


        public bool Initialize(string serialNumber)
        {

            this.serialNumber = serialNumber;
            try
            {
                mDevice = BaumerCameraSystem.listCamera.Find((c) => c.strSN == serialNumber).pDevice;
                if (mDevice == null || mDevice.IsOpen)
                {
                    return false;
                }
                this.describe = mDevice.Model;
                mDevice.Open();

                bPersistentIP = true;
                if (mDevice.TLType == "GEV")
                {
                    if (bPersistentIP)
                    {
                        long iDevIPAddress = (long)mDevice.NodeList["GevDeviceIPAddress"].Value;
                        string strDevIpAddress = (iDevIPAddress >> 24).ToString() + "." +
                                                 ((iDevIPAddress & 0xffffff) >> 16).ToString() + "." +
                                                 ((iDevIPAddress & 0xffff) >> 8).ToString() + "." +
                                                 (iDevIPAddress & 0xff).ToString();

                        long iDevSubnetMask = (long)mDevice.NodeList["GevDeviceSubnetMask"].Value;
                        string strDevSubnetMask = (iDevSubnetMask >> 24).ToString() + "." +
                                                  ((iDevSubnetMask & 0xffffff) >> 16).ToString() + "." +
                                                  ((iDevSubnetMask & 0xffff) >> 8).ToString() + "." +
                                                  (iDevSubnetMask & 0xff).ToString();

                        long iPersistentIP = (long)mDevice.RemoteNodeList["GevPersistentIPAddress"].Value;
                        string strDevPersistentIpAddress = (iPersistentIP >> 24).ToString() + "." +
                                                           ((iPersistentIP & 0xffffff) >> 16).ToString() + "." +
                                                           ((iPersistentIP & 0xffff) >> 8).ToString() + "." +
                                                           (iPersistentIP & 0xff).ToString();


                        if (iPersistentIP != iDevIPAddress)
                        {
                            if (iPersistentIP > 100 && iDevSubnetMask > 1000)
                            {
                                mDevice.RemoteNodeList["GevCurrentIPConfigurationPersistentIP"].Value = true;
                                mDevice.RemoteNodeList["GevPersistentIPAddress"].Value = iDevIPAddress;
                                mDevice.RemoteNodeList["GevPersistentSubnetMask"].Value = iDevSubnetMask;
                            }

                            Thread.Sleep(1000);
                        }
                    }

                }

                mDevice.RemoteNodeList["AcquisitionStop"].Execute();

                mDatastreamList = mDevice.DataStreams;
                mDatastreamList.Refresh();

                foreach (KeyValuePair<string, BGAPI2.DataStream> dst_pair in mDatastreamList)
                {
                    dst_pair.Value.Open();

                    if (dst_pair.Key != "")
                    {
                        string sDataStreamID = dst_pair.Key;
                        mDataStream = mDatastreamList[sDataStreamID];
                        break;
                    }
                    else
                    {
                        mDevice.Close();
                        mDevice.Parent.Close();
                        return false;
                    }
                }
                mBufferList = mDataStream.BufferList;

                for (int i = 0; i < 10; i++)
                {
                    mBuffer = new BGAPI2.Buffer();
                    mBufferList.Add(mBuffer);
                }

                foreach (KeyValuePair<string, BGAPI2.Buffer> buf_pair in mBufferList)
                {
                    buf_pair.Value.QueueBuffer();
                }
                mDevice.RemoteNodeList["TriggerMode"].Value = "On";

                mDataStream.NewBufferEvent += new BGAPI2.Events.DataStreamEventControl.NewBufferEventHandler(mDataStream_NewBufferEvent);
                mDataStream.RegisterNewBufferEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);

                GrabComplete = new ManualResetEvent(false);
                mDataStream.AbortAcquisition();
                mDataStream.StartAcquisition();

                mDevice.RemoteNodeList["AcquisitionStart"].Execute();


                bOpened = true;

                return true;

            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public void Close()
        {
            Release();
        }
        public event GrabCompleted Ran
        {
            add { RanComplete += value; }
            remove { RanComplete -= value; }
        }

        #endregion

        private event GrabCompleted RanComplete;
        void OnRan(byte[] bytes, int width, int height, string format)
        {
            if (RanComplete != null)
            {
                RanComplete.Invoke(OutputImageData);
            }
        }

        public BGAPI2.Device mDevice = null;
        public BGAPI2.DataStreamList mDatastreamList = null;
        public BGAPI2.DataStream mDataStream = null;
        public BGAPI2.BufferList mBufferList = null;
        BGAPI2.Buffer mBuffer = null;

        public BGAPI2.ImageProcessor imgProcessor;
        public Byte[] m_pImgData = null;
        public int m_nImageWidth = 0;
        public int m_nImageHeight = 0;
        public string m_sPixelFormat = "";

        public int HTRGNum = 0;

        private object Mutex = new object();

        private bool bOpened = false;
        private bool bPersistentIP = false;
        public double AcqTime;

        ManualResetEvent GrabComplete;

        static public BaumerCameraSystem CameraSystem = new BaumerCameraSystem();

        public BaumerCamera()
        {
            imgProcessor = new BGAPI2.ImageProcessor();
            if (CameraSystem!=null)
            {
                CameraSystem.SearchAllCamera();
            }
            
        }
        public void mDataStream_NewBufferEvent(object sender, BGAPI2.Events.NewBufferEventArgs mDSEvent)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Restart();

                BGAPI2.Buffer mBufferFilled = null;

                mBufferFilled = mDSEvent.BufferObj;

                if (mBufferFilled == null)
                {

                }

                else if (mBufferFilled.IsIncomplete == true)
                {
                    mBufferFilled.QueueBuffer();
                }

                else
                {
                    int width = (int)mBufferFilled.Width;
                    int height = (int)mBufferFilled.Height;
                    m_nImageWidth = width;
                    m_nImageHeight = height;
                    m_sPixelFormat = mBufferFilled.PixelFormat;

                    if (mBufferFilled.PixelFormat == "Mono8")
                    {
                        ulong size = mBufferFilled.SizeFilled;

                        IntPtr pBuffer = mBufferFilled.MemPtr;

                        m_pImgData = new byte[width * height];
                        Marshal.Copy(pBuffer, m_pImgData, 0, width * height);
                    }

                    else //color
                    {
                        ulong size = mBufferFilled.SizeFilled;

                        BGAPI2.Image mImage = imgProcessor.CreateImage((uint)mBufferFilled.Width, (uint)mBufferFilled.Height, (string)mBufferFilled.PixelFormat, mBufferFilled.MemPtr, (ulong)mBufferFilled.MemSize);

                        //  BGAPI2.Image mImageTransfer = mImage.TransformImage("BGR8");
                        BGAPI2.Image mTransformImage = imgProcessor.CreateTransformedImage(mImage, "BGR8");
                        m_pImgData = new byte[(uint)((uint)mImage.Width * (uint)mImage.Height * 3.0)];
                        Marshal.Copy(mTransformImage.Buffer, m_pImgData, 0, (int)((int)mImage.Width * (int)mImage.Height * 3.0));
                  
                        if (mImage != null)
                        {
                            mImage.Release();
                            mTransformImage.Release();
                        }
                    }

                    string mode=mDevice.RemoteNodeList["TriggerSource"].Value;
                   
                    if (mode=="Line0")
                    {
                        HTRGNum++;
                    }
                    OnRan(m_pImgData, width, height, m_sPixelFormat);
                    GrabComplete.Set();

                    mBufferFilled.QueueBuffer();
                    sw.Stop();
                    AcqTime = sw.ElapsedMilliseconds;
                    //MessageBox.Show(AcqTime.ToString());
                
                }
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);
            }
        }


        public bool Release()
        {
            lock (Mutex)
            {

                try
                {
                    if (mDevice == null)
                    {
                        return true;
                    }

                    //STOP CAMERA
                    //if (mDevice.RemoteNodeList.GetNodePresent("AcquisitionAbort"))
                    //{
                    //    mDevice.RemoteNodeList["AcquisitionAbort"].Execute();
                    //}
                    //    mDevice.RemoteNodeList["AcquisitionStop"].Execute();

                    //STOP  DataStream acquisition
                    mDataStream.StopAcquisition();
                    mBufferList.DiscardAllBuffers();

                    //RESET EVENT MODE TO UNREGISTERED
                    mDataStream.UnregisterNewBufferEvent();
                    mDataStream.RegisterNewBufferEvent(BGAPI2.Events.EventMode.UNREGISTERED);

                    //Release buffers
                    while (mBufferList.Count > 0)
                    {
                        mBuffer = (BGAPI2.Buffer)mBufferList.Values.First();
                        mBufferList.RevokeBuffer(mBuffer);
                    }

                    //CLOSE
                    mDataStream.Close();
                    mDevice.Close();

                }
                catch (BGAPI2.Exceptions.IException ex)
                {
                    string str;
                    str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
               //     MessageBox.Show(str);
                    return false;
                }

                return true;
            }
        }

        Stopwatch gsw = new System.Diagnostics.Stopwatch();
        public bool GrabOneImage()
        {
            if (!bOpened)
            {
                return false;
            }

            try
            {
                GrabComplete.Reset();
                Stopwatch sw = new Stopwatch();
                sw.Restart();
               mDevice.RemoteNodeList["TriggerSoftware"].Execute();
              
                if (GrabComplete.WaitOne(timeOut))
                {
                    sw.Stop();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);
                return false;
            }

        }

        public bool SaveImage(string filePath, Byte[] data, int width, int height, string pixelformat)
        {
            if (/*!bOpened ||*/data.Length == 0)
            {
                return false;
            }

            try
            {
                if ("Mono8" == pixelformat)
                {
                    Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                    System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);


                    int offset = bmpData.Stride - bmpData.Width;        // 计算每行未用空间字节数
                    IntPtr ptr = bmpData.Scan0;                         // 获取首地址
                    int scanBytes = bmpData.Stride * bmpData.Height;    // 图像字节数 = 扫描字节数 * 高度
                    byte[] grayValues = new byte[scanBytes];            // 为图像数据分配内存


                    int posSrc = 0, posScan = 0;                        // dataValues和grayValues的索引
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            grayValues[posScan++] = data[posSrc++];
                        }
                        // 跳过图像数据每行未用空间的字节，length = stride - width * bytePerPixel
                        posScan += offset;
                    }

                    Marshal.Copy(grayValues, 0, ptr, scanBytes);
                    bitmap.UnlockBits(bmpData);

                    // 修改生成位图的索引表，从伪彩修改为灰度
                    System.Drawing.Imaging.ColorPalette palette;

                    using (Bitmap bmp = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format8bppIndexed))
                    {
                        palette = bmp.Palette;
                    }
                    for (int i = 0; i < 256; i++)
                    {
                        palette.Entries[i] = Color.FromArgb(i, i, i);
                    }
                    // 修改生成位图的索引表
                    bitmap.Palette = palette;
                    bitmap.Save(filePath);
                }

                else //color
                {
                    Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                    System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);


                    int stride = bmpData.Stride;  // 扫描线的宽度,比实际图片要大
                    int offset = stride - width * 3;  // 显示宽度与扫描线宽度的间隙
                    IntPtr ptr = bmpData.Scan0;   // 获取bmpData的内存起始位置的指针
                    int scanBytesLength = stride * height;  // 用stride宽度，表示这是内存区域的大小

                    Marshal.Copy(data, 0, ptr, scanBytesLength);
                    bitmap.UnlockBits(bmpData);  // 解锁内存区域

                    bitmap.Save(filePath);

                }

                return true;
            }

            catch (Exception)
            {
                return false;
            }

        }

        public bool SetExposureTime(double exposure)
        {
            if (mDevice == null)
            {
                return false;
            }
            try
            {
                if (mDevice.RemoteNodeList.GetNodePresent("ExposureTimeAbs"))
                {
                    mDevice.RemoteNodeList["ExposureTimeAbs"].Value = exposure;
                }
                else
                {
                    mDevice.RemoteNodeList["ExposureTime"].Value = exposure;
                }

                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public bool GetExposureTime(ref double exposure)
        {
            if (mDevice == null)
            {
                return false;
            }
            try
            {
                if (mDevice.RemoteNodeList.GetNodePresent("ExposureTimeAbs"))
                {
                    exposure = (double)mDevice.RemoteNodeList["ExposureTimeAbs"].Value;
                }
                else
                {
                    exposure = (double)mDevice.RemoteNodeList["ExposureTime"].Value;
                }
                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public bool GetExposureTimeRange(ref double min, ref double max)
        {
            if (mDevice == null)
            {
                return false;
            }
            try
            {
                if (mDevice.RemoteNodeList.GetNodePresent("ExposureTimeAbs"))
                {
                    min = (double)mDevice.RemoteNodeList["ExposureTimeAbs"].Min;
                    max = (double)mDevice.RemoteNodeList["ExposureTimeAbs"].Max;
                }
                else
                {
                    min = (double)mDevice.RemoteNodeList["ExposureTime"].Min;
                    max = (double)mDevice.RemoteNodeList["ExposureTime"].Max;
                }

                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public bool SetGain(double gain)
        {
            if (mDevice == null)
            {
                return false;
            }
            try
            {
                bool status = mDevice.RemoteNodeList.GetNodePresent("GainFactor");
                if (mDevice.RemoteNodeList.GetNodePresent("GainFactor"))
                {
                    mDevice.RemoteNodeList["GainFactor"].Value = gain;
                }
                else
                {
                    mDevice.RemoteNodeList["Gain"].Value = gain;
                }
                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public bool GetGain(ref double gain)
        {
            if (mDevice == null)
            {
                return false;
            }
            try
            {
                if (mDevice.RemoteNodeList.GetNodePresent("GainFactor"))
                {
                    gain = (double)mDevice.RemoteNodeList["GainFactor"].Value;
                }
                else
                {
                    gain = (double)mDevice.RemoteNodeList["GainAbs"].Value;
                    
                }
                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public bool GetGainRange(ref double min, ref double max)
        {
            if (mDevice == null)
            {
                return false;
            }
            try
            {
                if (mDevice.RemoteNodeList.GetNodePresent("GainFactor"))
                {
                    min = (double)mDevice.RemoteNodeList["GainFactor"].Min;
                    max = (double)mDevice.RemoteNodeList["GainFactor"].Max;
                }
                else
                {
                    min = (double)mDevice.RemoteNodeList["Gain"].Min;
                    max = (double)mDevice.RemoteNodeList["Gain"].Max;
                }
                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        /// <summary>
        /// format = {"Mono8", "BGR8", "BayerRG8", "RGB8"}
        /// </summary>
        public bool SetPixelFormat(string format)
        {
            if (mDevice == null)
            {
                return false;
            }
            try
            {
                mDataStream.StopAcquisition();
                mDevice.RemoteNodeList["AcquisitionStop"].Execute();

                if (mDevice.RemoteNodeList.GetNodePresent("PixelFormat") == true)
                {
                    mDevice.RemoteNodeList["PixelFormat"].Value = format;
                    mDataStream.StartAcquisition();
                    mDevice.RemoteNodeList["AcquisitionStart"].Execute();
                    return true;
                }
                else
                {
                    mDataStream.StartAcquisition();
                    mDevice.RemoteNodeList["AcquisitionStart"].Execute();
                    return false;
                }

            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                mDataStream.StartAcquisition();
                mDevice.RemoteNodeList["AcquisitionStart"].Execute();
                return false;
            }

        }

        public bool GetPixelFormat(ref string format)
        {
            try
            {
                if (mDevice.RemoteNodeList.GetNodePresent("PixelFormat") == true)
                {
                    format = (string)mDevice.RemoteNodeList["PixelFormat"].Value;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }

        }


        public bool SetTriggerActivation(TriggerActivation activation)
        {
            try
            {
                if (!mDevice.RemoteNodeList["TriggerActivation"].IsAvailable)
                {
                    return true; ;
                }

                //mDataStream.StopAcquisition();
                //mDevice.RemoteNodeList["AcquisitionStop"].Execute();
                mDevice.RemoteNodeList["TriggerActivation"].Value = Enum.GetName(typeof(TriggerActivation), activation);
                //}
                //mDataStream.StartAcquisition();
                //mDevice.RemoteNodeList["AcquisitionStart"].Execute();
                return true;
            }
            catch (BGAPI2.Exceptions.IException ex)
            {

                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public TriggerActivation GetTriggerActivation()
        {
            try
            {
                if (!mDevice.RemoteNodeList["TriggerActivation"].IsAvailable)
                {
                    return (TriggerActivation)Enum.Parse(typeof(TriggerActivation), "None");
                }

                return (TriggerActivation)Enum.Parse(typeof(TriggerActivation), mDevice.RemoteNodeList["TriggerActivation"].Value.ToString());
            }
            catch (BGAPI2.Exceptions.IException ex)
            {

                return 0;
            }
        }

        public bool SetTriggerSource(TriggerSource triggerSource)
        {
            try
            {
                if (triggerSource == TriggerSource.Software)
                {
                    if (mDevice.RemoteNodeList["TriggerSource"].EnumNodeList.GetNodePresent("SoftwareTrigger"))
                    {
                        mDevice.RemoteNodeList["TriggerSource"].Value = "SoftwareTrigger";
                    }
                    else
                    {
                        mDevice.RemoteNodeList["TriggerSource"].Value = "Software";
                    }
                    return true;
                }
                else if (triggerSource == TriggerSource.Hardware)
                {
                    mDevice.RemoteNodeList["TriggerSource"].Value = "Line0";
                }
                else if (triggerSource == TriggerSource.All)
                {
                    mDevice.RemoteNodeList["TriggerSource"].Value = "All";
                }
                else
                {
                    mDevice.RemoteNodeList["TriggerSource"].Value = "Off";
                }

                //mDataStream.StartAcquisition();
                //mDevice.RemoteNodeList["AcquisitionStart"].Execute();
                return true;

            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public TriggerSource GetTriggerSource()
        {

            string back = mDevice.RemoteNodeList["TriggerSource"].Value.ToString();
            switch (back)
            {
                case "All":
                    return (TriggerSource)Enum.Parse(typeof(TriggerSource), back);
                case "Software":
                    return (TriggerSource)Enum.Parse(typeof(TriggerSource), "Software");
                case "Line0":
                    return (TriggerSource)Enum.Parse(typeof(TriggerSource), "Hardware");
                case "SoftwareTrigger":
                    return (TriggerSource)Enum.Parse(typeof(TriggerSource), "Software");
                default:
                    return (TriggerSource)Enum.Parse(typeof(TriggerSource), back);
            }
            // return (TriggerSource)Enum.Parse(typeof(TriggerSource), mDevice.RemoteNodeList["TriggerSource"].Value.ToString() == "Software" ? "Software" : "Hardware");

        }

        public List<string> GetAllNodeName()
        {
            List<string> names = new List<string>();
            for (ulong i = 0; i < mDevice.RemoteNodeList.Count; i++)
            {
                names.Add(mDevice.RemoteNodeList[i].Name);
            }
            return names;
        }

        public string GetTriggerMode(string mode)
        {
            if (mDevice!=null)
      	{
		 mode= mDevice.RemoteNodeList["TriggerMode"].Value;
	    }
            return mode;
        }


        public bool SetTriggerMode(string mode)
        {
            try
            {

                mDevice.RemoteNodeList["TriggerMode"].Value = mode;

                return true;

            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        #region PartialScan
        public bool SetPartialScan(int width, int height, int offsetX, int offsetY)
        {
            try
            {
                mDevice.RemoteNodeList["AcquisitionStop"].Execute();
                mDataStream.StopAcquisition();

                //IMAGE WIDTH
                int iImageWidth = (int)mDevice.RemoteNodeList["Width"].Value;
                int iImageWidthMin = (int)mDevice.RemoteNodeList["Width"].Min;
                int iImageWidthMax = (int)mDevice.RemoteNodeList["Width"].Max;
                int iImageWidthInc = (int)mDevice.RemoteNodeList["Width"].Inc;

                int widthvalue = width / iImageWidthInc * iImageWidthInc;

                if (widthvalue < iImageWidthMin)
                    widthvalue = iImageWidthMin;

                if (widthvalue > iImageWidthMax)
                    widthvalue = iImageWidthMax;

                mDevice.RemoteNodeList["Width"].Value = widthvalue;


                //IMAGE OFFSET X
                int iImageOffsetX = (int)mDevice.RemoteNodeList["OffsetX"].Value;
                int iImageOffsetXMin = (int)mDevice.RemoteNodeList["OffsetX"].Min;
                int iImageOffsetXMax = (int)mDevice.RemoteNodeList["OffsetX"].Max;
                int iImageOffsetXInc = (int)mDevice.RemoteNodeList["OffsetX"].Inc;

                int OffsetXvalue = offsetX / iImageOffsetXInc * iImageOffsetXInc;

                if (OffsetXvalue < iImageOffsetXMin)
                    OffsetXvalue = iImageOffsetXMin;

                if (OffsetXvalue > iImageOffsetXMax)
                    OffsetXvalue = iImageOffsetXMax;

                mDevice.RemoteNodeList["OffsetX"].Value = OffsetXvalue;


                //IMAGE HEIGHT
                int iImageHeight = (int)mDevice.RemoteNodeList["Height"].Value;
                int iImageHeightMin = (int)mDevice.RemoteNodeList["Height"].Min;
                int iImageHeightMax = (int)mDevice.RemoteNodeList["Height"].Max;
                int iImageHeightInc = (int)mDevice.RemoteNodeList["Height"].Inc;

                int heightvalue = height / iImageHeightInc * iImageHeightInc;

                if (heightvalue < iImageHeightMin)
                    heightvalue = iImageHeightMin;

                if (heightvalue > iImageHeightMax)
                    heightvalue = iImageHeightMax;

                mDevice.RemoteNodeList["Height"].Value = heightvalue;


                //IMAGE OFFSET Y
                int iImageOffsetY = (int)mDevice.RemoteNodeList["OffsetY"].Value;
                int iImageOffsetYMin = (int)mDevice.RemoteNodeList["OffsetY"].Min;
                int iImageOffsetYMax = (int)mDevice.RemoteNodeList["OffsetY"].Max;
                int iImageOffsetYInc = (int)mDevice.RemoteNodeList["OffsetY"].Inc;

                int OffsetYvalue = offsetY / iImageOffsetYInc * iImageOffsetYInc;

                if (OffsetYvalue < iImageOffsetYMin)
                    OffsetYvalue = iImageOffsetYMin;

                if (OffsetYvalue > iImageOffsetYMax)
                    OffsetYvalue = iImageOffsetYMax;

                mDevice.RemoteNodeList["OffsetY"].Value = OffsetYvalue;


                mDataStream.StartAcquisition();
                mDevice.RemoteNodeList["AcquisitionStart"].Execute();

                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                mDataStream.StartAcquisition();
                mDevice.RemoteNodeList["AcquisitionStart"].Execute();
                return false;
            }

        }

        public bool ResetScanParameter()
        {
            try
            {
                mDevice.RemoteNodeList["AcquisitionStop"].Execute();
                mDataStream.StopAcquisition();

                mDevice.RemoteNodeList["OffsetY"].Value = mDevice.RemoteNodeList["OffsetY"].Min;
                mDevice.RemoteNodeList["Height"].Value = mDevice.RemoteNodeList["Height"].Max;
                mDevice.RemoteNodeList["OffsetX"].Value = mDevice.RemoteNodeList["OffsetX"].Min;
                mDevice.RemoteNodeList["Width"].Value = mDevice.RemoteNodeList["Width"].Max;

                mDataStream.StartAcquisition();
                mDevice.RemoteNodeList["AcquisitionStart"].Execute();

                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                mDataStream.StartAcquisition();
                mDevice.RemoteNodeList["AcquisitionStart"].Execute();
                return false;
            }
        }

        public bool GetScanParameter(ref int width, ref int height, ref int offsetX, ref int offsetY)
        {
            try
            {
                width = (int)mDevice.RemoteNodeList["Width"].Value;
                height = (int)mDevice.RemoteNodeList["Height"].Value;
                offsetX = (int)mDevice.RemoteNodeList["OffsetX"].Value;
                offsetY = (int)mDevice.RemoteNodeList["OffsetY"].Value;

                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public bool GetImageWidthRange(ref int min, ref int max, ref int step)
        {
            try
            {
                min = (int)mDevice.RemoteNodeList["Width"].Min;
                max = (int)mDevice.RemoteNodeList["Width"].Max;
                step = (int)mDevice.RemoteNodeList["Width"].Inc;

                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public bool GetImageHeightRange(ref int min, ref int max, ref int step)
        {
            try
            {
                min = (int)mDevice.RemoteNodeList["Height"].Min;
                max = (int)mDevice.RemoteNodeList["Height"].Max;
                step = (int)mDevice.RemoteNodeList["Height"].Inc;

                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public bool GetImageOffsetXRange(ref int min, ref int max, ref int step)
        {
            try
            {
                min = (int)mDevice.RemoteNodeList["OffsetX"].Min;
                max = (int)mDevice.RemoteNodeList["OffsetX"].Max;
                step = (int)mDevice.RemoteNodeList["OffsetX"].Inc;

                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public bool GetImageOffsetYRange(ref int min, ref int max, ref int step)
        {
            try
            {
                min = (int)mDevice.RemoteNodeList["OffsetY"].Min;
                max = (int)mDevice.RemoteNodeList["OffsetY"].Max;
                step = (int)mDevice.RemoteNodeList["OffsetY"].Inc;

                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }
        #endregion

        #region Statistics

        private bool GetFrameRate()
        {
            try
            {
                this.currentFrameRate = (double)mDevice.RemoteNodeList["AcquisitionFrameRate"].Value;

                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }

        }
        public bool ResetStatistics()
        {
            try
            {
                mDataStream.NodeList["Reset"].Execute();

                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public bool GetReceivedGoodFrames(ref int num)
        {
            try
            {
                num = (int)mDataStream.NodeList["GoodFrames"].Value;

                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public bool GetReceivedCorruptedFrames(ref int num)
        {
            try
            {
                num = (int)mDataStream.NodeList["CorruptedFrames"].Value;

                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }


        public bool GetLostFrames(ref int num)
        {
            try
            {
                num = Convert.ToInt32(mDataStream.NodeList["LostFrames"].Value.ToString());
                this.currentFrameRateLoss = num;
                return true;
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }
        #endregion

        #region Colour Camera
        /// <summary>
        /// chanel = {"Red", "Blue", "GreenBlue", "GreenRed"}
        /// </summary>
        public bool SetBalanceParameter(string chanel, double gain)
        {
            try
            {
                if (mDevice.RemoteNodeList.GetNodePresent("GainSelector") == true)
                {
                    mDevice.RemoteNodeList["GainSelector"].Value = chanel;
                    mDevice.RemoteNodeList["Gain"].Value = gain;
                    mDevice.RemoteNodeList["GainSelector"].Value = "All";
                    return true;
                }
                else
                {
                    return false;
                }

            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        public bool GetBalanceParameter(string chanel, ref double gain)
        {
            try
            {
                if (mDevice.RemoteNodeList.GetNodePresent("GainSelector") == true)
                {
                    mDevice.RemoteNodeList["GainSelector"].Value = chanel;
                    gain = (double)mDevice.RemoteNodeList["Gain"].Value;
                    mDevice.RemoteNodeList["GainSelector"].Value = "All";
                    return true;
                }
                else
                {
                    return false;
                }

            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }

        /// <summary>
        /// method = {"NearestNeighbor",  "Bilinear3x3",  "Baumer5x5"}
        /// </summary>
        public bool SetBayerFilterMethod(string method)
        {
            try
            {
                //    if (imgProcessor.NodeList.GetNodePresent("DemosaicingMethod") == true)
                //    {
                //        imgProcessor.NodeList["DemosaicingMethod"].Value = method;
                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }

            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                //  MessageBox.Show(str);

                return false;
            }
            return false;
        }

        public bool GetBayerFilterMethod(ref string method)
        {
            try
            {
                //if (imgProcessor.NodeList.GetNodePresent("DemosaicingMethod") == true)
                //{
                //    method = imgProcessor.NodeList["DemosaicingMethod"].Value;

                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
            return false;
        }

        public bool DoWhiteBalance()
        {
            try
            {
                if (mDevice.RemoteNodeList.GetNodePresent("BalanceWhiteAuto") == true)
                {
                    mDevice.RemoteNodeList["BalanceWhiteAuto"].Value = "Continuous";

                    for (int i = 0; i < 10; i++)
                    {
                        GrabOneImage();
                        //SaveImage("D:\\Image_" + i.ToString() + ".bmp", m_pImgData, m_nImageWidth, m_nImageHeight, m_sPixelFormat);
                    }

                    mDevice.RemoteNodeList["BalanceWhiteAuto"].Value = "Off";

                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (BGAPI2.Exceptions.IException ex)
            {
                string str;
                str = string.Format("ExceptionType:{0}! ErrorDescription:{1} in function:{2}", ex.GetType(), ex.GetErrorDescription(), ex.GetFunctionName());
                MessageBox.Show(str);

                return false;
            }
        }
        #endregion

    }
}
