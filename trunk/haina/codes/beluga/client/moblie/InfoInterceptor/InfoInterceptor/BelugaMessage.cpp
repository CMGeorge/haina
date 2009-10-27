#include "StdAfx.h"
#include "BelugaMessage.h"
#include <cemapi.h>

#define MMBUFSIZE 1024  
BelugaMessage::BelugaMessage(void)
{
}

BelugaMessage::~BelugaMessage(void)
{
}


typedef struct {

      WCHAR g_szMessageBody[255];

      WCHAR g_szPhoneNr[255];

} SMS_BUFFER;

typedef SMS_BUFFER *PSMS_BUFFER;


HANDLE g_hClientEvent = NULL;

HANDLE g_hClientMutex= NULL;

HANDLE g_hClientMMObj= NULL;

PSMS_BUFFER g_pClientSmsBuffer=NULL;

typedef BOOL (CALLBACK *LPFSMSMESSAGEAVAILABLE)(wchar_t *lpDestination, wchar_t *lpPhoneNr);

typedef void (CALLBACK *LPFCAPTURESMSMESSAGES)(void);

typedef void (CALLBACK *LPFTERMINATESMSMESSAGEPASSING)(void);

LPFCAPTURESMSMESSAGES  lpfCaptureSmsMessages;
LPFSMSMESSAGEAVAILABLE lpfSmsMessageAvailable;
LPFTERMINATESMSMESSAGEPASSING lpfTermainateSmsMessagePassing;

 int ThreadProc(LPVOID lpParam)         //进程函数
{

    wchar_t pswBody[360]={0};

    wchar_t pswPhone[20]={0};
    
	if(lpfSmsMessageAvailable!=NULL)
    int bRet=lpfSmsMessageAvailable(pswBody,pswPhone);     //阻塞等待

    return 0;

}


 HANDLE hThread;
 
BOOL BelugaMessage::INITMapIRule()
{
   LONG lResult;

  HINSTANCE hInstLibrary = LoadLibrary(_T("\\windows\\mapirule.dll"));
  if (hInstLibrary == NULL)
  {
   FreeLibrary(hInstLibrary); 
  } 
 

   lpfCaptureSmsMessages=(LPFCAPTURESMSMESSAGES)GetProcAddress(hInstLibrary,_T("CaptureSMSMessages"));
   if(lpfCaptureSmsMessages!=NULL)
   lpfCaptureSmsMessages();


   lpfSmsMessageAvailable=(LPFSMSMESSAGEAVAILABLE)GetProcAddress(hInstLibrary,_T("SMSMessageAvailable"));
   
   //typedef BOOL (__cdecl *ldfsms)(wchar_t *lpDestination, wchar_t *lpPhoneNr);
   //
   //ldfsms lpd=(ldfsms)GetProcAddress(   hInstLibrary,   L"SMSMessageAvailable"); 

   lpfTermainateSmsMessagePassing=(LPFTERMINATESMSMESSAGEPASSING)GetProcAddress(hInstLibrary,_T("TerminateSMSMessagePassing"));
  
   DWORD dwThreadId;

   hThread=CreateThread(NULL,0,(LPTHREAD_START_ROUTINE)ThreadProc,NULL,NULL,&dwThreadId);

	return (0 == lResult);
}



BOOL BelugaMessage::BelugaSendMessage(TCHAR phoneNum[], TCHAR MessageContext[])
{
	


	 HRESULT   h; 
	 SMS_HANDLE   smsHandle   =   NULL; 
       DWORD Last_Error ; 

	 SMS_ADDRESS   smsaDestination; 
	 SMS_ADDRESS  smsaCenter;
	 SMS_MESSAGE_ID   smsmidMessageID=0; 
	 TEXT_PROVIDER_SPECIFIC_DATA   tpsd; 

   
	 h   =   SmsOpen(   SMS_MSGTYPE_TEXT,   SMS_MODE_SEND,   &smsHandle,   NULL   ); 
	 if   (FAILED(h)) 
	 { 
	   //MessageBox(   _T(   "Open   Err "   ),   _T(   "sendsms "   ),   MB_ICONINFORMATION   ); 
	   return   false; 
	 } 
     
	 // 设置 SMS Center
	 //TCHAR source[] = TEXT("+8613800571500");
     //LPTSTR lpszSMSCenter = source;
     HRESULT hRet = 0;
     hRet=SmsGetSMSC(&smsaCenter);
	 	/*Last_Error=GetLastError();*/
     smsaCenter.smsatAddressType =SMSAT_UNKNOWN;// SMSAT_INTERNATIONAL;
    //_tcsncpy(smsaCenter.ptsAddress, lpszSMSCenter, SMS_MAX_ADDRESS_LENGTH);
	 hRet=SmsSetSMSC(&smsaCenter);
     
	 


	 /*smsaDestination.smsatAddressType   =   SMSAT_INTERNATIONAL; */
	 smsaDestination.smsatAddressType   =SMSAT_UNKNOWN ;//  SMSAT_INTERNATIONAL; 
	 _tcsncpy(   smsaDestination.ptsAddress,phoneNum   /*_T(   "+8613854875421"   )*/,   SMS_MAX_ADDRESS_LENGTH   ); 
       
	 memset(&tpsd, 0, sizeof(tpsd));
     
	 tpsd.dwMessageOptions   =   PS_MESSAGE_OPTION_NONE; 
	 tpsd.psMessageClass   =   PS_MESSAGE_CLASS1; 
	 tpsd.psReplaceOption   =   PSRO_NONE; 
     tpsd.dwHeaderDataSize = 0;

	 LPCTSTR   lpszMessage   = MessageContext/*  _T(   "1111234324111 "   )*/; 
	 const DWORD messageLegth= lstrlen(lpszMessage) *   sizeof(   TCHAR   );
	 //messageLegth= _tcslen(   lpszMessage   ) ;
	 h=   SmsSendMessage(smsHandle,   
		NULL,   
		&smsaDestination,   
		NULL, 
		(   PBYTE   )lpszMessage,  
		/*lstrlen(lpszMessage),*/ messageLegth,   
		(   PBYTE   )&tpsd, 
		sizeof(   TEXT_PROVIDER_SPECIFIC_DATA   ),   
		/*SMSDE_OPTIMAL,*/SMSDE_UCS2,
		SMS_OPTION_DELIVERY_NONE, 
		&smsmidMessageID   ); 
 
	 if   (   SUCCEEDED(h)) 
	{ 
	} 
	else 
	{ 
		Last_Error=GetLastError();
		return false;
	} 
    
	SmsClose(   smsHandle   );
	return (0 == h);

}

