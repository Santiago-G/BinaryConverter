using BinaryConverter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Instructions.FlowControl_Layouts
{
    [OpCode(0x30)]
    public class JMP : Instruction
    {
        //fix this later
        public override string Pattern => @"(JMP) (\d+)";

        private JMPLayout jmpLayout = new JMPLayout();
        protected override ILayout Layout => jmpLayout;

        public JMP(byte[] data) : base(data) {}
        public JMP() : base() {}

        public override void DoWork()
        {
            Program.IP = Data[3];
        }
    }
}
