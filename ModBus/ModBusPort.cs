using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModBus {
	class ModBusPort : System.IO.Ports.SerialPort {
		public System.Timers.Timer t;
		List<byte> buffer;
		bool waitingMessage = false;

		//Queue<byte> fila = new Queue<byte>();	//coloca as mensagens na fila caso esteja esperando a resposta de um servo

		public ModBusPort() {
			buffer=new List<byte>();
			t =new System.Timers.Timer();
			t.Enabled=false;

			Parity=System.IO.Ports.Parity.None;
			StopBits=System.IO.Ports.StopBits.One;

			DataReceived+=delegate {
				byte[] smallBuffer = new byte[BytesToRead];
				Read(smallBuffer, 0, BytesToRead);
				buffer.AddRange(smallBuffer);
				if(buffer.Count()>256) {
					buffer.Clear();
					DiscardInBuffer();
					throw new ModBusFrameOerFlow("more than 256 byte per frame");
				}
				//buffer.Add((byte)ReadByte());
				t.Enabled=false;				//search a way to clear timer count//
				t.Enabled=true;
			};

			t.Elapsed+=delegate {
				t.Enabled=false;
				waitingMessage=false;
			};
		}

		public new void Open() {
			t.Interval=timeChar3_5();
			base.Open();
			DiscardInBuffer();
		}

		public new void Close() {
			t.Enabled=false;
			base.Close();		//"basse" keyword to call parent method Close
			buffer.Clear();
			waitingMessage=false;
		}

		float timeChar3_5() {
			float bpc;
			if(StopBits==System.IO.Ports.StopBits.None) bpc=0;
			else if(StopBits==System.IO.Ports.StopBits.One) bpc=1;
			else if(StopBits==System.IO.Ports.StopBits.Two) bpc=2;
			else bpc=1.5f;
			if(Parity!=System.IO.Ports.Parity.None) bpc+=1;
			bpc+=9;		//8 bits + start_bit
			return 3500f * bpc/BaudRate;	//em milisegundos para o timer
		}

		public void SendMesssage(Message message) {
			if(waitingMessage)			throw new ModBusException("Wait slave!");
			if(message.GetDevice()!=0)	waitingMessage=true;
			Write(message.GetMessage().ToArray(), 0, message.GetMessage().Count);
		}

		public Message ReadMesssage() {
			int i = buffer.Count();
			if(i<4) {  //ver o tamanho da menor mensagem valida (provavelmente eh 6)
				int d = i==0 ? -1 : buffer[0];
				int m = i<2 ? -1 : buffer[1];
				DiscardInBuffer();
				buffer.Clear();		//se a mensagem é inalida todo conteudo do buffer deve ser descartado!
				throw new ModBusReceiveTimeOut("Incomplete message", d, m);
			}
			Message res = new Message(buffer);  //can throw CrcError exception
			buffer.Clear();
			waitingMessage=false;
			return res;
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

	class ModBusFrameOerFlow : Exception {
		public ModBusFrameOerFlow() {
		}

		public ModBusFrameOerFlow(string message) : base(message){
		}

		public ModBusFrameOerFlow(string message, Exception inner) : base(message, inner){
		}
	}
}
