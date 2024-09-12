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

void setLogFile(){

	FILE *tmp;

	if (create_file(logFile, &tmp) == fseNoError){
		logOut = tmp;
		fprintf(logOut,"set log file to: %s\n",logFile);
	}else{
		fprintf(logOut,"error setting log file to: %s\n",logFile);
	}
	
}


void errorExit(int status){
	close_log_file();
	exit(status);
}