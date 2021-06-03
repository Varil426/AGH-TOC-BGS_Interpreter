using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    class IfStatement : LanguageObject, IExecutable
    {
        private readonly IReadOnlyCollection<IExecutable> _executablesTrue;

        private readonly IReadOnlyCollection<IExecutable> _executablesFalse;

        private readonly IValue<BaseTypes.Boolean> _condition;

        public IfStatement(IValue<BaseTypes.Boolean> condtion, IExecutable[] ifTrue, IExecutable[] ifFalse)
        {
            _condition = condtion;
            _executablesTrue = new List<IExecutable>(ifTrue).AsReadOnly();
            _executablesFalse = new List<IExecutable>(ifFalse).AsReadOnly();
        }

        public IfStatement(IValue<BaseTypes.Boolean> condtion, IExecutable[] ifTrue) : this(condtion, ifTrue, Array.Empty<IExecutable>())
        {
        }

        public void Execute(Scope context)
        {
            var ifScope = new Scope(context);

            var result = _condition.Evaluate() as BaseTypes.Boolean;

            if (result.Value)
            {
                _executablesTrue.ToList().ForEach(x => x.Execute(ifScope));
            }
            else
            {
                _executablesFalse.ToList().ForEach(x => x.Execute(ifScope));
            }
        }
    }
}
