using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Toolkit.Interfaces;

namespace System.Device
{
    public class Gryphon_GM4400 : SerialPort, ISerialPortTriggerModel
    {
        /*
         * Enter Service Mode $S<CR(0x0D)>
         * Exit Service Mode $Ar<CR(0x0D)>
         * OperatingModels - Serial Online - $CSNRM01<CR(0x0D)>
         * ManualTriggerControl - Enable - $CSOTM01<CR(0x0D)>
         * TRG - <STX(0x02)><CR(0x0D)>
         * STOP - <ETX(0x03)><CR(0x0D)>    
         */

        #region 变量

        string receiveString = "";

        #endregion

        #region 构造函数

        public Gryphon_GM4400() : base() { }

        /// <summary>
        /// 使用指定的端口名称、波特率、奇偶校验位、数据位和停止位初始化 System.IO.Ports.SerialPort 类的新实例。
        /// </summary>
        /// <param name="portName">要使用的端口（例如 COM1）。</param>
        /// <param name="baudRate">波特率。</param>
        /// <param name="parity"> System.IO.Ports.SerialPort.Parity 值之一。</param>
        /// <param name="dataBits">数据位值。</param>
        /// <param name="stopBits">System.IO.Ports.SerialPort.StopBits 值之一。</param>
        public Gryphon_GM4400(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits) : base(portName, baudRate, parity, dataBits, stopBits) { }

        #endregion

        #region 方法

        [ExecuteInfo("Initialize","初始化读码设备","Initialize")]
        public string Initialize()
        {
            byte step = 0;
            try
            {
                //进入服务模式
                Write("$S\r");
                CheckResult(ReadTo("\r"));

                step++;
                //更改波特率
                Close();
                BaudRate = 115200;

                step++;
                //设置波特率为115200
                Open();
                Write("$CR2BA07\r");
                CheckResult(ReadTo("\r"));

                step++;
                //设置触发模式为手动触发
                Write("$CSOTM01\r");
                CheckResult(ReadTo("\r"));

                step++;
                //设置模式为串口在线模式
                Write("$CSNRM01\r");
                CheckResult(ReadTo("\r"));

                step++;
                //设置触发字符串
                Write("$CSTON0200000000000000000000000000000000000000\r");
                CheckResult(ReadTo("\r"));

                step++;
                //设置停止字符串
                Write("$CSTOF0300000000000000000000000000000000000000\r");
                CheckResult(ReadTo("\r"));

                step++;
                //保存设置
                Write("$Ar\r");
                CheckResult(ReadTo("\r"));

                return "读码器初始化成功，请将波特率修改为115200，并点击保存按钮";
            }
            catch (Exception ex)
            {
                return UniversalFlags.errorStr + " Step: " + step.ToString() + " " + ex.Message;
            }
        }

        void CheckResult(string str)
        {
            if (!str.Contains("$>"))
                throw new Exception("读码器返回错误数据");
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
        }

        public void DeviceOpen()
        {
            if (IsOpen)
                throw new Exception("设备已经连接\n");
            Open();
        }

        public void DeviceClose()
        {
            if (IsOpen)
            {
                Application.DoEvents();
                Close();
            }
        }

        delegate void TriggerDelegate(TriggerArgs args);
        TriggerDelegate triggerMethod;
        public IAsyncResult TriggerResult;
        public IAsyncResult BeginTrigger(TriggerArgs args)
        {
            if (triggerMethod == null)
                triggerMethod = new TriggerDelegate(Trigger);

            if (TriggerResult != null && !TriggerResult.IsCompleted)
                return TriggerResult;

            TriggerResult = triggerMethod.BeginInvoke(args, null, null);

            return TriggerResult;
        }

        [ExecuteInfo("TRG", "读码器开始解码", "TRG")]
        public void Trigger(TriggerArgs args)
        {
            for (int i = 0; i < args.tryTimes; i++)
            {
                try
                {
                    StopTrigger();
                    receiveString = "";
                    DiscardInBuffer();
                    WriteLine(((char)0x02).ToString());
                    receiveString = ReadTo("\r");
                    break;
                }
                catch (Exception ex)
                {
                    receiveString = UniversalFlags.errorStr + ex.Message;
                }
            }
            StopTrigger();
            if (args.sender.GetType() != typeof(Gryphon_GFS4400))
            {
                if (DeviceDataReceiveCompelete != null)
                    DeviceDataReceiveCompelete(this, receiveString);
            }    
        }

        [ExecuteInfo("RST", "读码器停止解码", "RST")]
        public string StopTrigger()
        {
            try
            {
                WriteLine(((char)0x03).ToString());
                return UniversalFlags.successStr;
            }
            catch (Exception ex)
            {
                return UniversalFlags.errorStr + ex.Message;
            }
        }

        public string Execute(string cmd)
        {
            string result = "";
            cmd = cmd.ToUpper().Trim();
            if ("HELP" == cmd.Substring(0, 4))
            {
                Attribute[] attribs = typeof(Gryphon_GFS4400).GetMethods().Select(s =>
                    Attribute.GetCustomAttribute(s, typeof(ExecuteInfo))
                    ).Where(s => s != null).ToArray();

                result = "该设备支持以下指令：" + Environment.NewLine;
                foreach (Attribute attrib in attribs)
                {
                    ExecuteInfo exe = (ExecuteInfo)attrib;
                    result += Environment.NewLine + exe.Command + " - " + exe.Describe + Environment.NewLine
                        + "示例：" + exe.Example + Environment.NewLine;
                }
            }
            else if ("TRG" == cmd.Substring(0, 3))
            {
                Trigger(new TriggerArgs()
                {
                    sender = this,
                    tryTimes = 1
                });
                result = receiveString;
            }
            else if ("RST" == cmd.Substring(0, 3))
            {
                result = StopTrigger();
            }
            else if ("INITIALIZE" == cmd.Substring(0, 9))
            {
                result = Initialize();
            }
            else
            {
                result = "不支持指令" + cmd;
            }
            return result;
        }

        #endregion
    }    
}
