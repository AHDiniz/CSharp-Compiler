using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public class TupleType : Type
    {
        private List<Type> subTypes;

        public List<Type> SubTypes { get => subTypes; }

        public TupleType(IToken token, Type[] subTypes) : base(token)
        {
            this.subTypes = new List<Type>(subTypes);
        }
    }
}