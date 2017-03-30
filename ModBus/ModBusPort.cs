using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModBus {
	//delegate void MessageReceivedEventHandler(object source, EventArgs e);
	delegate void MessageReceivedEventHandler(object source, MessageReceivedEventArgs e);
	//delegate void BadMessageReceivedEventHandler(object source, EventArgs e);
	delegate void MessageTimeOutEventHandler(object source, MessageTimeOutEventArgs e);

	class ModBusPort : System.IO.Ports.SerialPort {
		//
		// Summary:
		//     Invoke when a message is received from any slave.
		public event MessageReceivedEventHandler MessageReceived;
		//
		// Summary:
		//     Invoke if no message is received in time from the slave.
		public event MessageTimeOutEventHandler MessageTimeOut;

		//
		// Summary:
		//     The time of 3.5 char is used to indicate the end of the frame/message.
		System.Timers.Timer tmr_3_5char = new System.Timers.Timer();
		//
		// Summary:
		//     Indicate that the slave is ofline or don't received last message.
		System.Timers.Timer tmr_TimeOut = new System.Timers.Timer();    //use this to see if is waiting a message
		//
		// Summary:
		//     List of all bytes received by System.IO.Ports.SerialPort,
		//     the will be used to make a Message.
		List<byte> buffer = new List<byte>();
		//
		// Summary:
		//     Indicate that the master is waiting a message from any slave.
		bool waitingMessage = false;
		//
		// Summary:
		//     Queue of Messages to send after receive answer to the last quest.
		Queue<Message> writeBuffer = new Queue<Message>(); //coloca as mensagens na fila caso esteja esperando a resposta de um servo
		//Queue<KeyValuePair<Message, Message>> readBuffer = new Queue<KeyValuePair<Message, Message>>();  //coloca as mensagens na fila até o usuario ler

		/*public int TimeOutQueueMaxSize = 100;
		Queue<Message> TimeOutQueue = new Queue<Message>();*/			//mensagens que nao foram respondidas
		/*public int BadMessagesQueueMaxSize = 100;
		Queue<KeyValuePair<Message, Message>> BadMessagesQueue = new Queue<KeyValuePair<Message, Message>>();*/		//mensagens com erro, CRC ou dispositivo diferente

		public ModBusPort() {
			tmr_3_5char.Enabled=false;
			Parity=System.IO.Ports.Parity.None;
			StopBits=System.IO.Ports.StopBits.One;

			DataReceived+=delegate {
				tmr_TimeOut.Enabled=false;
				tmr_3_5char.Enabled=false;
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
								
				tmr_3_5char.Enabled=true; //search a beter way to clear the timer count//
			};
			tmr_3_5char.Elapsed+=delegate {	//after 3.5 char
				tmr_3_5char.Enabled=false;
				//waitingMessage=false;		//muda dentro do metodo ReadMessage(), somente apos ler o conteudo do bufer e descartar o mesmo
				ReadMesssage();
			};
			tmr_TimeOut.Elapsed+=delegate {
				tmr_TimeOut.Enabled=false;
				//TimeOutQueue.Enqueue(writeBuffer.Dequeue());
				//if(TimeOutQueue.Count>TimeOutQueueMaxSize)	TimeOutQueue.Dequeue();
				//MessageTimeOut?.Invoke(this, new EventArgs());
				Message mes = writeBuffer.Dequeue();    //para tirar a mensagem da fila, mesmo que MessageTimeOut nao seja invocado
				MessageTimeOut?.Invoke(this, new MessageTimeOutEventArgs(mes));
				buffer.Clear();
				waitingMessage=false;
				//if(writeBuffer.Count>0) //se tiver mensagem para ser enviada ja envia
				SendMesssage();
			};
		}

		/*protected virtual void OnMessageReceived(EventArgs e) {
			MessageReceived?.Invoke(this, e);	//just past this line to call
		}*/

		public new void Open() {
			float charTime = timeChar_ms();
			tmr_3_5char.Interval=charTime*3.5;	//3,5 caracteres, conforme o protocolo
			tmr_TimeOut.Interval=charTime*1000;	//planejar esse tempo depois
			base.Open();
			DiscardInBuffer();
		}
		public new void Close() {
			tmr_3_5char.Enabled=false;
			tmr_TimeOut.Enabled=false;
			base.Close();		//"base" keyword to call parent method Close
			buffer.Clear();
			waitingMessage=false;
		}

		//
		// Summary:
		//     Add a message to the writeBuffer of the ModBusPot object.
		//
		// Parameters:
		//   message:
		//     the message to be added in the queue.
		public void EscreverMensagem(Message message) {
			writeBuffer.Enqueue(message);
			SendMesssage();
			/*if(writeBuffer.Count == 1 && waitingMessage ==false) {
				try { SendMesssage(); } catch(ModBusException) { }
			}*/
		}
		/*public KeyValuePair<Message, Message> LerMensagem() {
			if(readBuffer.Count()==0)
				throw new ModBusException("nothing to read");
			return readBuffer.Dequeue();
		}*/

		//
		// Summary:
		//     Calculate the time in miliseconds to the
		//     System.IO.Ports.SerialPort send one byte.
		private float timeChar_ms() {
			float bpc;
			if(StopBits==System.IO.Ports.StopBits.None) bpc=0;
			else if(StopBits==System.IO.Ports.StopBits.One) bpc=1;
			else if(StopBits==System.IO.Ports.StopBits.Two) bpc=2;
			else bpc=1.5f;
			if(Parity!=System.IO.Ports.Parity.None) bpc+=1;		//talvez de erro! verificar se None significa sem bit de paridade ou sem testar o bit de paridade
			bpc+=9;		//8 bits + start_bit
			return 1000f * bpc/BaudRate;	//em milisegundos para o timer
		}
		//
		// Summary:
		//     Send a copy of the last message of the writeBuffer to the 
		//     System.IO.Ports.SerialPort.
		private void SendMesssage() {
			//if(waitingMessage)			throw new ModBusException("busy");
			if(waitingMessage) return;  //ModBusPort is busy now
			//if(tmr_TimeOut.Enabled) return;  //ModBusPort is busy now		//melhor usar waitingMessage pois ela só é trocada para false depois de limpar os buffers
			if(writeBuffer.Count==0) return;	//Nothing to write
			Message message = writeBuffer.Last();
			Write(message.GetMessage().ToArray(), 0, message.GetMessage().Count);
			if(message.GetDevice()!=0) {
				waitingMessage=true;
				tmr_TimeOut.Enabled=true;
			}
		}

		//
		// Summary:
		//     Read a message from System.IO.Ports.SerialPort input buffer
		//     and invoke the event MessageReseved with the message as arg.
		private void ReadMesssage() {   //par pergunta/resposta
			int i = buffer.Count();
			if(i>4) {  //ver o tamanho da menor mensagem valida (provavelmente eh 6)
				Message mes = null;
				try {
					mes=writeBuffer.Dequeue();
					KeyValuePair<Message, Message> messagePair = new KeyValuePair<Message, Message>(mes, new Message(buffer));
					MessageReceived?.Invoke(this, new MessageReceivedEventArgs(messagePair));
				} catch(CrcError) {//pode ser jogada uma excecao pelo "Message(byte[])"
					//BadMessagesQueue.Enqueue();
					//BadMessageReceived?.Invoke(this, new EventArgs());
				}
			}else {
				writeBuffer.Dequeue();	//remove a mensagem que foi respondida corretamente
										//pode ser colocado um flag na classe message para indicar se ela deve ser enviada novemente para o servo

				//pode ser disparado um evento para indicar o erro
			}
			buffer.Clear();
			DiscardInBuffer();
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

	class MessageTimeOutEventArgs : EventArgs {
		public Message SendMessage { get; }

		public MessageTimeOutEventArgs(Message message) {
			SendMessage=message;
		}
	}
	class MessageReceivedEventArgs : EventArgs {
		public KeyValuePair<Message, Message> MessagePair { get; }

		public MessageReceivedEventArgs(KeyValuePair<Message, Message> messagePair) {
			this.MessagePair=messagePair;
		}
	}
}
