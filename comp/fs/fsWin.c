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



/* OS_WINDOWS */
#endif 

typedef int make_iso_compiler_happy;

