using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Toolkit;
using Motion.AdlinkAps;
using Motion.Interfaces;
namespace Motion.Enginee
{
    public partial class PointView : UserControl
    {
        public PointView()
        {
            InitializeComponent();
        }
        public Point<double> Point
        {
            set
            {
                lblGetProductX.Text = value.X.ToString("0.000");
                lblGetProductY.Text = value.Y.ToString("0.000");
            }
        }
    }
}
