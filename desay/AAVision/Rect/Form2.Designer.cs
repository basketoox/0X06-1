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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
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
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHiLimt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLowLimt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCenterY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCenterX)).BeginInit();
            this.SuspendLayout();
            // 
            // imageViewer
            // 
            resources.ApplyResources(this.imageViewer, "imageViewer");
            this.imageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            resources.ApplyResources(this.openFileDialog, "openFileDialog");
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.imageViewer);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
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
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
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
            resources.ApplyResources(this.CheckResult, "CheckResult");
            this.CheckResult.BackColor = System.Drawing.Color.Silver;
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
            // Form2
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHiLimt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLowLimt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCenterY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCenterX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NationalInstruments.Vision.WindowsForms.ImageViewer imageViewer;
        private System.Windows.Forms.Button LoadImageButton;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBox1;
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
    }
}

