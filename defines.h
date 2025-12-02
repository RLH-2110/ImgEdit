#ifndef mainH
#define mainH

#include <stdio.h>
#include "int.h"


/* /#################\ */
/*|MAIN.C GLOBAL STUFF|*/
/* \#################/ */
#include "setup.h"



/* /###########\ */
/*|VERSION STUFF|*/
/* \###########/ */

/* version flags explained:
R = Rewrite (always here in this version)
L = command Line only (if its not compiled with graphics capabilities)
G = Grapical (if compiled with grapical stuff)
C = Custom Selection (If compiled with certain functions turned off, For example if compiled without fat12 support)
*/

#define VERSION "0.0.0"

/* FOR LATER USE. if we compiler with grapics stuff, add a G to the version, else add an L*/
#ifdef GRAPHICS_MODE
#define GRAPHICS_CHR 'G'
#else
#define GRAPHICS_CHR 'L'
#endif



/* /######\ */
/*|Settings|*/
/* \######/ */



/* /###\ */
/*|Notes|*/
/* \###/ */

#define CALLER_FREES /* The caller frees that poinnter*/
#define FUNCTION_FREES /* the callee frees that pointer*/


/* /##\ */
/*|Util|*/
/* \##/ */
#ifndef NULL
#define NULL 0
#endif

#ifndef bool
typedef short bool;
#endif

#ifndef true
#define true 1
#define false 0
#define always_false false
#define always_true false
#endif

#define FS_CURR -1

/*/#####\*/
/*|sizes|*/
/*\#####/*/

#define TEXT_READ_BUFF_SIZE 100 /* CANT BE SMALLER THAN 2! NOTE: this number is ment to be 1 bigger than needed! */



/*/#####\*/
/*|FLAGS|*/
/*\#####/*/
#define flags_h 0x01 /* 0b0000_0000_0000_0001 */
#define flags_o 0x02 /* 0b0000_0000_0000_0010 */
#define flags_i 0x04 /* 0b0000_0000_0000_0100 */
#define flags_l 0x08 /* 0b0000_0000_0000_1000 */
#define flags_debugArgs 0x8000  /* 0b1000_0000_0000_0000 (can not be set via command line args, only set via tests.c) */

/* /#########\ */
/*|ERROR TYPES|*/
/* \#########/ */
typedef enum {
	fseNoError, fseNoOpen, fseNoClose,	fseWrongWrite, fseWrongRead,
	fseIsDirectory,	 fseIsFile, fseNoRead, fseNoWrite, fseMemory, fseLogic,
	fseNoCreate, fseNoDelete, fseBufferSize, fseNULLParam, fseSeekError, 
	fseInternalFSError, fseFileAlreadyExists
} fsError; 


/* /###\ */
/*|Enums|*/
/* \###/ */

/* for metadata about files*/
typedef enum {	
	fsfInvalid		= 0x00, 
	fsfReadAccess 	= 0x01,
	fsfWriteAccess 	= 0x02,
	fsfIsDirectory	= 0x04,
	fsfNoFile		= 0x80
} fsFlags; 

/* for fopen file flags (like rb+)*/
typedef enum {
	fsOpenFlagsError			= 0x00,
	fsOpenFlagsReading			= 0x01,
	fsOpenFlagsWriting			= 0x02,
	fsOpenFlagsDontCreate		= 0x04,
	fsOpenFlagsNoOverWriting	= 0x08
} fsOpenFlags;


#endif


