#pragma once
#include <tapi.h>
class BelugaMessage
{
public:
	BelugaMessage(void);
	BOOL INITMapIRule(void); 
	BOOL SendMessage(TCHAR	phoneNum[],TCHAR MessageContext[]);
public:
	~BelugaMessage(void);
};
