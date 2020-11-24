namespace Motion.Enginee
{
    partial class PointView
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
            this.lblName1 = new System.Windows.Forms.Label();
            this.lblName2 = new System.Windows.Forms.Label();
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
            this.tableLayoutPanel2.Controls.Add(this.lblName1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblName2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblGetProductX, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblGetProductY, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(165, 54);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // lblName1
            // 
            this.lblName1.AutoSize = true;
            this.lblName1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblName1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName1.Location = new System.Drawing.Point(5, 1);
            this.lblName1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName1.Name = "lblName1";
            this.lblName1.Size = new System.Drawing.Size(32, 25);
            this.lblName1.TabIndex = 0;
            this.lblName1.Text = "X轴";
            this.lblName1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblName2
            // 
            this.lblName2.AutoSize = true;
            this.lblName2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblName2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName2.Location = new System.Drawing.Point(5, 27);
            this.lblName2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName2.Name = "lblName2";
            this.lblName2.Size = new System.Drawing.Size(32, 26);
            this.lblName2.TabIndex = 1;
            this.lblName2.Text = "Y轴";
            this.lblName2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.lblGetProductX.Size = new System.Drawing.Size(114, 25);
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
            this.lblGetProductY.Location = new System.Drawing.Point(46, 27);
            this.lblGetProductY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGetProductY.Name = "lblGetProductY";
            this.lblGetProductY.Size = new System.Drawing.Size(114, 26);
            this.lblGetProductY.TabIndex = 3;
            this.lblGetProductY.Text = "0000.000";
            this.lblGetProductY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PointView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PointView";
            this.Size = new System.Drawing.Size(165, 54);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblName1;
        private System.Windows.Forms.Label lblName2;
        private System.Windows.Forms.Label lblGetProductX;
        private System.Windows.Forms.Label lblGetProductY;
    }
}
