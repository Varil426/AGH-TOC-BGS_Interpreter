
using System;
using System.IO;
using System.Runtime.Serialization;
using com.calitha.goldparser;
using com.calitha.commons;
using System.Collections.Generic;
using BGS_Interpreter.LanguageConcepts;
using BGS_Interpreter.LanguageConcepts.Expressions;

namespace BGS_Interpreter
{

    [Serializable()]
    public class SymbolException : System.Exception
    {
        public SymbolException(string message) : base(message)
        {
        }

        public SymbolException(string message,
            Exception inner) : base(message, inner)
        {
        }

        protected SymbolException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

    }

    [Serializable()]
    public class RuleException : System.Exception
    {

        public RuleException(string message) : base(message)
        {
        }

        public RuleException(string message,
                             Exception inner) : base(message, inner)
        {
        }

        protected RuleException(SerializationInfo info,
                                StreamingContext context) : base(info, context)
        {
        }

    }

    enum SymbolConstants : int
    {
        SYMBOL_EOF            =  0, // (EOF)
        SYMBOL_ERROR          =  1, // (Error)
        SYMBOL_WHITESPACE     =  2, // Whitespace
        SYMBOL_MINUS          =  3, // '-'
        SYMBOL_EXCLAMEQ       =  4, // '!='
        SYMBOL_AMPAMP         =  5, // '&&'
        SYMBOL_LPAREN         =  6, // '('
        SYMBOL_RPAREN         =  7, // ')'
        SYMBOL_TIMES          =  8, // '*'
        SYMBOL_COMMA          =  9, // ','
        SYMBOL_DIV            = 10, // '/'
        SYMBOL_COLON          = 11, // ':'
        SYMBOL_SEMI           = 12, // ';'
        SYMBOL_LBRACE         = 13, // '{'
        SYMBOL_PIPEPIPE       = 14, // '||'
        SYMBOL_RBRACE         = 15, // '}'
        SYMBOL_PLUS           = 16, // '+'
        SYMBOL_LT             = 17, // '<'
        SYMBOL_LTEQ           = 18, // '<='
        SYMBOL_EQ             = 19, // '='
        SYMBOL_EQEQ           = 20, // '=='
        SYMBOL_GT             = 21, // '>'
        SYMBOL_GTEQ           = 22, // '>='
        SYMBOL_BOOLEAN        = 23, // boolean
        SYMBOL_BOOLEANVAL     = 24, // BooleanVal
        SYMBOL_DOUBLE         = 25, // double
        SYMBOL_DOUBLEVAL      = 26, // DoubleVal
        SYMBOL_ELSE           = 27, // else
        SYMBOL_FOR            = 28, // for
        SYMBOL_FUNCTION       = 29, // function
        SYMBOL_IDENTIFIER     = 30, // Identifier
        SYMBOL_IF             = 31, // if
        SYMBOL_IN             = 32, // in
        SYMBOL_INT            = 33, // int
        SYMBOL_INTEGER        = 34, // Integer
        SYMBOL_PRINT          = 35, // print
        SYMBOL_RETURN         = 36, // return
        SYMBOL_STRING         = 37, // string
        SYMBOL_STRINGVAL      = 38, // StringVal
        SYMBOL_WHILE          = 39, // while
        SYMBOL_ADDEXP         = 40, // <AddExp>
        SYMBOL_CALLFUNC       = 41, // <callFunc>
        SYMBOL_DECLARATION    = 42, // <Declaration>
        SYMBOL_EXPRESSION     = 43, // <Expression>
        SYMBOL_FORSTATEMENT   = 44, // <ForStatement>
        SYMBOL_FUNCSTATMENT   = 45, // <FuncStatment>
        SYMBOL_IFSTATEMENT    = 46, // <IfStatement>
        SYMBOL_LOGICEXP       = 47, // <LogicExp>
        SYMBOL_MULEXP         = 48, // <MulExp>
        SYMBOL_NUMBER         = 49, // <Number>
        SYMBOL_PARAMS         = 50, // <Params>
        SYMBOL_PRINTSTMT      = 51, // <PrintStmt>
        SYMBOL_PROGRAM        = 52, // <Program>
        SYMBOL_RETVALUE       = 53, // <RetValue>
        SYMBOL_STATEMENT      = 54, // <Statement>
        SYMBOL_STATEMENTS     = 55, // <Statements>
        SYMBOL_TOCALLPARAM    = 56, // <toCallParam>
        SYMBOL_VALUE          = 57, // <Value>
        SYMBOL_WHILESTATEMENT = 58  // <WhileStatement>
    };

