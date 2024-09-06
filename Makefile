
CC=gcc
CFILES= *.c
CHEADERS = *.h

make: $(CFILES) $(CHEADERS)
	$(CC) -std=c90 -o imgEdit $(CFILES) 
