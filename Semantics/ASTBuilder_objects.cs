using System;
using System.Reflection;
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
            Console.WriteLine("Entering object_initializer context.");
            
        }

        public override void ExitObject_initializer(CSharpParser.Object_initializerContext context)
        {
            Console.WriteLine("Exiting object_initializer context.");
            
        }

        public override void EnterCollection_initializer(CSharpParser.Collection_initializerContext context)
        {
            Console.WriteLine("Entering collection_initializer context.");
            
        }

        public override void ExitCollection_initializer(CSharpParser.Collection_initializerContext context)
        {
            Console.WriteLine("Exiting collection_initializer context.");
            
        }

        public override void EnterMember_initializer(CSharpParser.Member_initializerContext context)
        {
            Console.WriteLine("Entering member_initializer context.");
            
        }

        public override void ExitMember_initializer(CSharpParser.Member_initializerContext context)
        {
            Console.WriteLine("Exiting member_initializer context.");
            
        }

        public override void EnterMember_initializer_list(CSharpParser.Member_initializer_listContext context)
        {
            Console.WriteLine("Entering member_initializer_list context.");
            
        }

        public override void ExitMember_initializer_list(CSharpParser.Member_initializer_listContext context)
        {
            Console.WriteLine("Exiting member_initializer_list context.");
            
        }

        public override void EnterElement_initializer(CSharpParser.Element_initializerContext context)
        {
            Console.WriteLine("Entering element_initializer context.");
            
        }

        public override void ExitElement_initializer(CSharpParser.Element_initializerContext context)
        {
            Console.WriteLine("Exiting element_initializer context.");
            
        }

        public override void EnterAnonymous_object_initializer(CSharpParser.Anonymous_object_initializerContext context)
        {
            Console.WriteLine("Entering anonymous_object_initializer context.");
            
        }

        public override void ExitAnonymous_object_initializer(CSharpParser.Anonymous_object_initializerContext context)
        {
            Console.WriteLine("Exiting anonymous_object_initializer context.");
            
        }

        public override void EnterMember_declarator_list(CSharpParser.Member_declarator_listContext context)
        {
            Console.WriteLine("Entering member_declarator_list context.");
            
        }

        public override void ExitMember_declarator_list(CSharpParser.Member_declarator_listContext context)
        {
            Console.WriteLine("Exiting member_declarator_list context.");
            
        }

        public override void EnterMember_declarator(CSharpParser.Member_declaratorContext context)
        {
            Console.WriteLine("Entering member_declarator context.");
        }

        public override void ExitMember_declarator(CSharpParser.Member_declaratorContext context)
        {
            Console.WriteLine("Exiting member_declarator context.");
        }

        public override void EnterClass_definition(CSharpParser.Class_definitionContext context)
        {
            Console.WriteLine("Entering class_definition context.");
            
            // Add class symbol to symbol table: parent class, type parameters and other informations and add to node data
            // ClassSymbol classSymbol = new ClassSymbol();
            
            Type type = new Type(context.CLASS().Symbol);
            Node classNode = new Node(context.CLASS().Symbol, Node.Kind.ClassDefinition, type);
            int classNodeIndex = ast.NodeIndex(classNode);
            
            // Enter scope in the symbol table
            symbolTable.EnterScope(classNodeIndex);
            
            // Adding information to the abstract syntax tree:

            // Adding the class node:
            ast.AddNode(classNode);

            // Adding an identifier node as a class node child:
            CSharpParser.IdentifierContext idCtx = context.identifier();
            List<ITerminalNode> terminalNodes = new List<ITerminalNode>(idCtx.GetTokens(0));

            // Node idNode = new Node(terminalNodes[0].Symbol, Node.Kind.Identifier, null);
            // ast.AddNode(idNode);
            // int idNodeIndex = ast.NodeIndex(idNode);
            // classNode.AddChildIndex(idNodeIndex);

            // Adding the class body node as a class node child:
            CSharpParser.Class_bodyContext bodyCtx = context.class_body();
            Node bodyNode = new Node(null, Node.Kind.ClassBody, null);
            ast.AddNode(bodyNode);
            int bodyNodeIndex = ast.NodeIndex(bodyNode);
            classNode.AddChildIndex(bodyNodeIndex);
        }

        public override void ExitClass_definition(CSharpParser.Class_definitionContext context)
        {
            // Exit class scope in the symbol table
            symbolTable.ExitScope();
        }

        public override void EnterClass_member_declaration(CSharpParser.Class_member_declarationContext context)
        {
            // Getting the current class node from symbol table in scope:

            // Getting the member modifiers:

            CSharpParser.All_member_modifiersContext modsCtx = context.all_member_modifiers();
            if (modsCtx != null)
            {
                CSharpParser.All_member_modifierContext[] modifiers = modsCtx.all_member_modifier();
                foreach (CSharpParser.All_member_modifierContext modifier in modifiers)
                {

                }
            }
        }
    }
}