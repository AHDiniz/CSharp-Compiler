using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public class ClassSymbol : Symbol
    {
        private ClassSymbol baseClass;
        private ClassSymbol ownerClass;
        private List<ClassSymbol> typeParams;

        public ClassSymbol BaseClass { get => baseClass; }
        public ClassSymbol OwnerClass { get => ownerClass; }
        public List<ClassSymbol> TypeParams { get => typeParams; }

        public ClassSymbol(ModifierFlag mods, ClassSymbol baseClass, ClassSymbol ownerClass, ClassSymbol[] typeParams)
        {
            this.modifiers = mods;
            this.baseClass = baseClass;
            this.ownerClass = ownerClass;
            this.typeParams = new List<ClassSymbol>(typeParams);
        }
    }
}
