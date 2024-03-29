﻿using BinaryConverter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Assembler.Instructions.FlowControl_Layouts
{
    public class JMPLayout : ILayout
    {

        // Addr(hb), Addr(lb), pad
        // set R(loc), Val(hb), Val(lb)
        public byte[] Parse(Match match)
        {
            byte[] data = new byte[4];

            data[0] = Program.NameToOpCode[match.Groups[1].Value];

            data[1] = 0xFF;
            //data[2] = byte.Parse(match.Groups[2].Value);
            data[3] = byte.Parse(match.Groups[2].Value);

            return data;
        }
    }
}
