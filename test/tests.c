#include <stdio.h>
#include <stdlib.h>
#include <memory.h>
#include <string.h>
#include <errno.h>

#include "../argParse/flags.h"
#include "../comp/fs/fs.h"
#include "../compat.h"
#include "../str.h"
#include "../int.h"

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

void test0(){ /* TEST 0 */
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


		puts(" passed!");
		passed++;
}








void test1(){ /* TEST 1 */
	fputs("testing file writing and reading... ",stdout);

	fail = false;

	remove("out.txt");
	if (getAttributes("out.txt") != 0) {
		puts("test can't commence!");
		critical_test_fail();
		return;
	}

	error = fseNoError;
	/* creating and writing the file*/
	{
		error =  write_file("out.txt", "Hello World\n12", 15);
		if (error != fseNoError)
			goto test1_cleanup;
	}

	/* Opens the file and prepares the reader*/

	{
		errno = 0;
		if (open_file("out.txt","r",&file) != fseNoError)
			goto test1_cleanup;
		if (errno != 0)
			goto test1_cleanup;

		reader = create_lineRead(file);
	}


	/* read first line */
	{
		sExpected = "Hello World\n";
		errno = 0;
		tmp = read_line(reader,0);

		if (errno != 0 || tmp == NULL)
			goto test1_cleanup;

		if (strcmp(tmp,sExpected) != 0)
			goto test1_cleanup;

		free(tmp); tmp = NULL;

	}

	/* read second/last line */
	{
		sExpected = "12";
		errno = 0;
		tmp = read_line(reader, 1);

		if (errno != 0 || tmp == NULL)
			goto test1_cleanup;

		if (strcmp(tmp,sExpected) != 0)
			goto test1_cleanup;

		free(tmp); tmp = NULL;
	}

	/* read first line again (twice), to see if we can go backwards or stay backwards*/
	for (i = 0;i < 2;i++)
	{

		sExpected = "Hello World\n";
		errno = 0;
		tmp = read_line(reader, 0);

		if (errno != 0 || tmp == NULL)
			goto test1_cleanup;

		if (strcmp(tmp,sExpected) != 0)
			goto test1_cleanup;

		free(tmp); tmp = NULL;
	}

	goto test1_noFail;
test1_cleanup:
	fail = true;
test1_noFail:

	if (reader != NULL)
		if (close_file(reader->file, false) != fseNoError)
			fail = true;
	reader->file = NULL;

	free(reader); reader = NULL;


	if (!fail) {
		puts("passed!");
		passed++;
	}
	else 
		critical_test_fail();
}



void test2() /* TEST 2 */ {
	fail = false;
	fputs("testing getAttributes and mkdir/rmdir functions... ", stdout);

	remove("out.txt");
	if (getAttributes("out.txt") != 0)
		fail = true;

	write_file("out.txt", "hi", 3);
	if (getAttributes("out.txt") != (fsfReadAccess | fsfWriteAccess))
		fail = true;
	remove("out.txt");
	
	make_dir("out.txt");
	if (getAttributes("out.txt") != fsfIsDirectory)
		fail = true;
	remove_dir("out.txt");

	if (getAttributes("out.txt") != 0)
		fail = true;


	if (!fail) {
		puts("passed!");
		passed++;
	}
	else {
		puts("failed!");
		failed++;
	}
}


void test3(){ /* TEST 3 */
	fail = false;

	fputs("testing create_file function... ",stdout);


	remove("log.txt");
	if (getAttributes("log.txt") != 0) {
		skipped++;
		puts("test can't commence!");
		return;
	}
	

	if (create_file("log.txt", &file) != fseNoError) 
		fail = true;
	

	if (close_file(file, false) != fseNoError) 
		fail = true;
	file = NULL;
	

	/* chceck if file exists here */

	if (getAttributes("log.txt") == 0) {
		fail = true;
	}


	if (!fail){
		puts("passed!");
		passed++;
	}else{
		puts("failed!");
		failed++;
	}
}


