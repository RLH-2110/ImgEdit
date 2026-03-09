#ifndef setupH
#define setupH

#include <stdio.h>
#include "defines.h"
#include "comp/fs/src/fs.h"

extern FILE *logOut; /* outout steam for logging*/
extern FILE *scrOut; /* normal output stream */

void setup(void);
short set_log_file(void); /* its a bool , but the compiler hates me if I write 'bool' even if I included the definition of bool from defines.h*/
fsError close_log_file(void);
void error_exit(int status);

/* setupH */
#endif

