﻿using System;
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
		ModBusPort modBusPort = new ModBusPort();
		Motor[] motors = new Motor[3];		// o quarto motor é controlado pelo kit (assim espero)

		static string[] devices = { "broadcast", "kl25" };
		enum e_devices : byte { broadcast=0, kl25=1 };

		enum mesages_num : byte { broadcast, set_coins, read_cois };	//search the real values

		//enum BaudRate { asd, assd};
		static int[] iBaudRate = { 9600, 14400, 19200, 38400, 57600, 115200, 128000, 256000 };
		//static string[] sBaudRate = { "9600", "14400", "19200", "38400", "57600", "115200", "128000", "256000" };
		

		public Form1() {
			InitializeComponent();

			foreach (int i in iBaudRate)
				ms_sp_baud_combobox.Items.Add(i.ToString());
			//ms_sp_baud_combobox.Items.AddRange(sBaudRate);
			lbl_dType.Text = "kl25";

			modBusPort.t.Elapsed+=delegate {
				InterpretaMensagem();
			};

			//System.IO.Ports.SerialPort sp = new System.IO.Ports.SerialPort();			
		}

		public void InterpretaMensagem() {
			Message mes;
			try {
				mes = modBusPort.ReadMesssage();
			} catch(CrcError ex) {
				//CRC error
				//make a log of with time and message type
				return;
			} catch(ModBusReceiveTimeOut ex) {
				//message time out error
				//make a log of with time and message type
				return;
			}
			if(mes.GetDevice()==(byte)e_devices.kl25) {
				switch(mes.GetMessageType()) {
					case (byte)mesages_num.read_cois:
						//code here;
					break;
					case (byte)Message.MessageType.broadcast:
						//code here too;
					break;
				}
			}
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
			modBusPort.PortName=ms_sp_port_combobox.SelectedItem.ToString();
			//serialPort.PortName=ms_sp_port_combobox.SelectedItem.ToString();
			//tb_receive.Text="Debug - "+serialPort.PortName;
		}

		private void ms_sp_baud_combobox_SelectedIndexChanged(object sender, EventArgs e) {
			modBusPort.BaudRate=iBaudRate[ms_sp_baud_combobox.SelectedIndex];
			//serialPort.BaudRate = iBaudRate[ms_sp_baud_combobox.SelectedIndex];
			//tb_receive.Text="Debug - "+serialPort.BaudRate.ToString();
		}

		private void ms_sp_conect_Click(object sender, EventArgs e) {
			try {
				modBusPort.Open();
				//serialPort.Open();
				//timer1.Enabled=true;
				ms_sp_conect.Enabled=false;
				ms_sp_disconect.Enabled=true;
			} catch(Exception) {
				//MessageBox.Show("Unable to conect to "+serialPort.PortName+" at "+serialPort.BaudRate+" kbps, select a valid port", "Conect Error");
				MessageBox.Show("Unable to conect to "+modBusPort.PortName+" at "+modBusPort.BaudRate+" kbps, select a valid ModBusPort", "Conect Error");
			}
		}

		private void ms_sp_disconect_Click(object sender, EventArgs e) {
			try {
				modBusPort.Close();
				//serialPort.Close();
				timer1.Enabled=false;
				ms_sp_conect.Enabled=true;
				ms_sp_disconect.Enabled=false;
			} catch(Exception) {
				MessageBox.Show("Unable to disconect this serial port", "Disconect Error");
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			if(modBusPort.IsOpen) modBusPort.Close();
			//if(serialPort.IsOpen) serialPort.Close();
		}

		/*private void btn_send_Click(object sender, EventArgs e) {
			if(tb_send.Text.Length==0) return;
			if(!serialPort.IsOpen) {
				MessageBox.Show("Serial port not conected", "Send Error");
				return;
			}
			serialPort.Write(tb_send.Text);
		}*/

		private void timer1_Tick(object sender, EventArgs e) {
			/*if(serialPort.BytesToRead>0){
				byte[] buffer = new byte[serialPort.BytesToRead];
				serialPort.Read(buffer, 0, serialPort.BytesToRead);
				//tb_receive.AppendText(buffer.ToArray().ToString());
				string s = "";
				foreach(char b in buffer)
					s+=""+b;
				tb_receive.AppendText(s);
				//tb_receive.AppendText(serialPort.ReadLine());
			}*/
		}

		private void nud_dType_ValueChanged(object sender, EventArgs e) {
			if (nud_dType.Value == (int)e_devices.broadcast)
				lbl_dType.Text = devices[(int)e_devices.broadcast];
			else if (nud_dType.Value == (int)e_devices.kl25)
				lbl_dType.Text = devices[(int)e_devices.kl25];
			else
                lbl_dType.Text = "unknow";
		}

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }

        private void nud_m1_ValueChanged(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {

        }

        private void ms_SerialPort_Click(object sender, EventArgs e) {

        }
	}
}
