#include "../../defines.h"
#include "../../setup.h"
#include "../../int.h"

#include "fs.h" /* to get the isDirectory definition*/

#include <stdio.h>
#include <errno.h>
#include <stdlib.h>



/*  /########\  */
/* | Writeing | */
/*  \########/  */



/* writes COUNT bytes from BUFFER and write them in FILE at LOCATION.
	If LOCATION is FS_CURR (-1) then it writes at the current position. 
*/
fsError write_file(FILE* file, const char* buffer, size_t count, uint32 location){
	

	if (file == NULL)
		return fseNULLParam;
	

	/* seek to location, unless location is FS_CURR*/
	if (location != FS_CURR)
		fseek(file,location,SEEK_SET);
	

	errno = 0;
	if (fwrite(buffer,1,count,file) != count){
		fprintf(logOut,"Error: write_file write error. errno: %d\n",errno);
		return fseWrongWrite;
	}

	return fseNoError;
}





/*  /#######\  */
/* | OPENING | */
/*  \#######/  */





fsError open_file(const char* filePath, char* fileFlags, FILE** output){

	FILE *file;
	int flags;

	


	if (!filePath || !fileFlags || !output){ /* if parameters are NULL */
		fputs("Error: open_file got a does not take NULL\n",logOut);
		return fseLogic;
	}

	if (fileFlags[0] == '\0'){
		fputs("Error: open_file needs file flags!\n",logOut);
		return fseLogic;
	}

	/* Get flags and filter out flags that wont work for us*/
	flags = getAttributes(filePath);



	if ((flags & fsfReadAccess) == 0 && (fileFlags[0] == 'r' || fileFlags[1] == '+')){
		fprintf(logOut,"Error: open_file has no read access to %s\n",filePath);
		return fseNoRead;
	}

	/* if we try to have write access, but dont have write acces.  DOES NOT TRIGGER IF THE FLAGS ARE fsfInvalid, because that likely means we will create the file later*/
	if ((flags & fsfWriteAccess) == 0 && (fileFlags[0] == 'w' || fileFlags[0] == 'a' || fileFlags[1] == '+') && flags != fsfInvalid){
		fprintf(logOut,"Error: open_file has no write access to %s\n",filePath);
		return fseNoWrite;
	}

	if (flags & fsfIsDirectory){
		fprintf(logOut,"Error: open_file error! %s is a directory!\n",filePath);
		return fseIsDirectory;
	}

	errno = 0;
	file = fopen(filePath,fileFlags);

	if (file == NULL /*|| errno != 0*/){
		/*fprintf(logOut,"Error: open_file open error. errno: %d\n",errno);*/
		fputs("Error: open_file open error!",logOut);

		return fseNoOpen;
	}
	
	*output = file;
	
	return fseNoError;
}









/*  /#######\  */
/* | Reading | */
/*  \#######/  */



