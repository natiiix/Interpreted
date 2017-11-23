namespace Interpreted
{
    public struct StringSubstitution
    {
        private static int substitutionCounter = 0;

        private readonly int substitutionId;

        public readonly string Text;

        public string Substitution
        {
            get
            {
                return substitutionId.ToString().Encapsulate('%');
            }
        }

        public StringSubstitution(string text)
        {
            Text = text;
            substitutionId = substitutionCounter++;
        }
    }
}