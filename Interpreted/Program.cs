using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Interpreted
{
    public class Program
    {
        private static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            if (args.Length != 1)
            {
                ErrorHandler.GenericError("Invalid arguments." + Environment.NewLine + "Please provide a path of a single source code file.");
            }

            RuntimeEnvironment env = new RuntimeEnvironment(args[0]);

            try
            {
                env.Run();
            }
            catch (RuntimeException e)
            {
                Console.WriteLine("Runtime exception occurred: " + e.Message);
            }

            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
