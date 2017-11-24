using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Interpreted
{
    public static class InstructionParser
    {
        public static List<Instruction> GetInstructionsFromFile(string path, out List<StringSubstitution> substitutions)
        {
            string[] lines = new string[0];

            // Read all lines from the source code file
            try
            {
                lines = File.ReadAllLines(path);
            }
            catch (Exception e)
            {
                ErrorHandler.GenericError(e.Message);
            }

            // Create lists for the output objects
            List<Instruction> instructions = new List<Instruction>();
            substitutions = new List<StringSubstitution>();

            // Iterate through all the lines
            for (int i = 0; i < lines.Length; i++)
            {
                // Clean this line up
                string lineClean = CleanLine(lines[i], out List<StringSubstitution> newSubstitutions);

                // Skip empty lines and comments
                if (lineClean.Length == 0 || lineClean.StartsWith("#") || lineClean.StartsWith("//"))
                {
                    continue;
                }

                // Add the string substitutions from this line to the list of substitutions in this file
                substitutions.AddRange(newSubstitutions);

                // Add the cleaned up line to the list of instructions
                instructions.Add(new Instruction(path, i + 1, lineClean));
            }

            // Returns the instructions from the source file
            return instructions;
        }

        private static string CleanLine(string line, out List<StringSubstitution> substitutions)
        {
            // Remove all whitespace
            string clean = line.Trim(' ', '\t');

            // There must not be an ever number of double quotes on a line
            if (clean.Count(x => x == '"') % 2 > 0)
            {
                ErrorHandler.GenericError("Odd number of double quotes on a line: " + line);

                substitutions = null;
                return null;
            }

            // List of string substitutions
            substitutions = new List<StringSubstitution>();

            // Start from the first charater
            int scanStartIdx = 0;

            while (scanStartIdx < clean.Length)
            {
                // Find the beginning and end of the string
                int startIdx = line.IndexOf('"', scanStartIdx);
                int endIdx = line.IndexOf('"', startIdx + 1);

                // End of string reached
                if (startIdx < 0 || endIdx < 0)
                {
                    break;
                }

                // Extract the text of the string
                string text = clean.Substring(startIdx + 1, endIdx - startIdx - 1);
                StringSubstitution substitution = new StringSubstitution(text);

                // Get what should be replaced and with what should it be replaced
                string toReplace = text.Encapsulate('"');
                string replaceWith = substitution.Substitution;

                // Replace the string with a substitution
                clean = clean.Replace(toReplace, replaceWith);
                // Add the string substitution to the list
                substitutions.Add(substitution);

                // Update the scan start index
                scanStartIdx += endIdx + 1 - toReplace.Length + replaceWith.Length;
            }

            // Replace multiple spaces with a single space
            while (clean.Contains("  "))
            {
                clean = clean.Replace("  ", " ");
            }

            // Return the cleaned up string
            return clean;
        }
    }
}