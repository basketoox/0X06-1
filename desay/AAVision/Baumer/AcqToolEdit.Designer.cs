namespace desay
{
    partial class AcqToolEdit
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Tab_setting = new System.Windows.Forms.TabControl();
            this.tab_set = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1_CamsList = new System.Windows.Forms.Label();
            this.CB_CamsList = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.Tab_setting.SuspendLayout();
            this.tab_set.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 154F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(131, 312);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Tab_setting);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(127, 305);
            this.panel1.TabIndex = 0;
            // 
            // Tab_setting
            // 
            this.Tab_setting.Controls.Add(this.tab_set);
            this.Tab_setting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tab_setting.ItemSize = new System.Drawing.Size(60, 18);
            this.Tab_setting.Location = new System.Drawing.Point(0, 0);
            this.Tab_setting.Multiline = true;
            this.Tab_setting.Name = "Tab_setting";
            this.Tab_setting.SelectedIndex = 0;
            this.Tab_setting.Size = new System.Drawing.Size(127, 305);
            this.Tab_setting.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.Tab_setting.TabIndex = 0;
            this.Tab_setting.Tag = "";
            // 
            // tab_set
            // 
            this.tab_set.Controls.Add(this.checkBox1);
            this.tab_set.Controls.Add(this.label1);
            this.tab_set.Controls.Add(this.button2);
            this.tab_set.Controls.Add(this.button1);
            this.tab_set.Controls.Add(this.label1_CamsList);
            this.tab_set.Controls.Add(this.CB_CamsList);
            this.tab_set.Location = new System.Drawing.Point(4, 22);
            this.tab_set.Name = "tab_set";
            this.tab_set.Padding = new System.Windows.Forms.Padding(3);
            this.tab_set.Size = new System.Drawing.Size(119, 279);
            this.tab_set.TabIndex = 0;
            this.tab_set.Text = "Setting";
            this.tab_set.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(87, 206);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 206);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "保存图片：";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(18, 130);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "关闭相机";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "打开相机";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1_CamsList
            // 
            this.label1_CamsList.AutoSize = true;
            this.label1_CamsList.Location = new System.Drawing.Point(6, 15);
            this.label1_CamsList.Name = "label1_CamsList";
            this.label1_CamsList.Size = new System.Drawing.Size(65, 12);
            this.label1_CamsList.TabIndex = 4;
            this.label1_CamsList.Text = "相机列表：";
            // 
            // CB_CamsList
            // 
            this.CB_CamsList.FormattingEnabled = true;
            this.CB_CamsList.Location = new System.Drawing.Point(6, 30);
            this.CB_CamsList.Name = "CB_CamsList";
            this.CB_CamsList.Size = new System.Drawing.Size(104, 20);
            this.CB_CamsList.TabIndex = 0;
            this.CB_CamsList.SelectedIndexChanged += new System.EventHandler(this.CB_CamsList_SelectedIndexChanged);
            // 
            // AcqToolEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AcqToolEdit";
            this.Size = new System.Drawing.Size(131, 312);
            this.Load += new System.EventHandler(this.AcqToolEdit_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.Tab_setting.ResumeLayout(false);
            this.tab_set.ResumeLayout(false);
            this.tab_set.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl Tab_setting;
        private System.Windows.Forms.TabPage tab_set;
        private System.Windows.Forms.Label label1_CamsList;
        private System.Windows.Forms.ComboBox CB_CamsList;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}
