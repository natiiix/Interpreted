using System;
using System.Collections.Generic;
using System.Linq;

namespace Interpreted
{
    public class Variable
    {
        public readonly string Name;

        // int
        public int? IntValue { get; private set; }

        // double
        public double? DoubleValue { get; private set; }

        public Variable(string name)
        {
            Name = name;
            IntValue = null;
            DoubleValue = null;
        }

        public Variable(string name, int value) : this(name)
        {
            SetValue(value);
        }

        public Variable(string name, double value) : this(name)
        {
            SetValue(value);
        }

        public Variable(string name, string valueString) : this(name)
        {
            int intValue = 0;
            double doubleValue = 0;

            if (int.TryParse(valueString, out intValue))
            {
                SetValue(intValue);
            }
            else if (double.TryParse(valueString, out doubleValue))
            {
                SetValue(doubleValue);
            }
            else
            {
                throw new VariableValueException(name, valueString);
            }
        }

        public void SetValue<T>(T value)
        {
            IntValue = null;
            DoubleValue = null;

            if (typeof(T) == typeof(int))
            {
                IntValue = value as int?;
            }
            else if (typeof(T) == typeof(double))
            {
                DoubleValue = value as double?;
            }
            else
            {
                ErrorHandler.GenericError("Unexpected variable data type");
            }
        }

        public void CopyValueFrom(Variable sourceVar)
        {
            IntValue = sourceVar.IntValue;
            DoubleValue = sourceVar.DoubleValue;
        }

        public override string ToString()
        {
            return IntValue != null ? IntValue.ToString() : DoubleValue != null ? DoubleValue.ToString() : "null";
        }

        public int ToInt()
        {
            return IntValue ?? (int?)DoubleValue ?? 0;
        }

        public double ToDouble()
        {
            return DoubleValue ?? IntValue ?? 0;
        }

        private Type GetVariableType()
        {
            return IntValue != null ? typeof(int) : DoubleValue != null ? typeof(double) : null;
        }

        private static Type GetCommonType(IEnumerable<Type> types)
        {
            IEnumerable<Type> distinctTypes = types.Distinct();

            if (distinctTypes.Contains(typeof(double)))
            {
                return typeof(double);
            }
            else if (distinctTypes.Contains(typeof(int)))
            {
                return typeof(int);
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<Variable> ToCommonType(IEnumerable<Variable> original)
        {
            Type commonType = GetCommonType(original.Select(x => x.GetVariableType()));

            if (commonType == typeof(double))
            {
                return original.Select(x => new Variable(x.Name, x.ToDouble()));
            }
            else if (commonType == typeof(int))
            {
                return original.Select(x => new Variable(x.Name, x.ToInt()));
            }
            else
            {
                return original;
            }
        }
    }
}