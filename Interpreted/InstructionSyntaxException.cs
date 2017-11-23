using System;

namespace Interpreted
{
    public class InstructionSyntaxException : InstructionException
    {
        public InstructionSyntaxException(string correctSyntax) : base("Unexpected instruction syntax." + Environment.NewLine + "Expected syntax: " + correctSyntax)
        {
        }
    }
}