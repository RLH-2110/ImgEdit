#include <stdio.h>
#include "../../defines.h"
#include <errno.h>
#include "fs.h" /* to get the isDirectory definition*/

fsError write_file(const char* filePath, const char* buffer, size_t bufferSize){
	
	FILE *file;
	int result;

	if (isDirectory(filePath)){
		fputs("(debug) write_file error. File is a directory!\n",logOut);
		return fseIsDirectory;
	}

	errno = 0;
	file = fopen(filePath,"w");

	if (file == NULL){
		fprintf(logOut,"(debug) write_file open error. errno: %d\n",errno);
		return fseNoOpen;
	}

	errno = 0;
	if (fwrite(buffer,1,bufferSize,file) != bufferSize){
		fprintf(logOut,"(debug) write_file write error. errno: %d\n",errno);
		return fseWrongWrite;
	}

	errno = 0;
	if (fclose(file) != 0){
		fprintf(logOut,"(debug) write_file close error. errno: %d\n",errno);
		return fseNoClose;
	}

	return fseNoError;
}

/* creates a file and checks for write access */
fsError create_file(const char* filePath, FILE **out_file){
	
	int result;

	if (isDirectory(filePath)){
		fputs("(debug) create_file error. File is a directory!\n",logOut);
		return fseIsDirectory;
	}

	/* Create file and probe */

	errno = 0;
	*out_file = fopen(filePath,"w");

	if (*out_file == NULL){
		fprintf(logOut,"(debug) create_file open error. errno: %d\n",errno);
		return fseNoOpen;
	}

	errno = 0;
	if (fwrite("\r\n",1,2,*out_file) != 2){ /* write a newline into the file, to see if we have write access*/
		fprintf(logOut,"(debug) create_file write access error. errno: %d\n",errno);
		return fseWrongWrite;
	}

	/* Close file and reopen it, so the probed thing is gone */

	errno = 0;
	if (fclose(*out_file) != 0){
		fprintf(logOut,"(debug) create_file close error. errno: %d\n",errno);
		return fseNoClose;
	}

	errno = 0;
	*out_file = fopen(filePath,"w");

	if (*out_file == NULL){
		fprintf(logOut,"(debug) create_file second open error. errno: %d\n",errno);
		return fseNoOpen;
	}

	return fseNoError;
}

/* Closes the log file, if it exists. */
fsError close_log_file(){
	
	if (logOut != stdout){

		fputs("closing log file...",logOut);

		errno = 0;
		if (fclose(logOut) != 0){
			fprintf(logOut,"close_log_file error! errno: %d\nLOG FILE COULD NOT BE CLOSED!\n",errno);
			return fseNoClose;
		}
	}

	return fseNoError;
}


