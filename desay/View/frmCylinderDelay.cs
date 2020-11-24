using desay.ProductData;
using Motion.Enginee;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Toolkit.Helpers;
using System.Windows.Forms;

namespace desay
{
    public partial class frmCylinderDelay : Form
    {
        #region 气缸延时
        private CylinderParameter CarrierStopParameter;
        private CylinderParameter CarrierClampParameter;
        private CylinderParameter CarrierUpParameter;
        private CylinderParameter CarrierMoveParameter;

        private CylinderParameter CleanStopParameter;
        private CylinderParameter CleanUpParameter;
        private CylinderParameter CleanRotateParameter;
        private CylinderParameter CleanClampParameter;
        private CylinderParameter CleanUpDownParameter;
        private CylinderParameter LightUpDownParameter;

        private CylinderParameter GlueStopParameter;
        private CylinderParameter GlueUpParameter;

        #endregion
        public frmCylinderDelay()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 气缸参数
            Delay.Instance.moveCylinderDelay = CarrierMoveParameter.Save;
            Delay.Instance.carrierUpCylinderDelay = CarrierUpParameter.Save;
            Delay.Instance.carrierClampCylinderDelay = CarrierClampParameter.Save;
            Delay.Instance.carrierStopCylinderDelay = CarrierStopParameter.Save;

            Delay.Instance.cleanStopCylinderDelay = CleanStopParameter.Save;
            Delay.Instance.cleanUpCylinderDelay = CleanUpParameter.Save;
            Delay.Instance.cleanRotateCylinderDelay = CleanRotateParameter.Save;
            Delay.Instance.cleanClampCylinderDelay = CleanClampParameter.Save;
            Delay.Instance.cleanUpDownCylinderDelay = CleanUpDownParameter.Save;
            Delay.Instance.lightUpDownCylinderDelay = LightUpDownParameter.Save;

            Delay.Instance.glueStopCylinderDelay = GlueStopParameter.Save;
            Delay.Instance.glueUpCylinderDelay = GlueUpParameter.Save;
            #endregion

            //SerializerManager<Delay>.Instance.Save(AppConfig.ConfigDelayName, Delay.Instance);
        }

        private void frmCylinderDelay_Load(object sender, EventArgs e)
        {
            CarrierMoveParameter = new CylinderParameter(Delay.Instance.moveCylinderDelay) { Name = "接驳台平移气缸" };
            CarrierUpParameter = new CylinderParameter(Delay.Instance.carrierUpCylinderDelay) { Name = "接驳台顶升气缸" };
            CarrierClampParameter = new CylinderParameter(Delay.Instance.carrierClampCylinderDelay) { Name = "接驳台开夹气缸" };
            CarrierStopParameter = new CylinderParameter(Delay.Instance.carrierStopCylinderDelay) { Name = "回流线阻挡气缸" };

            CleanStopParameter = new CylinderParameter(Delay.Instance.cleanStopCylinderDelay) { Name = "清洗阻挡气缸" };
            CleanUpParameter = new CylinderParameter(Delay.Instance.cleanUpCylinderDelay) { Name = "清洗顶升气缸" };
            CleanRotateParameter = new CylinderParameter(Delay.Instance.cleanRotateCylinderDelay) { Name = "清洗旋转气缸" };
            CleanClampParameter = new CylinderParameter(Delay.Instance.cleanClampCylinderDelay) { Name = "清洗夹紧气缸" };
            CleanUpDownParameter = new CylinderParameter(Delay.Instance.cleanUpDownCylinderDelay) { Name = "清洗上下气缸" };
            LightUpDownParameter = new CylinderParameter(Delay.Instance.lightUpDownCylinderDelay) { Name = "光源上下气缸" };

            GlueStopParameter = new CylinderParameter(Delay.Instance.glueStopCylinderDelay) { Name = "点胶阻挡气缸" };
            GlueUpParameter = new CylinderParameter(Delay.Instance.glueUpCylinderDelay) { Name = "点胶顶升气缸" };

            flpCylinder.Controls.Add(CarrierMoveParameter);
            flpCylinder.Controls.Add(CarrierUpParameter);
            flpCylinder.Controls.Add(CarrierClampParameter);
            flpCylinder.Controls.Add(CarrierStopParameter);
            flpCylinder.Controls.Add(CleanStopParameter);
            flpCylinder.Controls.Add(CleanUpParameter);
            flpCylinder.Controls.Add(CleanRotateParameter);
            flpCylinder.Controls.Add(CleanClampParameter);
            flpCylinder.Controls.Add(CleanUpDownParameter);
            flpCylinder.Controls.Add(LightUpDownParameter);
            flpCylinder.Controls.Add(GlueStopParameter);
            flpCylinder.Controls.Add(GlueUpParameter);
        }
    }
}
