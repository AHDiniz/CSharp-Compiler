using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public partial class ASTBuilder : CSharpParserBaseListener
    {
        public override void EnterObject_initializer(CSharpParser.Object_initializerContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering object_initializer context.");
        }

        public override void ExitObject_initializer(CSharpParser.Object_initializerContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting object_initializer context.");
        }

        public override void EnterCollection_initializer(CSharpParser.Collection_initializerContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering collection_initializer context.");
        }

        public override void ExitCollection_initializer(CSharpParser.Collection_initializerContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting collection_initializer context.");
        }

        public override void EnterMember_initializer(CSharpParser.Member_initializerContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering member_initializer context.");
        }

        public override void ExitMember_initializer(CSharpParser.Member_initializerContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting member_initializer context.");
        }

        public override void EnterMember_initializer_list(CSharpParser.Member_initializer_listContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering member_initializer_list context.");
        }

        public override void ExitMember_initializer_list(CSharpParser.Member_initializer_listContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting member_initializer_list context.");
        }

        public override void EnterElement_initializer(CSharpParser.Element_initializerContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering element_initializer context.");
        }

        public override void ExitElement_initializer(CSharpParser.Element_initializerContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting element_initializer context.");
        }

        public override void EnterAnonymous_object_initializer(CSharpParser.Anonymous_object_initializerContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering anonymous_object_initializer context.");
        }

        public override void ExitAnonymous_object_initializer(CSharpParser.Anonymous_object_initializerContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting anonymous_object_initializer context.");
        }
    }
}