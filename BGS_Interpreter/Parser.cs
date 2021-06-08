
using System;
using System.IO;
using System.Linq;
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
        SYMBOL_EXCLAM         =  4, // '!'	
        SYMBOL_EXCLAMEQ       =  5, // '!='	
        SYMBOL_AMPAMP         =  6, // '&&'	
        SYMBOL_LPAREN         =  7, // '('	
        SYMBOL_RPAREN         =  8, // ')'	
        SYMBOL_TIMES          =  9, // '*'	
        SYMBOL_COMMA          = 10, // ','	
        SYMBOL_DIV            = 11, // '/'	
        SYMBOL_COLON          = 12, // ':'	
        SYMBOL_SEMI           = 13, // ';'	
        SYMBOL_LBRACE         = 14, // '{'	
        SYMBOL_PIPEPIPE       = 15, // '||'	
        SYMBOL_RBRACE         = 16, // '}'	
        SYMBOL_PLUS           = 17, // '+'	
        SYMBOL_LT             = 18, // '<'	
        SYMBOL_LTEQ           = 19, // '<='	
        SYMBOL_EQ             = 20, // '='	
        SYMBOL_EQEQ           = 21, // '=='	
        SYMBOL_GT             = 22, // '>'	
        SYMBOL_GTEQ           = 23, // '>='	
        SYMBOL_BOOLEAN        = 24, // boolean	
        SYMBOL_BOOLEANVAL     = 25, // BooleanVal	
        SYMBOL_DOUBLE         = 26, // double	
        SYMBOL_DOUBLEVAL      = 27, // DoubleVal	
        SYMBOL_ELSE           = 28, // else	
        SYMBOL_FOR            = 29, // for	
        SYMBOL_FUNCTION       = 30, // function	
        SYMBOL_IDENTIFIER     = 31, // Identifier	
        SYMBOL_IF             = 32, // if	
        SYMBOL_IN             = 33, // in	
        SYMBOL_INT            = 34, // int	
        SYMBOL_INTEGER        = 35, // Integer	
        SYMBOL_PRINT          = 36, // print	
        SYMBOL_RETURN         = 37, // return	
        SYMBOL_STRING         = 38, // string	
        SYMBOL_STRINGVAL      = 39, // StringVal	
        SYMBOL_WHILE          = 40, // while	
        SYMBOL_ADDEXP         = 41, // <AddExp>	
        SYMBOL_CALLFUNC       = 42, // <callFunc>	
        SYMBOL_DECLARATION    = 43, // <Declaration>	
        SYMBOL_EXPRESSION     = 44, // <Expression>	
        SYMBOL_FORSTATEMENT   = 45, // <ForStatement>	
        SYMBOL_FUNCSTATMENT   = 46, // <FuncStatment>	
        SYMBOL_IFSTATEMENT    = 47, // <IfStatement>	
        SYMBOL_LOGICEXP       = 48, // <LogicExp>	
        SYMBOL_MULEXP         = 49, // <MulExp>	
        SYMBOL_NUMBER         = 50, // <Number>	
        SYMBOL_PARAMS         = 51, // <Params>	
        SYMBOL_PRINTSTMT      = 52, // <PrintStmt>	
        SYMBOL_PROGRAM        = 53, // <Program>	
        SYMBOL_RETVALUE       = 54, // <RetValue>	
        SYMBOL_STATEMENT      = 55, // <Statement>	
        SYMBOL_STATEMENTS     = 56, // <Statements>	
        SYMBOL_TOCALLPARAM    = 57, // <toCallParam>	
        SYMBOL_VALUE          = 58, // <Value>	
        SYMBOL_WHILESTATEMENT = 59  // <WhileStatement>
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
        RULE_STATEMENT_RETURN_SEMI                                                  =  8, // <Statement> ::= return <Expression> ';'	
        RULE_STATEMENT_SEMI                                                         =  9, // <Statement> ::= <Declaration> ';'	
        RULE_STATEMENT                                                              = 10, // <Statement> ::= <IfStatement>	
        RULE_STATEMENT2                                                             = 11, // <Statement> ::= <WhileStatement>	
        RULE_STATEMENT3                                                             = 12, // <Statement> ::= <ForStatement>	
        RULE_STATEMENT4                                                             = 13, // <Statement> ::= <FuncStatment>	
        RULE_STATEMENT_SEMI2                                                        = 14, // <Statement> ::= <Expression> ';'	
        RULE_STATEMENT_SEMI3                                                        = 15, // <Statement> ::= <PrintStmt> ';'	
        RULE_STATEMENTS                                                             = 16, // <Statements> ::= <Statement> <Statements>	
        RULE_STATEMENTS2                                                            = 17, // <Statements> ::= <Statement>	
        RULE_PRINTSTMT_PRINT_LPAREN_RPAREN                                          = 18, // <PrintStmt> ::= print '(' <Expression> ')'	
        RULE_IFSTATEMENT_IF_LPAREN_RPAREN_LBRACE_RBRACE                             = 19, // <IfStatement> ::= if '(' <Expression> ')' '{' <Statements> '}'	
        RULE_IFSTATEMENT_IF_LPAREN_RPAREN_LBRACE_RBRACE_ELSE_LBRACE_RBRACE          = 20, // <IfStatement> ::= if '(' <Expression> ')' '{' <Statements> '}' else '{' <Statements> '}'	
        RULE_FORSTATEMENT_FOR_IDENTIFIER_IN_LPAREN_COMMA_RPAREN_LBRACE_RBRACE       = 21, // <ForStatement> ::= for Identifier in '(' <Number> ',' <Number> ')' '{' <Statements> '}'	
        RULE_FORSTATEMENT_FOR_IDENTIFIER_IN_LPAREN_COMMA_COMMA_RPAREN_LBRACE_RBRACE = 22, // <ForStatement> ::= for Identifier in '(' <Number> ',' <Number> ',' <Number> ')' '{' <Statements> '}'	
        RULE_WHILESTATEMENT_WHILE_LPAREN_RPAREN_LBRACE_RBRACE                       = 23, // <WhileStatement> ::= while '(' <Expression> ')' '{' <Statements> '}'	
        RULE_RETVALUE_INT                                                           = 24, // <RetValue> ::= int	
        RULE_RETVALUE_DOUBLE                                                        = 25, // <RetValue> ::= double	
        RULE_RETVALUE_STRING                                                        = 26, // <RetValue> ::= string	
        RULE_RETVALUE_BOOLEAN                                                       = 27, // <RetValue> ::= boolean	
        RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_LBRACE_RBRACE           = 28, // <FuncStatment> ::= function Identifier '(' <Params> ')' '{' <Statements> '}'	
        RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_LBRACE_RBRACE2          = 29, // <FuncStatment> ::= function Identifier '(' ')' '{' <Statements> '}'	
        RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_COLON_LBRACE_RBRACE     = 30, // <FuncStatment> ::= function Identifier '(' <Params> ')' ':' <RetValue> '{' <Statements> '}'	
        RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_COLON_LBRACE_RBRACE2    = 31, // <FuncStatment> ::= function Identifier '(' ')' ':' <RetValue> '{' <Statements> '}'	
        RULE_PARAMS                                                                 = 32, // <Params> ::= <Declaration>	
        RULE_PARAMS_COMMA                                                           = 33, // <Params> ::= <Declaration> ',' <Params>	
        RULE_DECLARATION_INT_IDENTIFIER                                             = 34, // <Declaration> ::= int Identifier	
        RULE_DECLARATION_DOUBLE_IDENTIFIER                                          = 35, // <Declaration> ::= double Identifier	
        RULE_DECLARATION_STRING_IDENTIFIER                                          = 36, // <Declaration> ::= string Identifier	
        RULE_DECLARATION_BOOLEAN_IDENTIFIER                                         = 37, // <Declaration> ::= boolean Identifier	
        RULE_DECLARATION_STRING_IDENTIFIER_EQ                                       = 38, // <Declaration> ::= string Identifier '=' <Expression>	
        RULE_DECLARATION_DOUBLE_IDENTIFIER_EQ                                       = 39, // <Declaration> ::= double Identifier '=' <Expression>	
        RULE_DECLARATION_INT_IDENTIFIER_EQ                                          = 40, // <Declaration> ::= int Identifier '=' <Expression>	
        RULE_DECLARATION_BOOLEAN_IDENTIFIER_EQ                                      = 41, // <Declaration> ::= boolean Identifier '=' <Expression>	
        RULE_EXPRESSION_GT                                                          = 42, // <Expression> ::= <Expression> '>' <LogicExp>	
        RULE_EXPRESSION_LT                                                          = 43, // <Expression> ::= <Expression> '<' <LogicExp>	
        RULE_EXPRESSION_GTEQ                                                        = 44, // <Expression> ::= <Expression> '>=' <LogicExp>	
        RULE_EXPRESSION_LTEQ                                                        = 45, // <Expression> ::= <Expression> '<=' <LogicExp>	
        RULE_EXPRESSION_EQEQ                                                        = 46, // <Expression> ::= <Expression> '==' <LogicExp>	
        RULE_EXPRESSION_EXCLAMEQ                                                    = 47, // <Expression> ::= <Expression> '!=' <LogicExp>	
        RULE_EXPRESSION_EQ                                                          = 48, // <Expression> ::= <Expression> '=' <LogicExp>	
        RULE_EXPRESSION                                                             = 49, // <Expression> ::= <LogicExp>	
        RULE_LOGICEXP_AMPAMP                                                        = 50, // <LogicExp> ::= <LogicExp> '&&' <AddExp>	
        RULE_LOGICEXP_PIPEPIPE                                                      = 51, // <LogicExp> ::= <LogicExp> '||' <AddExp>	
        RULE_LOGICEXP                                                               = 52, // <LogicExp> ::= <AddExp>	
        RULE_ADDEXP_PLUS                                                            = 53, // <AddExp> ::= <AddExp> '+' <MulExp>	
        RULE_ADDEXP_MINUS                                                           = 54, // <AddExp> ::= <AddExp> '-' <MulExp>	
        RULE_ADDEXP                                                                 = 55, // <AddExp> ::= <MulExp>	
        RULE_MULEXP_TIMES                                                           = 56, // <MulExp> ::= <MulExp> '*' <Value>	
        RULE_MULEXP_DIV                                                             = 57, // <MulExp> ::= <MulExp> '/' <Value>	
        RULE_MULEXP                                                                 = 58, // <MulExp> ::= <Value>	
        RULE_MULEXP_EXCLAM                                                          = 59, // <MulExp> ::= '!' <Value>	
        RULE_MULEXP2                                                                = 60, // <MulExp> ::= <callFunc>	
        RULE_TOCALLPARAM_IDENTIFIER                                                 = 61, // <toCallParam> ::= Identifier	
        RULE_TOCALLPARAM_IDENTIFIER_COMMA                                           = 62, // <toCallParam> ::= Identifier ',' <toCallParam>	
        RULE_CALLFUNC_IDENTIFIER_LPAREN_RPAREN                                      = 63, // <callFunc> ::= Identifier '(' <toCallParam> ')'	
        RULE_CALLFUNC_IDENTIFIER_LPAREN_RPAREN2                                     = 64  // <callFunc> ::= Identifier '(' ')'
    };

    internal class MyParser
    {
        private LALRParser parser;

        private readonly Stack<Dictionary<string, System.Type>> variablesTypes = new();

        private readonly Stack<Dictionary<string, System.Type>> functionsTypes = new();

        private void CreateNewStackLevel()
        {
            variablesTypes.Push(new());
            functionsTypes.Push(new());
        }

        private void ReleaseStackLevel()
        {
            variablesTypes.Pop();
            functionsTypes.Pop();
        }

        private System.Type GetVariableType(string name)
        {
            return variablesTypes.SelectMany(x => x).First(x => x.Key == name).Value;
        }

        private System.Type GetFuntionType(string name)
        {
            return functionsTypes.SelectMany(x => x).First(x => x.Key == name).Value;
        }

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

            throw new Exception("Error during parsing.");
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

                case (int)SymbolConstants.SYMBOL_EXCLAM:
                //'!'
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
                        CreateNewStackLevel();
                        var executables = CreateObject(token.Tokens[0]) as List<IExecutable>;
                        var program = new LanguageConcepts.Program(executables.ToArray());
                        ReleaseStackLevel();
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
                    {
                        var name = CreateObject(token.Tokens[0]) as string;
                        var typeOfVariable = GetVariableType(name);
                        if (typeOfVariable == typeof(LanguageConcepts.BaseTypes.Integer))
                        {
                            return new VariableIdentifier<LanguageConcepts.BaseTypes.Integer>(name);
                        }
                        else if (typeOfVariable == typeof(LanguageConcepts.BaseTypes.Double))
                        {
                            return new VariableIdentifier<LanguageConcepts.BaseTypes.Double>(name);
                        }
                        else if (typeOfVariable == typeof(LanguageConcepts.BaseTypes.String))
                        {
                            return new VariableIdentifier<LanguageConcepts.BaseTypes.String>(name);
                        }
                        else if (typeOfVariable == typeof(LanguageConcepts.BaseTypes.Boolean))
                        {
                            return new VariableIdentifier<LanguageConcepts.BaseTypes.Boolean>(name);
                        }
                        throw new Exception("Value not accepted.");
                    }

                case (int)RuleConstants.RULE_VALUE_BOOLEANVAL :
                    //<Value> ::= BooleanVal
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_NUMBER_INTEGER :
                //<Number> ::= Integer
                //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_NUMBER_DOUBLEVAL :
                    //<Number> ::= DoubleVal
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_STATEMENT_RETURN_SEMI :
                    //<Statement> ::= return <Expression> ';'
                    //todo: Create a new object using the stored tokens.
                    { 
                        if (CreateObject(token.Tokens[1]) is IValue value)
                        {
                            switch (value)
                            {
                                case IValue<LanguageConcepts.BaseTypes.Boolean> val:
                                    return new ReturnStatement<LanguageConcepts.BaseTypes.Boolean>(val);
                                case IValue<LanguageConcepts.BaseTypes.Double> val:
                                    return new ReturnStatement<LanguageConcepts.BaseTypes.Double>(val);
                                case IValue<LanguageConcepts.BaseTypes.Integer> val:
                                    return new ReturnStatement<LanguageConcepts.BaseTypes.Integer>(val);
                                case IValue<LanguageConcepts.BaseTypes.String> val:
                                    return new ReturnStatement<LanguageConcepts.BaseTypes.String>(val);
                                default:
                                    throw new Exception("Return value is not correct.");
                            }
                        }
                        throw new Exception("Return value is not correct.");
                    }

                case (int)RuleConstants.RULE_STATEMENT_SEMI :
                    //<Statement> ::= <Declaration> ';'
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_STATEMENT :
                //<Statement> ::= <IfStatement>
                //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_STATEMENT2 :
                    //<Statement> ::= <WhileStatement>
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_STATEMENT3 :
                //<Statement> ::= <ForStatement>
                //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_STATEMENT4 :
                //<Statement> ::= <FuncStatment>
                //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_STATEMENT_SEMI2 :
                    //<Statement> ::= <Expression> ';'
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_STATEMENT_SEMI3 :
                //<Statement> ::= <PrintStmt> ';'
                //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

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
                        throw new Exception("Forbidden token in print statement.");
                    }

                case (int)RuleConstants.RULE_IFSTATEMENT_IF_LPAREN_RPAREN_LBRACE_RBRACE :
                    //<IfStatement> ::= if '(' <Expression> ')' '{' <Statements> '}'
                    //todo: Create a new object using the stored tokens.
                    {
                        CreateNewStackLevel();
                        var condition = CreateObject(token.Tokens[2]) as IValue<LanguageConcepts.BaseTypes.Boolean>;
                        var statements = CreateObject(token.Tokens[5]) as List<IExecutable>;
                        ReleaseStackLevel();
                        
                        if (condition is not null && statements is not null)
                        {
                            return new IfStatement(condition, statements.ToArray());
                        }
                        throw new Exception("Forbidden token in if statement.");
                    }

                case (int)RuleConstants.RULE_IFSTATEMENT_IF_LPAREN_RPAREN_LBRACE_RBRACE_ELSE_LBRACE_RBRACE :
                    //<IfStatement> ::= if '(' <Expression> ')' '{' <Statements> '}' else '{' <Statements> '}'
                    //todo: Create a new object using the stored tokens.
                    {
                        CreateNewStackLevel();
                        var condition = CreateObject(token.Tokens[2]) as IValue<LanguageConcepts.BaseTypes.Boolean>;
                        var statementsTrue = CreateObject(token.Tokens[5]) as List<IExecutable>;
                        var statementsFalse = CreateObject(token.Tokens[9]) as List<IExecutable>;
                        ReleaseStackLevel();

                        if (condition is not null && statementsTrue is not null && statementsFalse is not null)
                        {
                            return new IfStatement(condition, statementsTrue.ToArray(), statementsFalse.ToArray());
                        }
                        throw new Exception("Forbidden token in if-else statement.");
                    }

                case (int)RuleConstants.RULE_FORSTATEMENT_FOR_IDENTIFIER_IN_LPAREN_COMMA_RPAREN_LBRACE_RBRACE :
                    //<ForStatement> ::= for Identifier in '(' <Number> ',' <Number> ')' '{' <Statements> '}'
                    //todo: Create a new object using the stored tokens.
                    {
                        CreateNewStackLevel();
                        var name = CreateObject(token.Tokens[1]) as string;
                        variablesTypes.Peek()[name] = typeof(LanguageConcepts.BaseTypes.Double);
                        var identifier = new VariableIdentifier<LanguageConcepts.BaseTypes.Double>(name);
                        var start = CreateObject(token.Tokens[4]) as IValue;
                        var end = CreateObject(token.Tokens[6]) as IValue;
                        var statements = CreateObject(token.Tokens[9]) as List<IExecutable>;
                        ReleaseStackLevel();
                        if (name is not null && start is not null && end is not null && statements is not null)
                        {
                            return new LanguageConcepts.Loops.ForLoop(identifier, start, end, statements.ToArray());
                        }
                        throw new Exception("For statement is not correct.");
                    }

                case (int)RuleConstants.RULE_FORSTATEMENT_FOR_IDENTIFIER_IN_LPAREN_COMMA_COMMA_RPAREN_LBRACE_RBRACE :
                    //<ForStatement> ::= for Identifier in '(' <Number> ',' <Number> ',' <Number> ')' '{' <Statements> '}'
                    //todo: Create a new object using the stored tokens.
                    {
                        CreateNewStackLevel();
                        var name = CreateObject(token.Tokens[1]) as string;
                        variablesTypes.Peek()[name] = typeof(LanguageConcepts.BaseTypes.Double);
                        var identifier = new VariableIdentifier<LanguageConcepts.BaseTypes.Double>(name);
                        var start = CreateObject(token.Tokens[4]) as IValue;
                        var end = CreateObject(token.Tokens[6]) as IValue;
                        var step = CreateObject(token.Tokens[8]) as IValue;
                        var statements = CreateObject(token.Tokens[11]) as List<IExecutable>;
                        ReleaseStackLevel();
                        if (name is not null && start is not null && end is not null && step is not null && statements is not null)
                        {
                            return new LanguageConcepts.Loops.ForLoop(identifier, start, end, step, statements.ToArray());
                        }
                        throw new Exception("For statement is not correct.");
                    }

                case (int)RuleConstants.RULE_WHILESTATEMENT_WHILE_LPAREN_RPAREN_LBRACE_RBRACE :
                    //<WhileStatement> ::= while '(' <Expression> ')' '{' <Statements> '}'
                    //todo: Create a new object using the stored tokens.
                    {
                        CreateNewStackLevel();
                        if (CreateObject(token.Tokens[2]) is IValue<LanguageConcepts.BaseTypes.Boolean> condition && CreateObject(token.Tokens[5]) is List<IExecutable> statements)
                        {
                            ReleaseStackLevel();
                            return new LanguageConcepts.Loops.WhileLoop(condition, statements.ToArray());
                        }
                        throw new Exception("Forbidden token in while statement.");
                    }

                case (int)RuleConstants.RULE_RETVALUE_INT :
                    //<RetValue> ::= int
                    //todo: Create a new object using the stored tokens.
                    return new LanguageConcepts.BaseTypes.Integer(0);

                case (int)RuleConstants.RULE_RETVALUE_DOUBLE :
                //<RetValue> ::= double
                //todo: Create a new object using the stored tokens.
                    return new LanguageConcepts.BaseTypes.Double(0);

                case (int)RuleConstants.RULE_RETVALUE_STRING :
                    //<RetValue> ::= string
                    //todo: Create a new object using the stored tokens.
                    return new LanguageConcepts.BaseTypes.String(string.Empty);

                case (int)RuleConstants.RULE_RETVALUE_BOOLEAN :
                    //<RetValue> ::= boolean
                    //todo: Create a new object using the stored tokens.
                    return new LanguageConcepts.BaseTypes.Boolean(false);

                case (int)RuleConstants.RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_LBRACE_RBRACE :
                    //<FuncStatment> ::= function Identifier '(' <Params> ')' '{' <Statements> '}'
                    //todo: Create a new object using the stored tokens.
                    {
                        var identifier = CreateObject(token.Tokens[1]) as string;
                        functionsTypes.Peek()[identifier] = typeof(LanguageConcepts.BaseTypes.Void);

                        CreateNewStackLevel();
                        var declarations = CreateObject(token.Tokens[3]) as List<Declaration>;
                        var statements = CreateObject(token.Tokens[6]) as List<IExecutable>;
                        ReleaseStackLevel();
                        if (identifier is not null && declarations is not null && statements is not null)
                        {
                            return new FunctionDeclaration<LanguageConcepts.BaseTypes.Void>(identifier, declarations.ToArray(), statements.ToArray(), new LanguageConcepts.BaseTypes.Void()) ;
                        }

                        throw new Exception("Function declaration is not correct.");
                    }

                case (int)RuleConstants.RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_LBRACE_RBRACE2 :
                    //<FuncStatment> ::= function Identifier '(' ')' '{' <Statements> '}'
                    //todo: Create a new object using the stored tokens.
                    {
                        var identifier = CreateObject(token.Tokens[1]) as string;
                        functionsTypes.Peek()[identifier] = typeof(LanguageConcepts.BaseTypes.Void);

                        CreateNewStackLevel();
                        var statements = CreateObject(token.Tokens[5]) as List<IExecutable>;
                        ReleaseStackLevel();
                        if (identifier is not null && statements is not null)
                        {
                            return new FunctionDeclaration<LanguageConcepts.BaseTypes.Void>(identifier, Array.Empty<Declaration>(), statements.ToArray(), new LanguageConcepts.BaseTypes.Void());
                        }

                        throw new Exception("Function declaration is not correct.");
                    }

                case (int)RuleConstants.RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_COLON_LBRACE_RBRACE :
                //<FuncStatment> ::= function Identifier '(' <Params> ')' ':' <RetValue> '{' <Statements> '}'
                //todo: Create a new object using the stored tokens.
                {
                        var identifier = CreateObject(token.Tokens[1]) as string;
                        var returnType = CreateObject(token.Tokens[6]).GetType();

                        functionsTypes.Peek()[identifier] = returnType;

                        CreateNewStackLevel();
                        var declarations = CreateObject(token.Tokens[3]) as List<Declaration>;
                        var statements = CreateObject(token.Tokens[8]) as List<IExecutable>;
                        ReleaseStackLevel();
                        if (identifier is not null && returnType is not null && declarations is not null && statements is not null)
                        {
                            switch (returnType)
                            {
                                case System.Type when returnType == typeof(LanguageConcepts.BaseTypes.Boolean):
                                    return new FunctionDeclaration<LanguageConcepts.BaseTypes.Boolean>(identifier, declarations.ToArray(), statements.ToArray(), new LanguageConcepts.BaseTypes.Boolean(false));
                                case System.Type when returnType == typeof(LanguageConcepts.BaseTypes.String):
                                    return new FunctionDeclaration<LanguageConcepts.BaseTypes.String>(identifier, declarations.ToArray(), statements.ToArray(), new LanguageConcepts.BaseTypes.String(string.Empty));
                                case System.Type when returnType == typeof(LanguageConcepts.BaseTypes.Integer):
                                    return new FunctionDeclaration<LanguageConcepts.BaseTypes.Integer>(identifier, declarations.ToArray(), statements.ToArray(), new LanguageConcepts.BaseTypes.Integer(0));
                                case System.Type when returnType == typeof(LanguageConcepts.BaseTypes.Double):
                                    return new FunctionDeclaration<LanguageConcepts.BaseTypes.Double>(identifier, declarations.ToArray(), statements.ToArray(), new LanguageConcepts.BaseTypes.Double(0));
                                default:
                                    throw new Exception("Return value does not match expected type.");
                            }
                        }

                        throw new Exception("Function declaration is not correct.");
                    }

                case (int)RuleConstants.RULE_FUNCSTATMENT_FUNCTION_IDENTIFIER_LPAREN_RPAREN_COLON_LBRACE_RBRACE2 :
                    //<FuncStatment> ::= function Identifier '(' ')' ':' <RetValue> '{' <Statements> '}'
                    //todo: Create a new object using the stored tokens.
                    {
                        var identifier = CreateObject(token.Tokens[1]) as string;
                        var returnType = CreateObject(token.Tokens[5]).GetType();

                        functionsTypes.Peek()[identifier] = returnType;

                        CreateNewStackLevel();
                        var statements = CreateObject(token.Tokens[7]) as List<IExecutable>;
                        ReleaseStackLevel();
                        if (identifier is not null && returnType is not null && statements is not null)
                        {
                            switch (returnType)
                            {
                                case System.Type when returnType == typeof(LanguageConcepts.BaseTypes.Boolean):
                                    return new FunctionDeclaration<LanguageConcepts.BaseTypes.Boolean>(identifier, Array.Empty<Declaration>(), statements.ToArray(), new LanguageConcepts.BaseTypes.Boolean(false));
                                case System.Type when returnType == typeof(LanguageConcepts.BaseTypes.String):
                                    return new FunctionDeclaration<LanguageConcepts.BaseTypes.String>(identifier, Array.Empty<Declaration>(), statements.ToArray(), new LanguageConcepts.BaseTypes.String(string.Empty));
                                case System.Type when returnType == typeof(LanguageConcepts.BaseTypes.Integer):
                                    return new FunctionDeclaration<LanguageConcepts.BaseTypes.Integer>(identifier, Array.Empty<Declaration>(), statements.ToArray(), new LanguageConcepts.BaseTypes.Integer(0));
                                case System.Type when returnType == typeof(LanguageConcepts.BaseTypes.Double):
                                    return new FunctionDeclaration<LanguageConcepts.BaseTypes.Double>(identifier, Array.Empty<Declaration>(), statements.ToArray(), new LanguageConcepts.BaseTypes.Double(0));
                                default:
                                    throw new Exception("Return value does not match expected type.");
                            }
                        }

                        throw new Exception("Function declaration is not correct.");
                    }

                case (int)RuleConstants.RULE_PARAMS :
                    //<Params> ::= <Declaration>
                    //todo: Create a new object using the stored tokens.
                    { 
                        if (CreateObject(token.Tokens[0]) is Declaration declaration)
                        {
                            return new List<Declaration>{
                                declaration
                            };
                        }
                        throw new Exception("Param in function is not correct.");
                    }

                case (int)RuleConstants.RULE_PARAMS_COMMA :
                    //<Params> ::= <Declaration> ',' <Params>
                    //todo: Create a new object using the stored tokens.
                    {
                        var declarations = new List<Declaration>();
                        var firstDeclaration = CreateObject(token.Tokens[0]) as Declaration;
                        var nextDeclarations = CreateObject(token.Tokens[2]) as List<Declaration>;

                        if (firstDeclaration is not null && nextDeclarations is not null)
                        {
                            declarations.Add(firstDeclaration);
                            declarations.AddRange(nextDeclarations);

                            return declarations;
                        }
                        throw new Exception("Param in function is not correct.");
                    }

                case (int)RuleConstants.RULE_DECLARATION_INT_IDENTIFIER :
                    //<Declaration> ::= int Identifier
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = token.Tokens[1].ToString();
                        variablesTypes.Peek()[name] = typeof(LanguageConcepts.BaseTypes.Integer);
                        return new VariableDeclaration<LanguageConcepts.BaseTypes.Integer>(new LanguageConcepts.BaseTypes.Integer(0), name);
                    }

                case (int)RuleConstants.RULE_DECLARATION_DOUBLE_IDENTIFIER :
                    //<Declaration> ::= double Identifier
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = token.Tokens[1].ToString();
                        variablesTypes.Peek()[name] = typeof(LanguageConcepts.BaseTypes.Double);
                        return new VariableDeclaration<LanguageConcepts.BaseTypes.Double>(new LanguageConcepts.BaseTypes.Double(0), name);
                    }

                case (int)RuleConstants.RULE_DECLARATION_STRING_IDENTIFIER :
                    //<Declaration> ::= string Identifier
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = token.Tokens[1].ToString();
                        variablesTypes.Peek()[name] = typeof(LanguageConcepts.BaseTypes.String);
                        return new VariableDeclaration<LanguageConcepts.BaseTypes.String>(new LanguageConcepts.BaseTypes.String(string.Empty), name);
                    }

                case (int)RuleConstants.RULE_DECLARATION_BOOLEAN_IDENTIFIER :
                    //<Declaration> ::= boolean Identifier
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = token.Tokens[1].ToString();
                        variablesTypes.Peek()[name] = typeof(LanguageConcepts.BaseTypes.Boolean);
                        return new VariableDeclaration<LanguageConcepts.BaseTypes.Boolean>(new LanguageConcepts.BaseTypes.Boolean(false), name);
                    }

                case (int)RuleConstants.RULE_DECLARATION_STRING_IDENTIFIER_EQ :
                    //<Declaration> ::= string Identifier '=' <Expression>
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = CreateObject(token.Tokens[1]) as string;
                        var value = CreateObject(token.Tokens[3]) as IValue<LanguageConcepts.BaseTypes.String>;
                        if (name is not null && value is not null)
                        {
                            variablesTypes.Peek()[name] = typeof(LanguageConcepts.BaseTypes.String);
                            return new LanguageConcepts.VariableDeclaration<LanguageConcepts.BaseTypes.String>(new LanguageConcepts.BaseTypes.String(string.Empty), name, value);
                        }
                        throw new Exception("Forbidden token in string declaration.");
                    }

                case (int)RuleConstants.RULE_DECLARATION_DOUBLE_IDENTIFIER_EQ :
                    //<Declaration> ::= double Identifier '=' <Expression>
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = CreateObject(token.Tokens[1]) as string;
                        var value = CreateObject(token.Tokens[3]) as IValue<LanguageConcepts.BaseTypes.Double>;
                        if (name is not null && value is not null)
                        {
                            variablesTypes.Peek()[name] = typeof(LanguageConcepts.BaseTypes.Double);
                            return new LanguageConcepts.VariableDeclaration<LanguageConcepts.BaseTypes.Double>(new LanguageConcepts.BaseTypes.Double(0), name, value);
                        }
                        throw new Exception("Forbidden token in double declaration.");
                    }

                case (int)RuleConstants.RULE_DECLARATION_INT_IDENTIFIER_EQ :
                    //<Declaration> ::= int Identifier '=' <Expression>
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = CreateObject(token.Tokens[1]) as string;
                        var value = CreateObject(token.Tokens[3]) as IValue<LanguageConcepts.BaseTypes.Integer>;
                        if (name is not null && value is not null)
                        {
                            variablesTypes.Peek()[name] = typeof(LanguageConcepts.BaseTypes.Integer);
                            return new LanguageConcepts.VariableDeclaration<LanguageConcepts.BaseTypes.Integer>(new LanguageConcepts.BaseTypes.Integer(0), name, value);
                        }
                        throw new Exception("Forbidden token in int declaration.");
                    }

                case (int)RuleConstants.RULE_DECLARATION_BOOLEAN_IDENTIFIER_EQ:
                    //<Declaration> ::= boolean Identifier '=' <Expression>
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = CreateObject(token.Tokens[1]) as string;
                        var value = CreateObject(token.Tokens[3]) as IValue<LanguageConcepts.BaseTypes.Boolean>;
                        if (name is not null && value is not null)
                        {
                            variablesTypes.Peek()[name] = typeof(LanguageConcepts.BaseTypes.Boolean);
                            return new LanguageConcepts.VariableDeclaration<LanguageConcepts.BaseTypes.Boolean>(new LanguageConcepts.BaseTypes.Boolean(false), name, value);
                        }
                        throw new Exception("Forbidden token in boolean declaration.");
                    }

                case (int)RuleConstants.RULE_EXPRESSION_GT :
                    //<Expression> ::= <Expression> '>' <LogicExp>
                    //todo: Create a new object using the stored tokens.
                    {
                        if (CreateObject(token.Tokens[0]) is IValue left && CreateObject(token.Tokens[2]) is IValue right)
                        {
                            return new NumberGreaterExpression(left, right);
                        }
                        throw new Exception("Forbidden token in comparison.");

                    }

                case (int)RuleConstants.RULE_EXPRESSION_LT :
                    //<Expression> ::= <Expression> '<' <LogicExp>
                    //todo: Create a new object using the stored tokens.
                    {
                        if (CreateObject(token.Tokens[0]) is IValue left && CreateObject(token.Tokens[2]) is IValue right)
                        {
                            return new NumberLessExpression(left, right);
                        }
                        throw new Exception("Forbidden token in comparison.");
                        
                    }

                case (int)RuleConstants.RULE_EXPRESSION_GTEQ :
                    //<Expression> ::= <Expression> '>=' <LogicExp>
                    //todo: Create a new object using the stored tokens.
                    {
                        if (CreateObject(token.Tokens[0]) is IValue left && CreateObject(token.Tokens[2]) is IValue right)
                        {
                            return new NumberGreaterOrEqualExpression(left, right);
                        }
                        throw new Exception("Forbidden token in comparison.");

                    }

                case (int)RuleConstants.RULE_EXPRESSION_LTEQ :
                    //<Expression> ::= <Expression> '<=' <LogicExp>
                    //todo: Create a new object using the stored tokens.
                    {
                        if (CreateObject(token.Tokens[0]) is IValue left && CreateObject(token.Tokens[2]) is IValue right)
                        {
                            return new NumberLessOrEqualExpression(left, right);
                        }
                        throw new Exception("Forbidden token in comparison.");

                    }

                case (int)RuleConstants.RULE_EXPRESSION_EQEQ :
                    //<Expression> ::= <Expression> '==' <LogicExp>
                    //todo: Create a new object using the stored tokens.
                    {
                        if (CreateObject(token.Tokens[0]) is IValue left && CreateObject(token.Tokens[2]) is IValue right)
                        {
                            return new EqualExpression(left, right);
                        }
                        throw new Exception("Forbidden token in comparison.");

                    }

                case (int)RuleConstants.RULE_EXPRESSION_EXCLAMEQ :
                    //<Expression> ::= <Expression> '!=' <LogicExp>
                    //todo: Create a new object using the stored tokens.
                    {
                        if (CreateObject(token.Tokens[0]) is IValue left && CreateObject(token.Tokens[2]) is IValue right)
                        {
                            return new NotEqualExpression(left, right);
                        }
                        throw new Exception("Forbidden token in comparison.");

                    }

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
                        }
                        throw new Exception("Forbidden token in assignment.");
                    }

                case (int)RuleConstants.RULE_EXPRESSION :
                    //<Expression> ::= <LogicExp>
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_LOGICEXP_AMPAMP :
                    //<LogicExp> ::= <LogicExp> '&&' <AddExp>
                    //todo: Create a new object using the stored tokens.
                    {
                        var left = CreateObject(token.Tokens[0]) as IValue;
                        var right = CreateObject(token.Tokens[2]) as IValue;
                        if (left is not null && right is not null)
                        {
                            switch (left)
                            {
                                case IValue<LanguageConcepts.BaseTypes.Boolean> val1 when right is IValue<LanguageConcepts.BaseTypes.Boolean> val2:
                                    return new LogicalAndExpression(val1, val2);
                            }
                        }
                        throw new Exception("Forbidden token in && expression.");
                    }

                case (int)RuleConstants.RULE_LOGICEXP_PIPEPIPE :
                    //<LogicExp> ::= <LogicExp> '||' <AddExp>
                    //todo: Create a new object using the stored tokens.
                    {
                        var left = CreateObject(token.Tokens[0]) as IValue;
                        var right = CreateObject(token.Tokens[2]) as IValue;
                        if (left is not null && right is not null)
                        {
                            switch (left)
                            {
                                case IValue<LanguageConcepts.BaseTypes.Boolean> val1 when right is IValue<LanguageConcepts.BaseTypes.Boolean> val2:
                                    return new LogicalOrExpression(val1, val2);
                            }
                        }
                        throw new Exception("Forbidden token in || expression.");
                    }

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
                                case IValue<LanguageConcepts.BaseTypes.Double> val1 when right is IValue <LanguageConcepts.BaseTypes.Double> val2:
                                    return new AdditionExpression<LanguageConcepts.BaseTypes.Double>(val1, val2);
                                case IValue<LanguageConcepts.BaseTypes.String> val1 when right is IValue<LanguageConcepts.BaseTypes.String> val2:
                                    return new AdditionExpression<LanguageConcepts.BaseTypes.String>(val1, val2);
                            }
                        }
                        throw new Exception("Forbidden token in add expression.");
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
                        throw new Exception("Forbidden token in subtract expression.");
                    }

                case (int)RuleConstants.RULE_ADDEXP :
                    //<AddExp> ::= <MulExp>
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_MULEXP_TIMES :
                    //<MulExp> ::= <MulExp> '*' <Value>
                    //todo: Create a new object using the stored tokens.
                    {
                        var left = CreateObject(token.Tokens[0]) as IValue;
                        var right = CreateObject(token.Tokens[2]) as IValue;
                        if (left is not null && right is not null)
                        {
                            switch (left)
                            {
                                case IValue<LanguageConcepts.BaseTypes.Integer> val1 when right is IValue<LanguageConcepts.BaseTypes.Integer> val2:
                                    return new MultiplicationExpression<LanguageConcepts.BaseTypes.Integer>(val1, val2);
                                case IValue<LanguageConcepts.BaseTypes.Double> val1 when right is IValue<LanguageConcepts.BaseTypes.Double> val2:
                                    return new MultiplicationExpression<LanguageConcepts.BaseTypes.Double>(val1, val2);
                            }
                        }
                        throw new Exception("Forbidden token in multiply expression.");
                    }

                case (int)RuleConstants.RULE_MULEXP_DIV :
                    //<MulExp> ::= <MulExp> '/' <Value>
                    //todo: Create a new object using the stored tokens.
                    {
                        var left = CreateObject(token.Tokens[0]) as IValue;
                        var right = CreateObject(token.Tokens[2]) as IValue;
                        if (left is not null && right is not null)
                        {
                            switch (left)
                            {
                                case IValue<LanguageConcepts.BaseTypes.Integer> val1 when right is IValue<LanguageConcepts.BaseTypes.Integer> val2:
                                    return new DivisionExpression<LanguageConcepts.BaseTypes.Integer>(val1, val2);
                                case IValue<LanguageConcepts.BaseTypes.Double> val1 when right is IValue<LanguageConcepts.BaseTypes.Double> val2:
                                    return new DivisionExpression<LanguageConcepts.BaseTypes.Double>(val1, val2);
                            }
                        }
                        throw new Exception("Forbidden token in divide expression.");
                    }

                case (int)RuleConstants.RULE_MULEXP :
                    //<MulExp> ::= <Value>
                    //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_MULEXP_EXCLAM:
                    //<MulExp> ::= '!' <Value>
                    //todo: Create a new object using the stored tokens.
                    {
                        var value = CreateObject(token.Tokens[1]) as IValue<LanguageConcepts.BaseTypes.Boolean>;
                        if (value is not null)
                        {
                            return new LogicalNotExpression(value);
                        }
                        throw new Exception("Forbidden token in negation.");
                    }

                case (int)RuleConstants.RULE_MULEXP2 :
                //<MulExp> ::= <callFunc>
                //todo: Create a new object using the stored tokens.
                    return CreateObject(token.Tokens[0]);

                case (int)RuleConstants.RULE_TOCALLPARAM_IDENTIFIER :
                    //<toCallParam> ::= Identifier
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = CreateObject(token.Tokens[0]) as string;
                        if (name is not null)
                        {
                            IValue value = GetVariableType(name) switch
                            {
                                System.Type type when type == typeof(LanguageConcepts.BaseTypes.Boolean) => new VariableIdentifier<LanguageConcepts.BaseTypes.Boolean>(name),
                                System.Type type when type == typeof(LanguageConcepts.BaseTypes.Double) => new VariableIdentifier<LanguageConcepts.BaseTypes.Double>(name),
                                System.Type type when type == typeof(LanguageConcepts.BaseTypes.Integer) => new VariableIdentifier<LanguageConcepts.BaseTypes.Integer>(name),
                                System.Type type when type == typeof(LanguageConcepts.BaseTypes.String) => new VariableIdentifier<LanguageConcepts.BaseTypes.String>(name),
                                _ => throw new Exception("Value is not correct in call function.")
                            };

                            return new List<IValue> { 
                                value
                            };
                        }
                        throw new Exception("Value is not correct in call function.");
                    }

                case (int)RuleConstants.RULE_TOCALLPARAM_IDENTIFIER_COMMA :
                    //<toCallParam> ::= Identifier ',' <toCallParam>
                    //todo: Create a new object using the stored tokens.
                    {
                        if (CreateObject(token.Tokens[0]) is string firstParameterName && CreateObject(token.Tokens[2]) is List<IValue> restOfParams)
                        {
                            var listOfParams = new List<IValue>();
                            IValue firstValue = GetVariableType(firstParameterName) switch
                            {
                                System.Type type when type == typeof(LanguageConcepts.BaseTypes.Boolean) => new VariableIdentifier<LanguageConcepts.BaseTypes.Boolean>(firstParameterName),
                                System.Type type when type == typeof(LanguageConcepts.BaseTypes.Double) => new VariableIdentifier<LanguageConcepts.BaseTypes.Double>(firstParameterName),
                                System.Type type when type == typeof(LanguageConcepts.BaseTypes.Integer) => new VariableIdentifier<LanguageConcepts.BaseTypes.Integer>(firstParameterName),
                                System.Type type when type == typeof(LanguageConcepts.BaseTypes.String) => new VariableIdentifier<LanguageConcepts.BaseTypes.String>(firstParameterName),
                                _ => throw new Exception("Value is not correct in call function.")
                            };
                            listOfParams.Add(firstValue);
                            listOfParams.AddRange(restOfParams);
                            return listOfParams;
                        }
                        throw new Exception("Value is not correct in call function.");
                    }

                case (int)RuleConstants.RULE_CALLFUNC_IDENTIFIER_LPAREN_RPAREN :
                    //<callFunc> ::= Identifier '(' <toCallParam> ')'
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = CreateObject(token.Tokens[0]) as string;
                        var inputs = CreateObject(token.Tokens[2]) as List<IValue>;
                        if (name is not null && inputs is not null)
                        {
                            var functionType = GetFuntionType(name);
                            switch (functionType)
                            {
                                case System.Type when functionType == typeof(LanguageConcepts.BaseTypes.Void):
                                    return new FunctionCall<LanguageConcepts.BaseTypes.Void>(name, inputs.ToArray());
                                case System.Type when functionType == typeof(LanguageConcepts.BaseTypes.Boolean):
                                    return new FunctionCall<LanguageConcepts.BaseTypes.Boolean>(name, inputs.ToArray());
                                case System.Type when functionType == typeof(LanguageConcepts.BaseTypes.Double):
                                    return new FunctionCall<LanguageConcepts.BaseTypes.Double>(name, inputs.ToArray());
                                case System.Type when functionType == typeof(LanguageConcepts.BaseTypes.Integer):
                                    return new FunctionCall<LanguageConcepts.BaseTypes.Integer>(name, inputs.ToArray());
                                case System.Type when functionType == typeof(LanguageConcepts.BaseTypes.String):
                                    return new FunctionCall<LanguageConcepts.BaseTypes.String>(name, inputs.ToArray());
                            }

                        }
                        throw new Exception("Value is not correct in call function.");
                    }

                case (int)RuleConstants.RULE_CALLFUNC_IDENTIFIER_LPAREN_RPAREN2 :
                    //<callFunc> ::= Identifier '(' ')'
                    //todo: Create a new object using the stored tokens.
                    {
                        var name = CreateObject(token.Tokens[0]) as string;
                        if (name is not null)
                        {
                            var functionType = GetFuntionType(name);
                            switch (functionType)
                            {
                                case System.Type when functionType == typeof(LanguageConcepts.BaseTypes.Void):
                                    return new FunctionCall<LanguageConcepts.BaseTypes.Void>(name, Array.Empty<IValue>());
                                case System.Type when functionType == typeof(LanguageConcepts.BaseTypes.Boolean):
                                    return new FunctionCall<LanguageConcepts.BaseTypes.Boolean>(name, Array.Empty<IValue>());
                                case System.Type when functionType == typeof(LanguageConcepts.BaseTypes.Double):
                                    return new FunctionCall<LanguageConcepts.BaseTypes.Double>(name, Array.Empty<IValue>());
                                case System.Type when functionType == typeof(LanguageConcepts.BaseTypes.Integer):
                                    return new FunctionCall<LanguageConcepts.BaseTypes.Integer>(name, Array.Empty<IValue>());
                                case System.Type when functionType == typeof(LanguageConcepts.BaseTypes.String):
                                    return new FunctionCall<LanguageConcepts.BaseTypes.String>(name, Array.Empty<IValue>());
                            }
                            
                        }
                        throw new Exception("Calling function is not correct.");
                    }
            }
            throw new RuleException("Unknown rule");
        }

        private void TokenErrorEvent(LALRParser parser, TokenErrorEventArgs args)
        {
            string message = "Token error with input: '"+args.Token.ToString()+"'";
            Console.WriteLine(message);
            Console.WriteLine($"Line: {args.Token.Location.LineNr}\nColumn: {args.Token.Location.ColumnNr}");
        }

        private void ParseErrorEvent(LALRParser parser, ParseErrorEventArgs args)
        {
            string message = "Parse error caused by token: '"+args.UnexpectedToken.ToString()+"'";
            Console.WriteLine(message);
            Console.WriteLine($"Expected: {args.ExpectedTokens}\nGot: {args.UnexpectedToken}");
            Console.WriteLine($"Line: {args.UnexpectedToken?.Location.LineNr}\nColumn: {args.UnexpectedToken?.Location.ColumnNr}");
        }

    }
}
