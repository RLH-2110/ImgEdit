#ifndef flagsH
#define flagsH

/* This file handles the command line flags */


extern short argumentFlags; /* the flags that are set with the command line! */

extern char** inputFiles;
extern int inputFilesC; /* list of how many filenames there are in inputFiles */

extern char* outputFile;

/* arg = the raw text of the argument, THIS POINER MUST STAY ALIVE FOR THE ENTIRE RUNTIME!
   list = a pointer to a char** like inputFiles
   listC = pointer to the count of how many elements are in the list*/
void appendArg(char *arg, char ***list, int *listC);

#endif