    enum RuleConstants : int
    {
        RULE_PROGRAM                                                                =  0, // <Program> ::= <Statements>
        RULE_VALUE_STRINGVAL                                                        =  1, // <Value> ::= StringVal
        RULE_VALUE_INTEGER                                                          =  2, // <Value> ::= Integer
        RULE_VALUE_DOUBLEVAL                                                        =  3, // <Value> ::= DoubleVal
        RULE_VALUE_IDENTIFIER                                                       =  4, // <Value> ::= Identifier
        RULE_VALUE_BOOLEANVAL                                                       =  5, // <Value> ::= BooleanVal
        RULE_NUMBER_INTEGER                                                         =  6, // <Number> ::= Integer
        RULE_NUMBER_DOUBLEVAL                                                       =  7, // <Number> ::= DoubleVal
        RULE_STATEMENT                                                              =  8, // <Statement> ::= <Declaration> <Statement>
        RULE_STATEMENT2                                                             =  9, // <Statement> ::= <IfStatement> <Statement>
        RULE_STATEMENT3                                                             = 10, // <Statement> ::= <WhileStatement> <Statement>
        RULE_STATEMENT4                                                             = 11, // <Statement> ::= <ForStatement> <Statement>
        RULE_STATEMENT5                                                             = 12, // <Statement> ::= <FuncStatment> <Statement>
        RULE_STATEMENT_SEMI                                                         = 13, // <Statement> ::= ';'
        RULE_STATEMENT_RETURN_SEMI                                                  = 14, // <Statement> ::= return <Expression> ';'
        RULE_STATEMENT6                                                             = 15, // <Statement> ::= <Expression> <Statement>
        RULE_STATEMENT7                                                             = 16, // <Statement> ::= <PrintStmt> <Statement>
        RULE_STATEMENT8                                                             = 17, // <Statement> ::= <Declaration>
        RULE_STATEMENT9                                                             = 18, // <Statement> ::= <IfStatement>
        RULE_STATEMENT10                                                            = 19, // <Statement> ::= <WhileStatement>
        RULE_STATEMENT11                                                            = 20, // <Statement> ::= <ForStatement>
        RULE_STATEMENT12                                                            = 21, // <Statement> ::= <FuncStatment>
        RULE_STATEMENT13                                                            = 22, // <Statement> ::= <Expression>
        RULE_STATEMENT14                                                            = 23, // <Statement> ::= <PrintStmt>
        RULE_STATEMENTS                                                             = 24, // <Statements> ::= <Statement> <Statements>
        RULE_STATEMENTS2                                                            = 25, // <Statements> ::= <Statement>
        RULE_PRINTSTMT_PRINT_LPAREN_RPAREN                                          = 26, // <PrintStmt> ::= print '(' <Expression> ')'
        RULE_IFSTATEMENT_IF_LPAREN_RPAREN_LBRACE_RBRACE                             = 27, // <IfStatement> ::= if '(' <Expression> ')' '{' <Statements> '}'
        RULE_IFSTATEMENT_IF_LPAREN_RPAREN_LBRACE_RBRACE_ELSE_LBRACE_RBRACE          = 28, // <IfStatement> ::= if '(' <Expression> ')' '{' <Statements> '}' else '{' <Statements> '}'
        RULE_FORSTATEMENT_FOR_IDENTIFIER_IN_LPAREN_COMMA_RPAREN_LBRACE_RBRACE       = 29, // <ForStatement> ::= for Identifier in '(' <Number> ',' <Number> ')' '{' <Statements> '}'
        RULE_FORSTATEMENT_FOR_IDENTIFIER_IN_LPAREN_COMMA_COMMA_RPAREN_LBRACE_RBRACE = 30, // <ForStatement> ::= for Identifier in '(' <Number> ',' <Number> ',' <Number> ')' '{' <Statements> '}'
        RULE_WHILESTATEMENT_WHILE_LPAREN_RPAREN_LBRACE_RBRACE                       = 31, // <WhileStatement> ::= while '(' <Expression> ')' '{' <Statements> '}'
        RULE_RETVALUE_INT                                                           = 32, // <RetValue> ::= int
        RULE_RETVALUE_DOUBLE                                                        = 33, // <RetValue> ::= double
        RULE_RETVALUE_STRING                                                        = 34, // <RetValue> ::= string
        RULE_RETVALUE_BOOLEAN                                                       = 35, // <RetValue> ::= boolean
        RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_LBRACE_RBRACE           = 36, // <FuncStatment> ::= function Identifier '(' <Params> ')' '{' <Statements> '}'
        RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_LBRACE_RBRACE2          = 37, // <FuncStatment> ::= function Identifier '(' ')' '{' <Statements> '}'
        RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_COLON_LBRACE_RBRACE     = 38, // <FuncStatment> ::= function Identifier '(' <Params> ')' ':' <RetValue> '{' <Statements> '}'
        RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_COLON_LBRACE_RBRACE2    = 39, // <FuncStatment> ::= function Identifier '(' ')' ':' <RetValue> '{' <Statements> '}'
        RULE_PARAMS                                                                 = 40, // <Params> ::= <Declaration>
        RULE_PARAMS_COMMA                                                           = 41, // <Params> ::= <Declaration> ',' <Params>
        RULE_DECLARATION_INT_IDENTIFIER                                             = 42, // <Declaration> ::= int Identifier
        RULE_DECLARATION_DOUBLE_IDENTIFIER                                          = 43, // <Declaration> ::= double Identifier
        RULE_DECLARATION_STRING_IDENTIFIER                                          = 44, // <Declaration> ::= string Identifier
        RULE_DECLARATION_BOOLEAN_BOOLEANVAL                                         = 45, // <Declaration> ::= boolean BooleanVal
        RULE_DECLARATION_STRING_IDENTIFIER_EQ                                       = 46, // <Declaration> ::= string Identifier '=' <Expression>
        RULE_DECLARATION_DOUBLE_IDENTIFIER_EQ                                       = 47, // <Declaration> ::= double Identifier '=' <Expression>
        RULE_DECLARATION_INT_IDENTIFIER_EQ                                          = 48, // <Declaration> ::= int Identifier '=' <Expression>
        RULE_EXPRESSION_GT                                                          = 49, // <Expression> ::= <Expression> '>' <LogicExp>
        RULE_EXPRESSION_LT                                                          = 50, // <Expression> ::= <Expression> '<' <LogicExp>
        RULE_EXPRESSION_GTEQ                                                        = 51, // <Expression> ::= <Expression> '>=' <LogicExp>
        RULE_EXPRESSION_LTEQ                                                        = 52, // <Expression> ::= <Expression> '<=' <LogicExp>
        RULE_EXPRESSION_EQEQ                                                        = 53, // <Expression> ::= <Expression> '==' <LogicExp>
        RULE_EXPRESSION_EXCLAMEQ                                                    = 54, // <Expression> ::= <Expression> '!=' <LogicExp>
        RULE_EXPRESSION_EQ                                                          = 55, // <Expression> ::= <Expression> '=' <LogicExp>
        RULE_EXPRESSION                                                             = 56, // <Expression> ::= <LogicExp>
        RULE_LOGICEXP_AMPAMP                                                        = 57, // <LogicExp> ::= <LogicExp> '&&' <AddExp>
        RULE_LOGICEXP_PIPEPIPE                                                      = 58, // <LogicExp> ::= <LogicExp> '||' <AddExp>
        RULE_LOGICEXP                                                               = 59, // <LogicExp> ::= <AddExp>
        RULE_ADDEXP_PLUS                                                            = 60, // <AddExp> ::= <AddExp> '+' <MulExp>
        RULE_ADDEXP_MINUS                                                           = 61, // <AddExp> ::= <AddExp> '-' <MulExp>
        RULE_ADDEXP                                                                 = 62, // <AddExp> ::= <MulExp>
        RULE_MULEXP_TIMES                                                           = 63, // <MulExp> ::= <MulExp> '*' <Value>
        RULE_MULEXP_DIV                                                             = 64, // <MulExp> ::= <MulExp> '/' <Value>
        RULE_MULEXP                                                                 = 65, // <MulExp> ::= <Value>
        RULE_MULEXP2                                                                = 66, // <MulExp> ::= <callFunc>
        RULE_TOCALLPARAM_IDENTIFIER                                                 = 67, // <toCallParam> ::= Identifier
        RULE_TOCALLPARAM_IDENTIFIER_COMMA                                           = 68, // <toCallParam> ::= Identifier ',' <toCallParam>
        RULE_CALLFUNC_IDENTIFIER_LPAREN_RPAREN                                      = 69, // <callFunc> ::= Identifier '(' <toCallParam> ')'
        RULE_CALLFUNC_IDENTIFIER_LPAREN_RPAREN2                                     = 70  // <callFunc> ::= Identifier '(' ')'
    };

