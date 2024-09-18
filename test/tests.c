#include <stdio.h>
#include <stdlib.h>
#include <memory.h>
#include <string.h>
#include <errno.h>

#include "../comp/fs/fs.h"
#include "../compat.h"
#include "../str.h"
#include "tests.h"

#include "../setup.h"

#ifdef OS_POSIX
#define EXECPATH "./imgEdit"
#else
#define EXECPATH "imgEdit.exe"
#endif

#ifndef NULL
#define NULL 0
#endif


int failed;
int passed;

int i;

char *sInputA;
char *sInputB;
char *sInputC;
char *sExpected;
char *tmp;
lineRead *reader;

fsError error;
FILE* file;

void oom(){
	puts("\nTESTER IS OUT OF MEMORY!");
	close_log_file();
	exit(1);
}

void critical_test_fail(){
	failed++;
	printf("\nTest Failed!\nCritial Test failed! We cant test anything else with this failure!\nPassed: %d\nFailed: %d\n",passed,failed);
	close_log_file();
	exit(1);
}

char* set_flags(CALLER_FREES char *result, const char *flags){

	

	if (!flags){
		puts("set_flags: flags should not be a NULL pointer!");
		close_log_file();
		exit(1);
	}
	if (result != NULL){
		fputs("WARNING: input pointer is not NULL! It will be freed now! ",stdout);
		free(result);
	}
	result = malloc(1);

	if (result == NULL)
		oom();

	result[0] = 0;

	result = strcat_c(result,EXECPATH);
	result = strcat_c(result,flags);

	if (result == NULL)
		oom();

	return result;
}

int main(){

	puts("\nInitializing..."); /* print new line, so we have a bit of distance to the `make` output */
	setup();

	failed = 0;
	passed = 0;

	{ /* TEST 0 */
		fputs("testing strcat_c... ",stdout);


		sInputA = "hallo";
		sInputB = " Welt";
		sInputC = "!";
		sExpected = "hallo Welt!";


		tmp = malloc(1);
		if (!tmp)
			oom();
		tmp[0] = 0;

		tmp = strcat_c(tmp,sInputA);
		tmp = strcat_c(tmp,sInputB);
		tmp = strcat_c(tmp,sInputC);

		if (tmp == NULL)
			critical_test_fail();

		if (strcmp(tmp,sExpected) != 0)
			critical_test_fail();

		free(tmp); tmp = NULL;

		passed++;
		puts(" passed!");
	}

	{ /* TEST 1 */
		fputs("testing file writing and reading... ",stdout);

		error = fseNoError;

		/* creating and writing the file*/
		{
			error =  write_file("out.txt", "Hello World\n12", 15);
			if (error != fseNoError)
				critical_test_fail();

			if (close_file(file,false) != fseNoError)
				critical_test_fail();
			file = NULL;
		}

		/* Opens the file and prepares the reader*/

		{
			errno = 0;
			if (open_file("out.txt","r",&file) != fseNoError)
				critical_test_fail();
			if (errno != 0)
				critical_test_fail();

			reader = create_lineRead(file);
		}


		/* read first line */
		{
			sExpected = "Hello World\n";
			errno = 0;
			tmp = read_line(reader,0);

			if (strcmp(tmp,sExpected) != 0)
				critical_test_fail();
			
			free(tmp); tmp = NULL;

		}

		/* read second/last line */
		{
			sExpected = "12";
			errno = 0;
			tmp = read_line(reader, 1);

			if (strcmp(tmp,sExpected) != 0)
				critical_test_fail();

			free(tmp); tmp = NULL;
		}

		/* read first line again (twice), to see if we can go backwards or stay backwards*/
		for (i = 0;i < 2;i++)
		{

			sExpected = "Hello World\n";
			errno = 0;
			tmp = read_line(reader, 0);

			if (strcmp(tmp,sExpected) != 0)
				critical_test_fail();

			free(tmp); tmp = NULL;
		}

		if (close_file(reader->file,false) != fseNoError)
			critical_test_fail();
		reader->file = NULL;

		free(reader); reader = NULL;

		puts("passed!");
		passed++;
	}





	{ /* TEST 2 */
		fputs("testing file logging... ",stdout);
		tmp = set_flags(tmp," -l log.txt");

		puts("TODO - IMPLEMENT!");

		/*
		this causes errors. read up on it!
		system(tmp);
		free(tmp); tmp = NULL;*/
	}


	printf("\n#------------------#\nPassed: %d/%d\nFailed: %d/%d\n",passed,NUM_TESTS,failed,NUM_TESTS);
	close_log_file();
	return 0;
}
