#ifndef argsH
#define argsH

/* gets all the arguments and sets the argument flags */
void getArgs(int argc, char*argv[]);

/* arg = the raw text of the argument, THIS POINER MUST STAY ALIVE FOR THE ENTIRE RUNTIME!
   list = a pointer to a char** like inputFiles
   listC = pointer to the count of how many elements are in the list*/
void appendArg(char *arg, char ***list, int *listC);


#endif

