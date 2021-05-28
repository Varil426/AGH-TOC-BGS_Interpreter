using BGS_Interpreter.LanguageConcepts.Exceptions;
using BGS_Interpreter.LanguageConcepts.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    internal abstract class Function : LanguageObject, IExecutable
    {
        public abstract void Execute(Scope context);

        public string Name { get; protected init; }

        public Function(string name)
        {
            Name = name;
        }
    }

    class Function<T> : Function, IValue<T> where T : BaseTypes.Type
    {

        protected readonly IReadOnlyCollection<Declaration> _inputDeclarations;

        protected readonly IReadOnlyCollection<IExecutable> _executables;

        protected T _returnValue;

        protected IReadOnlyCollection<IValue> _inputValues;

        public Function(string name, Declaration[] inputs, IExecutable[] executables) : base(name)
        {
            _inputDeclarations = inputs;
            _executables = executables;
        }

        public void SetInputs(IValue[] inputs)
        {
            _inputValues = new ReadOnlyCollection<IValue>(inputs);
        }

        public T Evaluate()
        {
            Execute(null);
            return _returnValue;
        }

        public T Evaluate(Scope context)
        {
            Execute(context);
            return _returnValue;
        }

        public override void Execute(Scope context)
        {
            var functionScope = new Scope(context);
            var inputDeclarationsList = _inputDeclarations?.ToList() ?? new List<Declaration>();
            var inputValuesList = _inputValues?.ToList() ?? new List<IValue>();

            for (var i=0; i<_inputDeclarations.Count; i++)
            {
                var inputDeclaration = inputDeclarationsList[i];
                inputDeclaration.Execute(functionScope);
                var variable = functionScope.GetVariable(inputDeclaration.Name);
                switch (inputDeclaration.Type)
                {
                    case BaseTypes.Integer when variable is Variable<BaseTypes.Integer> var && inputValuesList[i] is IValue<BaseTypes.Integer> val:
                        var.Assign(val.Evaluate());
                        break;
                    case BaseTypes.Double when variable is Variable<BaseTypes.Double> var && inputValuesList[i] is IValue<BaseTypes.Double> val:
                        var.Assign(val.Evaluate());
                        break;
                    case BaseTypes.String when variable is Variable<BaseTypes.String> var && inputValuesList[i] is IValue<BaseTypes.String> val:
                        var.Assign(val.Evaluate());
                        break;
                    case BaseTypes.Boolean when variable is Variable<BaseTypes.Boolean> var && inputValuesList[i] is IValue<BaseTypes.Boolean> val:
                        var.Assign(val.Evaluate());
                        break;
                }
            }

            try
            {
                foreach (var executable in _executables)
                {
                    executable.Execute(functionScope);
                }
            }
            catch (ReturnException returnVal)
            {
                _returnValue = returnVal.ReturnValue as T;
            }
            var test = 123;
        }
    }
}
