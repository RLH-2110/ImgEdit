#include "config.h"
#include "../src/defines.h"
#include "../src/compat.h"
#include <stdlib.h>

void print_config(void){

	/*print version*/
	printf("version: %s\n",VERSION);

	/*print GUI enabled?*/
#ifdef GRAPHICS_MODE
	puts("GUI Enabled");
#else
	puts("GUI Disabled");
#endif

	/*print OS*/
	fputs("Operating System: ",stdout);
	puts(OS_STRING);

	exit(EXIT_SUCCESS);
	return;
}

