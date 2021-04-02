using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public partial class ASTBuilder : CSharpParserBaseListener
    {
        private Stack<ParserRuleContext> contextStack;
        private AST ast;

        public AST AbstractSyntaxTree
        {
            get => ast;
        }

        public ASTBuilder() : base()
        {
            contextStack = new Stack<ParserRuleContext>();
            ast = new AST();
        }

        public override void EnterCompilation_unit(CSharpParser.Compilation_unitContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering compilation_unit context.");
        }

        public override void ExitCompilation_unit(CSharpParser.Compilation_unitContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting compilation_unit context.");
        }

        public override void EnterNamespace_or_type_name(CSharpParser.Namespace_or_type_nameContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering namespace_or_type_name context.");
        }

        public override void ExitNamespace_or_type_name(CSharpParser.Namespace_or_type_nameContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting namespace_or_type_name context.");
        }

        public override void EnterType_(CSharpParser.Type_Context context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering namespace_or_type_name context.");
        }

        public override void ExitType_(CSharpParser.Type_Context context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting type_ context.");
        }

        public override void EnterBase_type(CSharpParser.Base_typeContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering base_type context.");
        }

        public override void ExitBase_type(CSharpParser.Base_typeContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting base_type context.");
        }

        public override void EnterTuple_type(CSharpParser.Tuple_typeContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering tuple_type context.");
        }

        public override void ExitTuple_type(CSharpParser.Tuple_typeContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting tuple_type context.");
        }

        public override void EnterTuple_element(CSharpParser.Tuple_elementContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering tuple_element context.");
        }

        public override void ExitTuple_element(CSharpParser.Tuple_elementContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting tuple_element context.");
        }

        public override void EnterSimple_type(CSharpParser.Simple_typeContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering simple_type context.");
        }

        public override void ExitSimple_type(CSharpParser.Simple_typeContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting simple_type context.");
        }

        public override void EnterNumeric_type(CSharpParser.Numeric_typeContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering numeric_type context.");
        }

        public override void ExitNumeric_type(CSharpParser.Numeric_typeContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting numeric_type context.");
        }

        public override void EnterIntegral_type(CSharpParser.Integral_typeContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering integral_type context.");
        }

        public override void ExitIntegral_type(CSharpParser.Integral_typeContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting integral_type context.");
        }

        public override void EnterFloating_point_type(CSharpParser.Floating_point_typeContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering floating_point_type context.");
        }

        public override void ExitFloating_point_type(CSharpParser.Floating_point_typeContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting floating_point_type context.");
        }

        public override void EnterClass_type(CSharpParser.Class_typeContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering class_type context.");
        }

        public override void ExitClass_type(CSharpParser.Class_typeContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting class_type context.");
        }

        public override void EnterType_argument_list(CSharpParser.Type_argument_listContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering type_argument_list context.");
        }

        public override void ExitType_argument_list(CSharpParser.Type_argument_listContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting type_argument_list context.");
        }

        public override void EnterArgument_list(CSharpParser.Argument_listContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering argument_list context.");
        }

        public override void ExitArgument_list(CSharpParser.Argument_listContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting argument_list context.");
        }

        public override void EnterArgument(CSharpParser.ArgumentContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering argument context.");
        }

        public override void ExitArgument(CSharpParser.ArgumentContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting argument context.");
        }

        public override void EnterPredefined_type(CSharpParser.Predefined_typeContext context)
        {
            contextStack.Push(context);
            Console.WriteLine("Entering predefined_type context.");
        }

        public override void ExitPredefined_type(CSharpParser.Predefined_typeContext context)
        {
            contextStack.Pop();
            Console.WriteLine("Exiting predefined_type context.");
        }
    }
}