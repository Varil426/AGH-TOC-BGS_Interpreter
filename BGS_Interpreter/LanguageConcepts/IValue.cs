using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    internal interface IValue
    {

    }

    internal interface IValue<T> : IValue where T : BaseTypes.Type
    {
        public T Evaluate();

        public T Evaluate(Scope context);
    }
}
