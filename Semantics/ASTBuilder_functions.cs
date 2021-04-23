using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public partial class ASTBuilder : CSharpParserBaseListener
    {
        public override void EnterMethod_declaration(CSharpParser.Method_declarationContext context)
        {
            Console.WriteLine("Entering method_declaration context.");

            // Getting the current scope node and current modifiers:
            Symbol.ModifierFlag modFlags = TreatModTokens();
            modifiersTokens.Clear();

            Node currentScopeNode = ast.GetNode(symbolTable.CurrentScopeNode);
            ClassSymbol ownerSymbol = (ClassSymbol)(currentScopeNode.Data);

            MethodSymbol methodSymbol = new MethodSymbol(modFlags, ownerSymbol);
            IToken idToken = context.method_member_name().identifier(0).Start;

            symbolTable.AddSymbol(idToken, methodSymbol);

            // Creating the method AST node:
            Node methodNode = new Node(idToken, Node.Kind.MethodDeclaration, currentType, methodSymbol);
            ast.AddNode(methodNode);
            int methodIndex = ast.NodeIndex(methodNode);
            currentScopeNode.AddChildIndex(methodIndex);

            // Entering the method scope:
            symbolTable.EnterScope(methodIndex);

            // Creating a node for each parameter:
            CSharpParser.Formal_parameter_listContext parameters = context.formal_parameter_list();
            if (parameters != null)
            {
                CSharpParser.Fixed_parameterContext[] fixedParams = parameters.fixed_parameters().fixed_parameter();
                foreach (CSharpParser.Fixed_parameterContext parameter in fixedParams)
                {
                    CSharpParser.Arg_declarationContext arg = parameter.arg_declaration();
                    if (arg != null)
                    {
                        CSharpParser.Type_Context typeContext = arg.type_();
                        IToken varToken = arg.identifier().Start;
                        Type t = TreatTypeContext(typeContext);

                        VariableSymbol varSymbol = new VariableSymbol(Symbol.ModifierFlag.None, methodSymbol);
                        symbolTable.AddSymbol(varToken, varSymbol);

                        Node varNode = new Node(varToken, Node.Kind.MethodParameter, t, null);
                        ast.AddNode(varNode);
                        int varIndex = ast.NodeIndex(varNode);
                        methodNode.AddChildIndex(varIndex);
                    }
                }
            }

            // Creating the subtree for the method implementation:
            CSharpParser.BlockContext blockContext = context.method_body().block();
            if (blockContext != null) TreatBlock(blockContext, methodNode);
        }

        public override void ExitMethod_declaration(CSharpParser.Method_declarationContext context)
        {
            Console.WriteLine("Exiting method_declaration context.");
            symbolTable.ExitScope();
        }
    }
}