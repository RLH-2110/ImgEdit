#include <stdio.h>
#include "defines.h"
#include <errno.h>

fsError write_file(char* filePath, char* buffer, size_t bufferSize){
	
	FILE *file;
	int result;

	errno = 0;
	file = fopen(filePath,"w");

	if (file == NULL){
		printf("(debug) write_file open error. errno: %d",errno);
		return fseNoOpen;
	}

	errno = 0;
	if (fwrite(buffer,1,bufferSize,file) != bufferSize){
		printf("(debug) write_file write error. errno: %d",errno);
		return fseWrongWrite;
	}

	errno = 0;
	if (fclose(file) != 0){
		printf("(debug) write_file close error. errno: %d",errno);
		return fseNoClose;
	}

	return fseNoError;
}

