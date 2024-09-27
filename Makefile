
CC=gcc
CCFLAGS = -ansi -pedantic

OUTPUT = imgEdit
CFILES= $(wildcard *.c) $(wildcard argParse/*.c) $(wildcard comp/fs/*.c) 
CHEADERS = $(wildcard *.h) $(wildcard argParse/*.h) $(wildcard comp/fs/*.h)

TESTER_OUTPUT = test/test.exe
TEST_CFILES := $(CFILES) test/tests.c
TEST_CHEADERS = $(CHEADERS) test/tests.h

# OS FLAGS
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


# rules

$(OUTPUT): $(CFILES) $(CHEADERS)
	$(CC) -o $(OUTPUT) $(CFILES) $(CCFLAGS) 
	
test: $(OUTPUT)
	$(CC) -o $(TESTER_OUTPUT) $(TEST_CFILES) $(CCFLAGS) -D testing
	./$(TESTER_OUTPUT)
	
	
# clean
clear: clean
clean:
	
ifeq ($(OS),Windows_NT)
	rm $(OUTPUT).exe
else
	rm $(OUTPUT)
endif
	rm $(TESTER_OUTPUT)
	rm log.txt out.txt 


	
.PHONY : clean clear test