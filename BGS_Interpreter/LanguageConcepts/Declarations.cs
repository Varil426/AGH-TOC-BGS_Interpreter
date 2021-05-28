using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    internal abstract class Declaration : LanguageObject, IExecutable
    {
        public string Name { get; private init; }

        public BaseTypes.Type Type { get; protected init; }

        public abstract void Execute(Scope scope);

        public Declaration(string name)
        {
            Name = name;
        }
    }

    internal class VariableDeclaration<T> : Declaration where T : BaseTypes.Type
    {
        public VariableDeclaration(BaseTypes.Type type, string name) : base(name)
        {
            Type = type;
        }

        public override void Execute(Scope scope)
        {
            switch (Type)
            {
                case BaseTypes.Integer:
                    scope.AddVariable(new Variable<BaseTypes.Integer>(Name));
                    break;
                case BaseTypes.Double:
                    scope.AddVariable(new Variable<BaseTypes.Double>(Name));
                    break;
                case BaseTypes.Boolean:
                    scope.AddVariable(new Variable<BaseTypes.Boolean>(Name));
                    break;
                case BaseTypes.String:
                    scope.AddVariable(new Variable<BaseTypes.String>(Name));
                    break;
                default:
                    throw new InvalidOperationException("Unknow type.");
            }
        }
    }

    internal class FunctionDeclaration<T> : Declaration where T : BaseTypes.Type
    {
        protected readonly IReadOnlyCollection<Declaration> _inputDeclarations;

        protected readonly IReadOnlyCollection<IExecutable> _executables;

        public FunctionDeclaration(string name, Declaration[] inputs, IExecutable[] executables, BaseTypes.Type returnType) : base(name)
        {
            _inputDeclarations = new ReadOnlyCollection<Declaration>(inputs.ToList());
            _executables = new ReadOnlyCollection<IExecutable>(executables.ToList());
            Type = returnType;
        }

        public override void Execute(Scope scope)
        {
            switch (Type)
            {
                case BaseTypes.Integer:
                    scope.AddFunction(new Function<BaseTypes.Integer>(Name, _inputDeclarations.ToArray(), _executables.ToArray()));
                    break;
                case BaseTypes.Double:
                    scope.AddFunction(new Function<BaseTypes.Double>(Name, _inputDeclarations.ToArray(), _executables.ToArray()));
                    break;
                case BaseTypes.Boolean:
                    scope.AddFunction(new Function<BaseTypes.Boolean>(Name, _inputDeclarations.ToArray(), _executables.ToArray()));
                    break;
                case BaseTypes.String:
                    scope.AddFunction(new Function<BaseTypes.String>(Name, _inputDeclarations.ToArray(), _executables.ToArray()));
                    break;
                default:
                    throw new InvalidOperationException("Unknow type.");
            }
        }
    }
}
