using System;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assembler;
using Assembler.Instructions.Math_Layouts;
using Assembler.Instructions;

namespace BinaryConverter
{
    class Program
    {
        static string[] LoadAssemblyProgram()
        {
            return System.IO.File.ReadAllLines("stationTo.station");
        }

        static byte[] InstructionsToArray(List<Instruction> assemblyInstructions)
        {
            byte[] bytes = new byte[assemblyInstructions.Count * 4];

            int byteCounter = 0;

            foreach (var instruction in assemblyInstructions)
            {
                for (int j = 0; j < 4; j++)
                {
                    bytes[byteCounter] = instruction.Emit()[j];
                    byteCounter++;
                }
            }

            return bytes;
        }

        static byte[] InstructionsToArrayOneLiner(List<Instruction> assemblyInstructions)
            => assemblyInstructions.Select(ins => ins.Emit().AsEnumerable()).Aggregate((x, y) => x.Concat(y)).ToArray();
        //I DID NOT WRITE THIS, this is black magic ^

        public static Dictionary<string, Type> InstructionNameToType = new Dictionary<string, Type>();

        public static Dictionary<string, byte> NameToOpCode = new Dictionary<string, byte>();

        public static Instruction[] GetAllInstructions()
        {
            Instruction[] validInstructions =  new Instruction[] { new ADD(), new SUB(), new MUL(), new DIV(), new MOD()};
            //you would have to fill in NameToOpCode here.
            return validInstructions;
        }
        //the function I understand^

        public static Instruction[] CoolerGetAllInstructions()
        {
            Type instructionType = typeof(Instruction);
            Type[] typesInMyAssembly = Assembly.GetAssembly(instructionType).GetTypes();
            //gets all types (classes) in the program

            Type[] validInstructions = typesInMyAssembly.Where(type => type.IsSubclassOf(instructionType) && type.GetCustomAttribute<OpCodeAttribute>() != null).ToArray();
            //filters the types to only the instructions

            NameToOpCode = validInstructions.ToDictionary(type => type.Name, type => type.GetCustomAttribute<OpCodeAttribute>().OpCode);

            InstructionNameToType = validInstructions.ToDictionary(type => type.Name, type => type);
            Instruction[] instructions = validInstructions.Select(type => (Instruction)Activator.CreateInstance(type)).ToArray();
            //map the type to the instantiated class
            return instructions;
        }
        //voodoo magic (reflection)^

        //

        static void Main(string[] args)
        {
            Instruction[] instructions = CoolerGetAllInstructions();

            List<Instruction> assemblyInstructionsInProgram = new List<Instruction>();

            string[] assemblyInstructions = LoadAssemblyProgram();
            Match match;
            foreach (string assemblyInstruction in assemblyInstructions)
            {
                foreach (Instruction ins in instructions)
                {
                    match = Regex.Match(assemblyInstruction, ins.Pattern);

                    if(match.Success)
                    {
                        assemblyInstructionsInProgram.Add(ins.Parse(match));
                        break;
                    }
                }
            }
            byte[] electricBlue = InstructionsToArray(assemblyInstructionsInProgram);
            ;
            System.IO.File.WriteAllBytes("AssemblyProgramBytes.bin", electricBlue);

        }
    }
}
