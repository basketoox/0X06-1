namespace desay
{
    partial class frmMES
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
            this.IPAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ServiceAddress = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.pOrderCode = new System.Windows.Forms.TextBox();
            this.pOrderType = new System.Windows.Forms.TextBox();
            this.pchTestIdK = new System.Windows.Forms.TextBox();
            this.pchItemNameK = new System.Windows.Forms.TextBox();
            this.pchOperatorIDK = new System.Windows.Forms.TextBox();
            this.pchStationIDK = new System.Windows.Forms.TextBox();
            this.pchModelK = new System.Windows.Forms.TextBox();
            this.programCode = new System.Windows.Forms.TextBox();
            this.pchErrcdk = new System.Windows.Forms.TextBox();
            this.pchPfmdataK = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnGetInfo = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // IPAddress
            // 
            this.IPAddress.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IPAddress.Location = new System.Drawing.Point(16, 115);
            this.IPAddress.Name = "IPAddress";
            this.IPAddress.ReadOnly = true;
            this.IPAddress.Size = new System.Drawing.Size(166, 26);
            this.IPAddress.TabIndex = 0;
            this.IPAddress.TabStop = false;
            this.IPAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "本机IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "工单";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 222);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "测试关卡";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(33, 283);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "人员工号";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "机台名称";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 222);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "机台型号";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 161);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "条码";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(32, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "单别";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 344);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 9;
            this.label9.Text = "结果代码";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 283);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 10;
            this.label10.Text = "程序代码";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(216, 100);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 11;
            this.label11.Text = "测试数据";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(19, 39);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 12);
            this.label12.TabIndex = 12;
            this.label12.Text = "服务器地址：";
            // 
            // ServiceAddress
            // 
            this.ServiceAddress.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ServiceAddress.Location = new System.Drawing.Point(16, 54);
            this.ServiceAddress.Name = "ServiceAddress";
            this.ServiceAddress.Size = new System.Drawing.Size(432, 26);
            this.ServiceAddress.TabIndex = 13;
            this.ServiceAddress.Text = "http://172.16.1.235:8889/Mes_WebServiceMain.asmx";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(467, 52);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(102, 29);
            this.btnTest.TabIndex = 14;
            this.btnTest.Text = "通讯测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // pOrderCode
            // 
            this.pOrderCode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pOrderCode.Location = new System.Drawing.Point(29, 54);
            this.pOrderCode.Name = "pOrderCode";
            this.pOrderCode.Size = new System.Drawing.Size(205, 26);
            this.pOrderCode.TabIndex = 15;
            this.pOrderCode.Text = "110155445";
            this.pOrderCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pOrderType
            // 
            this.pOrderType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pOrderType.Location = new System.Drawing.Point(29, 115);
            this.pOrderType.Name = "pOrderType";
            this.pOrderType.Size = new System.Drawing.Size(205, 26);
            this.pOrderType.TabIndex = 16;
            this.pOrderType.Text = "S100";
            this.pOrderType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pchTestIdK
            // 
            this.pchTestIdK.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pchTestIdK.Location = new System.Drawing.Point(29, 176);
            this.pchTestIdK.Name = "pchTestIdK";
            this.pchTestIdK.Size = new System.Drawing.Size(205, 26);
            this.pchTestIdK.TabIndex = 17;
            this.pchTestIdK.Text = "09FX29";
            this.pchTestIdK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pchItemNameK
            // 
            this.pchItemNameK.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pchItemNameK.Location = new System.Drawing.Point(29, 237);
            this.pchItemNameK.Name = "pchItemNameK";
            this.pchItemNameK.Size = new System.Drawing.Size(205, 26);
            this.pchItemNameK.TabIndex = 18;
            this.pchItemNameK.Text = "SM-INPUT(T)";
            this.pchItemNameK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pchOperatorIDK
            // 
            this.pchOperatorIDK.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pchOperatorIDK.Location = new System.Drawing.Point(29, 298);
            this.pchOperatorIDK.Name = "pchOperatorIDK";
            this.pchOperatorIDK.Size = new System.Drawing.Size(205, 26);
            this.pchOperatorIDK.TabIndex = 19;
            this.pchOperatorIDK.Text = "S0873";
            this.pchOperatorIDK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pchStationIDK
            // 
            this.pchStationIDK.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pchStationIDK.Location = new System.Drawing.Point(16, 176);
            this.pchStationIDK.Name = "pchStationIDK";
            this.pchStationIDK.Size = new System.Drawing.Size(166, 26);
            this.pchStationIDK.TabIndex = 20;
            this.pchStationIDK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pchModelK
            // 
            this.pchModelK.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pchModelK.Location = new System.Drawing.Point(16, 237);
            this.pchModelK.Name = "pchModelK";
            this.pchModelK.Size = new System.Drawing.Size(166, 26);
            this.pchModelK.TabIndex = 21;
            this.pchModelK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // programCode
            // 
            this.programCode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.programCode.Location = new System.Drawing.Point(16, 298);
            this.programCode.Name = "programCode";
            this.programCode.Size = new System.Drawing.Size(166, 26);
            this.programCode.TabIndex = 22;
            this.programCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pchErrcdk
            // 
            this.pchErrcdk.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pchErrcdk.Location = new System.Drawing.Point(16, 359);
            this.pchErrcdk.Name = "pchErrcdk";
            this.pchErrcdk.ReadOnly = true;
            this.pchErrcdk.Size = new System.Drawing.Size(119, 26);
            this.pchErrcdk.TabIndex = 23;
            this.pchErrcdk.Text = "00";
            this.pchErrcdk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pchPfmdataK
            // 
            this.pchPfmdataK.Location = new System.Drawing.Point(218, 115);
            this.pchPfmdataK.Multiline = true;
            this.pchPfmdataK.Name = "pchPfmdataK";
            this.pchPfmdataK.ReadOnly = true;
            this.pchPfmdataK.Size = new System.Drawing.Size(351, 270);
            this.pchPfmdataK.TabIndex = 24;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGetInfo);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.pchOperatorIDK);
            this.groupBox1.Controls.Add(this.pchItemNameK);
            this.groupBox1.Controls.Add(this.pchTestIdK);
            this.groupBox1.Controls.Add(this.pOrderType);
            this.groupBox1.Controls.Add(this.pOrderCode);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 410);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "生产信息";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSend);
            this.groupBox2.Controls.Add(this.pchPfmdataK);
            this.groupBox2.Controls.Add(this.programCode);
            this.groupBox2.Controls.Add(this.pchErrcdk);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.pchModelK);
            this.groupBox2.Controls.Add(this.pchStationIDK);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.btnTest);
            this.groupBox2.Controls.Add(this.ServiceAddress);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.IPAddress);
            this.groupBox2.Location = new System.Drawing.Point(289, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(595, 410);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "机台信息";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(135, 356);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 29);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "保存参数";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGetInfo
            // 
            this.btnGetInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetInfo.Location = new System.Drawing.Point(29, 356);
            this.btnGetInfo.Name = "btnGetInfo";
            this.btnGetInfo.Size = new System.Drawing.Size(100, 29);
            this.btnGetInfo.TabIndex = 20;
            this.btnGetInfo.Text = "解析工单";
            this.btnGetInfo.UseVisualStyleBackColor = true;
            this.btnGetInfo.Click += new System.EventHandler(this.btnGetInfo_Click);
            // 
            // btnSend
            // 
            this.btnSend.BackgroundImage = global::desay.Properties.Resources.ic_action_reload1;
            this.btnSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSend.Location = new System.Drawing.Point(157, 352);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(41, 40);
            this.btnSend.TabIndex = 25;
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // frmMES
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 443);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMES";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MES";
            this.Load += new System.EventHandler(this.frmMES_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox IPAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox ServiceAddress;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox pOrderCode;
        private System.Windows.Forms.TextBox pOrderType;
        private System.Windows.Forms.TextBox pchTestIdK;
        private System.Windows.Forms.TextBox pchItemNameK;
        private System.Windows.Forms.TextBox pchOperatorIDK;
        private System.Windows.Forms.TextBox pchStationIDK;
        private System.Windows.Forms.TextBox pchModelK;
        private System.Windows.Forms.TextBox programCode;
        private System.Windows.Forms.TextBox pchErrcdk;
        private System.Windows.Forms.TextBox pchPfmdataK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnGetInfo;
        private System.Windows.Forms.Button btnSend;
    }
}