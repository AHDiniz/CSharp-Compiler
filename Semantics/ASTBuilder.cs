using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public partial class ASTBuilder : CSharpParserBaseListener
    {
        private Stack<ParserRuleContext> contextStack;
        private AST ast;

        public AST AbstractSyntaxTree
        {
            get => ast;
        }

        public ASTBuilder() : base()
        {
            contextStack = new Stack<ParserRuleContext>();
            ast = new AST();
        }
    }
}