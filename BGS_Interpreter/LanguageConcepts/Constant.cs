using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    internal abstract class Constant : LanguageObject
    {
    }

    internal class Constant<T> : Constant, IValue<T> where T : BaseTypes.Type
    {
        private readonly T _value;

        public Constant(T value)
        {
            _value = value;
        }

        public T Evaluate()
        {
            return _value;
        }

        public T Evaluate(Scope context)
        {
            return Evaluate();
        }
    }
}
