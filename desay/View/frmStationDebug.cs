using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using desay.Flow;
using Motion.Enginee;
using desay.ProductData;

namespace desay
{
    public partial class frmStationDebug : Form
    {
        CleanPlateform m_LeftPlateform;
        GluePlateform m_RightPlateform;
        Carrier m_Assembly;
        ModelOperate mopLeftPlateform, mopRightPlateform, mopAssembly;

        #region 气缸
        private CylinderOperate CleanRotateOperate, CleanClampOperate, CleanUpDownOperate;

        private CylinderOperate LightUpDownOperate, LightingTestOperate;

        private CylinderOperate CleanStopOperate, CleanUpOperate;

        private void btnCarrier1_Click(object sender, EventArgs e)
        {
            if (IoPoints.IDO9.Value)
            {
                IoPoints.IDO9.Value = false;
            }
            else
            {
                IoPoints.IDO9.Value = true;
            }
        }

        private void btnCarrier2_Click(object sender, EventArgs e)
        {
            if (IoPoints.IDO8.Value)
            {
                IoPoints.IDO8.Value = false;
            }
            else
            {
                IoPoints.IDO8.Value = true;
            }
        }

        private void btnCarrier3_Click(object sender, EventArgs e)
        {
            if (IoPoints.IDO0.Value)
            {
                IoPoints.IDO0.Value = false;
            }
            else
            {
                IoPoints.IDO1.Value = false;
                IoPoints.IDO0.Value = true;
            }
        }

        private void btnCarrier4_Click(object sender, EventArgs e)
        {
            if (IoPoints.IDO1.Value)
            {
                IoPoints.IDO1.Value = false;
            }
            else
            {
                IoPoints.IDO0.Value = false;
                IoPoints.IDO1.Value = true;
            }
        }

        private CylinderOperate GlueStopOperate, GlueUpOperate;
        private CylinderOperate MoveOperate, CarrierUpOperate, CarrierClampOperate, CarrierStopOperate;
        #endregion

        public frmStationDebug()
        {
            InitializeComponent();
        }

        public frmStationDebug(CleanPlateform leftPlateform, GluePlateform rightPlateform, Carrier assembly) : this()
        {
            m_LeftPlateform = leftPlateform;
            m_RightPlateform = rightPlateform;
            m_Assembly = assembly;
            mopLeftPlateform = new ModelOperate();
            mopLeftPlateform.Dock = DockStyle.Fill;
            mopRightPlateform = new ModelOperate();
            mopRightPlateform.Dock = DockStyle.Fill;
            mopAssembly = new ModelOperate();
            mopAssembly.Dock = DockStyle.Fill;
        }

