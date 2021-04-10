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
        private Symbol type;
        private object value;

        public ClassSymbol OwnerClass { get => ownerClass; }
        public Symbol Type { get => type; }
        public object Value { get => value; }

        public AttributeSymbol(ClassSymbol ownerClass, Symbol type, object value = null)
        {
            this.ownerClass = ownerClass;
            this.type = type;
            this.value = value;
        }
    }
}