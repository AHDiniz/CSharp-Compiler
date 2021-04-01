using System;
using System.Collections.Generics;
using Antlr4.Runtime;

namespace CSharp_Compiler.Semantics
{
    public class Type
    {
        private Token typeToken;

        public Type(Token typeToken)
        {
            this.typeToken = typeToken
        }
    }
}
