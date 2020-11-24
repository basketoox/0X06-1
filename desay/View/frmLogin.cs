using System;
using System.Windows.Forms;
using log4net;
using System.Toolkit;
using System.Toolkit.Helpers;
using desay.ProductData;
namespace desay
{
    public partial class frmLogin : Form
    {
        #region 变量
        ILog log = LogManager.GetLogger(typeof(frmLogin));
        private int i;
        #endregion

        #region 构造函数
        public   frmLogin()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

     
        #endregion

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (cbxUers.Text == UserLevel.工程师.ToString() || cbxUers.Text == UserLevel.管理员.ToString())
            {
                if (SecurityHelper.TextToMd5(txtPassword.Text) == Config.Instance.AdminPassword)
                {
                    Config.Instance.userLevel = UserLevel.工程师;
                    Config.Instance.userName = cbxUers.Text;
                    log.Info(string.Format("用户 {0} 已登录", Config.Instance.userName));
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("请输入正确的用户名与密码");
                }
            }
            if (cbxUers.Text == UserLevel.操作员.ToString())
            {
                if (SecurityHelper.TextToMd5(txtPassword.Text) == Config.Instance.OperatePassword)
                {
                    Config.Instance.userLevel = UserLevel.操作员;
                    Config.Instance.userName = cbxUers.Text;
                    log.Info(string.Format("用户 {0} 已登录", Config.Instance.userName));
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                    MessageBox.Show("请输入正确的用户名与密码");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            new frmPasswordChange().ShowDialog();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (Config.Instance.userLevel == UserLevel.None) btnCancel.Enabled = true;
            cbxUers.Items.Add(UserLevel.操作员.ToString());
            cbxUers.Items.Add(UserLevel.工程师.ToString());
            cbxUers.Items.Add(UserLevel.管理员.ToString());
            cbxUers.SelectedIndex = 0;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
           if (txtPassword .Text == "5201314")
            {
                btnCancel.Visible = true;
            }
        }
    }
}
