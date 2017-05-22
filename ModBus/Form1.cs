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
	public enum MemoryAddress : byte {
		Inputs				= 0x00,
		Coils				= 0x10,
		HoldingRegisters	= 0x18,
		Temp				= 0x18,
		SetPoint			= 0x20,
		Tms					= 0x22,
		Tm1					= 0x22,
		Tm2					= 0x23,
		Tm3					= 0x24,
		Tm4					= 0x25,
		IHM					= 0x26,
		IHM_l1				= 0x26,
		IHM_l2				= 0x36
	}

	public partial class Form1 : Form {
		ModBusPort modBusPort = new ModBusPort();
		System.Timers.Timer refreshTimer = new System.Timers.Timer();

		string folder = "%APPDATA%/ModBusPort/";	//pasta onde serão salvos os log do programa

		enum e_devices : byte { broadcast, kl25 };
		static string[] devices = { "broadcast", "kl25" };

		enum e_motorState : byte { desligado, estrela, triangulo };
		//byte[] motorState = { (byte)e_motorState.desligado, (byte)e_motorState.desligado, (byte)e_motorState.desligado, (byte)e_motorState.desligado };
		string[] s_motorState = { "desligado", "estrela", "triangulo", "est e tri" };
		bool[] motorUserComand = { false, false, false, false };
		byte B_motorState = 0x00;

		float temperatura;

		static int[] iBaudRate = { 9600, 14400, 19200, 38400, 57600, 115200, 128000, 256000 };
		//static string[] sBaudRate = { "9600", "14400", "19200", "38400", "57600", "115200", "128000", "256000" };

		static string[] stopBits = { "One", "Two" };    //see System.IO.Ports.StopBits	//None e OnePointFive nao suportados
		static string[] parity = { "None", "Odd", "Even", "Mark", "Space" };    //see System.IO.Ports.Parity

		static char[] teclas = { '1', '2', '3', 'A', '4', '5', '6', 'B', '7', '8', '9', 'C', '*', '0', '#', 'D' };
		static byte[] IHMmessage = new byte[32];
		int n=0;

		bool refreshMotorTime = true;
		bool refreshTemperature = true;

//		bool refreshLcdL1 = false;
//		bool refreshLcdL2 = false;
//		int lostMessagesCount = 0; //o contador deve ser interno da classe ModBusPort. Um arquivo de log deve ser salvo em %APPDATA%/ModBusPort/errors.log.txt

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
			tb_set_tem.TextChanged+=delegate {
				refreshTemperature=true;
			};

			btn_m1_on.Click+=delegate { motorUserComand[0]=true; };
			btn_m2_on.Click+=delegate { motorUserComand[1]=true; };
			btn_m3_on.Click+=delegate { motorUserComand[2]=true; };
			btn_m1_off.Click+=delegate { motorUserComand[0]=false; };
			btn_m2_off.Click+=delegate { motorUserComand[1]=false; };
			btn_m3_off.Click+=delegate { motorUserComand[2]=false; };

			refreshTimer.Elapsed+=delegate { RefreshRegisters(); };
			refreshTimer.Interval=10000; //2,5s planejar esse tempo depois

			//modBusPort.MessageReceived+=delegate { InterpretaMensagem(); }; //modBusPort.t.Elapsed+=delegate { InterpretaMensagem(); };
			modBusPort.MessageReceived+=InterpretaMensagem;
			modBusPort.MessageTimeOut+=InterpretaMensagemSemResposta;

			lbl_status_m1.Text=s_motorState[0];
			lbl_status_m2.Text=s_motorState[0];
			lbl_status_m3.Text=s_motorState[0];
			lbl_status_m4.Text=s_motorState[0];

/*			tb_LDC_l1.TextChanged+=LDC_TextChanged;
			tb_LDC_l2.TextChanged+=LDC_TextChanged;
			tb_LDC_l1.TextChanged+=delegate { refreshLcdL1=true; };
			tb_LDC_l2.TextChanged+=delegate { refreshLcdL2=true; };*/

			IHMmessage[0]=(byte)'t';
			IHMmessage[1]=(byte)'e';
			IHMmessage[2]=(byte)'c';
			IHMmessage[3]=(byte)'l';
			IHMmessage[4]=(byte)'a';
			IHMmessage[5]=(byte)' ';
			for(int i=0x06; i<0x10; i++) IHMmessage[i]=(byte)' ';

			IHMmessage[0x10]=(byte)'p';
			IHMmessage[0x11]=(byte)'r';
			IHMmessage[0x12]=(byte)'e';
			IHMmessage[0x13]=(byte)'c';
			IHMmessage[0x14]=(byte)'i';
			IHMmessage[0x15]=(byte)'o';
			IHMmessage[0x16]=(byte)'n';
			IHMmessage[0x17]=(byte)'a';
			IHMmessage[0x18]=(byte)'d';
			IHMmessage[0x19]=(byte)'a';
			for(int i = 0x1A; i<0x20; i++) IHMmessage[i]=(byte)' ';
		}

		private void InterpretaMensagem(object sender, MessageReceivedEventArgs e) {
			Message query = e.MessagePair.Key, answer = e.MessagePair.Value;

			if(query.GetDevice()==(byte)e_devices.kl25) {
				switch(query.GetMessageType()) {
					case (byte)MessageType.ReadNCoils:   //message 1
														 //byte 0:	*N = Quantity of Outputs / 8, if the remainder is different of 0 ⇒ N = N+1
						if(query.GetBody()[0]!=1) return;       //só é feita leitura de 8 bits
						B_motorState=query.GetBody()[1];        //contem a resposta dos 8 bits
						Invoke(new EventHandler(AtualizaTelaMotores));
						//talez colocar o codigo de ligar e desligar os motores aqui, logo apos ler o estado atual deles
						break;
					case (byte)MessageType.ReadNInputs:
						if(answer.GetBody()[0]!=2&&answer.GetBody().Count()!=3) return;       //só é feita leitura de 16 bits

						//bool[] kbbits = new bool[16];
						ushort kb = (ushort)(256*answer.GetBody()[1]+answer.GetBody()[2]);
						byte c=0;
						List<ushort> mes = new List<ushort>();
						for(ushort mask = 0x8000; mask>0; mask>>=1, c++) {
							if(( kb&mask )!=0) {
								//IHMmessage[6]=(byte)teclas[c];
								IHMmessage=makeMessage("tecla "+teclas[c], "precionada");

								foreach(byte b in IHMmessage) mes.Add((ushort)b);
								modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x26, mes));
								//modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x26, IHMmessage));
								Invoke(new EventHandler(AtualizaLcd));
								n=0;
								return;
							}
						}

						IHMmessage=makeMessage(""+( ++n ), "atualizacoes");
						foreach(byte b in IHMmessage) mes.Add((ushort)b);
						modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x26, mes));
						//modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x26, IHMmessage));
						Invoke(new EventHandler(AtualizaLcd));

						/*byte b;
						int c = 0;
						b=answer.GetBody()[1];
						for(byte mask = 0x01; mask<=0x80; mask<<=1) {
							if((b&mask)!=0){
								IHMmessage[6]=(byte)teclas[c];

								int j=c;

								modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x26, IHMmessage));
//								modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x36, IHMmessagel2));
								return;
							}else c++;
						}
						b=answer.GetBody()[2];
						for(byte mask = 0x01; mask<=0x80; mask<<=1) {
							if((b&mask)!=0){
								IHMmessage[6]=(byte)teclas[c];
								modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x26, IHMmessage));
//								modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x36, IHMmessagel2));
								return;
							}else c++;
						}*/
						break;
					/*case (byte)MessageType.WriteNHoldingRegisters:	//aqui nao precisa fazer nada	//drink a beer
						switch(answer.GetBody()[1]) {	
							case 0x20:  //set valor de temperatura ok
								break;
							case 0x22:  //set tempo de motores ok
								break;
						}
						break;*/
					case (byte)MessageType.ReadNHoldingRegisters:
						List<byte> bv = answer.GetBody();
						//if(bv[0]!=4) return;                    //se for a leitura de mais de 2 HoldingRegisters (um float 32 bits)
						if(query.GetBody()[1]==(byte)MemoryAddress.Temp) {  // leitura da temperatura	//adrress
							if(bv.Count!=5) return;                 //verificar //1 para o numero de bytes e 4 para o float
							if(bv[0]!=4) return;					//byte count
							try {
								byte[] temp = new byte[4];
								temp[3]=bv[1];
								temp[2]=bv[2];
								temp[1]=bv[3];
								temp[0]=bv[4];
								temperatura=BitConverter.ToSingle(temp, 0);
								Invoke(new EventHandler(AtualizaTelaTemperatura));
							} catch(Exception) { }
						}
						//if(query.GetBody()[1]==(byte)MemoryAddress.Tms){			////////////////////////////////////////////////////////////////////
						//}
						break;
					//testar o enio antes de habilitar a funcoes de erro
					/*case 128+(byte)MessageType.ReadNCoils:	//erro
						break;
					case 128+(byte)MessageType.WriteNHoldingRegisters:  //aqui nao precisa fazer nada	//drink a beer
						switch(answer.GetBody()[1]) {
							case 0x20:  //erro ao setar valor de temperatura
								refreshTemperature=true;
								break;
							case 0x22:  //erro ao setar tempo de motores
								refreshMotorTime=true;
								break;
						}
						break;*/
				}
			}
		}

		byte[] makeMessage(string l1, string l2) {
			byte[] mes = new byte[32];
			for(int i = 0; i<l1.Length; i++) mes[i]=(byte)l1[i];
			for(int i = l1.Length; i<16; i++) mes[i]=(byte)' ';
			for(int i = 0; i<l2.Length; i++) mes[i+16]=(byte)l2[i];
			for(int i = l2.Length; i<16; i++) mes[i+16]=(byte)' ';
			return mes;
		}

		private void AtualizaTelaMotores(object sender, EventArgs e) {
			byte b = B_motorState;
			byte mask = 0x03;
			byte aux = (byte)(b&mask);
			lbl_status_m1.Text=s_motorState[aux];

			mask<<=2;
			aux=(byte)(b&mask);
			aux>>=2;
			lbl_status_m2.Text=s_motorState[aux];

			mask<<=2;
			aux=(byte)(b&mask);
			aux>>=4;
			lbl_status_m3.Text=s_motorState[aux];

			mask<<=2;
			aux=(byte)(b&mask);
			aux>>=6;
			lbl_status_m4.Text=s_motorState[aux];
		}
		private void AtualizaTelaTemperatura(object sender, EventArgs e) {
			tb_cur_tem.Text=temperatura.ToString();
		}
		private void AtualizaLcd(object sender, EventArgs e) {
			tb_LDC_l1.Clear();
			for(byte b = 0; b<16; b++) tb_LDC_l1.AppendText( ""+(char)IHMmessage[b] );// ($"{IHMmessage[b]:x2}");
			tb_LDC_l2.Clear();
			for(byte b = 16; b<32; b++) tb_LDC_l2.AppendText(""+(char)IHMmessage[b] );// ($"{IHMmessage[b]:x2}");
		}

		private void InterpretaMensagemSemResposta(object sender, MessageTimeOutEventArgs e) {  //so para teste por enquanto
			MessageBox.Show("Erro ao transmitir a mensagem "+e.SendMessage.GetMessageType().ToString()+" verifique a comunicação", "Erro de transmição");
		}

		public void RefreshRegisters() {
			if(modBusPort.WaitingMessage) return;	// evita sobrecarga no buffer da porta ModBus
			if(refreshMotorTime) { //write in 4 hold register
				List<ushort> temp = new List<ushort>();
				temp.Add((ushort)nud_m1.Value);
				temp.Add((ushort)nud_m2.Value);
				temp.Add((ushort)nud_m3.Value);
				temp.Add((ushort)nud_m4.Value);
				modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x22, temp));
				refreshMotorTime=false;
			}
			if(refreshTemperature) { //write in 2 hold register
				try {
					float ftemp = float.Parse(tb_set_tem.Text.ToString());
					byte[] vec = BitConverter.GetBytes(ftemp);
					Array.Reverse(vec);
					modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, (int)MemoryAddress.SetPoint, vec));
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
			modBusPort.EscreverMensagem(Message.ReadNInputs(1, 0x00, 16));              //le o teclado
			//modBusPort.EscreverMensagem(Message.ReadNCoils(1, 0x10, 8));				//le o estado dos motores

			//talvez torcar a parte de ligar e desligar para a funcao InterpretaMensagem,
			//depois de receber a resposta do estado dos motores, dai só troca os motores que realmente precisa trocar
			if(motorUserComand[0] &&lbl_status_m1.Text==s_motorState[0]) {
				//ligar motor 1
			}else if(!motorUserComand[0]&&lbl_status_m1.Text!=s_motorState[0]) {
				//desligar motor 1
			}
			if(motorUserComand[1]&&lbl_status_m2.Text==s_motorState[0]) {
				//ligar motor 1
			} else if(!motorUserComand[1]&&lbl_status_m2.Text!=s_motorState[0]) {
				//desligar motor 1
			}
			if(motorUserComand[2]&&lbl_status_m3.Text==s_motorState[0]) {
				//ligar motor 1
			} else if(!motorUserComand[2]&&lbl_status_m3.Text!=s_motorState[0]) {
				//desligar motor 1
			}

			//LCD Change
