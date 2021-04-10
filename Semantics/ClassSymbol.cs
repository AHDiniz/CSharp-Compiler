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

        public List<ClassSymbol> BaseClasses { get => baseClasses; }

        public ClassSymbol(ModifierFlag mods, ClassSymbol[] baseClasses)
        {
            this.modifiers = mods;
            this.baseClasses = new List<ClassSymbol>(baseClasses);
        }
    }
}
