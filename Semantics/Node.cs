using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace CSharp_Compiler.Semantics
{
    public class Node
    {
        public enum Kind
        {
            None,
            CodeBlock,
            Assignment,
            EqualityComparision,
            MemberVariableDeclaration,
            MethodVariableDeclaration,
            MethodDeclaration,
            ClassDefinition,
            EnumDefinition,
            MainScope,
            ClassBody,
            Identifier,
            Constructor,
            Destructor,
            Label,
            IfStatement,
            IfStatementBody,
            ElseStatement,
            ElseStatementBody,
            SwitchStatement,
            SwitchDefaultLabel,
            SwitchCaseLabel,
            DoStatement,
            ForStatement,
            ForeachStatement,
            IteratorDeclaration,
            BreakStatement,
            ContinueStatement,
            GoToStatement,
            GoToTarget,
            ReturnStatement,
            Argument,
            ExpressionAssignment,
            ExpressionNonAssignment,
            LambdaExpression,
            QueryExpression,
            ConditionalExpression,
            AssignmentOperator,
            CoalescingExpression,
            ConditionalOrExpression,
            ConditionalAndExpression,
            InclusiveOrExpression,
            InclusiveAndExpression,
            AndExpression,
            EqualityExpression,
            RelationalExpression,
            ShiftExpression,
            AdditiveExpression,
            MultiplicativeExpression,
            SwitchExpression,
            SwitchExpressionArms,
            SwitchExpressionArm,
            RangeExpression,
            UnaryExpression,
            PrimaryExpression,
            BracketExpression,
            ExpressionList,
            Expression
        }

        private IToken token;
        private List<int> children;
        private Kind kind = Kind.None;
        private Type type;
        private object data;

        public object Data
        {
            set => this.data = value;
            get => data;
        }

        public IToken Token { get => token; }

        public Type Type { get => type; }

        public Kind NodeKind { get => kind; }

        public Node(IToken token, Kind kind, Type type, object data = null)
        {
            this.token = token;
            this.children = new List<int>();
            this.kind = kind;
            this.type = type;
            this.data = data;
        }

        public void AddChildIndex(int index)
        {
            children.Add(index);
        }
    }
}
