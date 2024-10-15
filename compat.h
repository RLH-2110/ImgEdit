#ifndef compatibilityH
#define compatibilityH

#ifdef OS_LINUX
#define OS_STRING "LINUX"
#define OS_POSIX
#endif

#ifdef OS_WINDOWS
#define OS_STRING "WINDOWS"
#endif

#ifdef OS_DOS
#define OS_STRING "DOS"
#endif

#ifdef OS_HAIKU
#define OS_STRING "HAIKU"
#define OS_POSIX
#endif

/* I would be very surprised if the program compiles in any scenario where OS_STRING is not set. */
#ifndef OS_STRING
#define OS_STRING "UNKNOWN OS" 
#endif


#endif


