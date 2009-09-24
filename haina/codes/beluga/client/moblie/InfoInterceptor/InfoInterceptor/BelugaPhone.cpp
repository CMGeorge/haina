#include "StdAfx.h"
#include "BelugaPhone.h"
#include  <phone.h>
#define MAX_NUMBER_LEN 64
#define MAX_BUF MAX_NUMBER_LEN * 4

BelugaPhone::BelugaPhone(void)
{
}

HRESULT BelugaPhone::MakePhoneCall(TCHAR* phoneNums,bool showPrompt)
{
	HRESULT hr = S_OK;
	PHONEMAKECALLINFO phCallInfo; 
    if(showPrompt)
	{
		phCallInfo.dwFlags=2;
	}else
	{
		phCallInfo.dwFlags=1;
	}
	UINT cchLen = 0;
	TCHAR pszDialString[MAX_BUF]; 
	/*phCallInfo.pszDestAddress=*phoneNums;*/
	phCallInfo.pszDestAddress=pszDialString;
    ZeroMemory(&phCallInfo, sizeof(phCallInfo));
    phCallInfo.cbSize = sizeof(phCallInfo);
	PhoneMakeCall(&phCallInfo);

    return hr;   

}

BelugaPhone::~BelugaPhone(void)
{
}

BOOL DialNumber(void)
{
	static TCHAR	gszDefaultNum[] = TEXT("+1 (425) 882-8080");
    LPTSTR			gpszPhoneNum = gszDefaultNum;
	LONG lResult;
	lResult = tapiRequestMakeCall(gpszPhoneNum, NULL, NULL, NULL);
	return (0 == lResult);
}