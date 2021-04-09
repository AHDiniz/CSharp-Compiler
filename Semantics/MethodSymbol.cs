using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public class MethodSymbol : Symbol
    {
        private ClassSymbol ownerClass;
        private Symbol returnType;

        public ClassSymbol OwnerClass { get => ownerClass; }
        public Symbol ReturnType { get => returnType; }

        public MethodSymbol(ClassSymbol ownerClass, Symbol returnType)
        {
            this.ownerClass = ownerClass;
            this.returnType = returnType;
        }
    }
}