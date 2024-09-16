#ifndef fsH
#define fsh

#include <stdio.h>
#include "../../defines.h"

typedef struct lineRead{ 
	long currentLine;
	FILE *file;
} lineRead;

bool isDirectory(const char *path);


/* creates a file and checks for write access */
fsError create_file(const char* filePath, FILE **out_file);

/* Opens file and stores it in output*/
fsError open_file(const char* filePath, FILE** output);

fsError write_file(const char* filePath, const char* buffer, size_t bufferSize);



/* Reads the speciefied line of a file, if it exists
	reader: stuct that contains the FILE pointer and the currentLine (should be set to 0 and then let the function handle it)
	line: the line you want to read (starts at 0)

	returns: char pointer to the line. YOU HAVE TO FREE IT!

	errnos: EINVAL, EIO, ENOMEM, ESPIPE
*/
CALLER_FREES char* read_line(lineRead *reader, long line);


/* Closes the log file, if it exists. */
fsError close_log_file();

/* closes file, if its not NULL. Logs if log is non zero, otherwhise it wont log*/
fsError close_file(FILE* file,bool log);


/* initalizes and lineRead struct*/
CALLER_FREES lineRead* create_lineRead(FILE* file);

#endif /* fsH */
