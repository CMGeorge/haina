#include "StdAfx.h"
#include "BelugaMessage.h"

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

//EXTERN_C void CaptureSMSMessages (void)
//
//{
//
//      g_hClientEvent = CreateEvent(NULL, FALSE, FALSE, _T("SMSAvailableEvent"));
//
//      ASSERT(g_hClientEvent != NULL);
//
//      g_hClientMutex = CreateMutex(NULL, FALSE, _T("SMSDataMutex"));
//
//      ASSERT(g_hClientMutex != NULL);
//
// 
//
//      g_hClientMMObj = CreateFileMapping((HANDLE)-1, NULL, PAGE_READWRITE, 0, MMBUFSIZE, TEXT("SmsBuffer"));
//
//      ASSERT(g_hClientMMObj != NULL);
//
//	  
//
//     g_pClientSmsBuffer = (PSMS_BUFFER)MapViewOfFile(g_hClientMMObj, FILE_MAP_WRITE, 0, 0, 0);
//
//      if (g_pClientSmsBuffer == NULL) {
//
//             CloseHandle(g_hClientMMObj);
//
//      }
//
//      ASSERT(g_pClientSmsBuffer != NULL);
//
// 
//
//}
//
//
//void TerminateSMSMessagePassing (void)
//{
// // Make sure to have one last empty string available to copy to the client.
// memset(g_pClientSmsBuffer, 0, sizeof(SMS_BUFFER));
//
// SetEvent(g_hClientEvent);    // optionally allow the calling application to  return from GetData.
// CloseHandle(g_hClientEvent);
// CloseHandle(g_hClientMutex);
//
// if (g_pClientSmsBuffer) {
//  UnmapViewOfFile(g_pClientSmsBuffer);
//  g_pClientSmsBuffer = NULL;
// }
// if (g_hClientMMObj) {
//  CloseHandle(g_hClientMMObj);
//  g_hClientMMObj = NULL;
// }
//}
//
//BOOL SMSMessageAvailable (wchar_t *lpDestination, wchar_t *lpPhoneNr)
//{
// WaitForSingleObject(g_hClientEvent, INFINITE);
//
// if (g_pClientSmsBuffer != NULL) {
//  WaitForSingleObject(g_hClientMutex, INFINITE);
//  lstrcpy(lpPhoneNr, g_pClientSmsBuffer->g_szPhoneNr);
//  lstrcpy(lpDestination, g_pClientSmsBuffer->g_szMessageBody);
//  ReleaseMutex(g_hClientMutex);
// } else {
//  *lpPhoneNr = '\0';
//  *lpDestination = '\0';
// }
// return *lpPhoneNr != '\0';
//}



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

BOOL BelugaMessage::SendMessageW(TCHAR phoneNum[], TCHAR MessageContext[])
{
	 LONG lResult;
	
	return (0 == lResult);

}