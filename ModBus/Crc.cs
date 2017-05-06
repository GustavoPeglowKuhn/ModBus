using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModBus {
	static class Crc {
		static public byte[] Calculate(List<byte> buffer) {
			ushort CRC = 0xffff;
			for(byte i=0; i<buffer.Count; i++) {//6bytes
				CRC^=(ushort)buffer[i];
				for(byte j=0; j<8; j++) {
					if((CRC & 1)==0)
						CRC=(ushort)(CRC>>1);
					else {
						CRC=(ushort)(CRC>>1);
						CRC=(ushort)(CRC^0xa001);
					}
				}
			}
			//return CRC;
			byte[] res = new byte[2];

			res[1]=(byte)CRC;       //lsb
			res[0]=(byte)(CRC>>8);  //msb
			return res;
		}
		/*static ushort[] crctable16 = null;
		static void CalculateTable_CRC16() {
			const ushort generator = 0xa001;
			crctable16=new ushort[256];
			for(int divident = 0; divident<256; divident++) { // iterate over all possible input byte values 0 - 255 
				ushort curByte = (ushort)(divident<<8); // move divident byte into MSB of 16Bit CRC 
				for(byte bit = 0; bit<8; bit++) {
					if((curByte&0x8000)!=0) {
						curByte<<=1;
						curByte^=generator;
					} else {
						curByte<<=1;
					}
				}
				crctable16[divident]=curByte;
			}
		}

		static public byte[] Calculate(List<byte> message) {
			if(crctable16==null) CalculateTable_CRC16();
			ushort crc = 0xffff;
			foreach(byte b in message) {
				// XOR-in next input byte into MSB of crc, that's our new intermediate divident 
				byte pos = (byte)((crc>>8)^b); // equal: ((crc ^ (b << 8)) >> 8) /
											   // Shift out the MSB used for division per lookuptable and XOR with the remainder /
				crc=(ushort)((crc<<8)^(ushort)(crctable16[pos]));
			}
			//return crc;

			byte[] res = new byte[2];
			//res[0]=49;
			//res[1]=48;	//apenas para teste

			res[1]=(byte)crc;		//lsb
			res[0]=(byte)(crc>>8);	//msb

			return res;
		}*/

		/*static public byte[] Calculate(List<byte> message) {
			const ushort generator = 0x1021;    // divisor is 16bit
			ushort crc = 0; // CRC value is 16bit //
							//ushort crc = 0xFFFF; // CRC value is 16bit //

			foreach(byte b in message) {
				crc^=(ushort)(b<<8); // move byte into MSB of 16bit CRC //

				for(int i = 0; i<8; i++) {
					if((crc&0x8000)!=0) // test for MSB = bit 15 //
					{
						crc=(ushort)((crc<<1)^generator);
					} else {
						crc<<=1;
					}
				}
			}
			//return crc;

			byte[] res = new byte[2];
			//res[0]=49;
			//res[1]=48;	//apenas para teste

			res[1]=(byte)crc;       //lsb
			res[0]=(byte)(crc>>8);  //msb

			return res;
		}*/
	}

	class CrcError : Exception {
		public CrcError() {
		}

		public CrcError(string message) : base(message){
		}

		public CrcError(string message, Exception inner) : base(message, inner){
		}
	}
}
