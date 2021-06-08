using System;

namespace BGS_Interpreter
{
    class Interpreter
    {

        private LanguageConcepts.Program _program;

        public Interpreter(LanguageConcepts.Program program)
        {
            _program = program;
        }

        public void Run()
        {
            try
            {
                _program.Execute(null);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
