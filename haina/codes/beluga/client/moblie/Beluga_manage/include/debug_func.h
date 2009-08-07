
#pragma once
#define	 CHECK_DELETE_ARR_DLG(pl)	CHECK_DELETE((char *)pl-4)
#define  CHECK_NEW_ARR_DLG(pl) CHECK_NEW((char *)pl-4)
#define  CHECK_NEW(pl) AddDebugInfo((char *)pl,__FILE__,__LINE__-1)
#define  CHECK_DELETE(pl) DeleteDebugInfo((char *)pl) 

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
static void AddDebugInfo(char * class_ptr, const char* file, int line)
{
	new_ptr_list_t *ptr=(new_ptr_list_t *)((char *)class_ptr-sizeof(new_ptr_list_t));
#ifdef DEBUG_NEW_NO_FILENAME_COPY
	ptr->file = file;
#else
	strncpy(ptr->file, file, DEBUG_NEW_FILENAME_LEN - 1);
	ptr->file[DEBUG_NEW_FILENAME_LEN - 1] = '\0';
#endif
	ptr->line = line;
}
static void DeleteDebugInfo(char *pointer)
{
	if (pointer == NULL)
		return;
	//new_ptr_list_t* ptr_last = NULL;
	//new_ptr_list_t* ptr=(char *)(pointer-sizeof(new_ptr_list_t));
	::operator delete(pointer);
}