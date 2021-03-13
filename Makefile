ANTLR := java -jar antlr-4.9.2-complete.jar

GRAMMAR := $(wildcard Parser/*.g4)

OUT := out/

ANTLR_OPT := -Dlanguage=CSharp

EXE := $(OUT)csharp

main:
	dotnet build

parser:
	$(ANTLR) $(ANTLR_OPT) $(GRAMMAR)

clean:
	rm $(wildcard Parser/*.cs) $(wildcard Parser/*.interp) $(wildcard Parser/*.tokens)

project_setup:
	dotnet new console && dotnet add package ANTLR4.Runtime.Standard --version 4.7.2
