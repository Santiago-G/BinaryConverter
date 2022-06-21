using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryConverter
{
    public abstract class Instruction
    {
        public byte[] instructionData = new byte[4];

        public uint MachineCode { get; set; }
        public abstract byte OpCode { get;}


        public Instruction()
        {

        }

        //abstract void Execute()
        //{

        //}

        public static Instruction Parse(string instruct)
        {
            if (instruct.Substring(0,3) == "ADD")
            {
                ADD add = new ADD();
                return add.IndivualParse(instruct);
            }
            else if (instruct.Substring(0, 3) == "SUB")
            {
                SUB sub = new SUB();
                return sub.IndivualParse(instruct);
            }

            throw new Exception("... wrong?");
        }

        public abstract Instruction IndivualParse(string ins);
    }
}
