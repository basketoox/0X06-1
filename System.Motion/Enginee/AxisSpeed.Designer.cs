namespace Motion.Enginee
{
    partial class AxisSpeed
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
            this.lblAxisSpeed = new System.Windows.Forms.Label();
            this.tkrSpeedRate = new System.Windows.Forms.TrackBar();
            this.lblAxisSpeedRate = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tkrSpeedRate)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAxisSpeed
            // 
            this.lblAxisSpeed.AutoSize = true;
            this.lblAxisSpeed.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblAxisSpeed.Location = new System.Drawing.Point(352, 0);
            this.lblAxisSpeed.Name = "lblAxisSpeed";
            this.lblAxisSpeed.Size = new System.Drawing.Size(59, 12);
            this.lblAxisSpeed.TabIndex = 85;
            this.lblAxisSpeed.Text = "10.00mm/s";
            this.lblAxisSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tkrSpeedRate
            // 
            this.tkrSpeedRate.AutoSize = false;
            this.tkrSpeedRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tkrSpeedRate.Location = new System.Drawing.Point(0, 14);
            this.tkrSpeedRate.Maximum = 1000;
            this.tkrSpeedRate.Name = "tkrSpeedRate";
            this.tkrSpeedRate.Size = new System.Drawing.Size(411, 16);
            this.tkrSpeedRate.TabIndex = 84;
            this.tkrSpeedRate.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tkrSpeedRate.Value = 10;
            this.tkrSpeedRate.Scroll += new System.EventHandler(this.tkrSpeedRate_Scroll);
            // 
            // lblAxisSpeedRate
            // 
            this.lblAxisSpeedRate.AutoSize = true;
            this.lblAxisSpeedRate.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblAxisSpeedRate.Location = new System.Drawing.Point(0, 0);
            this.lblAxisSpeedRate.Name = "lblAxisSpeedRate";
            this.lblAxisSpeedRate.Size = new System.Drawing.Size(101, 12);
            this.lblAxisSpeedRate.TabIndex = 83;
            this.lblAxisSpeedRate.Text = "X轴运行速度(10%)";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblAxisSpeedRate);
            this.panel1.Controls.Add(this.lblAxisSpeed);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(411, 14);
            this.panel1.TabIndex = 86;
            // 
            // AxisSpeed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tkrSpeedRate);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AxisSpeed";
            this.Size = new System.Drawing.Size(411, 30);
            ((System.ComponentModel.ISupportInitialize)(this.tkrSpeedRate)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAxisSpeed;
        private System.Windows.Forms.TrackBar tkrSpeedRate;
        private System.Windows.Forms.Label lblAxisSpeedRate;
        private System.Windows.Forms.Panel panel1;
    }
}
