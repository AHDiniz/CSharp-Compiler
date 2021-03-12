ANTLR := antlr4

GRAMMAR := $(wildcard Parser/*.g4)

OUT := out/

ANTLR_OPT := -Dlanguage=CSharp

EXE := $(OUT)csharp

main:
	dotnet build

parser:
	$(ANTLR) $(ANTLR_OPT) $(GRAMMAR)
