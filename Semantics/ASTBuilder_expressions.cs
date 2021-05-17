using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public partial class ASTBuilder : CSharpParserBaseListener
    {
        //Precedencia de tipos:
        /* Primary > Unary > Range > switch  > with  > Multiplicative > Additive
         > Shift > Relational and type-testing > Equality >  Boolean logical AND or bitwise logical AND
        > Boolean logical XOR or bitwise logical XOR > Boolean logical OR or bitwise logical OR
        > Conditional AND > Conditional OR > Null-coalescing operator > Conditional operator > Assignment and lambda declaration
        > query
         * */
        private void beginTreatExpression(CSharpParser.ExpressionContext context, Node parentNode)
        {
            //comeca aqui
            // Getting the Parent Node:
            Node ParentNode = parentNode;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de child node

            if (!context.IsEmpty)
            {
                childTreat = treatExpression(context);
                ParentNode.AddChildIndex(childTreat.ChildNodeIndex);
            }
        }

        private (int ChildNodeIndex, Type tipo) treatExpression(CSharpParser.ExpressionContext context)
        {
            Console.WriteLine("Entering expression context.");
            Node.Kind expressionKind = Node.Kind.Expression;
            IToken token = null;
            Type expressionType = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de child node
            
            if (context.assignment() != null)
            {
                childTreat = treatExpressionAssignment(context.assignment());
                expressionType = childTreat.tipo;
            }
            else //non_assignment_expression | REF non_assignment_expression;
            {
                childTreat = treatExpressionNonAssignment(context.non_assignment_expression());
                //tipo em non_assignment_expression
                expressionType = childTreat.tipo;
                if (context.REF() != null)
                {
                    token = context.REF().Symbol;
                }
            }
            // Creating the expression node and adding it to the AST:
            Node expressionNode = new Node(token, expressionKind, expressionType);
            ast.AddNode(expressionNode);
            //add child 
            expressionNode.AddChildIndex(childTreat.ChildNodeIndex);
            return (ast.NodeIndex(expressionNode), expressionType);
        }

        private (int ChildNodeIndex, Type tipo) treatExpressionAssignment(CSharpParser.AssignmentContext context)
        {
            Console.WriteLine("Entering assignment context.");
            Node.Kind currentKind = Node.Kind.ExpressionAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child

            if (!context.unary_expression().IsEmpty && !context.assignment_operator().IsEmpty && !context.expression().IsEmpty)
            {
                //expression determina o tipo
                childTreat = treatExpression(context.expression());
                currentType = childTreat.tipo;
                //criando node
                currentNode =  new Node(token, currentKind, currentType);
                ast.AddNode(currentNode);
                //adicionando ao subnode
                currentNode.AddChildIndex(childTreat.ChildNodeIndex);//expression
                childTreat = treatUnaryExpression(context.unary_expression());
                currentNode.AddChildIndex(childTreat.ChildNodeIndex);//unary 
                childTreat = treatAssignmentOperator(context.assignment_operator());
                currentNode.AddChildIndex(childTreat.ChildNodeIndex);//assign op
                
            }
            else //unary_expression '??=' throwable_expression;
            {
                childTreat = treatUnaryExpression(context.unary_expression());
                currentType = childTreat.tipo;
                //criando node
                token = context.OP_COALESCING_ASSIGNMENT().Symbol;
                currentNode = new Node(token, currentKind, currentType);
                ast.AddNode(currentNode);
                //adicionando child
                currentNode.AddChildIndex(childTreat.ChildNodeIndex);//unary 
                //childTreat = treatthrowable_expression(context.unary_expression());
                //currentNode.AddChildIndex(childTreat.ChildNodeIndex);
            }
            return (ast.NodeIndex(currentNode), currentType);
        }
        
        private (int ChildNodeIndex, Type tipo) treatExpressionNonAssignment(CSharpParser.Non_assignment_expressionContext context)
        {
            Console.WriteLine("Entering non_assignment_expression context.");
            Node.Kind currentKind = Node.Kind.ExpressionNonAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
            if (context.lambda_expression() != null)
            {
                //childTreat = treatLambdaExpression(context.lambda_expression());
                //currentType = childTreat.tipo;
                currentNode = new Node(token, currentKind, currentType);
                ast.AddNode(currentNode);
                //adicionando child
                //currentNode.AddChildIndex(childTreat.ChildNodeIndex);
            }
            else if (context.query_expression() != null)
            {
                /*
                childTreat = treatQueryExpression(context.query_expression());
                currentType = childTreat.tipo;
                currentNode = new Node(token, currentKind, currentType);
                ast.AddNode(currentNode);
                //adicionando child
                currentNode.AddChildIndex(childTreat.ChildNodeIndex);
                */
            }
            else //conditional_expression
            {
                childTreat = treatConditionalExpression(context.conditional_expression());
                currentType = childTreat.tipo;
                currentNode = new Node(token, currentKind, currentType);
                ast.AddNode(currentNode);
                //adicionando child
                currentNode.AddChildIndex(childTreat.ChildNodeIndex);
            }
            return (ast.NodeIndex(currentNode), currentType);
        }

       /* private (int ChildNodeIndex, Type tipo) treatLambdaExpression(CSharpParser.Lambda_expressionContext context)
        {
            
            Console.WriteLine("Entering lambda_expression context.");
            Node.Kind currentKind = Node.Kind.LambdaExpression;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
            //ASYNC? anonymous_function_signature right_arrow anonymous_function_body;
            //prioridade async e anonymous function
            childTreat = treatAnonymousFunctionSignature(context.anonymous_function_signature());
            currentType = childTreat.tipo;
            if (context.ASYNC().Symbol != null)
            {
                token = context.ASYNC().Symbol;
                currentType = new Type(token);
            }
            


            return (ast.NodeIndex(currentNode), currentType);
        }
            
        private (int ChildNodeIndex, Type tipo) treatAnonymousFunctionSignature(CSharpParser.Anonymous_function_signatureContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatExplicitAnonymousFunctionParameterList(CSharpParser.Explicit_anonymous_function_parameter_listContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatExplicitAnonymousFunctionParameter(CSharpParser.Explicit_anonymous_function_parameterContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatImplicitAnonymousFunctionParameterList(CSharpParser.Implicit_anonymous_function_parameter_listContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatImplicitAnonymousFunctionParameter(CSharpParser.Implicit_anonymous_function_parameter_listContext context)
        {

        }

        
        private (int ChildNodeIndex, Type tipo) treatQueryExpression(CSharpParser.Query_expressionContext context)
        {
            Console.WriteLine("Entering query_expression context.");
            Node.Kind currentKind = Node.Kind.QueryExpression;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
            //tipo vem do from clause
            childTreat = treatFromClause(context.from_clause());
            //criando node
            currentType = TreatTypeContext(context.from_clause().type_());
            currentNode = new Node(token, currentKind, currentType);
            ast.AddNode(currentNode);
            //adicionando child
            currentNode.AddChildIndex(childTreat.ChildNodeIndex);
            childTreat = treatQueryBody(context.query_body());
            currentNode.AddChildIndex(childTreat.ChildNodeIndex);
            return (ast.NodeIndex(currentNode), currentType);

        }

        private (int ChildNodeIndex, Type tipo) treatFromClause(CSharpParser.From_clauseContext context)
        {
            Console.WriteLine("Entering from_clause context.");
            Node.Kind currentKind = Node.Kind.Type;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
        }

        private (int ChildNodeIndex, Type tipo) treatQueryBody(CSharpParser.Query_bodyContext context)
        {

        }
        */
        
        private (int ChildNodeIndex, Type tipo) treatConditionalExpression(CSharpParser.Conditional_expressionContext context)
        {
            Console.WriteLine("Entering conditional_expression context.");
            Node.Kind currentKind = Node.Kind.ConditionalExpression;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child

            childTreat = treatCoalescingExpression(context.null_coalescing_expression());
            currentType = childTreat.tipo;
            currentNode = new Node(token, currentKind, currentType);
            ast.AddNode(currentNode);
            //adicionando child
            currentNode.AddChildIndex(childTreat.ChildNodeIndex);
            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatAssignmentOperator(CSharpParser.Assignment_operatorContext context)
        {
            Console.WriteLine("Entering conditional_expression context.");
            Node.Kind currentKind = Node.Kind.ExpressionAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child

            if (context.right_shift_assignment() != null)
            {
                childTreat = treatRightShiftAssignment(context.right_shift_assignment());
                currentType = childTreat.tipo;
                currentNode = new Node(token, currentKind, currentType);
                ast.AddNode(currentNode);
                //adicionando child
                currentNode.AddChildIndex(childTreat.ChildNodeIndex);
            }
            else
            {
                if (context.OP_ADD_ASSIGNMENT().Symbol != null)
                {
                    token = context.OP_ADD_ASSIGNMENT().Symbol;

                }
                else if (context.OP_AND_ASSIGNMENT().Symbol != null)
                {
                    token = context.OP_AND_ASSIGNMENT().Symbol;
                }
                else if (context.OP_DIV_ASSIGNMENT().Symbol != null)
                {
                    token = context.OP_DIV_ASSIGNMENT().Symbol;
                }
                else if (context.OP_LEFT_SHIFT_ASSIGNMENT().Symbol != null)
                {
                    token = context.OP_LEFT_SHIFT_ASSIGNMENT().Symbol;
                }
                else if (context.OP_MOD_ASSIGNMENT().Symbol != null)
                {
                    token = context.OP_MOD_ASSIGNMENT().Symbol;
                }
                else if (context.OP_MULT_ASSIGNMENT().Symbol != null)
                {
                    token = context.OP_MULT_ASSIGNMENT().Symbol;
                }
                else if (context.OP_OR_ASSIGNMENT().Symbol != null)
                {
                    token = context.OP_OR_ASSIGNMENT().Symbol;
                }
                else if (context.OP_SUB_ASSIGNMENT().Symbol != null)
                {
                    token = context.OP_SUB_ASSIGNMENT().Symbol;
                }
                else if (context.OP_XOR_ASSIGNMENT().Symbol != null)
                {
                    token = context.OP_XOR_ASSIGNMENT().Symbol;
                }
                else
                {
                    token = context.ASSIGNMENT().Symbol;
                }
                currentType = new Type(token);
                currentNode = new Node(token, currentKind, currentType);
                ast.AddNode(currentNode);
            }
            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatRightShiftAssignment(CSharpParser.Right_shift_assignmentContext context)
        {
            Console.WriteLine("Entering right_shift_assignment context.");
            Node.Kind currentKind = Node.Kind.ShiftExpression;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
            //
            // fazer
            //

            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatCoalescingExpression(CSharpParser.Null_coalescing_expressionContext context)
        {
            Console.WriteLine("Entering null_coalescing_expression context.");
            Node.Kind currentKind = Node.Kind.CoalescingExpression;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child

            if (context.OP_COALESCING() != null)
            {
                token = context.OP_COALESCING().Symbol;
                if (context.null_coalescing_expression() != null)
                {
                    childTreat = treatCoalescingExpression(context.null_coalescing_expression());
                    if (childTreat.tipo != null)
                    {
                        currentType = childTreat.tipo;
                    }
                    currentNode = new Node(token, currentKind, currentType);
                    ast.AddNode(currentNode);
                    currentNode.AddChildIndex(childTreat.ChildNodeIndex);
                    childTreat = treatConditionalOrExpression(context.conditional_or_expression());
                    currentNode.AddChildIndex(childTreat.ChildNodeIndex);
                }
            }
            else
            {
                childTreat = treatConditionalOrExpression(context.conditional_or_expression());
                currentType = childTreat.tipo;
                currentNode = new Node(token, currentKind, currentType);
                ast.AddNode(currentNode);
                currentNode.AddChildIndex(childTreat.ChildNodeIndex);
            }
           return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatConditionalOrExpression(CSharpParser.Conditional_or_expressionContext context)
        {
            Console.WriteLine("Entering conditional_or_expression context.");
            Node.Kind currentKind = Node.Kind.ConditionalOrExpression;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
            List<int> childIndex = new List<int>();
            if (context.OP_OR() != null)
            {
                if (context.OP_OR().Length > 0 && context.OP_OR()[0].Symbol != null)
                {
                    //mais de um
                    token = context.OP_OR()[0].Symbol;
                    foreach (CSharpParser.Conditional_and_expressionContext i in context.conditional_and_expression())
                    {
                        if (!i.IsEmpty)
                        {
                            childTreat = treatConditionalAndExpression(i);
                            currentType = childTreat.tipo;
                            childIndex.Add(childTreat.ChildNodeIndex);
                        }
                    }
                    currentNode = new Node(token, currentKind, currentType);
                    ast.AddNode(currentNode);
                    foreach (int i in childIndex)
                    {
                        childIndex.Add(i);
                    }
                }
            }
            else
            {
                childTreat = treatConditionalAndExpression(context.conditional_and_expression()[0]);
                currentNode = ast.GetNode(childTreat.ChildNodeIndex);
                currentType = childTreat.tipo;
            }
            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatConditionalAndExpression(CSharpParser.Conditional_and_expressionContext context)
        {
            Console.WriteLine("Entering conditional_and_expression context.");
            Node.Kind currentKind = Node.Kind.ConditionalAndExpression;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
            List<int> childIndex = new List<int>();
            if (context.OP_AND() != null)
            {
                if (context.OP_AND()[0].Symbol != null)
                {
                    //mais de um
                    token = context.OP_AND()[0].Symbol;
                    foreach (CSharpParser.Inclusive_or_expressionContext i in context.inclusive_or_expression())
                    {
                        if (!i.IsEmpty)
                        {
                            childTreat = treatInclusiveOrExpression(i);
                            currentType = childTreat.tipo;
                            childIndex.Add(childTreat.ChildNodeIndex);
                        }
                    }
                    currentNode = new Node(token, currentKind, currentType);
                    ast.AddNode(currentNode);
                    foreach (int i in childIndex)
                    {
                        childIndex.Add(i);
                    }
                }
            }
            else
            {
                childTreat = treatInclusiveOrExpression(context.inclusive_or_expression()[0]);
                currentNode = ast.GetNode(childTreat.ChildNodeIndex);
                currentType = childTreat.tipo;
            }
            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatInclusiveOrExpression(CSharpParser.Inclusive_or_expressionContext context)
        {
            Console.WriteLine("Entering inclusive_or_expression context.");
            Node.Kind currentKind = Node.Kind.InclusiveOrExpression;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
            List<int> childIndex = new List<int>();
            if (context.BITWISE_OR() != null)
            {
                if (context.BITWISE_OR()[0].Symbol != null)
                {
                    //mais de um
                    token = context.BITWISE_OR()[0].Symbol;
                    foreach (CSharpParser.Exclusive_or_expressionContext i in context.exclusive_or_expression())
                    {
                        if (!i.IsEmpty)
                        {
                            childTreat = treatExclusiveOrExpression(i);
                            currentType = childTreat.tipo;
                            childIndex.Add(childTreat.ChildNodeIndex);
                        }
                    }
                    currentNode = new Node(token, currentKind, currentType);
                    ast.AddNode(currentNode);
                    foreach (int i in childIndex)
                    {
                        childIndex.Add(i);
                    }
                }
            }
            else
            {
                childTreat = treatExclusiveOrExpression(context.exclusive_or_expression()[0]);
                currentNode = ast.GetNode(childTreat.ChildNodeIndex);
                currentType = childTreat.tipo;
            }
            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatExclusiveOrExpression(CSharpParser.Exclusive_or_expressionContext context)
        {
            Console.WriteLine("Entering exclusive_or_expression context.");
            Node.Kind currentKind = Node.Kind.ExclusiveOrExpression;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
            List<int> childIndex = new List<int>();
            if (context.CARET() != null)
            {
                if (context.CARET()[0].Symbol != null)
                {
                    //mais de um
                    token = context.CARET()[0].Symbol;
                    foreach (CSharpParser.And_expressionContext i in context.and_expression())
                    {
                        if (!i.IsEmpty)
                        {
                            childTreat = treatAndExpression(i);
                            currentType = childTreat.tipo;
                            childIndex.Add(childTreat.ChildNodeIndex);
                        }
                    }
                    currentNode = new Node(token, currentKind, currentType);
                    ast.AddNode(currentNode);
                    foreach (int i in childIndex)
                    {
                        childIndex.Add(i);
                    }
                }
            }
            else
            {
                childTreat = treatAndExpression(context.and_expression()[0]);
                currentNode = ast.GetNode(childTreat.ChildNodeIndex);
                currentType = childTreat.tipo;
            }
            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatAndExpression(CSharpParser.And_expressionContext context)
        {
            Console.WriteLine("Entering and_expression context.");
            Node.Kind currentKind = Node.Kind.AndExpression;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
            List<int> childIndex = new List<int>();
            if (context.AMP() != null)
            {
                if (context.AMP()[0].Symbol != null)
                {
                    //mais de um
                    token = context.AMP()[0].Symbol;
                    foreach (CSharpParser.Equality_expressionContext i in context.equality_expression())
                    {
                        if (!i.IsEmpty)
                        {
                            childTreat = treatEqualityExpression(i);
                            currentType = childTreat.tipo;
                            childIndex.Add(childTreat.ChildNodeIndex);
                        }
                    }
                    currentNode = new Node(token, currentKind, currentType);
                    ast.AddNode(currentNode);
                    foreach (int i in childIndex)
                    {
                        childIndex.Add(i);
                    }
                }
            }
            else
            {
                childTreat = treatEqualityExpression(context.equality_expression()[0]);
                currentNode = ast.GetNode(childTreat.ChildNodeIndex);
                currentType = childTreat.tipo;
            }
            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatEqualityExpression(CSharpParser.Equality_expressionContext context)
        {
            Console.WriteLine("Entering equality_expression context.");
            Node.Kind currentKind = Node.Kind.EqualityComparision;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
            List<int> childIndex = new List<int>();

            if (context.OP_EQ() != null || context.OP_NE() != null)
            {
                if (context.OP_EQ()[0].Symbol!=null || context.OP_NE()[0].Symbol != null)
                {
                    //mais de um
                    if (context.OP_EQ()[0].Symbol != null) token = context.OP_EQ()[0].Symbol;
                    else token = context.OP_NE()[0].Symbol;
                    foreach (CSharpParser.Relational_expressionContext i in context.relational_expression())
                    {
                        if (!i.IsEmpty)
                        {
                            childTreat = treatRelationalExpression(i);
                            currentType = childTreat.tipo;
                            childIndex.Add(childTreat.ChildNodeIndex);
                        }
                    }
                    currentNode = new Node(token, currentKind, currentType);
                    ast.AddNode(currentNode);
                    foreach (int i in childIndex)
                    {
                        childIndex.Add(i);
                    }

                }
            }

            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatRelationalExpression(CSharpParser.Relational_expressionContext context)
        {
            Console.WriteLine("Entering relational_expression context.");
            Node.Kind currentKind = Node.Kind.RelationalExpression;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
            List<int> childIndex = new List<int>();
            if (context.shift_expression().Length > 1)
            {
                foreach(CSharpParser.Shift_expressionContext i in context.shift_expression())
                {
                    childTreat = treatShiftExpression(i);
                    childIndex.Add(childTreat.ChildNodeIndex);
                }
                foreach (CSharpParser.IsTypeContext i in context.isType()) 
                {
                    //childTreat=treatIsType(i);
                    //childIndex.Add(childTreat.ChildNodeIndex);
                }
            }


            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatShiftExpression(CSharpParser.Shift_expressionContext context)
        {
            Console.WriteLine("Entering conditional_expression context.");
            Node.Kind currentKind = Node.Kind.ExpressionAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;


            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatAdditiveExpression(CSharpParser.Additive_expressionContext context)
        {
            Console.WriteLine("Entering conditional_expression context.");
            Node.Kind currentKind = Node.Kind.ExpressionAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;


            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatMultiplicativeExpression(CSharpParser.Multiplicative_expressionContext context)
        {
            Console.WriteLine("Entering conditional_expression context.");
            Node.Kind currentKind = Node.Kind.ExpressionAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;


            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatSwitchExpression(CSharpParser.Switch_expressionContext context)
        {
            Console.WriteLine("Entering conditional_expression context.");
            Node.Kind currentKind = Node.Kind.ExpressionAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;


            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatSwitchExpressionArms(CSharpParser.Switch_expression_armsContext context)
        {
            Console.WriteLine("Entering conditional_expression context.");
            Node.Kind currentKind = Node.Kind.ExpressionAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;


            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatSwitchExpressionArm(CSharpParser.Switch_expression_armContext context)
        {
            Console.WriteLine("Entering conditional_expression context.");
            Node.Kind currentKind = Node.Kind.ExpressionAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;


            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatRangeExpression(CSharpParser.Range_expressionContext context)
        {
            Console.WriteLine("Entering conditional_expression context.");
            Node.Kind currentKind = Node.Kind.ExpressionAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;


            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatUnaryExpression(CSharpParser.Unary_expressionContext context)
        {
            Console.WriteLine("Entering conditional_expression context.");
            Node.Kind currentKind = Node.Kind.ExpressionAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;


            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatPrimaryExpression(CSharpParser.Primary_expressionContext context)
        {
            Console.WriteLine("Entering conditional_expression context.");
            Node.Kind currentKind = Node.Kind.ExpressionAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;


            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatBracketExpression(CSharpParser.Bracket_expressionContext context)
        {
            Console.WriteLine("Entering conditional_expression context.");
            Node.Kind currentKind = Node.Kind.ExpressionAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;


            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatExpressionList(CSharpParser.Expression_listContext context)
        {
            Console.WriteLine("Entering conditional_expression context.");
            Node.Kind currentKind = Node.Kind.ExpressionAssignment;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;


            return (ast.NodeIndex(currentNode), currentType);
        }


    }
}