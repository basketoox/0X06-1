using System;
using System.Windows.Forms;

namespace desay
{
    public partial class frmStarting : Form
    {
        public frmStarting()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public frmStarting(byte maxValue)
            : this()
        {
            lblVersion.Text = $"Ver {Application.ProductVersion}";
            PBress.Maximum = maxValue;
            PBress.Step = 1;
            label1.Text = "加载中";
        }

        public void ShowMessage(string str)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(ShowMessage), str);
            }
            else
            {
                label1.Text = str;
                PBress.PerformStep();
                if (PBress.Value >= PBress.Maximum)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
