#include <stdio.h>
#include <stdlib.h>
#include <memory.h>
#include <string.h>

#include "../comp/string/str.h"
#include "../compat.h"


#include "../setup.h"

#ifdef OS_POSIX
#define EXECPATH "./imgEdit"
#else
#define EXECPATH "imgEdit.exe"
#endif

#ifndef NULL
#define NULL 0
#endif

#define NUM_TESTS 2

int failed;
int passed;
char* sInputA;
char* sInputB;
char* sInputC;
char* sExpected;
char* tmp;

void oom(){
	puts("\nTESTER IS OUT OF MEMORY!");
	exit(1);
}

void criticalTestFail(){
	failed++;
	printf("Failed!\nCritial Test failed! We cant test anything else with this failure!\nPassed: %d\nFailed: %d\n",passed,failed);
	exit(1);
}

void setFlags(char **result, const char *flags){
	free(*result);
	*result = malloc(1);
	
	if (*result == NULL)
		oom();

	*result[0] = 0;

	strcat_c(result,EXECPATH);
	strcat_c(result,flags);
	if (!*result)
		oom();
}

int main(){

	puts(""); /* print new line, so we have a bit of distance to the `make` output */
	setup();

	failed = 0;
	passed = 0;

	sInputA = "hallo";
	sInputB = " Welt";
	sInputC = "!";
	sExpected = "hallo Welt!";

	{ /* TEST 0 */
		fputs("testing strcat_c...",stdout);

		tmp = malloc(1);
		if (!tmp)
			oom();

		tmp[0] = 0;

		strcat_c(&tmp,sInputA);
		strcat_c(&tmp,sInputB);
		strcat_c(&tmp,sInputC);
		
		if (tmp == NULL)
			criticalTestFail();
		
		if (strcmp(tmp,sExpected) == 0){
			passed++;
			puts(" passed!");
		}
		else
			criticalTestFail();

		tmp[0] = 0;
	}

	{
		fputs("testing file logging...",stdout);
		setFlags(&tmp," -l log.txt");

		puts("TODO - IMPLEMENT!")

		system(tmp);
	}


	printf("\n#------------------#\nPassed: %d/%d\nFailed: %d/%d\n",passed,NUM_TESTS,failed,NUM_TESTS);
	return 0;
}
