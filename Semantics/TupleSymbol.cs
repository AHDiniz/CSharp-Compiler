using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public class TupleSymbol : Symbol
    {
        private List<Symbol> subMembers;

        public TupleSymbol(ModifierFlag flags, Symbol[] subMembers) : base(flags)
        {
            this.subMembers = new List<Symbol>(subMembers);
        }
    }
}