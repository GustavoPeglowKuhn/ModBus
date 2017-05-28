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

		string folder = "%APPDATA%/ModBusPort/";    //pasta onde serão salvos os log do programa
		int ErrorCount = 0;

		enum e_devices : byte { broadcast, kl25 };
		static string[] devices = { "broadcast", "kl25" };

		enum e_motorState : byte { desligado=0, triangulo=0x01, estrela = 0x02 };       //estrela b10 e triangulo b01
		byte[] motorState = { (byte)e_motorState.desligado, (byte)e_motorState.desligado, (byte)e_motorState.desligado, (byte)e_motorState.desligado };
		string[] s_motorState = { "desligado", "triangulo", "estrela", "est e tri" };  //estrela b10 e triangulo b01
		bool[] motorUserComand = { false, false, false, false };
		//byte B_motorState = 0x00;
		//bool[] CoilsState = { false, false, false, false , false, false, false, false };	//8-bits

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

//		int lostMessagesCount = 0; //o contador deve ser interno da classe ModBusPort. Um arquivo de log deve ser salvo em %APPDATA%/ModBusPort/errors.log.txt

		public Form1() {
			InitializeComponent();

			foreach (int i in iBaudRate) ms_sp_baud_combobox.Items.Add(i.ToString());
			lbl_dType.Text = "kl25";

			ms_sp_stop_combobox.Items.AddRange(stopBits);
			ms_sp_par_combobox.Items.AddRange(parity);
			
			nud_m4.ValueChanged+=delegate { refreshMotorTime=true; };
			nud_setTemp.ValueChanged+=delegate { refreshTemperature=true; };

			btn_m1_on.Click+=delegate { motorUserComand[0]=true; };
			btn_m2_on.Click+=delegate { motorUserComand[1]=true; };
			btn_m3_on.Click+=delegate { motorUserComand[2]=true; };
			btn_m1_off.Click+=delegate { motorUserComand[0]=false; };
			btn_m2_off.Click+=delegate { motorUserComand[1]=false; };
			btn_m3_off.Click+=delegate { motorUserComand[2]=false; };

			refreshTimer.Elapsed+=delegate { RefreshRegisters(); };
			refreshTimer.Interval=750; //planejar esse tempo depois
			
			modBusPort.MessageReceived+=InterpretaMensagem;
			modBusPort.MessageTimeOut+=InterpretaMensagemSemResposta;

			lbl_status_m1.Text=s_motorState[0];
			lbl_status_m2.Text=s_motorState[0];
			lbl_status_m3.Text=s_motorState[0];
			lbl_status_m4.Text=s_motorState[0];

			IHMmessage=makeMessage("It isn't fun", "     any more");
		}

		private void InterpretaMensagem(object sender, MessageReceivedEventArgs e) {
			Message query = e.MessagePair.Key, answer = e.MessagePair.Value;
			List<byte> queryBody=query.GetBody(), answerBody=answer.GetBody();

			if(query.GetDevice()==(byte)e_devices.kl25) {
				switch(query.GetMessageType()) {
					case (byte)MessageType.ReadNCoils:   //message 1
														 //byte 0:	*N = Quantity of Outputs / 8, if the remainder is different of 0 ⇒ N = N+1
						if(answerBody[0]!=1) return;     //só é feita leitura de 2 bits
						if(queryBody[0]==0&&queryBody[1]==(int)MemoryAddress.Coils+6&&queryBody[2]==0x00&&queryBody[3]==0x02) {
							motorState[3]=(byte)( ( ( ( answerBody[1]&0x01 )!=0 ) ? 2 : 0 )+( ( ( answerBody[1]&0x02 )!=0 ) ? 1 : 0 ) );
							Invoke(new EventHandler(AtualizaTelaMotores));
						}
						break;
					case (byte)MessageType.ReadNInputs:
						if(answerBody[0]!=2&&answerBody.Count()!=3) return;       //só é feita leitura de 16 bits	// o teclado inteiro
						ushort kb = (ushort)(256*answerBody[1]+answerBody[2]);
						byte c=0;
						List<ushort> mes = new List<ushort>();
						for(ushort mask = 0x8000; mask>0; mask>>=1, c++) {
							if(( kb&mask )!=0) {
								IHMmessage=makeMessage("tecla "+teclas[c], "precionada");
								foreach(byte b in IHMmessage) mes.Add((ushort)b);
								modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x26, mes));
								Invoke(new EventHandler(AtualizaLcd));
								n=0;
								return;
							}
						}
						IHMmessage=makeMessage(""+( ++n ), "atualizacoes");
						foreach(byte b in IHMmessage) mes.Add((ushort)b);
						modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, 0x26, mes));
						Invoke(new EventHandler(AtualizaLcd));
						break;
					//case (byte)MessageType.WriteNHoldingRegisters:	//aqui nao precisa fazer nada	//drink a beer
					//	break;
					case (byte)MessageType.ReadNHoldingRegisters:
						if(queryBody[1]==(byte)MemoryAddress.Temp) {  // leitura da temperatura	//adrress
							if(answerBody.Count!=5) return;                 //verificar //1 para o numero de bytes e 4 para o float
							if(answerBody[0]!=4) return;					//byte count
							try {
								byte[] temp = new byte[4];
								temp[3]=answerBody[1];
								temp[2]=answerBody[2];
								temp[1]=answerBody[3];
								temp[0]=answerBody[4];
								temperatura=BitConverter.ToSingle(temp, 0);
								Invoke(new EventHandler(AtualizaTelaTemperatura));
							} catch(Exception) { }
						}else if(queryBody[1]==(byte)MemoryAddress.Tms) {
							ushort t;
							bool changeMotorState=false;
							ushort[] tn = new ushort[3];
							tn[0]=(ushort)nud_m1.Value;
							tn[1]=(ushort)nud_m2.Value;
							tn[2]=(ushort)nud_m3.Value;
							for(int i=0; i<3; i++) {
								t=(ushort)( 256*answerBody[1+2*i]+answerBody[0+2*i] );
								if(t>tn[i]&&motorState[i]==(byte)e_motorState.estrela) {           //troca para triangulo
									motorState[i]=(byte)e_motorState.triangulo;
									changeMotorState=true;
								}
							}
							if(changeMotorState) {
								List<bool> Coils = CoilsState();
								modBusPort.EscreverMensagem(Message.WriteNCoils(1, (ushort)MemoryAddress.Coils, Coils));
								Invoke(new EventHandler(AtualizaTelaMotores));
							}
						}
						break;
					case (byte)MessageType.WriteNCoils:
						if(answerBody[1]==(byte)MemoryAddress.Coils && answerBody[3] == 6) {
							/*byte mask=0x03;
							motorState[0]=(byte)( queryBody[5]&mask );
							mask<<=2;
							motorState[0]=(byte)( queryBody[5]&mask );
							mask<<=2;
							motorState[0]=(byte)( queryBody[5]&mask );*/
						}
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
			lbl_status_m1.Text=s_motorState[motorState[0]];
			lbl_status_m2.Text=s_motorState[motorState[1]];
			lbl_status_m3.Text=s_motorState[motorState[2]];
			lbl_status_m4.Text=s_motorState[motorState[3]];
		}
		private void AtualizaTelaTemperatura(object sender, EventArgs e) {
			tb_cur_tem.Text=temperatura.ToString();
		}
		private void AtualizaLcd(object sender, EventArgs e) {
			tb_LDC_l1.Clear();
			for(byte b = 0; b<16; b++)  tb_LDC_l1.AppendText(""+(char)IHMmessage[b]);
			tb_LDC_l2.Clear();
			for(byte b = 16; b<32; b++) tb_LDC_l2.AppendText(""+(char)IHMmessage[b]);
		}

		private void InterpretaMensagemSemResposta(object sender, MessageTimeOutEventArgs e) {  //so para teste por enquanto
			if(++ErrorCount>3) {
				ms_sp_disconect_Click(this, new EventArgs());
				MessageBox.Show("ModBusPort fechadas apos "+ErrorCount+" erros", "Erro de transmição");
			} else {
				MessageBox.Show("Erro ao transmitir a mensagem "+e.SendMessage.GetMessageType().ToString()+" verifique a comunicação", "Erro de transmição");
			}
		}

		public void RefreshRegisters() {
			if(modBusPort.WaitingMessage) return;									// evita sobrecarga no buffer da porta ModBus
			if(refreshMotorTime) {
				modBusPort.EscreverMensagem(Message.WriteSigleHoldingRegisters(1, (ushort)MemoryAddress.Tm4, (ushort)nud_m4.Value));
				refreshMotorTime=false;
			}
			if(refreshTemperature) {                                                //write in 2 hold register
				byte[] vec = BitConverter.GetBytes((float)nud_setTemp.Value);
				Array.Reverse(vec);
				modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, (int)MemoryAddress.SetPoint, vec));
				refreshTemperature=false;
			}
			modBusPort.EscreverMensagem(Message.ReadNHoldingRegisters(1, (ushort)MemoryAddress.Temp, 2));       //le a temperatura do lm35
			modBusPort.EscreverMensagem(Message.ReadNInputs(1, (ushort)MemoryAddress.Inputs, 16));              //le o teclado
			modBusPort.EscreverMensagem(Message.ReadNCoils(1, (ushort)MemoryAddress.Coils+6, 2));               //le o estado do quarto motor

			//////////////////////////////////////troca os  motores
			bool changeMotorState=false;
			for(int i = 0; i<3; i++) {
				if(motorUserComand[i]&&motorState[i]==(byte)e_motorState.desligado) {         //ligar motor
					motorState[i]=(byte)e_motorState.estrela;
					changeMotorState=true;
				} else if(!motorUserComand[i]&&motorState[i]!=(byte)e_motorState.desligado) { //desligar motor
					motorState[i]=(byte)e_motorState.desligado;
					changeMotorState=true;
				}
			}
			if(changeMotorState) {
				List<bool> Coils = CoilsState();
				modBusPort.EscreverMensagem(Message.WriteNCoils(1, (ushort)MemoryAddress.Coils, Coils));
				Invoke(new EventHandler(AtualizaTelaMotores));
			}

			if(motorState[0]==(byte)e_motorState.estrela || motorState[1]==(byte)e_motorState.estrela || motorState[2]==(byte)e_motorState.estrela)
				modBusPort.EscreverMensagem(Message.ReadNHoldingRegisters(1, (ushort)MemoryAddress.Tms, 3));	// ja le os 3 juntos, nao  faz tanta diferenca caso so precise de 1, mas economiza muito caso  precise dos 3
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
				refreshTimer.Enabled=true;
				ms_sp_conect.Enabled=false;
				ms_sp_disconect.Enabled=true;
				ErrorCount=0;
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

				////////////////////////////////////////troca o setPoint do slave
				//float ftemp = (float)nud_setTemp.Value;										//float.Parse(tb_set_tem.Text.ToString());
				//byte[] vec = BitConverter.GetBytes(ftemp);
				//Array.Reverse(vec);
				//modBusPort.EscreverMensagem(Message.WriteNHoldingRegisters(1, (ushort)MemoryAddress.SetPoint, vec));

				//////////////////////////////////////troca os  motores
				//bool changeMotorState=false;
				//for(int i = 0; i<3; i++) {
				//	if(motorUserComand[i]&&motorState[i]==(byte)e_motorState.desligado) {         //ligar motor
				//		motorState[i]=(byte)e_motorState.estrela;
				//		changeMotorState=true;
				//	} else if(!motorUserComand[i]&&motorState[i]!=(byte)e_motorState.desligado) { //desligar motor
				//		motorState[i]=(byte)e_motorState.desligado;
				//		changeMotorState=true;
				//	}
				//}
				//if(changeMotorState) {
				//	List<bool> Coils = CoilsState();
				//	modBusPort.EscreverMensagem(Message.WriteNCoils(1, (ushort)MemoryAddress.Coils, Coils));
				//	Invoke(new EventHandler(AtualizaTelaMotores));
				//}
				
				//if(	motorUserComand[0]&&motorState[0]==(byte)e_motorState.desligado||
				//	motorUserComand[1]&&motorState[1]==(byte)e_motorState.desligado||
				//	motorUserComand[2]&&motorState[2]==(byte)e_motorState.desligado){
				//	List<bool> Coils = CoilsState();
				//	modBusPort.EscreverMensagem(Message.WriteNCoils(1, (ushort)MemoryAddress.Coils, Coils));
				//}

				//////////////////////////////////////le o estado do quarto motor
				//modBusPort.EscreverMensagem(Message.ReadNCoils(1, (ushort)MemoryAddress.Coils+6, 2));
				//////////////////////////////////////escreve o tempo estrela do quarto motor
				//modBusPort.EscreverMensagem(Message.WriteSigleHoldingRegisters(1, (ushort)MemoryAddress.Tm4, (ushort)nud_m4.Value));
			} catch(Exception) { }
		}
		private void button1_Click(object sender, EventArgs e) {
			try {
				/////////////////////////////////verifica o tempo dos motores
				//modBusPort.EscreverMensagem(Message.ReadNHoldingRegisters(1, (ushort)MemoryAddress.Tm1, 3));
			} catch(Exception) { }
		}		

		List<bool> CoilsState() {
			List<bool> res = new List<bool>();
			res.Add(( motorState[0]&0x02 )!=0);
			res.Add(( motorState[0]&0x01 )!=0);
			res.Add(( motorState[1]&0x02 )!=0);
			res.Add(( motorState[1]&0x01 )!=0);
			res.Add(( motorState[2]&0x02 )!=0);
			res.Add(( motorState[2]&0x01 )!=0);
			return res;
		}
	}
}
