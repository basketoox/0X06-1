using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Toolkit.Interfaces;
using System.Windows.Forms;
using System.Toolkit;
using System.IO;
using System.Threading;

namespace System.Device
{
    public partial class Panasonic : SerialPort
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

        public Panasonic() : base() { }

        /// <summary>
        /// 使用指定的端口名称、波特率、奇偶校验位、数据位和停止位初始化 System.IO.Ports.SerialPort 类的新实例。
        /// </summary>
        /// <param name="portName">要使用的端口（例如 COM1）。</param>
        /// <param name="baudRate">波特率。</param>
        /// <param name="parity"> System.IO.Ports.SerialPort.Parity 值之一。</param>
        /// <param name="dataBits">数据位值。</param>
        /// <param name="stopBits">System.IO.Ports.SerialPort.StopBits 值之一。</param>
        public Panasonic(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits) : base(portName, baudRate, parity, dataBits, stopBits) { }

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
                MessageBox.Show(ex.ToString());
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

        /// <summary>
        /// 写入数据至松下PLC数据区域（data area）指令
        /// </summary>
        /// <param name="strData">需写入的数据</param>
        /// <param name="nBegPos">开始写入数据的首地址</param>
        /// <param name="nLength">待写入数据的地址长度</param>
        /// <param name="_MySerialPort">打开的串口对象</param>
        public void WriteRegister(string strData, int nBegPos, int nLength)
        {
            try
            {
                if (!IsOpen) return;
                string strCommand = "%" + Station.ToString("X2") + "#WD" + "D" + nBegPos.ToString("D5")
                    + (nBegPos + nLength - 1).ToString("D5") + strData;
                strCommand += GetBccCode(Encoding.Default.GetBytes(strCommand)) + (char)13;
                LogHelper.Debug("写入数据的指令为：" + strCommand);
                Write(Encoding.Default.GetBytes(strCommand), 0, strCommand.Length);//使用串口发送数据}
            }
            catch
            {
                LogHelper.Debug("数据写入失败，写入数据为：" + strData);
                return;
            }
        }

        public void WriteData(string strData, int nBegPos, int nLength)
        {
            if (!IsOpen) return;
            int nIndex = nLength / 24;
            int nNum = nLength % 24;
            for (int i = 0; i < nIndex; i++)
                WriteRegister(strData.Substring(0 + i * 96, 96), nBegPos + i * 24, 24);
            if (nNum > 0)
                WriteRegister(strData.Substring(0 + nIndex * 96, nNum * 4), nBegPos + nIndex * 24, nNum);
        }

        public void WriteDetectHeightCommand()
        {
            try
            {
                Station = 1;
                string strCommand = "%" + Station.ToString("X2") + "#RMD**" + (char)13;
                LogHelper.Debug("写入数据的指令为：" + strCommand);
                Write(Encoding.Default.GetBytes(strCommand), 0, strCommand.Length);//使用串口发送数据}
                ShowMsg = true;
                Flag = false;
                MsgShow = strCommand;
            }
            catch(Exception ex)
            {
                LogHelper.Debug("发送读取数据指令失败" );
                MessageBox.Show(ex.Message + ex.StackTrace);
                return;
            }
        }

        /// <summary>
        /// 读取松下PLC数据区域（data area）指令
        /// </summary>
        /// <param name="nBegPos">待读取数据的首地址</param>
        /// <param name="nLength">待读取数据的地址长度</param>
        /// <param name="_MySerialPort">打开的串口对象</param>
        public void ReadRegister(int nBegPos, int nLength)
        {
            try
            {
                if (!IsOpen) return;
                string strCommand = "%" + Station.ToString("X2") + "#RD" + "D" + nBegPos.ToString("D5")
                    + (nBegPos + nLength - 1).ToString("D5");//具体地址待修改
                strCommand += GetBccCode(Encoding.Default.GetBytes(strCommand)) + (char)13;
                //log.Debug("读取PLC的数据的指令为：" + strCommand);
                Write(Encoding.Default.GetBytes(strCommand), 0, strCommand.Length);//使用串口发送数据}
            }
            catch
            {
                LogHelper.Debug(string.Format("读取数据失败，读取地址为{0},地址长度为{1}", nBegPos, nLength));
                return;
            }
        }

