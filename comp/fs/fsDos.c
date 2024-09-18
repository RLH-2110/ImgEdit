#include "../../compat.h"
#ifdef OS_DOS

#include "../../defines.h"
#include <dos.h>


fsFlags getAttributes(const char *path){
	int flags;
	struct find_t fileinfo;

	flags = 0;

	/* DOS check if file is directory*/
	if (_dos_findfirst(path,_A_SUBDIR,&fileinfo) == 0) /* return 0 = found. we searched for the path as a directory*/
		flags += fsfIsDirectory;


	/* DOS does not have access flags*/
	flags += fsfWriteAccess;
	flags += fsfReadAccess; 

	return flags; /* returns non-zero if its a directory*/
}

/* OS_DOS */
#endif 

typedef int make_iso_compiler_happy;
