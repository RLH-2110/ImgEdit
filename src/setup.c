#include <stdio.h>
#include <stdlib.h>
#include "defines.h"
#include "comp/fs/src/fs.h"
#include "argParse/flags.h"
#include <errno.h>

FILE *logOut;
FILE *scrOut; 

void setup(void){
	logOut = stdout; /* log in the Terminal*/
	scrOut = stdout;
}

bool set_log_file(void){

	FILE *tmp;

	if (logFile == NULL){
		fputs("Error: set_log_file: logFile variable must not be NULL!",scrOut);
	}

	if (open_file(logFile,"w", &tmp) == fseNoError){
		logOut = tmp;
		fprintf(logOut,"set log file to: %s\n",logFile);
		return true;
	}else{
		fprintf(logOut,"error setting log file to: %s\n",logFile);
		return false;
	}

}

/* Closes the log file, if it exists. */
fsError close_log_file(void){
	
	if (logOut != stdout){

		fputs("closing log file...",logOut);

		errno = 0;
		if (fclose(logOut) != 0){
			fprintf(logOut,"close_log_file error! errno: %d\nLOG FILE COULD NOT BE CLOSED!\n",errno);
			return fseNoClose;
		}

		logOut = stdout;
	}

	return fseNoError;
}


void error_exit(int status){

	#ifdef testing
	#include "../test/tests.h"
	failed++;
	printf("\n#------------------#\nProgamm Terminated due to an error!\nPassed: %d/%d\nFailed: %d/%d\nSkipped: %d/%d\n", passed, NUM_TESTS, failed, NUM_TESTS, skipped, NUM_TESTS);
	#endif

	close_log_file();
	compFS_close_log_file();
	exit(status);
}
