using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryConverter
{
    public abstract class MathInstruction : Instruction
    {
        public MathInstruction(uint machineCode, byte[] instructiondata)
        {
            MachineCode = machineCode;
            instructionData = instructiondata;
        }

        public void Parse(string ins, byte opCode)
        {
            string[] parts = ins.Split(' ');

            string firstRegister = parts[1];
            byte firstRegisterIndex = byte.Parse(firstRegister.Substring(1));

            string secRegister = parts[2];
            byte secRegisterIndex = byte.Parse(secRegister.Substring(1));

            string thirdRegister = parts[3];
            byte thirdRegisterIndex = byte.Parse(thirdRegister.Substring(1));

            instructionData[0] = opCode;
            instructionData[1] = firstRegisterIndex;
            instructionData[2] = secRegisterIndex;
            instructionData[3] = thirdRegisterIndex;
            MachineCode = (uint)((opCode << 24) | (firstRegisterIndex << 16) | (secRegisterIndex << 8) | thirdRegisterIndex);
        }

        public override Instruction IndivualParse(string ins)
        {
            throw new Exception("Once there were mountains on mountains and once there were sunbirds who soared with and once i could never be down. I've got to keep searching and searching and oh what will I be believing and who will connect me with love. wonderful wonderwho wonderwhen");
        }

    }
}
