using System;

namespace Interpreted
{
    public class InstructionException : Exception
    {
        public InstructionException(string message) : base(message)
        {
        }
    }
}