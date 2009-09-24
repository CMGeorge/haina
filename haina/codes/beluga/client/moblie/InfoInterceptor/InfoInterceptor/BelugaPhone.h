#pragma once
#include <astdtapi.h>
class BelugaPhone
{
public:
	BelugaPhone(void);
	HRESULT MakePhoneCall(TCHAR* phoneNum,bool showPrompt);
public:
	~BelugaPhone(void);

};
