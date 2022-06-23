using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Assembler
{
    public interface ILayout
    {
        byte[] Parse(Match match);
    }
}
