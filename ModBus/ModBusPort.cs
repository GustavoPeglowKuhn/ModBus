using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModBus {
	class ModBusPort : System.IO.Ports.SerialPort {
		System.Timers.Timer timer;
		public ModBusPort() {
			timer=new System.Timers.Timer();
			timer.Enabled=false;

			/*float bpchar;
			if(StopBits==System.IO.Ports.StopBits.None) bpchar=0;
			else if(StopBits==System.IO.Ports.StopBits.One) bpchar=1;
			else if(StopBits==System.IO.Ports.StopBits.Two) bpchar=2;
			else bpchar=1.5f;
			if(Parity!=System.IO.Ports.Parity.None) bpchar+=1;
			bpchar+=8;
			float charTime = bpchar/BaudRate;	// * 3.5	//3.5char
			//timer.Interval = //BaudRate * constante
			*/
		}

		public void SendMesssage(Message message) {
			Write(message.GetMessage().ToArray(), 0, message.GetMessage().Count);
		}

		public Message ReadMesssage() {
			//read buffer;

			Message res = new Message(0, 0, new List<byte>());

			if(!res.CheckCrc()) throw new CrcError();

			return res;
		}
	}
}
