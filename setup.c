#include <stdio.h>
#include <stdlib.h>
#include "defines.h"
#include "comp/fs/fs.h"
#include "argParse/flags.h"

FILE *logOut;

void setup(){
	char* str;
	int i;

	logOut = stdout; /* log in the Terminal*/
}

void set_log_file(){

	FILE *tmp;

	if (create_file(logFile, &tmp) == fseNoError){
		logOut = tmp;
		fprintf(logOut,"set log file to: %s\n",logFile);
	}else{
		fprintf(logOut,"error setting log file to: %s\n",logFile);
	}
	
}


void error_exit(int status){

	#ifdef testing
	#include "test/tests.h"
	failed++;
	printf("\n#------------------#\nProgamm Terminated due to an error!\nPassed: %d/%d\nFailed: %d/%d\n",passed,NUM_TESTS,failed,NUM_TESTS);
	#endif

	close_log_file();
	exit(status);
}
