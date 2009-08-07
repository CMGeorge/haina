/*
 * debug_new.h  1.7 2003/07/03
 *
 * Header file for checking leakage by operator new
 *
 * By Wu Yongwei
 *
 */



#ifndef _DEBUG_NEW_H
#define _DEBUG_NEW_H

#include <new>
#define  DEBUG_NEW_EMULATE_MALLOC
/* Prototypes */
bool check_leaks();
void* operator new(size_t size, const char* file, int line);
void* operator new[](size_t size, const char* file, int line);
#ifndef NO_PLACEMENT_DELETE
void operator delete(void* pointer, const char* file, int line);
void operator delete[](void* pointer, const char* file, int line);
#endif // NO_PLACEMENT_DELETE
void operator delete[](void*);	// MSVC 6 requires this declaration

/* Macros */
#ifndef DEBUG_NEW_NO_NEW_REDEFINITION
#define new DEBUG_NEW
#define DEBUG_NEW new(__FILE__, __LINE__)
#define debug_new new
#else
#define debug_new new(__FILE__, __LINE__)
#endif // DEBUG_NEW_NO_NEW_REDEFINITION
#ifdef DEBUG_NEW_EMULATE_MALLOC
#include <stdlib.h>
#define malloc(s) ((void*)(debug_new char[s]))
#define free(p) delete[] (char*)(p)
#endif // DEBUG_NEW_EMULATE_MALLOC

/* Control flags */
extern bool new_verbose_flag;	// default to false: no verbose information
extern bool new_autocheck_flag;	// default to true: call check_leaks() on exit


#ifndef DEBUG_NEW_HASHTABLESIZE
#define DEBUG_NEW_HASHTABLESIZE 16384
#endif
#ifndef DEBUG_NEW_FILENAME_LEN
#define DEBUG_NEW_FILENAME_LEN	20
#endif
#if DEBUG_NEW_FILENAME_LEN == 0 && !defined(DEBUG_NEW_NO_FILENAME_COPY)
#define DEBUG_NEW_NO_FILENAME_COPY
#endif
#ifndef DEBUG_NEW_NO_FILENAME_COPY
#include <string.h>
#endif

struct new_ptr_list_t
{
	new_ptr_list_t*		next;
#ifdef DEBUG_NEW_NO_FILENAME_COPY
	const char*			file;
#else
	char				file[DEBUG_NEW_FILENAME_LEN];
#endif
	int					line;
	size_t				size;
};

static new_ptr_list_t* new_ptr_list[DEBUG_NEW_HASHTABLESIZE];
#endif // _DEBUG_NEW_H
