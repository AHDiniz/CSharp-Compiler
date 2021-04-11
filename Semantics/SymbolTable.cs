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

        public Symbol(ModifierFlag flags)
        {
            this.modifiers = flags;
        }
    }

    public class SymbolTable
    {
        private class Scope
        {
            private Dictionary<IToken, Symbol> symbols;
            private int scopeStartNodeIndex;

            public int CurrentScopeNode { get => scopeStartNodeIndex; }

            public Scope(int scopeStartNodeIndex)
            {
                this.scopeStartNodeIndex = scopeStartNodeIndex;
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

            public Symbol FindSymbol(IToken key)
            {
                return symbols[key];
            }
        }

        private Stack<Scope> scopes;

        public int CurrentScopeNode { get => scopes.Peek().CurrentScopeNode; }

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

        public void AddSymbol(IToken key, Symbol value)
        {
            scopes.Peek().AddSymbol(key, value);
        }

        public void RemoveSymbol(IToken key)
        {
            scopes.Peek().RemoveSymbol(key);
        }

        public Symbol FindSymbol(IToken key)
        {
            return scopes.Peek().FindSymbol(key);
        }

        public Symbol[] FindSymbols(IToken[] keys)
        {
            List<Symbol> symbols = new List<Symbol>();
            foreach (IToken key in keys)
            {
                symbols.Add(FindSymbol(key));
            }
            return symbols.ToArray();
        }
    }
}