        private List<char> hexCharList = new List<char>()
            {
                '0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'
            };

        /// <summary>
        /// BCC和校验代码返回16进制
        /// </summary>
        /// <param name="data">需要校验的数据包</param>
        /// <returns></returns>
        public string GetBccCode(byte[] data)
        {
            byte CheckCode = 0;
            CheckCode = data[0];
            for (int i = 1; i < data.Length; i++)
            {
                CheckCode ^= data[i];
            }
            //return Convert.ToString(CheckCode, 16).ToUpper();
            return ByteToHexString(new byte[] { CheckCode }, (char)0);
        }

        /// <summary>
        /// 计算CRC
        /// </summary>
        /// <param name="sb">待计算的字符串</param>
        /// <returns></returns>
        private string CalculateCrc(StringBuilder sb)
        {
            byte tmp = 0;
            tmp = (byte)sb[0];
            for (int i = 1; i < sb.Length; i++)
            {
                tmp ^= (byte)sb[i];
            }
            return ByteToHexString(new byte[] { tmp }, (char)0);
        }

        /// <summary>
        /// 处理收到的松下指令反馈信息
        /// </summary>
        /// <param name="strRecv"></param>
        /// <returns></returns>
        public string DealRecvData(string strRecv, ref double height)
        {
            height = -38.72;
            try
            {
                if (string.IsNullOrEmpty(strRecv))
                    return "接收到的数据为空!";
                if (strRecv.Length < 8)
                    return "接收的数据长度不够!";
                string strTemp = strRecv.Substring(3, 1);
                switch (strTemp)
                {
                    case "$":
                        strTemp = strRecv.Substring(4, 3);
                        if (strTemp.Equals("RMD"))
                        {
                            strTemp = strRecv.Substring(7, 8);
                            height = Convert.ToDouble(strTemp);
                            height /= 10000.0;
                        }
                        return "数据接收正常!";
                    case "!":
                        strTemp = strRecv.Substring(4, 2);
                        return "数据接收异常，异常代码为：" + strTemp;
                    default:
                        break;
                }
                return "收到的数据无法识别!";
            }
            catch
            {
                return "处理接收到的数据异常！";
            }
        }

        /// <summary>
        /// 将指定的字符串按指定长度分解成指定数量的子字符串
        /// </summary>
        /// <param name="oldStr">源字符串</param>
        /// <param name="nLength">子字符串长度</param>
        /// <param name="newStr">子字符串数组</param>
        /// <param name="nSize">子字符串数量</param>
        /// <returns>返回提示信息</returns>
        public string StringTruncat(string oldStr, int nLength, ref string[] newStr, int nSize)
        {
            try
            {
                if (string.IsNullOrEmpty(oldStr))
                    return "待分解的字符串不能为空!";
                if (nLength * nSize != oldStr.Length)
                    return "待分解字符串与需求的子字符串数组大小不符!";
                for (int i = 0; i < nSize; i++)
                {
                    newStr[i] = oldStr.Substring(i * nLength, nLength);
                }
                return "字符串解析完成!";
            }
            catch
            {
                return "字符串分解异常！";
            }
        }

