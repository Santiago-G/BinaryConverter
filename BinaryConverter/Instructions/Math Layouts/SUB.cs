using Assembler;
using Assembler.Instructions;
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

        public SUB(byte[] data) : base(data)
        {
        }

        public SUB() : base() { }
    }
}
