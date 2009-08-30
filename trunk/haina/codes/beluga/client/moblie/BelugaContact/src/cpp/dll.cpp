// Author JackyHo
// Create Date:2009.8.30
// Description:读取mobile6.0通讯录
// dll.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "dll.h"
#include <windows.h>
#include <commctrl.h>

#include <Pimstore.h>
#include <glib.h>

BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
    return TRUE;
}

// This is an example of an exported variable
DLL_API int ndll=0;

// This is an example of an exported function.
DLL_API int fndll(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see dll.h for the class definition
Cdll::Cdll()
{ 
	this->totalCount=0;
	return; 
}

/**
* Author:JackyHo
* Date:2009.8.30
* Description:读取设备上存储的联系人
* Return gboolean:方法执行是否成功
*/
gboolean Cdll::init(void)
{	
	this->totalCount=0;
	//var
	HRESULT hr;
	IPOutlookApp *polApp;

	// Initialize COM for Pocket Outlook.
	hr = CoInitializeEx(NULL, 0);
	if (FAILED(hr)) return FALSE;

	// Get the application object.
	hr = CoCreateInstance(CLSID_Application, 
		NULL, 
		CLSCTX_INPROC_SERVER,
		IID_IPOutlookApp, 
		(LPVOID*)&polApp);
	if (FAILED(hr)) return FALSE;

	// Log on to Pocket Outlook.
	hr = polApp->Logon(NULL);
	if (FAILED(hr)) return FALSE;

	//读取通讯录
	IPOutlookItemCollection * pItems;
	IFolder * pFolder;

	hr = polApp->GetDefaultFolder(olFolderContacts, &pFolder);
	if (FAILED(hr)) return FALSE;
	hr = pFolder->get_Items(&pItems);
	if (FAILED(hr)) return FALSE;
	int pnCount;
	hr = pItems->get_Count(&pnCount);
	if (FAILED(hr)) return FALSE;
	this->totalCount = pnCount;
	// Release objects.
	pItems->Release();
	pFolder->Release();
	// Log off and release the application object.
	polApp->Logoff();
	polApp->Release();
	return TRUE;
}



/**
* Author:JackyHo
* Date:2009.8.30
* Description:读取设备上存储的联系人，获取到的联系人放在pGListContact中
* Param GList *pGListContact:获取到的联系人放在该对象中
* Param guintOffset:获取联系人的偏移量，从0开始
* Param guintLen:从偏移量guintOffset开始读取联系人数量
* Return gboolean:方法执行是否成功
*/
gboolean Cdll::getContact(GList *pGListContact, guint guintOffset, guint guintLen) const
{
	if(guintOffset< 0||guintOffset< 0)
	{
		return FALSE;
	}
	if(guintLen==0)
	{
		return TRUE;
	}
	//var
	HRESULT hr;
	IPOutlookApp *polApp;

	// Initialize COM for Pocket Outlook.
	hr = CoInitializeEx(NULL, 0);
	if (FAILED(hr)) return FALSE;

	// Get the application object.
	hr = CoCreateInstance(CLSID_Application, 
		NULL, 
		CLSCTX_INPROC_SERVER,
		IID_IPOutlookApp, 
		(LPVOID*)&polApp);
	if (FAILED(hr)) return FALSE;

	// Log on to Pocket Outlook.
	hr = polApp->Logon(NULL);
	if (FAILED(hr)) return FALSE;

	//读取通讯录
	IPOutlookItemCollection * pItems;
	IFolder * pFolder;

	hr = polApp->GetDefaultFolder(olFolderContacts, &pFolder);
	if (FAILED(hr)) return FALSE;
	hr = pFolder->get_Items(&pItems);
	if (FAILED(hr)) return FALSE;
	int pnCount;
	hr = pItems->get_Count(&pnCount);
	if (FAILED(hr)) return FALSE;
	//安全性判断
	if(pnCount==0)
	{
		return TRUE;
	}
	gint count=0;
	if(guintOffset + guintLen>pnCount)
	{
		count = pnCount;
	}
	else
	{
		count = guintOffset + guintLen;
	}
	IContact *pContact = NULL;
	for(gint i=guintOffset;i<count;i++)
	{
		hr = pItems->Item(i+1,(IDispatch **)&pContact);
		if (FAILED(hr)) return FALSE;
		//储存联系人对象
		pGListContact = g_list_append(pGListContact, (gpointer)pContact);
		//pContact->Release();		
	}
	// Release objects.
	pItems->Release();
	pFolder->Release();
	// Log off and release the application object.
	polApp->Logoff();
	polApp->Release();
	return TRUE;
}
