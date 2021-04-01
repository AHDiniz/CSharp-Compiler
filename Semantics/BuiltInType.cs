using System;
using System.Collections.Generics;
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

        public BuiltInType(Token typeToken, TypeTag typeTag)
        {
            base(typeToken);
            this.typeTag = typeTag;
        }
    }
}
