using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public partial class ASTBuilder : CSharpParserBaseListener
    {
        public override void EnterQuery_body_clause(CSharpParser.Query_body_clauseContext context)
        {
            Console.WriteLine("Entering query_body_clause context.");
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

        public override void ExitQuery_body_clause(CSharpParser.Query_body_clauseContext context)
        {
            Console.WriteLine("Exiting query_body_clause context.");
            // Exit class scope in the symbol table
            symbolTable.ExitScope();
            
        }

        public override void EnterLet_clause(CSharpParser.Let_clauseContext context)
        {
            Console.WriteLine("Entering let_clause context.");
            
        }

        public override void ExitLet_clause(CSharpParser.Let_clauseContext context)
        {
            Console.WriteLine("Exiting let_clause context.");
            
        }

        public override void EnterWhere_clause(CSharpParser.Where_clauseContext context)
        {
            Console.WriteLine("Entering where_clause context.");
            
        }

        public override void ExitWhere_clause(CSharpParser.Where_clauseContext context)
        {
            Console.WriteLine("Exiting where_clause context.");
            
        }

        public override void EnterCombined_join_clause(CSharpParser.Combined_join_clauseContext context)
        {
            Console.WriteLine("Entering combined_join_clause context.");
            
        }

        public override void ExitCombined_join_clause(CSharpParser.Combined_join_clauseContext context)
        {
            Console.WriteLine("Exiting combined_join_clause context.");
            
        }

        public override void EnterOrderby_clause(CSharpParser.Orderby_clauseContext context)
        {
            Console.WriteLine("Entering orderby_clause context.");
            
        }

        public override void ExitOrderby_clause(CSharpParser.Orderby_clauseContext context)
        {
            Console.WriteLine("Exiting orderby_clause context.");
            
        }
    }
}
