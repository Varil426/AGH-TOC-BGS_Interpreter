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
            _program.Execute(null);
        }

    }
}
