ANTLR := antlr4

CSC := dotnet

GRAMMAR := $(wilcard src/Grammar/*.g4)

PARSEROUT := src/Parser/

OUT := out/

ANTLR_OPT := -Dlanguage=CSharp -o $(PARSEROUT) -lib $(OUT)

EXE := $(OUT)/csharp

parser:
	$(ANTLR) $(ANTLR_OPT) $(GRAMMAR)
