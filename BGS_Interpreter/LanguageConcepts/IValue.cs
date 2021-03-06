using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    internal interface IValue
    {
        public BaseTypes.Type Evaluate();
        public BaseTypes.Type Evaluate(Scope context);
    }

    internal interface IValue<out T> : IValue where T : BaseTypes.Type
    {
        //public T Evaluate();

        //public T Evaluate(Scope context);
    }
}
