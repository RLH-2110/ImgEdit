#include <stdio.h>
#include <stdlib.h>
#include <memory.h>
#include <string.h>
#include <errno.h>

#include "../src/argParse/flags.h"
#include "../src/argParse/args.h"
#include "../src/comp/fs/src/fs.h"
#include "../src/compat.h"
#include "../src/str.h"
#include "../src/int.h"

#include "tests.h"
#include "../src/setup.h"

#ifdef OS_POSIX
#define EXECPATH "./imgEdit"
#else
#define EXECPATH "imgEdit.exe"
#endif

#ifndef NULL
#define NULL 0
#endif




#ifdef testing

int failed;
int passed;
int skipped;

int i;

bool fail; /* for test functions to see if they failed locally*/

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
	printf("\nTest Failed!\nCritial Test failed! We cant test anything else with this failure!\nPassed: %d/%d\nFailed: %d/%d\nSkipped: %d/%d\n", passed, NUM_TESTS, failed, NUM_TESTS, skipped, NUM_TESTS);
	close_log_file();
	exit(1);
}

void critical_fail(){
	printf("Critical failure!\nPassed: %d/%d\nFailed: %d/%d\nSkipped: %d/%d\n", passed, NUM_TESTS, failed, NUM_TESTS, skipped, NUM_TESTS);
	close_log_file();
	exit(1);
}


/*	notes:

	the extractOpenFlags will not have its own tests.

*/

/* might become unused. it was inteded to be used for calling the app with SYSTEM() and checking the results.*/
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

void test0(){ /* TEST 0 */ /* own functions used: strcat_c */
		fputs("tst0 strcat_c...                 ",stdout);


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
		tmp = strcat_c(tmp,NULL);

		if (tmp == NULL)
			critical_test_fail();

		if (strcmp(tmp,sExpected) != 0)
			critical_test_fail();

		free(tmp); tmp = NULL;

		/* tmp is NULL, test it!*/

		tmp = strcat_c(tmp, sInputA);
		if (tmp != NULL)
			critical_test_fail();


		puts("passed!");
		passed++;
}


void test1(){  /* own functions used: set_log_file, close_log_file */
	fail = false;

	fputs("tst1 file logging...             ",stdout);

	remove("log.txt");

	if (getAttributes("log.txt") != fsfNoFile) {
		skipped++;
		puts("test can't commence!");
		return;
	}

	logFile = "log.txt";
	if (set_log_file() == false){
		failed++;
		return;
	}
	fputs("Logging test!\n",logOut);
	if (close_log_file() == fseNoClose){
		failed++;
		return;
	}


	/* Opens the file and prepares the reader*/

	{
		errno = 0;
		if (open_file("log.txt","r",&file) != fseNoError){
			fail = true;
			goto test1_cleanup;
		}
		if (errno != 0){
			fail = true;
			goto test1_cleanup;
		}

		reader = create_lineRead(file);
	}


	/* read second line (first one should be "set log file to: log.txt\n") */
	{
		sExpected = "Logging test!\n";
		errno = 0;
		tmp = read_line(reader,1);

		if (errno != 0 || tmp == NULL) {
			fail = true;
			goto test1_cleanup;
		}

		if (strcmp(tmp,sExpected) != 0) {
			fail = true;
			goto test1_cleanup;
		}



	test1_cleanup:

		free(tmp); tmp = NULL;

		if (reader != NULL)
			if (close_file(reader->file,false) != fseNoError)
				fail = true;
		
		reader->file = NULL;

		free(reader); reader = NULL;

		set_log_file();

		if (!fail){
			puts("passed!");
			passed++;
		}else{
			puts("failed!");
			failed++;
		}
	}
}


/*  /--------------------------------------\ */
/* | SCREEN IS REDIRECTED FROM HERE ONE OUT |*/
/*  \--------------------------------------/ */




void test2(){ /* own functions used: get_args*/

	char* oldLogFile;
	int argc;
	char** argv;


	fputs("tst2 argument parseing...        ",stdout);
	fail = false;

	/* backup old logging string */
	oldLogFile = logFile;


	/* imgEdit -h */
	{
		argc = 2;
		argv = malloc(argc*sizeof(void*)); /* argument count * size of pointer*/

		if (argv == NULL)
			oom();

		argv[0] = "ImgEdit";
		argv[1] = "-h";
		
		/* reset variables that may be set*/
		argumentFlags = 0; /* the flags that are set with the command line! */
		inputFiles = NULL; 
		inputFilesC = 0; /* list of how many filenames there are in inputFiles */
		outputFile = NULL;
		logFile = NULL;

		get_args(argc,argv);

		sExpected = "Usage:";
		sInputA = malloc(strlen(sExpected)+1);
		if (sInputA == NULL)
			oom();

		rewind(scrOut); /* go to start of file */
		fgets(sInputA,strlen(sExpected)+1,scrOut);  /* read the output */

		if (strcmp(sInputA,sExpected) != 0){
			fail = true;
			printf("\nexpected: %s\tgot: %s\n",sExpected,sInputA);
		}
		
		free(argv); argv = NULL;
		rewind(scrOut); /* go to start of file again, so new stuff overwrites the old one */
	}


	printf("TEST IS NOT YET DONE!!! ADD CASES HERE!\n");

	logFile = oldLogFile;

	if (!fail) {
		puts("passed!");
		passed++;
	}
	else {
		puts("failed!");
		failed++;
	}
}

void test3() { /* THIS TEST DOES NOT YET COUNT TO THE TEST COUNTER! */
	puts("TODO: add more tests!");


}

int main(int argc, char* argv[]){

	puts("\nInitializing..."); /* print new line, so we have a bit of distance to the `make` output */
	setup();

	failed = 0;
	passed = 0;
	skipped = 0;

	test0();
	test1();

	fputs("\nredirecting screen to src.txt... ",stdout);


	if(open_file("src.txt","w+", &scrOut) != fseNoError){
		puts("failed!\nWe can not proceed!\n\n");
		scrOut = stdout;
		critical_fail();
	}else
		puts("success\n\n");
	

	/*TODO: Test open_file to make sure it can do r,w,a,w+ and r+*/

	test2();
	test3();

	if (scrOut != stdout)
		close_file(scrOut,false);

	printf("\n#------------------#\nPassed: %d/%d\nFailed: %d/%d\nSkipped: %d/%d\n",passed,NUM_TESTS,failed,NUM_TESTS,skipped,NUM_TESTS);
	close_log_file();
	return 0;
}

/* testing */
#endif

typedef int make_iso_compiler_happy;