    internal class MyParser
    {
        private LALRParser parser;

        public MyParser(string pathToFile)
        {
            FileStream stream = new FileStream(pathToFile,
                                               FileMode.Open, 
                                               FileAccess.Read, 
                                               FileShare.Read);
            Init(stream);
            stream.Close();
        }

        public MyParser(string baseName, string resourceName)
        {
            byte[] buffer = ResourceUtil.GetByteArrayResource(
                System.Reflection.Assembly.GetExecutingAssembly(),
                baseName,
                resourceName);
            MemoryStream stream = new MemoryStream(buffer);
            Init(stream);
            stream.Close();
        }

        public MyParser(Stream stream)
        {
            Init(stream);
        }

        private void Init(Stream stream)
        {
            CGTReader reader = new CGTReader(stream);
            parser = reader.CreateNewParser();
            parser.TrimReductions = false;
            parser.StoreTokens = LALRParser.StoreTokensMode.NoUserObject;

            parser.OnTokenError += new LALRParser.TokenErrorHandler(TokenErrorEvent);
            parser.OnParseError += new LALRParser.ParseErrorHandler(ParseErrorEvent);
        }

        public LanguageConcepts.Program Parse(string source)
        {
            NonterminalToken token = parser.Parse(source);
            if (token != null)
            {
                Object obj = CreateObject(token);
                //todo: Use your object any way you like
                if (obj is LanguageConcepts.Program program)
                {
                    return program;
                }

            }

            throw new Exception();
        }

