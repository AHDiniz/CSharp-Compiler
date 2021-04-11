using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public class DestructorSymbol : Symbol
    {
        private ClassSymbol ownerClass;

        public ClassSymbol OwnerClass { get => ownerClass; }

        public DestructorSymbol(ModifierFlag mods, ClassSymbol ownerClass) : base(mods)
        {
            this.ownerClass = ownerClass;
        }
    }
}