using System.Windows.Forms;
using System.Toolkit;
using Motion.AdlinkAps;
using Motion.Interfaces;
namespace Motion.Enginee
{
    public partial class PlatePointView : UserControl
    {
        public PlatePointView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 索引编号
        /// </summary>
        public int Index
        {
            get
            {
                return (int)ndnIndex.Value;
            }
            set
            {
                ndnIndex.Value = value;
            }
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
