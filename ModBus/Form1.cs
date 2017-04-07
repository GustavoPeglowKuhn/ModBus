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
		System.Timers.Timer refreshTimer = new System.Timers.Timer();

		//Motor[] motors = new Motor[3];		// o quarto motor é controlado pelo kit (assim espero)
		//a classe Motor foi pensada para o supervisorio fica rmonitorando o motor e trocar de estrela para triangulo

		enum e_devices : byte { broadcast, kl25 };
		static string[] devices = { "broadcast", "kl25" };

		enum e_motorState : byte { desligado, estrela, triangulo };
		byte[] motorState = { (byte)e_motorState.desligado, (byte)e_motorState.desligado, (byte)e_motorState.desligado, (byte)e_motorState.desligado };
		string[] s_motorState = { "desligado", "estrela", "triangulo" };

		//enum BaudRate { asd, assd};
		static int[] iBaudRate = { 9600, 14400, 19200, 38400, 57600, 115200, 128000, 256000 };
		//static string[] sBaudRate = { "9600", "14400", "19200", "38400", "57600", "115200", "128000", "256000" };

		static string[] stopBits = { "One", "Two" };    //see System.IO.Ports.StopBits	//None e OnePointFive nao suportados
		static string[] parity = { "None", "Odd", "Even", "Mark", "Space" };    //see System.IO.Ports.Parity

		//bool[] refreshMotorTime = {true, true, true, true};
		bool[] MotorState = { false, false, false, false };
		bool refreshMotorTime = true;
		bool refreshTemperature = true;

		public Form1() {
			InitializeComponent();

			foreach (int i in iBaudRate)
				ms_sp_baud_combobox.Items.Add(i.ToString());
			//ms_sp_baud_combobox.Items.AddRange(sBaudRate);
			lbl_dType.Text = "kl25";

			ms_sp_stop_combobox.Items.AddRange(stopBits);
			ms_sp_par_combobox.Items.AddRange(parity);

			/*nud_m1.ValueChanged+=delegate { refreshMotorTime[0]=true; };
			nud_m2.ValueChanged+=delegate { refreshMotorTime[1]=true; };
			nud_m3.ValueChanged+=delegate { refreshMotorTime[2]=true; };
			nud_m4.ValueChanged+=delegate { refreshMotorTime[3]=true; };*/
			nud_m1.ValueChanged+=delegate { refreshMotorTime=true; };
			nud_m2.ValueChanged+=delegate { refreshMotorTime=true; };
			nud_m3.ValueChanged+=delegate { refreshMotorTime=true; };
			nud_m4.ValueChanged+=delegate { refreshMotorTime=true; };
			tb_set_tem.TextChanged+=delegate { refreshTemperature=true; };

			btn_m1_on.Click+=delegate { MotorState[0]=true; };
			btn_m2_on.Click+=delegate { MotorState[1]=true; };
			btn_m3_on.Click+=delegate { MotorState[2]=true; };
			btn_m1_off.Click+=delegate { MotorState[0]=false; };
			btn_m2_off.Click+=delegate { MotorState[1]=false; };
			btn_m3_off.Click+=delegate { MotorState[2]=false; };

			//modBusPort.MessageReceived+=delegate { InterpretaMensagem(); }; //modBusPort.t.Elapsed+=delegate { InterpretaMensagem(); };
			refreshTimer.Elapsed+=delegate { RefreshRegisters(); };
			refreshTimer.Interval=2500; //2,5s planejar esse tempo depois

			modBusPort.MessageReceived+=InterpretaMensagem;
			modBusPort.MessageTimeOut+=InterpretaMensagemSemResposta;
		}

		/*public void InterpretaMensagem() {
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
		}*/

		private void InterpretaMensagem(object sender, MessageReceivedEventArgs e) {
			KeyValuePair<Message, Message> par;
			par=e.MessagePair;
			if(par.Value.GetDevice()==(byte)e_devices.kl25) {
				switch(par.Value.GetMessageType()) {
					case (byte)MessageType.ReadNCoils:   //message 1
						//code here;
					break;
						case (byte)MessageType.WriteNHoldingRegisters:
							switch(par.Key.GetBody()[1]){
								case 0x20:	//set valor de temperatura ok
								break;
								case 0x22:	//set tempo de motores ok
								break;
							}
					break;
					case (byte)MessageType.ReadNHoldingRegisters:
						List<byte> bv = par.Value.GetMessage();
						float temperatura = 0;
						//temperatura =  bv[1] e[2];
						tb_cur_tem.Text=temperatura.ToString();
					break;
				}
			}
		}

		private void InterpretaMensagemSemResposta(object sender, MessageTimeOutEventArgs e) {  //so para teste por enquanto
			MessageBox.Show(e.SendMessage.GetMessageType().ToString(),"hello");
		}

		public void RefreshRegisters() {
			if(refreshMotorTime) { //write in hold register
				List<ushort> temp = new List<ushort>();
				temp.Add((ushort)nud_m1.Value);
				temp.Add((ushort)nud_m1.Value);
				temp.Add((ushort)nud_m1.Value);
				temp.Add((ushort)nud_m1.Value);
				modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x22, temp ));
				refreshMotorTime =false;
			}
			if(refreshTemperature) { /*write in hold register*/
				try {
					float ftemp = float.Parse(tb_set_tem.Text.ToString());
					List<ushort> temp = new List<ushort>();
					byte[] vec = BitConverter.GetBytes(ftemp);
					//temp.AddRange(vec);

					modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x22, temp));
				} catch(Exception) {
					MessageBox.Show("Temperatura invalida");
				}
				refreshTemperature=false;
			}
			//modBusPort.EscreverMensagem(Message.ReadNHoldingRegisters(1, 0x18, ));
			modBusPort.EscreverMensagem(Message.ReadNCoils(1, 0, 16));	//ver se é 8 ou 16
			if(MotorState[0] &&lbl_status_m1.Text==s_motorState[0]) {
				//ligar motor 1
			}else if(!MotorState[0]&&lbl_status_m1.Text!=s_motorState[0]) {
				//desligar motor 1
			}
			if(MotorState[1]&&lbl_status_m2.Text==s_motorState[0]) {
				//ligar motor 1
			} else if(!MotorState[1]&&lbl_status_m2.Text!=s_motorState[0]) {
				//desligar motor 1
			}
			if(MotorState[2]&&lbl_status_m3.Text==s_motorState[0]) {
				//ligar motor 1
			} else if(!MotorState[2]&&lbl_status_m3.Text!=s_motorState[0]) {
				//desligar motor 1
			}
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

		private void ms_sp_stop_combobox_SelectedIndexChanged(object sender, EventArgs e) {
			modBusPort.StopBits=(System.IO.Ports.StopBits)ms_sp_stop_combobox.SelectedIndex + 1;
		}
		private void ms_sp_par_combobox_SelectedIndexChanged(object sender, EventArgs e) {
			modBusPort.Parity=(System.IO.Ports.Parity)ms_sp_par_combobox.SelectedIndex;
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

		private void btn_test_Click(object sender, EventArgs e) {   //so para teste
			try {
				//Message.WriteNHoldingRegisters(1, 0x22, new List<ushort>());
				modBusPort.EscreverMensagem(Message.ReadNCoils(01, 00, 8));
			} catch(Exception) { }
		}
	}
}
