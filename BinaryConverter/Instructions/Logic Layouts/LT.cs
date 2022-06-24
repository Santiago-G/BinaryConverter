using System;
using System.Collections.Generic;
using System.Text;
using BinaryConverter;

namespace Assembler.Instructions.Logic_Layouts
{
    [OpCode(0x21)]
    public class LT : Instruction
    {
        public override string Pattern => @"(LT) R([012][\d]|3[01]\d) R([012][\d]|3[01]\d) R([012][\d]|3[01]\d)";

        private LogicLayout logicLayout = new LogicLayout();
        protected override ILayout Layout => logicLayout;

        public override void DoWork()
        {
            if (Program.Registers[Data[2]] < Program.Registers[Data[3]])
            {
                Program.Registers[Data[1]] = 0;
            }
            else
            {
                Program.Registers[Data[1]] = 1;
            }
        }

        public LT(byte[] data) : base(data) { }
        public LT() : base() { }
    }
}
