#include "windows.h"
#include "cemapi.h"

class smsInterceptor
{

enum EDialogResult
{
	eDialogResult_None = 0,				// no error. Just don't send the SMS
	eDialogResult_SendSMS = 1,			// Send the SMS
	eDialogResult_ErrorNoMem = -1,		// Out of memory
	eDialogResult_ErrorGeneral = -2		// Another error occurred
};

public:
	smsInterceptor(void);
    LRESULT RegSmsInterceptor();
    LRESULT unRegSmsInterceptor();

	//·¢ËÍ¶ÌÐÅ
	EDialogResult SendSMS(BOOL bSendConfirmation, BOOL bUseDefaultSMSC, LPCTSTR lpszSMSC, LPCTSTR lpszRecipient, LPCTSTR lpszMessage);

};



