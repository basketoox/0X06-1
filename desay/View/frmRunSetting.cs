using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using desay.ProductData;
namespace desay
{
    public partial class frmRunSetting : Form
    {
        public frmRunSetting()
        {
            InitializeComponent();
        }

        private void frmRunSetting_Load(object sender, EventArgs e)
        {
            chkCarrierHaveProduct.Checked = Marking.CarrierHaveProduct;
            chkCleanHaveProduct.Checked = Marking.CleanHaveProduct;
            chkGlueHaveProduct.Checked = Marking.GlueHaveProduct;
            chkDoorSheild.Checked = Marking.DoorShield;
            chkCurtainShield.Checked = Marking.CurtainShield;
            chkAAShield.Checked = Marking.AAShield;
            chkSnScannerShield.Checked = Marking.SnScannerShield;
            chkDryRun.Checked = Marking.DryRun;
            chkLeaveShield.Checked = Marking.LeaveShield;

            chkWhiteMode.Checked = Marking.WhiteMode;
            chkGlueMode.Checked = Marking.GlueMode;
            chkAAMode.Checked = Marking.AAMode;
            chkWeighMode.Checked = Marking.WeighMode;
               
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Marking.CarrierHaveProduct = chkCarrierHaveProduct.Checked;
            Marking.CleanHaveProduct = chkCleanHaveProduct.Checked;
            Marking.GlueHaveProduct = chkGlueHaveProduct.Checked;
            Marking.DoorShield = chkDoorSheild.Checked;
            Marking.CurtainShield = chkCurtainShield.Checked;
            Marking.AAShield = chkAAShield.Checked;
            Marking.SnScannerShield = chkSnScannerShield.Checked;
            Marking.DryRun = chkDryRun.Checked;
            Marking.LeaveShield = chkLeaveShield.Checked;
            Config.Instance.DoorShield = Marking.DoorShield ? 1 : 0;
            Config.Instance.AAShield = Marking.AAShield ? 1 : 0;
            Config.Instance.CurtainShield = Marking.CurtainShield ? 1 : 0;
            Config.Instance.SnScannerShield = Marking.SnScannerShield ? 1 : 0;

            Marking.WhiteMode = chkWhiteMode.Checked;
            Marking.GlueMode = chkGlueMode.Checked;
            Marking.AAMode = chkAAMode.Checked;
            Marking.WeighMode = chkWeighMode.Checked;
            if (Marking.WhiteMode || Marking.GlueMode || Marking.AAMode || Marking.WeighMode)
            {
                frmMain.DailyCheck = true;
            }
            else
            {
                frmMain.DailyCheck = false;
            }
            
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkWhiteMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWhiteMode.Checked)
            {
                chkGlueMode.Checked = false;
                chkAAMode.Checked = false;
                chkWeighMode.Checked = false;
            }
        }

        private void chkGlueMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGlueMode.Checked)
            {
                chkWhiteMode.Checked = false;
                chkAAMode.Checked = false;
                chkWeighMode.Checked = false;
            }
        }

        private void chkAAMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAAMode.Checked)
            {
                chkWhiteMode.Checked = false;
                chkGlueMode.Checked = false;
                chkWeighMode.Checked = false;
            }
        }

        private void chkWeighMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWeighMode.Checked)
            {
                chkWhiteMode.Checked = false;
                chkGlueMode.Checked = false;
                chkAAMode.Checked = false;
            }
        }
    }
}
