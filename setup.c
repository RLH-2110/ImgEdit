#include <stdio.h>
#include <stdlib.h>
#include "defines.h"
#include "comp/fs/fs.h"
#include "argParse/flags.h"

FILE *logOut;
FILE *scrOut; 

void setup(){
	logOut = stdout; /* log in the Terminal*/
}

bool set_log_file(){

	FILE *tmp;

	scrOut = stdout;

	if (logFile == NULL){
		fputs("Error: set_log_file: logFile variable must not be NULL!",scrOut);
	}

	if (create_file(logFile, &tmp) == fseNoError){
		logOut = tmp;
		fprintf(logOut,"set log file to: %s\n",logFile);
		return true;
	}else{
		fprintf(logOut,"error setting log file to: %s\n",logFile);
		return false;
	}

}


void error_exit(int status){

	#ifdef testing
	#include "test/tests.h"
	failed++;
	printf("\n#------------------#\nProgamm Terminated due to an error!\nPassed: %d/%d\nFailed: %d/%d\nSkipped: %d/%d\n", passed, NUM_TESTS, failed, NUM_TESTS, skipped, NUM_TESTS);
	#endif

	close_log_file();
	exit(status);
}
