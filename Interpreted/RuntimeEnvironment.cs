using System;
using System.Collections.Generic;

namespace Interpreted
{
    public partial class RuntimeEnvironment
    {
        private List<StringSubstitution> stringSubstitutions;
        private List<Instruction> instructions;

        private int programCounter;
        private List<Variable> variables;

        public RuntimeEnvironment(string mainSourceFilePath)
        {
            instructions = InstructionParser.GetInstructionsFromFile(mainSourceFilePath, out stringSubstitutions);
        }

        public void Run()
        {
            programCounter = 0;
            variables = new List<Variable>();

            while (programCounter < instructions.Count)
            {
                Instruction currentInstruction = instructions[programCounter];

                try
                {
                    ProcessInstruction(currentInstruction);
                }
                catch (InstructionException e)
                {
                    throw new RuntimeException(e.Message + Environment.NewLine + currentInstruction.ToString());
                }
            }
        }

        private void ProcessInstruction(Instruction instr)
        {
            string command = string.Empty;
            string[] arguments = new string[0];

            // Extract the command and its arguments
            // Syntax: <Command> [First Arugmnet] [Second Arguments] ...
            {
                int firstSpaceIdx = instr.Text.IndexOf(' ');

                if (firstSpaceIdx < 0)
                {
                    command = instr.Text;
                }
                else
                {
                    command = instr.Text.Substring(0, firstSpaceIdx);
                    arguments = instr.Text.Substring(firstSpaceIdx + 1).Split(' ');
                }
            }

            switch (command.ToLower())
            {
                case "var":
                    InstrVar(arguments);
                    programCounter++;
                    break;

                case "print":
                    InstrPrint(arguments);
                    programCounter++;
                    break;

                case "set":
                    InstrSet(arguments);
                    programCounter++;
                    break;

                default:
                    throw new InstructionException("Unrecognized command");
            }
        }

        private void AddVariable(Variable var)
        {
            if (variables.FindIndexByName(var.Name) < 0)
            {
                variables.Add(var);
            }
            else
            {
                throw new InstructionException("Variable with the identifier " + var.Name.Encapsulate('"') + " is already defined");
            }
        }
    }
}