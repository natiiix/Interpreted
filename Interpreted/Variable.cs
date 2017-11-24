namespace Interpreted
{
    public class Variable
    {
        public readonly string Name;
        public ValueContainer Value;

        public Variable(string name)
        {
            Name = name;
            Value = new ValueContainer();
        }

        public Variable(string name, ValueContainer value)
        {
            Name = name;
            Value = value;
        }

        //public Variable(string name, string strValue) : this(name, ValueContainer.Parse(strValue))
        //{
        //}
    }
}