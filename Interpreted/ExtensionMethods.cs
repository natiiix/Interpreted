using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreted
{
    public static class ExtensionMethods
    {
        public static string Repeat(this string str, int count)
        {
            string output = string.Empty;

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = 0; i < count; i++)
            {
                output += str;
            }

            return output;
        }

        public static string Encapsulate(this string str, char c)
        {
            return c + str + c;
        }

        public static int FindIndexByName(this List<Variable> list, string name)
        {
            return list.FindIndex(x => x.Name == name);
        }
    }
}
