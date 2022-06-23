using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Instructions.Math_Layouts
{
    [OpCode(0x12)]
    public class MUL : Instruction
    {
        public override string Pattern => @"(MUL) R([012][\d]|3[01]\d) R([012][\d]|3[01]\d) R([012][\d]|3[01]\d)";

        private MathLayout mathLayout = new MathLayout();
        protected override ILayout Layout => mathLayout;

        public MUL(byte[] data) : base(data)
        {
        }

        public MUL() : base() { }
    }
}
