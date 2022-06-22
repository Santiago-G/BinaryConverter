using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryConverter
{
    public class ADD : MathInstruction
    {
        public override byte OpCode {get => 0x10;}

        //string Pattern = "ADD R([012][\d]|3[01]|\d) R([012][\d]|3[01]|\d) R([012][\d]|3[01]|\d)";

        //opC
        public ADD(uint machineCode, byte[] instructiondata) : base(machineCode, instructiondata)
        {
            
        }
        public ADD() : base(0, null)
        {
        }

        public override Instruction IndivualParse(string ins)
        {
            base.Parse(ins, OpCode);
            return this;
        }
    }
}
