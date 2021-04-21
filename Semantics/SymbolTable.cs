using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public class Symbol
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
                this.nestedScopes = new Stack<Scope>();
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

            public Symbol FindSymbol(IToken key, AST ast)
            {
                Symbol result = symbols[key]; // Look for symbol in the current scope
                if (result != null) return result;

                foreach (Scope nested in nestedScopes)
                {
                    Node n = ast.GetNode(nested.CurrentScopeNode);
                    
                    // If token matches return symbol:
                    if (n.Token == key)
                    {
                        // If it's a class definition, return symbol in the class type data:
                        if (n.NodeKind == Node.Kind.ClassDefinition)
                        {
                            result = ((ClassType)(n.Type)).Symbol;
                        }
                        else
                        {
                            result = (Symbol)(n.Data); // Otherwise, return the data in the actual node
                        }

                        return result;
                    }
                }

                result = parentScope.FindSymbol(key, ast); // Search in parent scope if not in the requested scope

                return result;
            }

            public Scope EnterScope(int scopeStartNodeIndex)
            {
                Scope s = new Scope(scopeStartNodeIndex, this);
                nestedScopes.Push(s);
                return s;
            }

            public Scope ExitScope()
            {
                return this.parentScope;
            }
        }

        private Scope mainScope;
        private Scope currentScope;

        private List<Type> types;

        public int CurrentScopeNode { get => currentScope.CurrentScopeNode; }

        public SymbolTable()
        {
            mainScope = new Scope(-1);
            types = new List<Type>();
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

        public Symbol FindSymbol(IToken key, AST ast)
        {
            return currentScope.FindSymbol(key, ast);
        }

        public Symbol[] FindSymbols(IToken[] keys, AST ast)
        {
            List<Symbol> symbols = new List<Symbol>();
            foreach (IToken key in keys)
            {
                symbols.Add(FindSymbol(key, ast));
            }
            return symbols.ToArray();
        }

        public Type FindType(IToken token)
        {
            foreach(Type t in types)
            {
                if (t.Token == token)
                    return t;
            }
            return null;
        }

        public void AddType(Type t)
        {
            types.Add(t);
        }
    }
}
