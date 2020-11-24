using desay.AAVision.Algorithm;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace desay.AAVision
{
    public partial class frmNeedleLocParameter : Form
    {
        public HObject image;
        Bitmap bmp;
        HTuple width, height;
        public frmNeedleLocParameter(Bitmap bmpin)
        {
            InitializeComponent();
            bmp = bmpin;
            Algorithm.Bitmap2HObject.Bitmap2HObj(bmpin, out image);
            HOperatorSet.GetImageSize(image, out width, out height);
            HOperatorSet.SetPart(WinCtro_NLP.HalconWindow, 0, 0, height - 1, width - 1);
            HOperatorSet.DispObj(image, WinCtro_NLP.HalconWindow);
            tB_x.Text = Convert.ToString(NeedleLocate.circleCenter_x);
            tB_y.Text = Convert.ToString(NeedleLocate.circleCenter_y);
            tB_r.Text = Convert.ToString(NeedleLocate.circleRadius);
            textBox1.Text = Convert.ToString(NeedleLocate.areaSizeMin);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //点击确定，将数据设置到函数里
                NeedleLocate.circleCenter_x = double.Parse(tB_x.Text);
                NeedleLocate.circleCenter_y = double.Parse(tB_y.Text);
                NeedleLocate.circleRadius = double.Parse(tB_r.Text);
                frmAAVision.WriteParamToFile();
                MessageBox.Show("修改完成！", "提示信息",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //点击取消，将textbox里参数显示为函数里的数值
            tB_x.Text = Convert.ToString(NeedleLocate.circleCenter_x);
            tB_y.Text = Convert.ToString(NeedleLocate.circleCenter_y);
            tB_r.Text = Convert.ToString(NeedleLocate.circleRadius);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(trackBar1.Value);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                NeedleLocate.areaSizeMin = Convert.ToInt32(textBox1.Text);
                frmAAVision.WriteParamToFile();
                MessageBox.Show("修改完成！", "提示信息",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //点击取消，将textbox里参数显示为函数里的数值
            textBox1.Text = Convert.ToString(NeedleLocate.areaSizeMin);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在左侧图像里框取ROI区域，调整完后按鼠标右键以确定！", "提示信息",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DrawROI DR = new DrawROI();
            DR.DrawCricle(WinCtro_NLP.HalconWindow);
            tB_x.Text = Convert.ToString(System.Math.Round(DR.row[0].D, 4));
            tB_y.Text = Convert.ToString(System.Math.Round(DR.column[0].D, 4));
            tB_r.Text = Convert.ToString(System.Math.Round(DR.radius[0].D, 4));
        }
    }
}
