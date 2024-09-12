#ifndef fsH
#define fsh

#include <stdio.h>
#include "../../defines.h"

bool isDirectory(const char *path);
fsError write_file(const char* filePath, const char* buffer, size_t bufferSize);

/* creates a file and checks for write access */
fsError create_file(const char* filePath, FILE **out_file);

/* Closes the log file, if it exists. */
fsError close_log_file();

#endif /* fsH */
