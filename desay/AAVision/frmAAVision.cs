using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using desay.ProductData;
using desay.AAVision;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using HalconDotNet;
using desay.AAVision.Algorithm;
using System.IO;
using NationalInstruments.Vision;
using Vision_Assistant;

namespace desay
{
    public partial class frmAAVision : Form
    {
        private string _filePath = @"./Image";
        //该模块为独立模块，暂不与主线程代码融合（但功能已集成）,该模块可以单独调试（在program中启动）
        public static bool GetBmp = false;

        //private static string ConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + "AAVisionConfig.ini";

        public static frmAAVision acq;

        Form1 form1;
        Form2 form2;
        private BaumerCamera _Subject;

        public bool SaveImage = true;

        Bitmap InputImage;
        public bool bShowImagePoint = false;
        HObject ho_Image = null;//原图像
        HTuple hv_Width;
        HTuple hv_Height;
        private string str_imgSize;
        public frmAAVision()
        {
            InitializeComponent();
            HOperatorSet.GenEmptyObj(out ho_Image);
            acq = this;
            ReadParamFromFile();
            _Subject = new BaumerCamera();
            lblPath.Text = "图片保存路径： " + System.Environment.CurrentDirectory + @"\Image";
            NUPday.Value = Position.Instance.DayOfImageSave;
            DeleteFiles();

        }
        public static void ReadParamFromFile()
        {

            CenterLocate.circleCenter_x = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "circleCenter_x", CenterLocate.circleCenter_x);
            CenterLocate.circleCenter_y = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "circleCenter_y", CenterLocate.circleCenter_y);
            CenterLocate.circleRadius = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "circleRadius", CenterLocate.circleRadius);

            CenterLocate.threshold_min = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "threshold_min", CenterLocate.threshold_min);
            CenterLocate.threshold_max = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "m_laserOnVision", CenterLocate.threshold_max);
            CenterLocate.eroKernel = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "eroKernel", CenterLocate.eroKernel);

            CenterLocate.areaMin = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "areaMin", CenterLocate.areaMin);
            CenterLocate.areaMax = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "areaMax", CenterLocate.areaMax);

            NeedleLocate.circleCenter_x = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "NeedleLocate", "circleCenter_x", NeedleLocate.circleCenter_x);
            NeedleLocate.circleCenter_y = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "NeedleLocate", "circleCenter_y", NeedleLocate.circleCenter_y);
            NeedleLocate.circleRadius = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "NeedleLocate", "circleRadius", NeedleLocate.circleRadius);
            NeedleLocate.areaSizeMin = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "NeedleLocate", "areaSizeMin", NeedleLocate.areaSizeMin);

            GlueCheck.GlueInnerCircle = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "GlueInnerCircle", GlueCheck.GlueInnerCircle);
            GlueCheck.GlueWidth = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "GlueWidth", GlueCheck.GlueWidth);
            GlueCheck.glueOverflowOutter = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "glueOverflowOutter", GlueCheck.glueOverflowOutter);
            GlueCheck.glueOverflowInner = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "glueOverflowInner", GlueCheck.glueOverflowInner);
            GlueCheck.glueOffset = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "glueOffset", GlueCheck.glueOffset);
            GlueCheck.glueLackOutter = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "glueLackOutter", GlueCheck.glueLackOutter);
            GlueCheck.glueLackInner = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "glueLackInner", GlueCheck.glueLackInner);
            GlueCheck.kernel = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "kernel", GlueCheck.kernel);

            GlueCheck.tol = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "tol", GlueCheck.tol);
            GlueCheck.area = IniOperation.ReadDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "area", GlueCheck.area);

            acq.SaveImage = IniOperation.ReadBoolValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "acq", "SaveImage", acq.SaveImage);

        }
        public static void WriteParamToFile()
        {
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "circleCenter_x", CenterLocate.circleCenter_x);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "circleCenter_y", CenterLocate.circleCenter_y);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "circleRadius", CenterLocate.circleRadius);

            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "threshold_min", CenterLocate.threshold_min);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "m_laserOnVision", CenterLocate.threshold_max);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "eroKernel", CenterLocate.eroKernel);

            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "areaMin", CenterLocate.areaMin);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "CenterLocate", "areaMax", CenterLocate.areaMax);

            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "NeedleLocate", "circleCenter_x", NeedleLocate.circleCenter_x);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "NeedleLocate", "circleCenter_y", NeedleLocate.circleCenter_y);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "NeedleLocate", "circleRadius", NeedleLocate.circleRadius);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "NeedleLocate", "areaSizeMin", NeedleLocate.areaSizeMin);

            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "GlueInnerCircle", GlueCheck.GlueInnerCircle);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "GlueWidth", GlueCheck.GlueWidth);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "kernel", GlueCheck.kernel);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "glueOverflowOutter", GlueCheck.glueOverflowOutter);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "glueOverflowInner", GlueCheck.glueOverflowInner);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "glueOffset", GlueCheck.glueOffset);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "glueLackOutter", GlueCheck.glueLackOutter);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "glueLackInner", GlueCheck.glueLackInner);

            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "tol", GlueCheck.tol);
            IniOperation.WriteDoubleValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "GlueCheck", "area", GlueCheck.area);

            IniOperation.WriteBoolValue(AppConfig.VisonPath + "\\AAVisionConfig.ini", "SaveImage", "acq", acq.SaveImage);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Acquire();
            try
            {
                ho_Image.Dispose();
                Bitmap2HObject.Bitmap2HObj(ImageFactory.CreateBitmap(_Subject.OutputImageData), out ho_Image);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                str_imgSize = string.Format("{0}X{1}", hv_Width, hv_Height);
                HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, hv_Height - 1, hv_Width - 1);
                HOperatorSet.DispObj(ho_Image, hWindowControl1.HalconWindow);
            }
            catch { MessageBox.Show("无图像"); }
        }

        private void Acquire()
        {

            if (_Subject == null)
            {
                return;
            }
            _Subject.Acquire();
            if (SaveImage)
            {
                try
                {
                    ho_Image.Dispose();
                    Bitmap2HObject.Bitmap2HObj(ImageFactory.CreateBitmap(_Subject.OutputImageData), out ho_Image);
                    if (!Directory.Exists(_filePath))
                    { Directory.CreateDirectory(_filePath); }

                    //SaveImg($"{_filePath }//{ DateTime.Now.ToString("yy_MM_dd_HH_mm_ss")}.bmp");
                    SaveImg_JPG($"{_filePath }//{ DateTime.Now.ToString("yy_MM_dd_HH_mm_ss_fff")}.jpg");
                }
                catch {; }
            }

        }

        public void AcquireRec()
        {

            if (_Subject == null)
            {
                return;
            }
            _Subject.Acquire();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NeedleLocateTestAcquire();

        }

        public void NeedleLocateTestAcquire()
        {
            Marking.NeedleLocateTest = true;
            Marking.NeedleLocateTestSucceed = false;
            Acquire();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CenterLocateTestAcquire();
        }

        public void CenterLocateTestAcquire()
        {
            Marking.CenterLocateTest = true;
            //Marking.CenterLocateTestSucceed = false;
            Acquire();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cb_SelectImg.Checked)
            {
                try
                {
                    new CenterLocateParameter(InputImage).ShowDialog();
                }
                catch (Exception ex) { MessageBox.Show($"打开失败{ex.ToString()}"); }
            }
            else
            {
                try
                {
                    Acquire();
                    new CenterLocateParameter(ImageFactory.CreateBitmap(_Subject.OutputImageData)).ShowDialog();
                }
                catch (Exception ex) { MessageBox.Show($"打开失败{ex.ToString()}"); }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (cb_SelectImg.Checked)
            {

                try
                {

                    new frmNeedleLocParameter(InputImage).ShowDialog();
                }
                catch (Exception ex) { MessageBox.Show($"打开失败{ex.ToString()}"); }
            }
            else
            {
                try
                {
                    Acquire();
                    new frmNeedleLocParameter(ImageFactory.CreateBitmap(_Subject.OutputImageData)).ShowDialog();
                }
                catch (Exception ex) { MessageBox.Show($"打开失败{ex.ToString()}"); }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (cb_SelectImg.Checked)
            {

                try
                {

                    new frmGlueCheck(InputImage).ShowDialog();
                }
                catch (Exception ex) { MessageBox.Show($"打开失败{ex.ToString()}"); }
            }
            else
            {

                try
                {
                    Acquire();
                    new frmGlueCheck(ImageFactory.CreateBitmap(_Subject.OutputImageData)).ShowDialog();
                }
                catch (Exception ex) { MessageBox.Show($"打开失败{ex.ToString()}"); }
            }
        }

        public void frmAAVision_Load(object sender, EventArgs e)
        {
            try
            {
                //直接创建避免影响自动化流程中调用
                AcqToolEdit edit = new AcqToolEdit();
                edit.Subject = _Subject;
                this.panel1.Controls.Add(edit);
                edit.Dock = DockStyle.Fill;
                //this.WindowState = FormWindowState.Maximized;
                edit.AcqToolEdit_Load(null, null);
                NUPday.Value = Position.Instance.DayOfImageSave;
                DeleteFiles();
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GlueCheckTestAcquire();
        }

        public void GlueCheckTestAcquire()
        {
            Marking.GlueCheckTest = true;
            Marking.GlueCheckTestSucceed = false;
            Acquire();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "bmp图片(*.bmp)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                Bitmap InputImage = new Bitmap(Image.FromFile(dialog.FileName));  // 加载图像
                CenterLocate.CenterMatch(InputImage, hWindowControl1.HalconWindow);

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "bmp图片(*.bmp)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                Bitmap Bmp = new Bitmap(Image.FromFile(dialog.FileName));  // 加载图像
                GlueCheck.TestBmp(CenterLocate.LastCenterLocateBMP, Bmp, frmAAVision.acq.hWindowControl1.HalconWindow, acq.SaveImage);
                Bmp.Dispose();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            acq.SaveImage = this.checkBox1.Checked;
            WriteParamToFile();
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void btn_ReadImg_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "bmp图片(*.bmp)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                InputImage = new Bitmap(Image.FromFile(dialog.FileName));  // 加载图像
                ho_Image.Dispose();
                Bitmap2HObject.Bitmap2HObj(InputImage, out ho_Image);
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                str_imgSize = string.Format("{0}X{1}", hv_Width, hv_Height);
                CenterLocate.ShowIMg(InputImage, hWindowControl1.HalconWindow);
                CenterLocate.TestBmp(InputImage, hWindowControl1.HalconWindow, SaveImage);

            }
        }

        private void hWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (ho_Image == null || bShowImagePoint == false /*|| bVideo == true*/)
                return;
            try
            {
                HTuple htuple;
                HOperatorSet.CountChannels(ho_Image, out htuple);
                double row;
                double column;
                int button;

                hWindowControl1.HalconWindow.GetMpositionSubPix(out row, out column, out button);
                string ss1 = string.Format("Y: {0:0000.0},X: {1:0000.0}", row, column);
                if (column >= 0.0 && column < hv_Width && (row >= 0.0 && row < hv_Height))
                {
                    string ss2;
                    if ((htuple) == 1)
                    //ss2 = string.Format("Val: {0:000.0}", himage.GetGrayval((int)row, (int)column));
                    {
                        HTuple value = 0;
                        HOperatorSet.GetGrayval(ho_Image, (int)row, (int)column, out value);
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
                        HOperatorSet.AccessChannel(ho_Image, out R, 1);
                        HOperatorSet.AccessChannel(ho_Image, out G, 2);
                        HOperatorSet.AccessChannel(ho_Image, out B, 3);
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
                    m_CtrlHStatusLabelCtrl.Text = str_imgSize + "    " + ss1 + "    " + ss2 + "  Xpxiel" + Config.Instance.CameraPixelMM_X + "  Ypxiel" + Config.Instance.CameraPixelMM_Y;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cb_ShowImgInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_ShowImgInfo.Checked)
            {
                bShowImagePoint = true;
            }
            else bShowImagePoint = false;
        }
        /// 保存图片 直接保存
        /// </summary>
        /// <param name="sSaveName"></param>
        /// <returns></returns>
        public bool SaveImg(string sSaveName)
        {
            try
            {

                HOperatorSet.WriteImage((HObject)this.ho_Image, ("bmp"), (0), (sSaveName));
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }
        /// 保存图片 直接保存
        /// </summary>
        /// <param name="sSaveName"></param>
        /// <returns></returns>
        public bool SaveImg_JPG(string sSaveName)
        {
            try
            {

                HOperatorSet.WriteImage((HObject)this.ho_Image, ("jpg"), (0), (sSaveName));
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }
        private void btn_SaveImg_Click(object sender, EventArgs e)
        {
            try
            {

                SaveFileDialog dg = new SaveFileDialog();
                dg.Title = "请选择文件夹";
                dg.Filter = "图像文件(*.bmp;*.jpg; *.jpg; *.jpeg; *.gif; *.png)| *.jpg; *.jpeg; *.gif; *.png;*.bmp";
                if (dg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string file = dg.FileName;
                    SaveImg(file);

                }


            }


            catch (Exception x)
            { }
        }

        private void NUPday_ValueChanged(object sender, EventArgs e)
        {
            Position.Instance.DayOfImageSave = (int)NUPday.Value;
        }
        private void DeleteFiles()
        {
            DateTime nowTime = DateTime.Now;
            DirectoryInfo root = new DirectoryInfo(@"./Image");
            FileInfo[] dics = root.GetFiles();
            foreach (FileInfo file in dics)//遍历文件夹
            {
                TimeSpan t = nowTime - file.CreationTime;  //当前时间  减去 文件创建时间
                int day = t.Days;
                if (day >= Position.Instance.DayOfImageSave)   //保存的时间 ；  单位：天
                {

                    File.Delete(file.FullName);  //删除超过时间的文件
                }
            }
        }

        private void RectangleCheck_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;//该值确定是否可以选择多个文件
                dialog.Title = "请选择文件夹";
                dialog.Filter = "bmp图片(*.bmp)|*.*";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Bitmap bmp = new Bitmap(Image.FromFile(dialog.FileName));
                    VisionImage VI = new VisionImage(ImageType.Rgb32);
                    VI.ReadFile(dialog.FileName);
                    Image_Processing.ProcessImage(VI);
                    double ICCenter_X = 0;
                    double ICCenter_Y = 0;
                    if (Image_Processing.gpm2Results.Count == 2)
                    {
                        if (Image_Processing.gpm2Results[0].Position.Y < Image_Processing.gpm2Results[1].Position.Y)
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
                        ICCenter_X = Image_Processing.gpm2Results[0].CalibratedPosition.X - 457;//457
                        ICCenter_Y = Image_Processing.gpm2Results[0].CalibratedPosition.Y - 414;//414
                        Marking.CenterLocateTestSucceed = true;  
                    }
                    CenterLocate.RectangleMatch(bmp, frmAAVision.acq.hWindowControl1.HalconWindow, Image_Processing.gpm2Results.Count == 1, ICCenter_X, ICCenter_Y);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开失败{ex.ToString()}");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Marking.CenterLocateTest = true;
            Acquire();
        }

        private void frmAAVision_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.FormOwnerClosing || e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void GlueCheck_c_Click(object sender, EventArgs e)
        {
            if (form1 == null || form1.IsDisposed)
            {
                form1 = new Form1();
            }
            form1.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (form2 == null || form2.IsDisposed)
            {
                form2 = new Form2();
            }
            form2.ShowDialog();
        }
    }
}
