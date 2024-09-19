#ifndef setupH
#define setupH

#include <stdio.h>
#include "defines.h"

extern FILE *logOut;

void setup();
short set_log_file(); /* its a bool , but the compiler hates me if I write 'bool' even if I included the definition of bool from defines.h*/
void error_exit(int status);

/* setupH */
#endif

