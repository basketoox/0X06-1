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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string path;

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
            double[] ResultDistance = new double[4];
            imageViewer.Palette.Type = GlueCheck_c.ProcessImage(imageViewer.Image,path,out ResultDistance);
            button1.Enabled = true;
            LoadImageButton.Enabled = false;
            RunButton.Enabled = false;
            insideVal.Text = ResultDistance[0].ToString("f3");
            outsideVal.Text = ResultDistance[1].ToString("f3");
            label3.Text = "X:"+ResultDistance[2].ToString("f3") + ",Y:" + ResultDistance[3].ToString("f3");
            if (ResultDistance[0] <= (double)numoutside.Value && ResultDistance[1] <= (double)numinside.Value)
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
            Position.Instance.OutsideDistance = (double)numoutside.Value;
            Position.Instance.insideDistance = (double)numinside.Value;
            Position.Instance.RedMax_Threshold = (int)numRedMax.Value;
            Position.Instance.GreenMax_Threshold = (int)numGreenMax.Value;
            Position.Instance.BlueMax_Threshold = (int)numBlueMax.Value; 
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
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = true;
            LoadImageButton.Enabled = false;
            RunButton.Enabled = false;
            CheckResult.Text = "";
            CheckResult.BackColor = Color.Gray;
            label1.Location = new Point(19, 96);
            numoutside.Location = new Point(19, 111);
            label2.Location = new Point(19, 154);
            numinside.Location = new Point(19, 169);
            button1.Location = new Point(24,360);
            LoadImageButton.Location = new Point(24,386);
            RunButton.Location = new Point(24,412);
            ExitButton.Location = new Point(24,438);
            numoutside.Value = (decimal)Position.Instance.OutsideDistance;
            numinside.Value = (decimal)Position.Instance.insideDistance;
            numRedMax.Value = Position.Instance.RedMax_Threshold;
            numGreenMax.Value = Position.Instance.GreenMax_Threshold;
            numBlueMax.Value = Position.Instance.BlueMax_Threshold;
        }
    }
}