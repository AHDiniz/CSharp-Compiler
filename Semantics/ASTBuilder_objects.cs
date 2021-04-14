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

        public override void EnterAll_member_modifiers(CSharpParser.All_member_modifiersContext context)
        {
            Console.WriteLine("Entering all_member_modifiers context.");

            CSharpParser.All_member_modifierContext[] allModsCtxs = context.all_member_modifier();

            foreach (CSharpParser.All_member_modifierContext mod in allModsCtxs)
            {
                modifiersTokens.Add(mod.Start);
            }
        }

        public override void EnterClass_definition(CSharpParser.Class_definitionContext context)
        {
            Console.WriteLine("Entering class_definition context.");

            // Getting the current scope node:
            

            // Add class symbol to symbol table: parent class, owner class and other informations and add to node data:
            
            // Getting the parent classes:
            List<IToken> baseTokens = new List<IToken>(); // Will hold the base class tokens

            CSharpParser.Class_baseContext classBaseCtx = context.class_base();
            if (classBaseCtx != null)
            {
                CSharpParser.Class_typeContext classTypeCtx = classBaseCtx.class_type();

                if (classTypeCtx != null)
                {
                    CSharpParser.Namespace_or_type_nameContext typeNameCtx = classTypeCtx.namespace_or_type_name();
                    if (typeNameCtx != null)
                    {
                        CSharpParser.IdentifierContext[] typeIDCtxs = typeNameCtx.identifier();
                        foreach (CSharpParser.IdentifierContext id in typeIDCtxs)
                        {
                            baseTokens.Add(id.Start);
                        }
                    }
                    else
                    {
                        baseTokens.Add(typeNameCtx.Start);
                    }
                }
            }

            Symbol[] baseSymbols = symbolTable.FindSymbols(baseTokens.ToArray(), ast);
            List<ClassSymbol> baseClassSymbols = new List<ClassSymbol>();
            foreach (Symbol bs in baseSymbols) baseClassSymbols.Add((ClassSymbol)bs);

            Symbol.ModifierFlag modFlags = TreatModTokens();
            modifiersTokens.Clear();

            ClassSymbol classSymbol = new ClassSymbol(modFlags, baseClassSymbols.ToArray(), ClassTag.Class);
            
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
            IToken idToken = idCtx.Start;

            // Adding the class body node as a class node child:
            CSharpParser.Class_bodyContext bodyCtx = context.class_body();
            ClassType classType = new ClassType(idToken, ClassTag.Class, classSymbol);
            Node bodyNode = new Node(idToken, Node.Kind.ClassBody, classType);
            ast.AddNode(bodyNode);
            int bodyNodeIndex = ast.NodeIndex(bodyNode);
            classNode.AddChildIndex(bodyNodeIndex);

            symbolTable.AddSymbol(idToken, classSymbol);
        }

        public override void ExitClass_definition(CSharpParser.Class_definitionContext context)
        {
            Console.WriteLine("Exiting class_definition context.");
            // Exit class scope in the symbol table
            symbolTable.ExitScope();
        }

        public override void EnterDestructor_definition(CSharpParser.Destructor_definitionContext context)
        {
            Console.WriteLine("Entering descrutor_definition context.");

            // Getting the current class node from symbol table in scope:
            Node currentClassScopeNode = ast.GetNode(symbolTable.CurrentScopeNode);
            ClassType classType = (ClassType)(currentClassScopeNode.Type);
            ClassSymbol classSymbol = classType.Symbol;
            IToken classToken = currentClassScopeNode.Token;

            // Getting the destructor modifiers:
            Symbol.ModifierFlag modFlags = TreatModTokens();
            modifiersTokens.Clear();

            // Creating the destructor symbol:
            DestructorSymbol destructorSymbol = new DestructorSymbol(modFlags, classSymbol);

            // Creating the destructor AST node:
            BuiltInType destructorType = new BuiltInType(classToken, TypeTag.Destructor);
            IToken destructorToken = context.TILDE().Symbol;
            Node destructorNode = new Node(destructorToken, Node.Kind.Destructor, destructorType, destructorSymbol);
            ast.AddNode(destructorNode);

            // Adding the new node as a child of the class node:
            currentClassScopeNode.AddChildIndex(ast.NodeIndex(destructorNode));
            
            // Adding the destructor symbol to the symbol table:
            int destructorNodeIndex = ast.NodeIndex(destructorNode);
            symbolTable.EnterScope(destructorNodeIndex);
            symbolTable.AddSymbol(destructorToken, destructorSymbol);
        }

        public override void ExitDestructor_definition(CSharpParser.Destructor_definitionContext context)
        {
            Console.WriteLine("Exiting destructor_definition context.");

            // Exiting the destructor scope:
            symbolTable.ExitScope();
        }

        public override void EnterConstructor_declaration(CSharpParser.Constructor_declarationContext context)
        {
            Console.WriteLine("Entering constructor_declaration context.");

            // Getting the current class node from symbol table in scope:
            Node currentClassScopeNode = ast.GetNode(symbolTable.CurrentScopeNode);
            ClassType classType = (ClassType)(currentClassScopeNode.Type);
            ClassSymbol classSymbol = classType.Symbol;
            IToken classToken = currentClassScopeNode.Token;

            // Getting the destructor modifiers:
            Symbol.ModifierFlag modFlags = TreatModTokens();
            modifiersTokens.Clear();

            // Creating the destructor symbol:
            ConstructorSymbol constructorSymbol = new ConstructorSymbol(modFlags, classSymbol);

            // Creating the destructor AST node:
            BuiltInType constructorType = new BuiltInType(classToken, TypeTag.Constructor);
            Node constructorNode = new Node(classToken, Node.Kind.Constructor, constructorType, constructorSymbol);
            ast.AddNode(constructorNode);

            // Adding the new node as a child of the class node:
            currentClassScopeNode.AddChildIndex(ast.NodeIndex(constructorNode));
            
            // Adding the destructor symbol to the symbol table:
            int constructorNodeIndex = ast.NodeIndex(constructorNode);
            symbolTable.EnterScope(constructorNodeIndex);
            symbolTable.AddSymbol(classToken, constructorSymbol);
        }

        public override void ExitConstructor_declaration(CSharpParser.Constructor_declarationContext context)
        {
            Console.WriteLine("Entering constructor_declaration context.");

            // Exiting the constructor scope:
            symbolTable.ExitScope();
        }

        public override void EnterConstant_declaration(CSharpParser.Constant_declarationContext context)
        {
            Console.WriteLine("Entering constant_declaration context.");

            // Getting the current scope parent node:
            Node parentNode = ast.GetNode(symbolTable.CurrentScopeNode);
            Node.Kind parentKind = parentNode.NodeKind;

            // Getting the constants' type:
            Type t = TreatTypeContext(context.type_())

            // Getting all the modifiers:
            Symbol.ModifierFlag modFlags = TreatModTokens();
            modifiersTokens.Clear();
            modFlags |= Symbol.ModifierFlag.Const;

            // Getting the name of the constant(s):
            CSharpParser.Constant_declaratorContext[] declarators = context.constant_declarators().constant_declarator();
            foreach (CSharpParser.Constant_declaratorContext declarator in declarators)
            {
                CSharpParser.IdentifierContext idCtx = declarator.identifier();
                IToken idToken = idCtx.Start;

                // Creating the constant symbol:
                if (parentKind == Node.Kind.ClassBody)
                {
                    // Getting the owner class symbol:
                    ClassSymbol ownerClass = ((ClassType)(parentNode.Type)).Symbol;
                    AttributeSymbol constantSymbol = new AttributeSymbol(mods, ownerClass);

                    // Creating the constant member node:
                    Node constAttrNode = new Node(idToken, Node.Kind.ConstantDeclaration, t, constantSymbol);

                    // Adding the symbol to the table:
                    symbolTable.AddSymbol(idToken, constantSymbol);
                }
                else
                {
                    // Creating a method variable
                }

                // Creating the constant node:

                // Adding the node to the AST:

                // Adding the symbol to the symbol table:
            }
        }

        public override void EnterTyped_member_declaration(CSharpParser.Typed_member_declarationContext context)
        {
            Console.WriteLine("Entering typed_member_declaration context.");

            // Getting all the modifiers:
            Symbol.ModifierFlag modFlags = TreatModTokens();
            modifiersTokens.Clear();
            if (context.REF() != null) modFlags |= Symbol.ModifierFlag.Ref;
            if (context.READONLY() != null) modFlags |= Symbol.ModifierFlag.ReadOnly;
        }

        public override void EnterEvent_declaration(CSharpParser.Event_declarationContext context)
        {
            Console.WriteLine("Entering event_declaration context.");

            // Getting all the modifiers:
            Symbol.ModifierFlag modFlags = TreatModTokens();
            modifiersTokens.Clear();
        }
    }
}