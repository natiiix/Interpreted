namespace Interpreted
{
    public class VariableUndefinedException : InstructionException
    {
        public VariableUndefinedException(string varName) : base("Undefined variable " + varName.Encapsulate('"'))
        {
        }
    }
}