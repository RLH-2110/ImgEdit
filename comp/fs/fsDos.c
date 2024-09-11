#include "../../compat.h"
#ifdef OS_DOS

#include "../../defines.h"
#include <dos.h>

bool isDirectory(const char *path) {

	struct find_t fileinfo;
	if (_dos_findfirst(path,_A_SUBDIR,&fileinfo) == 0){ /* return 0 = found. we searched for the path as a directory*/
		return 1;
	}
	return 0;
}


/* OS_DOS */
#endif 

typedef int make_iso_compiler_happy;
