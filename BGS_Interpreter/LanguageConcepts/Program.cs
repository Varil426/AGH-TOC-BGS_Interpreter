using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGS_Interpreter.LanguageConcepts
{
    class Program : Function<BaseTypes.Void>
    {
        public Program(IExecutable[] executables) : base(string.Empty, Array.Empty<Declaration>(), executables)
        {
        }

        public override void Execute(Scope context)
        {
            base.Execute(null);
        }
    }
}
