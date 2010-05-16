#ifndef BELUGAPHONE_H
#define BELUGAPHONE_H

#include <astdtapi.h>
class BelugaPhone
{
public:
	BelugaPhone(void);
    BOOL DialNumber( TCHAR	gszDefaultNum[]);
public:
	~BelugaPhone(void);
};

#endif