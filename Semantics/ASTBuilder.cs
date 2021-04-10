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
        private List<IToken> modifiersTokens; // For the modifiers of the current class/method/struct/interface

        public AST AbstractSyntaxTree
        {
            get => ast;
        }

        public ASTBuilder(AST ast, SymbolTable symbolTable) : base()
        {
            contextStack = new Stack<ParserRuleContext>();
            modifiersTokens = new List<IToken>();
            this.ast = ast;
            this.symbolTable = symbolTable;
        }

        private Symbol.ModifierFlag TreatModTokens()
        {
            Symbol.ModifierFlag flags = Symbol.ModifierFlag.None;

            foreach (IToken modToken in modifiersTokens)
            {
                switch (modToken.Text)
                {
                    case "new":
                        flags |= Symbol.ModifierFlag.New;
                        break;
                    case "public":
                        flags |= Symbol.ModifierFlag.Public;
                        break;
                    case "protected":
                        flags |= Symbol.ModifierFlag.Protected;
                        break;
                    case "readonly":
                        flags |= Symbol.ModifierFlag.ReadOnly;
                        break;
                    case "volatile":
                        flags |= Symbol.ModifierFlag.Volatile;
                        break;
                    case "virtual":
                        flags |= Symbol.ModifierFlag.Virtual;
                        break;
                    case "sealed":
                        flags |= Symbol.ModifierFlag.Sealed;
                        break;
                    case "override":
                        flags |= Symbol.ModifierFlag.Override;
                        break;
                    case "abstract":
                        flags |= Symbol.ModifierFlag.Abstract;
                        break;
                    case "static":
                        flags |= Symbol.ModifierFlag.Static;
                        break;
                    case "unsafe":
                        flags |= Symbol.ModifierFlag.Unsafe;
                        break;
                    case "extern":
                        flags |= Symbol.ModifierFlag.Extern;
                        break;
                    case "partial":
                        flags |= Symbol.ModifierFlag.Partial;
                        break;
                    case "async":
                        flags |= Symbol.ModifierFlag.Async;
                        break;
                }
            }

            return flags;
        }
    }
}