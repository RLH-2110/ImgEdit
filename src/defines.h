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

#define CALLER_FREES 	/* The caller frees that pointer or closes that file */
#define FUNCTION_FREES 	/* the callee frees that pointer or closes that file */


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
#endif

#ifndef always_false
#define always_false false
#define always_true true
#endif

/*/#####\*/
/*|sizes|*/
/*\#####/*/

#define TEXT_READ_BUFF_SIZE 100 /* CANT BE SMALLER THAN 2! NOTE: this number is ment to be 1 bigger than needed! */ /*has been changed to be the inital size of a dynamic array*/



/*/#####\*/
/*|FLAGS|*/
/*\#####/*/
#define flags_h 0x0001 /* 0b0000_0000_0000_0001 */
#define flags_o 0x0002 /* 0b0000_0000_0000_0010 */
#define flags_i 0x0004 /* 0b0000_0000_0000_0100 */
#define flags_l 0x0008 /* 0b0000_0000_0000_1000 */
#define flags_c 0x0010 /* 0b0000_0000_0001_0000 */



/* /#########\ */
/*|ERROR TYPES|*/
/* \#########/ */


/* /###\ */
/*|Enums|*/
/* \###/ */


#endif


