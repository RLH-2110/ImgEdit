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

#ifndef OS_STRING
/* we got no idea what it is, so lets assume its a POSIX based system.*/
#define OS_STRING "UNKOWN OS"
#define OS_POXIX
#endif

#endif

