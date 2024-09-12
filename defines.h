#ifndef mainH
#define mainH

#include <stdio.h>

/* /##################\ */
/*|MAIN.C GLOBAL STUFF|*/
/* \##################/ */
extern FILE *logOut;

void errorExit(int status);

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


/* /#####\ */
/*|VARIOUS|*/
/* \#####/ */
#ifndef NULL
#define NULL 0
#endif

#ifndef bool
typedef short bool;
#endif


/*/#####\*/
/*|FLAGS|*/
/*\#####/*/
#define flags_h 0x01 /* 0b0000_0000_0000_0001 */
#define flags_o 0x02 /* 0b0000_0000_0000_0010 */
#define flags_i 0x04 /* 0b0000_0000_0000_0100 */
#define flags_l 0x08 /* 0b0000_0000_0000_1000 */

/* /#########\ */
/*|ERROR TYPES|*/
/* \#########/ */
typedef enum {fseNoError, fseNoOpen, fseNoClose, fseWrongWrite, fseIsDirectory, fseIsFile} fsError; 

#endif


