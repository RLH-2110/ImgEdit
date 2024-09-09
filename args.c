#include "stdio.h"
#include "stdlib.h"
#include "defines.h"
#include "flags.h"

void print_help(char* argv0); /* prints help screen */
void fetch_flagArgs(int argc, char*argv[], int firstIndex); /* counts how many parameters there are after a flag */

void getArgs(int argc, char*argv[]){
	
	int argI = 1; /* argument index, starts at first argument, ignores filename */
	int i;
	int result;

	for (;argI + 1 <= argc; argI++){ /* argc is a count, argI is an index thats why we need to increment argI */

		if (argv[argI][0] != '-')
			continue;

		switch (argv[argI][1]){


			case 'o':

				argumentFlags += flags_o;

				if (fetch_flagArgsCount(argc,argv,argI+1) != 1){
					puts("Error: flag -o only takes one argument!");
					print_help(argv[0]);
					exit(1);
				}

				outputFile = argv[argI+1];
				break;

			case 'i':

				argumentFlags += flags_i;

				result; result = fetch_flagArgsCount(argc,argv,argI+1);

				if (result < 1){
					puts("Error: flag -i needs at least one argument!");
					print_help(argv[0]);
					exit(1);
				}

				for (i = 1;i <= result; i++){ /* starts at 1, becuase argI + 0 == -i */
					appendArg(argv[argI + i],&inputFiles,&inputFilesC);
				}
				break;

			case 'h':
				argumentFlags += flags_h;

				print_help(argv[0]);
				break;

			default:
				printf("Error: unrecognized parameter: %s\n",argv[argI]);
				print_help(argv[0]);
				exit(1);
				
		}
	}
}

/* counts how many parameters there are after a flag */
int fetch_flagArgsCount(int argc, char*argv[], int firstIndex){
	
	int paramCount = 0;
	int i = firstIndex;


	if (firstIndex >= argc)	
		return 0;
	



	for (;i < argc && argv[i][0] != '-';i++)
		paramCount++;

	return paramCount;
}

void print_help(char* argv0){
	printf("Usage: %s [-o <output>] [-h] [-i <inputs>...]\n",argv0);
	puts("  -o <output>: <output> specifies the name of the output file");
	puts("  -i <inputs>: <inputs> specifies the input files, can be one or multiple");
	puts("  -h: shows this help screen");
}
