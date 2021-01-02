using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using desay.ProductData;
using log4net;
using HalconDotNet;
using NationalInstruments.Vision;
using Vision_Assistant;
using System.IO;

namespace desay
{

    public partial class AcqToolEdit : UserControl
    {
        static ILog log = LogManager.GetLogger(typeof(AcqToolEdit));
        public static double offset_x;
        public static double offset_y;
        public static double[] offsetOri;
        private TabPage[] tabpage;
        Bitmap bmp;
        VisionImage VI;
        VisionImage LastVI;

        private BaumerCamera _Subject;
        public BaumerCamera Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }

        private string _CamSN;
       

        public AcqToolEdit()
        {
            InitializeComponent();
            tabpage = new TabPage[] { this.tab_set};
            offsetOri = new double[] { 0, 0 };
         
        }

        public void AcqToolEdit_Load(object sender, EventArgs e)
        {
            
            UpdataControl(false);
            
            if (_Subject != null)
            {
                this.CB_CamsList.Enabled = !_Subject.IsOpen;
                _Subject.Ran += _Subject_Ran;
                
                var camlist  = BaumerCameraSystem.listCamera.Select((c) => c.pDevice.SerialNumber).ToList();
                CB_CamsList.DataSource = camlist;
                this.CB_CamsList.SelectedIndex = BaumerCameraSystem.listCamera.FindIndex((c) => c.pDevice.SerialNumber == _Subject.SerialNumber);
            }

            if (_Subject != null)
            {
                _Subject.Initialize(_CamSN);

                _Subject.TriggerSource = TriggerSource.Software;
            }
        }
        /// <summary>
        /// 代理异步调用以设置picturebox控件的image属性
        /// </summary>
        delegate void SetPictureCallBack(PictureBox pb, Image img);

        private void SetPictureBoxImg(PictureBox pb, Image img)
        {
            try
            {
                if (pb.InvokeRequired)
                {
                    SetPictureCallBack d = new SetPictureCallBack(SetPictureBoxImg);
                    this.BeginInvoke(d, new object[] { pb, img });
                }
                else
                {
                    pb.Image = img;
                    pb.Refresh();
                }
            }
            catch (Exception ex)
            {
                //Log.WriteLog(logType.ltError, "SetPictureBoxImg error:" + ex.ToString());
            }
        }

        public void bmpToVisionImage(Bitmap bmp)
        {
            if (!Marking.DryRun)
            {
                bmp.Save($"{ @"./ImageTemp/temp.jpg"}");
                VI = new VisionImage(ImageType.Rgb32);
                VI.ReadFile($"{ @"./ImageTemp/temp.jpg"}");
            }
            else
            {
                VI = new VisionImage(ImageType.Rgb32);
                string p = Config.Instance.CurrentProductType;
                VI.ReadFile(AppConfig.DryRunPic);
            }
            
        }