        private void frmStationDebug_Load(object sender, EventArgs e)
        {
            mopLeftPlateform.StationIni = m_LeftPlateform.stationInitialize;
            mopLeftPlateform.StationOpe = m_LeftPlateform.stationOperate;

            mopRightPlateform.StationIni = m_RightPlateform.stationInitialize;
            mopRightPlateform.StationOpe = m_RightPlateform.stationOperate;

            mopAssembly.StationIni = m_Assembly.stationInitialize;
            mopAssembly.StationOpe = m_Assembly.stationOperate;

            gbxLeftplateform.Controls.Add(mopLeftPlateform);
            gbxRightplateform.Controls.Add(mopRightPlateform);
            gbxAssembly.Controls.Add(mopAssembly);

            //清洗气缸
            CleanStopOperate = new CylinderOperate(() => { m_LeftPlateform.CleanStopCylinder.ManualExecute(); })
            {
                CylinderName = "清洗阻挡气缸",
                NoOriginShield = m_LeftPlateform.CleanStopCylinder.Condition.NoOriginShield,
                NoMoveShield = m_LeftPlateform.CleanStopCylinder.Condition.NoMoveShield
            };
            CleanUpOperate = new CylinderOperate(() => { m_LeftPlateform.CleanUpCylinder.ManualExecute(); })
            {
                CylinderName = "清洗顶升气缸",
                NoOriginShield = m_LeftPlateform.CleanUpCylinder.Condition.NoOriginShield,
                NoMoveShield = m_LeftPlateform.CleanUpCylinder.Condition.NoMoveShield
            };
            CleanClampOperate = new CylinderOperate(() => { m_LeftPlateform.CleanClampCylinder.ManualExecute(); })
            {
                CylinderName = "清洗夹紧气缸",
                NoOriginShield = m_LeftPlateform.CleanClampCylinder.Condition.NoOriginShield,
                NoMoveShield = m_LeftPlateform.CleanClampCylinder.Condition.NoMoveShield
            };
            CleanRotateOperate = new CylinderOperate(() => { m_LeftPlateform.CleanRotateCylinder.ManualExecute(); })
            {
                CylinderName = "清洗旋转气缸",
                NoOriginShield = m_LeftPlateform.CleanRotateCylinder.Condition.NoOriginShield,
                NoMoveShield = m_LeftPlateform.CleanRotateCylinder.Condition.NoMoveShield
            };
            CleanUpDownOperate = new CylinderOperate(() => { m_LeftPlateform.CleanUpDownCylinder.ManualExecute(); })
            {
                CylinderName = "清洗上下气缸",
                NoOriginShield = m_LeftPlateform.CleanUpDownCylinder.Condition.NoOriginShield,
                NoMoveShield = m_LeftPlateform.CleanUpDownCylinder.Condition.NoMoveShield
            };
            LightUpDownOperate = new CylinderOperate(() => { m_LeftPlateform.LightUpDownCylinder.ManualExecute(); })
            {
                CylinderName = "光源上下气缸",
                NoOriginShield = m_LeftPlateform.LightUpDownCylinder.Condition.NoOriginShield,
                NoMoveShield = m_LeftPlateform.LightUpDownCylinder.Condition.NoMoveShield
            };
            //点胶气缸
            GlueStopOperate = new CylinderOperate(() => { m_RightPlateform.GlueStopCylinder.ManualExecute(); })
            {
                CylinderName = "点胶阻挡气缸",
                NoOriginShield = m_RightPlateform.GlueStopCylinder.Condition.NoOriginShield,
                NoMoveShield = m_RightPlateform.GlueStopCylinder.Condition.NoMoveShield
            };
            GlueUpOperate = new CylinderOperate(() => { m_RightPlateform.GlueUpCylinder.ManualExecute(); })
            {
                CylinderName = "点胶顶升气缸",
                NoOriginShield = m_RightPlateform.GlueUpCylinder.Condition.NoOriginShield,
                NoMoveShield = m_RightPlateform.GlueUpCylinder.Condition.NoMoveShield
            };
            //载具气缸
            MoveOperate = new CylinderOperate(() => { m_Assembly.MoveCylinder.ManualExecute(); })
            {
                CylinderName = "载具移动气缸",
                NoOriginShield = m_Assembly.MoveCylinder.Condition.NoOriginShield,
                NoMoveShield = m_Assembly.MoveCylinder.Condition.NoMoveShield
            };
            CarrierUpOperate = new CylinderOperate(() => { m_Assembly.CarrierUpCylinder.ManualExecute(); })
            {
                CylinderName = "载具顶升气缸",
                NoOriginShield = m_Assembly.CarrierUpCylinder.Condition.NoOriginShield,
                NoMoveShield = m_Assembly.CarrierUpCylinder.Condition.NoMoveShield
            };
            CarrierClampOperate = new CylinderOperate(() => { m_Assembly.CarrierClampCylinder.ManualExecute(); })
            {
                CylinderName = "载具开夹气缸",
                NoOriginShield = m_Assembly.CarrierClampCylinder.Condition.NoOriginShield,
                NoMoveShield = m_Assembly.CarrierClampCylinder.Condition.NoMoveShield
            };
            CarrierStopOperate = new CylinderOperate(() => { m_Assembly.CarrierStopCylinder.ManualExecute(); })
            {
                CylinderName = "回流线阻挡气缸",
                NoOriginShield = m_Assembly.CarrierStopCylinder.Condition.NoOriginShield,
                NoMoveShield = m_Assembly.CarrierStopCylinder.Condition.NoMoveShield
            };
            flpCylinder.Controls.Add(CleanStopOperate);
            flpCylinder.Controls.Add(CleanUpOperate);
            flpCylinder.Controls.Add(CleanClampOperate);
            flpCylinder.Controls.Add(CleanRotateOperate);
            flpCylinder.Controls.Add(CleanUpDownOperate);
            flpCylinder.Controls.Add(LightUpDownOperate);
            //flpCylinder.Controls.Add(LightingTestOperate);
            flpCylinder.Controls.Add(GlueStopOperate);
            flpCylinder.Controls.Add(GlueUpOperate);
            flpCylinder.Controls.Add(MoveOperate);
            flpCylinder.Controls.Add(CarrierUpOperate);
            flpCylinder.Controls.Add(CarrierClampOperate);
            flpCylinder.Controls.Add(CarrierStopOperate);

            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            mopAssembly.Refreshing();
            mopLeftPlateform.Refreshing();
            mopRightPlateform.Refreshing();

            #region 气缸状态
            //清洗平台
            CleanStopOperate.InOrigin = m_LeftPlateform.CleanStopCylinder.OutOriginStatus;
            CleanStopOperate.InMove = m_LeftPlateform.CleanStopCylinder.OutMoveStatus;
            CleanStopOperate.OutMove = m_LeftPlateform.CleanStopCylinder.IsOutMove;
            CleanUpOperate.InOrigin = m_LeftPlateform.CleanUpCylinder.OutOriginStatus;
            CleanUpOperate.InMove = m_LeftPlateform.CleanUpCylinder.OutMoveStatus;
            CleanUpOperate.OutMove = m_LeftPlateform.CleanUpCylinder.IsOutMove;
            CleanClampOperate.InOrigin = m_LeftPlateform.CleanClampCylinder.OutOriginStatus;
            CleanClampOperate.InMove = m_LeftPlateform.CleanClampCylinder.OutMoveStatus;
            CleanClampOperate.OutMove = m_LeftPlateform.CleanClampCylinder.IsOutMove;
            CleanRotateOperate.InOrigin = m_LeftPlateform.CleanRotateCylinder.OutOriginStatus;
            CleanRotateOperate.InMove = m_LeftPlateform.CleanRotateCylinder.OutMoveStatus;
            CleanRotateOperate.OutMove = m_LeftPlateform.CleanRotateCylinder.IsOutMove;
            CleanUpDownOperate.InOrigin = m_LeftPlateform.CleanUpDownCylinder.OutOriginStatus;
            CleanUpDownOperate.InMove = m_LeftPlateform.CleanUpDownCylinder.OutMoveStatus;
            CleanUpDownOperate.OutMove = m_LeftPlateform.CleanUpDownCylinder.IsOutMove;
            LightUpDownOperate.InOrigin = m_LeftPlateform.LightUpDownCylinder.OutOriginStatus;
            LightUpDownOperate.InMove = m_LeftPlateform.LightUpDownCylinder.OutMoveStatus;
            LightUpDownOperate.OutMove = m_LeftPlateform.LightUpDownCylinder.IsOutMove;
            //LightingTestOperate.InOrigin = m_LeftPlateform.LightingTestCylinder.OutOriginStatus;
            //LightingTestOperate.InMove = m_LeftPlateform.LightingTestCylinder.OutMoveStatus;
            //LightingTestOperate.OutMove = m_LeftPlateform.LightingTestCylinder.IsOutMove;

            //点胶平台
            GlueStopOperate.InOrigin = m_RightPlateform.GlueStopCylinder.OutOriginStatus;
            GlueStopOperate.InMove = m_RightPlateform.GlueStopCylinder.OutMoveStatus;
            GlueStopOperate.OutMove = m_RightPlateform.GlueStopCylinder.IsOutMove;
            GlueUpOperate.InOrigin = m_RightPlateform.GlueUpCylinder.OutOriginStatus;
            GlueUpOperate.InMove = m_RightPlateform.GlueUpCylinder.OutMoveStatus;
            GlueUpOperate.OutMove = m_RightPlateform.GlueUpCylinder.IsOutMove;

            //载具平台
            MoveOperate.InOrigin = m_Assembly.MoveCylinder.OutOriginStatus;
            MoveOperate.InMove = m_Assembly.MoveCylinder.OutMoveStatus;
            MoveOperate.OutMove = m_Assembly.MoveCylinder.IsOutMove;
            CarrierUpOperate.InOrigin = m_Assembly.CarrierUpCylinder.OutOriginStatus;
            CarrierUpOperate.InMove = m_Assembly.CarrierUpCylinder.OutMoveStatus;
            CarrierUpOperate.OutMove = m_Assembly.CarrierUpCylinder.IsOutMove;
            CarrierClampOperate.InOrigin = m_Assembly.CarrierClampCylinder.OutOriginStatus;
            CarrierClampOperate.InMove = m_Assembly.CarrierClampCylinder.OutMoveStatus;
            CarrierClampOperate.OutMove = m_Assembly.CarrierClampCylinder.IsOutMove;
            CarrierStopOperate.InOrigin = m_Assembly.CarrierStopCylinder.OutOriginStatus;
            CarrierStopOperate.InMove = m_Assembly.CarrierStopCylinder.OutMoveStatus;
            CarrierStopOperate.OutMove = m_Assembly.CarrierStopCylinder.IsOutMove;

            #endregion

            #region 图像显示
            //输送装置信号显示
            txtCarrierCallOut.BackColor = Marking.CarrierCallOut ? Color.LimeGreen : SystemColors.Control;
            txtCarrierCallOutFinish.BackColor = Marking.CarrierCallOutFinish ? Color.LimeGreen : SystemColors.Control;
            txtCarrierCallIn.BackColor = Marking.CarrierCallIn ? Color.LimeGreen : SystemColors.Control; ;
            txtCarrierCallInFinish.BackColor = Marking.CarrierCallInFinish ? Color.LimeGreen : SystemColors.Control;
            //清洗工位信号显示
            txtCleanCallIn.BackColor = Marking.CleanCallIn ? Color.LimeGreen : SystemColors.Control;
            txtCleanCallInFinish.BackColor = Marking.CleanHaveProduct ? Color.LimeGreen : SystemColors.Control;
            txtCleanWorking.BackColor = Marking.CleanWorking ? Color.LimeGreen : SystemColors.Control;
            txtCleanHoming.BackColor = Marking.CleanHoming ? Color.LimeGreen : SystemColors.Control;
            txtCleanOver.BackColor = Marking.CleanWorkFinish ? Color.LimeGreen : SystemColors.Control;
            txtCleanCallOut.BackColor = Marking.CleanCallOut ? Color.LimeGreen : SystemColors.Control;
            txtCleanCallOutFinish.BackColor = Marking.CleanCallOutFinish ? Color.LimeGreen : SystemColors.Control;
            //点胶工位信号显示
            txtGlueCallIn.BackColor = Marking.GlueCallIn ? Color.LimeGreen : SystemColors.Control;
            txtGlueCallInFinish.BackColor = Marking.GlueHaveProduct ? Color.LimeGreen : SystemColors.Control;
            txtGlueWorking.BackColor = Marking.GlueWorking ? Color.LimeGreen : SystemColors.Control;
            txtGlueHoming.BackColor = Marking.GlueHoming ? Color.LimeGreen : SystemColors.Control;
            txtGlueOver.BackColor = Marking.GlueWorkFinish ? Color.LimeGreen : SystemColors.Control;
            txtGlueCallOut.BackColor = Marking.GlueCallOut ? Color.LimeGreen : SystemColors.Control;
            txtGlueCallOutFinish.BackColor = Marking.GlueCallOutFinish ? Color.LimeGreen : SystemColors.Control;

            txtCleanResult.BackColor = Marking.CleanResult ? Color.LimeGreen : Color.Red;
            txtGlueResult.BackColor = Marking.GlueCheckResult ? Color.LimeGreen : Color.Red;

            txtFN.Text = Marking.FN;
            txtSN.Text = Marking.SN;
            #endregion

            timer1.Enabled = true;
        }
    }
}
