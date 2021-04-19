namespace Vision_Assistant
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.imageViewer = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            this.LoadImageButton = new System.Windows.Forms.Button();
            this.RunButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numThreshold = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numHiLimt = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numLowLimt = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numCenterY = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numCenterX = new System.Windows.Forms.NumericUpDown();
            this.CheckResult = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label16 = new System.Windows.Forms.Label();
            this.X1 = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Y1 = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Y3 = new System.Windows.Forms.NumericUpDown();
            this.Y2 = new System.Windows.Forms.NumericUpDown();
            this.Y4 = new System.Windows.Forms.NumericUpDown();
            this.Y0 = new System.Windows.Forms.NumericUpDown();
            this.X3 = new System.Windows.Forms.NumericUpDown();
            this.X2 = new System.Windows.Forms.NumericUpDown();
            this.X4 = new System.Windows.Forms.NumericUpDown();
            this.X0 = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.numRectPosScore = new System.Windows.Forms.NumericUpDown();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHiLimt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLowLimt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCenterY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCenterX)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Y1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Y3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Y2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Y4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Y0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.X0)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRectPosScore)).BeginInit();
            this.SuspendLayout();
            // 
            // imageViewer
            // 
            this.imageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.imageViewer, "imageViewer");
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.ZoomToFit = true;
            // 
            // LoadImageButton
            // 
            resources.ApplyResources(this.LoadImageButton, "LoadImageButton");
            this.LoadImageButton.Name = "LoadImageButton";
            this.LoadImageButton.UseVisualStyleBackColor = true;
            this.LoadImageButton.Click += new System.EventHandler(this.LoadImageButton_Click);
            // 
            // RunButton
            // 
            resources.ApplyResources(this.RunButton, "RunButton");
            this.RunButton.Name = "RunButton";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // ExitButton
            // 
            resources.ApplyResources(this.ExitButton, "ExitButton");
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numRectPosScore);
            this.groupBox2.Controls.Add(this.numThreshold);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numHiLimt);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.numLowLimt);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.numCenterY);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.numCenterX);
            this.groupBox2.Controls.Add(this.CheckResult);
            this.groupBox2.Controls.Add(this.ExitButton);
            this.groupBox2.Controls.Add(this.RunButton);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.LoadImageButton);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // numThreshold
            // 
            resources.ApplyResources(this.numThreshold, "numThreshold");
            this.numThreshold.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.numThreshold.Name = "numThreshold";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // numHiLimt
            // 
            resources.ApplyResources(this.numHiLimt, "numHiLimt");
            this.numHiLimt.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numHiLimt.Name = "numHiLimt";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // numLowLimt
            // 
            resources.ApplyResources(this.numLowLimt, "numLowLimt");
            this.numLowLimt.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numLowLimt.Name = "numLowLimt";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // numCenterY
            // 
            resources.ApplyResources(this.numCenterY, "numCenterY");
            this.numCenterY.Maximum = new decimal(new int[] {
            1944,
            0,
            0,
            0});
            this.numCenterY.Name = "numCenterY";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // numCenterX
            // 
            resources.ApplyResources(this.numCenterX, "numCenterX");
            this.numCenterX.Maximum = new decimal(new int[] {
            2592,
            0,
            0,
            0});
            this.numCenterX.Name = "numCenterX";
            // 
            // CheckResult
            // 
            this.CheckResult.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.CheckResult, "CheckResult");
            this.CheckResult.Name = "CheckResult";
            this.CheckResult.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.imageViewer);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label16);
            this.tabPage2.Controls.Add(this.X1);
            this.tabPage2.Controls.Add(this.label15);
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.Y1);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.Y3);
            this.tabPage2.Controls.Add(this.Y2);
            this.tabPage2.Controls.Add(this.Y4);
            this.tabPage2.Controls.Add(this.Y0);
            this.tabPage2.Controls.Add(this.X3);
            this.tabPage2.Controls.Add(this.X2);
            this.tabPage2.Controls.Add(this.X4);
            this.tabPage2.Controls.Add(this.X0);
            this.tabPage2.Controls.Add(this.panel1);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // X1
            // 
            this.X1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.X1, "X1");
            this.X1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.X1.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.X1.Name = "X1";
            this.X1.Value = new decimal(new int[] {
            580,
            0,
            0,
            0});
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // Y1
            // 
            this.Y1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.Y1, "Y1");
            this.Y1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Y1.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.Y1.Name = "Y1";
            this.Y1.Value = new decimal(new int[] {
            650,
            0,
            0,
            -2147483648});
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // Y3
            // 
            this.Y3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.Y3, "Y3");
            this.Y3.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Y3.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.Y3.Name = "Y3";
            this.Y3.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
            // 
            // Y2
            // 
            this.Y2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.Y2, "Y2");
            this.Y2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Y2.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.Y2.Name = "Y2";
            this.Y2.Value = new decimal(new int[] {
            580,
            0,
            0,
            -2147483648});
            // 
            // Y4
            // 
            this.Y4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.Y4, "Y4");
            this.Y4.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Y4.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.Y4.Name = "Y4";
            this.Y4.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
            // 
            // Y0
            // 
            this.Y0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.Y0, "Y0");
            this.Y0.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.Y0.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.Y0.Name = "Y0";
            this.Y0.Value = new decimal(new int[] {
            650,
            0,
            0,
            -2147483648});
            // 
            // X3
            // 
            this.X3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.X3, "X3");
            this.X3.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.X3.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.X3.Name = "X3";
            this.X3.Value = new decimal(new int[] {
            655,
            0,
            0,
            0});
            // 
            // X2
            // 
            this.X2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.X2, "X2");
            this.X2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.X2.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.X2.Name = "X2";
            this.X2.Value = new decimal(new int[] {
            655,
            0,
            0,
            0});
            // 
            // X4
            // 
            this.X4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.X4, "X4");
            this.X4.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.X4.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.X4.Name = "X4";
            this.X4.Value = new decimal(new int[] {
            630,
            0,
            0,
            -2147483648});
            // 
            // X0
            // 
            this.X0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.X0, "X0");
            this.X0.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.X0.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.X0.Name = "X0";
            this.X0.Value = new decimal(new int[] {
            630,
            0,
            0,
            -2147483648});
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::desay.Properties.Resources._20_5M;
            this.panel1.Controls.Add(this.label6);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // numRectPosScore
            // 
            resources.ApplyResources(this.numRectPosScore, "numRectPosScore");
            this.numRectPosScore.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numRectPosScore.Name = "numRectPosScore";
            // 
            // Form2
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHiLimt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLowLimt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCenterY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCenterX)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.X1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Y1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Y3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Y2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Y4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Y0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.X0)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRectPosScore)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NationalInstruments.Vision.WindowsForms.ImageViewer imageViewer;
        private System.Windows.Forms.Button LoadImageButton;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button CheckResult;
        private System.Windows.Forms.NumericUpDown numCenterY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numCenterX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numHiLimt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numLowLimt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numThreshold;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown X0;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown X1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown Y1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown Y3;
        private System.Windows.Forms.NumericUpDown Y2;
        private System.Windows.Forms.NumericUpDown Y4;
        private System.Windows.Forms.NumericUpDown Y0;
        private System.Windows.Forms.NumericUpDown X3;
        private System.Windows.Forms.NumericUpDown X2;
        private System.Windows.Forms.NumericUpDown X4;
        private System.Windows.Forms.NumericUpDown numRectPosScore;
        private System.Windows.Forms.Label label17;
    }
}

