using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModBus {
	public partial class Form1 : Form {

		//enum BaudRate { asd, assd};
		static int[] iBaudRate = { 9600, 14400, 19200, 38400, 57600, 115200, 128000, 256000 };
		static string[] sBaudRate = { "9600", "14400", "19200", "38400", "57600", "115200", "128000", "256000" };
		

		public Form1() {
			InitializeComponent();

			ms_sp_baud_combobox.Items.AddRange(sBaudRate);
		}

		private void ms_sp_port_combobox_DropDown(object sender, EventArgs e) {
			ms_sp_port_combobox.Items.Clear();
			foreach(string port in System.IO.Ports.SerialPort.GetPortNames()) {
				ms_sp_port_combobox.Items.Add(port);
			}
		}

		private void ms_sp_port_combobox_SelectedIndexChanged(object sender, EventArgs e) {
			if(ms_sp_port_combobox.Items.Count==0) {
				MessageBox.Show("Conect any serial device!", "No serial ports found");
			}
			serialPort.PortName=ms_sp_port_combobox.SelectedItem.ToString();

			tb_receive.Text="Debug - "+serialPort.PortName;
		}

		private void ms_sp_baud_combobox_SelectedIndexChanged(object sender, EventArgs e) {
			serialPort.BaudRate = iBaudRate[ms_sp_baud_combobox.SelectedIndex];
			//tb_receive.Text="Debug - "+serialPort.BaudRate.ToString();
		}

		private void ms_sp_conect_Click(object sender, EventArgs e) {
			try {
				serialPort.Open();
				timer1.Enabled=true;
				ms_sp_conect.Enabled=false;
				ms_sp_disconect.Enabled=true;
			} catch(Exception ex) {
				MessageBox.Show("Unable to conect to "+serialPort.PortName+" at "+serialPort.BaudRate+" kbps, select a valid port", "Conect Error");
			}
		}

		private void ms_sp_disconect_Click(object sender, EventArgs e) {
			try {
				serialPort.Close();
				timer1.Enabled=false;
				ms_sp_conect.Enabled=true;
				ms_sp_disconect.Enabled=false;
			} catch(Exception ex) {
				MessageBox.Show("Unable to disconect this serial port", "Disconect Error");
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			if(serialPort.IsOpen) serialPort.Close();
		}

		private void btn_send_Click(object sender, EventArgs e) {
			if(tb_send.Text.Length==0) return;
			if(!serialPort.IsOpen) {
				MessageBox.Show("Serial port not conected", "Send Error");
				return;
			}
			serialPort.Write(tb_send.Text);
		}

		private void timer1_Tick(object sender, EventArgs e) {
			if(serialPort.BytesToRead>0){
				byte[] buffer = new byte[serialPort.BytesToRead];
				serialPort.Read(buffer, 0, serialPort.BytesToRead);
				//tb_receive.AppendText(buffer.ToArray().ToString());
				string s = "";
				foreach(char b in buffer)
					s+=b.ToString();
				tb_receive.AppendText(s);
				//tb_receive.AppendText(serialPort.ReadLine());
			}
		}
	}
}
