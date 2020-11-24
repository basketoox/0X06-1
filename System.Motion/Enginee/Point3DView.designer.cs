namespace Motion.Enginee
{
    partial class Point3DView
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblname3 = new System.Windows.Forms.Label();
            this.lblname1 = new System.Windows.Forms.Label();
            this.lblname2 = new System.Windows.Forms.Label();
            this.lblGetProductZ = new System.Windows.Forms.Label();
            this.lblGetProductX = new System.Windows.Forms.Label();
            this.lblGetProductY = new System.Windows.Forms.Label();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel2.Controls.Add(this.lblname3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblname1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblname2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblGetProductZ, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblGetProductX, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblGetProductY, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(165, 103);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // lblname3
            // 
            this.lblname3.AutoSize = true;
            this.lblname3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblname3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblname3.Location = new System.Drawing.Point(5, 69);
            this.lblname3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblname3.Name = "lblname3";
            this.lblname3.Size = new System.Drawing.Size(32, 33);
            this.lblname3.TabIndex = 4;
            this.lblname3.Text = "Z轴";
            this.lblname3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblname1
            // 
            this.lblname1.AutoSize = true;
            this.lblname1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblname1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblname1.Location = new System.Drawing.Point(5, 1);
            this.lblname1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblname1.Name = "lblname1";
            this.lblname1.Size = new System.Drawing.Size(32, 33);
            this.lblname1.TabIndex = 0;
            this.lblname1.Text = "X轴";
            this.lblname1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblname2
            // 
            this.lblname2.AutoSize = true;
            this.lblname2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblname2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblname2.Location = new System.Drawing.Point(5, 35);
            this.lblname2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblname2.Name = "lblname2";
            this.lblname2.Size = new System.Drawing.Size(32, 33);
            this.lblname2.TabIndex = 1;
            this.lblname2.Text = "Y轴";
            this.lblname2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGetProductZ
            // 
            this.lblGetProductZ.AutoSize = true;
            this.lblGetProductZ.BackColor = System.Drawing.Color.Transparent;
            this.lblGetProductZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGetProductZ.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblGetProductZ.ForeColor = System.Drawing.Color.Black;
            this.lblGetProductZ.Location = new System.Drawing.Point(46, 69);
            this.lblGetProductZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGetProductZ.Name = "lblGetProductZ";
            this.lblGetProductZ.Size = new System.Drawing.Size(114, 33);
            this.lblGetProductZ.TabIndex = 1;
            this.lblGetProductZ.Text = "0000.000";
            this.lblGetProductZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGetProductX
            // 
            this.lblGetProductX.AutoSize = true;
            this.lblGetProductX.BackColor = System.Drawing.Color.Transparent;
            this.lblGetProductX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGetProductX.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblGetProductX.ForeColor = System.Drawing.Color.Black;
            this.lblGetProductX.Location = new System.Drawing.Point(46, 1);
            this.lblGetProductX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGetProductX.Name = "lblGetProductX";
            this.lblGetProductX.Size = new System.Drawing.Size(114, 33);
            this.lblGetProductX.TabIndex = 2;
            this.lblGetProductX.Text = "0000.000";
            this.lblGetProductX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGetProductY
            // 
            this.lblGetProductY.AutoSize = true;
            this.lblGetProductY.BackColor = System.Drawing.Color.Transparent;
            this.lblGetProductY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGetProductY.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblGetProductY.ForeColor = System.Drawing.Color.Black;
            this.lblGetProductY.Location = new System.Drawing.Point(46, 35);
            this.lblGetProductY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGetProductY.Name = "lblGetProductY";
            this.lblGetProductY.Size = new System.Drawing.Size(114, 33);
            this.lblGetProductY.TabIndex = 3;
            this.lblGetProductY.Text = "0000.000";
            this.lblGetProductY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Point3DView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Point3DView";
            this.Size = new System.Drawing.Size(165, 103);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblname1;
        private System.Windows.Forms.Label lblname2;
        private System.Windows.Forms.Label lblGetProductZ;
        private System.Windows.Forms.Label lblGetProductX;
        private System.Windows.Forms.Label lblGetProductY;
        private System.Windows.Forms.Label lblname3;
    }
}
