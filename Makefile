
CC=gcc
CFILES= *.c
CHEADERS = *.h
CCFLAGS = 

$(info    OS is $(OS))

ifeq ($(OS),Windows_NT)
    CCFLAGS += -D OS_WINDOWS

$(warning    ADD OTHER OS CHECKS!)

make: $(CFILES) $(CHEADERS)
	$(CC) -ansi -o imgEdit $(CFILES) 
