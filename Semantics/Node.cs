using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace CSharp_Compiler.Semantics
{
    public class Node
    {
        public enum Kind
        {
            None,
            CodeBlock,
            Assignment,
            EqualityComparision,
            VariableDeclaration,
            ClassDefinition,
            ClassBody,
            Identifier
        }

        private IToken token;
        private List<int> children;
        private Kind kind = Kind.None;
        private Type type;
        private object data;

        public object Data
        {
            set => this.data = value;
            get => data;
        }

        public IToken Token { get => token; }

        public Type Type { get => type; }

        public Node(IToken token, Kind kind, Type type, object data = null)
        {
            this.token = token;
            this.children = new List<int>();
            this.kind = kind;
            this.type = type;
            this.data = data;
        }

        public void AddChildIndex(int index)
        {
            children.Add(index);
        }
    }
}
