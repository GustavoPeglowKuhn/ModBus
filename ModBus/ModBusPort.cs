using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModBus {
	delegate void MessageReceivedEventHandler(object source, EventArgs e);

	class ModBusPort : System.IO.Ports.SerialPort {
		public event MessageReceivedEventHandler MessageReceived;
		
		System.Timers.Timer char3_5 = new System.Timers.Timer();
		List<byte> buffer = new List<byte>();
		bool waitingMessage = false;
		Queue<Message> writeBuffer = new Queue<Message>(); //coloca as mensagens na fila caso esteja esperando a resposta de um servo
		Queue<KeyValuePair<Message, Message>> readBuffer = new Queue<KeyValuePair<Message, Message>>();  //coloca as mensagens na fila até o usuario ler

		public ModBusPort() {
			char3_5.Enabled=false;
			Parity=System.IO.Ports.Parity.None;
			StopBits=System.IO.Ports.StopBits.One;

			DataReceived+=delegate {
				char3_5.Enabled=false;
				byte[] smallBuffer = new byte[BytesToRead];
				Read(smallBuffer, 0, BytesToRead);
				buffer.AddRange(smallBuffer);
				if(buffer.Count()>255) {
					buffer.Clear();
					DiscardInBuffer();
					return;
					//throw new ModBusFrameOverFlow("more than 256 byte per frame");	//vai retornar para quem?????
				}
				//buffer.Add((byte)ReadByte());
								
				char3_5.Enabled=true; //search a beter way to clear the timer count//
			};

			char3_5.Elapsed+=delegate {	//after 3.5 char
				char3_5.Enabled=false;
				//waitingMessage=false;		//muda dentro do metodo ReadMessage(), somente apos ler o conteudo do bufer e descartar o mesmo
				ReadMesssage();
			};
		}

		/*protected virtual void OnMessageReceived(EventArgs e) {
			MessageReceived?.Invoke(this, e);	//just past this line to call
		}*/

		public new void Open() {
				char3_5.Interval=timeChar3_5();
				base.Open();
				DiscardInBuffer();
			}

		public new void Close() {
			char3_5.Enabled=false;
			base.Close();		//"base" keyword to call parent method Close
			buffer.Clear();
			waitingMessage=false;
		}

		public void EscreverMensagem(Message message) {
			writeBuffer.Enqueue(message);
			if(writeBuffer.Count == 1 && waitingMessage ==false) {
				try { SendMesssage(); } catch(ModBusException) { }
			}
		}

		public KeyValuePair<Message, Message> LerMensagem() {
			if(readBuffer.Count()==0)
				throw new ModBusException("nothing to read");
			return readBuffer.Dequeue();
		}

		private float timeChar3_5() {
			float bpc;
			if(StopBits==System.IO.Ports.StopBits.None) bpc=0;
			else if(StopBits==System.IO.Ports.StopBits.One) bpc=1;
			else if(StopBits==System.IO.Ports.StopBits.Two) bpc=2;
			else bpc=1.5f;
			if(Parity!=System.IO.Ports.Parity.None) bpc+=1;		//talvez de erro! verificar se None significa sem bit de paridade ou sem testar o bit de paridade
			bpc+=9;		//8 bits + start_bit
			return 3500f * bpc/BaudRate;	//em milisegundos para o timer
		}

		private void SendMesssage() {
			//if(waitingMessage)			throw new ModBusException("busy");
			if(waitingMessage) return;	//ModBusPort is busy now
			Message message = writeBuffer.Last();
			if(message.GetDevice()!=0)	waitingMessage=true;
			Write(message.GetMessage().ToArray(), 0, message.GetMessage().Count);
		}

		private void ReadMesssage() {	//par pergunta/resposta
			int i = buffer.Count();
			if(i>4) {  //ver o tamanho da menor mensagem valida (provavelmente eh 6)
				try {
					//Message res = ;  //can throw CrcError exception
					readBuffer.Enqueue(new KeyValuePair<Message, Message>(writeBuffer.Dequeue(), new Message(buffer)));
					//OnMessageReceived(new EventArgs());
					MessageReceived?.Invoke(this, new EventArgs());		//caso chege nesta linha o CRC está certo	// lembrando que o Codigo do CRC ainda nao esta certo
				} catch(CrcError) { }	//pode ser jogada uma excecao pelo "Message(byte[])"
			}
			buffer.Clear();
			waitingMessage=false;
			//talvez tornar o SendMesssage assincrono e chamalo antes de disparar o evento, ou nao
			if(writeBuffer.Count > 0)	//se tiver mensagem para ser enviada ja envia
				SendMesssage();
		}
	}

	class ModBusException : Exception {
		public ModBusException() {
		}

		public ModBusException(string message) : base(message){
		}

		public ModBusException(string message, Exception inner) : base(message, inner){
		}
	}
	class ModBusReceiveTimeOut : Exception {
		public int device, mes_num;
		public ModBusReceiveTimeOut(int device=-1, int mes_num = -1) {
			this.device=device;
			this.mes_num=mes_num;
		}

		public ModBusReceiveTimeOut(string message, int device = -1, int mes_num = -1) : base(message) {
			this.device=device;
			this.mes_num=mes_num;
		}

		public ModBusReceiveTimeOut(string message, Exception inner, int device = -1, int mes_num = -1) : base(message, inner) {
			this.device=device;
			this.mes_num=mes_num;
		}
	}
	class ModBusFrameOverFlow : Exception {
		public ModBusFrameOverFlow() {
		}

		public ModBusFrameOverFlow(string message) : base(message){
		}

		public ModBusFrameOverFlow(string message, Exception inner) : base(message, inner){
		}
	}
}
