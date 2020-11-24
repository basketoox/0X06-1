namespace Motion.Enginee
{
    partial class ModelOperate
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
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSpliceReset = new System.Windows.Forms.Button();
            this.btnJogStart = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnSingleStart = new System.Windows.Forms.Button();
            this.lblInitializeStatus = new System.Windows.Forms.Label();
            this.lblOperateStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnStop.Location = new System.Drawing.Point(728, 0);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(91, 40);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnStop_MouseDown);
            this.btnStop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnStop_MouseUp);
            // 
            // btnSpliceReset
            // 
            this.btnSpliceReset.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSpliceReset.Location = new System.Drawing.Point(819, 0);
            this.btnSpliceReset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSpliceReset.Name = "btnSpliceReset";
            this.btnSpliceReset.Size = new System.Drawing.Size(91, 40);
            this.btnSpliceReset.TabIndex = 6;
            this.btnSpliceReset.Text = "复位";
            this.btnSpliceReset.UseVisualStyleBackColor = true;
            this.btnSpliceReset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSpliceReset_MouseDown);
            this.btnSpliceReset.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSpliceReset_MouseUp);
            // 
            // btnJogStart
            // 
            this.btnJogStart.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnJogStart.Location = new System.Drawing.Point(546, 0);
            this.btnJogStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnJogStart.Name = "btnJogStart";
            this.btnJogStart.Size = new System.Drawing.Size(91, 40);
            this.btnJogStart.TabIndex = 8;
            this.btnJogStart.Text = "点动";
            this.btnJogStart.UseVisualStyleBackColor = true;
            this.btnJogStart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogStart_MouseDown);
            this.btnJogStart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogStart_MouseUp);
            // 
            // btnPause
            // 
            this.btnPause.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPause.Location = new System.Drawing.Point(637, 0);
            this.btnPause.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(91, 40);
            this.btnPause.TabIndex = 7;
            this.btnPause.Text = "暂停";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPause_MouseDown);
            this.btnPause.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPause_MouseUp);
            // 
            // btnSingleStart
            // 
            this.btnSingleStart.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSingleStart.Location = new System.Drawing.Point(455, 0);
            this.btnSingleStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSingleStart.Name = "btnSingleStart";
            this.btnSingleStart.Size = new System.Drawing.Size(91, 40);
            this.btnSingleStart.TabIndex = 9;
            this.btnSingleStart.Text = "单动";
            this.btnSingleStart.UseVisualStyleBackColor = true;
            this.btnSingleStart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSingleStart_MouseDown);
            this.btnSingleStart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSingleStart_MouseUp);
            // 
            // lblInitializeStatus
            // 
            this.lblInitializeStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblInitializeStatus.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInitializeStatus.ForeColor = System.Drawing.Color.Green;
            this.lblInitializeStatus.Location = new System.Drawing.Point(150, 0);
            this.lblInitializeStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInitializeStatus.Name = "lblInitializeStatus";
            this.lblInitializeStatus.Size = new System.Drawing.Size(127, 40);
            this.lblInitializeStatus.TabIndex = 17;
            this.lblInitializeStatus.Text = "初始化完成";
            this.lblInitializeStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOperateStatus
            // 
            this.lblOperateStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblOperateStatus.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOperateStatus.ForeColor = System.Drawing.Color.Green;
            this.lblOperateStatus.Location = new System.Drawing.Point(0, 0);
            this.lblOperateStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOperateStatus.Name = "lblOperateStatus";
            this.lblOperateStatus.Size = new System.Drawing.Size(142, 40);
            this.lblOperateStatus.TabIndex = 15;
            this.lblOperateStatus.Text = "模组准备好";
            this.lblOperateStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ModelOperate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblInitializeStatus);
            this.Controls.Add(this.lblOperateStatus);
            this.Controls.Add(this.btnSingleStart);
            this.Controls.Add(this.btnJogStart);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnSpliceReset);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ModelOperate";
            this.Size = new System.Drawing.Size(910, 40);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnSpliceReset;
        private System.Windows.Forms.Button btnJogStart;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnSingleStart;
        private System.Windows.Forms.Label lblInitializeStatus;
        private System.Windows.Forms.Label lblOperateStatus;
    }
}
