using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter
{
    public class SUB : MathInstruction
    {
        public override byte OpCode { get => 0x11; }
        public SUB(uint machineCode, byte[] instructiondata) : base(machineCode, instructiondata)
        { }
        public SUB() : base(0, null)
        {
        }

        public override Instruction IndivualParse(string ins)
        {
            base.Parse(ins, OpCode);
            return this;
        }
    }
}
