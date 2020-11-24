using System;
using System.Windows.Forms;
using log4net;
using System.Toolkit;
using System.Toolkit.Helpers;
using desay.ProductData;
namespace desay
{
    public partial class frmPasswordChange : Form
    {
        ILog log = LogManager.GetLogger(typeof(frmPasswordChange));

        public frmPasswordChange()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            comboBox_UserSelect.Items.Add(UserLevel.操作员.ToString());
            comboBox_UserSelect.Items.Add(UserLevel.工程师.ToString());
            comboBox_UserSelect.SelectedIndex = 1;
        }
        private void button_Yes_Click(object sender, EventArgs e)
        {
            if (comboBox_UserSelect.Text == UserLevel.工程师.ToString())
            {
                if (SecurityHelper.TextToMd5(textBox_OldPassword.Text.Trim()) == Config.Instance.AdminPassword)
                {
                    if (textBox_NewPassword.Text.Trim() == textBox_ConfirmPassword.Text.Trim())
                    {
                        Config.Instance.AdminPassword = SecurityHelper.TextToMd5(textBox_NewPassword.Text.Trim());
                        log.Debug("密码修改成功");
                        MessageBox.Show("密码修改成功");
                    }
                    else
                    {
                        log.Debug("新密码与确认密码不一致，请重新输入");
                        MessageBox.Show("新密码与确认密码不一致，请重新输入");
                    }
                }
                else
                {
                    log.Debug("原始密码输入错误，请重新输入");
                    MessageBox.Show("原始密码输入错误，请重新输入");   
                }
            }
            else if (comboBox_UserSelect.Text == UserLevel.操作员.ToString())
            {
                if (SecurityHelper.TextToMd5(textBox_OldPassword.Text.Trim()) == Config.Instance.OperatePassword)
                {
                    if (textBox_NewPassword.Text.Trim() == textBox_ConfirmPassword.Text.Trim())
                    {
                        Config.Instance.OperatePassword = SecurityHelper.TextToMd5(textBox_NewPassword.Text.Trim());
                        log.Debug("密码修改成功");
                        MessageBox.Show("密码修改成功"); 
                    }
                    else
                    {
                        log.Debug("新密码与确认密码不一致，请重新输入");
                        MessageBox.Show("新密码与确认密码不一致，请重新输入");
                    }
                }
                else
                {
                    log.Debug("原始密码输入错误，请重新输入");
                    MessageBox.Show("原始密码输入错误，请重新输入");
                }
            }
            else
            {
                log.Debug("用户操作类型非法!");
                MessageBox.Show("用户操作类型非法!");
                return;
            }
        }
        private void button_No_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
