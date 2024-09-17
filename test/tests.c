#include <stdio.h>
#include <stdlib.h>
#include <memory.h>
#include <string.h>
#include <errno.h>

#include "../comp/fs/fs.h"
#include "../compat.h"
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

void set_flags(char **result, const char *flags){
	free(*result);
	*result = malloc(1);
	
	if (*result == NULL)
		oom();

	*result[0] = 0;

	*result = strcat(*result,EXECPATH);
	*result = strcat(*result,flags);
	if (!*result) /* if null */
		oom();
}

int main(){

	puts("\nInitializing..."); /* print new line, so we have a bit of distance to the `make` output */
	setup();

	failed = 0;
	passed = 0;

	{ /* TEST 1 */
		fputs("testing file writing and reading... ",stdout);

		error = fseNoError;
		error =  write_file("out.txt", "Hello World\n12", 15);
		if (error != fseNoError)
			critical_test_fail();

		if (close_file(file,false) != fseNoError)
			critical_test_fail;
		
		
		errno = 0;
		if (open_file("out.txt","w",&file) != fseNoError)
			critical_test_fail();
		if (errno != 0)
			critical_test_fail();


		reader = create_lineRead(file);

		/* Reads the speciefied line of a file, if it exists
	reader: stuct that contains the FILE pointer and the currentLine (should be set to 0 and then let the function handle it)
	line: the line you want to read (starts at 0)

	returns: char pointer to the line. YOU HAVE TO FREE IT!

	errnos: EINVAL, EIO, ENOMEM, ESPIPE
*/
		sExpected = "Hello World\n";
		errno = 0;
		tmp = read_line(reader,0);
		fputs(tmp,stdout);
		if (strcmp(tmp,sExpected) == 0){
			puts("same string!");
		}
		free(tmp);

		sExpected = "12";
		errno = 0;
		tmp = read_line(reader, 0);
		
		free(tmp);

		if (close_file(file,false) != fseNoError)
			critical_test_fail;

		free(reader);

	}





	{ /* TEST 2 */
		fputs("testing file logging... ",stdout);
		set_flags(&tmp," -l log.txt");

		puts("TODO - IMPLEMENT!");

		system(tmp);
	}


	printf("\n#------------------#\nPassed: %d/%d\nFailed: %d/%d\n",passed,NUM_TESTS,failed,NUM_TESTS);
	return 0;
}