/*			if(refreshLcdL1) {
				char[] l1 = tb_LDC_l1.Text.ToArray();
				char[] l2 = new char[16];
				
				for(int i = 0; i<l1.Length; i++) l2[i]=l1[i];
				for(int i = l1.Length; i<16; i++) l2[i]='0';

				List<ushort> l3 = new List<ushort>();
				for(int i = 0; i<16; i+=2) l3.Add((ushort)(256*l2[i]+l2[i+1]));

				modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x26, l3));
			}
			if(refreshLcdL2) {
				char[] l1 = tb_LDC_l2.Text.ToArray();
				char[] l2 = new char[16];

				for(int i = 0; i<l1.Length; i++) l2[i]=l1[i];
				for(int i = l1.Length; i<16; i++) l2[i]='0';

				List<ushort> l3 = new List<ushort>();
				for(int i = 0; i<16; i+=2) l3.Add((ushort)(256*l2[i]+l2[i+1]));

				modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x36, l3));
			}*/
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
				//modBusPort.EscreverMensagem(Message.ReadNHoldingRegisters(1, (ushort)MemoryAddress.Tms, 4));	//temperatura
				//modBusPort.EscreverMensagem(Message.ReadNHoldingRegisters(1, 0x18, 2));	//temperatura
				//modBusPort.EscreverMensagem(Message.ReadNCoils(1, 0x00, 8));				//8leds
				//modBusPort.EscreverMensagem(Message.ReadNInputs(1, 0x00, 16));			//le o teclado

				//float ftemp = float.Parse(tb_set_tem.Text.ToString());
				//byte[] vec = BitConverter.GetBytes(ftemp);
				//Array.Reverse(vec);
				//modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, (int)MemoryAdrress.SetPoint, vec));

				bool[] coils = {false, false, true, true, false, false};
				List<bool> Coils = new List<bool>();
				Coils.AddRange(coils);
				modBusPort.EscreverMensagem(Message.WriteNCoils(1, (ushort)MemoryAddress.Coils, Coils));
			} catch(Exception) { }
		}
		
		private void LDC_TextChanged(object sender, EventArgs e) {
			TextBox t = (TextBox)sender;
			if(t.Text.Length>16) {
				t.Text = t.Text.Remove(16);		//remove todos os caracteres depois do decimo sexto
			}
		}
	}
}