        private Object CreateObject(Token token)
        {
            if (token is TerminalToken)
                return CreateObjectFromTerminal((TerminalToken)token);
            else
                return CreateObjectFromNonterminal((NonterminalToken)token);
        }

        private Object CreateObjectFromTerminal(TerminalToken token)
        {
            switch (token.Symbol.Id)
            {
                case (int)SymbolConstants.SYMBOL_EOF :
                //(EOF)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ERROR :
                //(Error)
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHITESPACE :
                //Whitespace
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MINUS :
                //'-'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXCLAMEQ :
                //'!='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_AMPAMP :
                //'&&'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LPAREN :
                //'('
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RPAREN :
                //')'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TIMES :
                //'*'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COMMA :
                //','
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DIV :
                //'/'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_COLON :
                //':'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_SEMI :
                //';'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LBRACE :
                //'{'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PIPEPIPE :
                //'||'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RBRACE :
                //'}'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PLUS :
                //'+'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LT :
                //'<'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LTEQ :
                //'<='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQ :
                //'='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EQEQ :
                //'=='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GT :
                //'>'
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_GTEQ :
                //'>='
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BOOLEAN :
                //boolean
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_BOOLEANVAL :
                    //BooleanVal
                    //todo: Create a new object that corresponds to the symbol
                    return new LanguageConcepts.Constant<LanguageConcepts.BaseTypes.Boolean>(new LanguageConcepts.BaseTypes.Boolean(bool.Parse(token.Text)));

                case (int)SymbolConstants.SYMBOL_DOUBLE :
                //double
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DOUBLEVAL :
                    //DoubleVal
                    //todo: Create a new object that corresponds to the symbol
                    return new LanguageConcepts.Constant<LanguageConcepts.BaseTypes.Double>(new LanguageConcepts.BaseTypes.Double(double.Parse(token.Text)));

                case (int)SymbolConstants.SYMBOL_ELSE :
                //else
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FOR :
                //for
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FUNCTION :
                //function
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IDENTIFIER :
                    //Identifier
                    //todo: Create a new object that corresponds to the symbol
                    return token.Text;

                case (int)SymbolConstants.SYMBOL_IF :
                //if
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IN :
                //in
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INT :
                //int
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_INTEGER :
                //Integer
                //todo: Create a new object that corresponds to the symbol
                    return new LanguageConcepts.Constant<LanguageConcepts.BaseTypes.Integer>(new LanguageConcepts.BaseTypes.Integer(int.Parse(token.Text)));

                case (int)SymbolConstants.SYMBOL_PRINT :
                //print
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RETURN :
                //return
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STRING :
                //string
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STRINGVAL :
                    //StringVal
                    //todo: Create a new object that corresponds to the symbol
                    return new LanguageConcepts.Constant<LanguageConcepts.BaseTypes.String>(new LanguageConcepts.BaseTypes.String(token.Text.Replace("\"", string.Empty)));

                case (int)SymbolConstants.SYMBOL_WHILE :
                //while
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_ADDEXP :
                //<AddExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_CALLFUNC :
                //<callFunc>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_DECLARATION :
                //<Declaration>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_EXPRESSION :
                //<Expression>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FORSTATEMENT :
                //<ForStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_FUNCSTATMENT :
                //<FuncStatment>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_IFSTATEMENT :
                //<IfStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_LOGICEXP :
                //<LogicExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_MULEXP :
                //<MulExp>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_NUMBER :
                //<Number>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PARAMS :
                //<Params>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PRINTSTMT :
                //<PrintStmt>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_PROGRAM :
                //<Program>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_RETVALUE :
                //<RetValue>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STATEMENT :
                //<Statement>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_STATEMENTS :
                //<Statements>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_TOCALLPARAM :
                //<toCallParam>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_VALUE :
                //<Value>
                //todo: Create a new object that corresponds to the symbol
                return null;

                case (int)SymbolConstants.SYMBOL_WHILESTATEMENT :
                //<WhileStatement>
                //todo: Create a new object that corresponds to the symbol
                return null;

            }
            throw new SymbolException("Unknown symbol");
        }

