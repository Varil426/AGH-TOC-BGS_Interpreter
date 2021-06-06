using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts.Loops
{
    internal class WhileLoop : LanguageObject, IExecutable
    {
        private readonly IReadOnlyCollection<IExecutable> _executables;

        private readonly IValue<BaseTypes.Boolean> _condition;

        public WhileLoop(IValue<BaseTypes.Boolean> condition, IExecutable[] executables)
        {
            _executables = executables.ToList().AsReadOnly();
            _condition = condition;
        }

        public void Execute(Scope context)
        {
            var loopContext = new Scope(context);

            while (_condition.Evaluate(context) is BaseTypes.Boolean result && result.Value)
            {
                _executables.ToList().ForEach(x => x.Execute(loopContext));
            }
        }
    }

    internal class ForLoop : LanguageObject, IExecutable
    {
        private readonly VariableIdentifier _indexer;

        private readonly IValue<BaseTypes.Integer> _start;

        private readonly IValue<BaseTypes.Integer> _end;

        private readonly IValue<BaseTypes.Integer> _step;

        private readonly IReadOnlyCollection<IExecutable> _executables;

        public ForLoop(VariableIdentifier variableIdentifier, IValue<BaseTypes.Integer> start, IValue<BaseTypes.Integer> end, IValue<BaseTypes.Integer> step, IExecutable[] executables)
        {
            _indexer = variableIdentifier;
            _start = start;
            _end = end;
            _step = step;
            _executables = executables.ToList().AsReadOnly();
        }

        public void Execute(Scope context)
        {
            var loopContext = new Scope(context);

            loopContext.AddVariable(new Variable<BaseTypes.Integer>(_indexer.Name));

            Func<int, int, bool> comparer = (a, b) => a < b;
            if (_start.Evaluate() is BaseTypes.Integer startValue && _end.Evaluate() is BaseTypes.Integer endValue)
            {
                if (startValue.Value < endValue.Value)
                {
                    comparer = (a, b) => a < b;
                }
                else
                {
                    comparer = (a, b) => b < a;
                }
            }

            var counter = loopContext.GetVariable(_indexer.Name);
            counter.Assign(_start.Evaluate());

            while (comparer.Invoke((counter.Evaluate() as BaseTypes.Integer).Value, (_end.Evaluate() as BaseTypes.Integer).Value))
            {
                _executables.ToList().ForEach(x => x.Execute(loopContext));

                counter.Assign(new BaseTypes.Integer((counter.Evaluate() as BaseTypes.Integer).Value + (_step.Evaluate() as BaseTypes.Integer).Value));
            }
        }
    }
}