        /// <summary>
        /// 将16进制的字符串转化成Byte数据，将检测每2个字符转化，也就是说，中间可以是任意字符 ->
        /// Converts a 16-character string into byte data, which will detect every 2 characters converted, that is, the middle can be any character
        /// </summary>
        /// <param name="hex">十六进制的字符串，中间可以是任意的分隔符</param>
        /// <returns>转换后的字节数组</returns>
        /// <remarks>参数举例：AA 01 34 A8</remarks>
        /// <example>
        /// <code lang="cs" source="HslCommunication_Net45.Test\Documentation\Samples\BasicFramework\SoftBasicExample.cs" region="HexStringToBytesExample" title="HexStringToBytes示例" />
        /// </example>
        public byte[] HexStringToBytes(string hex)
        {
            hex = hex.ToUpper();

            MemoryStream ms = new MemoryStream();

            for (int i = 0; i < hex.Length; i++)
            {
                if ((i + 1) < hex.Length)
                {
                    if (hexCharList.Contains(hex[i]) && hexCharList.Contains(hex[i + 1]))
                    {
                        // 这是一个合格的字节数据
                        ms.WriteByte((byte)(hexCharList.IndexOf(hex[i]) * 16 + hexCharList.IndexOf(hex[i + 1])));
                        i++;
                    }
                }
            }

            byte[] result = ms.ToArray();
            ms.Dispose();
            return result;
        }

        /// <summary>
        /// 将bool数组转换到byte数组 ->
        /// Converting a bool array to a byte array
        /// </summary>
        /// <param name="array">bool数组</param>
        /// <returns>转换后的字节数组</returns>
        /// <example>
        /// <code lang="cs" source="HslCommunication_Net45.Test\Documentation\Samples\BasicFramework\SoftBasicExample.cs" region="BoolArrayToByte" title="BoolArrayToByte示例" />
        /// </example>
        public static byte[] BoolArrayToByte(bool[] array)
        {
            if (array == null) return null;

            int length = array.Length % 8 == 0 ? array.Length / 8 : array.Length / 8 + 1;
            byte[] buffer = new byte[length];

            for (int i = 0; i < array.Length; i++)
            {
                int index = i / 8;
                int offect = i % 8;

                byte temp = 0;
                switch (offect)
                {
                    case 0: temp = 0x01; break;
                    case 1: temp = 0x02; break;
                    case 2: temp = 0x04; break;
                    case 3: temp = 0x08; break;
                    case 4: temp = 0x10; break;
                    case 5: temp = 0x20; break;
                    case 6: temp = 0x40; break;
                    case 7: temp = 0x80; break;
                    default: break;
                }

                if (array[i]) buffer[index] += temp;
            }

            return buffer;
        }

        /// <summary>
        /// 从Byte数组中提取位数组，length代表位数 ->
        /// Extracts a bit array from a byte array, length represents the number of digits
        /// </summary>
        /// <param name="InBytes">原先的字节数组</param>
        /// <param name="length">想要转换的长度，如果超出自动会缩小到数组最大长度</param>
        /// <returns>转换后的bool数组</returns>
        /// <example>
        /// <code lang="cs" source="HslCommunication_Net45.Test\Documentation\Samples\BasicFramework\SoftBasicExample.cs" region="ByteToBoolArray" title="ByteToBoolArray示例" />
        /// </example> 
        public static bool[] ByteToBoolArray(byte[] InBytes, int length)
        {
            if (InBytes == null) return null;

            if (length > InBytes.Length * 8) length = InBytes.Length * 8;
            bool[] buffer = new bool[length];

            for (int i = 0; i < length; i++)
            {
                int index = i / 8;
                int offect = i % 8;

                byte temp = 0;
                switch (offect)
                {
                    case 0: temp = 0x01; break;
                    case 1: temp = 0x02; break;
                    case 2: temp = 0x04; break;
                    case 3: temp = 0x08; break;
                    case 4: temp = 0x10; break;
                    case 5: temp = 0x20; break;
                    case 6: temp = 0x40; break;
                    case 7: temp = 0x80; break;
                    default: break;
                }

                if ((InBytes[index] & temp) == temp)
                {
                    buffer[i] = true;
                }
            }

            return buffer;
        }

