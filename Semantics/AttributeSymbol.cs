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
        public object Data { get => data; }

        public AttributeSymbol(ClassSymbol ownerClass, Symbol type, object data = null)
        {
            this.ownerClass = ownerClass;
            this.type = type;
            this.data = data;
        }
    }
}