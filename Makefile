
CC=gcc
CFILES= *.c argParse/*.c comp/fs/*.c 
CHEADERS = *.h argParse/*.h comp/fs/*.h
CCFLAGS = -ansi -pedantic
OUTPUT = imgEdit

TESTER_OUTPUT = test/test.exe
TEST_CFILES= argParse/*.c comp/fs/*.c setup.c test/*.c

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


make: $(CFILES) $(CHEADERS)
	$(CC) -o $(OUTPUT) $(CFILES) $(CCFLAGS) 


.PHONY : clean test
clean:
	
ifeq ($(OS),Windows_NT)
	rm $(OUTPUT).exe
else
	rm $(OUTPUT)
endif

test:
	$(CC) -o $(OUTPUT) $(CFILES) $(CCFLAGS)
	$(CC) -o $(TESTER_OUTPUT) $(TEST_CFILES) $(CCFLAGS) -D testing
	./$(TESTER_OUTPUT)