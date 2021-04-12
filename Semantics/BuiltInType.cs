using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace CSharp_Compiler.Semantics
{
    public enum TypeTag
    {
        Bool, Byte, SByte, Char, Decimal, Double, Float, Int, UInt, NInt, NUInt, Long,
        ULong, Short, UShort, Object, String, Dynamic, Void, Constructor, Destructor
    }

    public class BuiltInType : Type
    {
        private TypeTag typeTag;
        private bool pointer;

        public TypeTag Tag { get => typeTag; }
        public bool IsPointer { get => pointer; }

        public BuiltInType(IToken typeToken, TypeTag typeTag, bool pointer = false) : base(typeToken)
        {
            this.typeTag = typeTag;
            this.pointer = pointer;
        }
    }
}