        public Object CreateObjectFromNonterminal(NonterminalToken token)
        {
            switch (token.Rule.Id)
            {
                case (int)RuleConstants.RULE_PROGRAM :
                    {
                        //<Program> ::= <Statements>
                        //todo: Create a new object using the stored tokens.
                        var executables = CreateObject(token.Tokens[0]) as List<IExecutable>;
                        var program = new LanguageConcepts.Program(executables.ToArray());
                        return program;
                    }

                case (int)RuleConstants.RULE_VALUE_STRINGVAL :
                    //<Value> ::= StringVal
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_VALUE_INTEGER :
                //<Value> ::= Integer
                //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_VALUE_DOUBLEVAL :
                    //<Value> ::= DoubleVal
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_VALUE_IDENTIFIER :
                //<Value> ::= Identifier
                //todo: Create a new object using the stored tokens.
                    return new VariableIdentifier(CreateObject(token.Tokens[0]) as string);

                case (int)RuleConstants.RULE_VALUE_BOOLEANVAL :
                    //<Value> ::= BooleanVal
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_NUMBER_INTEGER :
                //<Number> ::= Integer
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_NUMBER_DOUBLEVAL :
                //<Number> ::= DoubleVal
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT :
                    {
                        //<Statement> ::= <Declaration> <Statement>
                        //todo: Create a new object using the stored tokens.
                        var executablesList = new List<IExecutable>();
                        for (var i = 0; i < token.Tokens.Length; i++)
                        {
                            var nextObject = CreateObject(token.Tokens[i]);
                            if (nextObject is IExecutable executable)
                            {
                                executablesList.Add(executable);
                            }
                            else if (nextObject is List<IExecutable> nextExecutableList)
                            {
                                executablesList.AddRange(nextExecutableList);
                            }
                        }
                        return executablesList;
                    }

                case (int)RuleConstants.RULE_STATEMENT2 :
                //<Statement> ::= <IfStatement> <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT3 :
                //<Statement> ::= <WhileStatement> <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT4 :
                //<Statement> ::= <ForStatement> <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT5 :
                //<Statement> ::= <FuncStatment> <Statement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT_SEMI :
                //<Statement> ::= ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT_RETURN_SEMI :
                //<Statement> ::= return <Expression> ';'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT6 :
                //<Statement> ::= <Expression> <Statement>
                //todo: Create a new object using the stored tokens.
                return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_STATEMENT7 :
                //<Statement> ::= <PrintStmt> <Statement>
                //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_STATEMENT8 :
                //<Statement> ::= <Declaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT9 :
                //<Statement> ::= <IfStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT10 :
                //<Statement> ::= <WhileStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT11 :
                //<Statement> ::= <ForStatement>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT12 :
                //<Statement> ::= <FuncStatment>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT13 :
                //<Statement> ::= <Expression>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENT14 :
                //<Statement> ::= <PrintStmt>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_STATEMENTS :
                    {
                        //<Statements> ::= <Statement> <Statements>
                        //todo: Create a new object using the stored tokens.
                        var executablesList = new List<IExecutable>();

                        var statementResult = CreateObject(token.Tokens[0]);
                        if (statementResult is List<IExecutable>)
                        {
                            executablesList.AddRange(statementResult as List<IExecutable>);
                        }
                        else if (statementResult is IExecutable)
                        {
                            executablesList.Add(statementResult as IExecutable);
                        }

                        var restOfTheStatement = CreateObject(token.Tokens[1]);
                        if (restOfTheStatement is List<IExecutable>)
                        {
                            executablesList.AddRange(restOfTheStatement as List<IExecutable>);
                        }
                        else if (restOfTheStatement is IExecutable)
                        {
                            executablesList.Add(restOfTheStatement as IExecutable);
                        }

                        return executablesList;
                    }

                case (int)RuleConstants.RULE_STATEMENTS2 :
                    {
                        //<Statements> ::= <Statement>
                        //todo: Create a new object using the stored tokens.
                        var executablesList  = new List<IExecutable>();
                        for (var i = 0; i < token.Tokens.Length; i++)
                        {
                            var nextObject = CreateObject(token.Tokens[i]);
                            if (nextObject is IExecutable executable)
                            {
                                executablesList.Add(executable);
                            }
                            else if (nextObject is List<IExecutable> nextExecutableList)
                            {
                                executablesList.AddRange(nextExecutableList);
                            }
                        }
                        return executablesList;
                    }

                case (int)RuleConstants.RULE_PRINTSTMT_PRINT_LPAREN_RPAREN :
                    //<PrintStmt> ::= print '(' <Expression> ')'
                    //todo: Create a new object using the stored tokens.
                    {
                        var returnedObject = CreateObject(token.Tokens[2]);
                        if (returnedObject is IValue value)
                        {
                            return new Print(value);
                        }
                        throw new Exception();
                    }

                case (int)RuleConstants.RULE_IFSTATEMENT_IF_LPAREN_RPAREN_LBRACE_RBRACE :
                //<IfStatement> ::= if '(' <Expression> ')' '{' <Statements> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_IFSTATEMENT_IF_LPAREN_RPAREN_LBRACE_RBRACE_ELSE_LBRACE_RBRACE :
                //<IfStatement> ::= if '(' <Expression> ')' '{' <Statements> '}' else '{' <Statements> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENT_FOR_IDENTIFIER_IN_LPAREN_COMMA_RPAREN_LBRACE_RBRACE :
                //<ForStatement> ::= for Identifier in '(' <Number> ',' <Number> ')' '{' <Statements> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FORSTATEMENT_FOR_IDENTIFIER_IN_LPAREN_COMMA_COMMA_RPAREN_LBRACE_RBRACE :
                //<ForStatement> ::= for Identifier in '(' <Number> ',' <Number> ',' <Number> ')' '{' <Statements> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_WHILESTATEMENT_WHILE_LPAREN_RPAREN_LBRACE_RBRACE :
                //<WhileStatement> ::= while '(' <Expression> ')' '{' <Statements> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_RETVALUE_INT :
                //<RetValue> ::= int
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_RETVALUE_DOUBLE :
                //<RetValue> ::= double
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_RETVALUE_STRING :
                //<RetValue> ::= string
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_RETVALUE_BOOLEAN :
                //<RetValue> ::= boolean
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_LBRACE_RBRACE :
                //<FuncStatment> ::= function Identifier '(' <Params> ')' '{' <Statements> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_LBRACE_RBRACE2 :
                //<FuncStatment> ::= function Identifier '(' ')' '{' <Statements> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_COLON_LBRACE_RBRACE :
                //<FuncStatment> ::= function Identifier '(' <Params> ')' ':' <RetValue> '{' <Statements> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_COLON_LBRACE_RBRACE2 :
                //<FuncStatment> ::= function Identifier '(' ')' ':' <RetValue> '{' <Statements> '}'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PARAMS :
                //<Params> ::= <Declaration>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_PARAMS_COMMA :
                //<Params> ::= <Declaration> ',' <Params>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_DECLARATION_INT_IDENTIFIER :
                    //<Declaration> ::= int Identifier
                    //todo: Create a new object using the stored tokens.
                    return new VariableDeclaration<LanguageConcepts.BaseTypes.Integer>(new LanguageConcepts.BaseTypes.Integer(0), token.Tokens[1].ToString());

                case (int)RuleConstants.RULE_DECLARATION_DOUBLE_IDENTIFIER :
                    //<Declaration> ::= double Identifier
                    //todo: Create a new object using the stored tokens.
                    return new VariableDeclaration<LanguageConcepts.BaseTypes.Double>(new LanguageConcepts.BaseTypes.Double(0), token.Tokens[1].ToString());

                case (int)RuleConstants.RULE_DECLARATION_STRING_IDENTIFIER :
                    //<Declaration> ::= string Identifier
                    //todo: Create a new object using the stored tokens.
                    return new VariableDeclaration<LanguageConcepts.BaseTypes.String>(new LanguageConcepts.BaseTypes.String(string.Empty), token.Tokens[1].ToString());

                case (int)RuleConstants.RULE_DECLARATION_BOOLEAN_BOOLEANVAL :
                    //<Declaration> ::= boolean BooleanVal
                    //todo: Create a new object using the stored tokens.
                    return new VariableDeclaration<LanguageConcepts.BaseTypes.Boolean>(new LanguageConcepts.BaseTypes.Boolean(false), token.Tokens[1].ToString());

                case (int)RuleConstants.RULE_DECLARATION_STRING_IDENTIFIER_EQ :
                    //<Declaration> ::= string Identifier '=' <Expression>
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = CreateObject(token.Tokens[1]) as string;
                        var value = CreateObject(token.Tokens[3]) as IValue<LanguageConcepts.BaseTypes.String>;
                        if (name is not null && value is not null)
                        {
                            return new LanguageConcepts.VariableDeclaration<LanguageConcepts.BaseTypes.String>(new LanguageConcepts.BaseTypes.String(string.Empty), name, value);
                        }
                        throw new Exception();
                    }

                case (int)RuleConstants.RULE_DECLARATION_DOUBLE_IDENTIFIER_EQ :
                    //<Declaration> ::= double Identifier '=' <Expression>
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = CreateObject(token.Tokens[1]) as string;
                        var value = CreateObject(token.Tokens[3]) as IValue<LanguageConcepts.BaseTypes.Double>;
                        if (name is not null && value is not null)
                        {
                            return new LanguageConcepts.VariableDeclaration<LanguageConcepts.BaseTypes.Double>(new LanguageConcepts.BaseTypes.Double(0), name, value);
                        }
                        throw new Exception();
                    }

