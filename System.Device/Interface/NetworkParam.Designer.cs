namespace System.Device
{
    partial class NetworkParam
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_WriteTimeOut = new System.Windows.Forms.TextBox();
            this.textBox_ReadTimeOut = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_PortNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_IPAddr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_WriteTimeOut
            // 
            this.textBox_WriteTimeOut.Location = new System.Drawing.Point(57, 108);
            this.textBox_WriteTimeOut.Name = "textBox_WriteTimeOut";
            this.textBox_WriteTimeOut.Size = new System.Drawing.Size(123, 21);
            this.textBox_WriteTimeOut.TabIndex = 48;
            // 
            // textBox_ReadTimeOut
            // 
            this.textBox_ReadTimeOut.Location = new System.Drawing.Point(57, 74);
            this.textBox_ReadTimeOut.Name = "textBox_ReadTimeOut";
            this.textBox_ReadTimeOut.Size = new System.Drawing.Size(123, 21);
            this.textBox_ReadTimeOut.TabIndex = 47;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 46;
            this.label7.Text = "写超时";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 45;
            this.label6.Text = "读超时";
            // 
            // textBox_PortNumber
            // 
            this.textBox_PortNumber.Location = new System.Drawing.Point(57, 40);
            this.textBox_PortNumber.Name = "textBox_PortNumber";
            this.textBox_PortNumber.Size = new System.Drawing.Size(123, 21);
            this.textBox_PortNumber.TabIndex = 50;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 49;
            this.label1.Text = "端口号";
            // 
            // textBox_IPAddr
            // 
            this.textBox_IPAddr.Location = new System.Drawing.Point(57, 6);
            this.textBox_IPAddr.Name = "textBox_IPAddr";
            this.textBox_IPAddr.Size = new System.Drawing.Size(123, 21);
            this.textBox_IPAddr.TabIndex = 52;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 51;
            this.label2.Text = "IP地址";
            // 
            // NetworkParam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox_IPAddr);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_PortNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_WriteTimeOut);
            this.Controls.Add(this.textBox_ReadTimeOut);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Name = "NetworkParam";
            this.Size = new System.Drawing.Size(189, 139);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_WriteTimeOut;
        private System.Windows.Forms.TextBox textBox_ReadTimeOut;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_PortNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_IPAddr;
        private System.Windows.Forms.Label label2;
    }
}
