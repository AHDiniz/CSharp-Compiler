ANTLR := antlr4

CSC := dotnet

GRAMMAR := $(wildcard src/*.g4)

OUT := out/

ANTLR_OPT := -Dlanguage=CSharp

EXE := $(OUT)/csharp

parser:
	$(ANTLR) $(ANTLR_OPT) $(GRAMMAR)
