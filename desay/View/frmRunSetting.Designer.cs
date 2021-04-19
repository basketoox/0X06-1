namespace desay
{
    partial class frmRunSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRunSetting));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkAAMode = new System.Windows.Forms.CheckBox();
            this.chkWeighMode = new System.Windows.Forms.CheckBox();
            this.chkGlueMode = new System.Windows.Forms.CheckBox();
            this.chkWhiteMode = new System.Windows.Forms.CheckBox();
            this.chkDryRun = new System.Windows.Forms.CheckBox();
            this.chkCarrierHaveProduct = new System.Windows.Forms.CheckBox();
            this.chkCleanHaveProduct = new System.Windows.Forms.CheckBox();
            this.chkSnScannerShield = new System.Windows.Forms.CheckBox();
            this.chkAAShield = new System.Windows.Forms.CheckBox();
            this.chkCurtainShield = new System.Windows.Forms.CheckBox();
            this.chkDoorSheild = new System.Windows.Forms.CheckBox();
            this.chkGlueHaveProduct = new System.Windows.Forms.CheckBox();
            this.chkLeaveShield = new System.Windows.Forms.CheckBox();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(41, 227);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(196, 227);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkLeaveShield);
            this.groupBox3.Controls.Add(this.chkAAMode);
            this.groupBox3.Controls.Add(this.chkWeighMode);
            this.groupBox3.Controls.Add(this.chkGlueMode);
            this.groupBox3.Controls.Add(this.chkWhiteMode);
            this.groupBox3.Controls.Add(this.chkDryRun);
            this.groupBox3.Controls.Add(this.chkCarrierHaveProduct);
            this.groupBox3.Controls.Add(this.chkCleanHaveProduct);
            this.groupBox3.Controls.Add(this.chkSnScannerShield);
            this.groupBox3.Controls.Add(this.chkAAShield);
            this.groupBox3.Controls.Add(this.chkCurtainShield);
            this.groupBox3.Controls.Add(this.chkDoorSheild);
            this.groupBox3.Controls.Add(this.chkGlueHaveProduct);
            this.groupBox3.Location = new System.Drawing.Point(6, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(323, 215);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "屏蔽设置";
            // 
            // chkAAMode
            // 
            this.chkAAMode.AutoSize = true;
            this.chkAAMode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkAAMode.Location = new System.Drawing.Point(183, 155);
            this.chkAAMode.Name = "chkAAMode";
            this.chkAAMode.Size = new System.Drawing.Size(97, 20);
            this.chkAAMode.TabIndex = 15;
            this.chkAAMode.Text = " AA 点检";
            this.chkAAMode.UseVisualStyleBackColor = true;
            this.chkAAMode.CheckedChanged += new System.EventHandler(this.chkAAMode_CheckedChanged);
            // 
            // chkWeighMode
            // 
            this.chkWeighMode.AutoSize = true;
            this.chkWeighMode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkWeighMode.Location = new System.Drawing.Point(183, 182);
            this.chkWeighMode.Name = "chkWeighMode";
            this.chkWeighMode.Size = new System.Drawing.Size(95, 20);
            this.chkWeighMode.TabIndex = 14;
            this.chkWeighMode.Text = "胶重点检";
            this.chkWeighMode.UseVisualStyleBackColor = true;
            this.chkWeighMode.CheckedChanged += new System.EventHandler(this.chkWeighMode_CheckedChanged);
            // 
            // chkGlueMode
            // 
            this.chkGlueMode.AutoSize = true;
            this.chkGlueMode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkGlueMode.Location = new System.Drawing.Point(183, 128);
            this.chkGlueMode.Name = "chkGlueMode";
            this.chkGlueMode.Size = new System.Drawing.Size(95, 20);
            this.chkGlueMode.TabIndex = 13;
            this.chkGlueMode.Text = "点胶点检";
            this.chkGlueMode.UseVisualStyleBackColor = true;
            this.chkGlueMode.CheckedChanged += new System.EventHandler(this.chkGlueMode_CheckedChanged);
            // 
            // chkWhiteMode
            // 
            this.chkWhiteMode.AutoSize = true;
            this.chkWhiteMode.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkWhiteMode.Location = new System.Drawing.Point(183, 101);
            this.chkWhiteMode.Name = "chkWhiteMode";
            this.chkWhiteMode.Size = new System.Drawing.Size(95, 20);
            this.chkWhiteMode.TabIndex = 12;
            this.chkWhiteMode.Text = "白板点检";
            this.chkWhiteMode.UseVisualStyleBackColor = true;
            this.chkWhiteMode.CheckedChanged += new System.EventHandler(this.chkWhiteMode_CheckedChanged);
            // 
            // chkDryRun
            // 
            this.chkDryRun.AutoSize = true;
            this.chkDryRun.Font = new System.Drawing.Font("宋体", 12F);
            this.chkDryRun.Location = new System.Drawing.Point(183, 47);
            this.chkDryRun.Name = "chkDryRun";
            this.chkDryRun.Size = new System.Drawing.Size(91, 20);
            this.chkDryRun.TabIndex = 11;
            this.chkDryRun.Text = "空跑模式";
            this.chkDryRun.UseVisualStyleBackColor = true;
            // 
            // chkCarrierHaveProduct
            // 
            this.chkCarrierHaveProduct.AutoSize = true;
            this.chkCarrierHaveProduct.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkCarrierHaveProduct.Location = new System.Drawing.Point(19, 20);
            this.chkCarrierHaveProduct.Name = "chkCarrierHaveProduct";
            this.chkCarrierHaveProduct.Size = new System.Drawing.Size(107, 20);
            this.chkCarrierHaveProduct.TabIndex = 10;
            this.chkCarrierHaveProduct.Text = "接驳台有料";
            this.chkCarrierHaveProduct.UseVisualStyleBackColor = true;
            // 
            // chkCleanHaveProduct
            // 
            this.chkCleanHaveProduct.AutoSize = true;
            this.chkCleanHaveProduct.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkCleanHaveProduct.Location = new System.Drawing.Point(19, 74);
            this.chkCleanHaveProduct.Name = "chkCleanHaveProduct";
            this.chkCleanHaveProduct.Size = new System.Drawing.Size(123, 20);
            this.chkCleanHaveProduct.TabIndex = 9;
            this.chkCleanHaveProduct.Text = "清洗平台有料";
            this.chkCleanHaveProduct.UseVisualStyleBackColor = true;
            // 
            // chkSnScannerShield
            // 
            this.chkSnScannerShield.AutoSize = true;
            this.chkSnScannerShield.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkSnScannerShield.Location = new System.Drawing.Point(19, 182);
            this.chkSnScannerShield.Name = "chkSnScannerShield";
            this.chkSnScannerShield.Size = new System.Drawing.Size(123, 20);
            this.chkSnScannerShield.TabIndex = 10;
            this.chkSnScannerShield.Text = "屏蔽产品扫码";
            this.chkSnScannerShield.UseVisualStyleBackColor = true;
            // 
            // chkAAShield
            // 
            this.chkAAShield.AutoSize = true;
            this.chkAAShield.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkAAShield.Location = new System.Drawing.Point(19, 128);
            this.chkAAShield.Name = "chkAAShield";
            this.chkAAShield.Size = new System.Drawing.Size(123, 20);
            this.chkAAShield.TabIndex = 10;
            this.chkAAShield.Text = "屏蔽 AA 软件";
            this.chkAAShield.UseVisualStyleBackColor = true;
            // 
            // chkCurtainShield
            // 
            this.chkCurtainShield.AutoSize = true;
            this.chkCurtainShield.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkCurtainShield.Location = new System.Drawing.Point(19, 155);
            this.chkCurtainShield.Name = "chkCurtainShield";
            this.chkCurtainShield.Size = new System.Drawing.Size(123, 20);
            this.chkCurtainShield.TabIndex = 10;
            this.chkCurtainShield.Text = "屏蔽安全光幕";
            this.chkCurtainShield.UseVisualStyleBackColor = true;
            // 
            // chkDoorSheild
            // 
            this.chkDoorSheild.AutoSize = true;
            this.chkDoorSheild.Checked = true;
            this.chkDoorSheild.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoorSheild.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkDoorSheild.Location = new System.Drawing.Point(19, 101);
            this.chkDoorSheild.Name = "chkDoorSheild";
            this.chkDoorSheild.Size = new System.Drawing.Size(91, 20);
            this.chkDoorSheild.TabIndex = 10;
            this.chkDoorSheild.Text = "屏蔽门禁";
            this.chkDoorSheild.UseVisualStyleBackColor = true;
            // 
            // chkGlueHaveProduct
            // 
            this.chkGlueHaveProduct.AutoSize = true;
            this.chkGlueHaveProduct.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkGlueHaveProduct.Location = new System.Drawing.Point(19, 47);
            this.chkGlueHaveProduct.Name = "chkGlueHaveProduct";
            this.chkGlueHaveProduct.Size = new System.Drawing.Size(123, 20);
            this.chkGlueHaveProduct.TabIndex = 10;
            this.chkGlueHaveProduct.Text = "点胶平台有料";
            this.chkGlueHaveProduct.UseVisualStyleBackColor = true;
            // 
            // chkLeaveShield
            // 
            this.chkLeaveShield.AutoSize = true;
            this.chkLeaveShield.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkLeaveShield.Location = new System.Drawing.Point(183, 24);
            this.chkLeaveShield.Name = "chkLeaveShield";
            this.chkLeaveShield.Size = new System.Drawing.Size(123, 20);
            this.chkLeaveShield.TabIndex = 16;
            this.chkLeaveShield.Text = "屏蔽自动排胶";
            this.chkLeaveShield.UseVisualStyleBackColor = true;
            // 
            // frmRunSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 259);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRunSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "运行设置";
            this.Load += new System.EventHandler(this.frmRunSetting_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkCleanHaveProduct;
        private System.Windows.Forms.CheckBox chkGlueHaveProduct;
        private System.Windows.Forms.CheckBox chkCarrierHaveProduct;
        private System.Windows.Forms.CheckBox chkDoorSheild;
        private System.Windows.Forms.CheckBox chkCurtainShield;
        private System.Windows.Forms.CheckBox chkAAShield;
        private System.Windows.Forms.CheckBox chkSnScannerShield;
        private System.Windows.Forms.CheckBox chkDryRun;
        private System.Windows.Forms.CheckBox chkAAMode;
        private System.Windows.Forms.CheckBox chkWeighMode;
        private System.Windows.Forms.CheckBox chkGlueMode;
        private System.Windows.Forms.CheckBox chkWhiteMode;
        private System.Windows.Forms.CheckBox chkLeaveShield;
    }
}