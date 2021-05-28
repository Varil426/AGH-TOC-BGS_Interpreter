using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts.Expressions
{
    internal abstract class Expression<T> : LanguageObject, IValue<T>, IExecutable where T : BaseTypes.Type
    {
        protected T _returnValue;

        public abstract T Evaluate();

        public abstract T Evaluate(Scope context);

        public abstract void Execute(Scope context);
    }

    internal class AssignmentExpression<T> : Expression<T> where T : BaseTypes.Type
    {
        private readonly string _variableName;

        private readonly IValue<T> _value;

        public AssignmentExpression(string varialbeName, IValue<T> value)
        {
            _variableName = varialbeName;
            _value = value;
        }

        public override T Evaluate()
        {
            throw new InvalidOperationException("Assignment needs context.");
        }

        public override T Evaluate(Scope context)
        {
            Execute(context);
            return _returnValue;
        }

        public override void Execute(Scope context)
        {
            var variable = context.GetVariable(_variableName);

            if (variable is Variable<T> convertedVariable)
            {
                var value = _value.Evaluate();
                convertedVariable.Assign(value);
                _returnValue = value;
            }
        }
    }
}
