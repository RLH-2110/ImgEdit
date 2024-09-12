#include <string.h>
#include <stdlib.h>
#include "../../defines.h"

/* strcat does not seem to work with my DOS compiler, so I make my own.*/
/* 
	dest = pointer to string
	src = string 

	*dest will be set to NULL or the new combined string.
*/
void strcat_c(char **dest, const char *src){
	int destLen, srcLen, combinedLen;

	if (!dest || !src){ /* If dest or src are null*/
		puts("ERROR: strcat_c does not take NULL!");
		return;
	}


	destLen = strlen(*dest);
	srcLen = strlen(src);
	combinedLen = destLen + srcLen + 1; /* add NULL terminator, since strlen does not include one!*/

	*dest = realloc(*dest,combinedLen); /* make space for both strings */

	if (*dest == NULL) /* error handled elsewere */
		return;


	strcpy(*dest + (destLen),src); /* add src to the end of *dest (and overwerite *dest's null terminator) */
}


