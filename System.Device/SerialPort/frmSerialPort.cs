using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace System.Device
{
    public partial class frmSerialPort : Form
    {
        #region 变量

        ISerialPortTriggerModel _device;

        #endregion

        #region 构造函数

        public frmSerialPort()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// 窗体构造函数
        /// </summary>
        /// <param name="devicename">设备标识</param>
        public frmSerialPort(ISerialPortTriggerModel idevice, Type type)
            : this()
        {
            _device = (ISerialPortTriggerModel)type.Assembly.CreateInstance(type.FullName);
            _device = idevice;
            this.Text = _device.Name;
        }

        #endregion

        #region 方法

        //清空文本内容
        void ToolStripMenuItem_Clear_Click(object sender, EventArgs e)
        {
            textBox_Receive.Clear();
        }

        //自动滚动文本
        void textBox_Receive_TextChanged(object sender, EventArgs e)
        {
            textBox_Receive.Select(textBox_Receive.TextLength, 0);
            textBox_Receive.ScrollToCaret();
        }

        /// <summary>
        /// 向接收区追加内容(已带换行符)
        /// </summary>
        /// <param name="tbx"></param>
        /// <param name="str"></param>
        void ShowMessage(string str)
        {
            if(InvokeRequired)
            {
                Invoke(new Action<string>(ShowMessage), str);
            }
            else
            {
                textBox_Receive.AppendText(str + Environment.NewLine);
            }
        }

        /// <summary>
        /// 加载通讯参数
        /// </summary>
        void LoadParam()
        {
            try
            {
                SerialPortParam1.SetConnectionString(_device.ConnectionParam);
                ShowMessage("已成功加载参数");
            }
            catch (Exception ex)
            {
                ShowMessage("参数加载失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 保存通讯参数
        /// </summary>
        void SaveParam()
        {
            try
            {
                _device.ConnectionParam = SerialPortParam1.GetConnectionString();
                ShowMessage("已保存参数\n");
            }
            catch (Exception ex)
            {
                ShowMessage("参数保存失败：" + ex.Message);
            }
        }

        #endregion

        void Form_SerialPort_Load(object sender, EventArgs e)
        {
            ToolStripMenuItem_Clear_Click(sender, e);
            LoadParam();
            ShowMessage(_device.Execute("Help"));
            button_Connect.Enabled = _device.IsOpen ? false : true;
            button_Close.Enabled = _device.IsOpen ? true : false;
        }

        void button_Save_Click(object sender, EventArgs e)
        {
            SaveParam();
        }

        void button_Connect_Click(object sender, EventArgs e)
        {
            try
            {
                _device.SetConnectionParam(_device.ConnectionParam);
                _device.DeviceOpen();

                ShowMessage("设备打开成功");
                button_Connect.Enabled = false;
                button_Close.Enabled = true;
            }
            catch (Exception ex)
            {
                ShowMessage(string.Format("设备打开失败。{0}", ex.ToString()));
            }
        }

        void button_Close_Click(object sender, EventArgs e)
        {
            try
            {
                _device.DeviceClose();

                ShowMessage("设备关闭成功");
                button_Connect.Enabled = true;
                button_Close.Enabled = false;
            }
            catch(Exception ex)
            {
                ShowMessage(string.Format("设备关闭失败。{0}", ex.ToString()));
            }
        }

        void button_Send_Click(object sender, EventArgs e)
        {
            ShowMessage(_device.Execute(textBox_Send.Text));
        }

    }
}
