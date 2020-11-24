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
    public partial class frmGlueCheck : Form

    {
        public HObject image;
        HTuple width, height;
        public frmGlueCheck(Bitmap bmpin)
        {
            InitializeComponent();
            Bitmap2HObject.Bitmap2HObj(bmpin, out image);
            HOperatorSet.GetImageSize(image, out width, out height);
            HOperatorSet.SetPart(winContr_GCP.HalconWindow, 0, 0, height - 1, width - 1);
            HOperatorSet.DispObj(image, winContr_GCP.HalconWindow);
            tB_x.Text = Convert.ToString(GlueCheck.circleCenter_x);
            tB_y.Text = Convert.ToString(GlueCheck.circleCenter_y);
            tB_r.Text = Convert.ToString(GlueCheck.circleRadius);
            textBox1.Text = Convert.ToString((double)GlueCheck.tol[0]);
            textBox2.Text = Convert.ToString((double)GlueCheck.area);
            textBox3.Text = Convert.ToString((double)GlueCheck.kernel);
            textBox4.Text = Convert.ToString((double)GlueCheck.glueOverflowOutter);
            textBox5.Text = Convert.ToString((double)GlueCheck.glueOverflowInner);
            textBox6.Text = Convert.ToString((double)GlueCheck.glueOffset);
            textBox7.Text = Convert.ToString((double)GlueCheck.glueLackOutter);
            textBox8.Text = Convert.ToString((double)GlueCheck.glueLackInner);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //点击确定，将数据设置到函数里
                GlueCheck.circleCenter_x = double.Parse(tB_x.Text);
                GlueCheck.circleCenter_y = double.Parse(tB_y.Text);
                GlueCheck.circleRadius = double.Parse(tB_r.Text);
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
            tB_x.Text = Convert.ToString(GlueCheck.circleCenter_x);
            tB_y.Text = Convert.ToString(GlueCheck.circleCenter_y);
            tB_r.Text = Convert.ToString(GlueCheck.circleRadius);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(trackBar1.Value);
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = Convert.ToString(trackBar2.Value);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                GlueCheck.tol = Convert.ToInt32(textBox1.Text);
                GlueCheck.area = Convert.ToInt32(textBox2.Text);
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
            textBox1.Text = (String)(GlueCheck.tol);
            textBox2.Text = (String)(GlueCheck.area);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text != null)
                {
                    GlueCheck.kernel = Convert.ToDouble(textBox3.Text);
                    frmAAVision.WriteParamToFile();
                    MessageBox.Show("设置完成！", "提示信息",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("没有输入！", "提示信息",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
        

        private void button7_Click(object sender, EventArgs e)
        {
            textBox3.Text = Convert.ToString((double)GlueCheck.kernel);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                //点击确定，将数据设置到函数里
                GlueCheck.glueOverflowOutter = Convert.ToDouble(textBox4.Text);
                GlueCheck.glueOverflowInner = Convert.ToDouble(textBox5.Text);
                GlueCheck.glueOffset = Convert.ToDouble(textBox6.Text);
                GlueCheck.glueLackOutter = Convert.ToDouble(textBox7.Text);
                GlueCheck.glueLackInner = Convert.ToDouble(textBox8.Text);
                frmAAVision.WriteParamToFile();
                MessageBox.Show("设置完成！", "提示信息",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox4.Text = (String)(GlueCheck.glueOverflowOutter[0]);
            textBox5.Text = (String)(GlueCheck.glueOverflowInner);
            textBox6.Text = (String)(GlueCheck.glueOffset);
            textBox7.Text = (String)(GlueCheck.glueLackOutter);
            textBox8.Text = (String)(GlueCheck.glueLackInner);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在左侧图像里框取ROI区域，调整完后按鼠标右键以确定！", "提示信息",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DrawROI DR = new DrawROI();
            DR.DrawCricle(winContr_GCP.HalconWindow);
            tB_x.Text = Convert.ToString(System.Math.Round(DR.row[0].D, 4));
            tB_y.Text = Convert.ToString(System.Math.Round(DR.column[0].D, 4));
            tB_r.Text = Convert.ToString(System.Math.Round(DR.radius[0].D, 4));
        }
    }
}
