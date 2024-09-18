#ifndef flagsH
#define flagsH

#include "../int.h"

/* This file handles the command line flags */

/* View Defines.h section 'Flags' */
extern uint32 argumentFlags; /* the flags that are set with the command line! */

extern char** inputFiles;
extern int inputFilesC; /* list of how many filenames there are in inputFiles */

extern char* outputFile;

extern char* logFile;

#endif

