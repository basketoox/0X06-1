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

namespace desay
{
    public partial class frmAAVision : Form
    {
        //该模块为独立模块，暂不与主线程代码融合（但功能已集成）,该模块可以单独调试（在program中启动）
        public static bool GetBmp = false;

        private static string ConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + "AAVisionConfig.ini";

        public static frmAAVision acq;

        private BaumerCamera _Subject;

        public bool SaveImage = false;

        public frmAAVision()
        {
            InitializeComponent();
            acq = this;
            ReadParamFromFile();
            _Subject = new BaumerCamera(); ;
            
        }
        public static void ReadParamFromFile()
        {

            CenterLocate.circleCenter_x = IniOperation.ReadDoubleValue(ConfigFilePath, "CenterLocate", "circleCenter_x", CenterLocate.circleCenter_x);
            CenterLocate.circleCenter_y = IniOperation.ReadDoubleValue(ConfigFilePath, "CenterLocate", "circleCenter_y", CenterLocate.circleCenter_y);
            CenterLocate.circleRadius = IniOperation.ReadDoubleValue(ConfigFilePath, "CenterLocate", "circleRadius", CenterLocate.circleRadius);

            CenterLocate.threshold_min = IniOperation.ReadDoubleValue(ConfigFilePath, "CenterLocate", "threshold_min", CenterLocate.threshold_min);
            CenterLocate.threshold_max = IniOperation.ReadDoubleValue(ConfigFilePath, "CenterLocate", "m_laserOnVision", CenterLocate.threshold_max);

            CenterLocate.areaMin = IniOperation.ReadDoubleValue(ConfigFilePath, "CenterLocate", "areaMin", CenterLocate.areaMin);
            CenterLocate.areaMax = IniOperation.ReadDoubleValue(ConfigFilePath, "CenterLocate", "areaMax", CenterLocate.areaMax);

            NeedleLocate.circleCenter_x = IniOperation.ReadDoubleValue(ConfigFilePath, "NeedleLocate", "circleCenter_x", NeedleLocate.circleCenter_x);
            NeedleLocate.circleCenter_y = IniOperation.ReadDoubleValue(ConfigFilePath, "NeedleLocate", "circleCenter_y", NeedleLocate.circleCenter_y);
            NeedleLocate.circleRadius = IniOperation.ReadDoubleValue(ConfigFilePath, "NeedleLocate", "circleRadius", NeedleLocate.circleRadius);
            NeedleLocate.areaSizeMin = IniOperation.ReadDoubleValue(ConfigFilePath, "NeedleLocate", "areaSizeMin", NeedleLocate.areaSizeMin);

            GlueCheck.circleCenter_x = IniOperation.ReadDoubleValue(ConfigFilePath, "GlueCheck", "circleCenter_x", GlueCheck.circleCenter_x);
            GlueCheck.circleCenter_y = IniOperation.ReadDoubleValue(ConfigFilePath, "GlueCheck", "circleCenter_y", GlueCheck.circleCenter_y);
            GlueCheck.circleRadius = IniOperation.ReadDoubleValue(ConfigFilePath, "GlueCheck", "circleRadius", GlueCheck.circleRadius);
            GlueCheck.glueOverflowOutter = IniOperation.ReadDoubleValue(ConfigFilePath, "GlueCheck", "glueOverflowOutter", GlueCheck.glueOverflowOutter);
            GlueCheck.glueOverflowInner = IniOperation.ReadDoubleValue(ConfigFilePath, "GlueCheck", "glueOverflowInner", GlueCheck.glueOverflowInner);
            GlueCheck.glueOffset = IniOperation.ReadDoubleValue(ConfigFilePath, "GlueCheck", "glueOffset", GlueCheck.glueOffset);
            GlueCheck.glueLackOutter = IniOperation.ReadDoubleValue(ConfigFilePath, "GlueCheck", "glueLackOutter", GlueCheck.glueLackOutter);
            GlueCheck.glueLackInner = IniOperation.ReadDoubleValue(ConfigFilePath, "GlueCheck", "glueLackInner", GlueCheck.glueLackInner);
            GlueCheck.kernel = IniOperation.ReadDoubleValue(ConfigFilePath, "GlueCheck", "kernel", GlueCheck.kernel);

            acq.SaveImage = IniOperation.ReadBoolValue(ConfigFilePath, "acq", "SaveImage", acq.SaveImage);
        }
        public static void WriteParamToFile()
        {
            IniOperation.WriteDoubleValue(ConfigFilePath, "CenterLocate", "circleCenter_x", CenterLocate.circleCenter_x);
            IniOperation.WriteDoubleValue(ConfigFilePath, "CenterLocate", "circleCenter_y", CenterLocate.circleCenter_y);
            IniOperation.WriteDoubleValue(ConfigFilePath, "CenterLocate", "circleRadius", CenterLocate.circleRadius);

            IniOperation.WriteDoubleValue(ConfigFilePath, "CenterLocate", "threshold_min", CenterLocate.threshold_min);
            IniOperation.WriteDoubleValue(ConfigFilePath, "CenterLocate", "m_laserOnVision", CenterLocate.threshold_max);

            IniOperation.WriteDoubleValue(ConfigFilePath, "CenterLocate", "areaMin", CenterLocate.areaMin);
            IniOperation.WriteDoubleValue(ConfigFilePath, "CenterLocate", "areaMax", CenterLocate.areaMax);

            IniOperation.WriteDoubleValue(ConfigFilePath, "NeedleLocate", "circleCenter_x", NeedleLocate.circleCenter_x);
            IniOperation.WriteDoubleValue(ConfigFilePath, "NeedleLocate", "circleCenter_y", NeedleLocate.circleCenter_y);
            IniOperation.WriteDoubleValue(ConfigFilePath, "NeedleLocate", "circleRadius", NeedleLocate.circleRadius);
            IniOperation.WriteDoubleValue(ConfigFilePath, "NeedleLocate", "areaSizeMin", NeedleLocate.areaSizeMin);

            IniOperation.WriteDoubleValue(ConfigFilePath, "GlueCheck", "circleCenter_x", GlueCheck.circleCenter_x);
            IniOperation.WriteDoubleValue(ConfigFilePath, "GlueCheck", "circleCenter_y", GlueCheck.circleCenter_y);
            IniOperation.WriteDoubleValue(ConfigFilePath, "GlueCheck", "circleRadius", GlueCheck.circleRadius);
            IniOperation.WriteDoubleValue(ConfigFilePath, "GlueCheck", "kernel", GlueCheck.kernel);
            IniOperation.WriteDoubleValue(ConfigFilePath, "GlueCheck", "glueOverflowOutter", GlueCheck.glueOverflowOutter);
            IniOperation.WriteDoubleValue(ConfigFilePath, "GlueCheck", "glueOverflowInner", GlueCheck.glueOverflowInner);
            IniOperation.WriteDoubleValue(ConfigFilePath, "GlueCheck", "glueOffset", GlueCheck.glueOffset);
            IniOperation.WriteDoubleValue(ConfigFilePath, "GlueCheck", "glueLackOutter", GlueCheck.glueLackOutter);
            IniOperation.WriteDoubleValue(ConfigFilePath, "GlueCheck", "glueLackInner", GlueCheck.glueLackInner);

            IniOperation.WriteBoolValue(ConfigFilePath, "SaveImage", "acq", acq.SaveImage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Acquire();
        }

        private void Acquire()
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

        private void NeedleLocateTestAcquire()
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
            Marking.CenterLocateTestSucceed = false;
            Acquire();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Acquire();
            new CenterLocateParameter(ImageFactory.CreateBitmap(_Subject.OutputImageData)).ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Acquire();
            new frmNeedleLocParameter(ImageFactory.CreateBitmap(_Subject.OutputImageData)).ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Acquire();
            new frmGlueCheck(ImageFactory.CreateBitmap(_Subject.OutputImageData)).ShowDialog();
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
                this.WindowState = FormWindowState.Maximized;
                edit.AcqToolEdit_Load(null, null);
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
                Bitmap Bmp = new Bitmap(Image.FromFile(dialog.FileName));  // 加载图像
                CenterLocate.TestBmp(Bmp, frmAAVision.acq.hWindowControl1.HalconWindow, acq.SaveImage);
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
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            acq.SaveImage = this.checkBox1.Checked;
            WriteParamToFile();
        }
    }
}
