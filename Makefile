# C# compiler Makefile project

# Build variables:

VENDOR := vendor

ANTLR := java -jar $(VENDOR)/ANTLR/antlr-4.9.2-complete.jar
ANTLR_FLAGS := -Dlanguage=Cpp
GRAMMAR := $(wildcard src/Parser/*.g4)

CC := g++
CFLAGS := -Llib/ -lantlr4runtime

OUT := bin/
LIB := lib/
EXE := $(OUT)csharp

SRC := $(wildcard src/Parser/*.cpp) $(wildcard src/*.cpp)
INC := -Isrc/Parser -Iinclude -Iinclude/Parser
PCH := include/pch.h

# Makefile settings:

ifeq ($(debug),)
	debug := true
endif

ifeq ($(debug), true)
	CFLAGS += -Wall -g
endif

ifeq ($(debug), false)
	CFLAGS += -w -O2
endif

# Build rules:

main: pch parser
	$(CC) $(INC) $(CFLAGS) $(SRC) -o $(EXE) 

pch:
	$(CC) $(PCH)

parser:
	$(ANTLR) $(ANTLR_FLAGS) $(GRAMMAR)

clean:
	rm $(wildcard $(OUT)*)

project_setup:
	mkdir $(OUT) && mkdir $(LIB)
