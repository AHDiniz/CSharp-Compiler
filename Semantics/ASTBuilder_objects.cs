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
            Node currentScopeNode = ast.GetNode(symbolTable.CurrentScopeNode);

            // Add class symbol to symbol table: parent class, owner class and other informations and add to node data:
            
            // Getting the parent classes:
            List<IToken> baseTokens = new List<IToken>(); // Will hold the base class tokens

            // Getting the base classes' names identifiers:
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

            // Getting the base classes' symbols:
            Symbol[] baseSymbols = symbolTable.FindSymbols(baseTokens.ToArray(), ast);
            List<ClassSymbol> baseClassSymbols = new List<ClassSymbol>();
            foreach (Symbol bs in baseSymbols) baseClassSymbols.Add((ClassSymbol)bs);

            // Getting the class' modifiers:
            Symbol.ModifierFlag modFlags = TreatModTokens();
            modifiersTokens.Clear();

            // Creating the class symbol:
            ClassSymbol classSymbol = new ClassSymbol(modFlags, baseClassSymbols.ToArray());
            
            // Adding the class node as a child to the current scope's AST node:
            Type type = new Type(context.CLASS().Symbol);
            Node classNode = new Node(context.CLASS().Symbol, Node.Kind.ClassDefinition, type);
            int classNodeIndex = ast.NodeIndex(classNode);
            currentScopeNode.AddChildIndex(classNodeIndex);
            
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

            // Enter scope in the symbol table
            symbolTable.EnterScope(bodyNodeIndex);

            symbolTable.AddSymbol(idToken, classSymbol);
        }

        public override void ExitClass_definition(CSharpParser.Class_definitionContext context)
        {
            Console.WriteLine("Exiting class_definition context.");
            // Exit class scope in the symbol table
            symbolTable.ExitScope();
        }

        public override void EnterStruct_definition(CSharpParser.Struct_definitionContext context)
        {
            Console.WriteLine("Entering struct_definition context.");

            // Getting the current scope node:
            Node currentScopeNode = ast.GetNode(symbolTable.CurrentScopeNode);

            // Add class symbol to symbol table: parent class, owner class and other informations and add to node data:
            
            // Getting the parent classes:
            List<IToken> interfaceTokens = new List<IToken>(); // Will hold the base class tokens

            // Getting the interfaces' names identifiers:
            CSharpParser.Struct_interfacesContext interfaces = context.struct_interfaces();
            if (interfaces != null)
            {
                CSharpParser.Interface_type_listContext typeListCtx = interfaces.interface_type_list();
                if (typeListCtx != null)
                {
                    CSharpParser.Namespace_or_type_nameContext[] types = typeListCtx.namespace_or_type_name();
                    foreach (CSharpParser.Namespace_or_type_nameContext name in types)
                    {
                        CSharpParser.IdentifierContext[] ids = name.identifier();
                        foreach (CSharpParser.IdentifierContext id in ids)
                        {
                            interfaceTokens.Add(id.Start);
                        }
                    }
                }
            }

            // Getting the base classes' symbols:
            Symbol[] interfaceSymbols = symbolTable.FindSymbols(interfaceTokens.ToArray(), ast);
            List<ClassSymbol> interfaceClassSymbols = new List<ClassSymbol>();
            foreach (Symbol bs in interfaceSymbols) interfaceClassSymbols.Add((ClassSymbol)bs);

            // Getting the class' modifiers:
            Symbol.ModifierFlag modFlags = TreatModTokens();
            modifiersTokens.Clear();

            // Creating the class symbol:
            ClassSymbol classSymbol = new ClassSymbol(modFlags, interfaceClassSymbols.ToArray());
            
            // Adding the class node as a child to the current scope's AST node:
            Type type = new Type(context.STRUCT().Symbol);
            Node structNode = new Node(context.STRUCT().Symbol, Node.Kind.ClassDefinition, type);
            int structNodeIndex = ast.NodeIndex(structNode);
            currentScopeNode.AddChildIndex(structNodeIndex);
            
            // Adding the struct node:
            ast.AddNode(structNode);

            // Adding an identifier node as a struct node child:
            CSharpParser.IdentifierContext idCtx = context.identifier();
            IToken idToken = idCtx.Start;

            // Adding the struct body node as a class node child:
            CSharpParser.Struct_bodyContext bodyCtx = context.struct_body();
            ClassType classType = new ClassType(idToken, ClassTag.Struct, classSymbol);
            Node bodyNode = new Node(idToken, Node.Kind.ClassBody, classType);
            ast.AddNode(bodyNode);
            int bodyNodeIndex = ast.NodeIndex(bodyNode);
            structNode.AddChildIndex(bodyNodeIndex);

            // Enter scope in the symbol table
            symbolTable.EnterScope(bodyNodeIndex);

            symbolTable.AddSymbol(idToken, classSymbol);
        }

        public override void ExitStruct_definition(CSharpParser.Struct_definitionContext context)
        {
            Console.WriteLine("Exiting struct_definition context.");
            // Exit struct scope in the symbol table
            symbolTable.ExitScope();
        }

        public override void EnterInterface_definition(CSharpParser.Interface_definitionContext context)
        {
            Console.WriteLine("Entering interface_definition context.");

            // Getting the current scope node:
            Node currentScopeNode = ast.GetNode(symbolTable.CurrentScopeNode);

            // Add class symbol to symbol table: parent class, owner class and other informations and add to node data:
            
            // Getting the parent classes:
            List<IToken> baseTokens = new List<IToken>(); // Will hold the base class tokens

            // Getting the interfaces' names identifiers:
            CSharpParser.Interface_baseContext interfaces = context.interface_base();
            if (interfaces != null)
            {
                CSharpParser.Interface_type_listContext typeListCtx = interfaces.interface_type_list();
                if (typeListCtx != null)
                {
                    CSharpParser.Namespace_or_type_nameContext[] types = typeListCtx.namespace_or_type_name();
                    foreach (CSharpParser.Namespace_or_type_nameContext name in types)
                    {
                        CSharpParser.IdentifierContext[] ids = name.identifier();
                        foreach (CSharpParser.IdentifierContext id in ids)
                        {
                            baseTokens.Add(id.Start);
                        }
                    }
                }
            }

            // Getting the base classes' symbols:
            Symbol[] baseSymbols = symbolTable.FindSymbols(baseTokens.ToArray(), ast);
            List<ClassSymbol> baseClassSymbols = new List<ClassSymbol>();
            foreach (Symbol bs in baseSymbols) baseClassSymbols.Add((ClassSymbol)bs);

            // Getting the interface's modifiers:
            Symbol.ModifierFlag modFlags = TreatModTokens();
            modifiersTokens.Clear();

            // Creating the interface symbol:
            ClassSymbol interfaceSymbol = new ClassSymbol(modFlags, baseClassSymbols.ToArray());
            
            // Adding the class node as a child to the current scope's AST node:
            Type type = new Type(context.INTERFACE().Symbol);
            Node interfaceNode = new Node(context.INTERFACE().Symbol, Node.Kind.ClassDefinition, type);
            int interfaceNodeIndex = ast.NodeIndex(interfaceNode);
            currentScopeNode.AddChildIndex(interfaceNodeIndex);
            
            // Adding the class node:
            ast.AddNode(interfaceNode);

            // Adding an identifier node as a class node child:
            CSharpParser.IdentifierContext idCtx = context.identifier();
            IToken idToken = idCtx.Start;

            // Adding the class body node as a class node child:
            CSharpParser.Class_bodyContext bodyCtx = context.class_body();
            ClassType interfaceType = new ClassType(idToken, ClassTag.Interface, interfaceSymbol);
            Node bodyNode = new Node(idToken, Node.Kind.ClassBody, interfaceType);
            ast.AddNode(bodyNode);
            int bodyNodeIndex = ast.NodeIndex(bodyNode);
            interfaceNode.AddChildIndex(bodyNodeIndex);

            // Enter scope in the symbol table
            symbolTable.EnterScope(bodyNodeIndex);

            symbolTable.AddSymbol(idToken, interfaceSymbol);
        }

        public override void ExitInterface_definition(CSharpParser.Interface_definitionContext context)
        {
            Console.WriteLine("Exiting interface_definition context.");

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
            Type t = TreatTypeContext(context.type_());

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
                    AttributeSymbol constantSymbol = new AttributeSymbol(modFlags, ownerClass);

                    // Creating the constant member node:
                    Node constMemberNode = new Node(idToken, Node.Kind.MemberVariableDeclaration, t, constantSymbol);
                    ast.AddNode(constMemberNode);
                    parentNode.AddChildIndex(ast.NodeIndex(constMemberNode));

                    // Adding the symbol to the table:
                    symbolTable.AddSymbol(idToken, constantSymbol);
                }
                else
                {
                    // Creating a method variable
                    // Getting the owner method symbol:
                    MethodSymbol ownerMethod = (MethodSymbol)(symbolTable.FindSymbol(parentNode.Token, ast));
                    VariableSymbol constantSymbol = new VariableSymbol(modFlags, ownerMethod);

                    // Creating the constant variable node:
                    Node constVarNode = new Node(idToken, Node.Kind.MethodVariableDeclaration, t, constantSymbol);
                    ast.AddNode(constVarNode);
                    parentNode.AddChildIndex(ast.NodeIndex(constVarNode));

                    // Adding the symbol to the table:
                    symbolTable.AddSymbol(idToken, constantSymbol);
                }
            }
        }

        public override void EnterEnum_definition(CSharpParser.Enum_definitionContext context)
        {
            Console.WriteLine("Entering enum_definition context.");

            // Getting the current scope node in the AST:
            Node parentNode = ast.GetNode(symbolTable.CurrentScopeNode);

            // Getting the modifier for the enumerator:
            Symbol.ModifierFlag modFlags = TreatModTokens();
            modifiersTokens.Clear();

            // Getting the base tokens for the enumeration:
            CSharpParser.Enum_baseContext enumBase = context.enum_base();
            Type enumBaseType = null;
            if (enumBase != null) enumBaseType = TreatTypeContext(enumBase.type_());

            // Accessing the enum's body values:
            List<IToken> values = new List<IToken>();
            CSharpParser.Enum_member_declarationContext[] members = context.enum_body().enum_member_declaration();
            foreach (CSharpParser.Enum_member_declarationContext member in members)
            {
                CSharpParser.IdentifierContext id = member.identifier();
                values.Add(id.Start);
            }

            // Creating the enum symbol:
            EnumSymbol enumSymbol = new EnumSymbol(modFlags, values.ToArray(), enumBaseType);

            // Creating the enum type name:
            CSharpParser.IdentifierContext idContext = context.identifier();
            IToken idToken = idContext.Start;

            // Creating the enum node and adding it to the AST:
            Type enumType = new Type(idToken);
            Node enumNode = new Node(idToken, Node.Kind.EnumDefinition, enumType, enumSymbol);
            parentNode.AddChildIndex(ast.NodeIndex(enumNode));
            ast.AddNode(enumNode);
        }
    }
}