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


fsFlags getAttributes(const char *path){
   int flags;
   struct stat statbuf;

   if (stat(path, &statbuf) != 0) /* gets statistics like permission and stuff */
       return fsfInvalid;

   flags = 0;

   /* set the flags */
   if (S_ISDIR(statbuf.st_mode))
      flags += fsfIsDirectory;

   if (statbuf.st_mode & S_IWUSR)
      flags += fsfWriteAccess;

   if (statbuf.st_mode & S_IRUSR)
      flags += fsfReadAccess;

   return flags; /* returns non-zero if its a directory*/
}

/* OS_POSIX */
#endif 


typedef int make_iso_compiler_happy;
