using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreted
{
    public class InstructionSyntaxException : InstructionException
    {
        public InstructionSyntaxException(string correctSyntax) : base("Unexpected instruction syntax." + Environment.NewLine + "Expected syntax: " + correctSyntax)
        {
        }
    }
}
