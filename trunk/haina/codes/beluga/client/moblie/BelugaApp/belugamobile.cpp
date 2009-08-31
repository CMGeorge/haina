
/* 
*  this class encapsulate the platform api
*/

#include "belugamobile.h"
#include "Pimstore.h"

BelugaWinMobile::Contact::Contact()
{
	m_pContact = new CContactDll();
	if (NULL != m_pContact)
	{
		m_pContact->init();
	}
}

BelugaWinMobile::Contact::~Contact()
{
	if (m_pContact)
		delete m_pContact;
	m_pContact = NULL;
}

gboolean BelugaWinMobile::Contact::importContacts(CContactDb * pContactDb)
{
	GList * pContactList = NULL;
	GList * pCurContact = NULL;
	GList * pLastContact = NULL;
	IContact * pIContact = NULL;
	
	m_pContact->getContact(&pContactList, 0, m_pContact->totalCount);
	pCurContact = g_list_first(pContactList);
	pLastContact = g_list_last(pContactList);
	
	do 
	{
		if (pCurContact == NULL)
			break;

		CPhoneContact* pContact = new CPhoneContact(pContactDb);

		/* set contact type */
		GString * value = g_string_new("1");  /* 1: ContactType_Phone */
		pContact->SetFieldValue(ContactField_Type, value);
		g_string_free(value, TRUE);
		
		pIContact = (IContact*)(pCurContact->data);
		value = g_string_new("");

		BSTR bstr;
		const wchar_t* wstr;
		char* cstr;

		/* set contact name */
		pIContact->get_FirstName(&bstr);
		wstr = (const wchar_t*)bstr;
		cstr = (char*)malloc(2 * wcslen(wstr));
		wcstombs(cstr, wstr, wcslen(wstr));
		g_string_sprintf(value, "%s", cstr);
		SysFreeString(bstr);
		free(cstr);

		pIContact->get_MiddleName(&bstr);
		wstr = (const wchar_t*)bstr;
		cstr = (char*)malloc(2 * wcslen(wstr));
		wcstombs(cstr, wstr, wcslen(wstr));
		g_string_sprintfa(value, "%s", cstr);
		SysFreeString(bstr);
		free(cstr);

		pIContact->get_LastName(&bstr);
		wstr = (const wchar_t*)bstr;
		cstr = (char*)malloc(2 * wcslen(wstr));
		wcstombs(cstr, wstr, wcslen(wstr));
		g_string_sprintfa(value, " %s", cstr);
		SysFreeString(bstr);
		free(cstr);

		pContact->SetFieldValue(ContactField_Name, value);
		g_string_free(value, TRUE);
		
		/* set contact sex, mobile contact has not sex field, default set male */
		value = g_string_new("0"); /* 0: male */
		pContact->SetFieldValue(ContactField_Sex, value);
		g_string_free(value, TRUE);
		
		/* name spell */
		/* nickname */
		/* nickname spell */

		/*
			ContactField_Photo,
			ContactField_Signature,
			ContactField_PhonePref,
			ContactField_EmailPref,
			ContactField_IMPref,
			ContactField_Birthday,
			ContactField_Org,
			ContactField_Url,
			ContactField_Ring,
			ContactField_Title,
			ContactField_Note,
			*/

		/* set mobile phone */ 
		pIContact->get_MobileTelephoneNumber(&bstr);
		wstr = (const wchar_t*)bstr;
		cstr = (char*)malloc(2 * wcslen(wstr));
		wcstombs(cstr, wstr, wcslen(wstr));
		value = g_string_new(cstr);
		SysFreeString(bstr);
		free(cstr);

		pContact->SetFieldValue(ContactField_PhonePref, value);
		g_string_free(value, TRUE);

		/* set work email */
		pIContact->get_Email1Address(&bstr);
		wstr = (const wchar_t*)bstr;
		cstr = (char*)malloc(2 * wcslen(wstr));
		wcstombs(cstr, wstr, wcslen(wstr));
		value = g_string_new(cstr);
		SysFreeString(bstr);
		free(cstr);

		pContact->SetFieldValue(ContactField_EmailPref, value);
		g_string_free(value, TRUE);

/*
		imtable = NULL;
		addrarray = NULL;
		guint32 commkey[2] = {0x45,0x46};

		imtable = g_hash_table_new_full(g_int_hash, g_int_equal, NULL, g_free);
		g_hash_table_insert(imtable, &commkey[0], g_strdup("82010953"));  //QQ
		g_hash_table_insert(imtable, &commkey[1], g_strdup("sherry.co@163.com")); //MSN

		addrarray = g_ptr_array_new();
		stAddress * pAddr = (stAddress*)g_malloc0(sizeof(stAddress));
		pAddr->aid = 1;
		pAddr->atype = CommType_Address | CommType_Home;
		g_stpcpy(pAddr->block, "1508号");  // 中文可能需要先做utf8转换
		g_stpcpy(pAddr->street, "梅家浜路");
		g_stpcpy(pAddr->district, "松江");
		g_stpcpy(pAddr->city, "上海");
		g_stpcpy(pAddr->state, "上海");
		g_stpcpy(pAddr->country, "中国");
		g_stpcpy(pAddr->postcode, "200233");
		g_ptr_array_add(addrarray, pAddr);

		pContact->SetIMs(imtable);
		pContact->SetAddresses(addrarray);

		g_hash_table_destroy(imtable);
		freeAddressArray(addrarray);
*/
		// 保存到数据库
		pContactDb->SaveEntity((CDbEntity*)pContact);
		pIContact->Release();

		pCurContact = g_list_next(pCurContact);
	} while(pCurContact != pLastContact);

	return TRUE;
}
