using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler
{
    class KeyPrinter : CSharpParserBaseListener
    {
        // override default listener behavior
        void ExitKey(CSharpParser.KeywordContext context )
        {
            Console.WriteLine("Oh, a key!");
        }

        override
        public void EnterClass_type(CSharpParser.Class_typeContext context)
        {
            Console.WriteLine("A class");
        }

        override
        public void EnterClass_body(CSharpParser.Class_bodyContext context)
        {
            Console.WriteLine("A class");
        }

        override
        public void EnterClass_base(CSharpParser.Class_baseContext context)
        {
            Console.WriteLine("A class");
        }

        override
        public void EnterClass_definition(CSharpParser.Class_definitionContext context)
        {
            Console.WriteLine("A class");
        }

        public override void EnterVariable_declarator([NotNull] CSharpParser.Variable_declaratorContext context)
        {
            Console.WriteLine("A Var");
        }

        public override void EnterLocal_variable_declarator([NotNull] CSharpParser.Local_variable_declaratorContext context)
        {
            base.EnterLocal_variable_declarator(context);
            Console.WriteLine("A Var");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            ICharStream stream = CharStreams.fromString("class Teste{ void Main(){int varaivel; Console.writer(\"testes\");}}");

            //AntlrInputStream inputStream = new AntlrInputStream(stream);

            CSharpLexer lexer = new CSharpLexer(stream);

            ITokenStream tokenStream = new CommonTokenStream(lexer);
                
            CSharpParser parser = new CSharpParser(tokenStream);

            parser.BuildParseTree = true;



            IParseTree tree = parser.compilation_unit();

            ParseTreeWalker.Default.Walk(new KeyPrinter(), tree);

        }
    }
}
