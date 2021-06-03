using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts.Expressions
{
    internal abstract class Expression<T> : LanguageObject, IValue<T>, IExecutable where T : BaseTypes.Type
    {
        protected BaseTypes.Type _returnValue;

        public abstract BaseTypes.Type Evaluate();

        public abstract BaseTypes.Type Evaluate(Scope context);

        public abstract void Execute(Scope context);
    }

    internal class AssignmentExpression<T> : Expression<T> where T : BaseTypes.Type
    {
        private readonly VariableIdentifier _variableIdentifier;

        private readonly IValue _value;

        public AssignmentExpression(string varialbeName, IValue value)
        {
            _variableIdentifier = new VariableIdentifier(varialbeName);
            _value = value;
        }

        public AssignmentExpression(VariableIdentifier identifier, IValue value)
        {
            _variableIdentifier = identifier;
            _value = value;
        }

        public override BaseTypes.Type Evaluate()
        {
            throw new InvalidOperationException("Assignment needs context.");
        }

        public override BaseTypes.Type Evaluate(Scope context)
        {
            Execute(context);
            return _returnValue;
        }

        public override void Execute(Scope context)
        {
            var variable = _variableIdentifier.GetVariable(context);

            var value = _value.Evaluate(context);
            variable.Assign(value);
            _returnValue = value;
        }
    }
}
