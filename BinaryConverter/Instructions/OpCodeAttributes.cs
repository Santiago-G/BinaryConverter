using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Instructions
{
    public class OpCodeAttribute : Attribute
    {
        public byte OpCode;

        public OpCodeAttribute(byte opCode)
        {
            OpCode = opCode;
        }
    }
}
