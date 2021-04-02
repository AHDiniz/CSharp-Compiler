using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace CSharp_Compiler.Semantics
{
    public enum TypeTag
    {
        Bool, Byte, SByte, Char, Decimal, Double, Float, Int, UInt, NInt,
        NUInt, Long, ULong, Short, UShort, Object, String, Dynamic
    }

    public class BuiltInType : Type
    {
        private TypeTag typeTag;

        public BuiltInType(IToken typeToken, TypeTag typeTag) : base(typeToken)
        {
            this.typeTag = typeTag;
        }
    }
}
