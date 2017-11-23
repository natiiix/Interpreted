using System;

namespace Interpreted
{
    public class RuntimeException : Exception
    {
        public RuntimeException(string message) : base(message)
        {
        }
    }
}