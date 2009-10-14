/*
 ============================================================================
 Name		: BelugaDbDll.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : BelugaDbDll.cpp - main DLL source
 ============================================================================
 */

//  Include Files

#include <glib.h>
#include <time.h>
#include "Beluga.h"		 


static void free_gstring (gpointer data, gpointer user_data)
	{
	g_string_free((GString*)data, TRUE);
	}

static void free_address_struct(gpointer data, gpointer user_data)
	{
	g_free(data);
	}

static void free_recent_contact_struct(gpointer data, gpointer user_data)
{
	g_free(data);
}

//  Exported Functions
EXPORT_C void freeGStringArray(GPtrArray * pArray)
	{
	if (pArray)
		{
		g_ptr_array_foreach(pArray, free_gstring, NULL);
		g_ptr_array_free(pArray, TRUE);
		}
	}

EXPORT_C void freeAddressArray(GPtrArray * pArray)
	{
	if (pArray)
		{
		g_ptr_array_foreach(pArray, free_address_struct, NULL);
	    g_ptr_array_free(pArray, TRUE);
		}
	}

EXPORT_C void freeRecentContactArray(GPtrArray * pArray)
{
	if (pArray)
	{
		g_ptr_array_foreach(pArray, free_recent_contact_struct, NULL);
		g_ptr_array_free(pArray, TRUE);
	}
}

#ifdef _WIN32_WCE
#include <winbase.h>

EXPORT_C void GetLocalTime(tm* time)
	{
	SYSTEMTIME systime; 
	GetLocalTime(&systime);
	time->tm_year = systime.wYear;
	time->tm_mon = systime.wMonth;
	time->tm_mday = systime.wDay;
	time->tm_hour = systime.wHour;
	time->tm_min = systime.wMinute;
	time->tm_sec = systime.wSecond;
	}

static WCHAR *utf8ToUnicode(const char *zFilename)
	{
	int nChar;
	WCHAR *zWideFilename;

	nChar = MultiByteToWideChar(CP_UTF8, 0, zFilename, -1, NULL, 0);
	zWideFilename = (WCHAR*)malloc(nChar * sizeof(zWideFilename[0]));
	if ( zWideFilename == 0 )
		{ 
		return 0;
		}

	nChar = MultiByteToWideChar(CP_UTF8, 0, zFilename, -1, zWideFilename, nChar);
	if( nChar==0 )
		{
		free(zWideFilename);
		zWideFilename = 0;
		}
	return zWideFilename;
	}

EXPORT_C void deleteFile(gchar * file)
	{
	WCHAR *zWide = utf8ToUnicode(file);
	if( zWide )
		{
		DeleteFileW(zWide);
		}
	}
#else
EXPORT_C void GetLocalTime(tm* tim)
	{
	time_t t;
	time(&t);
	tim = localtime(&t);
	}

#include <io.h>
EXPORT_C void deleteFile(gchar * file)
	{
	_unlink(file);
	}
#endif
