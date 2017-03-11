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
			this.components = new System.ComponentModel.Container();
			this.serialPort = new System.IO.Ports.SerialPort(this.components);
			this.tb_send = new System.Windows.Forms.TextBox();
			this.tb_receive = new System.Windows.Forms.TextBox();
			this.btn_send = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.ms_SerialPort = new System.Windows.Forms.ToolStripMenuItem();
			this.ms_sp_port = new System.Windows.Forms.ToolStripMenuItem();
			this.ms_sp_port_combobox = new System.Windows.Forms.ToolStripComboBox();
			this.ms_sp_baud = new System.Windows.Forms.ToolStripMenuItem();
			this.ms_sp_baud_combobox = new System.Windows.Forms.ToolStripComboBox();
			this.ms_sp_conect = new System.Windows.Forms.ToolStripMenuItem();
			this.ms_sp_disconect = new System.Windows.Forms.ToolStripMenuItem();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.nud_mType = new System.Windows.Forms.NumericUpDown();
			this.lbl_mType = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lbl_dType = new System.Windows.Forms.Label();
			this.nud_dType = new System.Windows.Forms.NumericUpDown();
			this.gb_1 = new System.Windows.Forms.GroupBox();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nud_mType)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nud_dType)).BeginInit();
			this.SuspendLayout();
			// 
			// tb_send
			// 
			this.tb_send.Location = new System.Drawing.Point(393, 320);
			this.tb_send.Name = "tb_send";
			this.tb_send.Size = new System.Drawing.Size(66, 20);
			this.tb_send.TabIndex = 0;
			// 
			// tb_receive
			// 
			this.tb_receive.Location = new System.Drawing.Point(542, 320);
			this.tb_receive.Name = "tb_receive";
			this.tb_receive.Size = new System.Drawing.Size(71, 20);
			this.tb_receive.TabIndex = 1;
			this.tb_receive.TextChanged += new System.EventHandler(this.tb_receive_TextChanged);
			// 
			// btn_send
			// 
			this.btn_send.Location = new System.Drawing.Point(465, 320);
			this.btn_send.Name = "btn_send";
			this.btn_send.Size = new System.Drawing.Size(71, 23);
			this.btn_send.TabIndex = 2;
			this.btn_send.Text = "Send";
			this.btn_send.UseVisualStyleBackColor = true;
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
			this.ms_sp_port.Size = new System.Drawing.Size(126, 22);
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
			this.ms_sp_baud.Size = new System.Drawing.Size(126, 22);
			this.ms_sp_baud.Text = "BaudRate";
			// 
			// ms_sp_baud_combobox
			// 
			this.ms_sp_baud_combobox.Name = "ms_sp_baud_combobox";
			this.ms_sp_baud_combobox.Size = new System.Drawing.Size(121, 23);
			this.ms_sp_baud_combobox.SelectedIndexChanged += new System.EventHandler(this.ms_sp_baud_combobox_SelectedIndexChanged);
			// 
			// ms_sp_conect
			// 
			this.ms_sp_conect.Name = "ms_sp_conect";
			this.ms_sp_conect.Size = new System.Drawing.Size(126, 22);
			this.ms_sp_conect.Text = "Conect";
			this.ms_sp_conect.Click += new System.EventHandler(this.ms_sp_conect_Click);
			// 
			// ms_sp_disconect
			// 
			this.ms_sp_disconect.Enabled = false;
			this.ms_sp_disconect.Name = "ms_sp_disconect";
			this.ms_sp_disconect.Size = new System.Drawing.Size(126, 22);
			this.ms_sp_disconect.Text = "Disconect";
			this.ms_sp_disconect.Click += new System.EventHandler(this.ms_sp_disconect_Click);
			// 
			// timer1
			// 
			this.timer1.Interval = 500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// nud_mType
			// 
			this.nud_mType.Location = new System.Drawing.Point(87, 27);
			this.nud_mType.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
			this.nud_mType.Name = "nud_mType";
			this.nud_mType.Size = new System.Drawing.Size(120, 20);
			this.nud_mType.TabIndex = 6;
			this.nud_mType.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nud_mType.ValueChanged += new System.EventHandler(this.nud_mType_ValueChanged);
			// 
			// lbl_mType
			// 
			this.lbl_mType.AutoSize = true;
			this.lbl_mType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl_mType.Location = new System.Drawing.Point(213, 27);
			this.lbl_mType.Name = "lbl_mType";
			this.lbl_mType.Size = new System.Drawing.Size(50, 17);
			this.lbl_mType.TabIndex = 7;
			this.lbl_mType.Text = "m type";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(357, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 17);
			this.label1.TabIndex = 9;
			this.label1.Text = "device:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(12, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(69, 17);
			this.label2.TabIndex = 10;
			this.label2.Text = "message:";
			// 
			// lbl_dType
			// 
			this.lbl_dType.AutoSize = true;
			this.lbl_dType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl_dType.Location = new System.Drawing.Point(542, 33);
			this.lbl_dType.Name = "lbl_dType";
			this.lbl_dType.Size = new System.Drawing.Size(49, 17);
			this.lbl_dType.TabIndex = 11;
			this.lbl_dType.Text = "device";
			// 
			// nud_dType
			// 
			this.nud_dType.Location = new System.Drawing.Point(416, 30);
			this.nud_dType.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nud_dType.Minimum = new decimal(new int[] {
            1,
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
			// gb_1
			// 
			this.gb_1.Location = new System.Drawing.Point(13, 56);
			this.gb_1.Name = "gb_1";
			this.gb_1.Size = new System.Drawing.Size(600, 163);
			this.gb_1.TabIndex = 12;
			this.gb_1.TabStop = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(627, 352);
			this.Controls.Add(this.gb_1);
			this.Controls.Add(this.lbl_dType);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nud_dType);
			this.Controls.Add(this.lbl_mType);
			this.Controls.Add(this.nud_mType);
			this.Controls.Add(this.btn_send);
			this.Controls.Add(this.tb_receive);
			this.Controls.Add(this.tb_send);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "ModBus - Master of Puppets";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nud_mType)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nud_dType)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.IO.Ports.SerialPort serialPort;
		private System.Windows.Forms.TextBox tb_send;
		private System.Windows.Forms.TextBox tb_receive;
		private System.Windows.Forms.Button btn_send;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem ms_SerialPort;
		private System.Windows.Forms.ToolStripMenuItem ms_sp_port;
		private System.Windows.Forms.ToolStripMenuItem ms_sp_baud;
		private System.Windows.Forms.ToolStripComboBox ms_sp_baud_combobox;
		private System.Windows.Forms.ToolStripComboBox ms_sp_port_combobox;
		private System.Windows.Forms.ToolStripMenuItem ms_sp_conect;
		private System.Windows.Forms.ToolStripMenuItem ms_sp_disconect;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NumericUpDown nud_mType;
        private System.Windows.Forms.Label lbl_mType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_dType;
        private System.Windows.Forms.NumericUpDown nud_dType;
        private System.Windows.Forms.GroupBox gb_1;
	}
}

