
CC=gcc
CFILES= *.c
CHEADERS = *.h

make: $(CFILES) $(CHEADERS)
	$(CC) -o imgEdit $(CFILES) 
