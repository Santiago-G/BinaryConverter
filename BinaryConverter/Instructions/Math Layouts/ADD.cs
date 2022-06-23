using Assembler;
using Assembler.Instructions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Assembler.Instructions.Math_Layouts
{
    [OpCode(0x10)]
    public class ADD : Instruction
    {   
        public override string Pattern => @"(ADD) R([012][\d]|3[01]\d) R([012][\d]|3[01]\d) R([012][\d]|3[01]\d)";

        private MathLayout mathLayout = new MathLayout();
        protected override ILayout Layout => mathLayout;

        public ADD(byte[] data) : base(data)
        { 
        }

        public ADD() : base() {}
    }
}
