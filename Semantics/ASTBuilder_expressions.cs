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
        private void beginTreatExpression(CSharpParser.ExpressionContext context)
        {
            //comeca aqui
            // Getting the Parent Node:
            Node ParentNode = ast.GetNode(symbolTable.CurrentScopeNode);
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
            
            if (!context.assignment().IsEmpty)
            {
                childTreat = treatExpressionAssignment(context.assignment());
                expressionType = childTreat.tipo;
            }
            else //non_assignment_expression | REF non_assignment_expression;
            {
                childTreat = treatExpressionNonAssignment(context.non_assignment_expression());
                expressionType = childTreat.tipo;
                if (context.REF() != null)
                {
                    token = context.REF().Symbol;
                    expressionType = new Type(token);
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
                //unary tem prioridade de tipo
                //subnode para agrupar node dos tres
                childTreat = treatUnaryExpression(context.unary_expression());
                currentType = childTreat.tipo;
                //criando node
                currentNode =  new Node(token, currentKind, currentType);
                ast.AddNode(currentNode);
                //adicionando ao subnode
                currentNode.AddChildIndex(childTreat.ChildNodeIndex);//unary 
                childTreat = treatAssignmentOperator(context.assignment_operator());
                currentNode.AddChildIndex(childTreat.ChildNodeIndex);//assign op
                childTreat = treatExpression(context.expression());
                currentNode.AddChildIndex(childTreat.ChildNodeIndex); // expression
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
            if (!context.lambda_expression().IsEmpty)
            {
                childTreat = treatLambdaExpression(context.lambda_expression());
                currentType = childTreat.tipo;
                currentNode = new Node(token, currentKind, currentType);
                ast.AddNode(currentNode);
                //adicionando child
                currentNode.AddChildIndex(childTreat.ChildNodeIndex);
            }
            else if (!context.query_expression().IsEmpty)
            {
                childTreat = treatQueryExpression(context.query_expression());
                currentType = childTreat.tipo;
                currentNode = new Node(token, currentKind, currentType);
                ast.AddNode(currentNode);
                //adicionando child
                currentNode.AddChildIndex(childTreat.ChildNodeIndex);
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

        private (int ChildNodeIndex, Type tipo) treatLambdaExpression(CSharpParser.Lambda_expressionContext context)
        {
            
            Console.WriteLine("Entering lambda_expression context.");
            Node.Kind currentKind = Node.Kind.LambdaExpression;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
            //ASYNC? anonymous_function_signature right_arrow anonymous_function_body;
            //prioridade async e anonymous function
            if(context.ASYNC().Symbol != null)
            {
                token = context.ASYNC().Symbol;
                currentType = new Type(token);
            }
            //criacao dos nós filhos 
            //
            //      A FAZER
            //
            return (ast.NodeIndex(currentNode), currentType);
        }

        private (int ChildNodeIndex, Type tipo) treatQueryExpression(CSharpParser.Query_expressionContext context)
        {
            Console.WriteLine("Entering query_expression context.");
            Node.Kind currentKind = Node.Kind.LambdaExpression;
            IToken token = null;
            Type currentType = null;
            Node currentNode = null;
            (int ChildNodeIndex, Type tipo) childTreat; //tipo e index de cada child
        }

        private (int ChildNodeIndex, Type tipo) treatConditionalExpression(CSharpParser.Conditional_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatAssignmentOperator(CSharpParser.Assignment_operatorContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatCoalescingExpression(CSharpParser.Null_coalescing_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatConditionalOrExpression(CSharpParser.Conditional_or_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatConditionalAndExpression(CSharpParser.Conditional_and_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatInclusiveOrExpression(CSharpParser.Inclusive_or_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatAndExpression(CSharpParser.And_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatEqualityExpression(CSharpParser.Equality_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatRelationalExpression(CSharpParser.Relational_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatShiftExpression(CSharpParser.Shift_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatAdditiveExpression(CSharpParser.Additive_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatMultiplicativeExpression(CSharpParser.Multiplicative_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatSwitchExpression(CSharpParser.Switch_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatSwitchExpressionArms(CSharpParser.Switch_expression_armsContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatSwitchExpressionArm(CSharpParser.Switch_expression_armContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatRangeExpression(CSharpParser.Range_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatUnaryExpression(CSharpParser.Unary_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatPrimaryExpression(CSharpParser.Primary_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatBracketExpression(CSharpParser.Bracket_expressionContext context)
        {

        }

        private (int ChildNodeIndex, Type tipo) treatExpressionList(CSharpParser.Expression_listContext context)
        {

        }


    }
}