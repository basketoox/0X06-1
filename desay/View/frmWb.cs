using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace desay
{
    public partial class frmWb : Form
    {
        public frmWb()
        {
            InitializeComponent();
            //this.WindowState = FormWindowState.Normal;
        }

        public void frmWb_Load(object sender, EventArgs e)
        {
            
        }
        IntPtr DlgHandle_wb;
        public void ShowWb(object sender, EventArgs e)
        {
            CallWb.ShowAAImageDlg();
            DlgHandle_wb = CallWb.GetAAImageDlgHwnd();
            // IntPtr 

            CallWb.SetWindowPos(DlgHandle_wb, 0, 0, 0, 500, 600, 0);
            CallWb.ShowWindow(DlgHandle_wb, 1);//1为显示，0为隐藏
        }
        public void Exit()
        {
            this.Visible = false;
           
        }
        private void frmWb_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.FormOwnerClosing || e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ShowWb(this, null);
        }
    }
}
