using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace CSharp_Compiler.Semantics
{
    public enum ClassTag
    {
        Class, Interface, Struct
    }

    public class ClassType : Type
    {
        private ClassTag classTag;
        private ClassSymbol classData;

        public ClassType(IToken typeToken, ClassTag classTag, ClassSymbol classData) : base(typeToken)
        {
            this.classTag = classTag;
            this.classData = classData;
        }
    }
}
