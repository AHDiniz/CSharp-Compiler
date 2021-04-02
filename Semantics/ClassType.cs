using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace CSharp_Compiler.Semantics
{
    public enum ClassTag
    {
        Class, Interface, Struct
    }

    public enum ScopeTag
    {
        Public, Private, Protected, Internal, ProtectedInternal, PrivateProtected
    }

    public enum ModifierTag
    {
        Abstract, Const, Extern, Static, Partial, Readonly, Sealed, Unsafe, Virtual, Volatile
    }

    public class ClassType : Type
    {
        private ScopeTag scopeTag;
        private ModifierTag modifierTag;
        private ClassTag classTag;

        public ClassType(IToken typeToken, ScopeTag scopeTag, ModifierTag modifierTag, ClassTag classTag) : base(typeToken)
        {
            this.scopeTag = scopeTag;
            this.modifierTag = modifierTag;
            this.classTag = classTag;
        }
    }
}
