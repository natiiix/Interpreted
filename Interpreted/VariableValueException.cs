using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreted
{
    public class VariableValueException : InstructionException
    {
        public VariableValueException(string varName, string valueString) : base("Unable to set variable " + varName.Encapsulate('"') + " to value " + valueString.Encapsulate('"'))
        {
        }
    }
}
