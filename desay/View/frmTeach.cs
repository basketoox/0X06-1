using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Motion.Enginee;
using System.Threading;
using Motion.AdlinkAps;
using Motion.Interfaces;
using log4net;
using System.IO;
using System.Toolkit.Helpers;
using desay.Flow;
using desay.ProductData;
using System.Toolkit;
using System.Diagnostics;

namespace desay
{
    public partial class frmTeach : Form
    {
        static ILog log = LogManager.GetLogger(typeof(frmTeach));
        GluePlateform m_GluePlateform;
        CleanPlateform m_CleanPlateform;
        Carrier m_Carrier;
        #region 气缸
        private CylinderOperate CleanRotateOperate, CleanClampOperate, CleanUpDownOperate;
        private CylinderOperate LightUpDownOperate, LightingTestOperate;
        private CylinderOperate CleanStopOperate, CleanUpOperate;
        private CylinderOperate GlueStopOperate, GlueUpOperate;
        private CylinderOperate MoveOperate, CarrierUpOperate, CarrierClampOperate, CarrierPressOperate, CarrierStopOperate;
        #endregion

        public StationInitialize stationInitialize;
        public StationOperate stationOperate;
        public External externalSign;
        public event Action<int> SendRequest;
        public frmTeach()
        {
            InitializeComponent();
        }

        public frmTeach(GluePlateform glueplateform, CleanPlateform cleanPlateform, Carrier carrier) : this()
        {
            m_GluePlateform = glueplateform;
            m_CleanPlateform = cleanPlateform;
            m_Carrier = carrier;
        }
        private void frmTeach_Load(object sender, EventArgs e)
        {
            #region 加载页面参数           
            if (Position.Instance.UseRectGlue)
            {
                btnRect.Enabled = true;
                btnVIRect.Enabled = true;
                btnArcMove.Enabled = false;
                btnCamGlue.Enabled = false;
            }
            else
            {
                btnRect.Enabled = false;
                btnVIRect.Enabled = false;
                btnArcMove.Enabled = true;
                btnCamGlue.Enabled = true;
            }
            nudTimeDelay.Value = Config.Instance.GlueRectNOoneDelayTime;
            nudRectX1.Value = (decimal)Config.Instance.RectX[0];
            nudRectX2.Value = (decimal)Config.Instance.RectX[1];
            nudRectX3.Value = (decimal)Config.Instance.RectX[2];
            nudRectX4.Value = (decimal)Config.Instance.RectX[3];
            nudRectX5.Value = (decimal)Config.Instance.RectX[4];
            nudRectY1.Value = (decimal)Config.Instance.RectY[0];
            nudRectY2.Value = (decimal)Config.Instance.RectY[1];
            nudRectY3.Value = (decimal)Config.Instance.RectY[2];
            nudRectY4.Value = (decimal)Config.Instance.RectY[3];
            nudRectY5.Value = (decimal)Config.Instance.RectY[4];
            nudRectZ.Value = (decimal)Config.Instance.RectZ;
            InitdgvCleanPositionRows();
            InitdgvGluePositionRows();
            lblJogSpeed.Text = "点动速度:" + tbrJogSpeed.Value.ToString("0.00") + "mm/s";

            //清洗气缸
            CleanStopOperate = new CylinderOperate(() => { m_CleanPlateform.CleanStopCylinder.ManualExecute(); }) { CylinderName = "清洗阻挡气缸" };
            CleanUpOperate = new CylinderOperate(() => { m_CleanPlateform.CleanUpCylinder.ManualExecute(); }) { CylinderName = "清洗顶升气缸" };
            CleanClampOperate = new CylinderOperate(() => { m_CleanPlateform.CleanClampCylinder.ManualExecute(); }) { CylinderName = "清洗夹紧气缸" };
            CleanRotateOperate = new CylinderOperate(() => { m_CleanPlateform.CleanRotateCylinder.ManualExecute(); }) { CylinderName = "清洗旋转气缸" };
            CleanUpDownOperate = new CylinderOperate(() => { m_CleanPlateform.CleanUpDownCylinder.ManualExecute(); }) { CylinderName = "清洗上下气缸" };
            LightUpDownOperate = new CylinderOperate(() => { m_CleanPlateform.LightUpDownCylinder.ManualExecute(); }) { CylinderName = "光源上下气缸" };
            //LightingTestOperate = new CylinderOperate(() => { m_CleanPlateform.LightingTestCylinder.ManualExecute(); }) { CylinderName = "点亮测试气缸" };

            //点胶气缸
            GlueStopOperate = new CylinderOperate(() => { m_GluePlateform.GlueStopCylinder.ManualExecute(); }) { CylinderName = "点胶阻挡气缸" };
            GlueUpOperate = new CylinderOperate(() => { m_GluePlateform.GlueUpCylinder.ManualExecute(); }) { CylinderName = "点胶顶升气缸" };

            //载具气缸
            MoveOperate = new CylinderOperate(() => { m_Carrier.MoveCylinder.ManualExecute(); }) { CylinderName = "载具移动气缸" };
            CarrierUpOperate = new CylinderOperate(() => { m_Carrier.CarrierUpCylinder.ManualExecute(); }) { CylinderName = "载具顶升气缸" };
            CarrierClampOperate = new CylinderOperate(() => { m_Carrier.CarrierClampCylinder.ManualExecute(); }) { CylinderName = "载具开夹气缸" };
            CarrierPressOperate = new CylinderOperate(() => { m_Carrier.CarrierPressCylinder.ManualExecute(); }) { CylinderName = "产品下压气缸" };
            CarrierStopOperate = new CylinderOperate(() => { m_Carrier.CarrierStopCylinder.ManualExecute(); }) { CylinderName = "回流线阻挡气缸" };

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
            flpCylinder.Controls.Add(CarrierPressOperate);
            flpCylinder.Controls.Add(CarrierStopOperate);
            #endregion

            timer1.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            #region 气缸状态刷新
            //清洗平台
            CleanStopOperate.InOrigin = m_CleanPlateform.CleanStopCylinder.OutOriginStatus;
            CleanStopOperate.InMove = m_CleanPlateform.CleanStopCylinder.OutMoveStatus;
            CleanStopOperate.OutMove = m_CleanPlateform.CleanStopCylinder.IsOutMove;
            CleanUpOperate.InOrigin = m_CleanPlateform.CleanUpCylinder.OutOriginStatus;
            CleanUpOperate.InMove = m_CleanPlateform.CleanUpCylinder.OutMoveStatus;
            CleanUpOperate.OutMove = m_CleanPlateform.CleanUpCylinder.IsOutMove;
            CleanClampOperate.InOrigin = m_CleanPlateform.CleanClampCylinder.OutOriginStatus;
            CleanClampOperate.InMove = m_CleanPlateform.CleanClampCylinder.OutMoveStatus;
            CleanClampOperate.OutMove = m_CleanPlateform.CleanClampCylinder.IsOutMove;
            CleanRotateOperate.InOrigin = m_CleanPlateform.CleanRotateCylinder.OutOriginStatus;
            CleanRotateOperate.InMove = m_CleanPlateform.CleanRotateCylinder.OutMoveStatus;
            CleanRotateOperate.OutMove = m_CleanPlateform.CleanRotateCylinder.IsOutMove;
            CleanUpDownOperate.InOrigin = m_CleanPlateform.CleanUpDownCylinder.OutOriginStatus;
            CleanUpDownOperate.InMove = m_CleanPlateform.CleanUpDownCylinder.OutMoveStatus;
            CleanUpDownOperate.OutMove = m_CleanPlateform.CleanUpDownCylinder.IsOutMove;
            LightUpDownOperate.InOrigin = m_CleanPlateform.LightUpDownCylinder.OutOriginStatus;
            LightUpDownOperate.InMove = m_CleanPlateform.LightUpDownCylinder.OutMoveStatus;
            LightUpDownOperate.OutMove = m_CleanPlateform.LightUpDownCylinder.IsOutMove;
            //LightingTestOperate.InOrigin = m_CleanPlateform.LightingTestCylinder.OutOriginStatus;
            //LightingTestOperate.InMove = m_CleanPlateform.LightingTestCylinder.OutMoveStatus;
            //LightingTestOperate.OutMove = m_CleanPlateform.LightingTestCylinder.IsOutMove;

            //点胶平台
            GlueStopOperate.InOrigin = m_GluePlateform.GlueStopCylinder.OutOriginStatus;
            GlueStopOperate.InMove = m_GluePlateform.GlueStopCylinder.OutMoveStatus;
            GlueStopOperate.OutMove = m_GluePlateform.GlueStopCylinder.IsOutMove;
            GlueUpOperate.InOrigin = m_GluePlateform.GlueUpCylinder.OutOriginStatus;
            GlueUpOperate.InMove = m_GluePlateform.GlueUpCylinder.OutMoveStatus;
            GlueUpOperate.OutMove = m_GluePlateform.GlueUpCylinder.IsOutMove;

            //载具平台
            MoveOperate.InOrigin = m_Carrier.MoveCylinder.OutOriginStatus;
            MoveOperate.InMove = m_Carrier.MoveCylinder.OutMoveStatus;
            MoveOperate.OutMove = m_Carrier.MoveCylinder.IsOutMove;
            CarrierUpOperate.InOrigin = m_Carrier.CarrierUpCylinder.OutOriginStatus;
            CarrierUpOperate.InMove = m_Carrier.CarrierUpCylinder.OutMoveStatus;
            CarrierUpOperate.OutMove = m_Carrier.CarrierUpCylinder.IsOutMove;
            CarrierClampOperate.InOrigin = m_Carrier.CarrierClampCylinder.OutOriginStatus;
            CarrierClampOperate.InMove = m_Carrier.CarrierClampCylinder.OutMoveStatus;
            CarrierClampOperate.OutMove = m_Carrier.CarrierClampCylinder.IsOutMove;
            CarrierPressOperate.InOrigin = m_Carrier.CarrierPressCylinder.OutOriginStatus;
            CarrierPressOperate.InMove = m_Carrier.CarrierPressCylinder.OutMoveStatus;
            CarrierPressOperate.OutMove = m_Carrier.CarrierPressCylinder.IsOutMove;
            CarrierStopOperate.InOrigin = m_Carrier.CarrierStopCylinder.OutOriginStatus;
            CarrierStopOperate.InMove = m_Carrier.CarrierStopCylinder.OutMoveStatus;
            CarrierStopOperate.OutMove = m_Carrier.CarrierStopCylinder.IsOutMove;

            #endregion

            #region 轴状态刷新
            chxLX.Checked = m_CleanPlateform.Xaxis.IsServon;
            chxLY.Checked = m_CleanPlateform.Yaxis.IsServon;
            chxLZ.Checked = m_CleanPlateform.Zaxis.IsServon;
            chxRX.Checked = m_GluePlateform.Xaxis.IsServon;
            chxRY.Checked = m_GluePlateform.Yaxis.IsServon;
            chxRZ.Checked = m_GluePlateform.Zaxis.IsServon;

            lblLXCurrentpos.Text = m_CleanPlateform.Xaxis.CurrentPos.ToString("0.000");
            lblLYCurrentpos.Text = m_CleanPlateform.Yaxis.CurrentPos.ToString("0.000");
            lblLZCurrentpos.Text = m_CleanPlateform.Zaxis.CurrentPos.ToString("0.000");
            lblRXCurrentpos.Text = m_GluePlateform.Xaxis.CurrentPos.ToString("0.000");
            lblRYCurrentpos.Text = m_GluePlateform.Yaxis.CurrentPos.ToString("0.000");
            lblRZCurrentpos.Text = m_GluePlateform.Zaxis.CurrentPos.ToString("0.000");

            lblLXCurrentSpeed.Text = m_CleanPlateform.Xaxis.CurrentSpeed.ToString("0.000");
            lblLYCurrentSpeed.Text = m_CleanPlateform.Yaxis.CurrentSpeed.ToString("0.000");
            lblLZCurrentSpeed.Text = m_CleanPlateform.Zaxis.CurrentSpeed.ToString("0.000");
            lblRXCurrentSpeed.Text = m_GluePlateform.Xaxis.CurrentSpeed.ToString("0.000");
            lblRYCurrentSpeed.Text = m_GluePlateform.Yaxis.CurrentSpeed.ToString("0.000");
            lblRZCurrentSpeed.Text = m_GluePlateform.Zaxis.CurrentSpeed.ToString("0.000");
            #endregion

            timer1.Enabled = true;
        }

        #region JOG速度刷新
        private void tbrJogSpeed_Scroll(object sender, EventArgs e)
        {
            if (Global.IsLocating) return;
            lblJogSpeed.Text = "点动速度:" + tbrJogSpeed.Value.ToString("0.00") + "mm/s";
        }
        #endregion

