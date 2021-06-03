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
        private readonly IValue _message;

        public Print(IValue message)
        {
            _message = message;
        }

        public void Execute(Scope context)
        {
            switch (_message.Evaluate(context))
            {
                case BaseTypes.Integer value:
                    System.Console.WriteLine(value.Value);
                    break;
                case BaseTypes.Double value:
                    System.Console.WriteLine(value.Value);
                    break;
                case BaseTypes.String value:
                    System.Console.WriteLine(value.Value);
                    break;
                case BaseTypes.Boolean value:
                    System.Console.WriteLine(value.Value);
                    break;
            }
            
        }
    }
}
