using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BinaryConverter;

namespace Assembler
{
    public abstract class Instruction
    {
        protected byte[] Data;

        public abstract string Pattern { get; }
        protected abstract ILayout Layout { get; }

        public Instruction(byte[] data)
        {
            Data = data;
        }

        public abstract void DoWork();

        protected Instruction() { }
        public Instruction Parse(Match match)
        {
            var type = Program.InstructionNameToType[match.Groups[1].Value];
            return (Instruction)Activator.CreateInstance(type, Layout.Parse(match));
        }

        public byte[] Emit() => Data;
    }
}
