using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace System.Device
{
    public partial class SerialPortParam :  UserControl
    {

        #region 变量

        string[] baudRates = { "1200", "2400", "4800", "9600", "14400", "19200", "28800", "38400", "57600", "115200" };
        string[] dataBits = { "7", "8"};

        #endregion

        public SerialPortParam()
        {
            InitializeComponent();

            comboBox_PortName.Items.AddRange(SerialPort.GetPortNames());
            comboBox_BaudRate.Items.AddRange(baudRates);
            comboBox_DataBit.Items.AddRange(dataBits);
            comboBox_Parity.Items.AddRange(Enum.GetNames(typeof(Parity)));
            comboBox_StopBit.Items.AddRange(Enum.GetNames(typeof(StopBits)).SkipWhile(s => s == "None").ToArray<string>());
        }

        /// <summary>
        /// 获取通讯字符串
        /// </summary>
        public string GetConnectionString()
        {
            string connectionString = comboBox_PortName.Text + ","
                + comboBox_BaudRate.Text + ","
                + comboBox_Parity.Text + ","
                + comboBox_DataBit.Text+ ","
                + comboBox_StopBit.Text + ","
                + textBox_ReadTimeOut.Text + ","
                + textBox_WriteTimeOut.Text;
            return connectionString;
        }

        /// <summary>
        /// 根据通讯字符串设置参数
        /// </summary>
        public void SetConnectionString(string connectionstring)
        {
            if (string.IsNullOrEmpty(connectionstring))
                return;
            string[] connectParams = connectionstring.Split(',');
            comboBox_PortName.Text = connectParams[0];
            comboBox_BaudRate.Text = connectParams[1];
            comboBox_Parity.Text = connectParams[2];
            comboBox_DataBit.Text = connectParams[3];
            comboBox_StopBit.Text = connectParams[4];
            textBox_ReadTimeOut.Text = connectParams[5];
            textBox_WriteTimeOut.Text = connectParams[6];
        }

    }
}
