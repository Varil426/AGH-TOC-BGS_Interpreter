using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    // TODO Think about it
    class Print : LanguageObject, IExecutable
    {
        private readonly string _message;

        public Print(string message)
        {
            _message = message;
        }

        public Print(BaseTypes.Integer value) : this($"{value.Value}")
        {
        }

        public Print(BaseTypes.Double value) : this($"{value.Value}")
        {
        }

        public Print(BaseTypes.Boolean value) : this($"{value.Value}")
        {
        }

        public Print(BaseTypes.String value) : this($"{value.Value}")
        {
        }

        public void Execute(Scope context)
        {
            System.Console.WriteLine(_message);
        }
    }
}
