// LoadContactDlg.cpp : implementation file
//
#define INITGUID
#include "stdafx.h"
#include <initguid.h>
#include <pimstore.h>
//#include <pimstoreex.h>

#undef INITGUID
#include "LoadContact.h"
#include "LoadContactDlg.h"

#define PHONE_FIRST _T("1111111")
#define PHONE_SECOND _T("2222222")
#define PHONE_THIRD _T("3333333")
#define PHONE_RESTRICTION_SECOND _T("[BusinessTelephoneNumber] = \"2222222\"")
#define PHONE_RESTRICTION_NOTSECOND _T("[BusinessTelephoneNumber] <> \"2222222\"")
#define PHONE_RESTRICTION_NOTTHIRD _T("[BusinessTelephoneNumber] <> \"3333333\"")
#define SIMONLY_RESTRICTION _T("[ContactType] = 1")
#define NOSIM_RESTRICTION _T("[ContactType] <> 1")


#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#define RELEASE(lpUnk) do \
	{ if ((lpUnk) != NULL) { (lpUnk)->Release(); (lpUnk) = NULL; } } while (0)
// CLoadContactDlg dialog
#include <simmgr.h>


bool GetContactFromSim(void)
{
	HRESULT result = S_OK;
	HSIM hSim;
	SIMPHONEBOOKENTRY phonebookEntry;
	DWORD dwContactCnt=10;
	DWORD dwBufferSize;
	SIMPHONEBOOKENTRYEX simEntries;
	result = SimInitialize(0, NULL, NULL, &hSim);
	if (result != S_OK)
	{
		// Handle error here
	}
	//SIM_PBSTORAGE_EMERGENCY			Emergency dial list.

	//SIM_PBSTORAGE_FIXEDDIALING		SIM fixed dialing list.

	//SIM_PBSTORAGE_LASTDIALING		SIM last dialing list.

	//SIM_PBSTORAGE_OWNNUMBERS		SIM own numbers lists.

	//SIM_PBSTORAGE_SIM				General SIM Storage.

	//result=SimReadPhonebookEntries(hSim,SIM_PBSTORAGE_SIM,1,&dwContactCnt,&simEntries,&dwBufferSize); 
	result = SimReadPhonebookEntry(hSim, SIM_PBSTORAGE_SIM, 1, &phonebookEntry);
	if (result != S_OK)
	{
		return false;
		// Handle error here
	}

	// Do something with the phone book entry here

	result = SimDeinitialize(hSim);
	if (result != S_OK)
	{
		// Handle error here
	}
	return true;
}
bool LoadContact(void)
{
	HRESULT                 hr = S_OK;
	IDispatch      * pDispatch = NULL;
	IContact        * pContact = NULL;

	IPOutlookApp      * polApp = NULL;
	IFolder          * pFolder = NULL;

	//IPOlItems3  * polItemsBase = NULL,
	//	* polItemsBase2 = NULL;

	IPOutlookItemCollection * polDefaultCollection        = NULL,
		* polDefaultCollection2       = NULL,
		* polRestrictedNotSecondPhone = NULL,
		* polRestrictedOnlyFirstPhone = NULL,
		* polRestrictedSimOnly        = NULL,
		* polRestrictedSecondPhone    = NULL,
		* polRestrictedNoSim          = NULL;

	// Logon to POOM.
	CoInitializeEx(NULL, COINIT_MULTITHREADED);
	hr = CoCreateInstance(CLSID_Application, NULL, CLSCTX_INPROC_SERVER, IID_IPOutlookApp, (LPVOID *) &polApp);
	hr = polApp->Logon(NULL);
	// Create three Device Contacts.
	
	hr = polApp->CreateItem(olContactItem, &pDispatch);
	hr = pDispatch->QueryInterface(IID_IContact, (void **)&pContact);
	hr = pContact->put_FirstName(_T("Device Contact 1"));
	hr = pContact->put_BusinessTelephoneNumber(PHONE_FIRST);
	hr = pContact->Save();
	pDispatch = NULL;
	pContact = NULL;

	hr = polApp->CreateItem(olContactItem, &pDispatch);
	hr = pDispatch->QueryInterface(IID_IContact, (void **)&pContact);
	hr = pContact->put_FirstName(_T("Device Contact 2"));
	hr = pContact->put_BusinessTelephoneNumber(PHONE_SECOND);
	hr = pContact->Save();
	pDispatch = NULL;
	pContact = NULL;

	hr = polApp->CreateItem(olContactItem, &pDispatch);
	hr = pDispatch->QueryInterface(IID_IContact, (void **)&pContact);
	hr = pContact->put_FirstName(_T("Device Contact 3"));
	hr = pContact->put_BusinessTelephoneNumber(PHONE_THIRD);
	hr = pContact->Save();
	pDispatch = NULL;
	pContact = NULL;

	// Create two SIM Contacts.
	hr = polApp->CreateItem(olContactItem, &pDispatch);
	hr = pDispatch->QueryInterface(IID_IContact, (void **)&pContact);
	hr = pContact->put_FirstName(_T("Sim Contact 1"));
	hr = pContact->put_BusinessTelephoneNumber(PHONE_FIRST);
	hr = pContact->Save();
	pDispatch = NULL;
	pContact = NULL;

	hr = polApp->CreateItem(olContactItem, &pDispatch);
	hr = pDispatch->QueryInterface(IID_IContact, (void **)&pContact);
	hr = pContact->put_FirstName(_T("Sim Contact 2"));
	hr = pContact->put_BusinessTelephoneNumber(PHONE_SECOND);
	hr = pContact->Save();

	RELEASE(pContact);
	RELEASE(pDispatch);
	pDispatch = NULL;
	pContact = NULL;

	// Get the default Contacts Collection (which contains no SIM Contacts).
	hr = polApp->GetDefaultFolder(olFolderContacts, &pFolder);
	hr = pFolder->get_Items(&polDefaultCollection);

	// Include SIM contacts in the Contacts Collection (both Device and SIM Contacts).
//	hr = polDefaultCollection->QueryInterface(IID_IPOlItems3, (void**)&polItemsBase);
//	hr = polItemsBase->IncludeSimContacts();

	// Restrict the collection to just the SIM Contacts.
//	hr = polItemsBase->Restrict(SIMONLY_RESTRICTION, &polRestrictedSimOnly);

	// Restrict the collection to just the Device Contacts.
//	hr = polItemsBase->Restrict(NOSIM_RESTRICTION, &polRestrictedNoSim);

	// Restrict the collection to just the 2nd Contacts.
//	hr = polItemsBase->Restrict(PHONE_RESTRICTION_SECOND, &polRestrictedSecondPhone);

	// Restriction placed over default SIM restriction - only #1 Device Contact.
	hr = pFolder->get_Items(&polDefaultCollection2);
	hr = polDefaultCollection2->Restrict(PHONE_RESTRICTION_NOTSECOND, &polRestrictedNotSecondPhone);
	hr = polRestrictedNotSecondPhone->Restrict(PHONE_RESTRICTION_NOTTHIRD, &polRestrictedOnlyFirstPhone);

	// Include SIM Contacts in the already restricted set - Only #1 Contacts.
//	hr = polRestrictedOnlyFirstPhone->QueryInterface(IID_IPOlItems3, (void**)&polItemsBase2);
//	hr = polItemsBase2->IncludeSimContacts();
//	hr = ShowCollection(polItemsBase2, _T("All contacts with the first phone number:"));

	RELEASE(polDefaultCollection);
	RELEASE(polDefaultCollection2);
	RELEASE(polRestrictedSimOnly);
	RELEASE(polRestrictedNoSim);
	RELEASE(polRestrictedSecondPhone);
//	RELEASE(polItemsBase);
	RELEASE(pFolder);
	RELEASE(polApp);

	return !SUCCEEDED(hr);

}
CLoadContactDlg::CLoadContactDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CLoadContactDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CLoadContactDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CLoadContactDlg, CDialog)
#if defined(_DEVICE_RESOLUTION_AWARE) && !defined(WIN32_PLATFORM_WFSP)
	ON_WM_SIZE()
#endif
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


// CLoadContactDlg message handlers

BOOL CLoadContactDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	GetContactFromSim();
LoadContact();
	// TODO: Add extra initialization here
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

#if defined(_DEVICE_RESOLUTION_AWARE) && !defined(WIN32_PLATFORM_WFSP)
void CLoadContactDlg::OnSize(UINT /*nType*/, int /*cx*/, int /*cy*/)
{
	if (AfxIsDRAEnabled())
	{
		DRA::RelayoutDialog(
			AfxGetResourceHandle(), 
			this->m_hWnd, 
			DRA::GetDisplayMode() != DRA::Portrait ? 
			MAKEINTRESOURCE(IDD_LOADCONTACT_DIALOG_WIDE) : 
			MAKEINTRESOURCE(IDD_LOADCONTACT_DIALOG));
	}
}
#endif

