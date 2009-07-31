// BelugaDbTest.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

extern void ContactTest();
extern void GroupTest();
extern void TagTest();

int _tmain(int argc, _TCHAR* argv[])
{
	ContactTest();
	GroupTest();
	TagTest();

	getchar();
	return 0;
}

