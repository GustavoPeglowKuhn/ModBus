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
		ModBusPort modBusPort = new ModBusPort();
		System.Timers.Timer refreshTimer = new System.Timers.Timer();

		//Motor[] motors = new Motor[3];		// o quarto motor é controlado pelo kit (assim espero)
		//a classe Motor foi pensada para o supervisorio fica rmonitorando o motor e trocar de estrela para triangulo
		
		enum e_devices : byte { broadcast, kl25 };
		static string[] devices = { "broadcast", "kl25" };

		enum e_motorState : byte { desligado, estrela, triangulo };
		byte[] motorState = { (byte)e_motorState.desligado, (byte)e_motorState.desligado, (byte)e_motorState.desligado, (byte)e_motorState.desligado };

		enum mesages_num : byte { broadcast, set_coins, read_cois };	//search the real values

		//enum BaudRate { asd, assd};
		static int[] iBaudRate = { 9600, 14400, 19200, 38400, 57600, 115200, 128000, 256000 };
		//static string[] sBaudRate = { "9600", "14400", "19200", "38400", "57600", "115200", "128000", "256000" };


		bool[] refreshMotorTime = {true, true, true, true };
		bool refreshTemperature = true;

		public Form1() {
			InitializeComponent();

			foreach (int i in iBaudRate)
				ms_sp_baud_combobox.Items.Add(i.ToString());
			//ms_sp_baud_combobox.Items.AddRange(sBaudRate);
			lbl_dType.Text = "kl25";
			
			nud_m1.ValueChanged+=delegate { refreshMotorTime[0]=true; };
			nud_m2.ValueChanged+=delegate { refreshMotorTime[1]=true; };
			nud_m3.ValueChanged+=delegate { refreshMotorTime[2]=true; };
			nud_m4.ValueChanged+=delegate { refreshMotorTime[3]=true; };
			tb_set_tem.TextChanged+=delegate { refreshTemperature=true; };

			modBusPort.MessageReceived+=delegate { InterpretaMensagem(); }; //modBusPort.t.Elapsed+=delegate { InterpretaMensagem(); };
			refreshTimer.Elapsed+=delegate { RefreshRegisters(); };
			refreshTimer.Interval=2500; //2,5s planejar esse tempo depois
		}

		public void InterpretaMensagem() {
			KeyValuePair<Message, Message> par;
			par=modBusPort.LerMensagem();
			if(par.Value.GetDevice()==(byte)e_devices.kl25) {
				switch(par.Value.GetMessageType()) {
					case (byte)mesages_num.read_cois:	//message 1
						
					break;
					case (byte)Message.MessageType.broadcast:
						//code here too;
					break;
				}
			}
		}

		public void RefreshRegisters() {
			if(refreshMotorTime[0]) { /*write in hold register*/
				int time = (int)nud_m1.Value;
				/*send message*/
				refreshMotorTime[0]=false;
			}
			if(refreshMotorTime[1]) { /*write in hold register*/
				int time = (int)nud_m1.Value;
				/*send message*/
				refreshMotorTime[0]=false;
			}
			if(refreshMotorTime[2]) { /*write in hold register*/
				int time = (int)nud_m1.Value;
				/*send message*/
				refreshMotorTime[0]=false;
			}
			if(refreshMotorTime[3]) { /*write in hold register*/
				int time = (int)nud_m1.Value;
				/*send message*/
				refreshMotorTime[0]=false;
			}
			if(refreshTemperature) { /*write in hold register*/
				try {
					float temp = float.Parse(tb_set_tem.Text.ToString());
					/*send message*/
				} catch(Exception) {
					MessageBox.Show("Temperatura invalida");
				}
				refreshTemperature=false;
			}
			/*Ler a temperatura do lm35, o estado dos motores (desligado, estrela, triangulo), o teclado e o display(IHM)*/
		}

		private void ms_sp_port_combobox_DropDown(object sender, EventArgs e) {
			/*	Essa funcao e chamada toda ves que o usuario verifica as portas seriais disponiveis		*/
			ms_sp_port_combobox.Items.Clear();
			foreach(string port in System.IO.Ports.SerialPort.GetPortNames()) {
				ms_sp_port_combobox.Items.Add(port);
			}
		}

		private void ms_sp_port_combobox_SelectedIndexChanged(object sender, EventArgs e) {
			/*	Essa funcao e chamada toda ves que o usuario troca de porta serial		*/
			if(ms_sp_port_combobox.Items.Count==0) {
				MessageBox.Show("Conect any serial device!", "No serial ports found");
			}
			modBusPort.PortName=ms_sp_port_combobox.SelectedItem.ToString();
			//serialPort.PortName=ms_sp_port_combobox.SelectedItem.ToString();
			//tb_receive.Text="Debug - "+serialPort.PortName;
		}

		private void ms_sp_baud_combobox_SelectedIndexChanged(object sender, EventArgs e) {
			/*	Essa funcao e chamada toda ves que o usuario troca o baudRate		*/
			modBusPort.BaudRate=iBaudRate[ms_sp_baud_combobox.SelectedIndex];
			//serialPort.BaudRate = iBaudRate[ms_sp_baud_combobox.SelectedIndex];
			//tb_receive.Text="Debug - "+serialPort.BaudRate.ToString();
		}

		private void ms_sp_conect_Click(object sender, EventArgs e) {
			/*	Essa funcao e chamada toda ves que o usuario abre a porta serial		*/
			try {
				modBusPort.Open();
				refreshTimer.Enabled=true;
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
			/*	Essa funcao e chamada toda ves que o usuario fecha a porta serial		*/
			try {
				modBusPort.Close();
				refreshTimer.Enabled=false;
				//serialPort.Close();
				//timer1.Enabled=false;
				ms_sp_conect.Enabled=true;
				ms_sp_disconect.Enabled=false;
			} catch(Exception) {
				MessageBox.Show("Unable to disconect this serial port", "Disconect Error");
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			/*	antes de fechar o programa, fecha a porta serial caso ela esteja aberta		*/
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

		private void nud_dType_ValueChanged(object sender, EventArgs e) {
			if (nud_dType.Value == (int)e_devices.broadcast)
				lbl_dType.Text = devices[(int)e_devices.broadcast];
			else if (nud_dType.Value == (int)e_devices.kl25)
				lbl_dType.Text = devices[(int)e_devices.kl25];
			else
                lbl_dType.Text = "unknow";
		}
	}
}
