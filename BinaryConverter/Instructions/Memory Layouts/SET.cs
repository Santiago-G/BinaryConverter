using BinaryConverter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Instructions.Memory_Layouts
{
    [OpCode(0x40)]
    public class SET : Instruction
    {
        //fix this later (larger numbers)
        public override string Pattern => @"(SET) R([012][\d]|3[01]\d) (\d+)";

        private SETLayout setLayout = new SETLayout();
        protected override ILayout Layout => setLayout;

        public SET(byte[] data) : base(data) { }
        public SET() : base() { }

        public override void DoWork()
        {
            Program.Registers[Data[1]] = Data[3];
        }
    }
}
