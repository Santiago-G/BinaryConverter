using Assembler;
using Assembler.Instructions;
using BinaryConverter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Instructions.Math_Layouts
{
    [OpCode(0x11)]
    public class SUB : Instruction
    {
        public override string Pattern => @"(SUB) R([012][\d]|3[01]\d) R([012][\d]|3[01]\d) R([012][\d]|3[01]\d)";

        private MathLayout mathLayout = new MathLayout();
        protected override ILayout Layout => mathLayout;
        public override void DoWork()
        {
            Program.Registers[Data[1]] = (byte)(Program.Registers[Data[2]] - Program.Registers[Data[3]]);
        }

        public SUB(byte[] data) : base(data)
        {
        }

        public SUB() : base() { }
    }
}
