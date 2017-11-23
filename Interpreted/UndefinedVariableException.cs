using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreted
{
    public class UndefinedVariableException : InstructionException
    {
        public UndefinedVariableException(string varName) : base("Undefined variable " + varName.Encapsulate('"'))
        {
        }
    }
}
