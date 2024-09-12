#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include "../../defines.h"

/* strcat does not seem to work with my DOS compiler, so I make my own.*/
/* 
	dest = destination string
	src = source string 

	dest will be set to NULL or the new combined string.
*/
void strcat_c(char *dest, const char *src){
	int destLen, srcLen, targetLen;

	if (!dest || !src){ /* If dest or src are null*/
		fputs("ERROR: strcat_c does not take NULL!",logOut);
		return;
	}

	destLen = strlen(dest);
	srcLen = strlen(src);
	targetLen = destLen + srcLen + 1; /* add NULL terminator, since strlen does not include one!*/
	
	fprintf(logOut,"\tdestLen: %d\n\tsrcLen: %d\n\ttargetLen: %d\n\tdest: %s\n\tsrc: %s\n\ttarget: %lu\n\t",destLen,srcLen,targetLen,dest,src,(dest + (destLen)) - dest);
		
	dest = realloc(dest,targetLen); /* make space for both strings */

	

	if (dest == NULL) /* error handled elsewere */
		return;


	strcpy(dest + (destLen),src); /* add src to the end of *dest (and overwerite *dest's null terminator) */
	
	fprintf(logOut,"now: %s\n\tstrlen: %lu\n",dest,strlen(dest));
}


