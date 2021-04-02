using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace CSharp_Compiler.Semantics
{
    public class AST
    {
        private List<Node> nodes;
        private int head;

        public AST()
        {
            nodes = new List<Node>();
            head = -1;
        }
    }
}
