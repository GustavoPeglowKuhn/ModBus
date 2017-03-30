namespace ModBus {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing&&(components!=null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.ms_SerialPort = new System.Windows.Forms.ToolStripMenuItem();
			this.ms_sp_port = new System.Windows.Forms.ToolStripMenuItem();
			this.ms_sp_port_combobox = new System.Windows.Forms.ToolStripComboBox();
			this.ms_sp_baud = new System.Windows.Forms.ToolStripMenuItem();
			this.ms_sp_baud_combobox = new System.Windows.Forms.ToolStripComboBox();
			this.ms_sp_parity = new System.Windows.Forms.ToolStripMenuItem();
			this.ms_sp_par_combobox = new System.Windows.Forms.ToolStripComboBox();
			this.ms_sp_stopBits = new System.Windows.Forms.ToolStripMenuItem();
			this.ms_sp_stop_combobox = new System.Windows.Forms.ToolStripComboBox();
			this.ms_sp_conect = new System.Windows.Forms.ToolStripMenuItem();
			this.ms_sp_disconect = new System.Windows.Forms.ToolStripMenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.lbl_dType = new System.Windows.Forms.Label();
			this.nud_dType = new System.Windows.Forms.NumericUpDown();
			this.lbl_temp = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.tb_set_tem = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.nud_m1 = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.nud_m2 = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.nud_m3 = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.nud_m4 = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.lbl_status_m1 = new System.Windows.Forms.Label();
			this.lbl_status_m2 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.lbl_status_m3 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.lbl_status_m4 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.tb_cur_tem = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.btn_test = new System.Windows.Forms.Button();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nud_dType)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nud_m1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nud_m2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nud_m3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nud_m4)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ms_SerialPort});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(627, 24);
			this.menuStrip1.TabIndex = 3;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// ms_SerialPort
			// 
			this.ms_SerialPort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ms_sp_port,
            this.ms_sp_baud,
            this.ms_sp_parity,
            this.ms_sp_stopBits,
            this.ms_sp_conect,
            this.ms_sp_disconect});
			this.ms_SerialPort.Name = "ms_SerialPort";
			this.ms_SerialPort.Size = new System.Drawing.Size(69, 20);
			this.ms_SerialPort.Text = "SerialPort";
			// 
			// ms_sp_port
			// 
			this.ms_sp_port.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ms_sp_port_combobox});
			this.ms_sp_port.Name = "ms_sp_port";
			this.ms_sp_port.Size = new System.Drawing.Size(152, 22);
			this.ms_sp_port.Text = "Port";
			// 
			// ms_sp_port_combobox
			// 
			this.ms_sp_port_combobox.Name = "ms_sp_port_combobox";
			this.ms_sp_port_combobox.Size = new System.Drawing.Size(121, 23);
			this.ms_sp_port_combobox.DropDown += new System.EventHandler(this.ms_sp_port_combobox_DropDown);
			this.ms_sp_port_combobox.SelectedIndexChanged += new System.EventHandler(this.ms_sp_port_combobox_SelectedIndexChanged);
			// 
			// ms_sp_baud
			// 
			this.ms_sp_baud.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ms_sp_baud_combobox});
			this.ms_sp_baud.Name = "ms_sp_baud";
			this.ms_sp_baud.Size = new System.Drawing.Size(152, 22);
			this.ms_sp_baud.Text = "BaudRate";
			// 
			// ms_sp_baud_combobox
			// 
			this.ms_sp_baud_combobox.Name = "ms_sp_baud_combobox";
			this.ms_sp_baud_combobox.Size = new System.Drawing.Size(121, 23);
			this.ms_sp_baud_combobox.SelectedIndexChanged += new System.EventHandler(this.ms_sp_baud_combobox_SelectedIndexChanged);
			// 
			// ms_sp_parity
			// 
			this.ms_sp_parity.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ms_sp_par_combobox});
			this.ms_sp_parity.Name = "ms_sp_parity";
			this.ms_sp_parity.Size = new System.Drawing.Size(152, 22);
			this.ms_sp_parity.Text = "Parity";
			// 
			// ms_sp_par_combobox
			// 
			this.ms_sp_par_combobox.Name = "ms_sp_par_combobox";
			this.ms_sp_par_combobox.Size = new System.Drawing.Size(121, 23);
			this.ms_sp_par_combobox.SelectedIndexChanged += new System.EventHandler(this.ms_sp_par_combobox_SelectedIndexChanged);
			// 
			// ms_sp_stopBits
			// 
			this.ms_sp_stopBits.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ms_sp_stop_combobox});
			this.ms_sp_stopBits.Name = "ms_sp_stopBits";
			this.ms_sp_stopBits.Size = new System.Drawing.Size(152, 22);
			this.ms_sp_stopBits.Text = "StopBits";
			// 
			// ms_sp_stop_combobox
			// 
			this.ms_sp_stop_combobox.Name = "ms_sp_stop_combobox";
			this.ms_sp_stop_combobox.Size = new System.Drawing.Size(121, 23);
			this.ms_sp_stop_combobox.SelectedIndexChanged += new System.EventHandler(this.ms_sp_stop_combobox_SelectedIndexChanged);
			// 
			// ms_sp_conect
			// 
			this.ms_sp_conect.Name = "ms_sp_conect";
			this.ms_sp_conect.Size = new System.Drawing.Size(152, 22);
			this.ms_sp_conect.Text = "Conect";
			this.ms_sp_conect.Click += new System.EventHandler(this.ms_sp_conect_Click);
			// 
			// ms_sp_disconect
			// 
			this.ms_sp_disconect.Enabled = false;
			this.ms_sp_disconect.Name = "ms_sp_disconect";
			this.ms_sp_disconect.Size = new System.Drawing.Size(152, 22);
			this.ms_sp_disconect.Text = "Disconect";
			this.ms_sp_disconect.Click += new System.EventHandler(this.ms_sp_disconect_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 17);
			this.label1.TabIndex = 9;
			this.label1.Text = "device:";
			// 
			// lbl_dType
			// 
			this.lbl_dType.AutoSize = true;
			this.lbl_dType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl_dType.Location = new System.Drawing.Point(197, 27);
			this.lbl_dType.Name = "lbl_dType";
			this.lbl_dType.Size = new System.Drawing.Size(49, 17);
			this.lbl_dType.TabIndex = 11;
			this.lbl_dType.Text = "device";
			// 
			// nud_dType
			// 
			this.nud_dType.Location = new System.Drawing.Point(71, 24);
			this.nud_dType.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nud_dType.Name = "nud_dType";
			this.nud_dType.Size = new System.Drawing.Size(120, 20);
			this.nud_dType.TabIndex = 8;
			this.nud_dType.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nud_dType.ValueChanged += new System.EventHandler(this.nud_dType_ValueChanged);
			// 
			// lbl_temp
			// 
			this.lbl_temp.AutoSize = true;
			this.lbl_temp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl_temp.Location = new System.Drawing.Point(12, 180);
			this.lbl_temp.Name = "lbl_temp";
			this.lbl_temp.Size = new System.Drawing.Size(156, 17);
			this.lbl_temp.TabIndex = 12;
			this.lbl_temp.Text = "Temperatura desejada:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(234, 179);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(23, 17);
			this.label3.TabIndex = 14;
			this.label3.Text = "°C";
			// 
			// tb_set_tem
			// 
			this.tb_set_tem.Location = new System.Drawing.Point(174, 179);
			this.tb_set_tem.Name = "tb_set_tem";
			this.tb_set_tem.Size = new System.Drawing.Size(54, 20);
			this.tb_set_tem.TabIndex = 15;
			this.tb_set_tem.Text = "35";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(12, 75);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 17);
			this.label2.TabIndex = 16;
			this.label2.Text = "Tempo M1:";
			// 
			// nud_m1
			// 
			this.nud_m1.Location = new System.Drawing.Point(98, 75);
			this.nud_m1.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nud_m1.Name = "nud_m1";
			this.nud_m1.Size = new System.Drawing.Size(81, 20);
			this.nud_m1.TabIndex = 17;
			this.nud_m1.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(185, 75);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 17);
			this.label4.TabIndex = 18;
			this.label4.Text = "Segundos";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(185, 101);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 17);
			this.label5.TabIndex = 21;
			this.label5.Text = "Segundos";
			// 
			// nud_m2
			// 
			this.nud_m2.Location = new System.Drawing.Point(98, 101);
			this.nud_m2.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nud_m2.Name = "nud_m2";
			this.nud_m2.Size = new System.Drawing.Size(81, 20);
			this.nud_m2.TabIndex = 20;
			this.nud_m2.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(12, 101);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(79, 17);
			this.label6.TabIndex = 19;
			this.label6.Text = "Tempo M2:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(185, 127);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(72, 17);
			this.label7.TabIndex = 24;
			this.label7.Text = "Segundos";
			// 
			// nud_m3
			// 
			this.nud_m3.Location = new System.Drawing.Point(98, 127);
			this.nud_m3.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nud_m3.Name = "nud_m3";
			this.nud_m3.Size = new System.Drawing.Size(81, 20);
			this.nud_m3.TabIndex = 23;
			this.nud_m3.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(12, 127);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(79, 17);
			this.label8.TabIndex = 22;
			this.label8.Text = "Tempo M3:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(185, 153);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(72, 17);
			this.label9.TabIndex = 27;
			this.label9.Text = "Segundos";
			// 
			// nud_m4
			// 
			this.nud_m4.Location = new System.Drawing.Point(98, 153);
			this.nud_m4.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nud_m4.Name = "nud_m4";
			this.nud_m4.Size = new System.Drawing.Size(81, 20);
			this.nud_m4.TabIndex = 26;
			this.nud_m4.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(12, 153);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(79, 17);
			this.label10.TabIndex = 25;
			this.label10.Text = "Tempo M4:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(398, 75);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(35, 23);
			this.button1.TabIndex = 28;
			this.button1.Text = "On";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(439, 75);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(35, 23);
			this.button2.TabIndex = 29;
			this.button2.Text = "Off";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(439, 101);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(35, 23);
			this.button3.TabIndex = 31;
			this.button3.Text = "Off";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(398, 101);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(35, 23);
			this.button4.TabIndex = 30;
			this.button4.Text = "On";
			this.button4.UseVisualStyleBackColor = true;
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(439, 127);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(35, 23);
			this.button5.TabIndex = 33;
			this.button5.Text = "Off";
			this.button5.UseVisualStyleBackColor = true;
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(398, 127);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(35, 23);
			this.button6.TabIndex = 32;
			this.button6.Text = "On";
			this.button6.UseVisualStyleBackColor = true;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(263, 75);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(52, 17);
			this.label11.TabIndex = 36;
			this.label11.Text = "Status:";
			// 
			// lbl_status_m1
			// 
			this.lbl_status_m1.AutoSize = true;
			this.lbl_status_m1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl_status_m1.Location = new System.Drawing.Point(310, 75);
			this.lbl_status_m1.Name = "lbl_status_m1";
			this.lbl_status_m1.Size = new System.Drawing.Size(71, 17);
			this.lbl_status_m1.TabIndex = 37;
			this.lbl_status_m1.Text = "Desligado";
			// 
			// lbl_status_m2
			// 
			this.lbl_status_m2.AutoSize = true;
			this.lbl_status_m2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl_status_m2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.lbl_status_m2.Location = new System.Drawing.Point(309, 101);
			this.lbl_status_m2.Name = "lbl_status_m2";
			this.lbl_status_m2.Size = new System.Drawing.Size(71, 17);
			this.lbl_status_m2.TabIndex = 39;
			this.lbl_status_m2.Text = "Desligado";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label13.Location = new System.Drawing.Point(262, 101);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(52, 17);
			this.label13.TabIndex = 38;
			this.label13.Text = "Status:";
			// 
			// lbl_status_m3
			// 
			this.lbl_status_m3.AutoSize = true;
			this.lbl_status_m3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl_status_m3.Location = new System.Drawing.Point(310, 127);
			this.lbl_status_m3.Name = "lbl_status_m3";
			this.lbl_status_m3.Size = new System.Drawing.Size(71, 17);
			this.lbl_status_m3.TabIndex = 41;
			this.lbl_status_m3.Text = "Desligado";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label15.Location = new System.Drawing.Point(263, 127);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(52, 17);
			this.label15.TabIndex = 40;
			this.label15.Text = "Status:";
			// 
			// lbl_status_m4
			// 
			this.lbl_status_m4.AutoSize = true;
			this.lbl_status_m4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl_status_m4.Location = new System.Drawing.Point(310, 153);
			this.lbl_status_m4.Name = "lbl_status_m4";
			this.lbl_status_m4.Size = new System.Drawing.Size(71, 17);
			this.lbl_status_m4.TabIndex = 43;
			this.lbl_status_m4.Text = "Desligado";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label14.Location = new System.Drawing.Point(263, 153);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(52, 17);
			this.label14.TabIndex = 42;
			this.label14.Text = "Status:";
			// 
			// tb_cur_tem
			// 
			this.tb_cur_tem.Location = new System.Drawing.Point(398, 180);
			this.tb_cur_tem.Name = "tb_cur_tem";
			this.tb_cur_tem.ReadOnly = true;
			this.tb_cur_tem.Size = new System.Drawing.Size(54, 20);
			this.tb_cur_tem.TabIndex = 46;
			this.tb_cur_tem.Text = "???";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(458, 180);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(23, 17);
			this.label12.TabIndex = 45;
			this.label12.Text = "°C";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label16.Location = new System.Drawing.Point(263, 180);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(129, 17);
			this.label16.TabIndex = 44;
			this.label16.Text = "Temperatura atual:";
			// 
			// btn_test
			// 
			this.btn_test.Location = new System.Drawing.Point(33, 284);
			this.btn_test.Name = "btn_test";
			this.btn_test.Size = new System.Drawing.Size(195, 23);
			this.btn_test.TabIndex = 47;
			this.btn_test.Text = "teste";
			this.btn_test.UseVisualStyleBackColor = true;
			this.btn_test.Click += new System.EventHandler(this.btn_test_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(627, 352);
			this.Controls.Add(this.btn_test);
			this.Controls.Add(this.tb_cur_tem);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.lbl_status_m4);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.lbl_status_m3);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.lbl_status_m2);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.lbl_status_m1);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.nud_m4);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.nud_m3);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.nud_m2);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.nud_m1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tb_set_tem);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lbl_temp);
			this.Controls.Add(this.lbl_dType);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nud_dType);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "ModBus - Master";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nud_dType)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nud_m1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nud_m2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nud_m3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nud_m4)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem ms_SerialPort;
		private System.Windows.Forms.ToolStripMenuItem ms_sp_port;
		private System.Windows.Forms.ToolStripMenuItem ms_sp_baud;
		private System.Windows.Forms.ToolStripComboBox ms_sp_baud_combobox;
		private System.Windows.Forms.ToolStripComboBox ms_sp_port_combobox;
		private System.Windows.Forms.ToolStripMenuItem ms_sp_conect;
		private System.Windows.Forms.ToolStripMenuItem ms_sp_disconect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_dType;
        private System.Windows.Forms.NumericUpDown nud_dType;
        private System.Windows.Forms.Label lbl_temp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_set_tem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nud_m1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nud_m2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nud_m3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nud_m4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbl_status_m1;
		private System.Windows.Forms.Label lbl_status_m2;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label lbl_status_m3;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label lbl_status_m4;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox tb_cur_tem;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.ToolStripMenuItem ms_sp_parity;
		private System.Windows.Forms.ToolStripComboBox ms_sp_par_combobox;
		private System.Windows.Forms.ToolStripMenuItem ms_sp_stopBits;
		private System.Windows.Forms.ToolStripComboBox ms_sp_stop_combobox;
		private System.Windows.Forms.Button btn_test;
	}
}

