using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public partial class ASTBuilder : CSharpParserBaseListener
    {
        public override void EnterMethod_declaration(CSharpParser.Method_declarationContext context)
        {
            Console.WriteLine("Entering method_declaration context.");
        }

        public override void ExitMethod_declaration(CSharpParser.Method_declarationContext context)
        {
            Console.WriteLine("Exiting method_declaration context.");
        }
    }
}