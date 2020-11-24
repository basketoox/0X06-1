using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace System.Device
{
    public partial class NetworkParam : UserControl
    {

        public NetworkParam()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取通讯字符串
        /// </summary>
        public string GetConnectionString()
        {
            string connectionString = textBox_IPAddr.Text + ","
                + textBox_PortNumber.Text + ","
                + textBox_ReadTimeOut.Text + ","
                + textBox_WriteTimeOut.Text;
            return connectionString;
        }

        /// <summary>
        /// 根据通讯字符串设置参数
        /// </summary>
        public void SetConnectionString(string connectionstring)
        {
            string[] connectParams = connectionstring.Split(',');
            textBox_IPAddr.Text = connectParams[0];
            textBox_PortNumber.Text = connectParams[1];
            textBox_ReadTimeOut.Text = connectParams[2];
            textBox_WriteTimeOut.Text = connectParams[3];
        }

    }
}