/* Reads the speciefied line of a file, if it exists
	reader: stuct that contains the FILE pointer and the currentLine (should be set to 0 and then let the function handle it)
	line: the line you want to read (starts at 0)

	returns: char pointer to the line. YOU HAVE TO FREE IT!

	errnos: EINVAL, EIO, ENOMEM, ESPIPE, ERANGE

	ERANGE 	is set when the buffer is too small to fit the line
	ENOMEM 	is set when there is no memory
	EINVAL 	is set when there is an invalid lineRead
	EIO 	is set on an IO error
	ESPIPE	is set when the line you tried to read it past the end of file

	errnos: EINVAL, EIO, ENOMEM, ESPIPE
*/
CALLER_FREES char* read_line(lineRead *reader, long line){
	char *buff;

	if (reader == NULL){
		fputs("Error: read_line does not take NULL!",logOut);
		errno = EINVAL;
		return NULL;
	}
	if (reader->file == NULL){
		fputs("Error: read_line reader->FILE can not be NULL!",logOut);
		errno = EINVAL;
		return NULL;
	}

	if (line < reader->currentLine){
		if (fseek(reader->file,0,SEEK_SET) != 0){
			fputs("Error: read_line failed seeking the begining of the file",logOut);
			errno = EIO;
			return NULL;
		}
		reader->currentLine = 0;
	}

	buff = malloc(TEXT_READ_BUFF_SIZE); /* See defines.h it should be 100.*/
	if (buff == NULL){
		fputs("Error: read_line out of memory!",logOut);
		errno = ENOMEM;
		return NULL;
	}

	/* wait till we are in the correct line*/
	for (;reader->currentLine < line;reader->currentLine++){
		buff[TEXT_READ_BUFF_SIZE - 2] = '\n'; /* set it to /n for a later test, it might get overwriten by \0 (good) or another char (bad)*/
		buff = fgets( buff, TEXT_READ_BUFF_SIZE, reader->file ); /* read characters (first should NOT be NULL, second should be NULL*/
		if (feof(reader->file)){
			fputs("Error: read_line function cant reach the specefied line, it does not exist", logOut);
			errno = ESPIPE; /* ESPIPE  = Illegal seek */
			return NULL;
		}
		if (ferror(reader->file) || buff == NULL){
			fputs("Error: read_line function had an IO error!",logOut);
			errno = EIO;
			return NULL;
		}

		if (buff[TEXT_READ_BUFF_SIZE - 2] != '\n' && buff[TEXT_READ_BUFF_SIZE - 2] != '\0') { /* if line was read (we already checked if its eof) and does not end in \n*/
			/*if we are here, the buffer is too small to read the line. so we decrement the line, so it stays the same when we increment it during the loop. this allows us to go though the whole line*/
			reader->currentLine--;
		}
	}

	/* We are in the correct line now !*/

	buff[TEXT_READ_BUFF_SIZE - 1] = 'a'; /* set to dectect if we cant fit the line*/
	buff[TEXT_READ_BUFF_SIZE - 2] = '\n'; /* set it to /n for a later test, it might get overwriten by \0 (good) or another char (bad)*/
	buff = fgets( buff, TEXT_READ_BUFF_SIZE, reader->file ); /* read line */
	if (feof(reader->file) && buff == NULL){
		fputs("Error: read_line function cant reach the specefied line, it does not exist!",logOut);
		errno = ESPIPE; /* ESPIPE  = Illegal seek */
		return NULL;
	}
	if (ferror(reader->file) || buff == NULL){
		fputs("Error: read_line function had an IO error!",logOut);
		printf("\n\t\terrno: %d\n",errno); /* errno 9 */
		errno = EIO;
		return NULL;
	}

	if (buff[TEXT_READ_BUFF_SIZE-2] != '\n' && buff[TEXT_READ_BUFF_SIZE-2] != '\0' && !feof(reader->file)) { /* not the last line, and does not end with \n and its not the end of file*/
		/* In this case our buffer is probably too small.*/
		fprintf(logOut, "Error: read_line function can't read line %ld! it does not fit in buffer of size %d!\n", reader->currentLine + 1, TEXT_READ_BUFF_SIZE);
	
		errno = ERANGE;
		return NULL;
	}


	reader->currentLine++;
	return buff;

}




/* reads COUNT bytes into BUFFER from LOCATION in FILE
	If LOCATION is FS_CURR (-1) then it reads at the current position. 
*/
fsError read_file(FILE* file, char** buffer, size_t count, uint32 location){
	

	if (file == NULL)
		return fseNULLParam;
	

	/* seek to location, unless location is FS_CURR*/
	if (location != FS_CURR)
		fseek(file,location,SEEK_SET);
	

	errno = 0;
	if (fread(*buffer,1, count, file ) != count){
		fprintf(logOut,"Error: read_file write error. errno: %d\n",errno);
		return fseWrongRead;
	}

	return fseNoError;
}










/*  /########\  */
/* | Closeing | */
/*  \########/  */






/* Closes file, if its not NULL. */
fsError close_file(FILE* file,bool log){
	
	if (file != NULL){

		if (log)
			fputs("closing file...",logOut);

		errno = 0;
		if (fclose(file) != 0){
			fprintf(logOut,"close_log_file error! errno: %d\nLOG FILE COULD NOT BE CLOSED!\n",errno);
			return fseNoClose;
		}
	}

	return fseNoError;
}



/* Closes the log file, if it exists. */
fsError close_log_file(){
	
	if (logOut != stdout){

		fputs("closing log file...",logOut);

		errno = 0;
		if (fclose(logOut) != 0){
			fprintf(logOut,"close_log_file error! errno: %d\nLOG FILE COULD NOT BE CLOSED!\n",errno);
			return fseNoClose;
		}

		logOut = stdout;
	}

	return fseNoError;
}




/*  /####\  */
/* | UTIL | */
/*  \####/  */


/* initalizes and lineRead struct*/
CALLER_FREES lineRead* create_lineRead(FILE* file){ 

	lineRead *reader = malloc(sizeof(lineRead));
	if (reader == NULL){
		fputs("create_lineRead out of memory!",logOut);
		error_exit(1);
	}

	reader->currentLine = 0;
	reader->file = file;

	return reader;
}

