using System;

namespace Interpreted
{
    public struct Instruction
    {
        public readonly string File;
        public readonly int Line;
        public readonly string Text;

        public Instruction(string file, int line, string text)
        {
            File = file;
            Line = line;
            Text = text;
        }

        public override string ToString()
        {
            return "File: " + File + Environment.NewLine +
                "Line: " + Line.ToString() + Environment.NewLine +
                "Code: " + Text;
        }
    }
}