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
            
        }

        public override void ExitQuery_body_clause(CSharpParser.Query_body_clauseContext context)
        {
            Console.WriteLine("Exiting query_body_clause context.");
            
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
