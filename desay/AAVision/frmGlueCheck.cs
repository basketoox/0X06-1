using desay.AAVision.Algorithm;
using desay.ProductData;
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
        }

        private void frmGlueCheck_Load(object sender, EventArgs e)
        {
            tB_GlueInnerCircle.Text = Convert.ToString((double)GlueCheck.GlueInnerCircle);
            tB_GlueWidth.Text = Convert.ToString((double)GlueCheck.GlueWidth);
            threshold_min.Text = Convert.ToString((double)GlueCheck.threshold_min);
            threshold_max.Text = Convert.ToString((double)GlueCheck.threshold_max);
            openarea.Text = Convert.ToString((double)GlueCheck.Shapearea_min);
            closearea.Text = Convert.ToString((double)GlueCheck.Shapearea_max);
            fillarea.Text = Convert.ToString((double)GlueCheck.Fillarea);
            GlueOver_out.Text = Convert.ToString((double)GlueCheck.glueOverflowOutter);
            GlueOver_in.Text = Convert.ToString((double)GlueCheck.glueOverflowInner);            
            GlueLack_out.Text = Convert.ToString((double)GlueCheck.glueLackOutter);
            GlueLack_in.Text = Convert.ToString((double)GlueCheck.glueLackInner);
            GlueCenterOffset.Text = Convert.ToString((double)GlueCheck.glueOffset);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                //点击确定，将数据设置到函数里
                GlueCheck.GlueInnerCircle = double.Parse(tB_GlueInnerCircle.Text);
                GlueCheck.GlueWidth = double.Parse(tB_GlueWidth.Text);
                GlueCheck.threshold_min = Convert.ToDouble(threshold_min.Text);
                GlueCheck.threshold_max = Convert.ToDouble(threshold_max.Text);
                GlueCheck.Shapearea_min = Convert.ToDouble(openarea.Text);
                GlueCheck.Shapearea_max = Convert.ToDouble(closearea.Text);
                GlueCheck.Fillarea = Convert.ToDouble(fillarea.Text);
                GlueCheck.glueOverflowOutter = Convert.ToDouble(GlueOver_out.Text);
                GlueCheck.glueOverflowInner = Convert.ToDouble(GlueOver_in.Text);                
                GlueCheck.glueLackOutter = Convert.ToDouble(GlueLack_out.Text);
                GlueCheck.glueLackInner = Convert.ToDouble(GlueLack_in.Text);
                GlueCheck.glueOffset = Convert.ToDouble(GlueCenterOffset.Text);
                frmAAVision.WriteParamToFile();
                MessageBox.Show("设置完成！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            tB_GlueInnerCircle.Text = Convert.ToString((double)GlueCheck.GlueInnerCircle);
            tB_GlueWidth.Text = Convert.ToString((double)GlueCheck.GlueWidth);
            threshold_min.Text = Convert.ToString((double)GlueCheck.threshold_min);
            threshold_max.Text = Convert.ToString((double)GlueCheck.threshold_max);
            openarea.Text = Convert.ToString((double)GlueCheck.Shapearea_min);
            closearea.Text = Convert.ToString((double)GlueCheck.Shapearea_max);
            fillarea.Text = Convert.ToString((double)GlueCheck.Fillarea);
            GlueOver_out.Text = Convert.ToString((double)GlueCheck.glueOverflowOutter);
            GlueOver_in.Text = Convert.ToString((double)GlueCheck.glueOverflowInner);
            GlueLack_out.Text = Convert.ToString((double)GlueCheck.glueLackOutter);
            GlueLack_in.Text = Convert.ToString((double)GlueCheck.glueLackInner);
            GlueCenterOffset.Text = Convert.ToString((double)GlueCheck.glueOffset);
        }

        private void btnCreateRing_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在右侧图像里框取模板区域，调整完后按鼠标右键以确定！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string fileName = AppConfig.VisonPath + "\\GlueCheckRegion.shm";
            CreateModle Createtor = new CreateModle(fileName);
            Createtor.DrawCircleRing(winContr_GCP.HalconWindow, image);
        }

        private void btnCreateCircle_Click(object sender, EventArgs e)
        {
            MessageBox.Show("在右侧图像里框取模板区域，调整完后按鼠标右键以确定！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string fileName = AppConfig.VisonPath + "\\GlueCheckRegion.shm";
            CreateModle Createtor = new CreateModle(fileName);
            Createtor.DrawCircle(winContr_GCP.HalconWindow, image);
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
    }
}
