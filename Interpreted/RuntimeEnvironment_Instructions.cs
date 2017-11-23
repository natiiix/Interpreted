using System;
using System.Collections.Generic;

namespace Interpreted
{
    public partial class RuntimeEnvironment
    {
        private void InstrVar(string[] args)
        {
            if (args.Length == 1)
            {
                AddVariable(new Variable(args[0]));
            }
            else if (args.Length == 2)
            {
                AddVariable(new Variable(args[0], args[1]));
            }
            else
            {
                throw new InstructionSyntaxException("VAR <name> [value]");
            }
        }

        private void InstrPrint(string[] args)
        {
            List<string> varValues = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                int varIdx = variables.FindIndexByName(args[i]);

                if (varIdx < 0)
                {
                    throw new UndefinedVariableException(args[i]);
                }

                varValues.Add(variables[varIdx].ToString());
            }

            Console.WriteLine(string.Join(" ", varValues));
        }

        private void InstrSet(string[] args)
        {
            if (args.Length == 2)
            {
                string targetVarName = args[0];

                // Find target variable
                int targetIdx = variables.FindIndexByName(targetVarName);

                if (targetIdx < 0)
                {
                    throw new UndefinedVariableException(targetVarName);
                }

                string source = args[1];

                // Try to treat the source value as a variable name
                int sourceIdx = variables.FindIndexByName(source);

                // There is no such variable
                if (sourceIdx < 0)
                {
                    variables[targetIdx] = new Variable(targetVarName, source);
                }
                // There is a variable with the name specified in source
                else
                {
                    variables[targetIdx].CopyValueFrom(variables[sourceIdx]);
                }
            }
            else
            {
                throw new InstructionSyntaxException("SET <target variable> <source variable or literal value>");
            }
        }
    }
}