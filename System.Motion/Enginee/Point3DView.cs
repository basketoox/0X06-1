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
using System.Toolkit;
using Motion.Interfaces;
namespace Motion.Enginee
{
    public partial class Point3DView : UserControl
    {
        public Point3DView()
        {
            InitializeComponent();
        }
        public Point3D<double> Point
        {
            set
            {
                lblGetProductX.Text = value.X.ToString("0.000");
                lblGetProductY.Text = value.Y.ToString("0.000");
                lblGetProductZ.Text = value.Z.ToString("0.000");
            }
        }
    }
}
