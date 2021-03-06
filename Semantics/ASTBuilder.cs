using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CSharp_Compiler.Semantics
{
    public partial class ASTBuilder : CSharpParserBaseListener
    {
        private Stack<ParserRuleContext> contextStack;
        private AST ast;
        private SymbolTable symbolTable;
        private List<IToken> modifiersTokens; // For the modifiers of the current class/method/struct/interface
        private Type currentType; // Auxiliar variable to detect the type in member and variable declarations

        public AST AbstractSyntaxTree { get => ast; }

        public SymbolTable SymbolTable { get => symbolTable; }

        public ASTBuilder(AST ast, SymbolTable symbolTable) : base()
        {
            contextStack = new Stack<ParserRuleContext>();
            modifiersTokens = new List<IToken>();
            this.ast = ast;
            this.symbolTable = symbolTable;

            Node initialNode = new Node(null, Node.Kind.MainScope, null);
            this.ast.AddNode(initialNode);

            this.symbolTable.EnterScope(ast.NodeIndex(initialNode));
        }

        private Symbol.ModifierFlag TreatModTokens()
        {
            Symbol.ModifierFlag flags = Symbol.ModifierFlag.None;

            foreach (IToken modToken in modifiersTokens)
            {
                switch (modToken.Text)
                {
                    case "new":
                        flags |= Symbol.ModifierFlag.New;
                        break;
                    case "public":
                        flags |= Symbol.ModifierFlag.Public;
                        break;
                    case "protected":
                        flags |= Symbol.ModifierFlag.Protected;
                        break;
                    case "readonly":
                        flags |= Symbol.ModifierFlag.ReadOnly;
                        break;
                    case "volatile":
                        flags |= Symbol.ModifierFlag.Volatile;
                        break;
                    case "virtual":
                        flags |= Symbol.ModifierFlag.Virtual;
                        break;
                    case "sealed":
                        flags |= Symbol.ModifierFlag.Sealed;
                        break;
                    case "override":
                        flags |= Symbol.ModifierFlag.Override;
                        break;
                    case "abstract":
                        flags |= Symbol.ModifierFlag.Abstract;
                        break;
                    case "static":
                        flags |= Symbol.ModifierFlag.Static;
                        break;
                    case "unsafe":
                        flags |= Symbol.ModifierFlag.Unsafe;
                        break;
                    case "extern":
                        flags |= Symbol.ModifierFlag.Extern;
                        break;
                    case "partial":
                        flags |= Symbol.ModifierFlag.Partial;
                        break;
                    case "async":
                        flags |= Symbol.ModifierFlag.Async;
                        break;
                }
            }

            return flags;
        }

        private TypeTag TreatSimpleTypeToken(string typeStr)
        {
            switch (typeStr)
            {
                case "bool":
                    return TypeTag.Bool;
                case "decimal":
                    return TypeTag.Decimal;
                case "float":
                    return TypeTag.Float;
                case "double":
                    return TypeTag.Double;
                case "sbyte":
                    return TypeTag.SByte;
                case "byte":
                    return TypeTag.Byte;
                case "short":
                    return TypeTag.Short;
                case "ushort":
                    return TypeTag.UShort;
                case "int":
                    return TypeTag.Int;
                case "uint":
                    return TypeTag.UInt;
                case "long":
                    return TypeTag.Long;
                case "ulong":
                    return TypeTag.ULong;
                case "char":
                    return TypeTag.Char;
                case "object":
                    return TypeTag.Object;
                case "dynamic":
                    return TypeTag.Dynamic;
                case "string":
                    return TypeTag.String;
                default:
                    return TypeTag.Void;
            }
        }

        private Type TreatTypeContext(CSharpParser.Type_Context context)
        {
            // Getting the current modifiers:
            Symbol.ModifierFlag mods = TreatModTokens();
            modifiersTokens.Clear();
            
            // Defining the type that's being read:
            CSharpParser.Base_typeContext baseTypeCtx = context.base_type();
            
            CSharpParser.Simple_typeContext simpleType = baseTypeCtx.simple_type();
            if (simpleType != null)
            {
                IToken typeToken = baseTypeCtx.Start;
                Type t = symbolTable.FindType(typeToken);
                if (t != null) return t;

                TypeTag typeTag = TreatSimpleTypeToken(typeToken.Text);
                BuiltInType builtInType = new BuiltInType(typeToken, typeTag);
                symbolTable.AddType(t);
                return builtInType;
            }
            else
            {
                CSharpParser.Class_typeContext classTypeContext = baseTypeCtx.class_type();
                if (classTypeContext != null)
                {
                    CSharpParser.Namespace_or_type_nameContext typeName = classTypeContext.namespace_or_type_name();
                    if (typeName != null)
                    {
                        IToken typeIDToken = typeName.Start;
                        ClassType t = (ClassType)symbolTable.FindType(typeIDToken);
                        if (t != null) return t;
                        else
                        {
                            Console.WriteLine("FATAL ERROR: Unidentified type found.");
                            Environment.Exit(1);
                        }
                    }
                    else
                    {
                        IToken typeToken = classTypeContext.Start;
                        TypeTag typeTag = TreatSimpleTypeToken(typeToken.Text);
                        BuiltInType builtInType = new BuiltInType(typeToken, typeTag);
                        symbolTable.AddType(builtInType);
                        return builtInType;
                    }
                }
                else
                {
                    CSharpParser.Tuple_typeContext tupleType = baseTypeCtx.tuple_type();
                    if (tupleType != null)
                    {
                        CSharpParser.Tuple_elementContext[] tupleElems = tupleType.tuple_element();
                        List<Type> subTypes = new List<Type>();
                        foreach (CSharpParser.Tuple_elementContext tupleElem in tupleElems)
                        {
                            CSharpParser.Type_Context typeContext = tupleElem.type_();
                            subTypes.Add(TreatTypeContext(typeContext));
                        }
                        TupleType t = new TupleType(null, subTypes.ToArray()); 
                        symbolTable.AddType(t);
                        return t;
                    }
                    else
                    {
                        BuiltInType voidType = new BuiltInType(baseTypeCtx.VOID().Symbol, TypeTag.Void, true);
                        symbolTable.AddType(voidType);
                        return voidType;
                    }
                }
            }

            return null;
        }
    }
}