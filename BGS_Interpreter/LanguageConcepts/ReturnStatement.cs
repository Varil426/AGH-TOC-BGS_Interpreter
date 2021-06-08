using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    class ReturnStatement<T> : LanguageObject, IExecutable where T : BaseTypes.Type
    {
        private T _returnValue;

        private readonly IValue<T> _valueExpression;

        public ReturnStatement(IValue<T> value)
        {
            _valueExpression = value;
        }

        public void Execute(Scope context)
        {
            _returnValue = _valueExpression.Evaluate(context) as T;
            throw new Exceptions.ReturnException(_returnValue);
        }
    }
}
