namespace Interpreted
{
    public class VariableValueException : InstructionException
    {
        public VariableValueException(string varName, string valueString) : base("Unable to set variable " + varName.Encapsulate('"') + " to value " + valueString.Encapsulate('"'))
        {
        }
    }
}