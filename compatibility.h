#ifndef compatibilityH
#define compatibilityH

#ifdef OS_LINUX
#define OS_STRING "LINUX"
#endif

#ifdef OS_WINDOWS
#define OS_STRING "WINDOWS"
#endif

#ifdef OS_DOS
#define OS_STRING "DOS"
#endif

#ifdef OS_HAIKU
#define OS_STRING "HAIKU"
#endif

#ifndef OS_STRING
#define OS_STRING "UNKOWN OS"
#endif

#endif