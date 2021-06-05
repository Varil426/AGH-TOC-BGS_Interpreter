using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    internal abstract class Declaration : LanguageObject, IExecutable
    {
        public string Name { get; private init; }

        public BaseTypes.Type Type { get; protected init; }

        public abstract void Execute(Scope context);

        public Declaration(string name)
        {
            Name = name;
        }
    }

    internal class VariableDeclaration<T> : Declaration where T : BaseTypes.Type
    {
        private IValue<T> _initValue;

        public VariableDeclaration(BaseTypes.Type type, string name) : base(name)
        {
            Type = type;
        }

        public VariableDeclaration(BaseTypes.Type type, string name, IValue<T> value) : this(type, name)
        {
            _initValue = value;
        }

        public override void Execute(Scope context)
        {
            switch (Type)
            {
                case BaseTypes.Integer:
                    context.AddVariable(new Variable<BaseTypes.Integer>(Name));
                    break;
                case BaseTypes.Double:
                    context.AddVariable(new Variable<BaseTypes.Double>(Name));
                    break;
                case BaseTypes.Boolean:
                    context.AddVariable(new Variable<BaseTypes.Boolean>(Name));
                    break;
                case BaseTypes.String:
                    context.AddVariable(new Variable<BaseTypes.String>(Name));
                    break;
                default:
                    throw new InvalidOperationException("Unknow type.");
            }

            if (_initValue != null)
            {
                context.GetVariable(Name).Assign(_initValue.Evaluate());
            }
        }
    }

    internal class FunctionDeclaration<T> : Declaration where T : BaseTypes.Type
    {
        protected readonly IReadOnlyCollection<Declaration> _inputDeclarations;

        protected readonly IReadOnlyCollection<IExecutable> _executables;

        public FunctionDeclaration(string name, Declaration[] inputs, IExecutable[] executables, BaseTypes.Type returnType) : base(name)
        {
            _inputDeclarations = new List<Declaration>(inputs.ToList()).AsReadOnly();
            _executables = new List<IExecutable>(executables.ToList()).AsReadOnly();
            Type = returnType;
        }

        public override void Execute(Scope context)
        {
            switch (Type)
            {
                case BaseTypes.Integer:
                    context.AddFunction(new Function<BaseTypes.Integer>(Name, _inputDeclarations.ToArray(), _executables.ToArray()));
                    break;
                case BaseTypes.Double:
                    context.AddFunction(new Function<BaseTypes.Double>(Name, _inputDeclarations.ToArray(), _executables.ToArray()));
                    break;
                case BaseTypes.Boolean:
                    context.AddFunction(new Function<BaseTypes.Boolean>(Name, _inputDeclarations.ToArray(), _executables.ToArray()));
                    break;
                case BaseTypes.String:
                    context.AddFunction(new Function<BaseTypes.String>(Name, _inputDeclarations.ToArray(), _executables.ToArray()));
                    break;
                default:
                    throw new InvalidOperationException("Unknow type.");
            }
        }
    }
}
