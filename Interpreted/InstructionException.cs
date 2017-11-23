using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreted
{
    public class InstructionException : Exception
    {
        public InstructionException(string message) : base(message)
        {
        }
    }
}
