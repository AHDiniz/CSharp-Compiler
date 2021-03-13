# C# Compiler build file

ANTLR := java -jar antlr-4.9.2-complete.jar

main: parser
	dotnet build

parser:
	$(ANTLR) -Dlanguage=CSharp $(wildcard Grammar/*.g4)

project_setup:
	dotnet new console && dotnet add package ANTLR4.Runtime.Standard --version 4.9.2