                case (int)RuleConstants.RULE_DECLARATION_INT_IDENTIFIER_EQ :
                    //<Declaration> ::= int Identifier '=' <Expression>
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = CreateObject(token.Tokens[1]) as string;
                        var value = CreateObject(token.Tokens[3]) as IValue<LanguageConcepts.BaseTypes.Integer>;
                        if (name is not null && value is not null)
                        {
                            return new LanguageConcepts.VariableDeclaration<LanguageConcepts.BaseTypes.Integer>(new LanguageConcepts.BaseTypes.Integer(0), name, value);
                        }
                        throw new Exception();
                    }

                case (int)RuleConstants.RULE_EXPRESSION_GT :
                //<Expression> ::= <Expression> '>' <LogicExp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_LT :
                //<Expression> ::= <Expression> '<' <LogicExp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_GTEQ :
                //<Expression> ::= <Expression> '>=' <LogicExp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_LTEQ :
                //<Expression> ::= <Expression> '<=' <LogicExp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_EQEQ :
                //<Expression> ::= <Expression> '==' <LogicExp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_EXCLAMEQ :
                //<Expression> ::= <Expression> '!=' <LogicExp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_EXPRESSION_EQ :
                    //<Expression> ::= <Expression> '=' <LogicExp>
                    //todo: Create a new object using the stored tokens.
                    {
                        var variableIdentifier = CreateObject(token.Tokens[0]) as VariableIdentifier;
                        var value = CreateObject(token.Tokens[2]);
                        switch (value)
                        {
                            case IValue<LanguageConcepts.BaseTypes.Integer> switchValue:
                                return new LanguageConcepts.Expressions.AssignmentExpression<LanguageConcepts.BaseTypes.Integer>(variableIdentifier, switchValue);
                            case IValue<LanguageConcepts.BaseTypes.Double> switchValue:
                                return new LanguageConcepts.Expressions.AssignmentExpression<LanguageConcepts.BaseTypes.Double>(variableIdentifier, switchValue);
                            case IValue<LanguageConcepts.BaseTypes.String> switchValue:
                                return new LanguageConcepts.Expressions.AssignmentExpression<LanguageConcepts.BaseTypes.String>(variableIdentifier, switchValue);
                            case IValue<LanguageConcepts.BaseTypes.Boolean> switchValue:
                                return new LanguageConcepts.Expressions.AssignmentExpression<LanguageConcepts.BaseTypes.Boolean>(variableIdentifier, switchValue);
                            case VariableIdentifier switchValue:
                                return new LanguageConcepts.Expressions.AssignmentExpression<LanguageConcepts.BaseTypes.Type>(variableIdentifier, switchValue);
                        }
                        throw new Exception();
                    }

