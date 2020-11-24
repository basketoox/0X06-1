namespace Motion.Enginee
{
    partial class VacuoParameter
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAlarmDelay = new System.Windows.Forms.TextBox();
            this.txtBrokenDelay = new System.Windows.Forms.TextBox();
            this.txtInhaleDelay = new System.Windows.Forms.TextBox();
            this.gbxName.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxName
            // 
            this.gbxName.Controls.Add(this.txtAlarmDelay);
            this.gbxName.Controls.Add(this.txtBrokenDelay);
            this.gbxName.Controls.Add(this.txtInhaleDelay);
            this.gbxName.Controls.Add(this.label3);
            this.gbxName.Controls.Add(this.label2);
            this.gbxName.Controls.Add(this.label6);
            this.gbxName.Controls.Add(this.label5);
            this.gbxName.Controls.Add(this.label4);
            this.gbxName.Controls.Add(this.label1);
            this.gbxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxName.Location = new System.Drawing.Point(0, 0);
            this.gbxName.Name = "gbxName";
            this.gbxName.Size = new System.Drawing.Size(115, 99);
            this.gbxName.TabIndex = 0;
            this.gbxName.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "报警:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "破空:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(100, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "s";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(100, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "s";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(100, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "s";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "吸气:";
            // 
            // txtAlarmDelay
            // 
            this.txtAlarmDelay.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAlarmDelay.Location = new System.Drawing.Point(39, 68);
            this.txtAlarmDelay.Name = "txtAlarmDelay";
            this.txtAlarmDelay.Size = new System.Drawing.Size(58, 23);
            this.txtAlarmDelay.TabIndex = 2;
            this.txtAlarmDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtBrokenDelay
            // 
            this.txtBrokenDelay.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBrokenDelay.Location = new System.Drawing.Point(39, 41);
            this.txtBrokenDelay.Name = "txtBrokenDelay";
            this.txtBrokenDelay.Size = new System.Drawing.Size(58, 23);
            this.txtBrokenDelay.TabIndex = 3;
            this.txtBrokenDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtInhaleDelay
            // 
            this.txtInhaleDelay.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtInhaleDelay.Location = new System.Drawing.Point(39, 14);
            this.txtInhaleDelay.Name = "txtInhaleDelay";
            this.txtInhaleDelay.Size = new System.Drawing.Size(58, 23);
            this.txtInhaleDelay.TabIndex = 4;
            this.txtInhaleDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // VacuoParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxName);
            this.Name = "VacuoParameter";
            this.Size = new System.Drawing.Size(115, 99);
            this.gbxName.ResumeLayout(false);
            this.gbxName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAlarmDelay;
        private System.Windows.Forms.TextBox txtBrokenDelay;
        private System.Windows.Forms.TextBox txtInhaleDelay;
    }
}
