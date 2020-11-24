using desay.ProductData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Device;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Toolkit.Helpers;
using System.Windows.Forms;

namespace desay
{
    public partial class frmCommSetting : Form
    {
        public frmCommSetting()
        {
            InitializeComponent();

            autoScannerParam.SetConnectionString(Position.Instance.AutoConnectionString);
            manualScannerParam.SetConnectionString(Position.Instance.ManualConnectionString);
            HeightDetectParam.SetConnectionString(Position.Instance.HeightConnectionString);
            WhiteBoardPowerParam.SetConnectionString(Position.Instance.WhiteBoardPowerConnectionString);
            txtAAPort.Text = Position.Instance.AAPort.ToString();
            txtCCDPort.Text = Position.Instance.CCDPort.ToString();
            txtWhitePort.Text = Position.Instance.WBPort.ToString();
            txtSocketTimeout.Text = Position.Instance.SocketTimeout.ToString();
            txtServerIP.Text = Position.Instance.ServerIP;
            txtMesAddr.Text = Position.Instance.MesAddr;
            txtMesPort.Text = Position.Instance.MesPort.ToString();
            txtMesTimeout.Text = Position.Instance.MesTimeout.ToString();
            txtMesFailCount.Text = Position.Instance.MesFailCount.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Position.Instance.AutoConnectionString = autoScannerParam.GetConnectionString();
            Position.Instance.ManualConnectionString = manualScannerParam.GetConnectionString();
            Position.Instance.HeightConnectionString = HeightDetectParam.GetConnectionString();
            Position.Instance.WhiteBoardPowerConnectionString = WhiteBoardPowerParam.GetConnectionString();
            int.TryParse(txtAAPort.Text, out Position.Instance.AAPort);
            int.TryParse(txtCCDPort.Text, out Position.Instance.CCDPort);
            int.TryParse(txtWhitePort.Text, out Position.Instance.WBPort);
            int.TryParse(txtSocketTimeout.Text, out Position.Instance.SocketTimeout);
            Position.Instance.ServerIP = txtServerIP.Text;
            //待添加保存操作
            Position.Instance.MesAddr = txtMesAddr.Text;
            int.TryParse(txtMesPort.Text, out Position.Instance.MesPort);
            int.TryParse(txtMesTimeout.Text, out Position.Instance.MesTimeout);
            int.TryParse(txtMesFailCount.Text, out Position.Instance.MesFailCount);

            SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
        }

        public void GetTcpParam(string str, ref string ip, ref int port)
        {
            string[] param = str.Split(',');
            ip = param[0];
            port = int.Parse(param[1]);
            int readTimeout = int.Parse(param[2]);
            int writeTimeout = int.Parse(param[3]);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        Panasonic test = new Panasonic();
        private void button1_Click(object sender, EventArgs e)
        {
            test.SetConnectionParam(Position.Instance.HeightConnectionString);
            if (test.IsOpen)
                test.DeviceClose();
            test.DeviceOpen();
            if (test.IsOpen)
            {
                MessageBox.Show("open sucess");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            test.WriteDetectHeightCommand();
            Stopwatch time = new Stopwatch();
            time.Start();
            while (true)
            {
                if (time.ElapsedMilliseconds > test.ReadTimeout)
                {
                    time.Stop();
                    break;
                }
                if (test.StringReceived)
                {
                    test.StringReceived = false;
                    MessageBox.Show(test.ReceiveString);
                    break;
                }
            }
            if (test.IsOpen)
                test.DeviceClose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Marking.BeginTriggerFN = true;
            frmMain.main.m_Mes.SendRequestMsg(2);
            
        }
    }
}
