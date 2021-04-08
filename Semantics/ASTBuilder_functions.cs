using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public partial class ASTBuilder : CSharpParserBaseListener
    {
        public override void EnterLambda_expression(CSharpParser.Lambda_expressionContext context)
        {
            
            Console.WriteLine("Entering lambda_expression context.");
        }

        public override void ExitLambda_expression(CSharpParser.Lambda_expressionContext context)
        {
            
            Console.WriteLine("Exiting lambda_expression context.");
        }

        public override void EnterAnonymous_function_signature(CSharpParser.Anonymous_function_signatureContext context)
        {
            
            Console.WriteLine("Entering anonymous_function_signature context.");
        }

        public override void ExitAnonymous_function_signature(CSharpParser.Anonymous_function_signatureContext context)
        {
            
            Console.WriteLine("Exiting anonymous_function_signature context.");
        }

        public override void EnterExplicit_anonymous_function_parameter_list(CSharpParser.Explicit_anonymous_function_parameter_listContext context)
        {
            
            Console.WriteLine("Entering explicit_anonymous_function_parameter_list context.");
        }

        public override void ExitExplicit_anonymous_function_parameter_list(CSharpParser.Explicit_anonymous_function_parameter_listContext context)
        {
            
            Console.WriteLine("Exiting explicit_anonymous_function_parameter_list context.");
        }

        public override void EnterExplicit_anonymous_function_parameter(CSharpParser.Explicit_anonymous_function_parameterContext context)
        {
            
            Console.WriteLine("Entering explicit_anonymous_function_parameter context.");
        }

        public override void ExitExplicit_anonymous_function_parameter(CSharpParser.Explicit_anonymous_function_parameterContext context)
        {
            
            Console.WriteLine("Exiting explicit_anonymous_function_parameter context.");
        }

        public override void EnterImplicit_anonymous_function_parameter_list(CSharpParser.Implicit_anonymous_function_parameter_listContext context)
        {
            
            Console.WriteLine("Entering implicit_anonymous_function_parameter_list context.");
        }

        public override void ExitImplicit_anonymous_function_parameter_list(CSharpParser.Implicit_anonymous_function_parameter_listContext context)
        {
            
            Console.WriteLine("Exiting implicit_anonymous_function_parameter_list context.");
        }

        public override void EnterAnonymous_function_body(CSharpParser.Anonymous_function_bodyContext context)
        {
            
            Console.WriteLine("Entering anonymous_function_body context.");
        }

        public override void ExitAnonymous_function_body(CSharpParser.Anonymous_function_bodyContext context)
        {
            
            Console.WriteLine("Exiting anonymous_function_body context.");
        }
    }
}