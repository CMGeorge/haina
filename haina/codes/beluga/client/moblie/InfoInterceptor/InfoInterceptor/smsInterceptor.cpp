#include <stdafx.h>
#include "smsInterceptor.h"
#include <windows.h>
#include <sms.h>

/**
* Author:lws
* Date:2009.9.9
* Description:向注册表注册key，拦截短信
* Return gboolean:方法执行是否成功
*/
LRESULT smsInterceptor::RegSmsInterceptor()
{
	 LRESULT lr;
    HRESULT hr = E_FAIL;
    HKEY hKey = NULL;
    HKEY hSubKey = NULL;
    DWORD dwDisposition;
    TCHAR wszValue[20];

    // Set up registry keys
    // Register with COM:
    //    [HKEY_CLASSES_ROOT\CLSID\{3AB4C10E-673C-494c-98A2-CC2E91A48115}\InProcServer32]
    //    @="mapirule.dll"
	

    lr = RegCreateKeyEx(HKEY_CLASSES_ROOT, TEXT("\\CLSID\\{3AB4C10E-673C-494c-98A2-CC2E91A48115}"),
	                              0, NULL, 0, 0, NULL, 
	                              &hKey, &dwDisposition);
    if (lr != ERROR_SUCCESS)
    {
        goto Exit;
    }

    lr = RegCreateKeyEx(hKey, TEXT("InprocServer32"),
	                              0, NULL, 0, 0, NULL, 
	                              &hSubKey, &dwDisposition);
    if (lr != ERROR_SUCCESS)
    {
        goto Exit;
    }

    lstrcpy(wszValue, TEXT("mapirule.dll"));
    lr = RegSetValueEx(hSubKey, NULL, 0, REG_SZ, (LPBYTE) wszValue, (lstrlen(wszValue) + 1) * sizeof(TCHAR));
    if (lr != ERROR_SUCCESS)
    {
        goto Exit;
    }

    RegCloseKey(hSubKey);
    hSubKey = NULL;
    RegCloseKey(hKey);
    hKey = NULL;

    // Register with Inbox:
    //    [HKEY_LOCAL_MACHINE\Software\Microsoft\Inbox\Svc\SMS\Rules]
    //    {3AB4C10E-673C-494c-98A2-CC2E91A48115}"=dword:1

    lr = RegCreateKeyEx(HKEY_LOCAL_MACHINE, TEXT("\\Software\\Microsoft\\Inbox\\Svc\\SMS\\Rules"),
	                              0, NULL, 0, 0, NULL, 
	                              &hKey, &dwDisposition);
    if (lr != ERROR_SUCCESS)
    {
        goto Exit;
    }

    dwDisposition = 1;
    lr = RegSetValueEx(hKey, TEXT("{3AB4C10E-673C-494c-98A2-CC2E91A48115}"), 0, REG_DWORD, 
                          (LPBYTE) &dwDisposition, sizeof(DWORD));
    if (lr != ERROR_SUCCESS)
    {
        goto Exit;
    }
 
    hr = S_OK;

Exit:
    if (hSubKey)
    {
        RegCloseKey(hSubKey);
    }
    if (hKey)
    {
        RegCloseKey(hKey);
    }

    return hr;
}




/**
* Author:lws
* Date:2009.9.9
* Description:向注册表去除key
* Return gboolean:方法执行是否成功
*/
LRESULT smsInterceptor::unRegSmsInterceptor()
{
	HKEY hKey = NULL;
    HRESULT hr = E_FAIL;
    LRESULT lr;
    DWORD dwDisposition;

    // Delete registry keys
    RegDeleteKey(HKEY_CLASSES_ROOT, TEXT("\\CLSID\\{3AB4C10E-673C-494c-98A2-CC2E91A48115}"));
    
    lr = RegCreateKeyEx(HKEY_LOCAL_MACHINE, TEXT("\\Software\\Microsoft\\Inbox\\Svc\\SMS\\Rules"),
	                              0, NULL, 0, 0, NULL, 
	                              &hKey, &dwDisposition);
    if (lr != ERROR_SUCCESS)
    {
        goto Exit;
    }

    RegDeleteValue(hKey, TEXT("{3AB4C10E-673C-494c-98A2-CC2E91A48115}"));

    hr = S_OK;

Exit:
    if (hKey)
    {
        RegCloseKey(hKey);
    }

    return hr;
}





