#include <stdio.h>
#include "../../defines.h"
#include <errno.h>
#include "fs.h" /* to get the isDirectory definition*/

fsError write_file(const char* filePath, const char* buffer, size_t bufferSize){
	
	FILE *file;
	int result;

	if (isDirectory(filePath)){
		puts("(debug) write_file error. File is a directory!");
		return fseIsDirectory;
	}

	errno = 0;
	file = fopen(filePath,"w");

	if (file == NULL){
		printf("(debug) write_file open error. errno: %d\n",errno);
		return fseNoOpen;
	}

	errno = 0;
	if (fwrite(buffer,1,bufferSize,file) != bufferSize){
		printf("(debug) write_file write error. errno: %d\n",errno);
		return fseWrongWrite;
	}

	errno = 0;
	if (fclose(file) != 0){
		printf("(debug) write_file close error. errno: %d\n",errno);
		return fseNoClose;
	}

	return fseNoError;
}

