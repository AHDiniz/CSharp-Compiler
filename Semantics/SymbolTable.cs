using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public abstract class Symbol
    {
        [System.Flags]
        public enum ModifierFlag
        {
            None, New, Public, Protected, ReadOnly, Volatile, Virtual, Sealed,
            Override, Abstract, Static, Unsafe, Extern, Partial, Async
        }

        protected ModifierFlag modifiers;

        public ModifierFlag Modifiers { get => modifiers; }
    }

    public class SymbolTable
    {
        private class Scope
        {
            Dictionary<IToken, Symbol> symbols;
            int scopeStartNodeIndex;

            public Scope(int scopeStartNodeIndex)
            {
                this.scopeStartNodeIndex = scopeStartNodeIndex
                symbols = new Dictionary<IToken, Symbol>();
            }

            public void AddSymbol(IToken key, Symbol value)
            {
                symbols.Add(key, value);
            }

            public void RemoveSymbol(IToken key)
            {
                symbols.Remove(key);
            }
        }

        private Stack<Scope> scopes;

        public SymbolTable()
        {
            scopes = new Stack<Scope>();
        }

        public void EnterScope(int nodeIndex)
        {
            scopes.Push(new Scope(nodeIndex));
        }

        public void ExitScope()
        {
            scopes.Pop();
        }

        public void AddScope(IToken key, Symbol value)
        {
            scopes.AddSymbol(key, value);
        }

        public void RemoveSymbol(IToken key)
        {
            scopes.RemoveSymbol(IToken key);
        }
    }
}
