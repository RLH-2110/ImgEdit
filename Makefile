
CC=gcc
CFILES= *.c
CHEADERS = *.h
CCFLAGS = -pedantic 



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
	$(CC) -ansi -o imgEdit $(CFILES) $(CCFLAGS) 
