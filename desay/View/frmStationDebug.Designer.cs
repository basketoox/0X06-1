namespace desay
{
    partial class frmStationDebug
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStationDebug));
            this.gbxLeftplateform = new System.Windows.Forms.GroupBox();
            this.gbxRightplateform = new System.Windows.Forms.GroupBox();
            this.gbxAssembly = new System.Windows.Forms.GroupBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flpCylinder = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtSN = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtGlueOver = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGlueWorking = new System.Windows.Forms.TextBox();
            this.txtGlueCallOutFinish = new System.Windows.Forms.TextBox();
            this.txtCleanHoming = new System.Windows.Forms.TextBox();
            this.txtCleanCallOutFinish = new System.Windows.Forms.TextBox();
            this.txtCarrierCallOutFinish = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtFN = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGlueHoming = new System.Windows.Forms.TextBox();
            this.txtGlueResult = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCleanOver = new System.Windows.Forms.TextBox();
            this.txtCleanResult = new System.Windows.Forms.TextBox();
            this.txtGlueCallInFinish = new System.Windows.Forms.TextBox();
            this.txtCleanWorking = new System.Windows.Forms.TextBox();
            this.txtCleanCallInFinish = new System.Windows.Forms.TextBox();
            this.txtCarrierCallInFinish = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtGlueCallOut = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGlueCallIn = new System.Windows.Forms.TextBox();
            this.txtCleanCallOut = new System.Windows.Forms.TextBox();
            this.txtCleanCallIn = new System.Windows.Forms.TextBox();
            this.txtCarrierCallOut = new System.Windows.Forms.TextBox();
            this.txtCarrierCallIn = new System.Windows.Forms.TextBox();
            this.btnCarrier1 = new System.Windows.Forms.Button();
            this.btnCarrier2 = new System.Windows.Forms.Button();
            this.btnCarrier3 = new System.Windows.Forms.Button();
            this.btnCarrier4 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxLeftplateform
            // 
            this.gbxLeftplateform.Location = new System.Drawing.Point(12, 3);
            this.gbxLeftplateform.Name = "gbxLeftplateform";
            this.gbxLeftplateform.Size = new System.Drawing.Size(819, 72);
            this.gbxLeftplateform.TabIndex = 0;
            this.gbxLeftplateform.TabStop = false;
            this.gbxLeftplateform.Text = "清洗工位";
            // 
            // gbxRightplateform
            // 
            this.gbxRightplateform.Location = new System.Drawing.Point(12, 81);
            this.gbxRightplateform.Name = "gbxRightplateform";
            this.gbxRightplateform.Size = new System.Drawing.Size(819, 72);
            this.gbxRightplateform.TabIndex = 0;
            this.gbxRightplateform.TabStop = false;
            this.gbxRightplateform.Text = "点胶工位";
            // 
            // gbxAssembly
            // 
            this.gbxAssembly.Location = new System.Drawing.Point(12, 159);
            this.gbxAssembly.Name = "gbxAssembly";
            this.gbxAssembly.Size = new System.Drawing.Size(819, 72);
            this.gbxAssembly.TabIndex = 0;
            this.gbxAssembly.TabStop = false;
            this.gbxAssembly.Text = "组装工位";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flpCylinder);
            this.groupBox1.Location = new System.Drawing.Point(12, 246);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(822, 87);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "气缸状态";
            // 
            // flpCylinder
            // 
            this.flpCylinder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpCylinder.Location = new System.Drawing.Point(2, 16);
            this.flpCylinder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flpCylinder.Name = "flpCylinder";
            this.flpCylinder.Size = new System.Drawing.Size(818, 69);
            this.flpCylinder.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.txtSN);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtGlueOver);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtGlueWorking);
            this.groupBox2.Controls.Add(this.txtGlueCallOutFinish);
            this.groupBox2.Controls.Add(this.txtCleanHoming);
            this.groupBox2.Controls.Add(this.txtCleanCallOutFinish);
            this.groupBox2.Controls.Add(this.txtCarrierCallOutFinish);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtFN);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtGlueHoming);
            this.groupBox2.Controls.Add(this.txtGlueResult);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtCleanOver);
            this.groupBox2.Controls.Add(this.txtCleanResult);
            this.groupBox2.Controls.Add(this.txtGlueCallInFinish);
            this.groupBox2.Controls.Add(this.txtCleanWorking);
            this.groupBox2.Controls.Add(this.txtCleanCallInFinish);
            this.groupBox2.Controls.Add(this.txtCarrierCallInFinish);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtGlueCallOut);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtGlueCallIn);
            this.groupBox2.Controls.Add(this.txtCleanCallOut);
            this.groupBox2.Controls.Add(this.txtCleanCallIn);
            this.groupBox2.Controls.Add(this.txtCarrierCallOut);
            this.groupBox2.Controls.Add(this.txtCarrierCallIn);
            this.groupBox2.Location = new System.Drawing.Point(14, 351);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(817, 206);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "状态显示";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(774, 165);
            this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(41, 12);
            this.label24.TabIndex = 1;
            this.label24.Text = "产品码";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(724, 116);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(53, 12);
            this.label18.TabIndex = 1;
            this.label18.Text = "点胶结束";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(724, 70);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 12);
            this.label12.TabIndex = 1;
            this.label12.Text = "点胶出料完成";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(451, 118);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 1;
            this.label17.Text = "点胶工作中";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(451, 72);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "清洗出料完成";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(176, 118);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 12);
            this.label16.TabIndex = 1;
            this.label16.Text = "清洗复位中";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(176, 72);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "载具出料完成";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(641, 166);
            this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(41, 12);
            this.label21.TabIndex = 1;
            this.label21.Text = "治具码";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(590, 118);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 12);
            this.label15.TabIndex = 1;
            this.label15.Text = "点胶复位中";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(317, 168);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 12);
            this.label20.TabIndex = 1;
            this.label20.Text = "点胶结果";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(590, 72);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 12);
            this.label11.TabIndex = 1;
            this.label11.Text = "点胶入料完成";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(43, 168);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(53, 12);
            this.label19.TabIndex = 1;
            this.label19.Text = "清洗结果";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(317, 119);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 1;
            this.label14.Text = "清洗结束";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(43, 119);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 1;
            this.label13.Text = "清洗工作中";
            // 
            // txtSN
            // 
            this.txtSN.Location = new System.Drawing.Point(686, 161);
            this.txtSN.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSN.Name = "txtSN";
            this.txtSN.ReadOnly = true;
            this.txtSN.Size = new System.Drawing.Size(88, 21);
            this.txtSN.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(317, 74);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "清洗入料完成";
            // 
            // txtGlueOver
            // 
            this.txtGlueOver.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGlueOver.Location = new System.Drawing.Point(700, 112);
            this.txtGlueOver.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtGlueOver.Name = "txtGlueOver";
            this.txtGlueOver.ReadOnly = true;
            this.txtGlueOver.Size = new System.Drawing.Size(19, 14);
            this.txtGlueOver.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 74);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "载具入料完成";
            // 
            // txtGlueWorking
            // 
            this.txtGlueWorking.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGlueWorking.Location = new System.Drawing.Point(428, 114);
            this.txtGlueWorking.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtGlueWorking.Name = "txtGlueWorking";
            this.txtGlueWorking.ReadOnly = true;
            this.txtGlueWorking.Size = new System.Drawing.Size(19, 14);
            this.txtGlueWorking.TabIndex = 0;
            // 
            // txtGlueCallOutFinish
            // 
            this.txtGlueCallOutFinish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGlueCallOutFinish.Location = new System.Drawing.Point(700, 66);
            this.txtGlueCallOutFinish.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtGlueCallOutFinish.Name = "txtGlueCallOutFinish";
            this.txtGlueCallOutFinish.ReadOnly = true;
            this.txtGlueCallOutFinish.Size = new System.Drawing.Size(19, 14);
            this.txtGlueCallOutFinish.TabIndex = 0;
            // 
            // txtCleanHoming
            // 
            this.txtCleanHoming.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCleanHoming.Location = new System.Drawing.Point(153, 114);
            this.txtCleanHoming.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCleanHoming.Name = "txtCleanHoming";
            this.txtCleanHoming.ReadOnly = true;
            this.txtCleanHoming.Size = new System.Drawing.Size(19, 14);
            this.txtCleanHoming.TabIndex = 0;
            // 
            // txtCleanCallOutFinish
            // 
            this.txtCleanCallOutFinish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCleanCallOutFinish.Location = new System.Drawing.Point(428, 68);
            this.txtCleanCallOutFinish.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCleanCallOutFinish.Name = "txtCleanCallOutFinish";
            this.txtCleanCallOutFinish.ReadOnly = true;
            this.txtCleanCallOutFinish.Size = new System.Drawing.Size(19, 14);
            this.txtCleanCallOutFinish.TabIndex = 0;
            // 
            // txtCarrierCallOutFinish
            // 
            this.txtCarrierCallOutFinish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCarrierCallOutFinish.Location = new System.Drawing.Point(153, 68);
            this.txtCarrierCallOutFinish.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCarrierCallOutFinish.Name = "txtCarrierCallOutFinish";
            this.txtCarrierCallOutFinish.ReadOnly = true;
            this.txtCarrierCallOutFinish.Size = new System.Drawing.Size(19, 14);
            this.txtCarrierCallOutFinish.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(724, 22);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "点胶出料";
            // 
            // txtFN
            // 
            this.txtFN.Location = new System.Drawing.Point(554, 162);
            this.txtFN.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtFN.Name = "txtFN";
            this.txtFN.ReadOnly = true;
            this.txtFN.Size = new System.Drawing.Size(87, 21);
            this.txtFN.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(451, 24);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "清洗出料";
            // 
            // txtGlueHoming
            // 
            this.txtGlueHoming.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGlueHoming.Location = new System.Drawing.Point(567, 114);
            this.txtGlueHoming.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtGlueHoming.Name = "txtGlueHoming";
            this.txtGlueHoming.ReadOnly = true;
            this.txtGlueHoming.Size = new System.Drawing.Size(19, 14);
            this.txtGlueHoming.TabIndex = 0;
            // 
            // txtGlueResult
            // 
            this.txtGlueResult.BackColor = System.Drawing.SystemColors.Control;
            this.txtGlueResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGlueResult.Location = new System.Drawing.Point(294, 164);
            this.txtGlueResult.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtGlueResult.Name = "txtGlueResult";
            this.txtGlueResult.ReadOnly = true;
            this.txtGlueResult.Size = new System.Drawing.Size(19, 14);
            this.txtGlueResult.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 24);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "载具出料";
            // 
            // txtCleanOver
            // 
            this.txtCleanOver.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCleanOver.Location = new System.Drawing.Point(294, 115);
            this.txtCleanOver.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCleanOver.Name = "txtCleanOver";
            this.txtCleanOver.ReadOnly = true;
            this.txtCleanOver.Size = new System.Drawing.Size(19, 14);
            this.txtCleanOver.TabIndex = 0;
            // 
            // txtCleanResult
            // 
            this.txtCleanResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCleanResult.Location = new System.Drawing.Point(20, 164);
            this.txtCleanResult.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCleanResult.Name = "txtCleanResult";
            this.txtCleanResult.ReadOnly = true;
            this.txtCleanResult.Size = new System.Drawing.Size(19, 14);
            this.txtCleanResult.TabIndex = 0;
            // 
            // txtGlueCallInFinish
            // 
            this.txtGlueCallInFinish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGlueCallInFinish.Location = new System.Drawing.Point(567, 68);
            this.txtGlueCallInFinish.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtGlueCallInFinish.Name = "txtGlueCallInFinish";
            this.txtGlueCallInFinish.ReadOnly = true;
            this.txtGlueCallInFinish.Size = new System.Drawing.Size(19, 14);
            this.txtGlueCallInFinish.TabIndex = 0;
            // 
            // txtCleanWorking
            // 
            this.txtCleanWorking.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCleanWorking.Location = new System.Drawing.Point(20, 115);
            this.txtCleanWorking.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCleanWorking.Name = "txtCleanWorking";
            this.txtCleanWorking.ReadOnly = true;
            this.txtCleanWorking.Size = new System.Drawing.Size(19, 14);
            this.txtCleanWorking.TabIndex = 0;
            // 
            // txtCleanCallInFinish
            // 
            this.txtCleanCallInFinish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCleanCallInFinish.Location = new System.Drawing.Point(294, 70);
            this.txtCleanCallInFinish.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCleanCallInFinish.Name = "txtCleanCallInFinish";
            this.txtCleanCallInFinish.ReadOnly = true;
            this.txtCleanCallInFinish.Size = new System.Drawing.Size(19, 14);
            this.txtCleanCallInFinish.TabIndex = 0;
            // 
            // txtCarrierCallInFinish
            // 
            this.txtCarrierCallInFinish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCarrierCallInFinish.Location = new System.Drawing.Point(20, 70);
            this.txtCarrierCallInFinish.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCarrierCallInFinish.Name = "txtCarrierCallInFinish";
            this.txtCarrierCallInFinish.ReadOnly = true;
            this.txtCarrierCallInFinish.Size = new System.Drawing.Size(19, 14);
            this.txtCarrierCallInFinish.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(590, 22);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 1;
            this.label9.Text = "点胶入料";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(317, 24);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "清洗入料";
            // 
            // txtGlueCallOut
            // 
            this.txtGlueCallOut.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGlueCallOut.Location = new System.Drawing.Point(700, 18);
            this.txtGlueCallOut.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtGlueCallOut.Name = "txtGlueCallOut";
            this.txtGlueCallOut.ReadOnly = true;
            this.txtGlueCallOut.Size = new System.Drawing.Size(19, 14);
            this.txtGlueCallOut.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "载具入料";
            // 
            // txtGlueCallIn
            // 
            this.txtGlueCallIn.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGlueCallIn.Location = new System.Drawing.Point(567, 18);
            this.txtGlueCallIn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtGlueCallIn.Name = "txtGlueCallIn";
            this.txtGlueCallIn.ReadOnly = true;
            this.txtGlueCallIn.Size = new System.Drawing.Size(19, 14);
            this.txtGlueCallIn.TabIndex = 0;
            // 
            // txtCleanCallOut
            // 
            this.txtCleanCallOut.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCleanCallOut.Location = new System.Drawing.Point(428, 20);
            this.txtCleanCallOut.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCleanCallOut.Name = "txtCleanCallOut";
            this.txtCleanCallOut.ReadOnly = true;
            this.txtCleanCallOut.Size = new System.Drawing.Size(19, 14);
            this.txtCleanCallOut.TabIndex = 0;
            // 
            // txtCleanCallIn
            // 
            this.txtCleanCallIn.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCleanCallIn.Location = new System.Drawing.Point(294, 20);
            this.txtCleanCallIn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCleanCallIn.Name = "txtCleanCallIn";
            this.txtCleanCallIn.ReadOnly = true;
            this.txtCleanCallIn.Size = new System.Drawing.Size(19, 14);
            this.txtCleanCallIn.TabIndex = 0;
            // 
            // txtCarrierCallOut
            // 
            this.txtCarrierCallOut.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCarrierCallOut.Location = new System.Drawing.Point(153, 20);
            this.txtCarrierCallOut.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCarrierCallOut.Name = "txtCarrierCallOut";
            this.txtCarrierCallOut.ReadOnly = true;
            this.txtCarrierCallOut.Size = new System.Drawing.Size(19, 14);
            this.txtCarrierCallOut.TabIndex = 0;
            // 
            // txtCarrierCallIn
            // 
            this.txtCarrierCallIn.BackColor = System.Drawing.SystemColors.Control;
            this.txtCarrierCallIn.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCarrierCallIn.Location = new System.Drawing.Point(20, 20);
            this.txtCarrierCallIn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCarrierCallIn.Name = "txtCarrierCallIn";
            this.txtCarrierCallIn.ReadOnly = true;
            this.txtCarrierCallIn.Size = new System.Drawing.Size(19, 14);
            this.txtCarrierCallIn.TabIndex = 0;
            // 
            // btnCarrier1
            // 
            this.btnCarrier1.Location = new System.Drawing.Point(59, 570);
            this.btnCarrier1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCarrier1.Name = "btnCarrier1";
            this.btnCarrier1.Size = new System.Drawing.Size(70, 34);
            this.btnCarrier1.TabIndex = 3;
            this.btnCarrier1.Text = "Plasma正转";
            this.btnCarrier1.UseVisualStyleBackColor = true;
            this.btnCarrier1.Click += new System.EventHandler(this.btnCarrier1_Click);
            // 
            // btnCarrier2
            // 
            this.btnCarrier2.Location = new System.Drawing.Point(184, 570);
            this.btnCarrier2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCarrier2.Name = "btnCarrier2";
            this.btnCarrier2.Size = new System.Drawing.Size(70, 34);
            this.btnCarrier2.TabIndex = 3;
            this.btnCarrier2.Text = "回流线反转";
            this.btnCarrier2.UseVisualStyleBackColor = true;
            this.btnCarrier2.Click += new System.EventHandler(this.btnCarrier2_Click);
            // 
            // btnCarrier3
            // 
            this.btnCarrier3.Location = new System.Drawing.Point(326, 570);
            this.btnCarrier3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCarrier3.Name = "btnCarrier3";
            this.btnCarrier3.Size = new System.Drawing.Size(70, 34);
            this.btnCarrier3.TabIndex = 3;
            this.btnCarrier3.Text = "接驳台正转";
            this.btnCarrier3.UseVisualStyleBackColor = true;
            this.btnCarrier3.Click += new System.EventHandler(this.btnCarrier3_Click);
            // 
            // btnCarrier4
            // 
            this.btnCarrier4.Location = new System.Drawing.Point(467, 570);
            this.btnCarrier4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCarrier4.Name = "btnCarrier4";
            this.btnCarrier4.Size = new System.Drawing.Size(70, 34);
            this.btnCarrier4.TabIndex = 3;
            this.btnCarrier4.Text = "接驳台反转";
            this.btnCarrier4.UseVisualStyleBackColor = true;
            this.btnCarrier4.Click += new System.EventHandler(this.btnCarrier4_Click);
            // 
            // frmStationDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 600);
            this.Controls.Add(this.btnCarrier4);
            this.Controls.Add(this.btnCarrier3);
            this.Controls.Add(this.btnCarrier2);
            this.Controls.Add(this.btnCarrier1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbxAssembly);
            this.Controls.Add(this.gbxRightplateform);
            this.Controls.Add(this.gbxLeftplateform);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmStationDebug";
            this.ShowInTaskbar = false;
            this.Text = "frmStationDebug";
            this.Load += new System.EventHandler(this.frmStationDebug_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxLeftplateform;
        private System.Windows.Forms.GroupBox gbxRightplateform;
        private System.Windows.Forms.GroupBox gbxAssembly;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flpCylinder;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGlueCallOutFinish;
        private System.Windows.Forms.TextBox txtCleanCallOutFinish;
        private System.Windows.Forms.TextBox txtCarrierCallOutFinish;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGlueCallInFinish;
        private System.Windows.Forms.TextBox txtCleanCallInFinish;
        private System.Windows.Forms.TextBox txtCarrierCallInFinish;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtGlueCallOut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGlueCallIn;
        private System.Windows.Forms.TextBox txtCleanCallOut;
        private System.Windows.Forms.TextBox txtCleanCallIn;
        private System.Windows.Forms.TextBox txtCarrierCallOut;
        private System.Windows.Forms.TextBox txtCarrierCallIn;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtGlueOver;
        private System.Windows.Forms.TextBox txtGlueWorking;
        private System.Windows.Forms.TextBox txtCleanHoming;
        private System.Windows.Forms.TextBox txtGlueHoming;
        private System.Windows.Forms.TextBox txtCleanOver;
        private System.Windows.Forms.TextBox txtCleanWorking;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtSN;
        private System.Windows.Forms.TextBox txtFN;
        private System.Windows.Forms.TextBox txtGlueResult;
        private System.Windows.Forms.TextBox txtCleanResult;
        private System.Windows.Forms.Button btnCarrier1;
        private System.Windows.Forms.Button btnCarrier2;
        private System.Windows.Forms.Button btnCarrier3;
        private System.Windows.Forms.Button btnCarrier4;
    }
}