        #region 数据表格初始化
        /// <summary>
        /// 数据初始化   
        /// </summary>
        private void InitdgvCleanPositionRows()
        {
            this.dgvCleanPosition.Rows.Clear();
            //in a real scenario, you may need to add different rows
            dgvCleanPosition.Rows.Add(new object[] {
                    "清洗安全位置",
                    Position.Instance.CleanSafePosition.X.ToString("0.000"),
                    Position.Instance.CleanSafePosition.Y.ToString("0.000"),
                    Position.Instance.CleanSafePosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvCleanPosition.Rows.Add(new object[] {
                    "有无料判断位置",
                    Position.Instance.LensDetectPosition.X.ToString("0.000"),
                    Position.Instance.LensDetectPosition.Y.ToString("0.000"),
                    Position.Instance.LensDetectPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvCleanPosition.Rows.Add(new object[] {
                    "白板测试位置",
                    Position.Instance.AdjustLightPosition.X.ToString("0.000"),
                    Position.Instance.AdjustLightPosition.Y.ToString("0.000"),
                    Position.Instance.AdjustLightPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvCleanPosition.Rows.Add(new object[] {
                    "清洗镜筒轨迹第一点位置",
                    Position.Instance.CleanConeFirstPosition.X.ToString("0.000"),
                    Position.Instance.CleanConeFirstPosition.Y.ToString("0.000"),
                    Position.Instance.CleanConeFirstPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvCleanPosition.Rows.Add(new object[] {
                    "清洗镜筒轨迹第二点位置",
                    Position.Instance.CleanConeSecondPosition.X.ToString("0.000"),
                    Position.Instance.CleanConeSecondPosition.Y.ToString("0.000"),
                    Position.Instance.CleanConeSecondPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvCleanPosition.Rows.Add(new object[] {
                    "清洗镜筒轨迹第三点位置",
                    Position.Instance.CleanConeThirdPositon.X.ToString("0.000"),
                    Position.Instance.CleanConeThirdPositon.Y.ToString("0.000"),
                    Position.Instance.CleanConeThirdPositon.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvCleanPosition.Rows.Add(new object[] {
                    "清洗镜筒轨迹第四点位置",
                    Position.Instance.CleanConeForthPosition.X.ToString("0.000"),
                    Position.Instance.CleanConeForthPosition.Y.ToString("0.000"),
                    Position.Instance.CleanConeForthPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvCleanPosition.Rows.Add(new object[] {
                    "清洗镜筒轨迹第五点位置",
                    Position.Instance.CleanConeFifthPosition.X.ToString("0.000"),
                    Position.Instance.CleanConeFifthPosition.Y.ToString("0.000"),
                    Position.Instance.CleanConeFifthPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvCleanPosition.Rows.Add(new object[] {
                    "清洗镜片轨迹第一点位置",
                    Position.Instance.CleanLensFirstPosition.X.ToString("0.000"),
                    Position.Instance.CleanLensFirstPosition.Y.ToString("0.000"),
                    Position.Instance.CleanLensFirstPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvCleanPosition.Rows.Add(new object[] {
                    "清洗镜片轨迹第二点位置",
                    Position.Instance.CleanLensSecondPosition.X.ToString("0.000"),
                    Position.Instance.CleanLensSecondPosition.Y.ToString("0.000"),
                    Position.Instance.CleanLensSecondPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvCleanPosition.Rows.Add(new object[] {
                    "清洗镜片轨迹第三点位置",
                    Position.Instance.CleanLensThirdPositon.X.ToString("0.000"),
                    Position.Instance.CleanLensThirdPositon.Y.ToString("0.000"),
                    Position.Instance.CleanLensThirdPositon.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvCleanPosition.Rows.Add(new object[] {
                    "清洗镜片轨迹第四点位置",
                    Position.Instance.CleanLensForthPosition.X.ToString("0.000"),
                    Position.Instance.CleanLensForthPosition.Y.ToString("0.000"),
                    Position.Instance.CleanLensForthPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvCleanPosition.Rows.Add(new object[] {
                    "清洗镜片轨迹第五点位置",
                    Position.Instance.CleanLensFifthPosition.X.ToString("0.000"),
                    Position.Instance.CleanLensFifthPosition.Y.ToString("0.000"),
                    Position.Instance.CleanLensFifthPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
        }
        private void InitdgvGluePositionRows()
        {
            this.dgvGluePosition.Rows.Clear();
            //in a real scenario, you may need to add different rows
            dgvGluePosition.Rows.Add(new object[] {
                    "点胶安全位置",
                    Position.Instance.GlueSafePosition.X.ToString("0.000"),
                    Position.Instance.GlueSafePosition.Y.ToString("0.000"),
                    Position.Instance.GlueSafePosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvGluePosition.Rows.Add(new object[] {
                    "点胶相机标定位置",
                    Position.Instance.GlueCameraCalibPosition.X.ToString("0.000"),
                    Position.Instance.GlueCameraCalibPosition.Y.ToString("0.000"),
                    Position.Instance.GlueCameraCalibPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvGluePosition.Rows.Add(new object[] {
                    "点胶相机拍照位置",
                    Position.Instance.GlueCameraPosition.X.ToString("0.000"),
                    Position.Instance.GlueCameraPosition.Y.ToString("0.000"),
                    Position.Instance.GlueCameraPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvGluePosition.Rows.Add(new object[] {
                    "点胶对针位置",
                    Position.Instance.GlueAdjustPinPosition.X.ToString("0.000"),
                    Position.Instance.GlueAdjustPinPosition.Y.ToString("0.000"),
                    Position.Instance.GlueAdjustPinPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvGluePosition.Rows.Add(new object[] {
                    "点胶割胶起始位置",
                    Position.Instance.CutGlueStartPosition.X.ToString("0.000"),
                    Position.Instance.CutGlueStartPosition.Y.ToString("0.000"),
                    Position.Instance.CutGlueStartPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvGluePosition.Rows.Add(new object[] {
                    "点胶割胶结束位置",
                    Position.Instance.CutGlueEndPosition.X.ToString("0.000"),
                    Position.Instance.CutGlueEndPosition.Y.ToString("0.000"),
                    Position.Instance.CutGlueEndPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvGluePosition.Rows.Add(new object[] {
                    "点胶轨迹开始位置",
                    Position.Instance.GlueStartPosition.X.ToString("0.000"),
                    Position.Instance.GlueStartPosition.Y.ToString("0.000"),
                    Position.Instance.GlueStartPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvGluePosition.Rows.Add(new object[] {
                    "点胶轨迹中间位置",
                    Position.Instance.GlueSecondPosition.X.ToString("0.000"),
                    Position.Instance.GlueSecondPosition.Y.ToString("0.000"),
                    Position.Instance.GlueSecondPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvGluePosition.Rows.Add(new object[] {
                    "点胶轨迹结束位置",
                    Position.Instance.GlueThirdPositon.X.ToString("0.000"),
                    Position.Instance.GlueThirdPositon.Y.ToString("0.000"),
                    Position.Instance.GlueThirdPositon.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvGluePosition.Rows.Add(new object[] {
                    "胶重点检位置",
                    Position.Instance.WeightGluePosition.X.ToString("0.000"),
                    Position.Instance.WeightGluePosition.Y.ToString("0.000"),
                    Position.Instance.WeightGluePosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
            dgvGluePosition.Rows.Add(new object[] {
                    "测高位置",
                    Position.Instance.GlueHeightPosition.X.ToString("0.000"),
                    Position.Instance.GlueHeightPosition.Y.ToString("0.000"),
                    Position.Instance.GlueHeightPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                });
        }
        #endregion

        #region 数据表格刷新
        /// <summary>
        /// 准数据刷新
        /// </summary>
        private void RefreshdgvCleanPositionRows(int i)
        {
            switch (i)
            {
                case 0:
                    dgvCleanPosition.Rows[i].SetValues(new object[] {
                        "清洗安全位置",
                        Position.Instance.CleanSafePosition.X.ToString("0.000"),
                        Position.Instance.CleanSafePosition.Y.ToString("0.000"),
                        Position.Instance.CleanSafePosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 1:
                    dgvCleanPosition.Rows[i].SetValues(new object[] {
                        "有无料判断位置",
                        Position.Instance.LensDetectPosition.X.ToString("0.000"),
                        Position.Instance.LensDetectPosition.Y.ToString("0.000"),
                        Position.Instance.LensDetectPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 2:
                    dgvCleanPosition.Rows[i].SetValues(new object[] {
                        "白板测试位置",
                        Position.Instance.AdjustLightPosition.X.ToString("0.000"),
                        Position.Instance.AdjustLightPosition.Y.ToString("0.000"),
                        Position.Instance.AdjustLightPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 3:
                    dgvCleanPosition.Rows[i].SetValues(new object[] {
                        "清洗镜筒轨迹第一点位置",
                        Position.Instance.CleanConeFirstPosition.X.ToString("0.000"),
                        Position.Instance.CleanConeFirstPosition.Y.ToString("0.000"),
                        Position.Instance.CleanConeFirstPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 4:
                    dgvCleanPosition.Rows[i].SetValues(new object[] {
                        "清洗镜筒轨迹第二点位置",
                        Position.Instance.CleanConeSecondPosition.X.ToString("0.000"),
                        Position.Instance.CleanConeSecondPosition.Y.ToString("0.000"),
                        Position.Instance.CleanConeSecondPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 5:
                    dgvCleanPosition.Rows[i].SetValues(new object[] {
                        "清洗镜筒轨迹第三点位置",
                        Position.Instance.CleanConeThirdPositon.X.ToString("0.000"),
                        Position.Instance.CleanConeThirdPositon.Y.ToString("0.000"),
                        Position.Instance.CleanConeThirdPositon.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 6:
                    dgvCleanPosition.Rows[i].SetValues(new object[] {
                        "清洗镜筒轨迹第四点位置",
                        Position.Instance.CleanConeForthPosition.X.ToString("0.000"),
                        Position.Instance.CleanConeForthPosition.Y.ToString("0.000"),
                        Position.Instance.CleanConeForthPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 7:
                    dgvCleanPosition.Rows[i].SetValues(new object[] {
                        "清洗镜筒轨迹第五点位置",
                        Position.Instance.CleanConeFifthPosition.X.ToString("0.000"),
                        Position.Instance.CleanConeFifthPosition.Y.ToString("0.000"),
                        Position.Instance.CleanConeFifthPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 8:
                    dgvCleanPosition.Rows[i].SetValues(new object[] {
                        "清洗镜片轨迹第一点位置",
                        Position.Instance.CleanLensFirstPosition.X.ToString("0.000"),
                        Position.Instance.CleanLensFirstPosition.Y.ToString("0.000"),
                        Position.Instance.CleanLensFirstPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 9:
                    dgvCleanPosition.Rows[i].SetValues(new object[] {
                        "清洗镜片轨迹第二点位置",
                        Position.Instance.CleanLensSecondPosition.X.ToString("0.000"),
                        Position.Instance.CleanLensSecondPosition.Y.ToString("0.000"),
                        Position.Instance.CleanLensSecondPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 10:
                    dgvCleanPosition.Rows[i].SetValues(new object[] {
                        "清洗镜片轨迹第三点位置",
                        Position.Instance.CleanLensThirdPositon.X.ToString("0.000"),
                        Position.Instance.CleanLensThirdPositon.Y.ToString("0.000"),
                        Position.Instance.CleanLensThirdPositon.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 11:
                    dgvCleanPosition.Rows[i].SetValues(new object[] {
                        "清洗镜片轨迹第四点位置",
                        Position.Instance.CleanLensForthPosition.X.ToString("0.000"),
                        Position.Instance.CleanLensForthPosition.Y.ToString("0.000"),
                        Position.Instance.CleanLensForthPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 12:
                    dgvCleanPosition.Rows[i].SetValues(new object[] {
                        "清洗镜片轨迹第五点位置",
                        Position.Instance.CleanLensFifthPosition.X.ToString("0.000"),
                        Position.Instance.CleanLensFifthPosition.Y.ToString("0.000"),
                        Position.Instance.CleanLensFifthPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                default: break;
            }
        }
        private void RefreshdgvRightPlatePositionRows(int i)
        {
            switch (i)
            {
                case 0:
                    dgvGluePosition.Rows[i].SetValues(new object[] {
                        "点胶安全位置",
                        Position.Instance.GlueSafePosition.X.ToString("0.000"),
                        Position.Instance.GlueSafePosition.Y.ToString("0.000"),
                        Position.Instance.GlueSafePosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 1:
                    dgvGluePosition.Rows[i].SetValues(new object[] {
                        "点胶相机标定位置",
                        Position.Instance.GlueCameraCalibPosition.X.ToString("0.000"),
                        Position.Instance.GlueCameraCalibPosition.Y.ToString("0.000"),
                        Position.Instance.GlueCameraCalibPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 2:
                    dgvGluePosition.Rows[i].SetValues(new object[] {
                        "点胶相机拍照位置",
                        Position.Instance.GlueCameraPosition.X.ToString("0.000"),
                        Position.Instance.GlueCameraPosition.Y.ToString("0.000"),
                        Position.Instance.GlueCameraPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 3:
                    dgvGluePosition.Rows[i].SetValues(new object[] {
                        "点胶对针位置",
                        Position.Instance.GlueAdjustPinPosition.X.ToString("0.000"),
                        Position.Instance.GlueAdjustPinPosition.Y.ToString("0.000"),
                        Position.Instance.GlueAdjustPinPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 4:
                    dgvGluePosition.Rows[i].SetValues(new object[] {
                        "点胶割胶起始位置",
                        Position.Instance.CutGlueStartPosition.X.ToString("0.000"),
                        Position.Instance.CutGlueStartPosition.Y.ToString("0.000"),
                        Position.Instance.CutGlueStartPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 5:
                    dgvGluePosition.Rows[i].SetValues(new object[] {
                        "点胶割胶结束位置",
                        Position.Instance.CutGlueEndPosition.X.ToString("0.000"),
                        Position.Instance.CutGlueEndPosition.Y.ToString("0.000"),
                        Position.Instance.CutGlueEndPosition.Z.ToString("0.000"),
                        "Save",
                        "GotoZ",
                        "GotoZero"
                    });
                    break;
                case 6:
                    dgvGluePosition.Rows[i].SetValues(new object[] {
                    "点胶轨迹开始位置",
                    Position.Instance.GlueStartPosition.X.ToString("0.000"),
                    Position.Instance.GlueStartPosition.Y.ToString("0.000"),
                    Position.Instance.GlueStartPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                    });
                    break;
                case 7:
                    dgvGluePosition.Rows[i].SetValues(new object[] {
                    "点胶轨迹中间位置",
                    Position.Instance.GlueSecondPosition.X.ToString("0.000"),
                    Position.Instance.GlueSecondPosition.Y.ToString("0.000"),
                    Position.Instance.GlueSecondPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                    });
                    break;
                case 8:
                    dgvGluePosition.Rows[i].SetValues(new object[] {
                    "点胶轨迹结束位置",
                    Position.Instance.GlueThirdPositon.X.ToString("0.000"),
                    Position.Instance.GlueThirdPositon.Y.ToString("0.000"),
                    Position.Instance.GlueThirdPositon.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                    });
                    break;
                case 9:
                    dgvGluePosition.Rows[i].SetValues(new object[] {
                    "胶重点检位置",
                    Position.Instance.WeightGluePosition.X.ToString("0.000"),
                    Position.Instance.WeightGluePosition.Y.ToString("0.000"),
                    Position.Instance.WeightGluePosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                    });
                    break;
                case 10:
                    dgvGluePosition.Rows[i].SetValues(new object[] {
                    "测胶高位置",
                    Position.Instance.GlueHeightPosition.X.ToString("0.000"),
                    Position.Instance.GlueHeightPosition.Y.ToString("0.000"),
                    Position.Instance.GlueHeightPosition.Z.ToString("0.000"),
                    "Save",
                    "GotoZ",
                    "GotoZero"
                    });
                    break;
                default: break;
            }
        }
        #endregion

        #region 数据表格操作
        private void dgvCleanPosition_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Global.IsLocating) return;
            var JogSpeed = (double)tbrJogSpeed.Value;
            Global.LXmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
            Global.LYmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
            Global.LZmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
            switch (e.ColumnIndex)
            {
                case 4:
                    if (MessageBox.Show($"是否保存{dgvCleanPosition.Rows[e.RowIndex].Cells[0].Value}的数据",
                        "保存", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        break;
                    if (dgvCleanPosition.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Save")
                    {
                        switch (e.RowIndex)
                        {
                            case 0://清洗安全位置
                                Position.Instance.CleanSafePosition.X = m_CleanPlateform.Xaxis.CurrentPos;
                                Position.Instance.CleanSafePosition.Y = m_CleanPlateform.Yaxis.CurrentPos;
                                Position.Instance.CleanSafePosition.Z = m_CleanPlateform.Zaxis.CurrentPos;
                                break;
                            case 1://有无料判断位置
                                Position.Instance.LensDetectPosition.X = m_CleanPlateform.Xaxis.CurrentPos;
                                Position.Instance.LensDetectPosition.Y = m_CleanPlateform.Yaxis.CurrentPos;
                                Position.Instance.LensDetectPosition.Z = m_CleanPlateform.Zaxis.CurrentPos;
                                break;
                            case 2://白板测试位置
                                Position.Instance.AdjustLightPosition.X = m_CleanPlateform.Xaxis.CurrentPos;
                                Position.Instance.AdjustLightPosition.Y = m_CleanPlateform.Yaxis.CurrentPos;
                                Position.Instance.AdjustLightPosition.Z = m_CleanPlateform.Zaxis.CurrentPos;
                                break;
                            case 3://清洗镜筒第一位置
                                Position.Instance.CleanConeFirstPosition.X = m_CleanPlateform.Xaxis.CurrentPos;
                                Position.Instance.CleanConeFirstPosition.Y = m_CleanPlateform.Yaxis.CurrentPos;
                                Position.Instance.CleanConeFirstPosition.Z = m_CleanPlateform.Zaxis.CurrentPos;
                                break;
                            case 4://清洗镜筒第二位置
                                Position.Instance.CleanConeSecondPosition.X = m_CleanPlateform.Xaxis.CurrentPos;
                                Position.Instance.CleanConeSecondPosition.Y = m_CleanPlateform.Yaxis.CurrentPos;
                                Position.Instance.CleanConeSecondPosition.Z = m_CleanPlateform.Zaxis.CurrentPos;
                                break;
                            case 5://清洗镜筒第三位置
                                Position.Instance.CleanConeThirdPositon.X = m_CleanPlateform.Xaxis.CurrentPos;
                                Position.Instance.CleanConeThirdPositon.Y = m_CleanPlateform.Yaxis.CurrentPos;
                                Position.Instance.CleanConeThirdPositon.Z = m_CleanPlateform.Zaxis.CurrentPos;
                                CalculateConeCenter();
                                break;
                            case 6://清洗镜筒第四位置
                                Position.Instance.CleanConeForthPosition.X = m_CleanPlateform.Xaxis.CurrentPos;
                                Position.Instance.CleanConeForthPosition.Y = m_CleanPlateform.Yaxis.CurrentPos;
                                Position.Instance.CleanConeForthPosition.Z = m_CleanPlateform.Zaxis.CurrentPos;
                                break;
                            case 7://清洗镜筒第五位置
                                Position.Instance.CleanConeFifthPosition.X = m_CleanPlateform.Xaxis.CurrentPos;
                                Position.Instance.CleanConeFifthPosition.Y = m_CleanPlateform.Yaxis.CurrentPos;
                                Position.Instance.CleanConeFifthPosition.Z = m_CleanPlateform.Zaxis.CurrentPos;
                                break;
                            case 8://清洗镜片第一位置
                                Position.Instance.CleanLensFirstPosition.X = m_CleanPlateform.Xaxis.CurrentPos;
                                Position.Instance.CleanLensFirstPosition.Y = m_CleanPlateform.Yaxis.CurrentPos;
                                Position.Instance.CleanLensFirstPosition.Z = m_CleanPlateform.Zaxis.CurrentPos;
                                break;
                            case 9://清洗镜片第二位置
                                Position.Instance.CleanLensSecondPosition.X = m_CleanPlateform.Xaxis.CurrentPos;
                                Position.Instance.CleanLensSecondPosition.Y = m_CleanPlateform.Yaxis.CurrentPos;
                                Position.Instance.CleanLensSecondPosition.Z = m_CleanPlateform.Zaxis.CurrentPos;
                                break;
                            case 10://清洗镜片第三位置
                                Position.Instance.CleanLensThirdPositon.X = m_CleanPlateform.Xaxis.CurrentPos;
                                Position.Instance.CleanLensThirdPositon.Y = m_CleanPlateform.Yaxis.CurrentPos;
                                Position.Instance.CleanLensThirdPositon.Z = m_CleanPlateform.Zaxis.CurrentPos;
                                CalculateLensCenter();
                                break;
                            case 11://清洗镜片第四位置
                                Position.Instance.CleanLensForthPosition.X = m_CleanPlateform.Xaxis.CurrentPos;
                                Position.Instance.CleanLensForthPosition.Y = m_CleanPlateform.Yaxis.CurrentPos;
                                Position.Instance.CleanLensForthPosition.Z = m_CleanPlateform.Zaxis.CurrentPos;
                                break;
                            case 12://清洗镜片第五位置
                                Position.Instance.CleanLensFifthPosition.X = m_CleanPlateform.Xaxis.CurrentPos;
                                Position.Instance.CleanLensFifthPosition.Y = m_CleanPlateform.Yaxis.CurrentPos;
                                Position.Instance.CleanLensFifthPosition.Z = m_CleanPlateform.Zaxis.CurrentPos;
                                break;
                            default: break;
                        }
                        RefreshdgvCleanPositionRows(e.RowIndex);

                        SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
                    }
                    break;
                case 5:
                    if (MessageBox.Show($"是否定位到{dgvCleanPosition.Rows[e.RowIndex].Cells[0].Value},Z位置",
                        "确认", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        break;
                    if (dgvCleanPosition.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "GotoZ")
                    {
                        var ret = 0;
                        switch (e.RowIndex)
                        {
                            case 0://清洗安全位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanSafePosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanSafePosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, Position.Instance.CleanSafePosition.Z, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 1://有无料判断位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.LensDetectPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.LensDetectPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, Position.Instance.LensDetectPosition.Z, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 2://白板测试位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.AdjustLightPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.AdjustLightPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, Position.Instance.AdjustLightPosition.Z, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 3://清洗镜筒轨迹第一点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanConeFirstPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanConeFirstPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, Position.Instance.CleanConeFirstPosition.Z, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 4://清洗镜筒轨迹第二点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanConeSecondPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanConeSecondPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, Position.Instance.CleanConeSecondPosition.Z, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 5://清洗镜筒轨迹第三点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanConeThirdPositon.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanConeThirdPositon.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, Position.Instance.CleanConeThirdPositon.Z, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 6://清洗镜筒轨迹第四点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanConeForthPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanConeForthPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, Position.Instance.CleanConeForthPosition.Z, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 7://清洗镜筒轨迹第五点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanConeFifthPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanConeFifthPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, Position.Instance.CleanConeFifthPosition.Z, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 8://清洗镜片轨迹第一点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanLensFirstPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanLensFirstPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, Position.Instance.CleanLensFirstPosition.Z, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 9://清洗镜片轨迹第二点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanLensSecondPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanLensSecondPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, Position.Instance.CleanLensSecondPosition.Z, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 10://清洗镜片轨迹第三点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanLensThirdPositon.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanLensThirdPositon.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, Position.Instance.CleanLensThirdPositon.Z, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 11://清洗镜片轨迹第四点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanLensForthPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanLensForthPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, Position.Instance.CleanLensForthPosition.Z, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 12://清洗镜片轨迹第五点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanLensFifthPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanLensFifthPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, Position.Instance.CleanLensFifthPosition.Z, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            default: break;
                        }
                        switch (ret)
                        {
                            case -2:
                                MessageBox.Show("伺服定位异常失败！");
                                break;
                            case -3:
                                MessageBox.Show("伺服未使能,或伺服状态不在安全位置");
                                break;
                            case -4:
                                MessageBox.Show("伺服忙碌中！");
                                break;
                        }
                    }
                    break;
                case 6:
                    if (MessageBox.Show($"是否定位到{dgvCleanPosition.Rows[e.RowIndex].Cells[0].Value},Z=0.5位置",
                        "确认", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        break;
                    if (dgvCleanPosition.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "GotoZero")
                    {
                        var ret = 0;
                        switch (e.RowIndex)
                        {
                            case 0://清洗安全位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanSafePosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanSafePosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, 0.5, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 1://有无料判断位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.LensDetectPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.LensDetectPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, 0.5, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 2://点胶对光位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.AdjustLightPosition.X, Global.RXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.AdjustLightPosition.Y, Global.RYmanualSpeed,
                                    m_CleanPlateform.Zaxis, 0.5, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 3://清洗镜筒轨迹第一点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanConeFirstPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanConeFirstPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, 0.5, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 4://清洗镜筒轨迹第二点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanConeSecondPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanConeSecondPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, 0.5, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 5://清洗镜筒轨迹第三点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanConeThirdPositon.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanConeThirdPositon.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, 0.5, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 6://清洗镜筒轨迹第四点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanConeForthPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanConeForthPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, 0.5, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 7://清洗镜筒轨迹第五点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanConeFifthPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanConeFifthPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, 0.5, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 8://清洗镜片轨迹第一点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanLensFirstPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanLensFirstPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, 0.5, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 9://清洗镜片轨迹第二点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanLensSecondPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanLensSecondPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, 0.5, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 10://清洗镜片轨迹第三点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanLensThirdPositon.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanLensThirdPositon.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, 0.5, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 11://清洗镜片轨迹第四点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanLensForthPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanLensForthPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, 0.5, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 12://清洗镜片轨迹第五点位置
                                ret = MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanLensFifthPosition.X, Global.LXmanualSpeed,
                                    m_CleanPlateform.Yaxis, Position.Instance.CleanLensFifthPosition.Y, Global.LYmanualSpeed,
                                    m_CleanPlateform.Zaxis, 0.5, Global.LZmanualSpeed,
                                    () =>
                                    {
                                        return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                 | m_CleanPlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            default: break;
                        }
                        switch (ret)
                        {
                            case -2:
                                MessageBox.Show("伺服定位异常失败！");
                                break;
                            case -3:
                                MessageBox.Show("伺服未使能,或伺服状态不在安全位置");
                                break;
                            case -4:
                                MessageBox.Show("伺服忙碌中！");
                                break;
                        }
                    }
                    break;
                default: break;
            }
        }
        private void dgvGluePosition_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Global.IsLocating) return;
            var JogSpeed = (double)tbrJogSpeed.Value;
            Global.RXmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
            Global.RYmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
            Global.RZmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
            switch (e.ColumnIndex)
            {
                case 4:
                    if (MessageBox.Show($"是否保存{dgvGluePosition.Rows[e.RowIndex].Cells[0].Value}的数据",
                        "保存", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        break;
                    if (dgvGluePosition.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Save")
                    {
                        switch (e.RowIndex)
                        {
                            case 0://点胶安全位置
                                Position.Instance.GlueSafePosition.X = m_GluePlateform.Xaxis.CurrentPos;
                                Position.Instance.GlueSafePosition.Y = m_GluePlateform.Yaxis.CurrentPos;
                                Position.Instance.GlueSafePosition.Z = m_GluePlateform.Zaxis.CurrentPos;
                                break;
                            case 1://点胶相机标定位置
                                Position.Instance.GlueCameraCalibPosition.X = m_GluePlateform.Xaxis.CurrentPos;
                                Position.Instance.GlueCameraCalibPosition.Y = m_GluePlateform.Yaxis.CurrentPos;
                                Position.Instance.GlueCameraCalibPosition.Z = m_GluePlateform.Zaxis.CurrentPos;
                                break;
                            case 2://点胶相机拍照位置
                                Position.Instance.GlueCameraPosition.X = m_GluePlateform.Xaxis.CurrentPos;
                                Position.Instance.GlueCameraPosition.Y = m_GluePlateform.Yaxis.CurrentPos;
                                Position.Instance.GlueCameraPosition.Z = m_GluePlateform.Zaxis.CurrentPos;
                                break;
                            case 3://点胶对针位置
                                double offsetX = m_GluePlateform.Xaxis.CurrentPos - Position.Instance.GlueAdjustPinPosition.X;
                                double offsetY = m_GluePlateform.Yaxis.CurrentPos - Position.Instance.GlueAdjustPinPosition.Y;
                                double offsetZ = m_GluePlateform.Zaxis.CurrentPos - Position.Instance.GlueAdjustPinPosition.Z;
                                //Position.Instance.GlueHeight += offsetZ; 
                                //Position.Instance.GlueStartPosition.X += offsetX;
                                //Position.Instance.GlueStartPosition.Y += offsetY;
                                //Position.Instance.GlueSecondPosition.X += offsetX;
                                //Position.Instance.GlueSecondPosition.Y += offsetY;
                                //Position.Instance.GlueThirdPositon.X += offsetX;
                                //Position.Instance.GlueThirdPositon.Y += offsetY;
                                //RefreshdgvRightPlatePositionRows(6);
                                //RefreshdgvRightPlatePositionRows(7);
                                //RefreshdgvRightPlatePositionRows(8);

                                Position.Instance.GlueAdjustPinPosition.X = m_GluePlateform.Xaxis.CurrentPos;
                                Position.Instance.GlueAdjustPinPosition.Y = m_GluePlateform.Yaxis.CurrentPos;
                                Position.Instance.GlueAdjustPinPosition.Z = m_GluePlateform.Zaxis.CurrentPos;
                                break;
                            case 4://点胶割胶起始位置
                                Position.Instance.CutGlueStartPosition.X = m_GluePlateform.Xaxis.CurrentPos;
                                Position.Instance.CutGlueStartPosition.Y = m_GluePlateform.Yaxis.CurrentPos;
                                Position.Instance.CutGlueStartPosition.Z = m_GluePlateform.Zaxis.CurrentPos;
                                break;
                            case 5://点胶割胶结束位置
                                Position.Instance.CutGlueEndPosition.X = m_GluePlateform.Xaxis.CurrentPos;
                                Position.Instance.CutGlueEndPosition.Y = m_GluePlateform.Yaxis.CurrentPos;
                                Position.Instance.CutGlueEndPosition.Z = m_GluePlateform.Zaxis.CurrentPos;
                                break;
                            case 6://点胶开始位置
                                Position.Instance.GlueStartPosition.X = m_GluePlateform.Xaxis.CurrentPos;
                                Position.Instance.GlueStartPosition.Y = m_GluePlateform.Yaxis.CurrentPos;
                                Position.Instance.GlueStartPosition.Z = m_GluePlateform.Zaxis.CurrentPos;
                                break;
                            case 7://点胶中间位置
                                Position.Instance.GlueSecondPosition.X = m_GluePlateform.Xaxis.CurrentPos;
                                Position.Instance.GlueSecondPosition.Y = m_GluePlateform.Yaxis.CurrentPos;
                                Position.Instance.GlueSecondPosition.Z = m_GluePlateform.Zaxis.CurrentPos;
                                break;
                            case 8://点胶结束位置
                                Position.Instance.GlueThirdPositon.X = m_GluePlateform.Xaxis.CurrentPos;
                                Position.Instance.GlueThirdPositon.Y = m_GluePlateform.Yaxis.CurrentPos;
                                Position.Instance.GlueThirdPositon.Z = m_GluePlateform.Zaxis.CurrentPos;
                                CalculateGlueCenter();
                                break;
                            case 9://胶重点检位置
                                Position.Instance.WeightGluePosition.X = m_GluePlateform.Xaxis.CurrentPos;
                                Position.Instance.WeightGluePosition.Y = m_GluePlateform.Yaxis.CurrentPos;
                                Position.Instance.WeightGluePosition.Z = m_GluePlateform.Zaxis.CurrentPos;
                                break;
                            case 10://测胶高位置
                                Position.Instance.GlueHeightPosition.X = m_GluePlateform.Xaxis.CurrentPos;
                                Position.Instance.GlueHeightPosition.Y = m_GluePlateform.Yaxis.CurrentPos;
                                Position.Instance.GlueHeightPosition.Z = m_GluePlateform.Zaxis.CurrentPos;
                                break;
                            default: break;
                        }
                        RefreshdgvRightPlatePositionRows(e.RowIndex);

                        SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
                    }
                    break;
                case 5:
                    if (MessageBox.Show($"是否定位到{dgvGluePosition.Rows[e.RowIndex].Cells[0].Value},Z位置",
                        "定位", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        break;
                    if (dgvGluePosition.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "GotoZ")
                    {
                        var ret = 0;
                        switch (e.RowIndex)
                        {
                            case 0://点胶安全位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueSafePosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueSafePosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, Position.Instance.GlueSafePosition.Z, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 1://点胶相机标定位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueCameraCalibPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueCameraCalibPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, Position.Instance.GlueCameraCalibPosition.Z, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 2://点胶相机拍照位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueCameraPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueCameraPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, Position.Instance.GlueCameraPosition.Z, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 3://点胶对针位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueAdjustPinPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueAdjustPinPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, Position.Instance.GlueAdjustPinPosition.Z, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 4://点胶割胶起始位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.CutGlueStartPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.CutGlueStartPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, Position.Instance.CutGlueStartPosition.Z, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 5://点胶割胶结束位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.CutGlueEndPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.CutGlueEndPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, Position.Instance.CutGlueEndPosition.Z, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 6://点胶开始位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueStartPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueStartPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, Position.Instance.GlueStartPosition.Z, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 7://点胶中间位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueSecondPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueSecondPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, Position.Instance.GlueSecondPosition.Z, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 8://点胶结束位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueThirdPositon.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueThirdPositon.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, Position.Instance.GlueThirdPositon.Z, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 9://胶重点检位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.WeightGluePosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.WeightGluePosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, Position.Instance.WeightGluePosition.Z, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 10://测胶高位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueHeightPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueHeightPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, Position.Instance.GlueHeightPosition.Z, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            default: break;
                        }
                        switch (ret)
                        {
                            case -2:
                                MessageBox.Show("伺服定位异常失败！");
                                break;
                            case -3:
                                MessageBox.Show("伺服未使能,或伺服状态不在安全位置");
                                break;
                            case -4:
                                MessageBox.Show("伺服忙碌中！");
                                break;
                        }
                    }
                    break;
                case 6:
                    if (MessageBox.Show($"是否定位到{dgvGluePosition.Rows[e.RowIndex].Cells[0].Value},Z=0.5位置",
                        "保存", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                        break;
                    if (dgvGluePosition.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "GotoZero")
                    {
                        var ret = 0;
                        switch (e.RowIndex)
                        {
                            case 0://点胶安全位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueSafePosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueSafePosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, 0.5, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 1://点胶相机标定位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueCameraCalibPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueCameraCalibPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, 0.5, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 2://点胶相机拍照位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueCameraPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueCameraPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, 0.5, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 3://点胶对针位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueAdjustPinPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueAdjustPinPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, 0.5, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 4://点胶割胶起始位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.CutGlueStartPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.CutGlueStartPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, 0.5, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 5://点胶割胶结束位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.CutGlueEndPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.CutGlueEndPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, 0.5, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 6://点胶开始位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueStartPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueStartPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, 0.5, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 7://点胶中间位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueStartPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueStartPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, 0.5, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 8://点胶结束位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueThirdPositon.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueThirdPositon.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, 0.5, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 9://胶重点检位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.WeightGluePosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.WeightGluePosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, 0.5, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            case 10://测胶高位置
                                ret = MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueHeightPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.GlueHeightPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, 0.5, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                                break;
                            default: break;
                        }
                        switch (ret)
                        {
                            case -2:
                                MessageBox.Show("伺服定位异常失败！");
                                break;
                            case -3:
                                MessageBox.Show("伺服未使能,或伺服状态不在安全位置");
                                break;
                            case -4:
                                MessageBox.Show("伺服忙碌中！");
                                break;
                        }
                    }
                    break;
                default: break;
            }
        }
        #endregion

        #region 轴运动操作
        private void BtnLXdec_MouseDown(object sender, MouseEventArgs e)
        {
            picLXdec.BackColor = Color.YellowGreen;
            if (Global.IsLocating) return;
            try
            {
                var JogSpeed = (double)tbrJogSpeed.Value;
                if (moveSelectHorizontal1.MoveMode.Continue)
                {
                    m_CleanPlateform.Xaxis.Speed = JogSpeed == 0 ? 25 : JogSpeed;
                    m_CleanPlateform.Xaxis.Negative();
                }
                else
                {
                    Global.LXmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
                    m_CleanPlateform.Xaxis.MoveDelta(-1 * moveSelectHorizontal1.MoveMode.Distance, Global.LXmanualSpeed);
                }
            }
            catch (Exception ex) { }
        }
        private void BtnLXadd_MouseDown(object sender, MouseEventArgs e)
        {
            picLXadd.BackColor = Color.YellowGreen;
            if (Global.IsLocating) return;
            try
            {
                var JogSpeed = (double)tbrJogSpeed.Value;
                if (moveSelectHorizontal1.MoveMode.Continue)
                {
                    m_CleanPlateform.Xaxis.Speed = JogSpeed == 0 ? 25 : JogSpeed;
                    m_CleanPlateform.Xaxis.Postive();
                }
                else
                {
                    Global.LXmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
                    m_CleanPlateform.Xaxis.MoveDelta(1 * moveSelectHorizontal1.MoveMode.Distance, Global.LXmanualSpeed);
                }
            }
            catch (Exception ex) { }
        }
        private void BtnLX_MouseUp(object sender, MouseEventArgs e)
        {
            picLXdec.BackColor = Color.Transparent;
            picLXadd.BackColor = Color.Transparent;
            if (Global.IsLocating) return;
            try
            {
                if (!moveSelectHorizontal1.MoveMode.Continue) return;
                m_CleanPlateform.Xaxis.Stop();
            }
            catch (Exception ex) { }
        }

        private void BtnLYdec_MouseDown(object sender, MouseEventArgs e)
        {
            picLYdec.BackColor = Color.YellowGreen;
            if (Global.IsLocating) return;
            try
            {
                var JogSpeed = (double)tbrJogSpeed.Value;
                if (moveSelectHorizontal1.MoveMode.Continue)
                {
                    m_CleanPlateform.Yaxis.Speed = JogSpeed == 0 ? 25 : JogSpeed;
                    m_CleanPlateform.Yaxis.Negative();
                }
                else
                {
                    Global.LYmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
                    m_CleanPlateform.Yaxis.MoveDelta(-1 * moveSelectHorizontal1.MoveMode.Distance, Global.LYmanualSpeed);
                }
            }
            catch (Exception ex) { }
        }
        private void BtnLYadd_MouseDown(object sender, MouseEventArgs e)
        {
            picLYadd.BackColor = Color.YellowGreen;
            if (Global.IsLocating) return;
            try
            {
                var JogSpeed = (double)tbrJogSpeed.Value;
                if (moveSelectHorizontal1.MoveMode.Continue)
                {
                    m_CleanPlateform.Yaxis.Speed = JogSpeed == 0 ? 25 : JogSpeed;
                    m_CleanPlateform.Yaxis.Postive();
                }
                else
                {
                    Global.LYmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
                    m_CleanPlateform.Yaxis.MoveDelta(1 * moveSelectHorizontal1.MoveMode.Distance, Global.LYmanualSpeed);
                }
            }
            catch (Exception ex) { }
        }

        private void BtnLY_MouseUp(object sender, MouseEventArgs e)
        {
            picLYdec.BackColor = Color.Transparent;
            picLYadd.BackColor = Color.Transparent;
            if (Global.IsLocating) return;
            try
            {
                if (!moveSelectHorizontal1.MoveMode.Continue) return;
                m_CleanPlateform.Yaxis.Stop();
            }
            catch (Exception ex) { }
        }

        private void BtnLZdec_MouseDown(object sender, MouseEventArgs e)
        {
            picLZdec.BackColor = Color.YellowGreen;
            if (Global.IsLocating) return;
            try
            {
                var JogSpeed = (double)tbrJogSpeed.Value;
                if (moveSelectHorizontal1.MoveMode.Continue)
                {
                    m_CleanPlateform.Zaxis.Speed = JogSpeed == 0 ? 25 : JogSpeed;
                    m_CleanPlateform.Zaxis.Negative();
                }
                else
                {
                    Global.LZmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
                    m_CleanPlateform.Zaxis.MoveDelta(-1 * moveSelectHorizontal1.MoveMode.Distance, Global.LZmanualSpeed);
                }
            }
            catch (Exception ex) { }
        }
        private void BtnLZadd_MouseDown(object sender, MouseEventArgs e)
        {
            picLZadd.BackColor = Color.YellowGreen;
            if (Global.IsLocating) return;
            try
            {
                var JogSpeed = (double)tbrJogSpeed.Value;
                if (moveSelectHorizontal1.MoveMode.Continue)
                {
                    m_CleanPlateform.Zaxis.Speed = JogSpeed == 0 ? 25 : JogSpeed;
                    m_CleanPlateform.Zaxis.Postive();
                }
                else
                {
                    Global.LZmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
                    m_CleanPlateform.Zaxis.MoveDelta(1 * moveSelectHorizontal1.MoveMode.Distance, Global.LZmanualSpeed);
                }
            }
            catch (Exception ex) { }
        }

        private void BtnLIZ_MouseUp(object sender, MouseEventArgs e)
        {
            picLZdec.BackColor = Color.Transparent;
            picLZadd.BackColor = Color.Transparent;
            if (Global.IsLocating) return;
            try
            {
                if (!moveSelectHorizontal1.MoveMode.Continue) return;
                m_CleanPlateform.Zaxis.Stop();
            }
            catch (Exception ex) { }
        }

        private void BtnRXdec_MouseDown(object sender, MouseEventArgs e)
        {
            picRXdec.BackColor = Color.YellowGreen;
            if (Global.IsLocating) return;
            try
            {
                if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z) || cb_pinmove.Checked)
                {
                    var JogSpeed = (double)tbrJogSpeed.Value;
                    if (moveSelectHorizontal1.MoveMode.Continue)
                    {
                        m_GluePlateform.Xaxis.Speed = JogSpeed == 0 ? 25 : JogSpeed;
                        m_GluePlateform.Xaxis.Postive();
                    }
                    else
                    {
                        Global.RXmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
                        m_GluePlateform.Xaxis.MoveDelta(1 * moveSelectHorizontal1.MoveMode.Distance, Global.RXmanualSpeed);
                    }
                }
            }
            catch (Exception ex) { }
        }
        private void BtnRXadd_MouseDown(object sender, MouseEventArgs e)
        {



            picRXadd.BackColor = Color.YellowGreen;
            if (Global.IsLocating) return;
            try
            {
                if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z) || cb_pinmove.Checked)
                {
                    var JogSpeed = (double)tbrJogSpeed.Value;
                    if (moveSelectHorizontal1.MoveMode.Continue)
                    {
                        m_GluePlateform.Xaxis.Speed = JogSpeed == 0 ? 25 : JogSpeed;
                        m_GluePlateform.Xaxis.Negative();
                    }
                    else
                    {
                        Global.RXmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
                        m_GluePlateform.Xaxis.MoveDelta(-1 * moveSelectHorizontal1.MoveMode.Distance, Global.RXmanualSpeed);
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void BtnRX_MouseUp(object sender, MouseEventArgs e)
        {
            picRXdec.BackColor = Color.Transparent;
            picRXadd.BackColor = Color.Transparent;
            if (Global.IsLocating) return;
            try
            {
                if (!moveSelectHorizontal1.MoveMode.Continue) return;
                m_GluePlateform.Xaxis.Stop();
            }
            catch (Exception ex) { }
        }

        private void BtnRYdec_MouseDown(object sender, MouseEventArgs e)
        {
            picRYdec.BackColor = Color.YellowGreen;
            if (Global.IsLocating) return;
            try
            {
                if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z) || cb_pinmove.Checked)
                {
                    var JogSpeed = (double)tbrJogSpeed.Value;
                    if (moveSelectHorizontal1.MoveMode.Continue)
                    {
                        m_GluePlateform.Yaxis.Speed = JogSpeed == 0 ? 25 : JogSpeed;
                        m_GluePlateform.Yaxis.Negative();
                    }
                    else
                    {
                        Global.RYmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
                        m_GluePlateform.Yaxis.MoveDelta(-1 * moveSelectHorizontal1.MoveMode.Distance, Global.RYmanualSpeed);
                    }
                }
            }
            catch (Exception ex) { }
        }
        private void BtnRYadd_MouseDown(object sender, MouseEventArgs e)
        {
            picRYadd.BackColor = Color.YellowGreen;
            if (Global.IsLocating) return;
            try
            {
                if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z) || cb_pinmove.Checked)
                {
                    var JogSpeed = (double)tbrJogSpeed.Value;
                    if (moveSelectHorizontal1.MoveMode.Continue)
                    {
                        m_GluePlateform.Yaxis.Speed = JogSpeed == 0 ? 25 : JogSpeed;
                        m_GluePlateform.Yaxis.Postive();
                    }
                    else
                    {
                        Global.RYmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
                        m_GluePlateform.Yaxis.MoveDelta(1 * moveSelectHorizontal1.MoveMode.Distance, Global.RYmanualSpeed);
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void BtnRY_MouseUp(object sender, MouseEventArgs e)
        {
            picRYdec.BackColor = Color.Transparent;
            picRYadd.BackColor = Color.Transparent;
            if (Global.IsLocating) return;
            try
            {
                if (!moveSelectHorizontal1.MoveMode.Continue) return;
                m_GluePlateform.Yaxis.Stop();
            }
            catch (Exception ex) { }
        }

        private void BtnRZdec_MouseDown(object sender, MouseEventArgs e)
        {
            picRZdec.BackColor = Color.YellowGreen;
            if (Global.IsLocating) return;
            try
            {
                var JogSpeed = (double)tbrJogSpeed.Value;
                if (moveSelectHorizontal1.MoveMode.Continue)
                {
                    m_GluePlateform.Zaxis.Speed = JogSpeed == 0 ? 25 : JogSpeed;
                    m_GluePlateform.Zaxis.Negative();
                }
                else
                {
                    Global.RZmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
                    m_GluePlateform.Zaxis.MoveDelta(-1 * moveSelectHorizontal1.MoveMode.Distance, Global.RZmanualSpeed);
                }
            }
            catch (Exception ex) { }
        }

        private void BtnRZadd_MouseDown(object sender, MouseEventArgs e)
        {
            picRZadd.BackColor = Color.YellowGreen;
            if (Global.IsLocating) return;
            try
            {
                var JogSpeed = (double)tbrJogSpeed.Value;
                if (moveSelectHorizontal1.MoveMode.Continue)
                {
                    m_GluePlateform.Zaxis.Speed = JogSpeed == 0 ? 25 : JogSpeed;
                    m_GluePlateform.Zaxis.Postive();
                }
                else
                {
                    Global.RZmanualSpeed.Maxvel = JogSpeed == 0 ? 25 : JogSpeed;
                    m_GluePlateform.Zaxis.MoveDelta(1 * moveSelectHorizontal1.MoveMode.Distance, Global.RZmanualSpeed);
                }
            }
            catch (Exception ex) { }
        }

        private void BtnRZ_MouseUp(object sender, MouseEventArgs e)
        {
            picRZdec.BackColor = Color.Transparent;
            picRZadd.BackColor = Color.Transparent;
            if (Global.IsLocating) return;
            try
            {
                if (!moveSelectHorizontal1.MoveMode.Continue) return;
                m_GluePlateform.Zaxis.Stop();
            }
            catch (Exception ex) { }
        }
        #endregion

        #region 轴使能操作
        private void chxLX_Click(object sender, EventArgs e)
        {
            m_CleanPlateform.Xaxis.IsServon = !m_CleanPlateform.Xaxis.IsServon;
        }

        private void chxLY_Click(object sender, EventArgs e)
        {
            m_CleanPlateform.Yaxis.IsServon = !m_CleanPlateform.Yaxis.IsServon;
        }

        private void chxLZ_Click(object sender, EventArgs e)
        {
            m_CleanPlateform.Zaxis.IsServon = !m_CleanPlateform.Zaxis.IsServon;
        }

        private void chxRX_Click(object sender, EventArgs e)
        {
            m_GluePlateform.Xaxis.IsServon = !m_GluePlateform.Xaxis.IsServon;
        }

        private void chxRY_Click(object sender, EventArgs e)
        {
            m_GluePlateform.Yaxis.IsServon = !m_GluePlateform.Yaxis.IsServon;
        }

        private void chxRZ_Click(object sender, EventArgs e)
        {
            m_GluePlateform.Zaxis.IsServon = !m_GluePlateform.Zaxis.IsServon;
        }
        #endregion

        #region 单按钮操作
        private void btnCleanOpen_Click(object sender, EventArgs e)
        {
            if (!IoPoints.IDO16.Value)
            {
                IoPoints.IDO16.Value = true;
                btnCleanOpen.Text = "清洁已打开";
            }
            else
            {
                IoPoints.IDO16.Value = false;
                btnCleanOpen.Text = "清洁已关闭";
            }
        }

        private void btnCleanCone_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show("是否进行产品清洗？", "提示", MessageBoxButtons.YesNo)) return;
            if (Position.Instance.UseRectGlue)
            {
                #region 矩形清洗
                var step = 0;
                bool itrue = true;
                while (itrue)
                {
                    switch (step)
                    {
                        case 0: //移动到矩形清洗第一位置
                            MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanConeFirstPosition.X, Global.LXmanualSpeed,
                                        m_CleanPlateform.Yaxis, Position.Instance.CleanConeFirstPosition.Y, Global.LYmanualSpeed,
                                        m_CleanPlateform.Zaxis, Position.Instance.CleanConeFirstPosition.Z, Global.LZmanualSpeed,
                                        () =>
                                        {
                                            return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                     | m_CleanPlateform.stationInitialize.InitializeDone;
                                        });
                            step = 10;
                            break;
                        case 10://矩形插补移动
                            if (m_CleanPlateform.Xaxis.IsInPosition(Position.Instance.CleanConeFirstPosition.X)
                               && m_CleanPlateform.Yaxis.IsInPosition(Position.Instance.CleanConeFirstPosition.Y)
                               && m_CleanPlateform.Zaxis.IsInPosition(Position.Instance.CleanConeFirstPosition.Z))
                            {
                                m_CleanPlateform.InitBufferMode(3, (int)AxisParameter.Instance.CleanPathSpeed.Maxvel);
                                m_CleanPlateform.DoRect(3, Position.Instance.CleanConeFirstPosition.X, Position.Instance.CleanConeFirstPosition.Y, Position.Instance.CleanConeFirstPosition.Z);
                                m_CleanPlateform.DoRect(3, Position.Instance.CleanConeSecondPosition.X, Position.Instance.CleanConeSecondPosition.Y, Position.Instance.CleanConeSecondPosition.Z);
                                m_CleanPlateform.DoRect(3, Position.Instance.CleanConeThirdPositon.X, Position.Instance.CleanConeThirdPositon.Y, Position.Instance.CleanConeThirdPositon.Z);
                                m_CleanPlateform.DoRect(3, Position.Instance.CleanConeForthPosition.X, Position.Instance.CleanConeForthPosition.Y, Position.Instance.CleanConeForthPosition.Z);
                                m_CleanPlateform.DoRect(3, Position.Instance.CleanConeFirstPosition.X, Position.Instance.CleanConeFirstPosition.Y, Position.Instance.CleanConeFirstPosition.Z);
                                //清洗启动
                                m_CleanPlateform.APSptStart();
                                step = 20;
                            }
                            break;
                        case 20://回清洗安全位置
                            if (m_CleanPlateform.Xaxis.IsDone && m_CleanPlateform.Yaxis.IsDone && m_CleanPlateform.Zaxis.IsDone)
                            {
                                MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanSafePosition.X, Global.LXmanualSpeed,
                                            m_CleanPlateform.Yaxis, Position.Instance.CleanSafePosition.Y, Global.LYmanualSpeed,
                                            m_CleanPlateform.Zaxis, Position.Instance.CleanSafePosition.Z, Global.LZmanualSpeed,
                                         () =>
                                         {
                                             return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                      | m_CleanPlateform.stationInitialize.InitializeDone;
                                         });
                                step = 30;
                            }
                            break;
                        case 30://结束流程
                            if (m_CleanPlateform.Xaxis.IsDone && m_CleanPlateform.Yaxis.IsDone && m_CleanPlateform.Zaxis.IsDone)
                            {
                                itrue = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }
            else
            {
                #region  圆形镜筒清洗
                //计算镜筒圆心位置
                CalculateConeCenter();
                //镜筒尺寸
                Int32 Dimension = 2;
                Int32[] Axis_ID_Array_For_2Axes_ArcMove = new Int32[2] { m_CleanPlateform.Xaxis.NoId, m_CleanPlateform.Yaxis.NoId };
                Int32[] Center_Pos_Array = new Int32[2] { Convert.ToInt32(Position.Instance.CleanConeCenterPositionReal.X/ AxisParameter.Instance.LXTransParams.PulseEquivalent),
                Convert.ToInt32(Position.Instance.CleanConeCenterPositionReal.Y  / AxisParameter.Instance.LYTransParams.PulseEquivalent) };//去掉了除以脉冲当量的计算
                Int32 Max_Arc_Speed = 5000;
                Int32 Angle = 360;
                var step = 0;
                bool itrue = true;
                while (itrue)
                {
                    switch (step)
                    {
                        case 0://移到镜筒清洗第一位置
                            MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanConeFirstPosition.X, Global.LXmanualSpeed,
                                        m_CleanPlateform.Yaxis, Position.Instance.CleanConeFirstPosition.Y, Global.LYmanualSpeed,
                                        m_CleanPlateform.Zaxis, Position.Instance.CleanConeFirstPosition.Z, Global.LZmanualSpeed,
                                        () =>
                                        {
                                            return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                     | m_CleanPlateform.stationInitialize.InitializeDone;
                                        });
                            step = 10;
                            break;
                        case 10://清洗一圈
                            if (m_CleanPlateform.Xaxis.IsInPosition(Position.Instance.CleanConeFirstPosition.X)
                                && m_CleanPlateform.Yaxis.IsInPosition(Position.Instance.CleanConeFirstPosition.Y)
                                && m_CleanPlateform.Zaxis.IsInPosition(Position.Instance.CleanConeFirstPosition.Z))
                            {
                                IoPoints.IDO7.Value = true;
                                APS168.APS_absolute_arc_move(Dimension, Axis_ID_Array_For_2Axes_ArcMove, Center_Pos_Array, Max_Arc_Speed, Angle);
                                Thread.Sleep(200);
                                step = 20;
                            }
                            break;
                        case 20://Z轴回清洗安全位置
                            if (m_CleanPlateform.Xaxis.IsDone && m_CleanPlateform.Yaxis.IsDone)
                            {
                                IoPoints.IDO7.Value = false;
                                m_CleanPlateform.Zaxis.MoveTo(Position.Instance.CleanSafePosition.Z, AxisParameter.Instance.LZspeed);
                                step = 30;
                            }
                            break;
                        case 30://XY轴回清洗安全位置
                            if (m_CleanPlateform.Zaxis.IsInPosition(Position.Instance.CleanSafePosition.Z))
                            {
                                m_CleanPlateform.Xaxis.MoveTo(Position.Instance.CleanSafePosition.X, AxisParameter.Instance.LXspeed);
                                m_CleanPlateform.Yaxis.MoveTo(Position.Instance.CleanSafePosition.Y, AxisParameter.Instance.LYspeed);
                                step = 40;
                            }
                            break;
                        case 40://结束流程
                            if (m_CleanPlateform.Xaxis.IsDone && m_CleanPlateform.Yaxis.IsDone)
                            {
                                itrue = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }


        }

        private void btnCleanLens_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show("是否进行产品清洗？", "提示", MessageBoxButtons.YesNo)) return;
            if (Position.Instance.UseRectGlue)
            {
                #region 矩形清洗
                var step = 0;
                bool itrue = true;
                while (itrue)
                {
                    switch (step)
                    {
                        case 0: //移动到矩形清洗第一位置
                            MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanLensFirstPosition.X, Global.LXmanualSpeed,
                                        m_CleanPlateform.Yaxis, Position.Instance.CleanLensFirstPosition.Y, Global.LYmanualSpeed,
                                        m_CleanPlateform.Zaxis, Position.Instance.CleanLensFirstPosition.Z, Global.LZmanualSpeed,
                                        () =>
                                        {
                                            return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                     | m_CleanPlateform.stationInitialize.InitializeDone;
                                        });
                            step = 10;
                            break;
                        case 10://矩形插补移动
                            if (m_CleanPlateform.Xaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.X)
                             && m_CleanPlateform.Yaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.Y)
                             && m_CleanPlateform.Zaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.Z))
                            {
                                m_CleanPlateform.InitBufferMode(3, (int)AxisParameter.Instance.CleanPathSpeed.Maxvel);
                                m_CleanPlateform.DoRect(3, Position.Instance.CleanLensFirstPosition.X, Position.Instance.CleanLensFirstPosition.Y, Position.Instance.CleanLensFirstPosition.Z);
                                m_CleanPlateform.DoRect(3, Position.Instance.CleanLensSecondPosition.X, Position.Instance.CleanLensSecondPosition.Y, Position.Instance.CleanLensSecondPosition.Z);
                                m_CleanPlateform.DoRect(3, Position.Instance.CleanLensThirdPositon.X, Position.Instance.CleanLensThirdPositon.Y, Position.Instance.CleanLensThirdPositon.Z);
                                m_CleanPlateform.DoRect(3, Position.Instance.CleanLensForthPosition.X, Position.Instance.CleanLensForthPosition.Y, Position.Instance.CleanLensForthPosition.Z);
                                m_CleanPlateform.DoRect(3, Position.Instance.CleanLensFirstPosition.X, Position.Instance.CleanLensFirstPosition.Y, Position.Instance.CleanLensFirstPosition.Z);
                                //清洗启动
                                m_CleanPlateform.APSptStart();
                                step = 20;
                            }
                            break;
                        case 20://回清洗安全位置
                            if (m_CleanPlateform.Xaxis.IsDone && m_CleanPlateform.Yaxis.IsDone && m_CleanPlateform.Zaxis.IsDone)
                            {
                                MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanSafePosition.X, Global.LXmanualSpeed,
                                            m_CleanPlateform.Yaxis, Position.Instance.CleanSafePosition.Y, Global.LYmanualSpeed,
                                            m_CleanPlateform.Zaxis, Position.Instance.CleanSafePosition.Z, Global.LZmanualSpeed,
                                         () =>
                                         {
                                             return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                      | m_CleanPlateform.stationInitialize.InitializeDone;
                                         });
                                step = 30;
                            }
                            break;
                        case 30://结束流程
                            if (m_CleanPlateform.Xaxis.IsDone && m_CleanPlateform.Yaxis.IsDone && m_CleanPlateform.Zaxis.IsDone)
                            {
                                itrue = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }
            else
            {
                #region 圆形清洗
                //计算镜片圆心位置
                CalculateLensCenter();
                //镜片尺寸
                Int32 Dimension = 2;
                Int32[] Axis_ID_Array_For_2Axes_ArcMove = new Int32[2] { m_CleanPlateform.Xaxis.NoId, m_CleanPlateform.Yaxis.NoId };
                Int32[] Center_Pos_Array = new Int32[2] { Convert.ToInt32(Position.Instance.CleanLensCenterPositionReal.X/ AxisParameter.Instance.LXTransParams.PulseEquivalent),
                Convert.ToInt32(Position.Instance.CleanLensCenterPositionReal.Y/ AxisParameter.Instance.LYTransParams.PulseEquivalent) };//去掉了除以脉冲当量的计算
                Int32 Max_Arc_Speed = 5000;
                Int32 Angle = 360;
                var step = 0;
                bool itrue = true;
                while (itrue)
                {
                    switch (step)
                    {
                        case 0://移到镜筒清洗第一位置
                            MoveToPoint(m_CleanPlateform.Xaxis, Position.Instance.CleanLensFirstPosition.X, Global.LXmanualSpeed,
                                        m_CleanPlateform.Yaxis, Position.Instance.CleanLensFirstPosition.Y, Global.LYmanualSpeed,
                                        m_CleanPlateform.Zaxis, Position.Instance.CleanLensFirstPosition.Z, Global.LZmanualSpeed,
                                        () =>
                                        {
                                            return !m_CleanPlateform.stationInitialize.Running | !m_CleanPlateform.stationOperate.Running
                                                     | m_CleanPlateform.stationInitialize.InitializeDone;
                                        });
                            step = 10;
                            break;
                        case 10://清洗一圈
                            if (m_CleanPlateform.Xaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.X)
                             && m_CleanPlateform.Yaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.Y)
                             && m_CleanPlateform.Zaxis.IsInPosition(Position.Instance.CleanLensFirstPosition.Z))
                            {
                                IoPoints.IDO7.Value = true;
                                APS168.APS_absolute_arc_move(Dimension, Axis_ID_Array_For_2Axes_ArcMove, Center_Pos_Array, Max_Arc_Speed, Angle);
                                Thread.Sleep(200);
                                step = 20;
                            }
                            break;
                        case 20://Z轴回清洗安全位置
                            if (m_CleanPlateform.Xaxis.IsDone && m_CleanPlateform.Yaxis.IsDone)
                            {
                                IoPoints.IDO7.Value = false;
                                m_CleanPlateform.Zaxis.MoveTo(Position.Instance.CleanSafePosition.Z, AxisParameter.Instance.LZspeed);
                                step = 30;
                            }
                            break;
                        case 30://XY轴回清洗安全位置
                            if (m_CleanPlateform.Zaxis.IsInPosition(Position.Instance.CleanSafePosition.Z))
                            {
                                m_CleanPlateform.Xaxis.MoveTo(Position.Instance.CleanSafePosition.X, AxisParameter.Instance.LXspeed);
                                m_CleanPlateform.Yaxis.MoveTo(Position.Instance.CleanSafePosition.Y, AxisParameter.Instance.LYspeed);
                                step = 40;
                            }
                            break;
                        case 40://结束流程
                            if (m_CleanPlateform.Xaxis.IsDone && m_CleanPlateform.Yaxis.IsDone)
                            {
                                itrue = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }
        }

        private void btnGlueOpen_Click(object sender, EventArgs e)
        {
            if (!IoPoints.IDO19.Value)
            {
                IoPoints.IDO19.Value = true;
                btnGlueOpen.Text = "点胶关闭";
            }
            else
            {
                IoPoints.IDO19.Value = false;
                btnGlueOpen.Text = "点胶打开";
            }
        }

        private void btnArcMove_Click(object sender, EventArgs e)
        {
            #region 圆弧点胶
            if (DialogResult.No == MessageBox.Show("是否进行圆弧点胶？", "提示", MessageBoxButtons.YesNo)) return;
            //计算圆弧圆心位置
            CalculateGlueCenter();
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var step = 0;
                    bool itrue = true;
                    while (itrue)
                    {
                        switch (step)
                        {
                            case 0:
                                step = 50;
                                break;
                            case 50:
                                MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueStartPosition.X, Global.RXmanualSpeed,
                                            m_GluePlateform.Yaxis, Position.Instance.GlueStartPosition.Y, Global.RYmanualSpeed,
                                            m_GluePlateform.Zaxis, Position.Instance.GlueStartPosition.Z, Global.RZmanualSpeed,
                                            () =>
                                            {
                                                return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                         | m_GluePlateform.stationInitialize.InitializeDone;
                                            });
                                Thread.Sleep(500);
                                step = 60;
                                break;
                            case 60://起始空胶
                                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueStartPosition.X)
                                    && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.GlueStartPosition.Y)
                                    && m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueStartPosition.Z))
                                {
                                    //先打开点胶电磁阀
                                    if (isUseGlue)
                                    {
                                        IoPoints.IDO19.Value = true;
                                        Thread.Sleep((int)Position.Instance.StartGlueDelay);
                                    }
                                    else
                                    {
                                        IoPoints.IDO19.Value = false;
                                    }
                                    APS168.APS_absolute_arc_move(2, new Int32[2] { m_GluePlateform.Xaxis.NoId, m_GluePlateform.Yaxis.NoId }, new Int32[2]
                                { (int)((Position.Instance.GlueCenterPosition.X) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                                  (int)((Position.Instance.GlueCenterPosition.Y) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                                  (int)Position.Instance.GluePathSpeed * 1000, Position.Instance.StartGlueAngle);
                                    Thread.Sleep(10);
                                    step = 70;
                                }
                                break;
                            case 70://再点胶一圈
                                if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone
                                    && m_GluePlateform.Xaxis.CurrentSpeed == 0 && m_GluePlateform.Yaxis.CurrentSpeed == 0
                                    && m_GluePlateform.Zaxis.CurrentSpeed == 0)
                                {
                                    APS168.APS_absolute_arc_move(2, new Int32[2] { m_GluePlateform.Xaxis.NoId, m_GluePlateform.Yaxis.NoId }, new Int32[2]
                                {  (int)((Position.Instance.GlueCenterPosition.X) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                                   (int)((Position.Instance.GlueCenterPosition.Y) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                                   (int)Position.Instance.GluePathSpeed * 1000, 360);

                                    step = 80;
                                }
                                break;
                            case 80://点胶补胶
                                if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone
                                    && m_GluePlateform.Xaxis.CurrentSpeed == 0 && m_GluePlateform.Yaxis.CurrentSpeed == 0
                                    && m_GluePlateform.Zaxis.CurrentSpeed == 0)
                                {
                                    APS168.APS_absolute_arc_move(2, new Int32[2] { m_GluePlateform.Xaxis.NoId, m_GluePlateform.Yaxis.NoId }, new Int32[2]
                                    { (int)((Position.Instance.GlueCenterPosition.X) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                                      (int)((Position.Instance.GlueCenterPosition.Y) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                                      (int)Position.Instance.GluePathSpeed * 1000, Position.Instance.SecondGlueAngle);
                                    Thread.Sleep((int)Position.Instance.StopGlueDelay);
                                    step = 90;
                                }
                                break;
                            case 90://点胶拖胶
                                if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone
                                    && m_GluePlateform.Xaxis.CurrentSpeed == 0 && m_GluePlateform.Yaxis.CurrentSpeed == 0
                                    && m_GluePlateform.Zaxis.CurrentSpeed == 0)
                                {
                                    //关闭点胶电磁阀
                                    IoPoints.IDO19.Value = false;
                                    APS168.APS_absolute_move(m_GluePlateform.Zaxis.NoId, (int)((m_GluePlateform.Zaxis.CurrentPos - Position.Instance.DragGlueHeight) / AxisParameter.Instance.RYTransParams.PulseEquivalent),
                                        1000);
                                    APS168.APS_absolute_arc_move(2, new Int32[2] { m_GluePlateform.Xaxis.NoId, m_GluePlateform.Yaxis.NoId }, new Int32[2]
                                    { (int)((Position.Instance.GlueCenterPosition.X) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                                      (int)((Position.Instance.GlueCenterPosition.Y) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                                      (int)Position.Instance.DragGlueSpeed * 1000, Position.Instance.DragGlueAngle);
                                    Thread.Sleep(1);
                                    step = 100;
                                }
                                break;
                            case 100://点胶结束
                                if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone &&
                                    m_GluePlateform.Xaxis.CurrentSpeed == 0
                                    && m_GluePlateform.Yaxis.CurrentSpeed == 0 && m_GluePlateform.Zaxis.CurrentSpeed == 0)
                                {
                                    IoPoints.IDO19.Value = false;
                                    m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, Global.RZmanualSpeed);
                                    itrue = false;
                                    step = 0;
                                }
                                break;
                            default:
                                step = 0;
                                return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.ToString());
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
            #endregion
        }

        private void btnAirOpen_Click(object sender, EventArgs e)
        {
            if (!IoPoints.IDO10.Value)
            {
                IoPoints.IDO10.Value = true;
                btnAirOpen.Text = "通气屏蔽";
            }
            else
            {
                IoPoints.IDO10.Value = false;
                btnAirOpen.Text = "通气打开";
            }
        }

        #region 未引用（对针标定和胶重检查）
        private void btnConfirmNeedle_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"是否更新对针数据", "确认", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;
            Position.Instance.NeedleOffset.X = m_GluePlateform.Xaxis.CurrentPos - Position.Instance.GlueAdjustPinPosition.X;
            Position.Instance.NeedleOffset.Y = m_GluePlateform.Yaxis.CurrentPos - Position.Instance.GlueAdjustPinPosition.Y;

            SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
        }

        private void btnGlueWeight_Click(object sender, EventArgs e)
        {
            CalculateGlueCenter();
            double offsetX = Position.Instance.WeightGluePosition.X - Position.Instance.GlueStartPosition.X;
            double offsetY = Position.Instance.WeightGluePosition.Y - Position.Instance.GlueStartPosition.Y;
            var step = 0;
            bool itrue = true;
            while (itrue)
            {
                switch (step)
                {
                    case 0://移至胶重点检位
                        MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.WeightGluePosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.WeightGluePosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, Position.Instance.WeightGluePosition.Z, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                        step = 5;
                        break;
                    case 5://起始空胶
                        if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.WeightGluePosition.X)
                            && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.WeightGluePosition.Y)
                            && m_GluePlateform.Zaxis.IsInPosition(Position.Instance.WeightGluePosition.Z))
                        {
                            APS168.APS_absolute_arc_move(2, new Int32[2] { m_GluePlateform.Xaxis.NoId, m_GluePlateform.Yaxis.NoId }, new Int32[2]
                        { (int)((Position.Instance.GlueCenterPosition.X + offsetX) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                          (int)((Position.Instance.GlueCenterPosition.Y + offsetY) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                          (int)Position.Instance.GluePathSpeed * 1000, Position.Instance.StartGlueAngle);
                            Thread.Sleep(1);
                            step = 10;
                        }
                        break;
                    case 10://点胶第一圈
                        if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone
                            && m_GluePlateform.Xaxis.CurrentSpeed == 0 && m_GluePlateform.Yaxis.CurrentSpeed == 0
                            && m_GluePlateform.Zaxis.CurrentSpeed == 0)
                        {
                            APS168.APS_absolute_arc_move(2, new Int32[2] { m_GluePlateform.Xaxis.NoId, m_GluePlateform.Yaxis.NoId }, new Int32[2]
                            { (int)((Position.Instance.GlueCenterPosition.X + offsetX) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                            (int)((Position.Instance.GlueCenterPosition.Y + offsetY) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                            (int)Position.Instance.GluePathSpeed * 1000, 360);
                            IoPoints.IDO19.Value = true;
                            //Thread.Sleep(1);
                            step = 20;
                        }
                        break;
                    case 20://点胶第二圈
                        if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone
                            && m_GluePlateform.Xaxis.CurrentSpeed == 0 && m_GluePlateform.Yaxis.CurrentSpeed == 0
                            && m_GluePlateform.Zaxis.CurrentSpeed == 0)
                        {
                            APS168.APS_absolute_arc_move(2, new Int32[2] { m_GluePlateform.Xaxis.NoId, m_GluePlateform.Yaxis.NoId }, new Int32[2]
                            {  (int)((Position.Instance.GlueCenterPosition.X + offsetX) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                            (int)((Position.Instance.GlueCenterPosition.Y + offsetY) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                            (int)Position.Instance.GluePathSpeed * 1000, Position.Instance.SecondGlueAngle);
                            Thread.Sleep((int)Position.Instance.StopGlueDelay);
                            step = 30;
                        }
                        break;
                    case 30://点胶拖胶
                        if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone
                            && m_GluePlateform.Xaxis.CurrentSpeed == 0 && m_GluePlateform.Yaxis.CurrentSpeed == 0
                            && m_GluePlateform.Zaxis.CurrentSpeed == 0)
                        {
                            IoPoints.IDO19.Value = false;
                            APS168.APS_absolute_move(m_GluePlateform.Zaxis.NoId, (int)((m_GluePlateform.Zaxis.CurrentPos - Position.Instance.DragGlueHeight) / AxisParameter.Instance.RYTransParams.PulseEquivalent),
                                1000);
                            APS168.APS_absolute_arc_move(2, new Int32[2] { m_GluePlateform.Xaxis.NoId, m_GluePlateform.Yaxis.NoId }, new Int32[2]
                            { (int)((Position.Instance.GlueCenterPosition.X + offsetX) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                            (int)((Position.Instance.GlueCenterPosition.Y + offsetY) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                            (int)Position.Instance.DragGlueSpeed * 1000, Position.Instance.DragGlueAngle);
                            Thread.Sleep(1);
                            step = 40;
                        }
                        break;
                    case 40://点胶结束
                        if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone &&
                            m_GluePlateform.Xaxis.CurrentSpeed == 0
                            && m_GluePlateform.Yaxis.CurrentSpeed == 0 && m_GluePlateform.Zaxis.CurrentSpeed == 0)
                        {
                            IoPoints.IDO19.Value = false;
                            itrue = false;
                            step = 0;
                        }
                        break;

                }
            }
        }

        private void btnNeedleCalib_Click(object sender, EventArgs e)
        {
            var watchPointCT = new Stopwatch();
            watchPointCT.Start();
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var vsstep = 0;
                    int Speed, Value, X0 = 0, X1 = 0, Y0 = 0, Y1 = 0, Z0 = 0;
                    while (true)
                    {
                        if (m_GluePlateform.Xaxis.IsAlarmed || m_GluePlateform.Xaxis.IsEmg || !m_GluePlateform.Xaxis.IsServon)
                        {
                            m_GluePlateform.Xaxis.Stop();
                            LogHelper.Debug("点胶X轴异常停止，请复位！");
                            return;
                        }
                        if (m_GluePlateform.Yaxis.IsAlarmed || m_GluePlateform.Yaxis.IsEmg || !m_GluePlateform.Yaxis.IsServon)
                        {
                            m_GluePlateform.Yaxis.Stop();
                            LogHelper.Debug("点胶Y轴异常停止，请复位！");
                            return;

                        }
                        if (m_GluePlateform.Zaxis.IsAlarmed || m_GluePlateform.Zaxis.IsEmg || !m_GluePlateform.Zaxis.IsServon)
                        {
                            m_GluePlateform.Zaxis.Stop();
                            LogHelper.Debug("点胶Z轴异常停止，请复位！");
                            return;

                        }
                        if (m_GluePlateform.stationOperate.Status == StationStatus.模组报警)
                        {
                            MessageBox.Show("模组报警中");
                            return;
                        }

                        switch (vsstep)
                        {

                            case 0://移动到对针位置
                                MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueAdjustPinPosition.X, Global.RXmanualSpeed,
                                            m_GluePlateform.Yaxis, Position.Instance.GlueAdjustPinPosition.Y, Global.RYmanualSpeed,
                                            m_GluePlateform.Zaxis, Position.Instance.GlueAdjustPinPosition.Z, Global.RZmanualSpeed,
                                            () =>
                                            {
                                                return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                         | m_GluePlateform.stationInitialize.InitializeDone;
                                            });
                                vsstep = 1;
                                break;
                            case 1:

                                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueAdjustPinPosition.X)
                                    && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.GlueAdjustPinPosition.Y)
                                    && m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueAdjustPinPosition.Z))
                                {
                                    vsstep = 5;
                                }
                                break;
                            case 5://判断X方向是否位于感应区
                                if (IoPoints.IDI27.Value)
                                {
                                    watchPointCT.Restart();
                                    vsstep = 10;
                                }
                                else
                                {
                                    vsstep = 200;//判断Y方向是否在感应区
                                }
                                break;
                            case 10:
                                if (m_GluePlateform.Xaxis.CurrentPos < Position.Instance.GlueAdjustPinPosition.X + 5)
                                {
                                    Speed = 5000;
                                    Value = 1;
                                    Value *= 1;
                                    APS168.APS_relative_move(0, Value, Speed);
                                    vsstep = 20;
                                }
                                else
                                {
                                    LogHelper.Debug("点胶X轴正向偏移值不够,开始负向偏移！");
                                    vsstep = 100;
                                }
                                break;
                            case 20:
                                if (!IoPoints.IDI27.Value)
                                {

                                }
                                break;
                            case 170://XY轴移到位对针OK
                                break;
                            case 180: //Z返回原位
                                vsstep = 0;
                                return;

                            default:
                                vsstep = 0;
                                return;
                        }
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.ToString());
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        private void btnTapping_Click(object sender, EventArgs e)
        {
            var step = 0;
            bool itrue = true;
            while (itrue)
            {
                switch (step)
                {
                    case 0://移至胶重点检位
                        MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.CutGlueStartPosition.X, Global.RXmanualSpeed,
                                    m_GluePlateform.Yaxis, Position.Instance.CutGlueStartPosition.Y, Global.RYmanualSpeed,
                                    m_GluePlateform.Zaxis, Position.Instance.CutGlueStartPosition.Z, Global.RZmanualSpeed,
                                    () =>
                                    {
                                        return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                 | m_GluePlateform.stationInitialize.InitializeDone;
                                    });
                        step = 5;
                        break;
                    case 5://起始空胶
                        if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.CutGlueStartPosition.X)
                            && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.CutGlueStartPosition.Y)
                            && m_GluePlateform.Zaxis.IsInPosition(Position.Instance.CutGlueStartPosition.Z))
                        {
                            m_GluePlateform.Xaxis.MoveTo(Position.Instance.CutGlueEndPosition.X, Global.RXmanualSpeed);
                            m_GluePlateform.Yaxis.MoveTo(Position.Instance.CutGlueEndPosition.Y, Global.RYmanualSpeed);
                            Thread.Sleep(1);
                            step = 10;
                        }
                        break;
                    case 10:
                        if (m_CleanPlateform.Xaxis.IsDone && m_CleanPlateform.Yaxis.IsDone)
                        {
                            itrue = false;
                            step = 30;
                        }
                        break;
                }
            }
        }
        #endregion

        private void btnCleanXHome_Click(object sender, EventArgs e)
        {
            if (IoPoints.m_ApsController.CheckHomeDone(m_CleanPlateform.Zaxis.NoId, 0.2) != 0)
            { MessageBox.Show("清洗Z轴请先回零！"); return; }
            if (m_CleanPlateform.Zaxis.CurrentPos > Position.Instance.CleanSafePosition.Z) { MessageBox.Show("清洗Z轴不在安全位！"); return; }
            if (m_CleanPlateform.Xaxis.IsServon)
                m_CleanPlateform.Xaxis.BackHome();
        }

        private void btnCleanXStop_Click(object sender, EventArgs e)
        {
            if (m_CleanPlateform.Xaxis.IsServon)
                m_CleanPlateform.Xaxis.Stop();
        }

        private void btnCleanYHome_Click(object sender, EventArgs e)
        {
            if (IoPoints.m_ApsController.CheckHomeDone(m_CleanPlateform.Zaxis.NoId, 0.2) != 0)
            { MessageBox.Show("清洗Z轴请先回零！"); return; }
            if (m_CleanPlateform.Zaxis.CurrentPos > Position.Instance.CleanSafePosition.Z) { MessageBox.Show("清洗Z轴不在安全位！"); return; }
            if (m_CleanPlateform.Yaxis.IsServon)
                m_CleanPlateform.Yaxis.BackHome();
        }

        private void btnCleanYStop_Click(object sender, EventArgs e)
        {
            if (m_CleanPlateform.Yaxis.IsServon)
                m_CleanPlateform.Yaxis.Stop();
        }

        private void btnCleanZHome_Click(object sender, EventArgs e)
        {
            if (m_CleanPlateform.Zaxis.IsServon)
                m_CleanPlateform.Zaxis.BackHome();
        }

        private void btnCleanZStop_Click(object sender, EventArgs e)
        {
            if (m_CleanPlateform.Zaxis.IsServon)
                m_CleanPlateform.Zaxis.Stop();
        }

        private void btnGlueXHome_Click(object sender, EventArgs e)
        {
            if (IoPoints.m_ApsController.CheckHomeDone(m_GluePlateform.Zaxis.NoId, 0.2) != 0)
            { MessageBox.Show("点胶Z轴请先回零！"); return; }
            if (m_GluePlateform.Zaxis.CurrentPos > Position.Instance.GlueSafePosition.Z) { MessageBox.Show("点胶Z轴不在安全位！"); return; }
            if (m_GluePlateform.Xaxis.IsServon)
                m_GluePlateform.Xaxis.BackHome();
        }

        private void btnGlueXStop_Click(object sender, EventArgs e)
        {
            if (m_GluePlateform.Xaxis.IsServon)
                m_GluePlateform.Xaxis.Stop();
        }

        private void btnGlueYHome_Click(object sender, EventArgs e)
        {
            if (IoPoints.m_ApsController.CheckHomeDone(m_GluePlateform.Zaxis.NoId, 0.2) != 0)
            { MessageBox.Show("点胶Z轴请先回零！"); return; }
            if (m_GluePlateform.Zaxis.CurrentPos > Position.Instance.GlueSafePosition.Z) { MessageBox.Show("点胶Z轴不在安全位！"); return; }
            if (m_GluePlateform.Yaxis.IsServon)
                m_GluePlateform.Yaxis.BackHome();
        }

        private void btnGlueYStop_Click(object sender, EventArgs e)
        {
            if (m_GluePlateform.Yaxis.IsServon)
                m_GluePlateform.Yaxis.Stop();
        }

        private void btnGlueZHome_Click(object sender, EventArgs e)
        {
            if (m_GluePlateform.Zaxis.IsServon)
                m_GluePlateform.Zaxis.BackHome();
        }

        private void btnGlueRStop_Click(object sender, EventArgs e)
        {
            if (m_GluePlateform.Zaxis.IsServon)
                m_GluePlateform.Zaxis.Stop();
        }

        #endregion

        /// <summary>
        /// 测高
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            frmMain.main.m_Mes.HeightDectector.WriteDetectHeightCommand();
        }

        /// <summary>
        /// 保存矩形点胶位置和出胶延时
        /// </summary>
        private void btnSaveRect_Click(object sender, EventArgs e)
        {
            Config.Instance.RectX[0] = (double)nudRectX1.Value;
            Config.Instance.RectX[1] = (double)nudRectX2.Value;
            Config.Instance.RectX[2] = (double)nudRectX3.Value;
            Config.Instance.RectX[3] = (double)nudRectX4.Value;
            Config.Instance.RectX[4] = (double)nudRectX5.Value;
            Config.Instance.RectY[0] = (double)nudRectY1.Value;
            Config.Instance.RectY[1] = (double)nudRectY2.Value;
            Config.Instance.RectY[2] = (double)nudRectY3.Value;
            Config.Instance.RectY[3] = (double)nudRectY4.Value;
            Config.Instance.RectY[4] = (double)nudRectY5.Value;
            Config.Instance.RectZ = (double)nudRectZ.Value;
            Config.Instance.GlueRectNOoneDelayTime = (int)nudTimeDelay.Value;
            SerializerManager<Config>.Instance.Save(AppConfig.ConfigFileName, Config.Instance);
            SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
            MessageBox.Show("参数保存成功");
        }

        /// <summary>
        /// 自动拍照定位，并进行圆形点胶
        /// </summary>
        private void btnCamGlue_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show("是否进行视觉圆弧点胶？", "提示", MessageBoxButtons.YesNo)) return;

            AutoGlue();
        }

        /// <summary>
        /// 拍照定位和圆形点胶流程
        /// </summary>
        private void AutoGlue()
        {
            Point3D<double> GlueCenterPosition, GlueStartPosition, GlueSecondPosition, GlueThirdPositon;
            var _watch = new Stopwatch();
            _watch.Start();
            double glueHeightOffset = 0.0;//高度差值
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var step = 0;
                    bool itrue = true;
                    while (itrue)
                    {
                        Thread.Sleep(10);
                        switch (step)
                        {
                            case 0:
                                step = 10;
                                break;
                            case 10://Z先回安全位
                                m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, Global.RZmanualSpeed);
                                Thread.Sleep(10);
                                step = 20;
                                break;
                            case 20://检测Z轴是否在安全位，XY轴移至测高位置
                                if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                                {
                                    m_GluePlateform.Xaxis.MoveTo(Position.Instance.GlueHeightPosition.X, Global.RXmanualSpeed);
                                    m_GluePlateform.Yaxis.MoveTo(Position.Instance.GlueHeightPosition.Y, Global.RXmanualSpeed);
                                    step = 30;
                                    Thread.Sleep(10);
                                }
                                break;
                            case 30://Z轴移至测高位置
                                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueHeightPosition.X)
                                 && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.GlueHeightPosition.Y))
                                {
                                    m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueHeightPosition.Z, Global.RXmanualSpeed);
                                    step = 40;
                                }
                                break;
                            case 40://发送测高指令
                                if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueHeightPosition.Z))
                                {
                                    Marking.RequestHeightFlg = true;
                                    Thread.Sleep(1500);//测高模块需要等待一会数据稳定
                                    SendRequest(1);
                                    _watch.Restart();
                                    step = 50;
                                    Thread.Sleep(100);
                                }
                                break;
                            case 50://读取测高结果                           
                                if (Marking.GetHeightFlg)
                                {
                                    if (Marking.RequestHeightError)
                                    {
                                        MessageBox.Show("测高报警!", "异常提示", MessageBoxButtons.OK);
                                        step = 0;
                                        itrue = false;
                                    }
                                    else
                                    {
                                        glueHeightOffset = Position.Instance.DetectHeight2BaseHeight;
                                        Marking.GetHeightFlg = false;
                                        step = 60;
                                    }
                                }
                                else
                                {
                                    if (_watch.ElapsedMilliseconds / 1000.0 > 35)
                                    {
                                        _watch.Restart();
                                        step = 0;
                                        itrue = false;
                                    }
                                }
                                Thread.Sleep(10);
                                break;
                            case 60:// Z轴返回安全位
                                m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                                if (glueHeightOffset > Position.Instance.DetectHeightOffsetUp || glueHeightOffset < -Position.Instance.DetectHeightOffsetDown)
                                {
                                    MessageBox.Show("测高偏差过大!", "异常提示", MessageBoxButtons.OK);
                                    step = 0;
                                    itrue = false;
                                }
                                step = 70;
                                break;
                            case 70:// XY轴前往拍照位置
                                if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                                {
                                    m_GluePlateform.Xaxis.MoveTo(Position.Instance.GlueCameraPosition.X, Global.RXmanualSpeed);
                                    m_GluePlateform.Yaxis.MoveTo(Position.Instance.GlueCameraPosition.Y, Global.RYmanualSpeed);
                                    step = 130;
                                }
                                break;
                            case 130:// Z轴前往拍照位置
                                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueCameraPosition.X)
                                && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.GlueCameraPosition.Y))
                                {
                                    m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueCameraPosition.Z, Global.RZmanualSpeed);
                                    step = 140;
                                }
                                break;
                            case 140://CCD拍照检测  定位位置
                                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueCameraPosition.X)
                                    && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.GlueCameraPosition.Y)
                                     && (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueCameraPosition.Z)))
                                {
                                    //稳定后触发拍照信号 
                                    Thread.Sleep(500);
                                    Marking.CenterLocateTestFinish = false;
                                    Marking.CenterLocateTestSucceed = false;
                                    frmAAVision.acq.CenterLocateTestAcquire();
                                    step = 160;
                                }
                                break;
                            case 160://获取CCD结果
                                if (Marking.CenterLocateTestFinish)
                                {
                                    Marking.CenterLocateTestFinish = false;
                                    if (Marking.CenterLocateTestSucceed)
                                    {
                                        step = 170;
                                    }
                                    else
                                    {
                                        m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, AxisParameter.Instance.RZspeed);
                                        MessageBox.Show("CCD识别失败!", "异常提示", MessageBoxButtons.OK);
                                        step = 0;
                                        itrue = false;
                                    }
                                }
                                break;
                            case 170://接收数据  计算
                                Position.Instance.GlueCenterPosition.X = Position.Instance.GlueCameraPosition.X - Position.Instance.CCD2NeedleOffset.X + Position.Instance.PCB2CCDOffset.X + Position.Instance.GlueOffsetX;
                                Position.Instance.GlueCenterPosition.Y = Position.Instance.GlueCameraPosition.Y - Position.Instance.CCD2NeedleOffset.Y - Position.Instance.PCB2CCDOffset.Y + Position.Instance.GlueOffsetY;
                                Position.Instance.GlueStartPosition.X = Position.Instance.GlueCenterPosition.X - Position.Instance.GlueRadius;
                                Position.Instance.GlueStartPosition.Y = Position.Instance.GlueCenterPosition.Y;
                                Position.Instance.GlueSecondPosition.X = Position.Instance.GlueCenterPosition.X;
                                Position.Instance.GlueSecondPosition.Y = Position.Instance.GlueCenterPosition.Y - Position.Instance.GlueRadius;
                                Position.Instance.GlueThirdPositon.X = Position.Instance.GlueCenterPosition.X + Position.Instance.GlueRadius;
                                Position.Instance.GlueThirdPositon.Y = Position.Instance.GlueCenterPosition.Y;
                                step = 180;
                                break;
                            case 180:
                                if (isUseGlueZero == false) { step = 190; break; }
                                MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueStartPosition.X, Global.RXmanualSpeed,
                                            m_GluePlateform.Yaxis, Position.Instance.GlueStartPosition.Y, Global.RYmanualSpeed,
                                            m_GluePlateform.Zaxis, Position.Instance.GlueSafePosition.Z, Global.RZmanualSpeed,
                                            () =>
                                            {
                                                return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                         | m_GluePlateform.stationInitialize.InitializeDone;
                                            });
                                step = 0;
                                itrue = false;
                                break;
                            case 190:
                                MoveToPoint(m_GluePlateform.Xaxis, Position.Instance.GlueStartPosition.X, Global.RXmanualSpeed,
                                            m_GluePlateform.Yaxis, Position.Instance.GlueStartPosition.Y, Global.RYmanualSpeed,
                                            m_GluePlateform.Zaxis, glueHeightOffset + Position.Instance.GlueHeight, Global.RZmanualSpeed,
                                            () =>
                                            {
                                                return !m_GluePlateform.stationInitialize.Running | !m_GluePlateform.stationOperate.Running
                                                         | m_GluePlateform.stationInitialize.InitializeDone;
                                            });
                                Thread.Sleep(500);
                                step = 200;
                                break;
                            case 200://起始空胶
                                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueStartPosition.X)
                                    && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.GlueStartPosition.Y)
                                    && m_GluePlateform.Zaxis.IsInPosition(glueHeightOffset + Position.Instance.GlueHeight))
                                {
                                    if (isUseGlue)
                                    {
                                        IoPoints.IDO19.Value = true;
                                        Thread.Sleep((int)Position.Instance.StartGlueDelay);
                                    }
                                    else
                                    {
                                        IoPoints.IDO19.Value = false;
                                    }
                                    APS168.APS_absolute_arc_move(2, new Int32[2] { m_GluePlateform.Xaxis.NoId, m_GluePlateform.Yaxis.NoId }, new Int32[2]
                                { (int)(( Position.Instance.GlueCenterPosition.X) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                                  (int)(( Position.Instance.GlueCenterPosition.Y) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                                  (int)Position.Instance.GluePathSpeed * 1000, Position.Instance.StartGlueAngle);
                                    Thread.Sleep(10);
                                    step = 210;
                                }
                                break;
                            case 210://点胶第一圈
                                if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone
                                    && m_GluePlateform.Xaxis.CurrentSpeed == 0 && m_GluePlateform.Yaxis.CurrentSpeed == 0
                                    && m_GluePlateform.Zaxis.CurrentSpeed == 0)
                                {
                                    APS168.APS_absolute_arc_move(2, new Int32[2] { m_GluePlateform.Xaxis.NoId, m_GluePlateform.Yaxis.NoId }, new Int32[2]
                                 {  (int)(( Position.Instance.GlueCenterPosition.X) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                                    (int)(( Position.Instance.GlueCenterPosition.Y) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                                    (int)Position.Instance.GluePathSpeed * 1000, 360);
                                    step = 220;
                                }
                                break;
                            case 220://补胶
                                if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone
                                    && m_GluePlateform.Xaxis.CurrentSpeed == 0 && m_GluePlateform.Yaxis.CurrentSpeed == 0
                                    && m_GluePlateform.Zaxis.CurrentSpeed == 0)
                                {
                                    APS168.APS_absolute_arc_move(2, new Int32[2] { m_GluePlateform.Xaxis.NoId, m_GluePlateform.Yaxis.NoId }, new Int32[2]
                                 {  (int)((Position.Instance.GlueCenterPosition.X) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                                    (int)((Position.Instance.GlueCenterPosition.Y) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                                    (int)Position.Instance.GluePathSpeed * 1000, Position.Instance.SecondGlueAngle);
                                    Thread.Sleep((int)Position.Instance.StopGlueDelay);//断胶延时
                                    step = 230;
                                }
                                break;
                            case 230://点胶拖胶
                                if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone
                                    && m_GluePlateform.Xaxis.CurrentSpeed == 0 && m_GluePlateform.Yaxis.CurrentSpeed == 0
                                    && m_GluePlateform.Zaxis.CurrentSpeed == 0)
                                {
                                    IoPoints.IDO19.Value = false;//关闭胶阀
                                    APS168.APS_absolute_move(m_GluePlateform.Zaxis.NoId, (int)((m_GluePlateform.Zaxis.CurrentPos - Position.Instance.DragGlueHeight) / AxisParameter.Instance.RYTransParams.PulseEquivalent),
                                        (int)Position.Instance.DragGlueSpeed * 1000);
                                    APS168.APS_absolute_arc_move(2, new Int32[2] { m_GluePlateform.Xaxis.NoId, m_GluePlateform.Yaxis.NoId }, new Int32[2]
                                 {  (int)((Position.Instance.GlueCenterPosition.X) / AxisParameter.Instance.RXTransParams.PulseEquivalent),
                                    (int)((Position.Instance.GlueCenterPosition.Y) / AxisParameter.Instance.RYTransParams.PulseEquivalent) },
                                    (int)Position.Instance.DragGlueSpeed * 1000, Position.Instance.DragGlueAngle);
                                    Thread.Sleep(10);
                                    step = 240;
                                }
                                break;
                            case 240://点胶结束，Z轴回点胶安全位置
                                if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone &&
                                    m_GluePlateform.Xaxis.CurrentSpeed == 0 && m_GluePlateform.Yaxis.CurrentSpeed == 0 && m_GluePlateform.Zaxis.CurrentSpeed == 0)
                                {
                                    IoPoints.IDO19.Value = false;
                                    m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, Global.RZmanualSpeed);
                                    step = 250;
                                }
                                break;
                            case 250://XY轴回点胶安全位置
                                if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                                {
                                    m_GluePlateform.Xaxis.MoveTo(Position.Instance.GlueSafePosition.X, Global.RXmanualSpeed);
                                    m_GluePlateform.Yaxis.MoveTo(Position.Instance.GlueSafePosition.Y, Global.RYmanualSpeed);
                                    step = 260;
                                }
                                break;
                            case 260://流程结束
                                if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone)
                                {
                                    itrue = false;
                                    step = 0;
                                }
                                break;
                            default:
                                step = 0;
                                return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.ToString());
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 出胶
        /// </summary>
        bool isUseGlue = false;
        private void cbUseGlue_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUseGlue.Checked)
            {
                isUseGlue = true;
            }
            else
                isUseGlue = false;
        }


        bool isUseGlueZero = false;
        private void cbZero_CheckedChanged(object sender, EventArgs e)
        {
            if (cbZero.Checked)
            {
                isUseGlueZero = true;
            }
            else
                isUseGlueZero = false;
        }

        #region 移动到矩形四个角
        private void btnGoToRect1_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show($"是否矩形第一点Z轴高度为{(double)nudRectZ.Value}", "", MessageBoxButtons.YesNo)) return;
            //Task a = new Task(() =>
            //{
            m_GluePlateform.Zaxis.MoveTo(0, Global.RZmanualSpeed);
            while (true)
            {
                Thread.Sleep(10);
                if (m_GluePlateform.Zaxis.IsInPosition(0)
                   )
                {

                    break;
                }


            }
            m_GluePlateform.Xaxis.MoveTo((double)nudRectX1.Value, Global.RXmanualSpeed);
            m_GluePlateform.Yaxis.MoveTo((double)nudRectY1.Value, Global.RYmanualSpeed);

            while (true)
            {
                Thread.Sleep(10);
                if (m_GluePlateform.Xaxis.IsInPosition((double)nudRectX1.Value) && m_GluePlateform.Yaxis.IsInPosition((double)nudRectY1.Value)
                   )
                {

                    break;
                }


            }
            m_GluePlateform.Zaxis.MoveTo((double)nudRectZ.Value, Global.RZmanualSpeed);
            //});
        }

        private void btnGoToRect2_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show($"是否矩形第二点Z轴高度为{(double)nudRectZ.Value}", "", MessageBoxButtons.YesNo)) return;
            //Task a = new Task(() =>
            //{
            m_GluePlateform.Zaxis.MoveTo(0, Global.RZmanualSpeed);
            while (true)
            {
                Thread.Sleep(10);
                if (m_GluePlateform.Zaxis.IsInPosition(0)
                   )
                {

                    break;
                }


            }
            m_GluePlateform.Xaxis.MoveTo((double)nudRectX2.Value, Global.RXmanualSpeed);
            m_GluePlateform.Yaxis.MoveTo((double)nudRectY2.Value, Global.RYmanualSpeed);

            while (true)
            {
                Thread.Sleep(10);
                if (m_GluePlateform.Xaxis.IsInPosition((double)nudRectX2.Value) && m_GluePlateform.Yaxis.IsInPosition((double)nudRectY2.Value)
                   )
                {

                    break;
                }


            }
            m_GluePlateform.Zaxis.MoveTo((double)nudRectZ.Value, Global.RZmanualSpeed);
            //});
        }

        private void btnGoToRect3_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show($"是否矩形第三点Z轴高度为{(double)nudRectZ.Value}", "", MessageBoxButtons.YesNo)) return;
            //Task a = new Task(() =>
            //{
            m_GluePlateform.Zaxis.MoveTo(0, Global.RZmanualSpeed);
            while (true)
            {
                Thread.Sleep(10);
                if (m_GluePlateform.Zaxis.IsInPosition(0)
                   )
                {

                    break;
                }


            }
            m_GluePlateform.Xaxis.MoveTo((double)nudRectX3.Value, Global.RXmanualSpeed);
            m_GluePlateform.Yaxis.MoveTo((double)nudRectY3.Value, Global.RYmanualSpeed);

            while (true)
            {
                Thread.Sleep(10);
                if (m_GluePlateform.Xaxis.IsInPosition((double)nudRectX3.Value) && m_GluePlateform.Yaxis.IsInPosition((double)nudRectY3.Value)
                   )
                {

                    break;
                }


            }
            m_GluePlateform.Zaxis.MoveTo((double)nudRectZ.Value, Global.RZmanualSpeed);
            //});
        }

        private void btnGoTorect4_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show($"是否矩形第四点Z轴高度为{(double)nudRectZ.Value}", "", MessageBoxButtons.YesNo)) return;
            //Task a = new Task(() =>
            //{
            m_GluePlateform.Zaxis.MoveTo(0, Global.RZmanualSpeed);
            while (true)
            {
                Thread.Sleep(10);
                if (m_GluePlateform.Zaxis.IsInPosition(0)
                   )
                {

                    break;
                }


            }
            m_GluePlateform.Xaxis.MoveTo((double)nudRectX4.Value, Global.RXmanualSpeed);
            m_GluePlateform.Yaxis.MoveTo((double)nudRectY4.Value, Global.RYmanualSpeed);

            while (true)
            {
                Thread.Sleep(10);
                if (m_GluePlateform.Xaxis.IsInPosition((double)nudRectX4.Value) && m_GluePlateform.Yaxis.IsInPosition((double)nudRectY4.Value)
                   )
                {

                    break;
                }


            }
            m_GluePlateform.Zaxis.MoveTo((double)nudRectZ.Value, Global.RZmanualSpeed);
            //});
        }
        #endregion

        /// <summary>
        /// 矩形点胶
        /// </summary>
        private void btnRect_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show("是否进行矩形点胶", "", MessageBoxButtons.YesNo)) return;

            try
            {
                int step = 0;
                bool iTrue = true;
                while (iTrue)
                {
                    Thread.Sleep(10);
                    switch (step)
                    {
                        case 0://Z轴移到点胶安全位置
                            m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, Global.RZmanualSpeed);
                            step = 10;
                            break;
                        case 10://XY轴移到矩形第一个点
                            if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                            {
                                m_GluePlateform.Xaxis.MoveTo((double)nudRectX1.Value, Global.RXmanualSpeed);
                                m_GluePlateform.Yaxis.MoveTo((double)nudRectY1.Value, Global.RYmanualSpeed);
                                step = 20;
                            }
                            break;
                        case 20://Z轴移到点胶位置
                            if (m_GluePlateform.Xaxis.IsInPosition((double)nudRectX1.Value)
                                && m_GluePlateform.Yaxis.IsInPosition((double)nudRectY1.Value))
                            {
                                m_GluePlateform.Zaxis.MoveTo((double)nudRectZ.Value, Global.RZmanualSpeed);
                                step = 30;
                            }
                            break;
                        case 30://开始矩形插补运动
                            if (m_GluePlateform.Zaxis.IsInPosition((double)nudRectZ.Value))
                            {
                                m_GluePlateform.InitBufferMode((int)nudAxisNum.Value, (int)AxisParameter.Instance.GluePathSpeed.Maxvel);
                                m_GluePlateform.DoRect((int)nudAxisNum.Value, (double)nudRectX1.Value, (double)nudRectY1.Value, (double)nudRectZ.Value);
                                m_GluePlateform.DoRect((int)nudAxisNum.Value, (double)nudRectX2.Value, (double)nudRectY2.Value, (double)nudRectZ.Value);
                                m_GluePlateform.DoRect((int)nudAxisNum.Value, (double)nudRectX3.Value, (double)nudRectY3.Value, (double)nudRectZ.Value);
                                m_GluePlateform.DoRect((int)nudAxisNum.Value, (double)nudRectX4.Value, (double)nudRectY4.Value, (double)nudRectZ.Value);
                                m_GluePlateform.DoRect((int)nudAxisNum.Value, (double)nudRectX5.Value, (double)nudRectY5.Value, (double)nudRectZ.Value);
                                m_GluePlateform.DoRect((int)nudAxisNum.Value, (double)nudRectX1.Value, (double)nudRectY1.Value, (double)nudRectZ.Value);
                                //根据胶水情况，确定延时时间
                                m_GluePlateform.APSptStart((int)nudTimeDelay.Value, cbUseGlue.Checked);
                                //拖胶
                                m_GluePlateform.Xaxis.MoveTo((double)nudRectX4.Value, Global.RXmanualSpeed);
                                m_GluePlateform.Yaxis.MoveTo((double)nudRectY4.Value, Global.RYmanualSpeed);
                                //

                                step = 40;
                            }
                            break;
                        case 40://拖胶结束，Z轴回安全位置
                            if (m_GluePlateform.Xaxis.IsInPosition((double)nudRectX4.Value)
                                && m_GluePlateform.Yaxis.IsInPosition((double)nudRectY4.Value))
                            {
                                m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, Global.RZmanualSpeed);
                                step = 50;
                            }
                            break;
                        case 50://XY轴回安全位置
                            if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                            {
                                m_GluePlateform.Xaxis.MoveTo(Position.Instance.GlueSafePosition.X, Global.RXmanualSpeed);
                                m_GluePlateform.Yaxis.MoveTo(Position.Instance.GlueSafePosition.Y, Global.RYmanualSpeed);
                                step = 60;
                            }
                            break;
                        case 60://结束流程
                            if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueSafePosition.X)
                                && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.GlueSafePosition.Y))
                            {
                                iTrue = false;
                                step = 0;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }

        private void btnVIRect_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("是否进行矩形视觉点胶？", "提示", MessageBoxButtons.YesNo))
            {
                #region 矩形视觉点胶
                var _watch = new Stopwatch();
                _watch.Start();
                double glueHeightOffset = 0.0;//高度差值
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var step = 0;
                        bool itrue = true;
                        while (itrue)
                        {
                            Thread.Sleep(10);
                            switch (step)
                            {
                                case 0:
                                    step = 10;
                                    break;
                                case 10://Z先回安全位
                                m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, Global.RZmanualSpeed);
                                    Thread.Sleep(10);
                                    step = 20;
                                    break;
                                case 20://检测Z轴是否在安全位，XY轴移至测高位置
                                if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                                    {
                                        m_GluePlateform.Xaxis.MoveTo(Position.Instance.GlueHeightPosition.X, Global.RXmanualSpeed);
                                        m_GluePlateform.Yaxis.MoveTo(Position.Instance.GlueHeightPosition.Y, Global.RYmanualSpeed);
                                        step = 30;
                                      //step = 70;
                                    Thread.Sleep(10);
                                    }
                                    break;
                                case 30://Z轴移至测高位置
                                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueHeightPosition.X)
                                 && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.GlueHeightPosition.Y))
                                    {
                                        m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueHeightPosition.Z, Global.RXmanualSpeed);
                                        step = 40;
                                    }
                                    break;
                                case 40://发送测高指令
                                if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueHeightPosition.Z))
                                    {
                                        Marking.RequestHeightFlg = true;
                                        Thread.Sleep(1500);//测高模块需要等待一会数据稳定
                                    SendRequest(1);
                                        _watch.Restart();
                                        step = 50;
                                    }
                                    break;
                                case 50://读取测高结果                           
                                if (Marking.GetHeightFlg)
                                    {
                                        if (Marking.RequestHeightError)
                                        {
                                            MessageBox.Show("测高报警!", "异常提示", MessageBoxButtons.OK);
                                            step = 0;
                                            itrue = false;
                                        }
                                        else
                                        {
                                            glueHeightOffset = Position.Instance.DetectHeight2BaseHeight;
                                            Marking.GetHeightFlg = false;
                                            step = 60;
                                        }
                                    }
                                    else
                                    {
                                        if (_watch.ElapsedMilliseconds / 1000.0 >35)
                                        {
                                            _watch.Restart();
                                            step = 0;
                                            itrue = false;
                                        }
                                    }
                                    Thread.Sleep(10);
                                    break;
                                case 60:// Z轴返回安全位
                                m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, Global.RZmanualSpeed);
                                    if (glueHeightOffset > Position.Instance.DetectHeightOffsetUp || glueHeightOffset < -Position.Instance.DetectHeightOffsetDown)
                                    {
                                        MessageBox.Show("测高偏差过大!", "异常提示", MessageBoxButtons.OK);
                                        step = 0;
                                        itrue = false;
                                    }
                                    step = 70;
                                    break;
                                case 70:// XY轴前往拍照位置
                                if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                                    {
                                        m_GluePlateform.Xaxis.MoveTo(Position.Instance.GlueCameraPosition.X, Global.RXmanualSpeed);
                                        m_GluePlateform.Yaxis.MoveTo(Position.Instance.GlueCameraPosition.Y, Global.RYmanualSpeed);
                                        step = 130;
                                    }
                                    break;
                                case 130:// Z轴前往拍照位置
                                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueCameraPosition.X)
                                && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.GlueCameraPosition.Y))
                                    {
                                        m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueCameraPosition.Z, Global.RZmanualSpeed);
                                        step = 140;
                                    }
                                    break;
                                case 140://CCD拍照检测  定位位置
                                if (m_GluePlateform.Xaxis.IsInPosition(Position.Instance.GlueCameraPosition.X)
                                    && m_GluePlateform.Yaxis.IsInPosition(Position.Instance.GlueCameraPosition.Y)
                                     && (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueCameraPosition.Z)))
                                    {
                                    //稳定后触发拍照信号 
                                    Thread.Sleep(500);
                                        Marking.CenterLocateTestFinish = false;
                                        Marking.CenterLocateTestSucceed = false;
                                        frmAAVision.acq.CenterLocateTestAcquire();
                                        step = 160;
                                    }
                                    break;
                                case 160://获取CCD结果
                                if (Marking.CenterLocateTestFinish)
                                    {
                                        Marking.CenterLocateTestFinish = false;
                                        if (Marking.CenterLocateTestSucceed)
                                        {
                                            step = 170;
                                        }
                                        else
                                        {
                                            m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, Global.RZmanualSpeed);
                                            MessageBox.Show("CCD识别失败!", "异常提示", MessageBoxButtons.OK);
                                            step = 0;
                                            itrue = false;
                                        }
                                    }
                                    break;
                                case 170://接收数据  计算

                                    step = 180;
                                    break;
                                case 180://Z轴移到点胶安全位置
                                m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, Global.RZmanualSpeed);
                                    step = 190;
                                    break;
                                case 190://XY轴移到矩形第一个点
                                if (m_GluePlateform.Zaxis.IsInPosition(Position.Instance.GlueSafePosition.Z))
                                    {
                                        m_GluePlateform.Xaxis.MoveTo(Config.Instance.RectX[0], Global.RXmanualSpeed);
                                        m_GluePlateform.Yaxis.MoveTo(Config.Instance.RectY[0], Global.RYmanualSpeed);
                                        step = 200;
                                    }
                                    break;
                                case 200://Z轴移到第一点胶位置
                                if (m_GluePlateform.Xaxis.IsInPosition(Config.Instance.RectX[0]) && m_GluePlateform.Yaxis.IsInPosition(Config.Instance.RectY[0]))
                                    {
                                        Config.Instance.RectZ = glueHeightOffset + Position.Instance.GlueHeight;
                                        m_GluePlateform.Zaxis.MoveTo(Config.Instance.RectZ, Global.RZmanualSpeed);
                                        Thread.Sleep(10);
                                        step = 210;
                                    }
                                    break;
                                case 210://点胶，拖胶
                                if (m_GluePlateform.Zaxis.IsInPosition(Config.Instance.RectZ))
                                    {
                                        m_GluePlateform.InitBufferMode(3, (int)Position.Instance.GluePathSpeed);
                                        m_GluePlateform.DoRect(3, Config.Instance.RectX[0], Config.Instance.RectY[0], Config.Instance.RectZ, (int)Position.Instance.GluePathSpeed, (int)Position.Instance.GluePathSpeed);
                                        m_GluePlateform.DoRect(3, Config.Instance.RectX[1], Config.Instance.RectY[1], Config.Instance.RectZ, (int)Position.Instance.GluePathSpeed, (int)Position.Instance.GluePathSpeed);
                                        m_GluePlateform.DoRect(3, Config.Instance.RectX[2], Config.Instance.RectY[2], Config.Instance.RectZ, (int)Position.Instance.GluePathSpeed, (int)Position.Instance.GluePathSpeed);
                                        m_GluePlateform.DoRect(3, Config.Instance.RectX[3], Config.Instance.RectY[3], Config.Instance.RectZ, (int)Position.Instance.GluePathSpeed, (int)Position.Instance.GluePathSpeed);
                                        m_GluePlateform.DoRect(3, Config.Instance.RectX[4], Config.Instance.RectY[4], Config.Instance.RectZ, (int)Position.Instance.GluePathSpeed, (int)Position.Instance.GluePathSpeed);
                                        m_GluePlateform.DoRect(3, Config.Instance.RectX[0], Config.Instance.RectY[0] - (0.1 * Position.Instance.DragGlueAngle), Config.Instance.RectZ, (int)Position.Instance.GluePathSpeed, (int)Position.Instance.GluePathSpeed);
                                        m_GluePlateform.APSptStart((int)nudTimeDelay.Value, cbUseGlue.Checked);
                                        //拖胶前等待
                                        Thread.Sleep(500);
                                        m_GluePlateform.Yaxis.MoveTo(Config.Instance.RectY[0] + (0.2 * Position.Instance.DragGlueAngle), AxisParameter.Instance.GluePathSpeed);
                                        step = 220;
                                    }
                                    break;
                                case 220://点胶结束，Z轴回点胶安全位置
                                if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone)
                                    {
                                        IoPoints.IDO19.Value = false;
                                        m_GluePlateform.Zaxis.MoveTo(Position.Instance.GlueSafePosition.Z, Global.RZmanualSpeed);
                                        step = 230;
                                    }
                                    break;
                                case 230://流程结束
                                if (m_GluePlateform.Xaxis.IsDone && m_GluePlateform.Yaxis.IsDone && m_GluePlateform.Zaxis.IsDone)
                                    {
                                        itrue = false;
                                        step = 0;
                                    }
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex.ToString());
                    }
                }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);

                #endregion
            }

        }

        private void btnGoTorect5_Click(object sender, EventArgs e)
        {
            if (DialogResult.No == MessageBox.Show($"是否矩形第五点Z轴高度为{(double)nudRectZ.Value}", "", MessageBoxButtons.YesNo)) return;
            //Task a = new Task(() =>
            //{
            m_GluePlateform.Zaxis.MoveTo(0, Global.RZmanualSpeed);
            while (true)
            {
                Thread.Sleep(10);
                if (m_GluePlateform.Zaxis.IsInPosition(0)
                   )
                {

                    break;
                }


            }
            m_GluePlateform.Xaxis.MoveTo((double)nudRectX5.Value, Global.RXmanualSpeed);
            m_GluePlateform.Yaxis.MoveTo((double)nudRectY5.Value, Global.RYmanualSpeed);

            while (true)
            {
                Thread.Sleep(10);
                if (m_GluePlateform.Xaxis.IsInPosition((double)nudRectX5.Value) && m_GluePlateform.Yaxis.IsInPosition((double)nudRectY5.Value)
                   )
                {

                    break;
                }


            }
            m_GluePlateform.Zaxis.MoveTo((double)nudRectZ.Value, Global.RZmanualSpeed);
            //});
        }


        #region MoveToPoint的重载
        private int MoveToPoint(ApsAxis Xaxis, double X, VelocityCurve XvelocityCurve,
                                ApsAxis Zaxis, double Z, VelocityCurve ZvelocityCurve,
                                Func<bool> Condition = null)
        {
            if (!Xaxis.IsServon || !Zaxis.IsServon) return -3;
            if (!Condition()) return -4;
            Global.IsLocating = true;
            Task.Factory.StartNew(() =>
            {
                try
                {            //判断Z轴是否在零点
                    if (!Zaxis.IsInPosition(0.5))
                        Zaxis.MoveTo(0.5, ZvelocityCurve);
                    while (true)
                    {
                        Thread.Sleep(10);
                        if (Zaxis.IsInPosition(0.5)) break;
                        if (ServoAxisIsReady(Zaxis) || ServoAxisIsReady(Zaxis))
                        {
                            Zaxis.Stop();
                            Global.IsLocating = false;
                            return -2;
                        }
                    }
                    //将X、Y移动到指定位置
                    if (!Xaxis.IsInPosition(X)) Xaxis.MoveTo(X, XvelocityCurve);
                    while (true)
                    {
                        Thread.Sleep(10);
                        if (Xaxis.IsInPosition(X)) break;
                        if (ServoAxisIsReady(Xaxis))
                        {
                            Xaxis.Stop();
                            Global.IsLocating = false;
                            return -2;
                        }
                    }
                    //将Z轴移动到指定位置
                    Zaxis.MoveTo(Z, ZvelocityCurve);
                    while (true)
                    {
                        Thread.Sleep(10);
                        if (Zaxis.IsInPosition(Z)) break;
                        if (ServoAxisIsReady(Zaxis))
                        {
                            Zaxis.Stop();
                            Global.IsLocating = false;
                            return -2;
                        }
                    }
                    Global.IsLocating = false;
                    return 0;
                }
                catch (Exception ex)
                {
                    Global.IsLocating = false;
                    //log.Fatal("设备驱动程序异常", ex);
                    return -2;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
            Global.IsLocating = false;
            return 0;
        }

        private int MoveToPoint(ApsAxis Xaxis, double X, VelocityCurve XvelocityCurve,
                                Func<bool> Condition = null,
                                ApsAxis Yaxis = null, double Y = 0, VelocityCurve YvelocityCurve = null)
        {
            if (!Xaxis.IsServon || !Yaxis.IsServon) return -3;
            if (!Condition()) return -4;
            Global.IsLocating = true;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (Yaxis != null)
                    {
                        //将X、Y移动到指定位置
                        if (!Xaxis.IsInPosition(X)) Xaxis.MoveTo(X, XvelocityCurve);
                        if (!Yaxis.IsInPosition(Y)) Yaxis.MoveTo(Y, YvelocityCurve);
                        while (true)
                        {
                            Thread.Sleep(10);
                            if (Xaxis.IsInPosition(X) && Yaxis.IsInPosition(Y)) break;
                            if (ServoAxisIsReady(Xaxis) || ServoAxisIsReady(Yaxis))
                            {
                                Xaxis.Stop();
                                Yaxis.Stop();
                                Global.IsLocating = false;
                                return -2;
                            }
                        }
                    }
                    else
                    {
                        if (!Xaxis.IsInPosition(X)) Xaxis.MoveTo(X, XvelocityCurve);
                        while (true)
                        {
                            Thread.Sleep(10);
                            if (Xaxis.IsInPosition(X)) break;
                            if (ServoAxisIsReady(Xaxis))
                            {
                                Xaxis.Stop();
                                Global.IsLocating = false;
                                return -2;
                            }
                        }
                    }
                    Global.IsLocating = false;
                    return 0;
                }
                catch (Exception ex)
                {
                    Global.IsLocating = false;
                    log.Fatal("设备驱动程序异常", ex);
                    return -2;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
            Global.IsLocating = false;
            return 0;
        }

        private int MoveToPoint(ApsAxis Xaxis, double X, VelocityCurve XvelocityCurve,
                                ApsAxis Yaxis, double Y, VelocityCurve YvelocityCurve,
                                ApsAxis Zaxis = null, double Z = 0, VelocityCurve ZvelocityCurve = null,
                                Func<bool> Condition = null)
        {
            if (!Xaxis.IsServon || !Yaxis.IsServon || !Zaxis.IsServon) return -3;
            if (!Condition()) return -4;
            Global.IsLocating = true;

            Task.Factory.StartNew(() =>
            {
                try
                {            //判断Z轴是否在零点
                    if (Zaxis != null)
                    {
                        if (!Zaxis.IsInPosition(0.5))
                            Zaxis.MoveTo(0.5, ZvelocityCurve ?? new VelocityCurve()
                            {
                                Strvel = 0,
                                Maxvel = Zaxis.Speed ?? 25,
                                Tacc = 0.1,
                                Tdec = 0.1,
                                VelocityCurveType = CurveTypes.T
                            });
                        while (true)
                        {
                            Thread.Sleep(10);
                            if (Zaxis.IsInPosition(0.5)) break;
                            if (ServoAxisIsReady(Zaxis))
                            {
                                Zaxis.Stop();
                                Global.IsLocating = false;
                                return -2;
                            }
                        }
                    }
                    //将X、Y移动到指定位置
                    if (!Xaxis.IsInPosition(X)) Xaxis.MoveTo(X, XvelocityCurve);
                    if (!Yaxis.IsInPosition(Y)) Yaxis.MoveTo(Y, YvelocityCurve);
                    while (true)
                    {
                        Thread.Sleep(10);
                        if (Xaxis.IsInPosition(X) && Yaxis.IsInPosition(Y)) break;
                        if (ServoAxisIsReady(Xaxis) || ServoAxisIsReady(Yaxis))
                        {
                            Xaxis.Stop();
                            Yaxis.Stop();
                            Global.IsLocating = false;
                            return -2;
                        }
                    }
                    //将Z轴移动到指定位置
                    if (Zaxis != null)
                    {
                        Zaxis.MoveTo(Z, ZvelocityCurve ?? new VelocityCurve()
                        {
                            Strvel = 0,
                            Maxvel = Zaxis.Speed ?? 25,
                            Tacc = 0.1,
                            Tdec = 0.1,
                            VelocityCurveType = CurveTypes.T
                        });
                        while (true)
                        {
                            Thread.Sleep(10);
                            if (Zaxis.IsInPosition(Z)) break;
                            if (ServoAxisIsReady(Zaxis))
                            {
                                Zaxis.Stop();
                                Global.IsLocating = false;
                                return -2;
                            }
                        }
                        Global.IsLocating = false;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    Global.IsLocating = false;
                    log.Fatal("设备驱动程序异常", ex);
                    return -2;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
            Global.IsLocating = false;
            return 0;
        }

        private int MoveToPoint(ApsAxis Xaxis, double X, VelocityCurve XvelocityCurve,
                                ApsAxis IXaxis, double IX, VelocityCurve IXvelocityCurve,
                                ApsAxis Yaxis, double Y, VelocityCurve YvelocityCurve,
                                ApsAxis Zaxis = null, double Z = 0, VelocityCurve ZvelocityCurve = null,
                                Func<bool> Condition = null)
        {
            if (!Xaxis.IsServon || !IXaxis.IsServon ||
                !Yaxis.IsServon || !Zaxis.IsServon)
                return -3;
            if (!Condition()) return -4;
            Global.IsLocating = true;

            Task.Factory.StartNew(() =>
            {
                try
                {   //判断Z轴是否在零点
                    if (Zaxis != null)
                    {
                        if (!Zaxis.IsInPosition(0))
                            Zaxis.MoveTo(0, ZvelocityCurve ?? new VelocityCurve()
                            {
                                Strvel = 0,
                                Maxvel = Zaxis.Speed ?? 25,
                                Tacc = 0.1,
                                Tdec = 0.1,
                                VelocityCurveType = CurveTypes.T
                            });
                        while (true)
                        {
                            Thread.Sleep(10);
                            if (Zaxis.IsInPosition(0)) break;
                            if (ServoAxisIsReady(Zaxis))
                            {
                                Zaxis.Stop();
                                Global.IsLocating = false;
                                return -2;
                            }
                        }
                    }
                    //将X、Y移动到指定位置
                    if (!Xaxis.IsInPosition(X)) Xaxis.MoveTo(X, XvelocityCurve);
                    if (!IXaxis.IsInPosition(IX)) Xaxis.MoveTo(IX, IXvelocityCurve);
                    if (!Yaxis.IsInPosition(Y)) Yaxis.MoveTo(Y, YvelocityCurve);
                    while (true)
                    {
                        Thread.Sleep(10);
                        if (Xaxis.IsInPosition(X) && IXaxis.IsInPosition(IX) && Yaxis.IsInPosition(Y)) break;
                        if (ServoAxisIsReady(Xaxis) || ServoAxisIsReady(IXaxis) || ServoAxisIsReady(Yaxis))
                        {
                            Xaxis.Stop();
                            IXaxis.Stop();
                            Yaxis.Stop();
                            Global.IsLocating = false;
                            return -2;
                        }
                    }
                    //将Z轴移动到指定位置
                    if (Zaxis != null)
                    {
                        Zaxis.MoveTo(Z, ZvelocityCurve ?? new VelocityCurve()
                        {
                            Strvel = 0,
                            Maxvel = Zaxis.Speed ?? 25,
                            Tacc = 0.1,
                            Tdec = 0.1,
                            VelocityCurveType = CurveTypes.T
                        });
                        while (true)
                        {
                            Thread.Sleep(10);
                            if (Zaxis.IsInPosition(Z)) break;
                            if (ServoAxisIsReady(Zaxis))
                            {
                                Zaxis.Stop();
                                Global.IsLocating = false;
                                return -2;
                            }
                        }
                        Global.IsLocating = false;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    Global.IsLocating = false;
                    log.Fatal("设备驱动程序异常", ex);
                    return -2;
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
            Global.IsLocating = false;
            return 0;
        }

        #endregion

        private bool ServoAxisIsReady(ApsAxis axis) => !axis.IsServon | axis.IsAlarmed | axis.IsEmg | axis.IsMEL | axis.IsPEL;

        /// <summary>
        /// 计算镜筒清洗轨迹中心
        /// </summary>
        public void CalculateConeCenter()
        {
            try
            {
                //1次清洗轨迹圆心计算
                Position.Instance.CleanConeFirstPositionReal.X = Convert.ToSingle(Position.Instance.CleanConeFirstPosition.X);
                Position.Instance.CleanConeFirstPositionReal.Y = Convert.ToSingle(Position.Instance.CleanConeFirstPosition.Y);
                Position.Instance.CleanConeSecondPositionReal.X = Convert.ToSingle(Position.Instance.CleanConeSecondPosition.X);
                Position.Instance.CleanConeSecondPositionReal.Y = Convert.ToSingle(Position.Instance.CleanConeSecondPosition.Y);
                Position.Instance.CleanConeThirdPositonReal.X = Convert.ToSingle(Position.Instance.CleanConeThirdPositon.X);
                Position.Instance.CleanConeThirdPositonReal.Y = Convert.ToSingle(Position.Instance.CleanConeThirdPositon.Y);
                //计算镜筒清洗中心
                AreaCalculate(Position.Instance.CleanConeFirstPositionReal, Position.Instance.CleanConeSecondPositionReal,
                    Position.Instance.CleanConeThirdPositonReal, ref Position.Instance.CleanConeCenterPositionReal);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
        }

        /// <summary>
        /// 计算镜片清洗轨迹中心
        /// </summary>
        public void CalculateLensCenter()
        {
            try
            {
                //2次清洗轨迹圆心计算
                Position.Instance.CleanLensFirstPositionReal.X = Convert.ToSingle(Position.Instance.CleanLensFirstPosition.X);
                Position.Instance.CleanLensFirstPositionReal.Y = Convert.ToSingle(Position.Instance.CleanLensFirstPosition.Y);
                Position.Instance.CleanLensSecondPositionReal.X = Convert.ToSingle(Position.Instance.CleanLensSecondPosition.X);
                Position.Instance.CleanLensSecondPositionReal.Y = Convert.ToSingle(Position.Instance.CleanLensSecondPosition.Y);
                Position.Instance.CleanLensThirdPositonReal.X = Convert.ToSingle(Position.Instance.CleanLensThirdPositon.X);
                Position.Instance.CleanLensThirdPositonReal.Y = Convert.ToSingle(Position.Instance.CleanLensThirdPositon.Y);
                //计算镜片清洗中心
                AreaCalculate(Position.Instance.CleanLensFirstPositionReal, Position.Instance.CleanLensSecondPositionReal,
                    Position.Instance.CleanLensThirdPositonReal, ref Position.Instance.CleanLensCenterPositionReal);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
        }

        /// <summary>
        /// 计算点胶轨迹中心
        /// </summary>
        public void CalculateGlueCenter()
        {
            try
            {
                //点胶轨迹圆心计算
                Position.Instance.GlueFirstPositionReal.X = Convert.ToSingle(Position.Instance.GlueStartPosition.X);
                Position.Instance.GlueFirstPositionReal.Y = Convert.ToSingle(Position.Instance.GlueStartPosition.Y);
                Position.Instance.GlueSecondPositionReal.X = Convert.ToSingle(Position.Instance.GlueSecondPosition.X);
                Position.Instance.GlueSecondPositionReal.Y = Convert.ToSingle(Position.Instance.GlueSecondPosition.Y);
                Position.Instance.GlueThirdPositonReal.X = Convert.ToSingle(Position.Instance.GlueThirdPositon.X);
                Position.Instance.GlueThirdPositonReal.Y = Convert.ToSingle(Position.Instance.GlueThirdPositon.Y);

                //Position.Instance.GlueFirstPositionReal.X = 5;
                //Position.Instance.GlueFirstPositionReal.Y = 0;
                //Position.Instance.GlueSecondPositionReal.X = 0;
                //Position.Instance.GlueSecondPositionReal.Y =5;
                //Position.Instance.GlueThirdPositonReal.X =-5;
                //Position.Instance.GlueThirdPositonReal.Y = 0;
                //点胶中心
                AreaCalculate(Position.Instance.GlueFirstPositionReal, Position.Instance.GlueSecondPositionReal,
                    Position.Instance.GlueThirdPositonReal, ref Position.Instance.GlueCenterPositionReal);
                Position.Instance.GlueCenterPosition.X = Position.Instance.GlueCenterPositionReal.X;
                Position.Instance.GlueCenterPosition.Y = Position.Instance.GlueCenterPositionReal.Y;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
        }

        /// <summary>
        /// 通过三点坐标计算圆心坐标
        /// </summary>
        /// <param name="startPoint">第一点坐标</param>
        /// <param name="secondPoint">第二点坐标</param>
        /// <param name="endPoint">第三点坐标</param>
        /// <param name="centerPoint">圆心坐标</param>
        /// <returns>圆心坐标</returns>
        public static Point<float> AreaCalculate(Point<float> startPoint, Point<float> secondPoint, Point<float> endPoint, ref Point<float> centerPoint)
        {
            float tempA1, tempA2, tempB1, tempB2, tempC1, tempC2, temp, x, y;
            try
            {
                if (startPoint.X == secondPoint.X && startPoint.Y == secondPoint.Y)
                    throw new Exception("第一组数据与第二组数据相同！");
                if (startPoint.X == endPoint.X && startPoint.Y == endPoint.Y)
                    throw new Exception("第一组数据与第三组数据相同！");
                if (secondPoint.X == endPoint.X && secondPoint.Y == endPoint.Y)
                    throw new Exception("第二组数据与第三组数据相同！");
                // else throw new Exception("三个点坐标在同一直线上！");

                tempA1 = 2 * (secondPoint.X - startPoint.X);
                tempB1 = 2 * (secondPoint.Y - startPoint.Y);
                tempC1 = secondPoint.X * secondPoint.X + secondPoint.Y * secondPoint.Y - startPoint.X * startPoint.X - startPoint.Y * startPoint.Y;
                tempA2 = 2 * (endPoint.X - secondPoint.X);
                tempB2 = 2 * (endPoint.Y - secondPoint.Y);
                tempC2 = endPoint.X * endPoint.X - secondPoint.X * secondPoint.X + endPoint.Y * endPoint.Y - secondPoint.Y * secondPoint.Y;
                temp = tempA1 * tempB2 - tempA2 * tempB1;
                if (temp == 0)
                {
                    x = startPoint.X;
                    y = startPoint.Y;
                }
                else
                {
                    x = ((tempC1 * tempB2) - (tempC2 * tempB1)) / temp;
                    y = ((tempA1 * tempC2) - (tempA2 * tempC1)) / temp;
                }

                centerPoint.X = x;
                centerPoint.Y = y;

            }
            catch (Exception ex)
            {

                ;//throw new Exception(ex.ToString());
            }
            return centerPoint;

        }
    }
}
