ANTLR := antlr4

GRAMMAR := $(wildcard src/*.g4)

OUT := out/

ANTLR_OPT := -Dlanguage=CSharp

EXE := $(OUT)csharp

SOURCES := $(wildcard src/*.cs)

main:
	dotnet build

parser:
	$(ANTLR) $(ANTLR_OPT) $(GRAMMAR)
