namespace Motion.Enginee
{
    partial class AxisOperate
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gbxName = new System.Windows.Forms.GroupBox();
            this.btnHome = new System.Windows.Forms.Button();
            this.chxServoON = new System.Windows.Forms.CheckBox();
            this.btnJogAdd = new System.Windows.Forms.Button();
            this.btnJogDec = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.picALM = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.picMEL = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.picPEL = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.picSZ = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.picORG = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.picSON = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblJogSpeed = new System.Windows.Forms.Label();
            this.lblCurrentPosition = new System.Windows.Forms.Label();
            this.lblCurrentSpeed = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbrJogSpeed = new System.Windows.Forms.TrackBar();
            this.gbxName.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picALM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMEL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPEL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picORG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSON)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbrJogSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // gbxName
            // 
            this.gbxName.Controls.Add(this.btnHome);
            this.gbxName.Controls.Add(this.chxServoON);
            this.gbxName.Controls.Add(this.btnJogAdd);
            this.gbxName.Controls.Add(this.btnJogDec);
            this.gbxName.Controls.Add(this.panel2);
            this.gbxName.Controls.Add(this.tableLayoutPanel2);
            this.gbxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxName.Location = new System.Drawing.Point(0, 0);
            this.gbxName.Margin = new System.Windows.Forms.Padding(2);
            this.gbxName.Name = "gbxName";
            this.gbxName.Padding = new System.Windows.Forms.Padding(0);
            this.gbxName.Size = new System.Drawing.Size(166, 189);
            this.gbxName.TabIndex = 0;
            this.gbxName.TabStop = false;
            this.gbxName.Text = "轴名称";
            // 
            // btnHome
            // 
            this.btnHome.Location = new System.Drawing.Point(96, 104);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(68, 19);
            this.btnHome.TabIndex = 7;
            this.btnHome.Text = "HOME";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // chxServoON
            // 
            this.chxServoON.AutoSize = true;
            this.chxServoON.Location = new System.Drawing.Point(5, 107);
            this.chxServoON.Name = "chxServoON";
            this.chxServoON.Size = new System.Drawing.Size(84, 16);
            this.chxServoON.TabIndex = 6;
            this.chxServoON.Text = "使能ON/OFF";
            this.chxServoON.UseVisualStyleBackColor = true;
            this.chxServoON.CheckedChanged += new System.EventHandler(this.chxServoON_CheckedChanged);
            // 
            // btnJogAdd
            // 
            this.btnJogAdd.Location = new System.Drawing.Point(96, 154);
            this.btnJogAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnJogAdd.Name = "btnJogAdd";
            this.btnJogAdd.Size = new System.Drawing.Size(66, 34);
            this.btnJogAdd.TabIndex = 4;
            this.btnJogAdd.Text = "JOG+";
            this.btnJogAdd.UseVisualStyleBackColor = true;
            this.btnJogAdd.Click += new System.EventHandler(this.btnJogAdd_Click);
            this.btnJogAdd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogAdd_MouseDown);
            this.btnJogAdd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogAdd_MouseUp);
            // 
            // btnJogDec
            // 
            this.btnJogDec.Location = new System.Drawing.Point(4, 153);
            this.btnJogDec.Margin = new System.Windows.Forms.Padding(2);
            this.btnJogDec.Name = "btnJogDec";
            this.btnJogDec.Size = new System.Drawing.Size(66, 34);
            this.btnJogDec.TabIndex = 4;
            this.btnJogDec.Text = "JOG-";
            this.btnJogDec.UseVisualStyleBackColor = true;
            this.btnJogDec.Click += new System.EventHandler(this.btnJogDec_Click);
            this.btnJogDec.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogDec_MouseDown);
            this.btnJogDec.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogDec_MouseUp);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.picALM);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.picMEL);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.picPEL);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.picSZ);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.picORG);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.picSON);
            this.panel2.Location = new System.Drawing.Point(0, 124);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(166, 27);
            this.panel2.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(138, 14);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 12);
            this.label9.TabIndex = 15;
            this.label9.Text = "ALM";
            // 
            // picALM
            // 
            this.picALM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picALM.Image = global::Motion.Enginee.Properties.Resources.LedNone;
            this.picALM.Location = new System.Drawing.Point(142, 2);
            this.picALM.Margin = new System.Windows.Forms.Padding(2);
            this.picALM.Name = "picALM";
            this.picALM.Size = new System.Drawing.Size(12, 13);
            this.picALM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picALM.TabIndex = 14;
            this.picALM.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(111, 14);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "MEL";
            // 
            // picMEL
            // 
            this.picMEL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picMEL.Image = global::Motion.Enginee.Properties.Resources.LedNone;
            this.picMEL.Location = new System.Drawing.Point(115, 2);
            this.picMEL.Margin = new System.Windows.Forms.Padding(2);
            this.picMEL.Name = "picMEL";
            this.picMEL.Size = new System.Drawing.Size(12, 13);
            this.picMEL.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picMEL.TabIndex = 12;
            this.picMEL.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(84, 14);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "PEL";
            // 
            // picPEL
            // 
            this.picPEL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picPEL.Image = global::Motion.Enginee.Properties.Resources.LedNone;
            this.picPEL.Location = new System.Drawing.Point(88, 2);
            this.picPEL.Margin = new System.Windows.Forms.Padding(2);
            this.picPEL.Name = "picPEL";
            this.picPEL.Size = new System.Drawing.Size(12, 13);
            this.picPEL.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPEL.TabIndex = 10;
            this.picPEL.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "SZ";
            // 
            // picSZ
            // 
            this.picSZ.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picSZ.Image = global::Motion.Enginee.Properties.Resources.LedNone;
            this.picSZ.Location = new System.Drawing.Point(61, 2);
            this.picSZ.Margin = new System.Windows.Forms.Padding(2);
            this.picSZ.Name = "picSZ";
            this.picSZ.Size = new System.Drawing.Size(12, 13);
            this.picSZ.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSZ.TabIndex = 8;
            this.picSZ.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 14);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "ORG";
            // 
            // picORG
            // 
            this.picORG.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picORG.Image = global::Motion.Enginee.Properties.Resources.LedNone;
            this.picORG.Location = new System.Drawing.Point(34, 2);
            this.picORG.Margin = new System.Windows.Forms.Padding(2);
            this.picORG.Name = "picORG";
            this.picORG.Size = new System.Drawing.Size(12, 13);
            this.picORG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picORG.TabIndex = 6;
            this.picORG.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "SON";
            // 
            // picSON
            // 
            this.picSON.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picSON.Image = global::Motion.Enginee.Properties.Resources.LedNone;
            this.picSON.Location = new System.Drawing.Point(7, 2);
            this.picSON.Margin = new System.Windows.Forms.Padding(2);
            this.picSON.Name = "picSON";
            this.picSON.Size = new System.Drawing.Size(12, 13);
            this.picSON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSON.TabIndex = 4;
            this.picSON.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblJogSpeed, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblCurrentPosition, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblCurrentSpeed, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 14);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(166, 90);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(4, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 21);
            this.label1.TabIndex = 4;
            this.label1.Text = "JOG速度:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(4, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 21);
            this.label6.TabIndex = 0;
            this.label6.Text = "当前位置:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(4, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 21);
            this.label7.TabIndex = 1;
            this.label7.Text = "当前速度:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblJogSpeed
            // 
            this.lblJogSpeed.AutoSize = true;
            this.lblJogSpeed.BackColor = System.Drawing.Color.Black;
            this.lblJogSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblJogSpeed.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblJogSpeed.ForeColor = System.Drawing.Color.White;
            this.lblJogSpeed.Location = new System.Drawing.Point(67, 45);
            this.lblJogSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.lblJogSpeed.Name = "lblJogSpeed";
            this.lblJogSpeed.Size = new System.Drawing.Size(98, 21);
            this.lblJogSpeed.TabIndex = 1;
            this.lblJogSpeed.Text = "0000.000";
            this.lblJogSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrentPosition
            // 
            this.lblCurrentPosition.AutoSize = true;
            this.lblCurrentPosition.BackColor = System.Drawing.Color.Black;
            this.lblCurrentPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentPosition.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCurrentPosition.ForeColor = System.Drawing.Color.Lime;
            this.lblCurrentPosition.Location = new System.Drawing.Point(67, 1);
            this.lblCurrentPosition.Margin = new System.Windows.Forms.Padding(0);
            this.lblCurrentPosition.Name = "lblCurrentPosition";
            this.lblCurrentPosition.Size = new System.Drawing.Size(98, 21);
            this.lblCurrentPosition.TabIndex = 2;
            this.lblCurrentPosition.Text = "0000.000";
            this.lblCurrentPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrentSpeed
            // 
            this.lblCurrentSpeed.AutoSize = true;
            this.lblCurrentSpeed.BackColor = System.Drawing.Color.Black;
            this.lblCurrentSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCurrentSpeed.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCurrentSpeed.ForeColor = System.Drawing.Color.Lime;
            this.lblCurrentSpeed.Location = new System.Drawing.Point(67, 23);
            this.lblCurrentSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.lblCurrentSpeed.Name = "lblCurrentSpeed";
            this.lblCurrentSpeed.Size = new System.Drawing.Size(98, 21);
            this.lblCurrentSpeed.TabIndex = 3;
            this.lblCurrentSpeed.Text = "0000.000";
            this.lblCurrentSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.tbrJogSpeed);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 67);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(164, 22);
            this.panel1.TabIndex = 5;
            // 
            // tbrJogSpeed
            // 
            this.tbrJogSpeed.AutoSize = false;
            this.tbrJogSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbrJogSpeed.Location = new System.Drawing.Point(0, 0);
            this.tbrJogSpeed.Maximum = 100;
            this.tbrJogSpeed.Name = "tbrJogSpeed";
            this.tbrJogSpeed.Size = new System.Drawing.Size(164, 22);
            this.tbrJogSpeed.TabIndex = 85;
            this.tbrJogSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbrJogSpeed.Value = 10;
            this.tbrJogSpeed.Scroll += new System.EventHandler(this.tbrJogSpeed_Scroll);
            // 
            // AxisOperate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxName);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AxisOperate";
            this.Size = new System.Drawing.Size(166, 189);
            this.Load += new System.EventHandler(this.AxisOperate_Load);
            this.gbxName.ResumeLayout(false);
            this.gbxName.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picALM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMEL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPEL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picORG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSON)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbrJogSpeed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox picALM;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox picMEL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox picPEL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox picSZ;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox picORG;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picSON;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblJogSpeed;
        private System.Windows.Forms.Label lblCurrentPosition;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar tbrJogSpeed;
        private System.Windows.Forms.Button btnJogAdd;
        private System.Windows.Forms.Button btnJogDec;
        private System.Windows.Forms.CheckBox chxServoON;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblCurrentSpeed;
    }
}
