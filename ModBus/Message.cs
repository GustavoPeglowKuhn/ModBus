using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModBus {
	public class Message {
		//private byte tentativasDeEnvio = 1;	//pode ser ussado pela ModBusPort em caso de erro de transmição
		//se for utilizar isso, criar um enum com valores como BAIXA_PRIORIDADE = 1, ALTA_PRIORIDADE = 10 e outros
		//o valor deve ser passado no construtor da mensagem

		static byte[] bitMaskD = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };
		static byte[] bitMaskC = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x02, 0x04, 0x08 };

		private List<byte> message;

		public Message(byte device, byte mType, List<byte> body) {
			message=new List<byte>();
			message.Add(device);
			message.Add(mType);
			message.AddRange(body);
			byte[] crc = Crc.Calculate(message);    //Criar algoritimo do crc
													//message.AddRange(crc);
			message.Add(crc[1]);    //CRC LO
			message.Add(crc[0]);    //CRC HI

		}

		public Message(byte device, byte mType, byte[] body) {
			message=new List<byte>();
			message.Add(device);
			message.Add(mType);
			message.AddRange(body);
			byte[] crc = Crc.Calculate(message);    //Testar algoritimo do crc
													//			message.AddRange(crc);
			message.Add(crc[1]);    //CRC LO
			message.Add(crc[0]);    //CRC HI
		}

		public Message(byte[] all) {
			message.AddRange(all);  //ver se funciona como ponteiro
			if(!CheckCrc())
				throw new CrcError("CRC diferente");
		}

		public Message(List<byte> all) {
			//message = all;			//assim funciona como ponteiro (c++ é muito mais facil de er quando é ponteiro e quando nao é :/)
			message=new List<byte>();
			message.AddRange(all);
			if(!CheckCrc())
				throw new CrcError("CRC diferente");
		}

		public byte GetDevice() {
			return message[0];
		}
		public byte GetMessageType() {
			return message[1];
		}
		public List<byte> GetBody() {
			return message.GetRange(2, message.Count-4);    // sem dispositovo, tipo e CRC
		}

		public bool CheckCrc() {
			byte[] crc = Crc.Calculate(message.GetRange(0, message.Count-2));
			if(crc[1]==message[message.Count-2]&&crc[0]==message[message.Count-1])    //Criar algoritimo do crc
				return true;
			return false;
		}

		public List<byte> GetMessage() {
			return message;
		}

		/*Message 1 - Read n coils from slave*/
		static public Message ReadNCoils(byte dispositivo, ushort startingAddress, ushort nCoils) {
			if(nCoils>2000) throw new BadMessageException("More than 2000 Coins");   //limite do protocolo

			//<metodo 1>
			byte[] fisrt = BitConverter.GetBytes(startingAddress);
			byte[] num = BitConverter.GetBytes(nCoils);
			byte[] body = new byte[4];
			body[0]=fisrt[1];   //startingAddress HI
			body[1]=fisrt[0];   //startingAddress LO
			body[2]=num[1];     //nCoils		  HI
			body[3]=num[0];     //nCoils		  LO
								//</metodo 1>

			//<metodo 2>
			/*List<byte> b = new List<byte>();		//outra maneira
			b.AddRange(BitConverter.GetBytes(nbobinas));
			b.AddRange(BitConverter.GetBytes(startingAddress));
			b.Reverse();*/
			//</metodo 2>

			return new Message(dispositivo, (byte)MessageType.ReadNCoils, body);
		}

		/*Message 2 - Read n inputs from slave*/
		static public Message ReadNInputs(byte dispositivo, ushort startingAddress, ushort nInputs) {    //envia a mensagem 1
			if(nInputs>2000) throw new BadMessageException("More than 2000 Inputs"); ;   //limite do protocolo

			byte[] fisrt = BitConverter.GetBytes(startingAddress);
			byte[] num = BitConverter.GetBytes(nInputs);
			byte[] body = new byte[4];
			body[0]=fisrt[1];   //startingAddress HI
			body[1]=fisrt[0];   //startingAddress LO
			body[2]=num[1];     //nInputs		  HI
			body[3]=num[0];     //nInputs		  LO

			return new Message(dispositivo, (byte)MessageType.ReadNInputs, body);
		}

		/*Message 3 - Read n Holding Registers from slave*/
		static public Message ReadNHoldingRegisters(byte dispositivo, ushort startingAddress, ushort nRegisters) {    //envia a mensagem 1
			if(nRegisters>125) throw new BadMessageException("More than 125 Registers");   //limite do protocolo, exede o tamanho do frame

			byte[] fisrt = BitConverter.GetBytes(startingAddress);
			//byte[] num = BitConverter.GetBytes(nRegisters);
			byte[] body = new byte[4];
			body[0]=fisrt[1];   //startingAddress HI
			body[1]=fisrt[0];   //startingAddress LO
			body[2]=0;          //ever 0
			body[3]=(byte)nRegisters;            //num[0];     //nRegisters	  LO

			return new Message(dispositivo, (byte)MessageType.ReadNHoldingRegisters, body);
		}

		/*Message 5 - Write a single coils*/
		static public Message WriteSigleCoil(byte dispositivo, ushort CoilAddress, bool coilValue) {
			byte[] fisrt = BitConverter.GetBytes(CoilAddress);
			byte[] body = new byte[4];
			body[0]=fisrt[1];   //CoilAddress	HI
			body[1]=fisrt[0];   //CoilAddress	LO
			body[2]=(byte)( coilValue ? 256 : 0 );  //value
			body[3]=0;                          //ever 0
			return new Message(dispositivo, (byte)MessageType.WriteSigleCoil, body);
		}

		/*Message 6 - Write a single Holding Registers*/
		static public Message WriteSigleHoldingRegisters(byte dispositivo, ushort registerAddress, ushort registerValue) {
			byte[] body = new byte[4];
			body[0]=(byte)( registerAddress>>8 );	//CoilAddress	HI
			body[1]=(byte)( registerAddress );		//CoilAddress	LO
			body[2]=(byte)( registerValue>>8 );		//registerValue HI
			body[3]=(byte)( registerValue );		//registerValue LO
			return new Message(dispositivo, (byte)MessageType.WriteSigleHoldingRegisters, body);
		}

		/*Message 15 - Write n coils*/ //o numero de bobinas é o tamanho da lista values
		static public Message WriteNCoils(byte dispositivo, ushort startingAddress, List<bool> values) {
			ushort nCoils = (ushort)values.Count;
			if(nCoils>2000) throw new BadMessageException("More than 2000 Inputs"); ;   //limite do protocolo

			byte N = (byte)(nCoils/8);
			if(nCoils%8!=0) N++;

			byte[] fisrt = BitConverter.GetBytes(startingAddress);
			byte[] num = BitConverter.GetBytes(nCoils);
			byte[] body = new byte[5 + N];
			body[0]=fisrt[1];
			body[1]=fisrt[0];
			body[2]=num[1];
			body[3]=num[0];
			body[4]=N;

			/*for(byte i = 0; i<N; i++) {
				int j = i*8;
				int aux = 0;
				for(int k = 0; k<8&&j+k<nCoils; k++) {
					if(values[j+k]) aux+=2^k;
				}
				body[5+i]=(byte)aux;
			}*/


			/*byte mask = (byte)(nCoils<8?bitMaskD[8-nCoils]:0x80);		///deixar so a declaracao
			byte resByte = 0;
			for(int i = 0; i<nCoils; i++) {
				if(mask==0) {
					mask=(byte)( nCoils-i<8 ? bitMaskD[nCoils-i] : 0x80 );
					resByte++;
					body[5+resByte]=0;
				}

				if(values[i]) body[5+resByte]+=mask;

				mask>>=1;
			}*/

			byte mask = 0x01;
			byte resByte = 0; ;
			body[5]=0;				//zera os bit não utilizados
			for(int i = 0; i<nCoils; i++) {
				if(values[i]) body[5+resByte]+=mask;

				if(mask==0x80) {
					mask=0x01;		//bits alinhads a direita
					resByte++;
					body[5+resByte]=0;
				}else mask<<=1;
			}

			//byte b = body[5];		//just for a test

			return new Message(dispositivo, (byte)MessageType.WriteNCoils, body);
		}

		/*Message 16 - Write n coils*/ //o numero de Holding Registers é o tamanho da lista values
		static public Message WriteNHoldingRegisters(byte dispositivo, ushort startingAddress, List<ushort> values) {
			ushort nRegisters = (ushort)values.Count;
			if(nRegisters>120) throw new BadMessageException("More than 120 Registers"); ;   //limite do protocolo

			byte[] fisrt = BitConverter.GetBytes(startingAddress);
			byte[] num = BitConverter.GetBytes(nRegisters);
			byte[] body = new byte[5+2*nRegisters];
			body[0]=fisrt[1];
			body[1]=fisrt[0];
			body[2]=num[1];
			body[3]=num[0];
			body[4]=(byte)( nRegisters*2 );

			for(int i = 0; i<nRegisters; i++) {
				byte[] aux = BitConverter.GetBytes(values[i]);
				body[5+2*i]=aux[1];     //iezimo valor HI
				body[6+2*i]=aux[0];     //iezimo valor LO
			}

			return new Message(dispositivo, (byte)MessageType.WriteNHoldingRegisters, body);
		}
		/*Message 16 - Write n coils*/
		//o numero de Holding Registers é o tamanho da lista dividido por 2
		//list deve conter o mais significavo com indice 0, por exemplo {0x12, 0x34, 0xab, 0xcd} sera enviado 0x1234 para startingAddress e 0xabcd para startingAddress+1
		static public Message WriteNHoldingRegisters(byte dispositivo, ushort startingAddress, byte[] list) {
			ushort nRegisters = (ushort)(list.Length/2);    //cada HoldingRegisters e de 16 bits
			if(nRegisters>120) throw new BadMessageException("More than 120 Registers"); ;   //limite do protocolo

			byte[] fisrt = BitConverter.GetBytes(startingAddress);
			byte[] num = BitConverter.GetBytes(nRegisters);
			byte[] body = new byte[5+2*nRegisters];
			body[0]=fisrt[1];
			body[1]=fisrt[0];
			body[2]=num[1];
			body[3]=num[0];
			body[4]=(byte)( nRegisters*2 );

			for(int i = 0; i<list.Length; i++) body[5+i]=list[i];

			return new Message(dispositivo, (byte)MessageType.WriteNHoldingRegisters, body);
		}
	}

	class InvelidMessage : List<byte> {
		public InvelidMessage(byte[] message) {
			AddRange(message);
		}
	}

	class BadMessageException : Exception {
		public BadMessageException() {
		}

		public BadMessageException(string message) : base(message) {
		}

		public BadMessageException(string message, Exception inner) : base(message, inner) {
		}
	}
}

/*	| Mensagens	|   estado	 |
	| 1			| testar	 |
	| 2			| testar	 |
	| 3			| testar	 |
	| 4			| nao usada	 |
	| 5			| testar	 |
	| 6			| testar	 |
	| 7 - 14	| nao usada	 |
	| 15		| testar	 |
	| 16		| testar	 |
*/
