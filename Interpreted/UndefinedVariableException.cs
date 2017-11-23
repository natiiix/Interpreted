namespace Interpreted
{
    public class UndefinedVariableException : InstructionException
    {
        public UndefinedVariableException(string varName) : base("Undefined variable " + varName.Encapsulate('"'))
        {
        }
    }
}