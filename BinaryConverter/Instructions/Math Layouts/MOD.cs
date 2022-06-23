using System;
using System.Collections.Generic;
using System.Text;

namespace Assembler.Instructions.Math_Layouts
{
    [OpCode(0x14)]
    public class MOD : Instruction
    {
        public override string Pattern => @"(MOD) R([012][\d]|3[01]\d) R([012][\d]|3[01]\d) R([012][\d]|3[01]\d)";

        private MathLayout mathLayout = new MathLayout();
        protected override ILayout Layout => mathLayout;

        public MOD(byte[] data) : base(data)
        {
        }

        public MOD() : base() { }
    }
}
