using System;
using System.Collections.Generics;
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
            VariableDeclaration
        }

        private Token token;
        private List<int> children;
        private Kind kind = Kind.None;
        private Type type;
        private object data;

        public Node(Token token, int[] children, Kind kind, Type type, object data = null)
        {
            this.token = token;
            this.children = new List<int>(children);
            this.kind = kind;
            this.type = type;
            this.data = data;
        }
    }
}
