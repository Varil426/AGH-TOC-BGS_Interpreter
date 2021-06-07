using BGS_Interpreter.LanguageConcepts.Exceptions;
using BGS_Interpreter.LanguageConcepts.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    internal class FunctionCall<T> : Expression<T> where T : BaseTypes.Type
    {
        private readonly string _functionName;

        private readonly IValue[] _inputs;

        public FunctionCall(string functionName, IValue[] inputs)
        {
            _functionName = functionName;
            _inputs = inputs;
        }

        public override T Evaluate()
        {
            throw new Exception("Scope is needed to call a function.");
        }

        public override T Evaluate(Scope context)
        {
            Execute(context);
            return _returnValue as T ?? throw new Exception();
        }

        public override void Execute(Scope context)
        {
            var function = context.GetFunction(_functionName);
            if (function is Function<T> convertedFunction)
            {
                convertedFunction.SetInputs(_inputs);
                _returnValue = convertedFunction.Evaluate(context) as T;
            }
            else
            {
                throw new InvalidOperationException("Expected and found types don't match.");
            }
        }
    }
}
