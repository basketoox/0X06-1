namespace desay
{
    partial class frmCommSetting
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.autoScannerParam = new System.Device.SerialPortParam();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.manualScannerParam = new System.Device.SerialPortParam();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSocketTimeout = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtAAPort = new System.Windows.Forms.TextBox();
            this.txtWhitePort = new System.Windows.Forms.TextBox();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.txtCCDPort = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtMesFailCount = new System.Windows.Forms.TextBox();
            this.txtMesTimeout = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMesPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMesAddr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.HeightDetectParam = new System.Device.SerialPortParam();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.WhiteBoardPowerParam = new System.Device.SerialPortParam();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.autoScannerParam);
            this.groupBox1.Location = new System.Drawing.Point(4, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(191, 254);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "自动扫码枪";
            // 
            // autoScannerParam
            // 
            this.autoScannerParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoScannerParam.Location = new System.Drawing.Point(2, 16);
            this.autoScannerParam.Margin = new System.Windows.Forms.Padding(4);
            this.autoScannerParam.Name = "autoScannerParam";
            this.autoScannerParam.Size = new System.Drawing.Size(187, 236);
            this.autoScannerParam.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.manualScannerParam);
            this.groupBox2.Location = new System.Drawing.Point(203, 10);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(191, 254);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "手动扫码枪";
            // 
            // manualScannerParam
            // 
            this.manualScannerParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.manualScannerParam.Location = new System.Drawing.Point(2, 16);
            this.manualScannerParam.Name = "manualScannerParam";
            this.manualScannerParam.Size = new System.Drawing.Size(187, 236);
            this.manualScannerParam.TabIndex = 7;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(469, 326);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(58, 30);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(469, 435);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(58, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "退出";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.txtSocketTimeout);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.txtAAPort);
            this.groupBox5.Controls.Add(this.txtWhitePort);
            this.groupBox5.Controls.Add(this.txtServerIP);
            this.groupBox5.Controls.Add(this.txtCCDPort);
            this.groupBox5.Location = new System.Drawing.Point(4, 276);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox5.Size = new System.Drawing.Size(191, 206);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "网口通讯端口";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 94);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "AA通讯端口：";
            // 
            // txtSocketTimeout
            // 
            this.txtSocketTimeout.Location = new System.Drawing.Point(125, 125);
            this.txtSocketTimeout.Margin = new System.Windows.Forms.Padding(2);
            this.txtSocketTimeout.Name = "txtSocketTimeout";
            this.txtSocketTimeout.Size = new System.Drawing.Size(62, 21);
            this.txtSocketTimeout.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 60);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "白板通讯端口：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 163);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "服务器地址：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 26);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "CCD通讯端口：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 129);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "超时时间：";
            // 
            // txtAAPort
            // 
            this.txtAAPort.Location = new System.Drawing.Point(125, 90);
            this.txtAAPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtAAPort.Name = "txtAAPort";
            this.txtAAPort.Size = new System.Drawing.Size(62, 21);
            this.txtAAPort.TabIndex = 1;
            // 
            // txtWhitePort
            // 
            this.txtWhitePort.Location = new System.Drawing.Point(125, 56);
            this.txtWhitePort.Margin = new System.Windows.Forms.Padding(2);
            this.txtWhitePort.Name = "txtWhitePort";
            this.txtWhitePort.Size = new System.Drawing.Size(62, 21);
            this.txtWhitePort.TabIndex = 1;
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(82, 159);
            this.txtServerIP.Margin = new System.Windows.Forms.Padding(2);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(110, 21);
            this.txtServerIP.TabIndex = 1;
            // 
            // txtCCDPort
            // 
            this.txtCCDPort.Location = new System.Drawing.Point(125, 22);
            this.txtCCDPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtCCDPort.Name = "txtCCDPort";
            this.txtCCDPort.Size = new System.Drawing.Size(62, 21);
            this.txtCCDPort.TabIndex = 1;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtMesFailCount);
            this.groupBox6.Controls.Add(this.txtMesTimeout);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.txtMesPort);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.txtMesAddr);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Location = new System.Drawing.Point(203, 276);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox6.Size = new System.Drawing.Size(191, 206);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "MES";
            // 
            // txtMesFailCount
            // 
            this.txtMesFailCount.Location = new System.Drawing.Point(123, 159);
            this.txtMesFailCount.Margin = new System.Windows.Forms.Padding(2);
            this.txtMesFailCount.Name = "txtMesFailCount";
            this.txtMesFailCount.Size = new System.Drawing.Size(62, 21);
            this.txtMesFailCount.TabIndex = 1;
            // 
            // txtMesTimeout
            // 
            this.txtMesTimeout.Location = new System.Drawing.Point(123, 122);
            this.txtMesTimeout.Margin = new System.Windows.Forms.Padding(2);
            this.txtMesTimeout.Name = "txtMesTimeout";
            this.txtMesTimeout.Size = new System.Drawing.Size(62, 21);
            this.txtMesTimeout.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 163);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "失败次数：";
            // 
            // txtMesPort
            // 
            this.txtMesPort.Location = new System.Drawing.Point(123, 85);
            this.txtMesPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtMesPort.Name = "txtMesPort";
            this.txtMesPort.Size = new System.Drawing.Size(62, 21);
            this.txtMesPort.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 126);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "超时时间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 89);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "端口：";
            // 
            // txtMesAddr
            // 
            this.txtMesAddr.Location = new System.Drawing.Point(5, 50);
            this.txtMesAddr.Margin = new System.Windows.Forms.Padding(2);
            this.txtMesAddr.Name = "txtMesAddr";
            this.txtMesAddr.Size = new System.Drawing.Size(180, 21);
            this.txtMesAddr.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "地址：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.HeightDetectParam);
            this.groupBox3.Location = new System.Drawing.Point(402, 10);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(191, 254);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "测高";
            // 
            // HeightDetectParam
            // 
            this.HeightDetectParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeightDetectParam.Location = new System.Drawing.Point(2, 16);
            this.HeightDetectParam.Margin = new System.Windows.Forms.Padding(4);
            this.HeightDetectParam.Name = "HeightDetectParam";
            this.HeightDetectParam.Size = new System.Drawing.Size(187, 236);
            this.HeightDetectParam.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(404, 289);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(58, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(533, 289);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(58, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(402, 318);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(58, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "button1";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.WhiteBoardPowerParam);
            this.groupBox4.Location = new System.Drawing.Point(607, 11);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(191, 254);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "白板电源控制";
            // 
            // WhiteBoardPowerParam
            // 
            this.WhiteBoardPowerParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WhiteBoardPowerParam.Location = new System.Drawing.Point(2, 16);
            this.WhiteBoardPowerParam.Margin = new System.Windows.Forms.Padding(4);
            this.WhiteBoardPowerParam.Name = "WhiteBoardPowerParam";
            this.WhiteBoardPowerParam.Size = new System.Drawing.Size(187, 236);
            this.WhiteBoardPowerParam.TabIndex = 0;
            // 
            // frmCommSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 490);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmCommSetting";
            this.Text = "通讯设置";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Device.SerialPortParam autoScannerParam;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txtMesFailCount;
        private System.Windows.Forms.TextBox txtMesTimeout;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMesPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMesAddr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSocketTimeout;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtAAPort;
        private System.Windows.Forms.TextBox txtWhitePort;
        private System.Windows.Forms.TextBox txtCCDPort;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Device.SerialPortParam HeightDetectParam;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Device.SerialPortParam manualScannerParam;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Device.SerialPortParam WhiteBoardPowerParam;
    }
}