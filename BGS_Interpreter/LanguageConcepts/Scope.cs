using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BGS_Interpreter.LanguageConcepts;

namespace BGS_Interpreter.LanguageConcepts
{
    internal class Scope
    {
        private readonly Dictionary<string, Variable> _variables = new();

        private readonly Dictionary<string, Function> _functions = new();

        private readonly Scope _outerScope;

        public Scope()
        {

        }

        public Scope(Scope outerScope)
        {
            _outerScope = outerScope;
        }

        public void AddVariable(Variable variable)
        {
            if (_variables.Keys.Any(x => x == variable.Name))
            {
                throw new ArgumentException("Variable with the same name is already defined in this scope.");
            }
            _variables[variable.Name] = variable;
        }

        public void AddFunction(Function function)
        {
            if (_functions.Keys.Any(x => x == function.Name))
            {
                throw new ArgumentException("Function with the same name is already defined in this scope.");
            }
            _functions[function.Name] = function;
        }

        public Variable GetVariable(string variableName)
        {
            try
            {
                return _variables[variableName];
            }
            catch(KeyNotFoundException)
            {
                // Ignore
            }

            if (_outerScope is not null)
            {
                return _outerScope.GetVariable(variableName);
            }

            throw new ArgumentException("Variable not found.");
        }

        public Function GetFunction(string functionName)
        {
            try
            {
                return _functions[functionName];
            }
            catch (KeyNotFoundException)
            {
                // Ignore
            }

            if (_outerScope is not null)
            {
                return _outerScope.GetFunction(functionName);
            }

            throw new ArgumentException("Function not found.");
        }

    }
}
