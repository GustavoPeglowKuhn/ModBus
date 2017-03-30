namespace ModBus {
	//
	// Summary:
	//     Specifies the number of each type of message used on the Message object.
	public enum MessageType : byte {
		broadcast = 0,
		ReadNCoils = 1,
		ReadNInputs = 2,
		ReadNHoldingRegisters = 3,
		WriteSigleCoil = 5,
		WriteSigleHoldingRegisters = 6,
		WriteNCoils = 15,
		WriteNHoldingRegisters = 16
	}
}