void test4(){ /* TEST 4 */
	fail = false;

	fputs("testing file logging... ",stdout);

	remove("log.txt");

	if (getAttributes("log.txt") != 0) {
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
			goto test4_cleanup;
		}
		if (errno != 0){
			fail = true;
			goto test4_cleanup;
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
			goto test4_cleanup;
		}

		if (strcmp(tmp,sExpected) != 0) {
			fail = true;
			goto test4_cleanup;
		}



	test4_cleanup:

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

void test5() { 
	fputs("testing file writing and reading with buffers bigger than TEXT_READ_BUFF_SIZE... ",stdout);
	fail = false;

	remove("out.txt");
	if (getAttributes("out.txt") != 0) {
		puts("test can't commence!");
		skipped++;
		return;
	}

	
	sInputA = malloc((TEXT_READ_BUFF_SIZE + 1)*2+5);
	if (sInputA == NULL)
		oom();


	/* part one: set the first long line of sInputA*/
	for (i = 0; i < TEXT_READ_BUFF_SIZE - 2; i++)
		sInputA[i] = 'A';
	sInputA[TEXT_READ_BUFF_SIZE - 2] = 'B';
	sInputA[TEXT_READ_BUFF_SIZE - 1] = '\n';

	/* part 2 set second line */

	sInputA[TEXT_READ_BUFF_SIZE+0] = '1';
	sInputA[TEXT_READ_BUFF_SIZE+1] = '2';
	sInputA[TEXT_READ_BUFF_SIZE+2] = '\n';


	/* part 3 set last long line */
	for (i = TEXT_READ_BUFF_SIZE + 3; i < (TEXT_READ_BUFF_SIZE + 3) + TEXT_READ_BUFF_SIZE - 2; i++)
		sInputA[i] = 'A';
	sInputA[(TEXT_READ_BUFF_SIZE + 3) + TEXT_READ_BUFF_SIZE - 2] = 'B';
	sInputA[(TEXT_READ_BUFF_SIZE + 3) + TEXT_READ_BUFF_SIZE - 1] = '\0';



	error = fseNoError;
	/* creating and writing the file*/
	{
		error =  write_file("out.txt", sInputA, strlen(sInputA)+1);
		if (error != fseNoError)
			goto test5_cleanup;
	}

	/* Opens the file and prepares the reader*/

	{
		errno = 0;
		if (open_file("out.txt", "r", &file) != fseNoError) 
			goto test5_cleanup;

		if (errno != 0) 
			goto test5_cleanup;

		reader = create_lineRead(file);
	}



	/* read second line */
	{
		sExpected = "12\n";
		errno = 0;
		tmp = read_line(reader, 1);

		if (tmp == NULL || errno != 0)
			goto test5_cleanup;

		if (strcmp(tmp,sExpected) != 0)
			goto test5_cleanup;

		free(tmp); tmp = NULL;
	}

	/* read last line */
	{
		fputs("we expect an buffer error in line 3\n", logOut);
		errno = 0;
		tmp = read_line(reader, 2);

		if (errno != ERANGE)
			goto test5_cleanup;

		free(tmp); tmp = NULL;
	}

	goto test5_noFail;
test5_cleanup:
	fail = true;
test5_noFail:

	if (reader != NULL)
		if (close_file(reader->file, false) != fseNoError)
			fail = true;
	
	reader->file = NULL;

	free(reader); reader = NULL;
	free(sInputA); sInputA = NULL;

	if (!fail) {
		puts("passed!");
		passed++;
	}
	else {
		puts("failed!");
		failed++;
	}


}

void test6() { /* THIS TEST DOES NOT YET COUNT TO THE TEST COUNTER! */
	puts("TODO: add more tests! like a test for argsparse");


}

int main(){

	puts("\nInitializing..."); /* print new line, so we have a bit of distance to the `make` output */
	setup();

	failed = 0;
	passed = 0;
	skipped = 0;

	test0();
	test1();
	test2();
	test3();
	test4();
	test5();
	test6();

	printf("\n#------------------#\nPassed: %d/%d\nFailed: %d/%d\nSkipped: %d/%d\n",passed,NUM_TESTS,failed,NUM_TESTS,skipped,NUM_TESTS);
	close_log_file();
	return 0;
}

/* testing */
#endif

typedef int make_iso_compiler_happy;

