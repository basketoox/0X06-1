using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using desay.ProductData;
using System.Toolkit.Helpers;

namespace Vision_Assistant
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        string path;
        double[] offsetOri;
        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog1 = new OpenFileDialog();
            dialog1.Multiselect = false;
            dialog1.Title = "";
            dialog1.Filter = "bmp(*.bmp)|*.*";
            if (dialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileInformation fileinfo = Algorithms.GetFileInformation(dialog1.FileName);
                imageViewer.Image.Type = fileinfo.ImageType;
                imageViewer.Image.ReadFile(dialog1.FileName);
                LoadImageButton.Enabled = false;
                RunButton.Enabled = false;
                RunButton.Enabled = true;
            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            imageViewer.Palette.Type = RectGlueCheck.ProcessImage(imageViewer.Image, offsetOri);
            button1.Enabled = true;
            LoadImageButton.Enabled = false;
            RunButton.Enabled = false;

            if (JudegCenterPosition() && JudgeRectangleSize() && JudegMaxMassArea() && JudgeRectPerimeter())
            {
                CheckResult.Text = "OK";
                CheckResult.BackColor = Color.GreenYellow;
            }
            else
            {
                CheckResult.Text = "NG";
                CheckResult.BackColor = Color.Red;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Position.Instance.CenterOffset_X = (double)numCenterX.Value;
            Position.Instance.CenterOffset_Y = (double)numCenterY.Value;
            Position.Instance.MaxGlueArea = (double)numHiLimt.Value;
            Position.Instance.MinGlueArea = (double)numLowLimt.Value;
            Position.Instance.ManualThreshold = (int)numThreshold.Value;

            Position.Instance.Pos_X[0] = (double)X0.Value;
            Position.Instance.Pos_X[1] = (double)X1.Value;
            Position.Instance.Pos_X[2] = (double)X2.Value;
            Position.Instance.Pos_X[3] = (double)X3.Value;
            Position.Instance.Pos_X[4] = (double)X4.Value;
            Position.Instance.Pos_Y[0] = (double)Y0.Value;
            Position.Instance.Pos_Y[1] = (double)Y1.Value;
            Position.Instance.Pos_Y[2] = (double)Y2.Value;
            Position.Instance.Pos_Y[3] = (double)Y3.Value;
            Position.Instance.Pos_Y[4] = (double)Y4.Value;
            SerializerManager<Position>.Instance.Save(AppConfig.ConfigPositionName, Position.Instance);
            MessageBox.Show("OK");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "";
            dialog.Filter = "bmp(*.bmp)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = dialog.FileName;
                FileInformation fileinfo = Algorithms.GetFileInformation(dialog.FileName);
                imageViewer.Image.Type = fileinfo.ImageType;
                imageViewer.Image.ReadFile(path);
                button1.Enabled = false;
                LoadImageButton.Enabled = true;
                CheckResult.Text = "";
                CheckResult.BackColor = Color.Gray;

                VisionImage VI = new VisionImage(ImageType.Rgb32, 7);
                VI.ReadFile(path);
                //获取图片相对基准的偏差
                Image_Processing.ProcessImage(VI);
                offsetOri = GetOffsetOri();
                VI.Dispose();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            button1.Enabled = true;
            LoadImageButton.Enabled = false;
            RunButton.Enabled = false;
            CheckResult.Text = "";
            CheckResult.BackColor = Color.Gray;
            numCenterX.Value = (decimal)Position.Instance.CenterOffset_X;
            numCenterY.Value = (decimal)Position.Instance.CenterOffset_Y;
            numHiLimt.Value = (decimal)Position.Instance.MaxGlueArea;
            numLowLimt.Value = (decimal)Position.Instance.MinGlueArea;
            numThreshold.Value = (decimal)Position.Instance.ManualThreshold;

            X0.Value = (decimal)Position.Instance.Pos_X[0];
            X1.Value = (decimal)Position.Instance.Pos_X[1];
            X2.Value = (decimal)Position.Instance.Pos_X[2];
            X3.Value = (decimal)Position.Instance.Pos_X[3];
            X4.Value = (decimal)Position.Instance.Pos_X[4];
            Y0.Value = (decimal)Position.Instance.Pos_Y[0];
            Y1.Value = (decimal)Position.Instance.Pos_Y[1];
            Y2.Value = (decimal)Position.Instance.Pos_Y[2];
            Y3.Value = (decimal)Position.Instance.Pos_Y[3];
            Y4.Value = (decimal)Position.Instance.Pos_Y[4];
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        public double[] GetOffsetOri()
        {
            double[] offset = new double[] { 0, 0 };
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
                offset[0] = Image_Processing.gpm2Results[0].CalibratedPosition.X - 1759.58;
                offset[1] = Image_Processing.gpm2Results[0].CalibratedPosition.Y - 1411.70;
            }
            else
            {
                offset[0] = 0;
                offset[1] = 0;
            }
            return offset;
        }

        public static bool JudegCenterPosition()
        {
            int CenterPos_X = (int)RectGlueCheck.vaParticleReport.PixelMeasurements[RectGlueCheck.MaxMassIndex, 0];
            int CenterPos_Y = (int)RectGlueCheck.vaParticleReport.PixelMeasurements[RectGlueCheck.MaxMassIndex, 1];
            int Left = 1296 - (int)Position.Instance.CenterOffset_X;
            int Top = 972 - (int)Position.Instance.CenterOffset_Y;
            int Right = 1296 + (int)Position.Instance.CenterOffset_X;
            int Bottom = 972 + (int)Position.Instance.CenterOffset_Y;
            if (CenterPos_X >= Left && CenterPos_X <= Right && CenterPos_Y >= Top && CenterPos_Y <= Bottom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool JudgeRectangleSize()
        {
            int width = (int)RectGlueCheck.vaParticleReport.PixelMeasurements[RectGlueCheck.MaxMassIndex, 2];
            int height = (int)RectGlueCheck.vaParticleReport.PixelMeasurements[RectGlueCheck.MaxMassIndex, 3];
            int AbsDiffValue = Math.Abs(width - height);
            if (AbsDiffValue < 100)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool JudegMaxMassArea()
        {
            double area = RectGlueCheck.TotalAreas;
            double LowLimt = Position.Instance.MinGlueArea;
            double HiLimt = Position.Instance.MaxGlueArea;
            if (area >= LowLimt && area <= HiLimt)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool JudgeRectPerimeter()
        {
            int Perimeter = (int)RectGlueCheck.vaParticleReport.PixelMeasurements[RectGlueCheck.MaxMassIndex, 4];            
            if (Perimeter >= Position.Instance.RectGluePreimeter)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}