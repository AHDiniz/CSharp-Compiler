using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public class ClassSymbol : Symbol
    {
        private List<ClassSymbol> baseClasses;
        private ClassTag tag;

        public List<ClassSymbol> BaseClasses { get => baseClasses; }
        public ClassTag Tag { get => tag; }

        public ClassSymbol(ModifierFlag mods, ClassSymbol[] baseClasses, ClassTag tag) : base(mods)
        {
            this.baseClasses = new List<ClassSymbol>(baseClasses);
            this.tag = tag;
        }
    }
}
