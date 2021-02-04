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

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                string test = MesClient.HelloWorld();
                string Tip = (test == "Hello World") ? "通讯正常！" : "通讯异常！";
                MessageBox.Show(Tip,"提示");
            }
            catch
            {
                MessageBox.Show("通讯异常！","提示");
            }
        }

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
            SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName,Config.Instance);
            SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName,Position.Instance);
            MessageBox.Show("参数保存成功", "提示");
        }

        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            //解析工单
            //string str = fOrderJx(IPAddress.Text, pOrderCode.Text, pchItemNameK.Text, pchOperatorIDK.Text, "");
            string str = fOrderJx(IPAddress.Text, pOrderCode.Text, "", "", "");
            if (str != "")
            {
                pchPfmdataK.Text = str;
            }
        }

        /// <summary>
        /// 解析工单返回解析结果
        /// </summary>
        /// <param name="pchIPK">本机IP</param>
        /// <param name="barcode">工单</param>
        /// <param name="pchItemNameK">测试关卡</param>
        /// <param name="UserCode">工号</param>
        /// <param name="pcName">电脑名称</param>
        /// <param name="str">预留</param>
        /// <returns></returns>
        public string fOrderJx(string pchIPK, string barcode, string pchItemNameK, string UserCode, string pcName, string str="")
        {
            try
            {
                return MesClient.fOrderJx(pchIPK, barcode, pchItemNameK, UserCode, pcName, str);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 返回检查结果
        /// </summary>
        /// <param name="pchIPK">本机IP</param>
        /// <param name="pchModelK">机型</param>
        /// <param name="pchTestIdK">条码</param>
        /// <param name="pchItemNameK">测试关卡</param>
        /// <param name="pchOperatorIDK">员工工号</param>
        /// <param name="pchStationIDK">机台名称</param>
        /// <param name="pOrderCode">工单</param>
        /// <param name="pOrderType">单别</param>
        /// <param name="programCode">程序代码</param>
        /// <param name="str1">预留</param>
        /// <returns></returns>
        public string fCanIGoTest(string pchIPK, string pchModelK, string pchTestIdK, string pchItemNameK, string pchOperatorIDK, string pchStationIDK, string pOrderCode, string pOrderType, string programCode, string str1 = "")
        {
            try
            {
                return MesClient.fCanIGoTest(pchIPK, pchModelK, pchTestIdK, pchItemNameK, pchOperatorIDK, pchStationIDK, pOrderCode, pOrderType, programCode, str1);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 上传数据并返回保存结果
        /// </summary>
        /// <param name="pchIPK">本机IP</param>
        /// <param name="pchModelK">机型</param>
        /// <param name="pchTestIdK">条码</param>
        /// <param name="pchItemNameK">测试关卡</param>
        /// <param name="pchErrcdK">结果代码 OK:00 NG:不良代码</param>
        /// <param name="pchPfmdataK">备注，测试数据</param>
        /// <param name="pchOperatorIDK">测试员工工号</param>
        /// <param name="pchStationIDK">测试机台名称</param>
        /// <param name="pOrderCode">工单</param>
        /// <param name="pOrderType">单别</param>
        /// <param name="programCode">程序代码</param>
        /// <param name="str1">预留</param>
        /// <returns></returns>
        public string fSendData(string pchIPK, string pchModelK, string pchTestIdK, string pchItemNameK, string pchErrcdK, string pchPfmdataK, string pchOperatorIDK, string pchStationIDK, string pOrderCode, string pOrderType, string programCode, string str1 = "")
        {
            try
            {
                return MesClient.fSendData(pchIPK, pchModelK, pchTestIdK, pchItemNameK, pchErrcdK, pchPfmdataK, pchOperatorIDK, pchStationIDK, pOrderCode, pOrderType, programCode, "");
            }
            catch
            {
                return "";
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (pchErrcdk.Text != "" && pchPfmdataK.Text != "")
            {
                string testresult = fCanIGoTest(IPAddress.Text, pchModelK.Text, pchTestIdK.Text, pchItemNameK.Text, pchOperatorIDK.Text, pchStationIDK.Text, pOrderCode.Text, pOrderType.Text, programCode.Text);
                if (testresult.Trim(' ') == "0")
                {
                    string sendresult = fSendData(IPAddress.Text, pchModelK.Text, pchTestIdK.Text, pchItemNameK.Text, pchErrcdk.Text, pchPfmdataK.Text, pchOperatorIDK.Text, pchStationIDK.Text, pOrderCode.Text, pOrderType.Text, programCode.Text);
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
    }
}
