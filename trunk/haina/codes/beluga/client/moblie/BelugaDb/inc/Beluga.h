/*
 ============================================================================
 Name		: Beluga.h
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Base define
 ============================================================================
 */

#ifndef BELUGA_H_
#define BELUGA_H_

#include <glib.h>

/**
@publishedAll

*/
#define IMPORT_C __declspec(dllexport)
/**
@publishedAll

*/
#define EXPORT_C __declspec(dllexport)


/**
@publishedAll
@deprecated
*/
#define GLREF_D extern
/**
@publishedAll
@deprecated
*/
#define GLDEF_D
/**
@publishedAll
@deprecated
*/
#define LOCAL_D static
/**
@publishedAll
@deprecated
*/
#define GLREF_C extern
/**
@publishedAll
@deprecated
*/
#define GLDEF_C
/**
@publishedAll
@deprecated
*/
#define LOCAL_C static



/* define error code */
enum ESide
	{
	ESide_Client = 0,
	ESide_Server = 1
	};

enum EModule
	{
	EModule_Sys = 0,
	EModule_Db = 1
	};

enum ECode
	{
	ECode_No_Error = 0,
	ECode_Not_Implemented = 1,
	ECode_Invalid_Param = 2,
	ECode_Not_Exist = 3,
	ECode_No_Memory = 4,
	ECode_Insert_Failed = 5,
	ECode_Update_Failed = 6,
	ECode_End_Of_Row = 7,
	ECode_Open_File_Failed = 8
	};

#define	ERROR(side, module, code)	((side << 24)|(module << 16)|(code))


IMPORT_C void freeGStringArray(GPtrArray * pArray);
IMPORT_C void freeAddressArray(GPtrArray * pArray);
IMPORT_C void freeRecentContactArray(GPtrArray * pArray);
IMPORT_C void GetLocalTime(tm* time);

#endif /* BELUGA_H_ */