                case (int)RuleConstants.RULE_EXPRESSION :
                    //<Expression> ::= <LogicExp>
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_LOGICEXP_AMPAMP :
                //<LogicExp> ::= <LogicExp> '&&' <AddExp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LOGICEXP_PIPEPIPE :
                //<LogicExp> ::= <LogicExp> '||' <AddExp>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_LOGICEXP :
                    //<LogicExp> ::= <AddExp>
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_ADDEXP_PLUS :
                    //<AddExp> ::= <AddExp> '+' <MulExp>
                    //todo: Create a new object using the stored tokens.
                    {
                        var left = CreateObject(token.Tokens[0]) as IValue;
                        var right = CreateObject(token.Tokens[2]) as IValue;
                        if (left is not null && right is not null)
                        {
                            switch (left)
                            {
                                case IValue<LanguageConcepts.BaseTypes.Integer> val1 when right is IValue<LanguageConcepts.BaseTypes.Integer> val2:
                                    return new AdditionExpression<LanguageConcepts.BaseTypes.Integer>(val1, val2);
                                case IValue<LanguageConcepts.BaseTypes.Double> val1 when right is IValue < LanguageConcepts.BaseTypes.Double> val2:
                                    return new AdditionExpression<LanguageConcepts.BaseTypes.Double>(val1, val2);
                                case IValue<LanguageConcepts.BaseTypes.String> val1 when right is IValue<LanguageConcepts.BaseTypes.String> val2:
                                    return new AdditionExpression<LanguageConcepts.BaseTypes.String>(val1, val2);
                            }
                        }
                        throw new Exception();
                    }

