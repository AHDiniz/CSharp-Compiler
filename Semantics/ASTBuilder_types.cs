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
            CSharpParser.Base_typeContext baseTypeCtx = context.base_type();
            
            CSharpParser.Simple_typeContext simpleType = baseTypeCtx.simple_type();
            if (simpleType != null)
            {
                IToken typeToken = baseTypeCtx.Start;
                TypeTag typeTag = TreatSimpleTypeToken(typeToken.Text);
                currentType = new BuiltInType(typeToken, typeTag);
            }
            else
            {
                CSharpParser.Class_typeContext classType = baseTypeCtx.class_type();
                if (classType != null)
                {
                    CSharpParser.Namespace_or_type_nameContext typeName = classType.namespace_or_type_name();
                    if (typeName != null)
                    {
                        IToken typeIDtoken = typeName.Start;
                        ClassSymbol typeSymbol = (ClassSymbol)(symbolTable.FindSymbol(typeIDToken));
                        if (typeSymbol != null)
                        {
                            ClassTag tag = typeSymbol.Tag;
                            currentType = new ClassType(typeIDToken, tag, typeSymbol);
                        } // Otherwise, needs to do another pass to fix symbol table
                    }
                    else
                    {
                        IToken typeToken = classType.Start;
                        TypeTag typeTag = TreatSimpleTypeToken(typeToken.Text);
                        currentType = new BuiltInType(typeToken, typeTag);
                    }
                }
                else
                {
                    CSharpParser.Tuple_typeContext tupleType = baseTypeCtx.tuple_type();
                    if (tupleType != null)
                    {
                        CSharpParser.Tuple_elementContext[] tupleElems = tupleType.tuple_element();
                        foreach (CSharpParser.Tuple_elementContext tupleElem in tupleElems)
                        {
                            
                        }
                    }
                    else
                    {
                        currentType = new BuiltInType(baseTypeCtx.VOID().Symbol, TypeTag.Void, true);
                    }
                }
            }
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