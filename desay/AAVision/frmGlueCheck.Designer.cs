using HalconDotNet;

namespace desay.AAVision
{
    partial class frmGlueCheck
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
            this.tB_GlueWidth = new System.Windows.Forms.TextBox();
            this.tB_GlueInnerCircle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.winContr_GCP = new HalconDotNet.HWindowControl();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.GlueLack_in = new System.Windows.Forms.TextBox();
            this.GlueLack_out = new System.Windows.Forms.TextBox();
            this.GlueCenterOffset = new System.Windows.Forms.TextBox();
            this.GlueOver_in = new System.Windows.Forms.TextBox();
            this.GlueOver_out = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.m_CtrlHStatusLabelCtrl = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.btnCreateCircle = new System.Windows.Forms.Button();
            this.btnCreateRing = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.fillarea = new System.Windows.Forms.TextBox();
            this.closearea = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.openarea = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.threshold_min = new System.Windows.Forms.TextBox();
            this.threshold_max = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // tB_GlueWidth
            // 
            this.tB_GlueWidth.Location = new System.Drawing.Point(100, 59);
            this.tB_GlueWidth.Name = "tB_GlueWidth";
            this.tB_GlueWidth.Size = new System.Drawing.Size(100, 21);
            this.tB_GlueWidth.TabIndex = 13;
            // 
            // tB_GlueInnerCircle
            // 
            this.tB_GlueInnerCircle.Location = new System.Drawing.Point(100, 23);
            this.tB_GlueInnerCircle.Name = "tB_GlueInnerCircle";
            this.tB_GlueInnerCircle.Size = new System.Drawing.Size(100, 21);
            this.tB_GlueInnerCircle.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "胶水最大宽度";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "产品内圈半径";
            // 
            // winContr_GCP
            // 
            this.winContr_GCP.BackColor = System.Drawing.Color.Black;
            this.winContr_GCP.BorderColor = System.Drawing.Color.Black;
            this.winContr_GCP.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.winContr_GCP.Location = new System.Drawing.Point(509, 24);
            this.winContr_GCP.Name = "winContr_GCP";
            this.winContr_GCP.Size = new System.Drawing.Size(648, 486);
            this.winContr_GCP.TabIndex = 0;
            this.winContr_GCP.WindowSize = new System.Drawing.Size(648, 486);
            this.winContr_GCP.HMouseMove += new HalconDotNet.HMouseEventHandler(this.winContr_GCP_HMouseMove);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(139, 450);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(90, 23);
            this.button9.TabIndex = 11;
            this.button9.Text = "取消";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(19, 450);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(90, 23);
            this.button8.TabIndex = 10;
            this.button8.Text = "确定";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // GlueLack_in
            // 
            this.GlueLack_in.Location = new System.Drawing.Point(106, 60);
            this.GlueLack_in.Name = "GlueLack_in";
            this.GlueLack_in.Size = new System.Drawing.Size(100, 21);
            this.GlueLack_in.TabIndex = 9;
            // 
            // GlueLack_out
            // 
            this.GlueLack_out.Location = new System.Drawing.Point(106, 60);
            this.GlueLack_out.Name = "GlueLack_out";
            this.GlueLack_out.Size = new System.Drawing.Size(100, 21);
            this.GlueLack_out.TabIndex = 8;
            // 
            // GlueCenterOffset
            // 
            this.GlueCenterOffset.Location = new System.Drawing.Point(106, 27);
            this.GlueCenterOffset.Name = "GlueCenterOffset";
            this.GlueCenterOffset.Size = new System.Drawing.Size(100, 21);
            this.GlueCenterOffset.TabIndex = 7;
            // 
            // GlueOver_in
            // 
            this.GlueOver_in.Location = new System.Drawing.Point(106, 23);
            this.GlueOver_in.Name = "GlueOver_in";
            this.GlueOver_in.Size = new System.Drawing.Size(100, 21);
            this.GlueOver_in.TabIndex = 6;
            // 
            // GlueOver_out
            // 
            this.GlueOver_out.Location = new System.Drawing.Point(106, 25);
            this.GlueOver_out.Name = "GlueOver_out";
            this.GlueOver_out.Size = new System.Drawing.Size(100, 21);
            this.GlueOver_out.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 12);
            this.label11.TabIndex = 4;
            this.label11.Text = "偏移判断阈值";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 3;
            this.label10.Text = "缺胶判断阈值";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "缺胶判断阈值";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "溢胶判断阈值";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "溢胶判断阈值";
            // 
            // m_CtrlHStatusLabelCtrl
            // 
            this.m_CtrlHStatusLabelCtrl.AutoSize = true;
            this.m_CtrlHStatusLabelCtrl.Location = new System.Drawing.Point(509, 9);
            this.m_CtrlHStatusLabelCtrl.Name = "m_CtrlHStatusLabelCtrl";
            this.m_CtrlHStatusLabelCtrl.Size = new System.Drawing.Size(41, 12);
            this.m_CtrlHStatusLabelCtrl.TabIndex = 25;
            this.m_CtrlHStatusLabelCtrl.Text = "Image:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox9);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(257, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 493);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "胶水判断参数";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.btnCreateCircle);
            this.groupBox9.Controls.Add(this.btnCreateRing);
            this.groupBox9.Location = new System.Drawing.Point(14, 358);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(218, 115);
            this.groupBox9.TabIndex = 28;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "胶水ROI";
            // 
            // btnCreateCircle
            // 
            this.btnCreateCircle.Location = new System.Drawing.Point(37, 71);
            this.btnCreateCircle.Name = "btnCreateCircle";
            this.btnCreateCircle.Size = new System.Drawing.Size(145, 23);
            this.btnCreateCircle.TabIndex = 0;
            this.btnCreateCircle.Text = "圆形";
            this.btnCreateCircle.UseVisualStyleBackColor = true;
            this.btnCreateCircle.Click += new System.EventHandler(this.btnCreateCircle_Click);
            // 
            // btnCreateRing
            // 
            this.btnCreateRing.Location = new System.Drawing.Point(37, 32);
            this.btnCreateRing.Name = "btnCreateRing";
            this.btnCreateRing.Size = new System.Drawing.Size(145, 23);
            this.btnCreateRing.TabIndex = 0;
            this.btnCreateRing.Text = "圆环";
            this.btnCreateRing.UseVisualStyleBackColor = true;
            this.btnCreateRing.Click += new System.EventHandler(this.btnCreateRing_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.GlueCenterOffset);
            this.groupBox4.Location = new System.Drawing.Point(14, 269);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(218, 76);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "胶圈位置";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.GlueOver_in);
            this.groupBox3.Controls.Add(this.GlueLack_in);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(14, 145);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(218, 102);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "胶水内圈";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.GlueOver_out);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.GlueLack_out);
            this.groupBox2.Location = new System.Drawing.Point(14, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(218, 98);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "胶水外圈";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button9);
            this.groupBox5.Controls.Add(this.groupBox8);
            this.groupBox5.Controls.Add(this.groupBox6);
            this.groupBox5.Controls.Add(this.button8);
            this.groupBox5.Controls.Add(this.groupBox7);
            this.groupBox5.Location = new System.Drawing.Point(5, 17);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(246, 493);
            this.groupBox5.TabIndex = 27;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "基本参数";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.fillarea);
            this.groupBox8.Controls.Add(this.closearea);
            this.groupBox8.Controls.Add(this.label14);
            this.groupBox8.Controls.Add(this.label13);
            this.groupBox8.Controls.Add(this.openarea);
            this.groupBox8.Controls.Add(this.label12);
            this.groupBox8.Location = new System.Drawing.Point(16, 269);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(213, 135);
            this.groupBox8.TabIndex = 29;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "胶水筛选参数";
            // 
            // fillarea
            // 
            this.fillarea.Location = new System.Drawing.Point(100, 89);
            this.fillarea.Name = "fillarea";
            this.fillarea.Size = new System.Drawing.Size(100, 21);
            this.fillarea.TabIndex = 1;
            // 
            // closearea
            // 
            this.closearea.Location = new System.Drawing.Point(100, 58);
            this.closearea.Name = "closearea";
            this.closearea.Size = new System.Drawing.Size(100, 21);
            this.closearea.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 95);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 12);
            this.label14.TabIndex = 0;
            this.label14.Text = "填充区间阈值";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 63);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "形状最大阈值";
            // 
            // openarea
            // 
            this.openarea.Location = new System.Drawing.Point(100, 27);
            this.openarea.Name = "openarea";
            this.openarea.Size = new System.Drawing.Size(100, 21);
            this.openarea.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 33);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "形状最小阈值";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.tB_GlueInnerCircle);
            this.groupBox6.Controls.Add(this.tB_GlueWidth);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Location = new System.Drawing.Point(16, 30);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(213, 98);
            this.groupBox6.TabIndex = 28;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "产品参数";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label6);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Controls.Add(this.threshold_min);
            this.groupBox7.Controls.Add(this.threshold_max);
            this.groupBox7.Location = new System.Drawing.Point(16, 145);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(213, 102);
            this.groupBox7.TabIndex = 28;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "胶水分割参数";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "胶水最大阈值";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "胶水最小阈值";
            // 
            // threshold_min
            // 
            this.threshold_min.Location = new System.Drawing.Point(100, 23);
            this.threshold_min.Name = "threshold_min";
            this.threshold_min.Size = new System.Drawing.Size(100, 21);
            this.threshold_min.TabIndex = 1;
            // 
            // threshold_max
            // 
            this.threshold_max.Location = new System.Drawing.Point(100, 60);
            this.threshold_max.Name = "threshold_max";
            this.threshold_max.Size = new System.Drawing.Size(100, 21);
            this.threshold_max.TabIndex = 1;
            // 
            // frmGlueCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 520);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_CtrlHStatusLabelCtrl);
            this.Controls.Add(this.winContr_GCP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmGlueCheck";
            this.Text = "胶水检查参数";
            this.Load += new System.EventHandler(this.frmGlueCheck_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private HWindowControl winContr_GCP;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox GlueLack_in;
        private System.Windows.Forms.TextBox GlueLack_out;
        private System.Windows.Forms.TextBox GlueCenterOffset;
        private System.Windows.Forms.TextBox GlueOver_in;
        private System.Windows.Forms.TextBox GlueOver_out;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tB_GlueWidth;
        private System.Windows.Forms.TextBox tB_GlueInnerCircle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label m_CtrlHStatusLabelCtrl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fillarea;
        private System.Windows.Forms.TextBox closearea;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox openarea;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox threshold_min;
        private System.Windows.Forms.TextBox threshold_max;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button btnCreateCircle;
        private System.Windows.Forms.Button btnCreateRing;
    }
}