                case (int)RuleConstants.RULE_ADDEXP_MINUS :
                    //<AddExp> ::= <AddExp> '-' <MulExp>
                    //todo: Create a new object using the stored tokens.
                    {
                        var left = CreateObject(token.Tokens[0]) as IValue;
                        var right = CreateObject(token.Tokens[2]) as IValue;
                        if (left is not null && right is not null)
                        {
                            switch (left)
                            {
                                case IValue<LanguageConcepts.BaseTypes.Integer> val1 when right is IValue<LanguageConcepts.BaseTypes.Integer> val2:
                                    return new SubtractionExpression<LanguageConcepts.BaseTypes.Integer>(val1, val2);
                                case IValue<LanguageConcepts.BaseTypes.Double> val1 when right is IValue<LanguageConcepts.BaseTypes.Double> val2:
                                    return new SubtractionExpression<LanguageConcepts.BaseTypes.Double>(val1, val2);
                            }
                        }
                        throw new Exception();
                    }

                case (int)RuleConstants.RULE_ADDEXP :
                    //<AddExp> ::= <MulExp>
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_MULEXP_TIMES :
                //<MulExp> ::= <MulExp> '*' <Value>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MULEXP_DIV :
                //<MulExp> ::= <MulExp> '/' <Value>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_MULEXP :
                    //<MulExp> ::= <Value>
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_MULEXP2 :
                //<MulExp> ::= <callFunc>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TOCALLPARAM_IDENTIFIER :
                //<toCallParam> ::= Identifier
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_TOCALLPARAM_IDENTIFIER_COMMA :
                //<toCallParam> ::= Identifier ',' <toCallParam>
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CALLFUNC_IDENTIFIER_LPAREN_RPAREN :
                //<callFunc> ::= Identifier '(' <toCallParam> ')'
                //todo: Create a new object using the stored tokens.
                return null;

                case (int)RuleConstants.RULE_CALLFUNC_IDENTIFIER_LPAREN_RPAREN2 :
                //<callFunc> ::= Identifier '(' ')'
                //todo: Create a new object using the stored tokens.
                return null;
            }
            throw new RuleException("Unknown rule");
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args)
        {
            string message = "Token error with input: '"+args.Token.ToString()+"'";
            //todo: Report message to UI?
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args)
        {
            string message = "Parse error caused by token: '"+args.UnexpectedToken.ToString()+"'";
            //todo: Report message to UI?
        }

    }
}