        void _Subject_Ran(ImageData imageData)
        {
            try
            {
                bmp = ImageFactory.CreateBitmap(_Subject.OutputImageData);
            }
            catch
            {
                bmp = null;
            }

            try
            {
                if (Marking.NeedleLocateTest)
                {
                    bmpToVisionImage(bmp);
                    try
                    {
                        Marking.NeedleLocateTest = false;
                        //NeedleLocate.TestBmp(bmp, frmAAVision.acq.hWindowControl1.HalconWindow, Position.Instance.GlueAdjustPinPosition, Position.Instance.GlueCameraCalibPosition, frmAAVision.acq.SaveImage);

                        FindNeedleLoc.ProcessImage(VI);
                        if (FindNeedleLoc.vaCircularEdgeReport.CircleFound)
                        {
                            double[] data = new double[2];
                            double[] offset = new double[2];
                            data[0] = FindNeedleLoc.vaCircularEdgeReport.Center.X;
                            data[1] = FindNeedleLoc.vaCircularEdgeReport.Center.Y;
                            offset[0] = (data[0] - VI.Width / 2) * Config.Instance.CameraPixelMM_X;
                            offset[1] = (data[1] - VI.Height / 2) * Config.Instance.CameraPixelMM_Y;
                            Position.Instance.CCD2NeedleOffset.X = Position.Instance.GlueCameraCalibPosition.X - Position.Instance.GlueAdjustPinPosition.X - offset[0];
                            Position.Instance.CCD2NeedleOffset.Y = Position.Instance.GlueCameraCalibPosition.Y - Position.Instance.GlueAdjustPinPosition.Y + offset[1];
                            NeedleLocate.FindNeedleLoc(bmp, frmAAVision.acq.hWindowControl1.HalconWindow, data, offset);
                            Marking.NeedleLocateTestSucceed = true;
                        }
                        else
                        {
                            Marking.NeedleLocateTestSucceed = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug("图像识别异常111！" + ex.Message + ex.StackTrace);

                    }
                    VI.Dispose();
                }
                else if (Marking.CenterLocateTest)
                {
                    bmpToVisionImage(bmp);
                    try
                    {
                        Marking.CenterLocateTest = false;
                        if (Position.Instance.UseRectGlue)
                        {
                            try
                            {
                                Image_Processing.ProcessImage(VI);
                                double ICCenter_X = 0;
                                double ICCenter_Y = 0;
                                if(Image_Processing.gpm2Results.Count == 2)
                                {
                                   if(Image_Processing.gpm2Results[0].Position.Y < Image_Processing.gpm2Results[1].Position.Y)
                                    {
                                        Image_Processing.gpm2Results.RemoveAt(0);
                                    }
                                    else
                                    {
                                        Image_Processing.gpm2Results.RemoveAt(1);
                                    }
                                }
                                if (Image_Processing.gpm2Results.Count == 1)
                                {
                                    offsetOri[0] = Image_Processing.gpm2Results[0].CalibratedPosition.X - 1759.58;
                                    offsetOri[1] = Image_Processing.gpm2Results[0].CalibratedPosition.Y - 1411.70;

                                    ICCenter_X = Image_Processing.gpm2Results[0].CalibratedPosition.X-457;//457
                                    ICCenter_Y = Image_Processing.gpm2Results[0].CalibratedPosition.Y-414;//414
                                    offset_x = ICCenter_X - VI.Width / 2;
                                    offset_y = ICCenter_Y - VI.Height / 2;
                                    double[] offset_x_pix = { -630, 580, 655, 655, -630, -630 };
                                    double[] offset_y_pix = { -650, -650, -580, 640, 640, -650 };
                                    for (int i = 0; i < offset_x_pix.Length; i++)
                                    {
                                        Config.Instance.RectX[i] = Position.Instance.GlueCameraPosition.X - Position.Instance.CCD2NeedleOffset.X + Position.Instance.GlueOffsetX - (offset_x_pix[i] - offset_x) / 96;
                                        Config.Instance.RectY[i] = Position.Instance.GlueCameraPosition.Y - Position.Instance.CCD2NeedleOffset.Y + Position.Instance.GlueOffsetY + (offset_y_pix[i] - offset_y) / 96;
                                    }
                                    //string str = Config.Instance.RectX[0].ToString("f3") + "," + Config.Instance.RectY[0].ToString("f3")+"\r\n";
                                    //System.IO.File.AppendAllText(@"C:\Users\Administrator\Desktop\123.txt", str);
                                    Marking.CenterLocateTestSucceed = true;
                                }
                                else
                                {
                                    Marking.CenterLocateTestSucceed = false;
                                }
                                CenterLocate.RectangleMatch(bmp, frmAAVision.acq.hWindowControl1.HalconWindow, Image_Processing.gpm2Results.Count == 1, ICCenter_X, ICCenter_Y);
                            }
                            catch
                            {
                                Marking.CenterLocateTestSucceed = false;
                            }
                        }
                        else
                        {
                            //CenterLocate.TestBmp(bmp, frmAAVision.acq.hWindowControl1.HalconWindow, frmAAVision.acq.SaveImage);

                            FindCircularCenter.ProcessImage(VI);
                            if (FindCircularCenter.vaCircularEdgeReport.CircleFound)
                            {
                                double[] data = new double[2];
                                data[0] = FindCircularCenter.vaCircularEdgeReport.Center.X;
                                data[1] = FindCircularCenter.vaCircularEdgeReport.Center.Y;
                                Position.Instance.PCB2CCDOffset.X = (data[0] - VI.Width / 2) * Config.Instance.CameraPixelMM_X;
                                Position.Instance.PCB2CCDOffset.Y = (data[1] - VI.Height / 2) * Config.Instance.CameraPixelMM_Y;
                                Marking.CenterLocateTestSucceed = true;
                            }
                            else
                            {
                                Marking.CenterLocateTestSucceed = false;
                            }
                            if (frmAAVision.acq.SaveImage)
                            {
                                SaveImage.Save(frmAAVision.acq.hWindowControl1.HalconWindow);
                            }
                            CenterLocate.CircularMatch(bmp, frmAAVision.acq.hWindowControl1.HalconWindow, FindCircularCenter.vaCircularEdgeReport.CircleFound);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        log.Debug("图像识别异常222！" + ex.Message + ex.StackTrace);
                    }
                    VI.Dispose();
                }
                else if (Marking.GlueCheckTest)
                {
                    if (!Marking.DryRun)
                    {
                        try
                        {
                            Marking.GlueCheckTest = false;
                            if (Position.Instance.UseRectGlue)
                            {
                                bmp.Save($"{ @"./ImageTemp/temp.jpg"}");
                                VI = new VisionImage(ImageType.Rgb32);
                                VI.ReadFile($"{ @"./ImageTemp/temp.jpg"}");
                                RectGlueCheck.ProcessImage(VI, offsetOri);
                                if (Form2.JudegCenterPosition() && Form2.JudgeRectangleSize() && Form2.JudegMaxMassArea())
                                {
                                    Marking.GlueResult = true;
                                }
                                else
                                {
                                    Marking.GlueResult = false;
                                }
                                GlueCheck.GlueCheck_R(bmp, frmAAVision.acq.hWindowControl1.HalconWindow, Marking.GlueResult);
                                VI.Dispose();
                            }
                            else
                            {
                                //GlueCheck.TestBmp(CenterLocate.LastCenterLocateBMP, bmp, frmAAVision.acq.hWindowControl1.HalconWindow, frmAAVision.acq.SaveImage);
                                double[] distance;
                                LastVI = new VisionImage(ImageType.Rgb32);
                                LastVI.ReadFile($"{ @"./ImageTemp/temp.jpg"}");
                                bmp.Save($"{ @"./ImageTemp/temp.jpg"}");
                                GlueCheck_c.ProcessImage(LastVI, $"{ @"./ImageTemp/temp.jpg"}", out distance);
                                if (distance[0] <= Position.Instance.OutsideDistance && distance[1] <= Position.Instance.insideDistance)
                                {
                                    Marking.GlueResult = true;
                                }
                                else
                                {
                                    Marking.GlueResult = false;
                                }
                                GlueCheck.GlueCheck_C(bmp, frmAAVision.acq.hWindowControl1.HalconWindow, Marking.GlueResult, distance);
                                LastVI.Dispose();
                            }
                            Marking.GlueCheckTestSucceed = true;
                        }
                        catch (Exception ex)
                        {
                            log.Debug("图像识别异常333！" + ex.Message + ex.StackTrace);
                        }
                        VI.Dispose();
                    }
                    else//空跑模式
                    {
                        Marking.GlueCheckTest = false;
                        Marking.GlueResult = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("图像识别异常！" + ex.Message + ex.StackTrace);
            }
            Marking.NeedleLocateTestFinish = true;
            Marking.CenterLocateTestFinish = true;
            Marking.GlueCheckTestFinish = true;
            //SetPictureBoxImg(pictureBox1, bmp);
        }

        private void UpdataControl(bool status)
        {
            if (status)
            {
               
                int Index = CB_CamsList.SelectedIndex;
                if (Index>=0)
                {
                    _CamSN = CB_CamsList.Items[Index].ToString();
                    CB_CamsList.Text = CB_CamsList.Items[Index].ToString();
                }
            }
            else
            {
                
                if (BaumerCameraSystem.listCamera!=null&&BaumerCameraSystem.listCamera.Count>0)
                {
                    for (int i = 0; i < BaumerCameraSystem.listCamera.Count; i++)
                    {
                        //CB_CamsList.Items.Add(BaumerCameraSystem.listCamera[i].strSN);
                    }
                    
                }
              
            }

        }

        private void CB_CamsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdataControl(true);
        }

        //int xPos;
        //int yPos;
        //bool MoveFlag;
        //private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    pictureBox1.Focus();
        //    MoveFlag = true;
        //    xPos = e.X;
        //    yPos = e.Y;
        //}

        //private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        //{
        //    MoveFlag = false;
        //}

        //private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (MoveFlag)
        //    {
        //        pictureBox1.Left += Convert.ToInt16(e.X - xPos);
        //        pictureBox1.Top += Convert.ToInt16(e.Y - yPos);
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            if (_Subject != null)
            {
                _Subject.Initialize(_CamSN);

                _Subject.TriggerSource = TriggerSource.Software;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_Subject != null)
            {
                _Subject.Close();
            }

            if (_Subject == null)
            {
                MessageBox.Show("关闭成功");
            }
        }
    }
}
