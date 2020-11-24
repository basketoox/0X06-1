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
        public bool bShowImagePoint = false;
    
   
        private string str_imgSize;
        public frmGlueCheck(Bitmap bmpin)
        {
            InitializeComponent();
            Bitmap2HObject.Bitmap2HObj(bmpin, out image);
            HOperatorSet.GetImageSize(image, out width, out height);
            
            str_imgSize = string.Format("{0}X{1}", width, height);
            HOperatorSet.SetPart(winContr_GCP.HalconWindow, 0, 0, height - 1, width - 1);
            HOperatorSet.DispObj(image, winContr_GCP.HalconWindow);
           
            tB_GlueInnerCircle.Text = Convert.ToString((double)GlueCheck.GlueInnerCircle);
            tB_GlueWidth.Text = Convert.ToString((double)GlueCheck.GlueWidth);
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
            

                GlueCheck.GlueInnerCircle = double.Parse(tB_GlueInnerCircle.Text);
                GlueCheck.GlueWidth = double.Parse(tB_GlueWidth.Text);
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
            tB_GlueInnerCircle.Text = Convert.ToString((double)GlueCheck.GlueInnerCircle);
            tB_GlueWidth.Text = Convert.ToString((double)GlueCheck.GlueWidth);
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

        private void winContr_GCP_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (image == null /*|| *//*bShowImagePoint == false*/ /*|| bVideo == true*/)
                return;
            try
            {
                HTuple htuple;
                HOperatorSet.CountChannels(image, out htuple);
                double row;
                double column;
                int button;

                winContr_GCP.HalconWindow.GetMpositionSubPix(out row, out column, out button);
                string ss1 = string.Format("Y: {0:0000.0},X: {1:0000.0}", row, column);
                if (column >= 0.0 && column < width && (row >= 0.0 && row < height))
                {
                    string ss2;
                    if ((htuple) == 1)
                    //ss2 = string.Format("Val: {0:000.0}", himage.GetGrayval((int)row, (int)column));
                    {
                        HTuple value = 0;
                        HOperatorSet.GetGrayval(image, (int)row, (int)column, out value);
                        ss2 = string.Format("灰度值: {0:000.0}", value);
                    }
                    else if ((htuple) == 3)
                    {

                        //HImage R = himage.AccessChannel(1);
                        //HImage G = himage.AccessChannel(2);
                        //HImage B = himage.AccessChannel(3);
                        HObject R, G, B;
                        HOperatorSet.GenEmptyObj(out R);
                        HOperatorSet.GenEmptyObj(out G);
                        HOperatorSet.GenEmptyObj(out B);
                        HOperatorSet.AccessChannel(image, out R, 1);
                        HOperatorSet.AccessChannel(image, out G, 2);
                        HOperatorSet.AccessChannel(image, out B, 3);
                        HTuple grayval1;
                        HTuple grayval2;
                        HTuple grayval3;

                        HOperatorSet.GetGrayval(R, (int)row, (int)column, out grayval1);
                        HOperatorSet.GetGrayval(G, (int)row, (int)column, out grayval2);
                        HOperatorSet.GetGrayval(B, (int)row, (int)column, out grayval3);
                        //double grayval1 = R.GetGrayval((int)row, (int)column);
                        //double grayval2 = G.GetGrayval((int)row, (int)column);
                        //double grayval3 = B.GetGrayval((int)row, (int)column);
                        (R).Dispose();
                        (G).Dispose();
                        (B).Dispose();
                        ss2 = string.Format("灰度值: ({0:000.0}, {1:000.0}, {2:000.0})", grayval1, grayval2, grayval3);
                    }
                    else
                        ss2 = "";
                    m_CtrlHStatusLabelCtrl.Text = str_imgSize + "    " + ss1 + "    " + ss2;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void frmGlueCheck_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox4.Text = (String)(GlueCheck.glueOverflowOutter[0]);
            textBox5.Text = (String)(GlueCheck.glueOverflowInner);
            textBox6.Text = (String)(GlueCheck.glueOffset);
            textBox7.Text = (String)(GlueCheck.glueLackOutter);
            textBox8.Text = (String)(GlueCheck.glueLackInner);
        }

    }
}
