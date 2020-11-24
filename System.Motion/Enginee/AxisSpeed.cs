using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Motion.AdlinkAps;
namespace Motion.Enginee
{
    public partial class AxisSpeed : UserControl
    {
        private double m_velocityMax;

        public AxisSpeed()
        {
            InitializeComponent();
        }
        public AxisSpeed(double velocityMax) : this()
        {
            m_velocityMax = velocityMax;
        }
        public string Name { get; set; }
        public double SpeedRate
        {
            get
            {
                return tkrSpeedRate.Value / 10.0;
            }
            set
            {
                tkrSpeedRate.Value = (int)(value * 10.0);
                lblAxisSpeedRate.Text = Name + "速度(" + value.ToString("0.00") + "%)";
                lblAxisSpeed.Text = ((m_velocityMax * value) / 100).ToString("0.000") + "mm/s";
            }
        }
        private void tkrSpeedRate_Scroll(object sender, EventArgs e)
        {
            lblAxisSpeedRate.Text = Name + "速度(" + SpeedRate + "%)";
            lblAxisSpeed.Text = ((m_velocityMax * SpeedRate) / 100).ToString("0.000") + "mm/s";
        }
    }
}
