using desay.ProductData;
using desay.ServiceReference1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Toolkit.Helpers;
using System.Windows.Forms;

namespace desay
{
    public partial class frmMES : Form
    {
        public Mes_WebServiceMainSoapClient MesClient;

        public frmMES()
        {
            InitializeComponent();
            GetIP();
            MesClient = new Mes_WebServiceMainSoapClient(Config.Instance.MesName, Config.Instance.MesIPAddress);
        }

        private void frmMES_Load(object sender, EventArgs e)
        {            
            ServiceAddress.Text = Config.Instance.MesIPAddress;
            pOrderCode.Text = Position.Instance.pOrderCode;
            pOrderType.Text = Position.Instance.pOrderType;
            pchTestIdK.Text = Position.Instance.pchTestIdK;
            pchItemNameK.Text = Position.Instance.pchItemNameK;
            pchOperatorIDK.Text = Position.Instance.pchOperatorIDK;
            pchStationIDK.Text = Position.Instance.pchStationIDK;
            pchModelK.Text = Position.Instance.pchModelK;
            programCode.Text = Position.Instance.programCode;
            pchErrcdk.Text = Position.Instance.pchErrcdk;
            pchPfmdataK.Text = Position.Instance.pchPfmdataK;
            if (Marking.DryRun)
            {
                pchErrcdk.ReadOnly = false;
                pchPfmdataK.ReadOnly = false;                
            }
            else
            {
                pchErrcdk.ReadOnly = true;
                pchPfmdataK.ReadOnly = true;
            }
        }

        private void GetIP()
        {
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    string IP = ipa.ToString();
                    if (IP != "192.168.10.10" && IP != Position.Instance.ServerIP)
                    {
                        string[] segment = IP.Split('.');
                        IP = string.Empty;
                        for (int i = 0; i < segment.Length; i++)
                        {
                            if (segment[i].Length != 3)
                            {
                                segment[i] = ((3 - segment[i].Length) == 2 ? "  " : " ") + segment[i];
                            }
                            IP += segment[i] + ".";
                        }
                        IPAddress.Text = IP.Substring(0, 15);
                        Config.Instance.LocalIP = IPAddress.Text;
                    }
                }                    
            }
        }


        /// <summary>
        /// 通讯测试
        /// </summary>
        public string HelloWord()
        {
            try
            {
                return MesClient.HelloWorld();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 解析工单返回解析结果
        /// </summary>
        public string fOrderJx()
        {
            try
            {
                return MesClient.fOrderJx(Config.Instance.LocalIP, Position.Instance.pOrderCode,
                           Position.Instance.pchItemNameK, Position.Instance.pchOperatorIDK, "", "");
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 返回数据检查结果
        /// </summary>
        public string fCanIGoTest(string pchTestIdK)
        {
            try
            {
                return MesClient.fCanIGoTest(Config.Instance.LocalIP, Position.Instance.pchModelK, pchTestIdK,
                                             Position.Instance.pchItemNameK, Position.Instance.pchOperatorIDK, Position.Instance.pchStationIDK, 
                                             Position.Instance.pOrderCode, Position.Instance.pOrderType, Position.Instance.programCode, "");
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 上传数据并返回保存结果
        /// </summary>
        /// <param name="pchErrcdK">结果代码 OK:00 NG:不良代码</param>
        /// <param name="pchPfmdataK">备注，测试数据</param>
        public string fSendData(string pchErrcdK, string pchPfmdataK)
        {
            try
            {
                return MesClient.fSendData(Config.Instance.LocalIP, Position.Instance.pchModelK, Position.Instance.pchTestIdK,
                                           Position.Instance.pchItemNameK, pchErrcdK, pchPfmdataK, 
                                           Position.Instance.pchOperatorIDK, Position.Instance.pchStationIDK, Position.Instance.pOrderCode, 
                                           Position.Instance.pOrderType, Position.Instance.programCode, "");
            }
            catch
            {
                return "";
            }
        }

        #region UI界面操作

        private void btnSave_Click(object sender, EventArgs e)
        {
            Config.Instance.LocalIP = IPAddress.Text;
            Config.Instance.MesIPAddress = ServiceAddress.Text;
            Position.Instance.pOrderCode = pOrderCode.Text;
            Position.Instance.pOrderType = pOrderType.Text;
            Position.Instance.pchTestIdK = pchTestIdK.Text;
            Position.Instance.pchItemNameK = pchItemNameK.Text;
            Position.Instance.pchOperatorIDK = pchOperatorIDK.Text;
            Position.Instance.pchStationIDK = pchStationIDK.Text;
            Position.Instance.pchModelK = pchModelK.Text;
            Position.Instance.programCode = programCode.Text;
            Position.Instance.pchErrcdk = pchErrcdk.Text;
            Position.Instance.pchPfmdataK = pchPfmdataK.Text;
            SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName, Config.Instance);
            SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
            MessageBox.Show("参数保存成功", "提示");
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                string test = MesClient.HelloWorld();
                string Tip = (test == "Hello World") ? "通讯正常！" : "通讯异常！";
                MessageBox.Show(Tip, "提示");
            }
            catch
            {
                MessageBox.Show("通讯异常！", "提示");
            }
        }

        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            //解析工单            
            string str = fOrderJx();
            if (str != "")
            {
                pchPfmdataK.Text = str;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (pchErrcdk.Text != "" && pchPfmdataK.Text != "")
            {
                //检查数据
                string testresult = fCanIGoTest(pchTestIdK.Text);
                if (testresult.Trim(' ') == "0")
                {
                    //上传数据
                    string sendresult = fSendData(pchErrcdk.Text, pchPfmdataK.Text);
                    if (sendresult.Trim(' ') == "0")
                    {
                        MessageBox.Show("数据上传成功！", "提示");
                    }
                    else
                    {
                        MessageBox.Show("数据上传失败！", "提示");
                    }
                }
                else
                {
                    MessageBox.Show("数据检查结果NG！", "提示");
                }
            }
            else
            {
                MessageBox.Show("测试数据不能为空！", "提示");
            }
            
        }

        #endregion

    }
}
