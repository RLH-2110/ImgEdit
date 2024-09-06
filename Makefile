
CC=gcc
CFILES= *.c
CHEADERS = *.h

make: $(CFILES) $(CHEADERS)
	$(CC) -ansi -o imgEdit $(CFILES) 
