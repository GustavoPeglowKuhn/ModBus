using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModBus {
	static class Crc {
		static public byte[] Calculate(List<byte> message) {
			byte[] res = new byte[2];
			res[0]=0xf0;
			res[0]=0x0f;	//apenas para teste
			return res;
		}
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
