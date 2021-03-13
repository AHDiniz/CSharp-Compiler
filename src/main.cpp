#include "pch.h"
#include "antlr4-runtime.h"
#include "CSharpLexer.h"
#include "CSharpParser.h"
#include "CSharpBaseListener.h"
#include "TreeShapeListener.h"

int main(int argc, char *argv[])
{
    std::ifstream stream;
    stream.open(argv[1]);
    antlr4::ANTLRInputStream input(stream);
    CSharpLexer lexer(&input);
    antlr4::CommonTokenStream tokens(&lexer);
    CSharpParser parser(&tokens);

    antlr4::tree::ParseTree *tree = parser.key();
    TreeShapeListener listener;
    antlr4::tree::ParserTreeWalker::DEFAULT.walk(&listener, tree);

    return EXIT_SUCCESS;
}
