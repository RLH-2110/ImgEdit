
#include "defines.h"
#include "argParse/flags.h"
#include <string.h>
#include <stdlib.h>
#include <stdio.h>

#include "argParse/args.h"
#include "compat.h"
#include "comp/fs/fs.h"

int main(int argc, char* argv[]){

	char* str;
	int i;

	printf("%s version %s %s\n",argv[0],version,OS_STRING);

	get_args(argc, argv);

	printf("(debug) argument flag variable (hex): %x\n",argumentFlags);

	if (outputFile != NULL)
		printf("(debug) output file: %s\n",outputFile);

	if (inputFiles != NULL){
		fputs("(debug) input files: ",stdout);

		for (i = 0;i < inputFilesC;i++)
			printf("%s ",inputFiles[i]);
		puts(""); /* new line*/
	}

	if (outputFile && inputFiles){ /* if both are not null */
		str = malloc(10); /* must be malloc*/

		for (i = 0;i < inputFilesC && str != NULL;i++){
			printf("Adding %s \n",inputFiles[i]);
			strcat(str,inputFiles[i]);
			strcat(str," ");
		}

		if (str == NULL){
			puts("Main.c OUT OF MEMORY!");
			exit(1);
		}


		printf("(debug) writing %s in %s\n",str,outputFile);
		write_file(outputFile,str,strlen(str));
	}

	return 0;
}


