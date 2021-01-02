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
            Config.Instance.DoorShield = Marking.DoorShield ? 1 : 0;
            Config.Instance.AAShield = Marking.AAShield ? 1 : 0;
            Config.Instance.CurtainShield = Marking.CurtainShield ? 1 : 0;
            Config.Instance.SnScannerShield = Marking.SnScannerShield ? 1 : 0;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
