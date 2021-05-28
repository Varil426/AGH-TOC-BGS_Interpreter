using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    internal abstract class Variable : LanguageObject
    {
        public Variable(string name)
        {
            Name = name;
        }

        public string Name { get; init; }
    }

    internal class Variable<T> : Variable, IValue<T> where T : BaseTypes.Type
    {
        private T _value;

        public Variable(string name) : base(name)
        {
        }

        public Variable(string name, T value):base(name)
        {
            _value = value;
        }

        public void Assign(T newValue)
        {
            _value = newValue;
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