        /// <summary>
        /// 将一个数组进行扩充到指定长度，或是缩短到指定长度 ->
        /// Extend an array to a specified length, or shorten to a specified length or fill
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="data">原先数据的数据</param>
        /// <param name="length">新数组的长度</param>
        /// <returns>新数组长度信息</returns>
        /// <example>
        /// <code lang="cs" source="HslCommunication_Net45.Test\Documentation\Samples\BasicFramework\SoftBasicExample.cs" region="ArrayExpandToLengthExample" title="ArrayExpandToLength示例" />
        /// </example>
        public static T[] ArrayExpandToLength<T>(T[] data, int length)
        {
            if (data == null) return new T[length];

            if (data.Length == length) return data;

            T[] buffer = new T[length];

            Array.Copy(data, buffer, Math.Min(data.Length, buffer.Length));

            return buffer;
        }

        /// <summary>
        /// 将一个数组进行扩充到偶数长度 ->
        /// Extend an array to even lengths
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="data">原先数据的数据</param>
        /// <returns>新数组长度信息</returns>
        /// <example>
        /// <code lang="cs" source="HslCommunication_Net45.Test\Documentation\Samples\BasicFramework\SoftBasicExample.cs" region="ArrayExpandToLengthEvenExample" title="ArrayExpandToLengthEven示例" />
        /// </example>
        public static T[] ArrayExpandToLengthEven<T>(T[] data)
        {
            if (data == null) return new T[0];

            if (data.Length % 2 == 1)
            {
                return ArrayExpandToLength(data, data.Length + 1);
            }
            else
            {
                return data;
            }
        }

        /// <summary>
        /// 将byte数组按照双字节进行反转，如果为单数的情况，则自动补齐 ->
        /// Reverses the byte array by double byte, or if the singular is the case, automatically
        /// </summary>
        /// <param name="inBytes">输入的字节信息</param>
        /// <returns>反转后的数据</returns>
        /// <example>
        /// <code lang="cs" source="HslCommunication_Net45.Test\Documentation\Samples\BasicFramework\SoftBasicExample.cs" region="BytesReverseByWord" title="BytesReverseByWord示例" />
        /// </example>
        public static byte[] BytesReverseByWord(byte[] inBytes)
        {
            if (inBytes == null) return null;
            byte[] buffer = ArrayExpandToLengthEven(inBytes);

            for (int i = 0; i < buffer.Length / 2; i++)
            {
                byte tmp = buffer[i * 2 + 0];
                buffer[i * 2 + 0] = buffer[i * 2 + 1];
                buffer[i * 2 + 1] = tmp;
            }

            return buffer;
        }

        /// <summary>
        /// 字节数据转化成16进制表示的字符串 ->
        /// Byte data into a string of 16 binary representations
        /// </summary>
        /// <param name="InBytes">字节数组</param>
        /// <param name="segment">分割符</param>
        /// <returns>返回的字符串</returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <example>
        /// <code lang="cs" source="HslCommunication_Net45.Test\Documentation\Samples\BasicFramework\SoftBasicExample.cs" region="ByteToHexStringExample2" title="ByteToHexString示例" />
        /// </example>
        public static string ByteToHexString(byte[] InBytes, char segment)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte InByte in InBytes)
            {
                if (segment == 0) sb.Append(string.Format("{0:X2}", InByte));
                else sb.Append(string.Format("{0:X2}{1}", InByte, segment));
            }

            if (segment != 0 && sb.Length > 1 && sb[sb.Length - 1] == segment)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        /// int数组转byte数组
        /// </summary>
        /// <param name="src">源int数组</param>
        /// <param name="offset">起始位置,一般为0</param>
        /// <returns>values</returns>
        public static byte[] intToBytes(int[] src, int offset = 0)
        {
            byte[] values = new byte[src.Length * 4];
            for (int i = 0; i < src.Length; i++)
            {
                values[offset + 3] = (byte)((src[i] >> 24) & 0xFF);
                values[offset + 2] = (byte)((src[i] >> 16) & 0xFF);
                values[offset + 1] = (byte)((src[i] >> 8) & 0xFF);
                values[offset] = (byte)(src[i] & 0xFF);
                offset += 4;
            }

            return values;
        }

        #endregion
    }
}
