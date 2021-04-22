using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public partial class ASTBuilder : CSharpParserBaseListener
    {
        public override void EnterCompilation_unit(CSharpParser.Compilation_unitContext context)
        {
            
            Console.WriteLine("Entering compilation_unit context.");
        }

        public override void ExitCompilation_unit(CSharpParser.Compilation_unitContext context)
        {
            
            Console.WriteLine("Exiting compilation_unit context.");
        }

        public override void EnterNamespace_or_type_name(CSharpParser.Namespace_or_type_nameContext context)
        {
            
            Console.WriteLine("Entering namespace_or_type_name context.");
        }

        public override void ExitNamespace_or_type_name(CSharpParser.Namespace_or_type_nameContext context)
        {
            
            Console.WriteLine("Exiting namespace_or_type_name context.");
        }

        public override void EnterType_(CSharpParser.Type_Context context)
        {
            Console.WriteLine("Entering type_ context.");

            // Getting the current modifiers:
            Symbol.ModifierFlag mods = TreatModTokens();
            modifiersTokens.Clear();
            
            // Defining the type that's being read:
            currentType = TreatTypeContext(context);
        }

        public override void ExitType_(CSharpParser.Type_Context context)
        {
            
            Console.WriteLine("Exiting type_ context.");
        }

        public override void EnterTuple_element(CSharpParser.Tuple_elementContext context)
        {
            
            Console.WriteLine("Entering tuple_element context.");
        }

        public override void ExitTuple_element(CSharpParser.Tuple_elementContext context)
        {
            
            Console.WriteLine("Exiting tuple_element context.");
        }

        public override void EnterClass_type(CSharpParser.Class_typeContext context)
        {
            
            Console.WriteLine("Entering class_type context.");
        }

        public override void ExitClass_type(CSharpParser.Class_typeContext context)
        {
            
            Console.WriteLine("Exiting class_type context.");
        }

        public override void EnterArgument_list(CSharpParser.Argument_listContext context)
        {

            Console.WriteLine("Entering argument_list context.");
        }

        public override void ExitArgument_list(CSharpParser.Argument_listContext context)
        {
            
            Console.WriteLine("Exiting argument_list context.");
        }

        public override void EnterArgument(CSharpParser.ArgumentContext context)
        {
            Node currentScopeNode = ast.GetNode(symbolTable.CurrentScopeNode);

            IToken argumentToken = context.Start;

            if(symbolTable.FindSymbol(argumentToken, ast) != null){
                RuleContext parentContext = context;
                while((parentContext = parentContext.Parent) != null){
                    if(parentContext.GetType() == typeof(CSharpParser.Method_invocationContext)){
                        break;
                    }
                }
                if(parentContext == null){
                    throw new Exception("Undefined method (WTF!!)");
                }

                //TODO: Need get method definition to check argument types 
                IToken methodDefinitionToken = ((CSharpParser.Method_declarationContext) parentContext).Start;
                Node methodDeclarationNode = ast.GetNode(methodDefinitionToken, Node.Kind.MethodVariableDeclaration);

                if(methodDeclarationNode == null){
                    throw new Exception("Undefined method symbol");
                }

                //calcule argument position
                int argumentPosition = 0;
                RuleContext parentContext_1 = context;
                while((parentContext_1 = parentContext_1.Parent).GetType() == typeof(CSharpParser.ArgumentContext)){
                    argumentPosition++;
                }
                
                Node ArgumentNode = new Node(argumentToken, Node.Kind.Argument, null);
                ast.AddNode(ArgumentNode);
                currentScopeNode.AddChildIndex(ast.NodeIndex(ArgumentNode));
                
            }else{
                throw new Exception("Undefined symbol");
            }
            
            Console.WriteLine("Entering argument context.");
        }

        public override void ExitArgument(CSharpParser.ArgumentContext context)
        {
            
            Console.WriteLine("Exiting argument context.");
        }

        public override void EnterPredefined_type(CSharpParser.Predefined_typeContext context)
        {
            
            Console.WriteLine("Entering predefined_type context.");
        }

        public override void ExitPredefined_type(CSharpParser.Predefined_typeContext context)
        {
            
            Console.WriteLine("Exiting predefined_type context.");
        }

        public override void EnterUnbound_type_name(CSharpParser.Unbound_type_nameContext context)
        {
            
            Console.WriteLine("Entering unbound_type_name context.");
        }

        public override void ExitUnbound_type_name(CSharpParser.Unbound_type_nameContext context)
        {
            
            Console.WriteLine("Exiting unbound_type_name context.");
        }

        public override void EnterGeneric_dimension_specifier(CSharpParser.Generic_dimension_specifierContext context)
        {
            
            Console.WriteLine("Entering generic_dimension_specifier context.");
        }

        public override void ExitGeneric_dimension_specifier(CSharpParser.Generic_dimension_specifierContext context)
        {
            
            Console.WriteLine("Exiting generic_dimension_specifier context.");
        }

        public override void EnterIsType(CSharpParser.IsTypeContext context)
        {
            
            Console.WriteLine("Entering isType context.");
        }

        public override void ExitIsType(CSharpParser.IsTypeContext context)
        {
            
            Console.WriteLine("Exiting isType context.");
        }

        public override void EnterIsTypePatternArm(CSharpParser.IsTypePatternArmContext context)
        {
            
            Console.WriteLine("Entering isTypePatternArm context.");
        }

        public override void ExitIsTypePatternArm(CSharpParser.IsTypePatternArmContext context)
        {
            
            Console.WriteLine("Exiting isTypePatternArm context.");
        }

        public override void EnterIsTypePatternArms(CSharpParser.IsTypePatternArmsContext context)
        {
            
            Console.WriteLine("Entering isTypePatternArms context.");
        }

        public override void ExitIsTypePatternArms(CSharpParser.IsTypePatternArmsContext context)
        {
            
            Console.WriteLine("Exiting isTypePatternArms context.");
        }
    }
}