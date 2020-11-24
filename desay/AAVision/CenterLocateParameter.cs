using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using desay.AAVision.Algorithm;
using desay.ProductData;
namespace desay
{
    public partial class CenterLocateParameter : Form
    {

        public HObject image;
        Bitmap bmp;
        HTuple width, height;
        public CenterLocateParameter(Bitmap bmpin)
        {
            InitializeComponent();
            bmp = bmpin;
            Bitmap2HObject.Bitmap2HObj(bmpin, out image);
            HOperatorSet.GetImageSize(image, out width, out height);
            HOperatorSet.SetPart(WinCtro_CLP.HalconWindow, 0, 0, height - 1, width - 1);
            HOperatorSet.DispObj(image, WinCtro_CLP.HalconWindow);
            tB_x.Text = Convert.ToString(CenterLocate.circleCenter_x);
            tB_y.Text = Convert.ToString(CenterLocate.circleCenter_y);
            tB_r.Text = Convert.ToString(CenterLocate.circleRadius);
            textBox1.Text = Convert.ToString(CenterLocate.threshold_min);
            textBox2.Text = Convert.ToString(CenterLocate.threshold_max);
            textBox3.Text = this.Text = Convert.ToString(CenterLocate.eroKernel);
            textBox4.Text = Convert.ToString(CenterLocate.areaMax);
            textBox5.Text = Convert.ToString(CenterLocate.areaMin);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在左侧图像里框取ROI区域，调整完后按鼠标右键以确定！", "提示信息",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            DrawROI DR = new DrawROI();
            DR.DrawCricle(WinCtro_CLP.HalconWindow);
            tB_x.Text = Convert.ToString(System.Math.Round(DR.row[0].D, 4));
            tB_y.Text = Convert.ToString(System.Math.Round(DR.column[0].D, 4));
            tB_r.Text = Convert.ToString(System.Math.Round(DR.radius[0].D, 4));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //点击确定，将数据设置到函数里
                CenterLocate.circleCenter_x = double.Parse(tB_x.Text);
                CenterLocate.circleCenter_y = double.Parse(tB_y.Text);
                CenterLocate.circleRadius = double.Parse(tB_r.Text);
                frmAAVision.WriteParamToFile();
                MessageBox.Show("修改完成！", "提示信息",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(trackBar1.Value);
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = Convert.ToString(trackBar2.Value);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            trackBar1.Value = Convert.ToInt32(textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            trackBar2.Value = Convert.ToInt32(textBox2.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //用户点击确认，将灰度值写到函数参数里
                //判断数值是否合法，min应小于max
                if (Convert.ToInt32(textBox1.Text) > Convert.ToInt32(textBox2.Text))
                {
                    MessageBox.Show("阈值不合法，最小阈值应小于最大阈值，请重新输入！", "提示信息",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    CenterLocate.threshold_min = Convert.ToInt32(textBox1.Text);
                    CenterLocate.threshold_max = Convert.ToInt32(textBox2.Text);
                    frmAAVision.WriteParamToFile();
                    MessageBox.Show("修改完成！", "提示信息",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //点击取消，将textbox里参数显示为函数里的数值
            tB_x.Text = Convert.ToString(CenterLocate.circleCenter_x);
            tB_y.Text = Convert.ToString(CenterLocate.circleCenter_y);
            tB_r.Text = Convert.ToString(CenterLocate.circleRadius);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text != null && textBox4 != null && textBox5 != null)
                {
                    CenterLocate.eroKernel = Convert.ToDouble(textBox3.Text);
                    CenterLocate.areaMin = Convert.ToDouble(textBox5.Text);
                    CenterLocate.areaMax = Convert.ToDouble(textBox4.Text);
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

            textBox3.Text = Convert.ToString(CenterLocate.eroKernel);
            textBox4.Text = Convert.ToString(CenterLocate.areaMax);
            textBox5.Text = Convert.ToString(CenterLocate.areaMin);
        }

        private void trackBar1_ValueChanged_1(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(trackBar1.Value);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在左侧图像里框取ROI区域，调整完后按鼠标右键以确定！", "提示信息",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            string fileName = AppConfig.VisonPath + "\\CenterLocROI.hobj";
            DrawROI Pen = new DrawROI();
            Pen.DrawCircleRing(WinCtro_CLP.HalconWindow, fileName);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在左侧图像里框取ROI区域，调整完后按鼠标右键以确定！", "提示信息",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            string fileName = AppConfig.VisonPath + "\\CenterLocROI.hobj";
            DrawROI Pen = new DrawROI();
            Pen.DrawRectangleROI(WinCtro_CLP.HalconWindow, fileName);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在左侧图像里框取模板区域，调整完后按鼠标右键以确定！", "提示信息",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            string fileName = AppConfig.VisonPath + "\\CenterModle.shm";
            CreateModle Createtor = new CreateModle(fileName);
            Createtor.DrawCircle(WinCtro_CLP.HalconWindow, image);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在左侧图像里框取模板区域，调整完后按鼠标右键以确定！", "提示信息",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            string fileName = AppConfig.VisonPath + "\\CenterModle.shm";
            CreateModle Createtor = new CreateModle(fileName);
            Createtor.DrawEllipse(WinCtro_CLP.HalconWindow, image);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在左侧图像里框取模板区域，调整完后按鼠标右键以确定！", "提示信息",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            string fileName = AppConfig.VisonPath + "\\CenterModle.shm";
            CreateModle Createtor = new CreateModle(fileName);
            Createtor.DrawRectangle(WinCtro_CLP.HalconWindow, image);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在左侧图像里框取模板区域，调整完后按鼠标右键以确定！", "提示信息",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            string fileName = AppConfig.VisonPath + "\\CenterModle.shm";
            CreateModle Createtor = new CreateModle(fileName);
            Createtor.DrawRegion(WinCtro_CLP.HalconWindow, image);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在左侧图像里框取ROI区域，调整完后按鼠标右键以确定！", "提示信息",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            string fileName = AppConfig.VisonPath + "\\CenterLocROI.hobj";
            DrawROI Pen = new DrawROI();
            Pen.DrawCircleRingROI(WinCtro_CLP.HalconWindow, fileName);
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("在左侧图像里框取模板区域，调整完后按鼠标右键以确定！", "提示信息",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            string fileName = AppConfig.VisonPath + "\\CenterModle.shm";
            CreateModle Createtor = new CreateModle(fileName);
            Createtor.DrawCircleRing(WinCtro_CLP.HalconWindow, image);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //点击取消，将textbox里参数显示为函数里的数值
            textBox1.Text = Convert.ToString(CenterLocate.threshold_min);
            textBox2.Text = Convert.ToString(CenterLocate.threshold_max);
  
        }
    }
}
