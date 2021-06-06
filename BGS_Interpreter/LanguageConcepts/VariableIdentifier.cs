using BGS_Interpreter.LanguageConcepts.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    abstract class VariableIdentifier : LanguageObject, IExecutable
    {
        public string Name { get; private init; }

        protected Variable _variable;
        public VariableIdentifier(string name)
        {
            Name = name;
        }

        public abstract void Execute(Scope context);

        public Variable GetVariable(Scope context)
        {
            return context.GetVariable(Name);
        }
    }

    class VariableIdentifier<T> : VariableIdentifier, IValue<T> where T : BaseTypes.Type
    {
        public VariableIdentifier(string name) : base(name)
        {
        }

        public override void Execute(Scope context)
        {
            _variable = context.GetVariable(Name);
        }

        public BaseTypes.Type Evaluate()
        {
            throw new Exception("Scope is needed to get a variable.");
        }

        public BaseTypes.Type Evaluate(Scope context)
        {
            Execute(context);
            return _variable.Evaluate(context);
        }
    }
}
