using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Toolkit.Interfaces;
using System.Windows.Forms;
using System.Toolkit;
using System.IO;
using System.Threading;

namespace System.Device
{
    public partial class HoneyWell1900 : SerialPort
    {
        #region 变量
        public delegate void EventReadComData(byte[] readbuffer);
        private EventReadComData _ReadComBuff = null;
        public volatile bool _keepreading;
        private Thread ReadComthread = null;


        public bool Flag { get; set; }

        public bool ShowMsg { get; set; }
        public string MsgShow { get; set; }
        public bool StringReceived { get; set; }
        public string ReceiveString { get; set; }
        public int Station { get; set; }

        public EventReadComData _OnReadComBuff
        {
            get
            {
                return _ReadComBuff;
            }

            set
            {
                _ReadComBuff = value;
            }
        }
        #endregion

        #region 构造函数

        public HoneyWell1900() : base() { }

        /// <summary>
        /// 使用指定的端口名称、波特率、奇偶校验位、数据位和停止位初始化 System.IO.Ports.SerialPort 类的新实例。
        /// </summary>
        /// <param name="portName">要使用的端口（例如 COM1）。</param>
        /// <param name="baudRate">波特率。</param>
        /// <param name="parity"> System.IO.Ports.SerialPort.Parity 值之一。</param>
        /// <param name="dataBits">数据位值。</param>
        /// <param name="stopBits">System.IO.Ports.SerialPort.StopBits 值之一。</param>
        public HoneyWell1900(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits) : base(portName, baudRate, parity, dataBits, stopBits) { }

        #endregion

        #region 方法

        private void ReadPort()
        {
            while (_keepreading)
            {
                if (IsOpen)
                {
                    if (BytesToRead > 0)
                    {
                        byte[] readbuffer = new byte[BytesToRead];
                        try
                        {
                            Application.DoEvents();
                            Read(readbuffer, 0, BytesToRead);
                            if (_ReadComBuff != null)
                                _ReadComBuff(readbuffer);
                            StringReceived = true;
                            ReceiveString = Encoding.Default.GetString(readbuffer);
                            ShowMsg = true;
                            Flag = true;
                            MsgShow = ReceiveString;
                            LogHelper.Debug(string.Format("读到字符为:{0}", readbuffer));
                        }
                        catch (Exception e)
                        {
                            ;
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }

        #endregion

        #region 接口

        public event DataReceiveCompleteEventHandler DeviceDataReceiveCompelete;

        public string Name { get; set; }
        public string ConnectionParam { get; set; }

        public void SetConnectionParam(string str)
        {
            ConnectionParam = str;
            string[] param = ConnectionParam.Split(',');
            PortName = param[0];
            BaudRate = int.Parse(param[1]);
            Parity = (Parity)Enum.Parse(typeof(Parity), param[2]);
            DataBits = int.Parse(param[3]);
            StopBits = (StopBits)Enum.Parse(typeof(StopBits), param[4]);
            ReadTimeout = int.Parse(param[5]);
            WriteTimeout = int.Parse(param[6]);
            RtsEnable = true;
            DtrEnable = true;
        }

        public void DeviceOpen()
        {
            try
            {
                if (IsOpen)
                    throw new Exception("设备已经连接\n");
                Open();
                _keepreading = true;
                ReadComthread = new Thread(new ThreadStart(ReadPort));
                ReadComthread.Start();
            }
            catch ( Exception ex)
            {

            }
        }

        public void DeviceClose()
        {
            try
            {
                if (IsOpen)
                {
                    Application.DoEvents();
                    Close();
                    _keepreading = false;
                    ReadComthread.Join();
                    ReadComthread = null;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
