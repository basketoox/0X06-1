using desay.Properties;

namespace desay
{
    partial class frmWhiteBorad
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
            bCloseCamera();
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblStain = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.lblCapImage = new System.Windows.Forms.Label();
            this.lblBad = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.button12 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnTakePicture = new System.Windows.Forms.Button();
            this.lblDisplay_fps = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblFram_fps = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblFram_ERROR = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFram_OK = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPlay = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.DevCombox = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ckbShowStain = new System.Windows.Forms.CheckBox();
            this.ckbShowDefect = new System.Windows.Forms.CheckBox();
            this.ckbSavePhotho = new System.Windows.Forms.CheckBox();
            this.button10 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txbBADPIXEL_SPEC = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txbBADPIXEL_THRESHOLD = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txbBADPIXEL_SORTNUM = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txbHOTPIXEL_SPEC = new System.Windows.Forms.TextBox();
            this.txbHOTPIXEL_SORTNUM = new System.Windows.Forms.TextBox();
            this.txbHOTPIXEL_THRESHOLD = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txbROISIZE = new System.Windows.Forms.TextBox();
            this.txbSPEC_NumNODEEP = new System.Windows.Forms.TextBox();
            this.txbSIZENODEEP = new System.Windows.Forms.TextBox();
            this.txbLUX_NoDEEPBLEMISH = new System.Windows.Forms.TextBox();
            this.txbSPEC_NumDEEP = new System.Windows.Forms.TextBox();
            this.txbSIZEDEEP = new System.Windows.Forms.TextBox();
            this.txbLUX_DEEPBLEMISH = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txbCIRCLE_R = new System.Windows.Forms.TextBox();
            this.txbBinaryThreshold = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.comboxPin_14 = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.label29 = new System.Windows.Forms.Label();
            this.comboxPin_13 = new System.Windows.Forms.ComboBox();
            this.comboxPin_26 = new System.Windows.Forms.ComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.comboxPin_12 = new System.Windows.Forms.ComboBox();
            this.comboxPin_25 = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.comboxPin_11 = new System.Windows.Forms.ComboBox();
            this.comboxPin_24 = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.comboxPin_10 = new System.Windows.Forms.ComboBox();
            this.comboxPin_23 = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.comboxPin_09 = new System.Windows.Forms.ComboBox();
            this.comboxPin_22 = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.comboxPin_08 = new System.Windows.Forms.ComboBox();
            this.comboxPin_21 = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.comboxPin_07 = new System.Windows.Forms.ComboBox();
            this.comboxPin_20 = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.comboxPin_06 = new System.Windows.Forms.ComboBox();
            this.comboxPin_19 = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.comboxPin_05 = new System.Windows.Forms.ComboBox();
            this.comboxPin_18 = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.comboxPin_04 = new System.Windows.Forms.ComboBox();
            this.comboxPin_17 = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.comboxPin_03 = new System.Windows.Forms.ComboBox();
            this.comboxPin_16 = new System.Windows.Forms.ComboBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.comboxPin_02 = new System.Windows.Forms.ComboBox();
            this.comboxPin_15 = new System.Windows.Forms.ComboBox();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.comboxPin_01 = new System.Windows.Forms.ComboBox();
            this.label45 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(796, 465);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblStain);
            this.tabPage1.Controls.Add(this.label49);
            this.tabPage1.Controls.Add(this.lblCapImage);
            this.tabPage1.Controls.Add(this.lblBad);
            this.tabPage1.Controls.Add(this.label46);
            this.tabPage1.Controls.Add(this.label47);
            this.tabPage1.Controls.Add(this.button12);
            this.tabPage1.Controls.Add(this.button11);
            this.tabPage1.Controls.Add(this.button8);
            this.tabPage1.Controls.Add(this.button9);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.btnStop);
            this.tabPage1.Controls.Add(this.btnTakePicture);
            this.tabPage1.Controls.Add(this.lblDisplay_fps);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.lblFram_fps);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.lblFram_ERROR);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.lblFram_OK);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btnPlay);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.DevCombox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(788, 439);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "控制";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblStain
            // 
            this.lblStain.AutoSize = true;
            this.lblStain.Location = new System.Drawing.Point(84, 394);
            this.lblStain.Name = "lblStain";
            this.lblStain.Size = new System.Drawing.Size(11, 12);
            this.lblStain.TabIndex = 36;
            this.lblStain.Text = "0";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(11, 394);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(59, 12);
            this.label49.TabIndex = 37;
            this.label49.Text = "污点检测:";
            // 
            // lblCapImage
            // 
            this.lblCapImage.AutoSize = true;
            this.lblCapImage.Location = new System.Drawing.Point(84, 334);
            this.lblCapImage.Name = "lblCapImage";
            this.lblCapImage.Size = new System.Drawing.Size(29, 12);
            this.lblCapImage.TabIndex = 34;
            this.lblCapImage.Text = "正常";
            // 
            // lblBad
            // 
            this.lblBad.AutoSize = true;
            this.lblBad.Location = new System.Drawing.Point(84, 364);
            this.lblBad.Name = "lblBad";
            this.lblBad.Size = new System.Drawing.Size(11, 12);
            this.lblBad.TabIndex = 34;
            this.lblBad.Text = "0";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(11, 334);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(59, 12);
            this.label46.TabIndex = 35;
            this.label46.Text = "采集图像:";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(11, 364);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(59, 12);
            this.label47.TabIndex = 35;
            this.label47.Text = "坏点检测:";
            // 
            // button12
            // 
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button12.Location = new System.Drawing.Point(76, 102);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(64, 64);
            this.button12.TabIndex = 33;
            this.button12.Text = "单次测试";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button11
            // 
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Location = new System.Drawing.Point(76, 172);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(64, 64);
            this.button11.TabIndex = 32;
            this.button11.Text = "抓取并污点检测";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button8
            // 
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Location = new System.Drawing.Point(76, 242);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(64, 64);
            this.button8.TabIndex = 32;
            this.button8.Text = "打开图片并污点检测";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Location = new System.Drawing.Point(6, 242);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(64, 64);
            this.button9.TabIndex = 32;
            this.button9.Text = "打开图片并坏点检测";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(6, 172);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(64, 64);
            this.button3.TabIndex = 32;
            this.button3.Text = "抓取并坏点检测";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnStop
            // 
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Image = global::desay.Properties.Resources.EndTest_64px_164_easyicon_net;
            this.btnStop.Location = new System.Drawing.Point(76, 32);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(64, 64);
            this.btnStop.TabIndex = 30;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnTakePicture
            // 
            this.btnTakePicture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTakePicture.Image = global::desay.Properties.Resources.MB_photo_64px_559754_easyicon_net;
            this.btnTakePicture.Location = new System.Drawing.Point(6, 102);
            this.btnTakePicture.Name = "btnTakePicture";
            this.btnTakePicture.Size = new System.Drawing.Size(64, 64);
            this.btnTakePicture.TabIndex = 27;
            this.btnTakePicture.UseVisualStyleBackColor = true;
            this.btnTakePicture.Click += new System.EventHandler(this.btnTakePicture_Click);
            // 
            // lblDisplay_fps
            // 
            this.lblDisplay_fps.AutoSize = true;
            this.lblDisplay_fps.Location = new System.Drawing.Point(735, 421);
            this.lblDisplay_fps.Name = "lblDisplay_fps";
            this.lblDisplay_fps.Size = new System.Drawing.Size(11, 12);
            this.lblDisplay_fps.TabIndex = 18;
            this.lblDisplay_fps.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(620, 421);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "Display_fps:";
            // 
            // lblFram_fps
            // 
            this.lblFram_fps.AutoSize = true;
            this.lblFram_fps.Location = new System.Drawing.Point(573, 421);
            this.lblFram_fps.Name = "lblFram_fps";
            this.lblFram_fps.Size = new System.Drawing.Size(11, 12);
            this.lblFram_fps.TabIndex = 20;
            this.lblFram_fps.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(464, 421);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "Fram_fps:";
            // 
            // lblFram_ERROR
            // 
            this.lblFram_ERROR.AutoSize = true;
            this.lblFram_ERROR.Location = new System.Drawing.Point(396, 421);
            this.lblFram_ERROR.Name = "lblFram_ERROR";
            this.lblFram_ERROR.Size = new System.Drawing.Size(11, 12);
            this.lblFram_ERROR.TabIndex = 22;
            this.lblFram_ERROR.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(296, 421);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 23;
            this.label2.Text = "Fram_ERROR:";
            // 
            // lblFram_OK
            // 
            this.lblFram_OK.AutoSize = true;
            this.lblFram_OK.Location = new System.Drawing.Point(219, 421);
            this.lblFram_OK.Name = "lblFram_OK";
            this.lblFram_OK.Size = new System.Drawing.Size(11, 12);
            this.lblFram_OK.TabIndex = 24;
            this.lblFram_OK.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(146, 421);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 25;
            this.label1.Text = "Fram_OK:";
            // 
            // btnPlay
            // 
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Image = global::desay.Properties.Resources.MB_WMP_64px_559727_easyicon_net;
            this.btnPlay.Location = new System.Drawing.Point(6, 32);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(64, 64);
            this.btnPlay.TabIndex = 17;
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(142, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(640, 400);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // DevCombox
            // 
            this.DevCombox.BackColor = System.Drawing.SystemColors.Window;
            this.DevCombox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DevCombox.FormattingEnabled = true;
            this.DevCombox.Location = new System.Drawing.Point(5, 6);
            this.DevCombox.Name = "DevCombox";
            this.DevCombox.Size = new System.Drawing.Size(85, 20);
            this.DevCombox.TabIndex = 15;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ckbShowStain);
            this.tabPage2.Controls.Add(this.ckbShowDefect);
            this.tabPage2.Controls.Add(this.ckbSavePhotho);
            this.tabPage2.Controls.Add(this.button10);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.label15);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.label19);
            this.tabPage2.Controls.Add(this.txbCIRCLE_R);
            this.tabPage2.Controls.Add(this.txbBinaryThreshold);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(788, 439);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "参数设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ckbShowStain
            // 
            this.ckbShowStain.AutoSize = true;
            this.ckbShowStain.Location = new System.Drawing.Point(6, 411);
            this.ckbShowStain.Name = "ckbShowStain";
            this.ckbShowStain.Size = new System.Drawing.Size(72, 16);
            this.ckbShowStain.TabIndex = 64;
            this.ckbShowStain.Text = "显示污点";
            this.ckbShowStain.UseVisualStyleBackColor = true;
            // 
            // ckbShowDefect
            // 
            this.ckbShowDefect.AutoSize = true;
            this.ckbShowDefect.Location = new System.Drawing.Point(6, 386);
            this.ckbShowDefect.Name = "ckbShowDefect";
            this.ckbShowDefect.Size = new System.Drawing.Size(72, 16);
            this.ckbShowDefect.TabIndex = 63;
            this.ckbShowDefect.Text = "显示坏点";
            this.ckbShowDefect.UseVisualStyleBackColor = true;
            // 
            // ckbSavePhotho
            // 
            this.ckbSavePhotho.AutoSize = true;
            this.ckbSavePhotho.Location = new System.Drawing.Point(6, 364);
            this.ckbSavePhotho.Name = "ckbSavePhotho";
            this.ckbSavePhotho.Size = new System.Drawing.Size(96, 16);
            this.ckbSavePhotho.TabIndex = 62;
            this.ckbSavePhotho.Text = "保存检测图片";
            this.ckbSavePhotho.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(602, 386);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 32);
            this.button10.TabIndex = 61;
            this.button10.Text = "确定";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(447, 202);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "坏点参数";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txbBADPIXEL_SPEC);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txbBADPIXEL_THRESHOLD);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txbBADPIXEL_SORTNUM);
            this.groupBox3.Location = new System.Drawing.Point(223, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(210, 160);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Bad Pixel";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "Spec Num：";
            // 
            // txbBADPIXEL_SPEC
            // 
            this.txbBADPIXEL_SPEC.Location = new System.Drawing.Point(94, 112);
            this.txbBADPIXEL_SPEC.Name = "txbBADPIXEL_SPEC";
            this.txbBADPIXEL_SPEC.Size = new System.Drawing.Size(100, 21);
            this.txbBADPIXEL_SPEC.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "连续个数：";
            // 
            // txbBADPIXEL_THRESHOLD
            // 
            this.txbBADPIXEL_THRESHOLD.Location = new System.Drawing.Point(94, 26);
            this.txbBADPIXEL_THRESHOLD.Name = "txbBADPIXEL_THRESHOLD";
            this.txbBADPIXEL_THRESHOLD.Size = new System.Drawing.Size(100, 21);
            this.txbBADPIXEL_THRESHOLD.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "亮点阈值：";
            // 
            // txbBADPIXEL_SORTNUM
            // 
            this.txbBADPIXEL_SORTNUM.Location = new System.Drawing.Point(94, 72);
            this.txbBADPIXEL_SORTNUM.Name = "txbBADPIXEL_SORTNUM";
            this.txbBADPIXEL_SORTNUM.Size = new System.Drawing.Size(100, 21);
            this.txbBADPIXEL_SORTNUM.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txbHOTPIXEL_SPEC);
            this.groupBox2.Controls.Add(this.txbHOTPIXEL_SORTNUM);
            this.groupBox2.Controls.Add(this.txbHOTPIXEL_THRESHOLD);
            this.groupBox2.Location = new System.Drawing.Point(5, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(210, 160);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Hot Pixel";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "Spec Num：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 78);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 1;
            this.label9.Text = "连续个数：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "暗点阈值：";
            // 
            // txbHOTPIXEL_SPEC
            // 
            this.txbHOTPIXEL_SPEC.Location = new System.Drawing.Point(92, 112);
            this.txbHOTPIXEL_SPEC.Name = "txbHOTPIXEL_SPEC";
            this.txbHOTPIXEL_SPEC.Size = new System.Drawing.Size(100, 21);
            this.txbHOTPIXEL_SPEC.TabIndex = 0;
            // 
            // txbHOTPIXEL_SORTNUM
            // 
            this.txbHOTPIXEL_SORTNUM.Location = new System.Drawing.Point(92, 72);
            this.txbHOTPIXEL_SORTNUM.Name = "txbHOTPIXEL_SORTNUM";
            this.txbHOTPIXEL_SORTNUM.Size = new System.Drawing.Size(100, 21);
            this.txbHOTPIXEL_SORTNUM.TabIndex = 0;
            // 
            // txbHOTPIXEL_THRESHOLD
            // 
            this.txbHOTPIXEL_THRESHOLD.Location = new System.Drawing.Point(92, 26);
            this.txbHOTPIXEL_THRESHOLD.Name = "txbHOTPIXEL_THRESHOLD";
            this.txbHOTPIXEL_THRESHOLD.Size = new System.Drawing.Size(100, 21);
            this.txbHOTPIXEL_THRESHOLD.TabIndex = 0;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(494, 98);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 6;
            this.label15.Text = "成像圆内缩：";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txbROISIZE);
            this.groupBox4.Controls.Add(this.txbSPEC_NumNODEEP);
            this.groupBox4.Controls.Add(this.txbSIZENODEEP);
            this.groupBox4.Controls.Add(this.txbLUX_NoDEEPBLEMISH);
            this.groupBox4.Controls.Add(this.txbSPEC_NumDEEP);
            this.groupBox4.Controls.Add(this.txbSIZEDEEP);
            this.groupBox4.Controls.Add(this.txbLUX_DEEPBLEMISH);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Location = new System.Drawing.Point(6, 214);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(671, 144);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "脏污参数";
            // 
            // txbROISIZE
            // 
            this.txbROISIZE.Location = new System.Drawing.Point(91, 98);
            this.txbROISIZE.Name = "txbROISIZE";
            this.txbROISIZE.Size = new System.Drawing.Size(100, 21);
            this.txbROISIZE.TabIndex = 0;
            // 
            // txbSPEC_NumNODEEP
            // 
            this.txbSPEC_NumNODEEP.Location = new System.Drawing.Point(516, 67);
            this.txbSPEC_NumNODEEP.Name = "txbSPEC_NumNODEEP";
            this.txbSPEC_NumNODEEP.Size = new System.Drawing.Size(100, 21);
            this.txbSPEC_NumNODEEP.TabIndex = 0;
            // 
            // txbSIZENODEEP
            // 
            this.txbSIZENODEEP.Location = new System.Drawing.Point(299, 67);
            this.txbSIZENODEEP.Name = "txbSIZENODEEP";
            this.txbSIZENODEEP.Size = new System.Drawing.Size(100, 21);
            this.txbSIZENODEEP.TabIndex = 0;
            // 
            // txbLUX_NoDEEPBLEMISH
            // 
            this.txbLUX_NoDEEPBLEMISH.Location = new System.Drawing.Point(91, 67);
            this.txbLUX_NoDEEPBLEMISH.Name = "txbLUX_NoDEEPBLEMISH";
            this.txbLUX_NoDEEPBLEMISH.Size = new System.Drawing.Size(100, 21);
            this.txbLUX_NoDEEPBLEMISH.TabIndex = 0;
            // 
            // txbSPEC_NumDEEP
            // 
            this.txbSPEC_NumDEEP.Location = new System.Drawing.Point(516, 31);
            this.txbSPEC_NumDEEP.Name = "txbSPEC_NumDEEP";
            this.txbSPEC_NumDEEP.Size = new System.Drawing.Size(100, 21);
            this.txbSPEC_NumDEEP.TabIndex = 0;
            // 
            // txbSIZEDEEP
            // 
            this.txbSIZEDEEP.Location = new System.Drawing.Point(299, 31);
            this.txbSIZEDEEP.Name = "txbSIZEDEEP";
            this.txbSIZEDEEP.Size = new System.Drawing.Size(100, 21);
            this.txbSIZEDEEP.TabIndex = 0;
            // 
            // txbLUX_DEEPBLEMISH
            // 
            this.txbLUX_DEEPBLEMISH.Location = new System.Drawing.Point(91, 31);
            this.txbLUX_DEEPBLEMISH.Name = "txbLUX_DEEPBLEMISH";
            this.txbLUX_DEEPBLEMISH.Size = new System.Drawing.Size(100, 21);
            this.txbLUX_DEEPBLEMISH.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 104);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 1;
            this.label11.Text = "ROI Size：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(433, 73);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 12);
            this.label13.TabIndex = 1;
            this.label13.Text = "浅脏污个数：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(216, 73);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 12);
            this.label12.TabIndex = 1;
            this.label12.Text = "浅污点群定义：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 73);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 12);
            this.label14.TabIndex = 1;
            this.label14.Text = "浅污点阈值：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(433, 40);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 12);
            this.label16.TabIndex = 1;
            this.label16.Text = "深脏污个数：";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(216, 40);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(89, 12);
            this.label17.TabIndex = 1;
            this.label17.Text = "深污点群定义：";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(8, 40);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(77, 12);
            this.label18.TabIndex = 1;
            this.label18.Text = "深污点阈值：";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(458, 65);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(113, 12);
            this.label19.TabIndex = 7;
            this.label19.Text = "binary threshold：";
            // 
            // txbCIRCLE_R
            // 
            this.txbCIRCLE_R.Location = new System.Drawing.Point(577, 93);
            this.txbCIRCLE_R.Name = "txbCIRCLE_R";
            this.txbCIRCLE_R.Size = new System.Drawing.Size(100, 21);
            this.txbCIRCLE_R.TabIndex = 4;
            // 
            // txbBinaryThreshold
            // 
            this.txbBinaryThreshold.Location = new System.Drawing.Point(577, 56);
            this.txbBinaryThreshold.Name = "txbBinaryThreshold";
            this.txbBinaryThreshold.Size = new System.Drawing.Size(100, 21);
            this.txbBinaryThreshold.TabIndex = 5;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Controls.Add(this.comboxPin_14);
            this.tabPage3.Controls.Add(this.button6);
            this.tabPage3.Controls.Add(this.button5);
            this.tabPage3.Controls.Add(this.button4);
            this.tabPage3.Controls.Add(this.button7);
            this.tabPage3.Controls.Add(this.label29);
            this.tabPage3.Controls.Add(this.comboxPin_13);
            this.tabPage3.Controls.Add(this.comboxPin_26);
            this.tabPage3.Controls.Add(this.label28);
            this.tabPage3.Controls.Add(this.label41);
            this.tabPage3.Controls.Add(this.comboxPin_12);
            this.tabPage3.Controls.Add(this.comboxPin_25);
            this.tabPage3.Controls.Add(this.label27);
            this.tabPage3.Controls.Add(this.label40);
            this.tabPage3.Controls.Add(this.comboxPin_11);
            this.tabPage3.Controls.Add(this.comboxPin_24);
            this.tabPage3.Controls.Add(this.label26);
            this.tabPage3.Controls.Add(this.label39);
            this.tabPage3.Controls.Add(this.comboxPin_10);
            this.tabPage3.Controls.Add(this.comboxPin_23);
            this.tabPage3.Controls.Add(this.label25);
            this.tabPage3.Controls.Add(this.label38);
            this.tabPage3.Controls.Add(this.comboxPin_09);
            this.tabPage3.Controls.Add(this.comboxPin_22);
            this.tabPage3.Controls.Add(this.label24);
            this.tabPage3.Controls.Add(this.label37);
            this.tabPage3.Controls.Add(this.comboxPin_08);
            this.tabPage3.Controls.Add(this.comboxPin_21);
            this.tabPage3.Controls.Add(this.label23);
            this.tabPage3.Controls.Add(this.label36);
            this.tabPage3.Controls.Add(this.comboxPin_07);
            this.tabPage3.Controls.Add(this.comboxPin_20);
            this.tabPage3.Controls.Add(this.label22);
            this.tabPage3.Controls.Add(this.label35);
            this.tabPage3.Controls.Add(this.comboxPin_06);
            this.tabPage3.Controls.Add(this.comboxPin_19);
            this.tabPage3.Controls.Add(this.label21);
            this.tabPage3.Controls.Add(this.label34);
            this.tabPage3.Controls.Add(this.comboxPin_05);
            this.tabPage3.Controls.Add(this.comboxPin_18);
            this.tabPage3.Controls.Add(this.label20);
            this.tabPage3.Controls.Add(this.label33);
            this.tabPage3.Controls.Add(this.comboxPin_04);
            this.tabPage3.Controls.Add(this.comboxPin_17);
            this.tabPage3.Controls.Add(this.label30);
            this.tabPage3.Controls.Add(this.label32);
            this.tabPage3.Controls.Add(this.comboxPin_03);
            this.tabPage3.Controls.Add(this.comboxPin_16);
            this.tabPage3.Controls.Add(this.label31);
            this.tabPage3.Controls.Add(this.label42);
            this.tabPage3.Controls.Add(this.comboxPin_02);
            this.tabPage3.Controls.Add(this.comboxPin_15);
            this.tabPage3.Controls.Add(this.label43);
            this.tabPage3.Controls.Add(this.label44);
            this.tabPage3.Controls.Add(this.comboxPin_01);
            this.tabPage3.Controls.Add(this.label45);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(788, 439);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Pin";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(471, 185);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 32);
            this.button1.TabIndex = 61;
            this.button1.Text = "加载";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboxPin_14
            // 
            this.comboxPin_14.FormattingEnabled = true;
            this.comboxPin_14.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_14.Location = new System.Drawing.Point(300, 35);
            this.comboxPin_14.Name = "comboxPin_14";
            this.comboxPin_14.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_14.TabIndex = 54;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(471, 335);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 32);
            this.button6.TabIndex = 60;
            this.button6.Text = "确定";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(471, 297);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 32);
            this.button5.TabIndex = 59;
            this.button5.Text = "取消";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(471, 259);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 32);
            this.button4.TabIndex = 58;
            this.button4.Text = "标准并口";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(471, 223);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 32);
            this.button7.TabIndex = 57;
            this.button7.Text = "标准MIPI";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(236, 35);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(41, 12);
            this.label29.TabIndex = 23;
            this.label29.Text = "Pin_14";
            // 
            // comboxPin_13
            // 
            this.comboxPin_13.FormattingEnabled = true;
            this.comboxPin_13.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_13.Location = new System.Drawing.Point(92, 347);
            this.comboxPin_13.Name = "comboxPin_13";
            this.comboxPin_13.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_13.TabIndex = 41;
            // 
            // comboxPin_26
            // 
            this.comboxPin_26.FormattingEnabled = true;
            this.comboxPin_26.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_26.Location = new System.Drawing.Point(300, 347);
            this.comboxPin_26.Name = "comboxPin_26";
            this.comboxPin_26.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_26.TabIndex = 42;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(28, 347);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(41, 12);
            this.label28.TabIndex = 22;
            this.label28.Text = "Pin_13";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(236, 347);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(41, 12);
            this.label41.TabIndex = 21;
            this.label41.Text = "Pin_26";
            // 
            // comboxPin_12
            // 
            this.comboxPin_12.FormattingEnabled = true;
            this.comboxPin_12.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_12.Location = new System.Drawing.Point(92, 321);
            this.comboxPin_12.Name = "comboxPin_12";
            this.comboxPin_12.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_12.TabIndex = 47;
            // 
            // comboxPin_25
            // 
            this.comboxPin_25.FormattingEnabled = true;
            this.comboxPin_25.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_25.Location = new System.Drawing.Point(300, 321);
            this.comboxPin_25.Name = "comboxPin_25";
            this.comboxPin_25.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_25.TabIndex = 44;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(28, 321);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(41, 12);
            this.label27.TabIndex = 25;
            this.label27.Text = "Pin_12";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(236, 321);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(41, 12);
            this.label40.TabIndex = 17;
            this.label40.Text = "Pin_25";
            // 
            // comboxPin_11
            // 
            this.comboxPin_11.FormattingEnabled = true;
            this.comboxPin_11.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_11.Location = new System.Drawing.Point(92, 295);
            this.comboxPin_11.Name = "comboxPin_11";
            this.comboxPin_11.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_11.TabIndex = 45;
            // 
            // comboxPin_24
            // 
            this.comboxPin_24.FormattingEnabled = true;
            this.comboxPin_24.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_24.Location = new System.Drawing.Point(300, 295);
            this.comboxPin_24.Name = "comboxPin_24";
            this.comboxPin_24.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_24.TabIndex = 46;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(28, 295);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(41, 12);
            this.label26.TabIndex = 15;
            this.label26.Text = "Pin_11";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(236, 295);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(41, 12);
            this.label39.TabIndex = 14;
            this.label39.Text = "Pin_24";
            // 
            // comboxPin_10
            // 
            this.comboxPin_10.FormattingEnabled = true;
            this.comboxPin_10.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_10.Location = new System.Drawing.Point(92, 269);
            this.comboxPin_10.Name = "comboxPin_10";
            this.comboxPin_10.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_10.TabIndex = 53;
            // 
            // comboxPin_23
            // 
            this.comboxPin_23.FormattingEnabled = true;
            this.comboxPin_23.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_23.Location = new System.Drawing.Point(300, 269);
            this.comboxPin_23.Name = "comboxPin_23";
            this.comboxPin_23.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_23.TabIndex = 48;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(28, 269);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(41, 12);
            this.label25.TabIndex = 11;
            this.label25.Text = "Pin_10";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(236, 269);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(41, 12);
            this.label38.TabIndex = 10;
            this.label38.Text = "Pin_23";
            // 
            // comboxPin_09
            // 
            this.comboxPin_09.FormattingEnabled = true;
            this.comboxPin_09.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_09.Location = new System.Drawing.Point(92, 243);
            this.comboxPin_09.Name = "comboxPin_09";
            this.comboxPin_09.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_09.TabIndex = 49;
            // 
            // comboxPin_22
            // 
            this.comboxPin_22.FormattingEnabled = true;
            this.comboxPin_22.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_22.Location = new System.Drawing.Point(300, 243);
            this.comboxPin_22.Name = "comboxPin_22";
            this.comboxPin_22.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_22.TabIndex = 50;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(28, 243);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(41, 12);
            this.label24.TabIndex = 9;
            this.label24.Text = "Pin_09";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(236, 243);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(41, 12);
            this.label37.TabIndex = 8;
            this.label37.Text = "Pin_22";
            // 
            // comboxPin_08
            // 
            this.comboxPin_08.FormattingEnabled = true;
            this.comboxPin_08.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_08.Location = new System.Drawing.Point(92, 217);
            this.comboxPin_08.Name = "comboxPin_08";
            this.comboxPin_08.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_08.TabIndex = 39;
            // 
            // comboxPin_21
            // 
            this.comboxPin_21.FormattingEnabled = true;
            this.comboxPin_21.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_21.Location = new System.Drawing.Point(300, 217);
            this.comboxPin_21.Name = "comboxPin_21";
            this.comboxPin_21.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_21.TabIndex = 52;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(28, 217);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(41, 12);
            this.label23.TabIndex = 5;
            this.label23.Text = "Pin_08";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(236, 217);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(41, 12);
            this.label36.TabIndex = 4;
            this.label36.Text = "Pin_21";
            // 
            // comboxPin_07
            // 
            this.comboxPin_07.FormattingEnabled = true;
            this.comboxPin_07.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_07.Location = new System.Drawing.Point(92, 191);
            this.comboxPin_07.Name = "comboxPin_07";
            this.comboxPin_07.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_07.TabIndex = 40;
            // 
            // comboxPin_20
            // 
            this.comboxPin_20.FormattingEnabled = true;
            this.comboxPin_20.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_20.Location = new System.Drawing.Point(300, 191);
            this.comboxPin_20.Name = "comboxPin_20";
            this.comboxPin_20.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_20.TabIndex = 43;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(28, 191);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(41, 12);
            this.label22.TabIndex = 16;
            this.label22.Text = "Pin_07";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(236, 191);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(41, 12);
            this.label35.TabIndex = 18;
            this.label35.Text = "Pin_20";
            // 
            // comboxPin_06
            // 
            this.comboxPin_06.FormattingEnabled = true;
            this.comboxPin_06.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_06.Location = new System.Drawing.Point(92, 165);
            this.comboxPin_06.Name = "comboxPin_06";
            this.comboxPin_06.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_06.TabIndex = 30;
            // 
            // comboxPin_19
            // 
            this.comboxPin_19.FormattingEnabled = true;
            this.comboxPin_19.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_19.Location = new System.Drawing.Point(300, 165);
            this.comboxPin_19.Name = "comboxPin_19";
            this.comboxPin_19.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_19.TabIndex = 31;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(28, 165);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(41, 12);
            this.label21.TabIndex = 6;
            this.label21.Text = "Pin_06";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(236, 165);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(41, 12);
            this.label34.TabIndex = 7;
            this.label34.Text = "Pin_19";
            // 
            // comboxPin_05
            // 
            this.comboxPin_05.FormattingEnabled = true;
            this.comboxPin_05.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_05.Location = new System.Drawing.Point(92, 139);
            this.comboxPin_05.Name = "comboxPin_05";
            this.comboxPin_05.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_05.TabIndex = 32;
            // 
            // comboxPin_18
            // 
            this.comboxPin_18.FormattingEnabled = true;
            this.comboxPin_18.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_18.Location = new System.Drawing.Point(300, 139);
            this.comboxPin_18.Name = "comboxPin_18";
            this.comboxPin_18.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_18.TabIndex = 34;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(28, 139);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(41, 12);
            this.label20.TabIndex = 12;
            this.label20.Text = "Pin_05";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(236, 139);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(41, 12);
            this.label33.TabIndex = 13;
            this.label33.Text = "Pin_18";
            // 
            // comboxPin_04
            // 
            this.comboxPin_04.FormattingEnabled = true;
            this.comboxPin_04.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_04.Location = new System.Drawing.Point(92, 113);
            this.comboxPin_04.Name = "comboxPin_04";
            this.comboxPin_04.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_04.TabIndex = 38;
            // 
            // comboxPin_17
            // 
            this.comboxPin_17.FormattingEnabled = true;
            this.comboxPin_17.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_17.Location = new System.Drawing.Point(300, 113);
            this.comboxPin_17.Name = "comboxPin_17";
            this.comboxPin_17.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_17.TabIndex = 29;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(28, 113);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(41, 12);
            this.label30.TabIndex = 19;
            this.label30.Text = "Pin_04";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(236, 113);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(41, 12);
            this.label32.TabIndex = 20;
            this.label32.Text = "Pin_17";
            // 
            // comboxPin_03
            // 
            this.comboxPin_03.FormattingEnabled = true;
            this.comboxPin_03.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_03.Location = new System.Drawing.Point(92, 87);
            this.comboxPin_03.Name = "comboxPin_03";
            this.comboxPin_03.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_03.TabIndex = 36;
            // 
            // comboxPin_16
            // 
            this.comboxPin_16.FormattingEnabled = true;
            this.comboxPin_16.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_16.Location = new System.Drawing.Point(300, 87);
            this.comboxPin_16.Name = "comboxPin_16";
            this.comboxPin_16.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_16.TabIndex = 35;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(28, 87);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(41, 12);
            this.label31.TabIndex = 26;
            this.label31.Text = "Pin_03";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(236, 87);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(41, 12);
            this.label42.TabIndex = 27;
            this.label42.Text = "Pin_16";
            // 
            // comboxPin_02
            // 
            this.comboxPin_02.FormattingEnabled = true;
            this.comboxPin_02.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_02.Location = new System.Drawing.Point(92, 61);
            this.comboxPin_02.Name = "comboxPin_02";
            this.comboxPin_02.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_02.TabIndex = 33;
            // 
            // comboxPin_15
            // 
            this.comboxPin_15.FormattingEnabled = true;
            this.comboxPin_15.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_15.Location = new System.Drawing.Point(300, 61);
            this.comboxPin_15.Name = "comboxPin_15";
            this.comboxPin_15.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_15.TabIndex = 37;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(28, 61);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(41, 12);
            this.label43.TabIndex = 28;
            this.label43.Text = "Pin_02";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(236, 61);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(41, 12);
            this.label44.TabIndex = 24;
            this.label44.Text = "Pin_15";
            // 
            // comboxPin_01
            // 
            this.comboxPin_01.FormattingEnabled = true;
            this.comboxPin_01.Items.AddRange(new object[] {
            "D0",
            "D1",
            "D2",
            "D3",
            "D4",
            "D5",
            "D6",
            "D7",
            "D8",
            "D9",
            "PCLK",
            "HSYNC",
            "VSYNC",
            "MCLK",
            "RESET",
            "PWDN",
            "PWDN2",
            "PO1",
            "SDA",
            "SCL",
            "NC",
            "PO2",
            "PO3",
            "PO4",
            "NC1",
            "NC2"});
            this.comboxPin_01.Location = new System.Drawing.Point(92, 35);
            this.comboxPin_01.Name = "comboxPin_01";
            this.comboxPin_01.Size = new System.Drawing.Size(121, 20);
            this.comboxPin_01.TabIndex = 51;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(28, 35);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(41, 12);
            this.label45.TabIndex = 3;
            this.label45.Text = "Pin_01";
            // 
            // frmWhiteBorad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 470);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmWhiteBorad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AA_Image";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWhiteBorad_FormClosing);
            this.Load += new System.EventHandler(this.frmWhiteBorad_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnTakePicture;
        private System.Windows.Forms.Label lblDisplay_fps;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblFram_fps;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblFram_ERROR;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFram_OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPlay;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox DevCombox;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txbBADPIXEL_SPEC;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txbBADPIXEL_THRESHOLD;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txbBADPIXEL_SORTNUM;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txbHOTPIXEL_SPEC;
        private System.Windows.Forms.TextBox txbHOTPIXEL_SORTNUM;
        private System.Windows.Forms.TextBox txbHOTPIXEL_THRESHOLD;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txbROISIZE;
        private System.Windows.Forms.TextBox txbSPEC_NumNODEEP;
        private System.Windows.Forms.TextBox txbSIZENODEEP;
        private System.Windows.Forms.TextBox txbLUX_NoDEEPBLEMISH;
        private System.Windows.Forms.TextBox txbSPEC_NumDEEP;
        private System.Windows.Forms.TextBox txbSIZEDEEP;
        private System.Windows.Forms.TextBox txbLUX_DEEPBLEMISH;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txbCIRCLE_R;
        private System.Windows.Forms.TextBox txbBinaryThreshold;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ComboBox comboxPin_14;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox comboxPin_13;
        private System.Windows.Forms.ComboBox comboxPin_26;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.ComboBox comboxPin_12;
        private System.Windows.Forms.ComboBox comboxPin_25;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ComboBox comboxPin_11;
        private System.Windows.Forms.ComboBox comboxPin_24;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.ComboBox comboxPin_10;
        private System.Windows.Forms.ComboBox comboxPin_23;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.ComboBox comboxPin_09;
        private System.Windows.Forms.ComboBox comboxPin_22;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.ComboBox comboxPin_08;
        private System.Windows.Forms.ComboBox comboxPin_21;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.ComboBox comboxPin_07;
        private System.Windows.Forms.ComboBox comboxPin_20;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.ComboBox comboxPin_06;
        private System.Windows.Forms.ComboBox comboxPin_19;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.ComboBox comboxPin_05;
        private System.Windows.Forms.ComboBox comboxPin_18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.ComboBox comboxPin_04;
        private System.Windows.Forms.ComboBox comboxPin_17;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.ComboBox comboxPin_03;
        private System.Windows.Forms.ComboBox comboxPin_16;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.ComboBox comboxPin_02;
        private System.Windows.Forms.ComboBox comboxPin_15;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.ComboBox comboxPin_01;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.CheckBox ckbSavePhotho;
        private System.Windows.Forms.CheckBox ckbShowDefect;
        private System.Windows.Forms.CheckBox ckbShowStain;
        private System.Windows.Forms.Label lblBad;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label lblStain;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label lblCapImage;
        // private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

