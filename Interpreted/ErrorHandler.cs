using System;

namespace Interpreted
{
    public static class ErrorHandler
    {
        public static void GenericError(string message)
        {
            Console.WriteLine("Error occurred: " + message);
            Console.ReadLine();
            Environment.Exit(-1);
        }
    }
}