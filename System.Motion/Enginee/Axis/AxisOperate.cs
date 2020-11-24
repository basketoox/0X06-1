using System;
using System.Windows.Forms;
using Motion.AdlinkAps;
using Motion.Interfaces;
using System.Threading;
using System.Threading.Tasks;
namespace Motion.Enginee
{
    public partial class AxisOperate : UserControl,IRefreshing
    {
        private ApsAxis m_Axis;
        public AxisOperate()
        {
            InitializeComponent();
        }
        public AxisOperate(ApsAxis axis):this()
        {
            m_Axis = axis;
            gbxName.Text = axis.Name;
        }
        /// <summary>
        /// 移动方向选择：连续？定距？
        /// </summary>
        public AxisMoveMode MoveMode { private get; set; }
        private void btnJogDec_Click(object sender, EventArgs e)
        {
            if (MoveMode.Continue) return;
            if (!m_Axis.IsDone) return;
            var Value =MoveMode.Distance;
            Value *= -1;
            var velocityCurve = new VelocityCurve { Maxvel = m_Axis.Speed ?? 0 };
            m_Axis.MoveDelta(Value, velocityCurve);
        }

        private void btnJogDec_MouseDown(object sender, MouseEventArgs e)
        {
            if (!MoveMode.Continue) return;
            m_Axis.Negative();
        }

        private void btnJogDec_MouseUp(object sender, MouseEventArgs e)
        {
            if (!MoveMode.Continue) return;
            m_Axis.Stop();
        }

        private void btnJogAdd_Click(object sender, EventArgs e)
        {
            if (MoveMode.Continue) return;
            if (!m_Axis.IsDone) return;
            var Value = MoveMode.Distance;
            Value *= 1;
            var velocityCurve = new VelocityCurve { Maxvel = m_Axis.Speed ?? 0 };
            m_Axis.MoveDelta(Value, velocityCurve);
        }

        private void btnJogAdd_MouseDown(object sender, MouseEventArgs e)
        {
            if (!MoveMode.Continue) return;
            m_Axis.Postive();
        }

        private void btnJogAdd_MouseUp(object sender, MouseEventArgs e)
        {
            if (!MoveMode.Continue) return;
            m_Axis.Stop();
        }

        private void tbrJogSpeed_Scroll(object sender, EventArgs e)
        {
            lblJogSpeed.Text =tbrJogSpeed.Value.ToString("0.00");
            m_Axis.Speed = tbrJogSpeed.Value;
        }

        private void AxisOperate_Load(object sender, EventArgs e)
        {
            lblJogSpeed.Text = tbrJogSpeed.Value.ToString("0.00");
            m_Axis.Speed = tbrJogSpeed.Value;
        }

        public void Refreshing()
        {
            picSON.Image = m_Axis.IsServon ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picORG.Image=m_Axis.IsOrign? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picSZ.Image= m_Axis.IsSZ ? Properties.Resources.LedGreen : Properties.Resources.LedNone;
            picMEL.Image= m_Axis.IsMEL ? Properties.Resources.LedRed : Properties.Resources.LedNone;
            picPEL.Image= m_Axis.IsPEL ? Properties.Resources.LedRed : Properties.Resources.LedNone;
            picALM.Image= m_Axis.IsAlarmed ? Properties.Resources.LedRed : Properties.Resources.LedNone;
            lblCurrentPosition.Text = m_Axis.CurrentPos.ToString("0.000");
            lblCurrentSpeed.Text = m_Axis.CurrentSpeed.ToString("0.000");
            if(m_Axis.IsServon!=chxServoON.Checked&&!sign)
            {
                chxServoON.Checked= m_Axis.IsServon;
            }
        }
        bool sign = false;
        private void chxServoON_CheckedChanged(object sender, EventArgs e)
        {
            sign = true;
            m_Axis.IsServon = chxServoON.Checked;
            sign = false;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            var Flow = 0;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    while (true)
                    {
                        switch (Flow)
                        {
                            case 0:   //清除所有标志位的状态
                                m_Axis.Stop();
                                if (!m_Axis.IsAlarmed)
                                {
                                    m_Axis.IsServon = true;
                                    Flow = 10;
                                }
                                break;
                            case 10:
                                m_Axis.BackHome();
                                Flow = 20;
                                break;
                            case 20://判断轴回原点是否完成，轴移动安全位置
                                if (m_Axis.CheckHomeDone(20.0) == 0)
                                {
                                    Thread.Sleep(1000);
                                    APS168.APS_set_command(m_Axis.NoId, 0);
                                    APS168.APS_set_position(m_Axis.NoId, 0);
                                    Flow = 30;
                                }
                                else  //异常处理
                                    Flow = -1;
                                break;
                            default:
                                return;
                        }
                        if (!m_Axis.IsServon) return;
                    }
                    //Log.Debug("{0}部件启动。", Name);
                }
                catch (OperationCanceledException)
                {
                    //ignorl
                }
                catch (Exception ex)
                {
                    //Log.Fatal("设备驱动程序异常", ex);
                }
            }, TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
        }
    }
}
