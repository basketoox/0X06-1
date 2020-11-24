namespace Motion.Enginee
{
    partial class MoveSelectVertical
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ndnPosOther = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.rbnPos10um = new System.Windows.Forms.RadioButton();
            this.rbnPosOtherum = new System.Windows.Forms.RadioButton();
            this.rbnPos100um = new System.Windows.Forms.RadioButton();
            this.rbnPos1000um = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbnContinueMoveSelect = new System.Windows.Forms.RadioButton();
            this.rbnLocationMoveSelect = new System.Windows.Forms.RadioButton();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndnPosOther)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Controls.Add(this.panel1);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(196, 218);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "选择";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ndnPosOther);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.rbnPos10um);
            this.groupBox5.Controls.Add(this.rbnPosOtherum);
            this.groupBox5.Controls.Add(this.rbnPos100um);
            this.groupBox5.Controls.Add(this.rbnPos1000um);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(4, 78);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(188, 136);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "定距选择";
            // 
            // ndnPosOther
            // 
            this.ndnPosOther.DecimalPlaces = 2;
            this.ndnPosOther.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ndnPosOther.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ndnPosOther.Location = new System.Drawing.Point(45, 99);
            this.ndnPosOther.Margin = new System.Windows.Forms.Padding(4);
            this.ndnPosOther.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.ndnPosOther.Name = "ndnPosOther";
            this.ndnPosOther.Size = new System.Drawing.Size(99, 25);
            this.ndnPosOther.TabIndex = 7;
            this.ndnPosOther.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(148, 104);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "mm";
            // 
            // rbnPos10um
            // 
            this.rbnPos10um.AutoSize = true;
            this.rbnPos10um.Location = new System.Drawing.Point(23, 19);
            this.rbnPos10um.Margin = new System.Windows.Forms.Padding(4);
            this.rbnPos10um.Name = "rbnPos10um";
            this.rbnPos10um.Size = new System.Drawing.Size(76, 19);
            this.rbnPos10um.TabIndex = 0;
            this.rbnPos10um.TabStop = true;
            this.rbnPos10um.Text = "0.01mm";
            this.rbnPos10um.UseVisualStyleBackColor = true;
            this.rbnPos10um.CheckedChanged += new System.EventHandler(this.rbnPos10um_CheckedChanged);
            // 
            // rbnPosOtherum
            // 
            this.rbnPosOtherum.AutoSize = true;
            this.rbnPosOtherum.Location = new System.Drawing.Point(23, 103);
            this.rbnPosOtherum.Margin = new System.Windows.Forms.Padding(4);
            this.rbnPosOtherum.Name = "rbnPosOtherum";
            this.rbnPosOtherum.Size = new System.Drawing.Size(17, 16);
            this.rbnPosOtherum.TabIndex = 0;
            this.rbnPosOtherum.TabStop = true;
            this.rbnPosOtherum.UseVisualStyleBackColor = true;
            this.rbnPosOtherum.CheckedChanged += new System.EventHandler(this.rbnPosOtherum_CheckedChanged);
            // 
            // rbnPos100um
            // 
            this.rbnPos100um.AutoSize = true;
            this.rbnPos100um.Location = new System.Drawing.Point(23, 46);
            this.rbnPos100um.Margin = new System.Windows.Forms.Padding(4);
            this.rbnPos100um.Name = "rbnPos100um";
            this.rbnPos100um.Size = new System.Drawing.Size(76, 19);
            this.rbnPos100um.TabIndex = 0;
            this.rbnPos100um.TabStop = true;
            this.rbnPos100um.Text = "0.10mm";
            this.rbnPos100um.UseVisualStyleBackColor = true;
            this.rbnPos100um.CheckedChanged += new System.EventHandler(this.rbnPos100um_CheckedChanged);
            // 
            // rbnPos1000um
            // 
            this.rbnPos1000um.AutoSize = true;
            this.rbnPos1000um.Location = new System.Drawing.Point(23, 73);
            this.rbnPos1000um.Margin = new System.Windows.Forms.Padding(4);
            this.rbnPos1000um.Name = "rbnPos1000um";
            this.rbnPos1000um.Size = new System.Drawing.Size(76, 19);
            this.rbnPos1000um.TabIndex = 0;
            this.rbnPos1000um.TabStop = true;
            this.rbnPos1000um.Text = "1.00mm";
            this.rbnPos1000um.UseVisualStyleBackColor = true;
            this.rbnPos1000um.CheckedChanged += new System.EventHandler(this.rbnPos1000um_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbnContinueMoveSelect);
            this.panel1.Controls.Add(this.rbnLocationMoveSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(4, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(188, 56);
            this.panel1.TabIndex = 0;
            // 
            // rbnContinueMoveSelect
            // 
            this.rbnContinueMoveSelect.AutoSize = true;
            this.rbnContinueMoveSelect.Location = new System.Drawing.Point(23, 4);
            this.rbnContinueMoveSelect.Margin = new System.Windows.Forms.Padding(4);
            this.rbnContinueMoveSelect.Name = "rbnContinueMoveSelect";
            this.rbnContinueMoveSelect.Size = new System.Drawing.Size(58, 19);
            this.rbnContinueMoveSelect.TabIndex = 0;
            this.rbnContinueMoveSelect.TabStop = true;
            this.rbnContinueMoveSelect.Text = "连续";
            this.rbnContinueMoveSelect.UseVisualStyleBackColor = true;
            this.rbnContinueMoveSelect.CheckedChanged += new System.EventHandler(this.rbnContinueMoveSelect_CheckedChanged);
            // 
            // rbnLocationMoveSelect
            // 
            this.rbnLocationMoveSelect.AutoSize = true;
            this.rbnLocationMoveSelect.Location = new System.Drawing.Point(23, 31);
            this.rbnLocationMoveSelect.Margin = new System.Windows.Forms.Padding(4);
            this.rbnLocationMoveSelect.Name = "rbnLocationMoveSelect";
            this.rbnLocationMoveSelect.Size = new System.Drawing.Size(58, 19);
            this.rbnLocationMoveSelect.TabIndex = 0;
            this.rbnLocationMoveSelect.TabStop = true;
            this.rbnLocationMoveSelect.Text = "定距";
            this.rbnLocationMoveSelect.UseVisualStyleBackColor = true;
            this.rbnLocationMoveSelect.CheckedChanged += new System.EventHandler(this.rbnLocationMoveSelect_CheckedChanged);
            // 
            // MoveSelectVertical
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Name = "MoveSelectVertical";
            this.Size = new System.Drawing.Size(196, 218);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndnPosOther)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown ndnPosOther;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbnPos10um;
        private System.Windows.Forms.RadioButton rbnPosOtherum;
        private System.Windows.Forms.RadioButton rbnPos100um;
        private System.Windows.Forms.RadioButton rbnPos1000um;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbnContinueMoveSelect;
        private System.Windows.Forms.RadioButton rbnLocationMoveSelect;
    }
}
