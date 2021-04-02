using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using CSharp_Compiler.Semantics;

namespace CSharp_Compiler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please name the C# file that you would like to compile as a program argument.");
                return;
            }

            StreamReader sr = new StreamReader(args[0]);

            ICharStream stream = CharStreams.fromString(sr.ReadToEnd());

            CSharpLexer lexer = new CSharpLexer(stream);

            ITokenStream tokenStream = new CommonTokenStream(lexer);

            CSharpParser parser = new CSharpParser(tokenStream);

            parser.BuildParseTree = true;

            IParseTree tree = parser.compilation_unit();

            Console.WriteLine(tree.ToStringTree(parser));

            ASTBuilder astBuilder = new ASTBuilder();

            ParseTreeWalker.Default.Walk(astBuilder, tree);
        }
    }
}
