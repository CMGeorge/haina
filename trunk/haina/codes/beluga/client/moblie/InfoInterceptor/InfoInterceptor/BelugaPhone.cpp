#include "StdAfx.h"
#include "BelugaPhone.h"


typedef struct tagLINEINFO
{
  HLINE hLine;              // Line handle returned by lineOpen
  BOOL  bVoiceLine;         // Indicates if the line is a voice line
  DWORD dwAPIVersion;       // API version that the line supports
  DWORD dwNumOfAddress;     // Number of available addresses on the line
  DWORD dwPermanentLineID;  // Permanent line identifier
  TCHAR szLineName[256];    // Name of the line
} LINEINFO, *LPLINEINFO;
HWND g_hwndDial      = NULL;    // Handle to the dialing window
HCALL g_hCall = NULL;           // Handle to the open line device on 
// which the call is to be originated 
// (lineMakeCall)

LONG g_MakeCallRequestID = 0;   // Request identifier returned by 
// lineMakeCall
LONG g_DropCallRequestID = 0;   // Request identifier returned by 


LINEINFO g_CurrentLineInfo;     // Contains the current line information

BelugaPhone::BelugaPhone(void)
{
}

BelugaPhone::~BelugaPhone(void)
{
}

BOOL BelugaPhone::DialNumber( TCHAR	gszDefaultNum[])
{
	/* TCHAR	gszDefaultNum[] = TEXT("+1 (425) 882-8080");*/
    LPTSTR			gpszPhoneNum = gszDefaultNum;
	LONG lResult;
	lResult = tapiRequestMakeCall(gpszPhoneNum, NULL, NULL, NULL);
	return (0 == lResult);
}

