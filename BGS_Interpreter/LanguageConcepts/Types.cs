using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts.BaseTypes
{
    internal abstract class Type
    {
        public System.Type BaseType { get; protected init; }
    }

    internal abstract class Type<T> : Type
    {
        public Type(T value)
        {
            Value = value;
            BaseType = typeof(T);
        }

        public T Value { get; init; }
    }

    internal class Integer : Type<int>
    {
        public Integer(int value):base(value)
        {
        }
    }

    internal class Double : Type<double>
    {
        public Double(double value):base(value)
        {
        }
    }

    internal class String : Type<string>
    {
        public String(string value):base(value)
        {
        }
    }

    internal class Boolean : Type<bool>
    {
        public Boolean(bool value):base(value)
        {
        }
    }

    internal class Void : Type<object>
    { 
        public Void():base(null)
        {
        }
    }
}
