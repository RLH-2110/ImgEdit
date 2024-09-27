
CC=gcc
CFILES= $(wildcard *.c) $(wildcard argParse/*.c) $(wildcard comp/fs/*.c) 
CHEADERS = $(wildcard *.h) $(wildcard argParse/*.h) $(wildcard comp/fs/*.h)
CCFLAGS = -ansi -pedantic
OUTPUT = imgEdit

TEST_CHEADERS = $(wildcard *.h) $(wildcard argParse/*.h) $(wildcard comp/fs/*.h) test/tests.h
TESTER_OUTPUT = test/test.exe
TEST_CFILES= $(wildcard argParse/*.c) $(wildcard comp/fs/*.c) $(wildcard test/*.c) str.c setup.c

ifeq ($(OS),Windows_NT)
    CCFLAGS += -D OS_WINDOWS
$(info    detected windows)
else
	UNAME_S := $(shell uname -s)

	ifeq ($(UNAME_S),Linux)
$(info    detected Linux)
	CCFLAGS += -D OS_LINUX
	endif

	ifeq ($(UNAME_S),Haiku)
$(info    detected Haiku)
    CCFLAGS += -D OS_HAIKU
	endif

endif


$(OUTPUT): $(CFILES) $(CHEADERS)
	$(CC) -o $(OUTPUT) $(CFILES) $(CCFLAGS) 

.PHONY : clean test
clean:
	
ifeq ($(OS),Windows_NT)
	rm $(OUTPUT).exe
else
	rm $(OUTPUT)
endif
	rm $(TESTER_OUTPUT)
	rm log.txt out.txt 

test:
	$(CC) -o $(OUTPUT) $(CFILES) $(CCFLAGS)
	$(CC) -o $(TESTER_OUTPUT) $(TEST_CFILES) $(CCFLAGS) -D testing
	./$(TESTER_OUTPUT)
