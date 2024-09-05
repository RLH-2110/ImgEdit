#include "stdio.h"


void getArgs(int argc, char*argv[]){
	
}

void print_help(char* argv0){
	printf("Usage: %s [-o <output>] [-h] [-i <inputs>...]\n",argv0)
	puts("  -o <output>: <output> specifies the name of the output file")
	puts("  -i <inputs>: <inputs> specifies the input files, can be one or multiple")
	puts("  -h: shows this help screen")
}
