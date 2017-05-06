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

		string folder = "%APPDATA%/ModBusPort/";	//pasta onde serão salvos os log do programa

		//Motor[] motors = new Motor[3];		// o quarto motor é controlado pelo kit (assim espero)
		//a classe Motor foi pensada para o supervisorio fica rmonitorando o motor e trocar de estrela para triangulo

		enum e_devices : byte { broadcast, kl25 };
		static string[] devices = { "broadcast", "kl25" };

		enum e_motorState : byte { desligado, estrela, triangulo };
		//byte[] motorState = { (byte)e_motorState.desligado, (byte)e_motorState.desligado, (byte)e_motorState.desligado, (byte)e_motorState.desligado };
		string[] s_motorState = { "desligado", "estrela", "triangulo" };
		bool[] b_motorState = { false, false, false, false };


		static int[] iBaudRate = { 9600, 14400, 19200, 38400, 57600, 115200, 128000, 256000 };
		//static string[] sBaudRate = { "9600", "14400", "19200", "38400", "57600", "115200", "128000", "256000" };

		static string[] stopBits = { "One", "Two" };    //see System.IO.Ports.StopBits	//None e OnePointFive nao suportados
		static string[] parity = { "None", "Odd", "Even", "Mark", "Space" };    //see System.IO.Ports.Parity
		
		bool refreshMotorTime = true;
		bool refreshTemperature = true;

		//int lostMessagesCount = 0; //o contador deve ser interno da classe ModBusPort. Um arquivo de log deve ser salvo em %APPDATA%/ModBusPort/errors.log.txt

		public Form1() {
			InitializeComponent();

			foreach (int i in iBaudRate)
				ms_sp_baud_combobox.Items.Add(i.ToString());
			//ms_sp_baud_combobox.Items.AddRange(sBaudRate);
			lbl_dType.Text = "kl25";

			ms_sp_stop_combobox.Items.AddRange(stopBits);
			ms_sp_par_combobox.Items.AddRange(parity);
						
			nud_m1.ValueChanged+=delegate { refreshMotorTime=true; };
			nud_m2.ValueChanged+=delegate { refreshMotorTime=true; };
			nud_m3.ValueChanged+=delegate { refreshMotorTime=true; };
			nud_m4.ValueChanged+=delegate { refreshMotorTime=true; };
			tb_set_tem.TextChanged+=delegate { refreshTemperature=true; };

			btn_m1_on.Click+=delegate { b_motorState[0]=true; };
			btn_m2_on.Click+=delegate { b_motorState[1]=true; };
			btn_m3_on.Click+=delegate { b_motorState[2]=true; };
			btn_m1_off.Click+=delegate { b_motorState[0]=false; };
			btn_m2_off.Click+=delegate { b_motorState[1]=false; };
			btn_m3_off.Click+=delegate { b_motorState[2]=false; };

			refreshTimer.Elapsed+=delegate { RefreshRegisters(); };
			refreshTimer.Interval=10000; //2,5s planejar esse tempo depois

			//modBusPort.MessageReceived+=delegate { InterpretaMensagem(); }; //modBusPort.t.Elapsed+=delegate { InterpretaMensagem(); };
			modBusPort.MessageReceived+=InterpretaMensagem;
			modBusPort.MessageTimeOut+=InterpretaMensagemSemResposta;

			lbl_status_m1.Text=s_motorState[0];
			lbl_status_m2.Text=s_motorState[0];
			lbl_status_m3.Text=s_motorState[0];
			lbl_status_m4.Text=s_motorState[0];
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
						if(par.Key.GetBody()[0]!=1) return;
						//BitConverter.


						//talez colocar o codigo de ligar e desligar os motores aqui, logo apos ler o estado atual deles
						break;
					case (byte)MessageType.WriteNHoldingRegisters:	//aqui nao precisa fazer nada	//drink a beer
						switch(par.Key.GetBody()[1]) {	
							case 0x20:  //set valor de temperatura ok
								break;
							case 0x22:  //set tempo de motores ok
								break;
						}
						break;
					case (byte)MessageType.ReadNHoldingRegisters:
						List<byte> bv = par.Value.GetMessage();
						if(bv[0]!=2) return;                    //se for a leitura de mais de 2 HoldingRegisters (um float 32 bits)
						if(par.Key.GetBody()[1]!=0x18) return;  // so é feita a leitura da temperatura	//adrress
						if(bv.Count!=6) return;					//verificar //2B para o numero de bytes e 4 para o float
						float temperatura = 0;
						byte[] temp = new byte[4];
						temp[3]=bv[1];
						temp[2]=bv[2];
						temp[1]=bv[3];
						temp[0]=bv[4];
						temperatura=BitConverter.ToSingle(temp, 2);
						tb_cur_tem.Text=temperatura.ToString();
						break;
				}
			}
		}

		private void InterpretaMensagemSemResposta(object sender, MessageTimeOutEventArgs e) {  //so para teste por enquanto
			MessageBox.Show("Erro ao transmitir a mensagem "+e.SendMessage.GetMessageType().ToString()+" verifique a comunicação", "Erro de transmição");
		}

		public void RefreshRegisters() {
			if(refreshMotorTime) { //write in 4 hold register
				List<ushort> temp = new List<ushort>();
				temp.Add((ushort)nud_m1.Value);
				temp.Add((ushort)nud_m1.Value);
				temp.Add((ushort)nud_m1.Value);
				temp.Add((ushort)nud_m1.Value);
				modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x22, temp));
				refreshMotorTime=false;
			}
			if(refreshTemperature) { //write in 2 hold register
				try {
					float ftemp = float.Parse(tb_set_tem.Text.ToString());
					byte[] vec = BitConverter.GetBytes(ftemp);
					Array.Reverse(vec);
					modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x20, vec));
					/*List<ushort> list = new List<ushort>();
					byte[] vec = BitConverter.GetBytes(ftemp);
					list.Add((ushort)(vec[1]*256+vec[0]));
					list.Add((ushort)(vec[3]*256+vec[2]));
					modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x20, list));*/
				} catch(Exception) {
					MessageBox.Show("Temperatura invalida");
				}
				refreshTemperature=false;
			}
			modBusPort.EscreverMensagem(Message.ReadNHoldingRegisters(1, 0x18, 2));		//le a temperatura do lm35
			modBusPort.EscreverMensagem(Message.ReadNInputs(1, 0x00, 16));				//le o teclado
			modBusPort.EscreverMensagem(Message.ReadNCoils(1, 0x10, 8));				//le o estado dos motores

			//talvez torcar a parte de ligar e desligar para a funcao InterpretaMensagem,
			//depois de receber a resposta do estado dos motores, dai só troca os motores que realmente precisa trocar
			if(b_motorState[0] &&lbl_status_m1.Text==s_motorState[0]) {
				//ligar motor 1
			}else if(!b_motorState[0]&&lbl_status_m1.Text!=s_motorState[0]) {
				//desligar motor 1
			}
			if(b_motorState[1]&&lbl_status_m2.Text==s_motorState[0]) {
				//ligar motor 1
			} else if(!b_motorState[1]&&lbl_status_m2.Text!=s_motorState[0]) {
				//desligar motor 1
			}
			if(b_motorState[2]&&lbl_status_m3.Text==s_motorState[0]) {
				//ligar motor 1
			} else if(!b_motorState[2]&&lbl_status_m3.Text!=s_motorState[0]) {
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
		//uncomment refreshTimer.Enabled=true;
		private void ms_sp_conect_Click(object sender, EventArgs e) {
			/*	Essa funcao e chamada toda ves que o usuario abre a porta serial		*/
			try {
				modBusPort.Open();
				//refreshTimer.Enabled=true;
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
				//modBusPort.EscreverMensagem(Message.ReadNHoldingRegisters(1, 0x18, 2));
				modBusPort.EscreverMensagem(Message.ReadNCoils(1, 0x00, 16));
			} catch(Exception) { }
		}
	}
}
