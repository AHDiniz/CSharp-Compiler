using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public partial class ASTBuilder : CSharpParserBaseListener
    {
        public override void EnterBlock(CSharpParser.BlockContext context)
        {
            Console.WriteLine("Entering block context.");

            // Getting the current scope node:
            Node currentScopeNode = ast.GetNode(symbolTable.CurrentScopeNode);

            // Creating a new node for the code block:
            Node blockNode = new Node(null, Node.Kind.CodeBlock, null);
            ast.AddNode(blockNode);
            int blockNodeIndex = ast.NodeIndex(blockNode);

            // Entering a new scope in the symbol table:
            symbolTable.EnterScope(blockNodeIndex);

            // Treating each statement in the statement list:
            CSharpParser.Statement_listContext listContext = context.statement_list();
            if (listContext != null)
            {
                CSharpParser.StatementContext[] statements = listContext.statement();
                foreach (CSharpParser.StatementContext statement in statements)
                {
                    TreatStatement(statement, blockNode);
                }
            }
        }

        public override void ExitBlock(CSharpParser.BlockContext context)
        {
            Console.WriteLine("Exiting block context.");

            // Exiting the code block scope:
            symbolTable.ExitScope();
        }

        private void TreatStatement(CSharpParser.StatementContext statement, Node parentNode)
        {
            CSharpParser.Labeled_StatementContext labeledStatement = statement.labeled_Statement();
            if (labeledStatement != null)
            {
                // Getting the label token:
                CSharpParser.IdentifierContext label = labeledStatement.identifier();
                IToken labelToken = label.Start;
                
                // Adding the label to the symbol table:
                Symbol labelSymbol = new Symbol(Symbol.ModifierFlag.None);
                symbolTable.AddSymbol(labelToken, labelSymbol);

                // Creating the label node and adding it to the AST:
                Node labelNode = new Node(labelToken, Node.Kind.Label, null);
                ast.AddNode(labelNode);
                int labelNodeIndex = ast.NodeIndex(labelNode);

                parentNode.AddChildIndex(labelNodeIndex);

                CSharpParser.StatementContext innerStatement = labeledStatement.statement();

                // Treating the labeled statement:
                TreatStatement(innerStatement, labelNode);
            }
            else
            {
                CSharpParser.DeclarationStatementContext declaration = statement.declarationStatement();
                if (declaration != null)
                {
                    CSharpParser.Local_variable_declarationContext localVariable = declaration.local_variable_declaration();
                    if (localVariable != null)
                    {
                        // Getting the local variables modifiers:
                        Symbol.ModifierFlag modFlags = Symbol.ModifierFlag.None;
                        if (localVariable.REF() != null) modFlags |= Symbol.ModifierFlag.Ref;
                        if (localVariable.READONLY() != null) modFlags |= Symbol.ModifierFlag.ReadOnly;

                        // Creating the local variables symbols:
                        IToken typeToken = localVariable.local_variable_type().Start;
                        Type t = symbolTable.FindType(typeToken);
                        if (t == null)
                        {
                            Console.WriteLine("FATAL ERROR: Unidentified type found.");
                            Environment.Exit(1);
                        }
                        MethodSymbol ownerMethod = (MethodSymbol)symbolTable.FindSymbol(parentNode.Token, ast);
                        CSharpParser.Local_variable_declaratorContext[] declarators = 
                            localVariable.local_variable_declarator();
                        foreach (CSharpParser.Local_variable_declaratorContext declarator in declarators)
                        {
                            // Creating the variable symbol:
                            CSharpParser.IdentifierContext identifier = declarator.identifier();
                            IToken variableID = identifier.Start;
                            VariableSymbol variableSymbol = new VariableSymbol(modFlags, ownerMethod);
                            symbolTable.AddSymbol(variableID, variableSymbol);

                            // Creating the variable node and adding it to the AST:
                            Node variableNode = new Node(variableID, Node.Kind.MethodVariableDeclaration, t);
                            ast.AddNode(variableNode);
                            int varDeclIndex = ast.NodeIndex(variableNode);
                            parentNode.AddChildIndex(varDeclIndex);

                            // Treating the variable initialization:
                            TreatVariableInitializer(declarator.local_variable_initializer(), variableNode);
                        }
                    }
                }
                else
                {
                    CSharpParser.Embedded_statementContext embedded = statement.embedded_statement();
                    CSharpParser.Simple_embedded_statementContext simpleStatement = statement.simple_embedded_statement();
                    if (simpleStatement != null)
                    {
                        if (simpleStatement.IF() != null) TreatIfStatement(simpleStatement, parentNode);
                        else if (simpleStatement.SWITCH() != null) TreatSwitchStatement(simpleStatement, parentNode);
                        else if (simpleStatement.DO() != null) TreatDoStatement(simpleStatement, parentNode);
                        else if (simpleStatement.WHILE() != null) TreatWhileStatement(simpleStatement, parentNode);
                        else if (simpleStatement.FOR() != null) TreatForStatement(simpleStatement, parentNode);
                        else if (simpleStatement.FOREACH() != null) TreatForeachStatement(simpleStatement, parentNode);
                        else if (simpleStatement.BREAK() != null) TreatBreakStatement(simpleStatement, parentNode);
                        else if (simpleStatement.CONTINUE() != null) TreatContinueStatement(simpleStatement, parentNode);
                        else if (simpleStatement.GOTO() != null) TreatGoToStatement(simpleStatement, parentNode);
                        else if (simpleStatement.RETURN() != null) TreatReturnStatement(simpleStatement, parentNode);
                    }
                }
            }
        }

        private void TreatVariableInitializer(CSharpParser.Local_variable_initializerContext initializer, Node parentNode)
        {
            Action<CSharpParser.Array_initializerContext, Node> TreatArrayInit = (arrayInit, parent) =>
            {
                CSharpParser.Variable_initializerContext[] varInit = arrayInit.variable_initializer();
                foreach (CSharpParser.Variable_initializerContext varInitializer in varInit)
                {
                    CSharpParser.ExpressionContext expression = varInitializer.expression();
                    if (expression != null)
                    {
                        // Treat expression here
                    }
                    else
                    {
                        CSharpParser.Array_initializerContext a = varInitializer.array_initializer();
                        if (a != null) TreatArrayInit(a, parent);
                    }
                }
            };

            CSharpParser.ExpressionContext expression = initializer.expression();
            if (expression != null)
            {
                // Treat expression here
            }
            else
            {
                CSharpParser.Array_initializerContext array = initializer.array_initializer();
                if (array != null)
                {
                    TreatArrayInit(array, parentNode);
                }
            }
        }

        private void TreatIfStatement(CSharpParser.Simple_embedded_statementContext statement, Node parentNode)
        {
            // Creating node indicating it's an if statement:
            IToken ifToken = statement.IF().Symbol;
            Node ifNode = new Node(ifToken, Node.Kind.IfStatement, null);
            ast.AddNode(ifNode);
            int ifNodeIndex = ast.NodeIndex(ifNode);
            parentNode.AddChildIndex(ifNodeIndex);

            // Creating sub tree for the condition expression:

            // Creating a node and a scope for the if body:
            Node ifBodyNode = new Node(null, Node.Kind.IfStatementBody, null);
            ast.AddNode(ifBodyNode);
            int ifBodyNodeIndex = ast.NodeIndex(ifBodyNode);
            ifNodeIndex.AddChildIndex(ifBodyNodeIndex);
            // The node with the statements will be a child of the if body node
            
            // Checking if there's an else statement:
            
        }

        private void TreatSwitchStatement(CSharpParser.Simple_embedded_statementContext statement, Node parentNode)
        {
            // Creating node indicating it's a switch statement:
            // Creating sub tree for the expression being switched:
            // For each switch section:
            // Creating node for the case label:
            // Creating case label sub node with expression
            // Craeting sub tree and scope for the list of statements:
        }

        private void TreatDoStatement(CSharpParser.Simple_embedded_statementContext statement, Node parentNode)
        {
            // Creating node indicating it's a do statement:
            // Craeting sub tree and scope for the list of statements:
            // Creating sub tree for the condition expression:
        }

        private void TreatWhileStatement(CSharpParser.Simple_embedded_statementContext statement, Node parentNode)
        {
            // Creating node indicating it's a do statement:
            // Creating sub tree for the condition expression:
            // Craeting sub tree and scope for the list of statements:
        }

        private void TreatForStatement(CSharpParser.Simple_embedded_statementContext statement, Node parentNode)
        {
            // Creating node indicating it's a for statement:
            // Craeting sub tree for the initializer expression:
            // Creating sub tree for the condition expression:
            // Creating sub tree for the iterator:
            // Craeting sub tree and scope for the list of statements:
        }

        private void TreatForeachStatement(CSharpParser.Simple_embedded_statementContext statement, Node parentNode)
        {
            // Creating node indicating it's a foreach statement:
            // Creating sub tree for the variable creation in the iterator:
            // Creating sub tree and scope for the list of statements:
        }

        private void TreatBreakStatement(CSharpParser.Simple_embedded_statementContext statement, Node parentNode)
        {
            // Creating node indicating it's a break statement:
        }

        private void TreatContinueStatement(CSharpParser.Simple_embedded_statementContext statement, Node parentNode)
        {
            // Creating node indicating it's a continue statement:
        }

        private void TreatGoToStatement(CSharpParser.Simple_embedded_statementContext statement, Node parentNode)
        {
            // Creating node indicating it's a goto statement:
            // Treating ordinary label:
            // Treating switch case label:
            // Treating switch default label:
        }

        private void TreatReturnStatement(CSharpParser.Simple_embedded_statementContext statement, Node parentNode)
        {
            // Creating node indicating it's a return statement:
            // Creating sub tree for the expression:
        }
    }
}