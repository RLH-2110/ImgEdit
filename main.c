
#include "defines.h"
#include "argParse/flags.h"
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include "argParse/args.h"
#include "compat.h"
#include "comp/fs/fs.h"
#include "comp/string/str.h"

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


	/* THE CODE HERE DOES NOT FULLY WORK ON MS-DOS YET! PLEASE DEBUG!*/
	if (outputFile && inputFiles){ /* if both are not null */
		str = malloc(10); /* must be malloc*/
		for(i = 0;i < 10;i++)
			str[i] = 0;

		for (i = 0;i < inputFilesC && str != NULL;i++){
			printf("Adding %s \n",inputFiles[i]);
			strcat_c(&str,inputFiles[i]);
			strcat_c(&str," ");
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


