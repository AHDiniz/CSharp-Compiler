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
            Override, Abstract, Static, Unsafe, Extern, Partial, Async, Ref, Const
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
            private Scope parentScope;
            private Stack<Scope> nestedScopes;
            private Dictionary<IToken, Symbol> symbols;
            private int scopeStartNodeIndex;

            public int CurrentScopeNode { get => scopeStartNodeIndex; }

            public Scope(int scopeStartNodeIndex, Scope parentScope = null)
            {
                this.parentScope = parentScope;
                this.scopeStartNodeIndex = scopeStartNodeIndex;
                this.nestedScopes = new Stack<Scopes>();
                this.symbols = new Dictionary<IToken, Symbol>();
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

            public Scope EnterScope(int scopeStartNodeIndex)
            {
                Scope s = new Scope(scopeStartNodeIndex, this);
                nestedScopes.Push(s);
                return s;
            }

            public Scope ExitScope()
            {
                return this.parent;
            }
        }

        private Scope mainScope;
        private Scope currentScope;

        public int CurrentScopeNode { get => currentScope.CurrentScopeNode; }

        public SymbolTable()
        {
            mainScope = new Scope(-1);
            currentScope = mainScope;
        }

        public void EnterScope(int nodeIndex)
        {
            currentScope = currentScope.EnterScope(nodeIndex);
        }

        public void ExitScope()
        {
            currentScope = currentScope.ExitScope();
        }

        public void AddSymbol(IToken key, Symbol value)
        {
            currentScope.AddSymbol(key, value);
        }

        public void RemoveSymbol(IToken key)
        {
            currentScope.RemoveSymbol(key);
        }

        public Symbol FindSymbol(IToken key)
        {
            return currentScope.FindSymbol(key);
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
