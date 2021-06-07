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

            while (_condition.Evaluate(loopContext) is BaseTypes.Boolean result && result.Value)
            {
                var innerContext = new Scope(loopContext);

                _executables.ToList().ForEach(x => x.Execute(innerContext));
            }
        }
    }

    internal class ForLoop : LanguageObject, IExecutable
    {
        private readonly VariableIdentifier _indexer;

        private readonly IValue _start;

        private readonly IValue _end;

        private readonly IValue _step;

        private readonly IReadOnlyCollection<IExecutable> _executables;

        private bool IsNumericType(IValue value) => value is IValue<BaseTypes.Integer> or IValue<BaseTypes.Double>;

        public ForLoop(VariableIdentifier<BaseTypes.Double> variableIdentifier, IValue start, IValue end, IValue step, IExecutable[] executables)
        {
            if (!IsNumericType(start) || !IsNumericType(end) || !IsNumericType(step))
            {
                throw new Exception();
            }

            _indexer = variableIdentifier;
            _start = start;
            _end = end;
            _step = step;
            _executables = executables.ToList().AsReadOnly();
        }

        public ForLoop(VariableIdentifier<BaseTypes.Double> variableIdentifier, IValue start, IValue end, IExecutable[] executables) : this(variableIdentifier, start, end, new Constant<BaseTypes.Double>(new BaseTypes.Double(1)), executables)
        {
        }

        public void Execute(Scope context)
        {
            var loopContext = new Scope(context);

            loopContext.AddVariable(new Variable<BaseTypes.Double>(_indexer.Name));

            dynamic start = _start.Evaluate(context);
            if (start is BaseTypes.Integer)
            {
                start = new BaseTypes.Double(start.Value);
            }

            dynamic end = _end.Evaluate(context);
            if (end is BaseTypes.Integer)
            {
                end = new BaseTypes.Double(end.Value);
            }

            dynamic step = _step.Evaluate(context);
            if (step is BaseTypes.Integer)
            {
                step = new BaseTypes.Double(step.Value);
            }

            Func<double, double, bool> comparer = start switch
            {
                BaseTypes.Type when start.Value < end.Value => (a, b) => a < b,
                _ => (a, b) => b < a,
            };

            dynamic counter = loopContext.GetVariable(_indexer.Name);
            counter.Assign(start);

            while (comparer.Invoke(counter.Evaluate(loopContext).Value, end.Value))
            {
                var innerContext = new Scope(loopContext);

                _executables.ToList().ForEach(x => x.Execute(innerContext));

                counter.Assign(new BaseTypes.Double(counter.Evaluate(innerContext).Value + step.Value));
            }
        }
    }
}
