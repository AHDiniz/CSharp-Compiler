using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public class AttributeSymbol : Symbol
    {
        private ClassSymbol ownerClass;
        private object value;

        public ClassSymbol OwnerClass { get => ownerClass; }
        public object Value { get => value; }

        public AttributeSymbol(ModifierFlag mods, ClassSymbol ownerClass, object value = null) : base(mods)
        {
            this.ownerClass = ownerClass;
            this.value = value;
        }
    }
}