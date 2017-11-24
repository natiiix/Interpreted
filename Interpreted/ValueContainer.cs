using System;
using System.Collections.Generic;
using System.Linq;

namespace Interpreted
{
    public class ValueContainer
    {
        private enum ValueType
        {
            Null,
            String,
            Int,
            Double
        }

        private ValueType type;

        private string stringValue;
        private int? intValue;
        private double? doubleValue;

        public ValueContainer()
        {
            SetToNull();
        }

        public ValueContainer(string str)
        {
            SetValue(str);
        }

        public ValueContainer(int i)
        {
            SetValue(i);
        }

        public ValueContainer(double d)
        {
            SetValue(d);
        }

        private void SetValuesToNull()
        {
            stringValue = null;
            intValue = null;
            doubleValue = null;
        }

        private void SetToNull()
        {
            SetValuesToNull();

            type = ValueType.Null;
        }

        private void SetValue(string str)
        {
            SetValuesToNull();

            stringValue = str;
            type = ValueType.String;
        }

        private void SetValue(int i)
        {
            SetValuesToNull();

            intValue = i;
            type = ValueType.Int;
        }

        private void SetValue(double d)
        {
            SetValuesToNull();

            doubleValue = d;
            type = ValueType.Double;
        }

        public override string ToString()
        {
            switch (type)
            {
                case ValueType.String:
                    return stringValue;

                case ValueType.Int:
                    return intValue.ToString();

                case ValueType.Double:
                    return doubleValue.ToString();

                default:
                    return "null";
            }
        }

        public int ToInt()
        {
            switch (type)
            {
                case ValueType.String:
                    if (int.TryParse(stringValue, out int i))
                    {
                        return i;
                    }
                    else
                    {
                        return 0;
                    }

                case ValueType.Int:
                    return intValue ?? 0;

                case ValueType.Double:
                    return (int)(doubleValue ?? 0);

                default:
                    return 0;
            }
        }

        public double ToDouble()
        {
            switch (type)
            {
                case ValueType.String:
                    if (double.TryParse(stringValue, out double d))
                    {
                        return d;
                    }
                    else
                    {
                        return 0;
                    }

                case ValueType.Int:
                    return intValue ?? 0;

                case ValueType.Double:
                    return doubleValue ?? 0;

                default:
                    return 0;
            }
        }

        public ValueContainer ToStringValue() => new ValueContainer(ToString());

        public ValueContainer ToIntValue() => new ValueContainer(ToInt());

        public ValueContainer ToDoubleValue() => new ValueContainer(ToDouble());

        public static ValueContainer Parse(string strValue)
        {
            if (strValue == "null")
            {
                return new ValueContainer();
            }
            else if (int.TryParse(strValue, out int i))
            {
                return new ValueContainer(i);
            }
            else if (double.TryParse(strValue, out double d))
            {
                return new ValueContainer(d);
            }
            else
            {
                return new ValueContainer(strValue);
            }
        }

        private static IEnumerable<ValueContainer> ToCommonType(IEnumerable<ValueContainer> values, bool optimizeValuesViaParsing)
        {
            // Find distinct value types
            IEnumerable<ValueType> types = values.Select(x => x.type).Distinct();

            // Optimize values via parsing
            if (optimizeValuesViaParsing && types.Count() > 1)
            {
                return ToCommonType(values.Select(x => Parse(x.ToString())), false);
            }

            // One of the values is a string
            if (types.Contains(ValueType.String))
            {
                return values.Select(x => x.ToStringValue());
            }
            // ... double
            else if (types.Contains(ValueType.Double))
            {
                return values.Select(x => x.ToDoubleValue());
            }
            // ... int
            else if (types.Contains(ValueType.Int))
            {
                return values.Select(x => x.ToIntValue());
            }
            // All the values are null
            else
            {
                return values.Select(x => new ValueContainer());
            }
        }

        public static ValueContainer operator -(ValueContainer a)
        {
            switch (a.type)
            {
                // double
                case ValueType.Double:
                    return new ValueContainer(-a.ToDouble());

                // int
                case ValueType.Int:
                    return new ValueContainer(-a.ToInt());

                // string or null
                default:
                    return new ValueContainer();
            }
        }

        private static void PrepareForOperation(ValueContainer a, ValueContainer b, out ValueType commonType, out ValueContainer aConverted, out ValueContainer bConverted)
        {
            IEnumerable<ValueContainer> commonTypeValues = ToCommonType(new ValueContainer[] { a, b }, true);

            commonType = commonTypeValues.Select(x => x.type).Distinct().First();
            aConverted = commonTypeValues.ElementAt(0);
            bConverted = commonTypeValues.ElementAt(1);
        }

        public static ValueContainer operator +(ValueContainer a, ValueContainer b)
        {
            PrepareForOperation(a, b, out ValueType commonType, out ValueContainer first, out ValueContainer second);

            switch (commonType)
            {
                case ValueType.String:
                    return new ValueContainer(first.ToString() + second.ToString());

                case ValueType.Double:
                    return new ValueContainer(first.ToDouble() + second.ToDouble());

                case ValueType.Int:
                    return new ValueContainer(first.ToInt() + second.ToInt());

                default:
                    return new ValueContainer();
            }
        }

        public static ValueContainer operator -(ValueContainer a, ValueContainer b) => a + -b;

        public static ValueContainer operator *(ValueContainer a, ValueContainer b)
        {
            PrepareForOperation(a, b, out ValueType commonType, out ValueContainer first, out ValueContainer second);

            switch (commonType)
            {
                case ValueType.Double:
                    return new ValueContainer(first.ToDouble() * second.ToDouble());

                case ValueType.Int:
                    return new ValueContainer(first.ToInt() * second.ToInt());

                default:
                    return new ValueContainer();
            }
        }

        public static ValueContainer operator /(ValueContainer a, ValueContainer b)
        {
            PrepareForOperation(a, b, out ValueType commonType, out ValueContainer first, out ValueContainer second);

            switch (commonType)
            {
                case ValueType.Double:
                    return new ValueContainer(first.ToDouble() / second.ToDouble());

                case ValueType.Int:
                    int firstInt = first.ToInt();
                    int secondInt = second.ToInt();

                    if (firstInt % secondInt == 0)
                    {
                        return new ValueContainer(first.ToInt() / second.ToInt());
                    }
                    else
                    {
                        return new ValueContainer((double)firstInt / secondInt);
                    }

                default:
                    return new ValueContainer();
            }
        }
    }
}