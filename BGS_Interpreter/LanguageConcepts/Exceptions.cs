using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts.Exceptions
{
    internal class ReturnException : Exception
    {
        public BaseTypes.Type ReturnValue { get; private init; }

        public ReturnException()
        {
        }

        public ReturnException(BaseTypes.Type returnValue)
        {
            ReturnValue = returnValue;
        }
    }
}
