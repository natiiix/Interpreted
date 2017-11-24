using System;
using System.Linq;
using System.Collections.Generic;

namespace Interpreted
{
    public partial class RuntimeEnvironment
    {
        private ValueContainer GetValueOf(string source)
        {
            // Try to treat the source value as a variable name
            int varIdx = variables.FindIndexByName(source);

            // Variable with the specified name exists
            if (varIdx >= 0)
            {
                // Returns its value
                return variables[varIdx].Value;
            }
            // There is no such variable
            else
            {
                // Parse the value from the input string
                return ValueContainer.Parse(source);
            }
        }

        private ValueContainer GetValueOf(IEnumerable<string> args)
        {
            int argsCount = args.Count();

            // No arguments
            if (argsCount == 0)
            {
                // Returns a null value
                return new ValueContainer();
            }
            // Single arugment
            else if (argsCount == 1)
            {
                // Returns the value of the single argument
                return GetValueOf(args.First());
            }
            // 2 or more arguments
            // Operations are evaluated from left to right regardless of the mathematical operator precedence
            else
            {
                // Value of the last operand
                ValueContainer operandValue = GetValueOf(args.Last());
                // Last operator
                string strOperator = args.ElementAt(argsCount - 2);
                // Remaining arguments
                List<string> remainingArguments = args.Take(argsCount - 2).ToList();

                // Addition
                if (strOperator == "+")
                {
                    return GetValueOf(remainingArguments) + operandValue;
                }
                // Subtraction
                else if (strOperator == "-")
                {
                    return GetValueOf(remainingArguments) - operandValue;
                }
                // Multiplication
                else if (strOperator == "*")
                {
                    return GetValueOf(remainingArguments) * operandValue;
                }
                // Division
                else if (strOperator == "/")
                {
                    return GetValueOf(remainingArguments) / operandValue;
                }
                // Unrecognized operator
                else
                {
                    throw new InstructionException("Unrecognized operator " + strOperator.Encapsulate('"'));
                }
            }
        }

        private void InstrPrint(IEnumerable<string> args)
        {
            // Print an empty new line
            if (args.Count() == 0)
            {
                Console.WriteLine();
            }
            // Print a value
            else
            {
                Console.WriteLine(GetValueOf(args).ToString());
            }
        }

        private void InstrVar(IEnumerable<string> args)
        {
            if (args.Count() == 0)
            {
                throw new InstructionSyntaxException("VAR <name> [value]");
            }

            AddVariable(new Variable(args.ElementAt(0), GetValueOf(args.Skip(1))));
        }

        private void InstrDelete(IEnumerable<string> args)
        {
            if (args.Count() != 1)
            {
                throw new InstructionSyntaxException("DELETE <variable name>");
            }

            DeleteVariable(args.First());
        }

        private void InstrSet(IEnumerable<string> args)
        {
            if (args.Count() < 2)
            {
                throw new InstructionSyntaxException("SET <target variable> <source>");
            }

            // Find target variable
            string targetVarName = args.ElementAt(0);

            int targetIdx = variables.FindIndexByName(targetVarName);

            if (targetIdx < 0)
            {
                throw new VariableUndefinedException(targetVarName);
            }

            // Get the source value
            ValueContainer value = GetValueOf(args.Skip(1));

            // Copy the source value to the target variable
            variables[targetIdx].Value = value;
        }
    }
}