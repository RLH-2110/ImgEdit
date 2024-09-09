
#include "defines.h"
#include "flags.h"
#include "args.h"
#include "stdio.h"
#include "compatibility.h"

int main(int argc, char* argv[]){

	int i;

	printf("%s version %s %s\n",argv[0],version,OS_STRING);

	getArgs(argc, argv);

	printf("(debug) argument flag variable (hex): %x\n",argumentFlags);

	if (outputFile != NULL)
		printf("(debug) output file: %s\n",outputFile);

	if (inputFiles != NULL){
		fputs("(debug) input files: ",stdout);

		for (i = 0;i < inputFilesC;i++)
			printf("%s ",inputFiles[i]);
		puts(""); /* new line*/
	}

	return 0;
}


