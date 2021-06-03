using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    internal abstract class Variable : LanguageObject, IValue
    {
        public Variable(string name)
        {
            Name = name;
        }

        public string Name { get; init; }

        public abstract BaseTypes.Type Evaluate();

        public abstract BaseTypes.Type Evaluate(Scope context);

        public abstract void Assign(BaseTypes.Type value);
    }

    internal class Variable<T> : Variable where T : BaseTypes.Type
    {
        private T _value;

        public Variable(string name) : base(name)
        {
        }

        public Variable(string name, T value):base(name)
        {
            _value = value;
        }

        public override void Assign(BaseTypes.Type newValue)
        {
            if (newValue is T newValueConverted)
            {
                _value = newValueConverted;
            }
            else
            {
                throw new Exception();
            }
        }

        public override T Evaluate()
        {
            return _value;
        }

        public override T Evaluate(Scope context)
        {
            return Evaluate();
        }
    }
}
