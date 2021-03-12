ANTLR := antlr4

CSC := dotnet

GRAMMAR := $(wildcard src/*.g4)

OUT := out/

ANTLR_OPT := -Dlanguage=CSharp

EXE := $(OUT)csharp

SOURCES := $(wildcard src/*.cs)

main:
	$(CSC) $(SOURCES) -o $(EXE)

parser:
	$(ANTLR) $(ANTLR_OPT) $(GRAMMAR)
