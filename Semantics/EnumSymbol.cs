using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public class EnumSymbol : Symbol
    {
        private List<IToken> enumTokensList = null;
        private Type baseType = null;

        public IToken[] Tokens { get => this.enumTokensList.ToArray(); }
        public Type BaseType { get => this.baseType; }

        public EnumSymbol(ModifierFlag modFlags, IToken[] tokensArray, Type baseType) : base(modFlags)
        {
            this.enumTokensList = new List<IToken>(tokensArray);
            this.baseType = baseType;
        }
    }
}
