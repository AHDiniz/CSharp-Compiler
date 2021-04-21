using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace CSharp_Compiler.Semantics
{
    public class Type
    {
        private IToken typeToken;

        public IToken Token { get => typeToken; }

        public Type(IToken typeToken)
        {
            this.typeToken = typeToken;
        }
    }
}
