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
            this.chkCarrierHaveProduct = new System.Windows.Forms.CheckBox();
            this.chkCleanHaveProduct = new System.Windows.Forms.CheckBox();
            this.chkSnScannerShield = new System.Windows.Forms.CheckBox();
            this.chkAAShield = new System.Windows.Forms.CheckBox();
            this.chkCurtainShield = new System.Windows.Forms.CheckBox();
            this.chkDoorSheild = new System.Windows.Forms.CheckBox();
            this.chkGlueHaveProduct = new System.Windows.Forms.CheckBox();
            this.chkDryRun = new System.Windows.Forms.CheckBox();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(32, 202);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(188, 202);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox3
            // 
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
            this.groupBox3.Size = new System.Drawing.Size(323, 170);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "屏蔽设置";
            // 
            // chkCarrierHaveProduct
            // 
            this.chkCarrierHaveProduct.AutoSize = true;
            this.chkCarrierHaveProduct.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkCarrierHaveProduct.Location = new System.Drawing.Point(12, 20);
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
            this.chkCleanHaveProduct.Location = new System.Drawing.Point(164, 20);
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
            this.chkSnScannerShield.Location = new System.Drawing.Point(12, 136);
            this.chkSnScannerShield.Name = "chkSnScannerShield";
            this.chkSnScannerShield.Size = new System.Drawing.Size(139, 20);
            this.chkSnScannerShield.TabIndex = 10;
            this.chkSnScannerShield.Text = "产品扫码枪屏蔽";
            this.chkSnScannerShield.UseVisualStyleBackColor = true;
            // 
            // chkAAShield
            // 
            this.chkAAShield.AutoSize = true;
            this.chkAAShield.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkAAShield.Location = new System.Drawing.Point(164, 100);
            this.chkAAShield.Name = "chkAAShield";
            this.chkAAShield.Size = new System.Drawing.Size(107, 20);
            this.chkAAShield.TabIndex = 10;
            this.chkAAShield.Text = "AA软件屏蔽";
            this.chkAAShield.UseVisualStyleBackColor = true;
            // 
            // chkCurtainShield
            // 
            this.chkCurtainShield.AutoSize = true;
            this.chkCurtainShield.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkCurtainShield.Location = new System.Drawing.Point(12, 100);
            this.chkCurtainShield.Name = "chkCurtainShield";
            this.chkCurtainShield.Size = new System.Drawing.Size(123, 20);
            this.chkCurtainShield.TabIndex = 10;
            this.chkCurtainShield.Text = "安全光幕屏蔽";
            this.chkCurtainShield.UseVisualStyleBackColor = true;
            // 
            // chkDoorSheild
            // 
            this.chkDoorSheild.AutoSize = true;
            this.chkDoorSheild.Checked = true;
            this.chkDoorSheild.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoorSheild.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkDoorSheild.Location = new System.Drawing.Point(164, 60);
            this.chkDoorSheild.Name = "chkDoorSheild";
            this.chkDoorSheild.Size = new System.Drawing.Size(91, 20);
            this.chkDoorSheild.TabIndex = 10;
            this.chkDoorSheild.Text = "门禁屏蔽";
            this.chkDoorSheild.UseVisualStyleBackColor = true;
            // 
            // chkGlueHaveProduct
            // 
            this.chkGlueHaveProduct.AutoSize = true;
            this.chkGlueHaveProduct.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkGlueHaveProduct.Location = new System.Drawing.Point(12, 60);
            this.chkGlueHaveProduct.Name = "chkGlueHaveProduct";
            this.chkGlueHaveProduct.Size = new System.Drawing.Size(123, 20);
            this.chkGlueHaveProduct.TabIndex = 10;
            this.chkGlueHaveProduct.Text = "点胶平台有料";
            this.chkGlueHaveProduct.UseVisualStyleBackColor = true;
            // 
            // chkDryRun
            // 
            this.chkDryRun.AutoSize = true;
            this.chkDryRun.Font = new System.Drawing.Font("宋体", 12F);
            this.chkDryRun.Location = new System.Drawing.Point(164, 140);
            this.chkDryRun.Name = "chkDryRun";
            this.chkDryRun.Size = new System.Drawing.Size(91, 20);
            this.chkDryRun.TabIndex = 11;
            this.chkDryRun.Text = "空跑模式";
            this.chkDryRun.UseVisualStyleBackColor = true;
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
    }
}