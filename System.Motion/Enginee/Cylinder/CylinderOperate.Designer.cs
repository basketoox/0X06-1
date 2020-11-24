namespace Motion.Enginee
{
    partial class CylinderOperate
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
            this.btn = new System.Windows.Forms.Button();
            this.picMove = new System.Windows.Forms.PictureBox();
            this.picOrigin = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picMove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOrigin)).BeginInit();
            this.SuspendLayout();
            // 
            // btn
            // 
            this.btn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn.Location = new System.Drawing.Point(0, 0);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(121, 35);
            this.btn.TabIndex = 3;
            this.btn.UseVisualStyleBackColor = false;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // picMove
            // 
            this.picMove.BackColor = System.Drawing.Color.Red;
            this.picMove.Dock = System.Windows.Forms.DockStyle.Right;
            this.picMove.Location = new System.Drawing.Point(111, 0);
            this.picMove.Name = "picMove";
            this.picMove.Size = new System.Drawing.Size(10, 35);
            this.picMove.TabIndex = 5;
            this.picMove.TabStop = false;
            // 
            // picOrigin
            // 
            this.picOrigin.BackColor = System.Drawing.Color.Red;
            this.picOrigin.Dock = System.Windows.Forms.DockStyle.Left;
            this.picOrigin.Location = new System.Drawing.Point(0, 0);
            this.picOrigin.Name = "picOrigin";
            this.picOrigin.Size = new System.Drawing.Size(10, 35);
            this.picOrigin.TabIndex = 4;
            this.picOrigin.TabStop = false;
            // 
            // CylinderOperate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picMove);
            this.Controls.Add(this.picOrigin);
            this.Controls.Add(this.btn);
            this.Name = "CylinderOperate";
            this.Size = new System.Drawing.Size(121, 35);
            ((System.ComponentModel.ISupportInitialize)(this.picMove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOrigin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picMove;
        private System.Windows.Forms.PictureBox picOrigin;
        private System.Windows.Forms.Button btn;
    }
}