// ***************************************************************************
// Function Name: SendSMS
// Author:lws
// Purpose: Send an SMS Message
// Date:2009.9.17 
// Arguments: none
//
// Return Values: none
//
// Description:
//	Called after everything has been set up, this function merely opens an
//	SMS_HANDLE and tries to send the SMS Message.
 
void SendSMS(BOOL bSendConfirmation, BOOL bUseDefaultSMSC, LPCTSTR lpszSMSC, LPCTSTR lpszRecipient, LPCTSTR lpszMessage)
{
	SMS_HANDLE smshHandle;
	SMS_ADDRESS smsaSource;
	SMS_ADDRESS smsaDestination;
	TEXT_PROVIDER_SPECIFIC_DATA tpsd;
	SMS_MESSAGE_ID smsmidMessageID;

	// try to open an SMS Handle
	if(FAILED(SmsOpen(SMS_MSGTYPE_TEXT, SMS_MODE_SEND, &smshHandle, NULL)))
	{
		/*MessageBox(NULL,
					(LPCTSTR)LoadString(ghInstance, IDS_ERROR_SMSOPEN, 0, 0), 
					(LPCTSTR)LoadString(ghInstance, IDS_CAPTION_ERROR, 0, 0),
					MB_OK | MB_ICONERROR);*/
		return;
	}

	// Create the source address
	if(!bUseDefaultSMSC)
	{
		smsaSource.smsatAddressType = SMSAT_INTERNATIONAL;
		_tcsncpy(smsaSource.ptsAddress, lpszSMSC, SMS_MAX_ADDRESS_LENGTH);
	}

	// Create the destination address
	smsaDestination.smsatAddressType = SMSAT_INTERNATIONAL;
	_tcsncpy(smsaDestination.ptsAddress, lpszRecipient, SMS_MAX_ADDRESS_LENGTH);

	// Set up provider specific data
    memset(&tpsd, 0, sizeof(tpsd));
	tpsd.dwMessageOptions = bSendConfirmation ? PS_MESSAGE_OPTION_STATUSREPORT : PS_MESSAGE_OPTION_NONE;
	tpsd.psMessageClass = PS_MESSAGE_CLASS1;
	tpsd.psReplaceOption = PSRO_NONE;
	tpsd.dwHeaderDataSize = 0;

	// Send the message, indicating success or failure
	if(SUCCEEDED(SmsSendMessage(smshHandle, ((bUseDefaultSMSC) ? NULL : &smsaSource), 
								 &smsaDestination, NULL, (PBYTE) lpszMessage, 
								 _tcslen(lpszMessage) * sizeof(TCHAR), (PBYTE) &tpsd, 
								 sizeof(TEXT_PROVIDER_SPECIFIC_DATA), SMSDE_OPTIMAL, 
								 SMS_OPTION_DELIVERY_NONE, &smsmidMessageID)))
	{
		/*MessageBox(NULL,
					(LPCTSTR)LoadString(ghInstance, IDS_SMSSENT, 0, 0), 
					(LPCTSTR)LoadString(ghInstance, IDS_CAPTION_SUCCESS, 0, 0),
					MB_OK);*/
	}
	else
	{
		/*MessageBox(NULL,
					(LPCTSTR)LoadString(ghInstance, IDS_ERROR_SMSSEND, 0, 0), 
					(LPCTSTR)LoadString(ghInstance, IDS_CAPTION_ERROR, 0, 0),
					MB_OK | MB_ICONERROR);*/
	}

	// clean up
	VERIFY(SUCCEEDED(SmsClose(smshHandle)));
}

