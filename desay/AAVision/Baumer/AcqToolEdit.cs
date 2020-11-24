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
namespace desay
{
    public partial class AcqToolEdit : UserControl
    {
        static ILog log = LogManager.GetLogger(typeof(AcqToolEdit));


        private TabPage[] tabpage;
        Bitmap bmp;

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
                    try
                    {
                        Marking.NeedleLocateTest = false;
                        NeedleLocate.TestBmp(bmp, frmAAVision.acq.hWindowControl1.HalconWindow, Position.Instance.GlueAdjustPinPosition, Position.Instance.GlueCameraCalibPosition, frmAAVision.acq.SaveImage);
                        Marking.NeedleLocateTestSucceed = true;
                    }
                    catch (Exception ex)
                    {
                        log.Debug("图像识别异常111！" + ex.Message + ex.StackTrace);

                    }

                }
                else if (Marking.CenterLocateTest)
                {
                    try
                    {
                        Marking.CenterLocateTest = false;
                        CenterLocate.TestBmp(bmp, frmAAVision.acq.hWindowControl1.HalconWindow, frmAAVision.acq.SaveImage);
                        //
                    }
                    catch (Exception ex)
                    {
                        log.Debug("图像识别异常222！" + ex.Message + ex.StackTrace);
                    }
                }
                else if (Marking.GlueCheckTest)
                {
                    try {
                        Marking.GlueCheckTest = false;

                        GlueCheck.TestBmp(CenterLocate.LastCenterLocateBMP, bmp, frmAAVision.acq.hWindowControl1.HalconWindow, frmAAVision.acq.SaveImage);
                        Marking.GlueCheckTestSucceed = true;
                    }
                    catch (Exception ex)
                    {
                        log.Debug("图像识别异常333！" + ex.Message + ex.StackTrace);
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
