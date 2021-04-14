using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public class VariableSymbol : Symbol
    {
        private MethodSymbol ownerMethod;
        private object value;

        public MethodSymbol OwnerMethod { get => ownerMethod; }
        public object Value { get => value; }

        public VariableSymbol(ModifierFlag mods, MethodSymbol ownerMethod, object value = null) : base(mods)
        {
            this.ownerMethod = ownerMethod;
            this.value = value;
        }
    }
}