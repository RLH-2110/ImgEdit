#include <string.h>
#include <stdlib.h>
#include "defines.h"
#include "flags.h"
#include <stdio.h>

/* This file handles the command line flags */


short argumentFlags = 0; /* the flags that are set with the command line! */

char** inputFiles = NULL; 
int inputFilesC = 0; /* list of how many filenames there are in inputFiles */

char* outputFile = NULL;

/* arg = pointer to the raw text of the argument, THIS POINER MUST STAY ALIVE FOR THE ENTIRE RUNTIME!
   list = a pointer to a char** like inputFiles
   listC = pointer to the count of how many elements are in the list */
void appendArg(char *arg, char ***list, int *listC){
	if (!arg || !list){ /* if one param is NULL */
		puts("ERROR: appendArg does not take NULL!");
		exit(1);
	} 

	if (*list == NULL){

		printf("(debug) size of malloc: %lu\n",sizeof(char*)); /* is 16 during the test*/
		*list = malloc(sizeof(char*));
		if (*list == NULL){
			puts("ERROR: appendArg is out of memory!");
			exit(1);
		}

		*listC = 1;
		*list[0] = arg; /* set the first element in the list to the argument pointer */	
	}else{
		*listC = *listC + 1;

		printf("(debug) size of realloc: %lu\n",*listC * sizeof(char*)); /* is 16 during the test*/

		*list = realloc(*list,*listC * sizeof(char*)); /* make space for one more pointer */


		if (*list == NULL){
			puts("ERROR: appendArg is out of memory (realloc)!");
			exit(1);
		}


		printf("(debug) index listC: %d\n",*listC - 1);
		*list[0] = arg;
		/* *list[*listC - 1] = arg; /* set the last element to the argument pointer */
	}
}

