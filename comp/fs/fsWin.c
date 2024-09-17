#include "../../compat.h"
#ifdef OS_WINDOWS

#include "../../defines.h"
#include <fileapi.h>

bool isDirectory(const char *path) {
	DWORD attributes = GetFileAttributesA(path);
	if (attributes == INVALID_FILE_ATTRIBUTES)
		return 0;	
	return attributes & FILE_ATTRIBUTE_DIRECTORY; /* returns non-zero if its a directory*/
}

fsFlags getAttributes(const char *path){
	int flags;
	DWORD attributes = GetFileAttributesA(path);
	if (attributes == INVALID_FILE_ATTRIBUTES)
		return fsfInvalid;

	flags = 0;

	/* set the flags */
	if (attributes & FILE_ATTRIBUTE_DIRECTORY)
		flags += fsfIsDirectory;

	if (attributes & FILE_ATTRIBUTE_READONLY == 0)
		flags += fsfWriteAccess;

	flags += fsfReadAccess; /* I cant seem to check for read acces. but well, I got error checking in other places, so it will be fine. user will just get an error*/

	return flags; /* returns non-zero if its a directory*/
}



/* OS_WINDOWS */
#endif 

typedef int make_iso_compiler_happy;

