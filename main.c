
#include "defines.h"
#include "argParse/flags.h"
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "argParse/args.h"
#include "compat.h"
#include "comp/fs/fs.h"
#include "comp/string/str.h"

FILE *logOut;
void setLogFile();


int main(int argc, char* argv[]){
	char* str;
	int i;

	logOut = stdout; /* log in the Terminal*/

	get_args(argc, argv);

	if (logFile != NULL)
		setLogFile();


	fprintf(logOut,"%s version %sR%c %s\n",argv[0],VERSION,GRAPHICS_CHR,OS_STRING);

	fprintf(logOut,"len(this is sparta !): %d\n",strlen("this is sparta ! ! !"));
	

	fprintf(logOut,"(debug) argument flag variable (hex): %x\n",argumentFlags);

	if (outputFile != NULL)
		fprintf(logOut,"(debug) output file: %s\n",outputFile);

	if (inputFiles != NULL){
		fputs("(debug) input files: ",logOut);

		for (i = 0;i < inputFilesC;i++)
			fprintf(logOut,"%s ",inputFiles[i]);
		fputs("\n",logOut); /* new line*/
	}



	if (outputFile && inputFiles){ /* if both are not null */
		str = malloc(10); /* must be malloc*/
		for(i = 0;i < 10;i++)
			str[i] = 0;

		for (i = 0;i < inputFilesC && str != NULL;i++){
			fprintf(logOut,"(debug) Adding %s \n",inputFiles[i]);
			strcat_c(str,inputFiles[i]);
			strcat_c(str," ");
		}

		if (str == NULL){
			fputs("Main.c OUT OF MEMORY!",logOut);
			errorExit(1);
		}


		fprintf(logOut,"(debug) writing %s in %s\n",str,outputFile);
		write_file(outputFile,str,strlen(str));
	}


	close_log_file();
	return 0;
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