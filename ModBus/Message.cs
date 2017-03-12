﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModBus {
	class Message {
		private List<byte> message;

		public Message(byte device, byte mType, List<byte> body) {
			message=new List<byte>();
			message.Add(device);
			message.Add(mType);
			message.AddRange(body);
			byte[] crc = Crc.Calculate(message);    //Criar algoritimo do crc
			message.AddRange(crc);
		}

		public Message(byte device, byte mType, byte[] body) {
			message=new List<byte>();
			message.Add(device);
			message.Add(mType);
			message.AddRange(body);
			byte[] crc = Crc.Calculate(message);	//Criar algoritimo do crc
			message.AddRange(crc);
		}

		public byte GetDevice() {
			return message[0];
		}
		public byte GetMessageType() {
			return message[1];
		}
		public List<byte> GetBody() {
			return message.GetRange(2, message.Count-2);
		}

		public bool CheckCrc() {
			byte[] crc = Crc.Calculate(message.GetRange(0, message.Count-2));
			if(crc[0]==message[message.Count-1]&&crc[1]==message[message.Count])    //Criar algoritimo do crc
				return true;
			return false;
		}

		public List<byte> GetMessage() {
			return message;
		}
	}
}
