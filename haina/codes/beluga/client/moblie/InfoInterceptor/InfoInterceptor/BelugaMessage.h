#pragma once
#include <tapi.h>
#include <sms.h>
class BelugaMessage
{
public:
	BelugaMessage(void);
	BOOL INITMapIRule(void); 
	BOOL BelugaSendMessage(TCHAR	phoneNum[],TCHAR MessageContext[]);
public:
	~BelugaMessage(void);
};
