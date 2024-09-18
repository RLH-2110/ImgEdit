#include <string.h>
#include <stdlib.h>
#include <stdio.h>

#include "../defines.h"
#include "flags.h"
#include "../int.h"

/* This file handles the command line flags */


uint32 argumentFlags = 0; /* the flags that are set with the command line! */

char** inputFiles = NULL; 
int inputFilesC = 0; /* list of how many filenames there are in inputFiles */

char* outputFile = NULL;

char* logFile = NULL;

