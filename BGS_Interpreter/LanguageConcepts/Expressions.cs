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

        public virtual BaseTypes.Type Evaluate()
        {
            Execute(null);
            return _returnValue;
        }

        public virtual BaseTypes.Type Evaluate(Scope context)
        {
            Execute(context);
            return _returnValue;
        }

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

        public override void Execute(Scope context)
        {
            var variable = _variableIdentifier.GetVariable(context);

            var value = _value.Evaluate(context);
            variable.Assign(value);
            _returnValue = value;
        }
    }

    internal class AdditionExpression<T> : Expression<T> where T : BaseTypes.Type
    {
        protected IValue<T> _leftSide;
        protected IValue<T> _rightSide;

        public AdditionExpression(IValue<T> left, IValue<T> right)
        {
            _leftSide = left;
            _rightSide = right;
        }

        public override void Execute(Scope context)
        {
            var val1 = _leftSide.Evaluate(context);
            var val2 = _rightSide.Evaluate(context);

            switch (val1)
            {
                case BaseTypes.Integer or BaseTypes.Double when val2 is BaseTypes.Integer or BaseTypes.Double:
                    // TODO Maybe change this in the future
                    if (val1 is BaseTypes.Integer intVal1 && val2 is BaseTypes.Integer intVal2)
                    {
                        _returnValue = new BaseTypes.Integer(intVal1.Value + intVal2.Value);
                    }
                    else if (val1 is BaseTypes.Double doubleVal1 && val2 is BaseTypes.Double doubleVal2)
                    {
                        _returnValue = new BaseTypes.Double(doubleVal1.Value + doubleVal2.Value);
                    }
                    break;
                case BaseTypes.String stringVal1 when val2 is BaseTypes.String stringVal2:
                    _returnValue = new BaseTypes.String(stringVal1.Value + stringVal2.Value);
                    break;
            }
        }
    }

    internal class SubtractionExpression<T> : AdditionExpression<T> where T : BaseTypes.Type
    {
        public SubtractionExpression(IValue<T> left, IValue<T> right) : base(left, right)
        {
        }

        public override void Execute(Scope context)
        {
            var val1 = _leftSide.Evaluate(context);
            var val2 = _rightSide.Evaluate(context);

            switch (val1)
            {
                case BaseTypes.Integer or BaseTypes.Double when val2 is BaseTypes.Integer or BaseTypes.Double:
                    // TODO Maybe change this in the future
                    if (val1 is BaseTypes.Integer intVal1 && val2 is BaseTypes.Integer intVal2)
                    {
                        _returnValue = new BaseTypes.Integer(intVal1.Value - intVal2.Value);
                    }
                    else if (val1 is BaseTypes.Double doubleVal1 && val2 is BaseTypes.Double doubleVal2)
                    {
                        _returnValue = new BaseTypes.Double(doubleVal1.Value - doubleVal2.Value);
                    }
                    break;
            }
        }
    }
}
