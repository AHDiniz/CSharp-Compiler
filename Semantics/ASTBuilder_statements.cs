using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public partial class ASTBuilder : CSharpParserBaseListener
    {
        public override void EnterBody(CSharpParser.BodyContext context)
        {
            Console.WriteLine("Entering body context.");

            TreatBlock(context.block(), ast.GetNode(symbolTable.CurrentScopeNode));
        }

        private void TreatBlock(CSharpParser.BlockContext context, Node parentNode)
        {
            // Creating a new node for the code block:
            Node blockNode = new Node(null, Node.Kind.CodeBlock, null);
            ast.AddNode(blockNode);
            int blockNodeIndex = ast.NodeIndex(blockNode);
            parentNode.AddChildIndex(blockNodeIndex);

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
                    CSharpParser.Simple_embedded_statementContext simpleStatement = embedded.simple_embedded_statement();
                    if (simpleStatement != null) TreatSimpleEmbeddedStatement(simpleStatement, parentNode);
                    else TreatBlock(embedded.block(), parentNode);
                }
            }
        }

        private void TreatSimpleEmbeddedStatement(CSharpParser.Simple_embedded_statementContext context, Node parentNode)
        {
            if (context is CSharpParser.IfStatementContext)
                TreatIfStatement((CSharpParser.IfStatementContext)context, parentNode);
            else if (context is CSharpParser.SwitchStatementContext)
                TreatSwitchStatement((CSharpParser.SwitchStatementContext)context, parentNode);
            else if (context is CSharpParser.DoStatementContext)
                TreatDoStatement((CSharpParser.DoStatementContext)context, parentNode);
            else if (context is CSharpParser.WhileStatementContext)
                TreatWhileStatement((CSharpParser.WhileStatementContext)context, parentNode);
            else if (context is CSharpParser.ForStatementContext)
                TreatForStatement((CSharpParser.ForStatementContext)context, parentNode);
            else if (context is CSharpParser.ForeachStatementContext)
                TreatForeachStatement((CSharpParser.ForeachStatementContext)context, parentNode);
            else if (context is CSharpParser.BreakStatementContext)
                TreatBreakStatement((CSharpParser.BreakStatementContext)context, parentNode);
            else if (context is CSharpParser.ContinueStatementContext)
                TreatContinueStatement((CSharpParser.ContinueStatementContext)context, parentNode);
            else if (context is CSharpParser.GotoStatementContext)
                TreatGoToStatement((CSharpParser.GotoStatementContext)context, parentNode);
            else if (context is CSharpParser.ReturnStatementContext)
                TreatReturnStatement((CSharpParser.ReturnStatementContext)context, parentNode);
            else if (context is CSharpParser.ExpressionStatementContext)
            {
                CSharpParser.ExpressionStatementContext expr = (CSharpParser.ExpressionStatementContext)context;
                beginTreatExpression(expr.expression(), parentNode);
            }
            else
            {
                Console.WriteLine("FATAL ERROR: Could not define the type of simple_embedded_statement rule.");
                Environment.Exit(1);
            }
        }

        private void TreatVariableInitializer(CSharpParser.Local_variable_initializerContext initializer, Node parentNode)
        {
            Action<CSharpParser.Array_initializerContext, Node> TreatArrayInit = null;
            TreatArrayInit = (arrayInit, parent) =>
            {
                CSharpParser.Variable_initializerContext[] varInit = arrayInit.variable_initializer();
                foreach (CSharpParser.Variable_initializerContext varInitializer in varInit)
                {
                    CSharpParser.ExpressionContext expression = varInitializer.expression();
                    if (expression != null) beginTreatExpression(expression, parentNode);
                    else
                    {
                        CSharpParser.Array_initializerContext a = varInitializer.array_initializer();
                        if (a != null) TreatArrayInit(a, parent);
                    }
                }
            };

            CSharpParser.ExpressionContext expression = initializer.expression();
            if (expression != null) beginTreatExpression(expression, parentNode);
            else
            {
                CSharpParser.Array_initializerContext array = initializer.array_initializer();
                if (array != null)
                {
                    TreatArrayInit(array, parentNode);
                }
            }
        }

        private void TreatIfStatement(CSharpParser.IfStatementContext statement, Node parentNode)
        {
            // Creating node indicating it's an if statement:
            IToken ifToken = statement.IF().Symbol;
            Node ifNode = new Node(ifToken, Node.Kind.IfStatement, null);
            ast.AddNode(ifNode);
            int ifNodeIndex = ast.NodeIndex(ifNode);
            parentNode.AddChildIndex(ifNodeIndex);

            // Creating sub tree for the condition expression:
            beginTreatExpression(statement.expression(), ifNode);

            // Creating a node and a scope for the if body:
            Node ifBodyNode = new Node(null, Node.Kind.IfStatementBody, null);
            ast.AddNode(ifBodyNode);
            int ifBodyNodeIndex = ast.NodeIndex(ifBodyNode);
            ifNode.AddChildIndex(ifBodyNodeIndex);
            // The node with the statements will be a child of the if body node

            CSharpParser.If_bodyContext[] ifBodies = statement.if_body();
            // Treating the if statement body:
            CSharpParser.Simple_embedded_statementContext embedded = ifBodies[0].simple_embedded_statement();
            if (embedded != null) TreatSimpleEmbeddedStatement(embedded, ifBodyNode);
            else TreatBlock(ifBodies[0].block(), ifBodyNode);

            // Checking if there's an else statement:
            if (ifBodies.Length == 2)
            {
                // Creating the else statement node:
                IToken elseToken = statement.ELSE().Symbol;
                Node elseNode = new Node(elseToken, Node.Kind.ElseStatement, null);
                ast.AddNode(elseNode);
                int elseNodeIndex = ast.NodeIndex(elseNode);
                ifNode.AddChildIndex(elseNodeIndex);

                // Creating the else body node:
                Node elseBody = new Node(null, Node.Kind.ElseStatementBody, null);
                ast.AddNode(elseBody);
                int elseBodyIndex = ast.NodeIndex(elseBody);
                elseNode.AddChildIndex(elseBodyIndex);

                // Treating the else statement body:
                CSharpParser.Simple_embedded_statementContext elseEmbedded = ifBodies[1].simple_embedded_statement();
                if (elseEmbedded != null) TreatSimpleEmbeddedStatement(elseEmbedded, elseBody);
                else TreatBlock(ifBodies[1].block(), elseBody);
            }         
        }

        private void TreatSwitchStatement(CSharpParser.SwitchStatementContext statement, Node parentNode)
        {
            // Creating node indicating it's a switch statement:
            IToken switchToken = statement.SWITCH().Symbol;
            Node switchNode = new Node(switchToken, Node.Kind.SwitchStatement, null);
            ast.AddNode(switchNode);
            int switchNodeIndex = ast.NodeIndex(switchNode);
            parentNode.AddChildIndex(switchNodeIndex);

            // Creating sub tree for the expression being switched:
            beginTreatExpression(statement.expression(), switchNode);
            
            // For each switch section:
            CSharpParser.Switch_sectionContext[] sections = statement.switch_section();
            foreach (CSharpParser.Switch_sectionContext section in sections)
            {
                // Creating a node for the switch section:
                Node sectionNode = new Node(null, Node.Kind.SwitchSection, null);
                ast.AddNode(sectionNode);
                int sectionIndex = ast.NodeIndex(sectionNode);
                switchNode.AddChildIndex(sectionIndex);

                // Entering the section scope:
                symbolTable.EnterScope(sectionIndex);

                // Treating the switch label:
                CSharpParser.Switch_labelContext[] labels = section.switch_label();
                foreach (CSharpParser.Switch_labelContext label in labels)
                {
                    // Creating node for the default label:
                    if (label.DEFAULT() != null)
                    {
                        Node defaultNode = new Node(label.DEFAULT().Symbol, Node.Kind.SwitchDefaultLabel, null);
                        ast.AddNode(defaultNode);
                        int defaultIndex = ast.NodeIndex(defaultNode);
                        sectionNode.AddChildIndex(defaultIndex);
                    }
                    else if (label.CASE() != null)
                    {
                        Node caseNode = new Node(label.CASE().Symbol, Node.Kind.SwitchCaseLabel, null);
                        ast.AddNode(caseNode);
                        int caseIndex = ast.NodeIndex(caseNode);
                        sectionNode.AddChildIndex(caseIndex);
                    }
                }

                // Treating the statements list:
                CSharpParser.Statement_listContext statementList = section.statement_list();
                CSharpParser.StatementContext[] statements = statementList.statement();
                foreach (CSharpParser.StatementContext s in statements)
                {
                    TreatStatement(s, sectionNode);
                }

                // Exiting the section scope:
                symbolTable.ExitScope();
            }
        }

        private void TreatDoStatement(CSharpParser.DoStatementContext statement, Node parentNode)
        {
            // Creating node indicating it's a do statement:
            IToken doToken = statement.DO().Symbol;
            Node doNode = new Node(doToken, Node.Kind.DoStatement, null);
            ast.AddNode(doNode);
            int doIndex = ast.NodeIndex(doNode);
            parentNode.AddChildIndex(doIndex);

            // Craeting sub tree and scope for the list of statements:
            symbolTable.EnterScope(doIndex);
            CSharpParser.Embedded_statementContext embedded = statement.embedded_statement();
            CSharpParser.Simple_embedded_statementContext simpleStatement = embedded.simple_embedded_statement();
            if (simpleStatement != null) TreatSimpleEmbeddedStatement(simpleStatement, doNode);
            else TreatBlock(embedded.block(), doNode);
            
            // Creating sub tree for the condition expression:
            beginTreatExpression(statement.expression(), doNode);
            
            symbolTable.ExitScope();
        }

        private void TreatWhileStatement(CSharpParser.WhileStatementContext statement, Node parentNode)
        {
            // Creating node indicating it's a do statement:
            IToken whileToken = statement.WHILE().Symbol;
            Node whileNode = new Node(whileToken, Node.Kind.DoStatement, null);
            ast.AddNode(whileNode);
            int whileIndex = ast.NodeIndex(whileNode);
            parentNode.AddChildIndex(whileIndex);

            symbolTable.EnterScope(whileIndex);

            // Creating sub tree for the condition expression:
            beginTreatExpression(statement.expression(), whileNode);

            // Craeting sub tree and scope for the list of statements:
            CSharpParser.Embedded_statementContext embedded = statement.embedded_statement();
            CSharpParser.Simple_embedded_statementContext simpleStatement = embedded.simple_embedded_statement();
            if (simpleStatement != null) TreatSimpleEmbeddedStatement(simpleStatement, whileNode);
            else TreatBlock(embedded.block(), whileNode);

            symbolTable.ExitScope();
        }

        private void TreatForStatement(CSharpParser.ForStatementContext statement, Node parentNode)
        {
            // Creating node indicating it's a for statement:
            IToken forToken = statement.FOR().Symbol;
            Node forNode = new Node(forToken, Node.Kind.ForStatement, null);
            ast.AddNode(forNode);
            int forIndex = ast.NodeIndex(forNode);
            parentNode.AddChildIndex(forIndex);

            symbolTable.EnterScope(forIndex);

            // Craeting sub tree for the initializer expression:
            CSharpParser.For_initializerContext initContext = statement.for_initializer();
            if (initContext != null)
            {
                CSharpParser.Local_variable_declarationContext declContext = initContext.local_variable_declaration();
                if (declContext != null)
                {
                    // Getting the local variables modifiers:
                    Symbol.ModifierFlag modFlags = Symbol.ModifierFlag.None;
                    if (declContext.REF() != null) modFlags |= Symbol.ModifierFlag.Ref;
                    if (declContext.READONLY() != null) modFlags |= Symbol.ModifierFlag.ReadOnly;

                    // Creating the local variables symbols:
                    IToken typeToken = declContext.local_variable_type().Start;
                    Type t = symbolTable.FindType(typeToken);
                    if (t == null)
                    {
                        Console.WriteLine("FATAL ERROR: Unidentified type found.");
                        Environment.Exit(1);
                    }
                    MethodSymbol ownerMethod = (MethodSymbol)symbolTable.FindSymbol(parentNode.Token, ast);
                    CSharpParser.Local_variable_declaratorContext[] declarators = 
                        declContext.local_variable_declarator();
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
                else
                {
                    CSharpParser.ExpressionContext[] initExpressions = initContext.expression();
                    foreach(CSharpParser.ExpressionContext expression in initExpressions)
                        beginTreatExpression(expression, forNode);
                }
            }
            
            // Creating sub tree for the condition expression:
            beginTreatExpression(statement.expression(), forNode);

            // Creating sub tree for the iterator:
            CSharpParser.For_iteratorContext iteratorContext = statement.for_iterator();
            if (iteratorContext != null)
            {
                CSharpParser.ExpressionContext[] expressions = iteratorContext.expression();
                foreach (CSharpParser.ExpressionContext expression in expressions)
                    beginTreatExpression(expression, forNode);
            }
            
            // Craeting sub tree and scope for the list of statements:
            CSharpParser.Embedded_statementContext embedded = statement.embedded_statement();
            CSharpParser.Simple_embedded_statementContext simpleStatement = embedded.simple_embedded_statement();
            if (simpleStatement != null) TreatSimpleEmbeddedStatement(simpleStatement, forNode);
            else TreatBlock(embedded.block(), forNode);

            symbolTable.ExitScope();
        }

        private void TreatForeachStatement(CSharpParser.ForeachStatementContext statement, Node parentNode)
        {
            // Creating node indicating it's a foreach statement:
            IToken foreachToken = statement.FOREACH().Symbol;
            Node foreachNode = new Node(foreachToken, Node.Kind.ForeachStatement, null);
            ast.AddNode(foreachNode);
            int foreachIndex = ast.NodeIndex(foreachNode);
            parentNode.AddChildIndex(foreachIndex);

            symbolTable.EnterScope(foreachIndex);

            // Creating sub tree for the variable creation in the iterator:
            Type t = symbolTable.FindType(statement.local_variable_type().type_().Stop);
            IToken idToken = statement.identifier().Start;
            Symbol iteratorSymbol = new Symbol(Symbol.ModifierFlag.None);
            symbolTable.AddSymbol(idToken, iteratorSymbol);
            Node iteratorNode = new Node(idToken, Node.Kind.IteratorDeclaration, t, iteratorSymbol);
            ast.AddNode(iteratorNode);
            int iteratorIndex = ast.NodeIndex(iteratorNode);
            foreachNode.AddChildIndex(iteratorIndex);

            // Treat the expression that returns the iterable
            beginTreatExpression(statement.expression(), foreachNode);

            // Creating sub tree and scope for the list of statements:
            CSharpParser.Embedded_statementContext embedded = statement.embedded_statement();
            CSharpParser.Simple_embedded_statementContext simpleStatement = embedded.simple_embedded_statement();
            if (simpleStatement != null) TreatSimpleEmbeddedStatement(simpleStatement, foreachNode);
            else TreatBlock(embedded.block(), foreachNode);

            symbolTable.ExitScope();
        }

        private void TreatBreakStatement(CSharpParser.BreakStatementContext statement, Node parentNode)
        {
            // Creating node indicating it's a break statement:
            IToken breakToken = statement.BREAK().Symbol;
            Node breakNode = new Node(breakToken, Node.Kind.BreakStatement, null);
            ast.AddNode(breakNode);
            int breakIndex = ast.NodeIndex(breakNode);
            parentNode.AddChildIndex(breakIndex);
        }

        private void TreatContinueStatement(CSharpParser.ContinueStatementContext statement, Node parentNode)
        {
            // Creating node indicating it's a continue statement:
            IToken continueToken = statement.CONTINUE().Symbol;
            Node continueNode = new Node(continueToken, Node.Kind.ContinueStatement, null);
            ast.AddNode(continueNode);
            int continueIndex = ast.NodeIndex(continueNode);
            parentNode.AddChildIndex(continueIndex);
        }

        private void TreatGoToStatement(CSharpParser.GotoStatementContext statement, Node parentNode)
        {
            // Creating node indicating it's a goto statement:
            IToken gotoToken = statement.GOTO().Symbol;
            Node gotoNode = new Node(gotoToken, Node.Kind.GoToStatement, null);
            ast.AddNode(gotoNode);
            int gotoIndex = ast.NodeIndex(gotoNode);
            parentNode.AddChildIndex(gotoIndex);

            // Treating ordinary label:
            if (statement.identifier() != null)
            {
                IToken labelToken = statement.identifier().Start;
                Node labelNode = ast.GetNode(labelToken, Node.Kind.Label);
                if (labelNode == null)
                {
                    Console.WriteLine("FATAL ERROR: Undeclared label being used in goto statement.");
                    Environment.Exit(1);
                }
                Node targetNode = new Node(labelToken, Node.Kind.GoToTarget, null);
                ast.AddNode(targetNode);
                int targetIndex = ast.NodeIndex(targetNode);
                gotoNode.AddChildIndex(targetIndex);
            }
            else
            {
                // Treating switch case label:
                if (statement.CASE() != null)
                {
                    IToken caseToken = statement.CASE().Symbol;
                    Node targetNode = new Node(caseToken, Node.Kind.GoToTarget, null);
                    ast.AddNode(targetNode);
                    int targetIndex = ast.NodeIndex(targetNode);
                    gotoNode.AddChildIndex(targetIndex);

                    // Treat condition expression:
                    beginTreatExpression(statement.expression(), gotoNode);
                }
                else
                {
                    // Treating switch default label:
                    IToken defaultToken = statement.DEFAULT().Symbol;
                    Node targetNode = new Node(defaultToken, Node.Kind.GoToTarget, null);
                    ast.AddNode(targetNode);
                    int targetIndex = ast.NodeIndex(targetNode);
                    gotoNode.AddChildIndex(targetIndex);
                }
            }
        }

        private void TreatReturnStatement(CSharpParser.ReturnStatementContext statement, Node parentNode)
        {
            // Creating node indicating it's a return statement:
            IToken returnToken = statement.RETURN().Symbol;
            Node returnNode = new Node(returnToken, Node.Kind.ReturnStatement, null);
            ast.AddNode(returnNode);
            int returnIndex = ast.NodeIndex(returnNode);
            parentNode.AddChildIndex(returnIndex);

            // Creating sub tree for the expression:
            beginTreatExpression(statement.expression(), returnNode);
        }
    }
}