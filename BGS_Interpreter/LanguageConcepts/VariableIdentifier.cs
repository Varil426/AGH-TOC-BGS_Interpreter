using BGS_Interpreter.LanguageConcepts.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    class VariableIdentifier : LanguageObject, IExecutable, IValue
    {
        public string Name { get; private init; }

        private Variable _variable;

        public VariableIdentifier(string name)
        {
            Name = name;
        }

        public Variable GetVariable(Scope context)
        {
            return context.GetVariable(Name);
        }

        public void Execute(Scope context)
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
            return _variable.Evaluate();
        }
    }
}
