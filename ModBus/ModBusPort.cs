using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModBus {
	class ModBusPort : System.IO.Ports.SerialPort {
		public System.Timers.Timer t;
		List<byte> buffer;
		public ModBusPort() {
			buffer=new List<byte>();
			t =new System.Timers.Timer();
			t.Enabled=false;

			DataReceived+=delegate {
				buffer.Add((byte)ReadByte());
				t.Enabled=false;				//search a way to clear timer count//
				t.Enabled=true;
			};
		}

		public new void Open() {
			t.Interval=timeChar3_5();
			Open();
		}

		float timeChar3_5() {
			float bpc;
			if(StopBits==System.IO.Ports.StopBits.None) bpc=0;
			else if(StopBits==System.IO.Ports.StopBits.One) bpc=1;
			else if(StopBits==System.IO.Ports.StopBits.Two) bpc=2;
			else bpc=1.5f;
			if(Parity!=System.IO.Ports.Parity.None) bpc+=1;
			bpc+=8;
			return 3.5f * BaudRate/bpc;
		}

		public void SendMesssage(Message message) {
			Write(message.GetMessage().ToArray(), 0, message.GetMessage().Count);
		}

		public Message ReadMesssage() {
			Message res = new Message(buffer);  //can throw CrcError exception
			return res;
		}
	}
}
