#include "../../compat.h"
#ifdef OS_POSIX

#include "../../defines.h"
#include <sys/stat.h>

/* https://stackoverflow.com/a/4553053/23420761 */
bool isDirectory(const char *path) {
   struct stat statbuf;
   if (stat(path, &statbuf) != 0) /* gets statistics like permission and stuff */
       return 0;
   return S_ISDIR(statbuf.st_mode); /* macro that returns non-zero if file is a directory. */
}


/* OS_POSIX */
#endif 


typedef int make_iso_compiler_happy;
