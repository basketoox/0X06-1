namespace desay
{
    partial class frmAAVision
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
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.GlueCheck_c = new System.Windows.Forms.Button();
            this.RectangleCheck = new System.Windows.Forms.Button();
            this.btn_ReadImg = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button11 = new System.Windows.Forms.Button();
            this.cb_SelectImg = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_ShowImgInfo = new System.Windows.Forms.CheckBox();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.m_CtrlHStatusLabelCtrl = new System.Windows.Forms.Label();
            this.btn_SaveImg = new System.Windows.Forms.Button();
            this.lblPath = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.NUPday = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUPday)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(24, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "一次获取";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(24, 117);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(116, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "圆心检测";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(24, 88);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(116, 23);
            this.button5.TabIndex = 8;
            this.button5.Text = "对针标定";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(168, 117);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(116, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "点胶圆心识别参数";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(168, 88);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(116, 23);
            this.button6.TabIndex = 9;
            this.button6.Text = "对针识别参数";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(168, 146);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(116, 23);
            this.button7.TabIndex = 10;
            this.button7.Text = "胶水识别参数";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(24, 146);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(116, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "胶水检测";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(347, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(137, 312);
            this.panel1.TabIndex = 12;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(29, 121);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(116, 23);
            this.button8.TabIndex = 15;
            this.button8.Text = "胶水检测";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(29, 63);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(116, 23);
            this.button9.TabIndex = 14;
            this.button9.Text = "对针标定";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(29, 92);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(116, 23);
            this.button10.TabIndex = 13;
            this.button10.Text = "圆心检测";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.GlueCheck_c);
            this.groupBox1.Controls.Add(this.RectangleCheck);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Controls.Add(this.btn_ReadImg);
            this.groupBox1.Location = new System.Drawing.Point(14, 345);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(312, 157);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "本地测试";
            // 
            // GlueCheck_c
            // 
            this.GlueCheck_c.Location = new System.Drawing.Point(168, 34);
            this.GlueCheck_c.Name = "GlueCheck_c";
            this.GlueCheck_c.Size = new System.Drawing.Size(116, 23);
            this.GlueCheck_c.TabIndex = 24;
            this.GlueCheck_c.Text = "点胶检测";
            this.GlueCheck_c.UseVisualStyleBackColor = true;
            this.GlueCheck_c.Click += new System.EventHandler(this.GlueCheck_c_Click);
            // 
            // RectangleCheck
            // 
            this.RectangleCheck.Location = new System.Drawing.Point(168, 121);
            this.RectangleCheck.Name = "RectangleCheck";
            this.RectangleCheck.Size = new System.Drawing.Size(116, 23);
            this.RectangleCheck.TabIndex = 24;
            this.RectangleCheck.Text = "矩形检测";
            this.RectangleCheck.UseVisualStyleBackColor = true;
            this.RectangleCheck.Click += new System.EventHandler(this.RectangleCheck_Click);
            // 
            // btn_ReadImg
            // 
            this.btn_ReadImg.Location = new System.Drawing.Point(29, 34);
            this.btn_ReadImg.Name = "btn_ReadImg";
            this.btn_ReadImg.Size = new System.Drawing.Size(116, 23);
            this.btn_ReadImg.TabIndex = 13;
            this.btn_ReadImg.Text = "读取图片";
            this.btn_ReadImg.UseVisualStyleBackColor = true;
            this.btn_ReadImg.Click += new System.EventHandler(this.btn_ReadImg_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button11);
            this.groupBox2.Controls.Add(this.cb_SelectImg);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.cb_ShowImgInfo);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button7);
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Location = new System.Drawing.Point(14, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(312, 320);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "在线检测";
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(24, 175);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(116, 23);
            this.button11.TabIndex = 14;
            this.button11.Text = "矩形检测";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // cb_SelectImg
            // 
            this.cb_SelectImg.AutoSize = true;
            this.cb_SelectImg.Location = new System.Drawing.Point(171, 35);
            this.cb_SelectImg.Name = "cb_SelectImg";
            this.cb_SelectImg.Size = new System.Drawing.Size(84, 16);
            this.cb_SelectImg.TabIndex = 13;
            this.cb_SelectImg.Text = "选择本地图";
            this.cb_SelectImg.UseVisualStyleBackColor = true;
            this.cb_SelectImg.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(29, 35);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "保存图片";
            // 
            // cb_ShowImgInfo
            // 
            this.cb_ShowImgInfo.AutoSize = true;
            this.cb_ShowImgInfo.Location = new System.Drawing.Point(171, 63);
            this.cb_ShowImgInfo.Name = "cb_ShowImgInfo";
            this.cb_ShowImgInfo.Size = new System.Drawing.Size(96, 16);
            this.cb_ShowImgInfo.TabIndex = 13;
            this.cb_ShowImgInfo.Text = "显示图像信息";
            this.cb_ShowImgInfo.UseVisualStyleBackColor = true;
            this.cb_ShowImgInfo.CheckedChanged += new System.EventHandler(this.cb_ShowImgInfo_CheckedChanged);
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(499, 27);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(603, 422);
            this.hWindowControl1.TabIndex = 18;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(603, 422);
            this.hWindowControl1.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseMove);
            // 
            // m_CtrlHStatusLabelCtrl
            // 
            this.m_CtrlHStatusLabelCtrl.AutoSize = true;
            this.m_CtrlHStatusLabelCtrl.Location = new System.Drawing.Point(506, 8);
            this.m_CtrlHStatusLabelCtrl.Name = "m_CtrlHStatusLabelCtrl";
            this.m_CtrlHStatusLabelCtrl.Size = new System.Drawing.Size(41, 12);
            this.m_CtrlHStatusLabelCtrl.TabIndex = 19;
            this.m_CtrlHStatusLabelCtrl.Text = "Image:";
            // 
            // btn_SaveImg
            // 
            this.btn_SaveImg.Location = new System.Drawing.Point(499, 466);
            this.btn_SaveImg.Name = "btn_SaveImg";
            this.btn_SaveImg.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveImg.TabIndex = 3;
            this.btn_SaveImg.Text = "保存图片";
            this.btn_SaveImg.UseVisualStyleBackColor = true;
            this.btn_SaveImg.Click += new System.EventHandler(this.btn_SaveImg_Click);
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Font = new System.Drawing.Font("宋体", 10F);
            this.lblPath.Location = new System.Drawing.Point(496, 500);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(105, 14);
            this.lblPath.TabIndex = 20;
            this.lblPath.Text = "图片保存路径：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10F);
            this.label3.Location = new System.Drawing.Point(882, 470);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 14);
            this.label3.TabIndex = 21;
            this.label3.Text = "图片文件清理夹间隔：";
            // 
            // NUPday
            // 
            this.NUPday.Font = new System.Drawing.Font("宋体", 10F);
            this.NUPday.Location = new System.Drawing.Point(1023, 466);
            this.NUPday.Name = "NUPday";
            this.NUPday.Size = new System.Drawing.Size(52, 23);
            this.NUPday.TabIndex = 22;
            this.NUPday.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NUPday.ValueChanged += new System.EventHandler(this.NUPday_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10F);
            this.label4.Location = new System.Drawing.Point(1081, 470);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 14);
            this.label4.TabIndex = 23;
            this.label4.Text = "天";
            // 
            // frmAAVision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1108, 525);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.NUPday);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.m_CtrlHStatusLabelCtrl);
            this.Controls.Add(this.hWindowControl1);
            this.Controls.Add(this.btn_SaveImg);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmAAVision";
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAAVision_FormClosing);
            this.Load += new System.EventHandler(this.frmAAVision_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUPday)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        public HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.CheckBox cb_SelectImg;
        private System.Windows.Forms.Button btn_ReadImg;
        private System.Windows.Forms.CheckBox cb_ShowImgInfo;
        private System.Windows.Forms.Label m_CtrlHStatusLabelCtrl;
        private System.Windows.Forms.Button btn_SaveImg;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NUPday;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button RectangleCheck;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button GlueCheck_c;
    }
}

