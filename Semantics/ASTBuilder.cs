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
        private SymbolTable symbolTable;

        public AST AbstractSyntaxTree
        {
            get => ast;
        }

        public ASTBuilder(AST ast, SymbolTable symbolTable) : base()
        {
            contextStack = new Stack<ParserRuleContext>();
            this.ast = ast;
            this.symbolTable = symbolTable;
        }
    }
}