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
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tb_send
			// 
			this.tb_send.Location = new System.Drawing.Point(12, 27);
			this.tb_send.Name = "tb_send";
			this.tb_send.Size = new System.Drawing.Size(443, 20);
			this.tb_send.TabIndex = 0;
			// 
			// tb_receive
			// 
			this.tb_receive.Location = new System.Drawing.Point(12, 82);
			this.tb_receive.Multiline = true;
			this.tb_receive.Name = "tb_receive";
			this.tb_receive.Size = new System.Drawing.Size(443, 107);
			this.tb_receive.TabIndex = 1;
			// 
			// btn_send
			// 
			this.btn_send.Location = new System.Drawing.Point(12, 53);
			this.btn_send.Name = "btn_send";
			this.btn_send.Size = new System.Drawing.Size(443, 23);
			this.btn_send.TabIndex = 2;
			this.btn_send.Text = "Send";
			this.btn_send.UseVisualStyleBackColor = true;
			this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ms_SerialPort});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(467, 24);
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
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(467, 281);
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
	}
}

