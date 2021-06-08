using System;
using System.IO;
using System.Diagnostics;

namespace BGS_Interpreter
{
    internal class Program
    {
        private const string grammarCgtFileName = "grammar.cgt";

        private static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                // Get path to file
                var pathToSource = args[0];

                // Initialiation of Parser
                var pathToGrammarCgt = Path.GetFullPath(grammarCgtFileName);
                var fileContent = new StreamReader(pathToSource).ReadToEnd();
                var parser = new MyParser(pathToGrammarCgt);

                try
                {
                    var program = parser.Parse(fileContent);

                    var interpreter = new Interpreter(program);
                    interpreter.Run();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Missing source file to interprete.");
            }
        }
    }
}