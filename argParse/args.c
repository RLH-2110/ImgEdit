#include "stdio.h"
#include "stdlib.h"
#include "../defines.h"
#include "flags.h"


int fetch_flag_arg_count(int argc, char*argv[], int firstIndex); /* counts how many parameters there are after a flag */
void print_help(char* argv0); /* prints help screen */

/* arg = pointer to the raw text of the argument, THIS POINER MUST STAY ALIVE FOR THE ENTIRE RUNTIME!
   list = a pointer to a char** like inputFiles
   listC = pointer to the count of how many elements are in the list */
void append_arg(char *arg, char ***list, int *listC){
	int i;

	if (!arg || !list){ /* if one param is NULL */
		fputs("ERROR: append_arg does not take NULL!\n",logOut);
		error_exit(1);
	} 



	if (*list == NULL){

		*list = malloc(sizeof(char*));
		if (*list == NULL){
			fputs("ERROR: append_arg is out of memory!\n",logOut);
			error_exit(1);
		}

		*listC = 1;
		*list[0] = arg; /* set the first element in the list to the argument pointer */	
	}else{
		*listC = *listC + 1;
		if (*listC == 0){
			/* If we got here, then that means there are so many parameters that we had an overflow. That sucks.*/
			fputs("ERROR: too many arguments for a flag! We had an overflow, what the fuck did you do to cause this?\n",logOut);
			error_exit(1);
		}

		*list = realloc(*list,*listC * sizeof(char*)); /* make space for one more pointer */


		if (*list == NULL){
			fputs("ERROR: append_arg is out of memory (realloc)!\n",logOut);
			error_exit(1);
		}


		(*list)[*listC - 1] = arg; /* set the last element to the argument pointer */



	}

}



void get_args(int argc, char*argv[]){
	
	int argI = 1; /* argument index, starts at first argument, ignores filename */
	int i;
	int result;

	for (;argI + 1 <= argc; argI++){ /* argc is a count, argI is an index thats why we need to increment argI */

		if (argv[argI][0] != '-') /* ignore if its not a flag */
			continue;

		switch (argv[argI][1]){

			case 'l':

				argumentFlags += flags_l;

				if (fetch_flag_arg_count(argc,argv,argI+1) != 1){
					fputs("Error: flag -l takes one argument!\n",logOut);
					print_help(argv[0]);
					error_exit(1);
				}

				logFile = argv[argI+1];
				break;

			case 'o':

				argumentFlags += flags_o;

				if (fetch_flag_arg_count(argc,argv,argI+1) != 1){
					fputs("Error: flag -o takes one argument!\n",logOut);
					print_help(argv[0]);
					error_exit(1);
				}

				outputFile = argv[argI+1];
				break;

			case 'i':

				argumentFlags += flags_i;

				result; result = fetch_flag_arg_count(argc,argv,argI+1);

				if (result < 1){
					fputs("Error: flag -i needs at least one argument!\n",logOut);
					print_help(argv[0]);
					error_exit(1);
				}

				for (i = 1;i <= result; i++){ /* starts at 1, becuase argI + 0 == -i */
					append_arg(argv[argI + i],&inputFiles,&inputFilesC);
				}
				break;

			case 'h':
				argumentFlags += flags_h;

				print_help(argv[0]);
				break;

			default:
				printf("Error: unrecognized parameter: %s\n",argv[argI]);
				print_help(argv[0]);
				error_exit(1);
				
		}
	}
}

/* counts how many parameters there are after a flag */
int fetch_flag_arg_count(int argc, char*argv[], int firstIndex){
	
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
	puts("  -l: <logfile>: write logs into <logfile> file");
}


