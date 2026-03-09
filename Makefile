
CC=gcc
CCFLAGS = -ansi -pedantic

OUTPUT = imgEdit
CFILES= $(wildcard src/*.c) $(wildcard src/argParse/*.c) $(wildcard config/*.c)
CHEADERS = $(wildcard src/*.h) $(wildcard src/argParse/*.h) $(wildcard config/*.h)

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

$(OUTPUT): $(CFILES) $(CHEADERS) src/comp/fs/libcmpfs.a
	$(CC) -o $(OUTPUT) $(CFILES) $(CCFLAGS) -Lsrc/comp/fs -lcmpFS

src/comp/fs/libcmpfs.a: 
	$(MAKE) -C src/comp/fs

	
test: $(OUTPUT)
	$(CC) -o $(TESTER_OUTPUT) $(TEST_CFILES) $(CCFLAGS) -D testing -Lsrc/comp/fs -lcmpFS
	./$(TESTER_OUTPUT)
	
	
# clean
clear: clean
clean:
	
	rm -f "$(OUTPUT)"
	rm -f "$(TESTER_OUTPUT)"
	rm -f "log.txt" "out.txt" "src.txt"


	
.PHONY : clean clear test
