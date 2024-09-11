#ifndef fsH
#define fsh

#include <stdio.h>
#include "../../defines.h"

bool isDirectory(const char *path);
fsError write_file(char* filePath, char* buffer, size_t bufferSize);

#endif /* fsH */
