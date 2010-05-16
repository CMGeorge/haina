#include "stdafx.h"
#include "glib.h"
#include "CContactDb.h"
#include "CContactIterator.h"
#include "CPhoneContact.h"
#include "CIMContact.h"


void  get_fields_name(gpointer data, gpointer user_data);
void print_hashtable_value(gpointer key, gpointer value, gpointer user_data);
void print_address(gpointer data, gpointer user_data);

void ContactTest()
{
	CContactDb * pContactDb = NULL;
	gint32 ret;

	pContactDb = new CContactDb();
	if (NULL == pContactDb)
	{
		printf("create contactdb instance error.\n");
		return;
	}

#ifdef _WIN32_WCE
	pContactDb->InitEntityDb("\\Program Files\\Beluga\\beluga.db"); /* 初始化联系人数据库 */
#else
	pContactDb->InitEntityDb("..\\..\\beluga.db"); /* 初始化联系人数据库 */
#endif
	
	guint16 updateFlag = 0;
	pContactDb->GetMaxPhoneDistrictUpdateFlag(&updateFlag);	
	return;

	/************************************ 1. 获取PhoneContact 列表 *****************/
	// IMPORT_C gint32 GetAllContactsByTag(guint32 nTagId, gboolean onlyPref, CContactIterator ** ppContactIterator);
	CContactIterator * pContactIterator = NULL;
	ret = pContactDb->GetAllContactsByTag(ContactType_Phone, TRUE, &pContactIterator);
	if (ret != 0)
	{
		printf("get all phone contacts error.\n");
		return;
	}
	
	CPhoneContact * pContact = NULL;
	gboolean hasNext = FALSE;

	if (ECode_No_Error == pContactIterator->Current((CDbEntity**)&pContact))
	{
		// 打印phone contact 不带扩展通讯信息的各字段名
		GPtrArray * pFieldsName = NULL;
		pContact->GetFieldsName(&pFieldsName);
		
		g_ptr_array_foreach(pFieldsName, get_fields_name, NULL);  /* foreach callback */
		freeGStringArray(pFieldsName); 

	}
	printf("\n");

	// 输出 phone contact 不带扩展通讯信息的各字段值
	GString * pFieldValue = NULL;
	for (int i=ContactField_Id; i<ContactField_EndFlag; i++)
	{
		pContact->GetFieldValue(i, &pFieldValue);
		printf(" %s", pFieldValue->str);
		g_string_free(pFieldValue, TRUE);
	}
	printf("\n");

	delete pContact;
	pContact = NULL;

	while (0 == pContactIterator->Next(&hasNext) && hasNext)
	{
		
		if (ECode_No_Error == pContactIterator->Current((CDbEntity**)&pContact))
		{
			GString * pFieldValue = NULL;
			for (int i=ContactField_Id; i<ContactField_EndFlag; i++)
			{
				pContact->GetFieldValue(i, &pFieldValue);
				printf(" %s", pFieldValue->str);
				g_string_free(pFieldValue, TRUE);
			}
			printf("\n");
		}

		delete pContact;
		pContact = NULL;
	}
	
	delete pContactIterator;
	pContactIterator = NULL;
	/****************************************************************************/
	

	/************************************ 2. 根据电话号码或者email 获取最匹配的 PhoneContact *****************/
	// IMPORT_C gint32 GetMostMatchingPhoneContactByPhoneOrEmail(gchar * commValue, CContact ** ppContact)；

	pContact = NULL;
	ret = pContactDb->GetMostMatchingPhoneContactByPhoneOrEmail("16247363", (CContact**)&pContact);
	if (ret != 0)
	{
		printf("get most matching phone contact error.\n");
		return;
	}
	printf("\n");

	// 输出该 phone contact 各字段值
	for (int i=ContactField_Id; i<ContactField_EndFlag; i++)
	{
		pContact->GetFieldValue(i, &pFieldValue);
		printf(" %s", pFieldValue->str);
		g_string_free(pFieldValue, TRUE);
	}

	// 输出该 phone contact 各通讯信息字段值
	printf("\n --- ext comm info ----\n");
	GHashTable * phonetable = NULL;
	GHashTable * imtable = NULL;
	GHashTable * emailtable = NULL;
	GPtrArray * addrarray = NULL;
	pContact->GetAllPhones(&phonetable);
	pContact->GetAllIMs(&imtable);
	pContact->GetAllEmails(&emailtable);
	pContact->GetAllAddresses(&addrarray);

	// phone hash table
	g_hash_table_foreach(phonetable, print_hashtable_value, NULL);		
	g_hash_table_destroy(phonetable);

	// im hash table
	g_hash_table_foreach(imtable, print_hashtable_value, NULL);
	g_hash_table_destroy(imtable);

	// email hash table
	g_hash_table_foreach(emailtable, print_hashtable_value, NULL);
	g_hash_table_destroy(emailtable);

	// address ptr array
	g_ptr_array_foreach(addrarray, print_address, NULL);
	freeAddressArray(addrarray);

	delete pContact;
	pContact = NULL;
	printf("\n");
	/****************************************************************************/

	/************************************ 3. 创建phone contact并存入数据库 *****************/
	// IMPORT_C gint32 SaveEntity(CDbEntity * pEntity);
#if 0
	pContact = new CPhoneContact(pContactDb);
	
	// 设置该 phone contact 各字段值
	
	GString * value = g_string_new("1");
	pContact->SetFieldValue(ContactField_Type, value);
	g_string_free(value, TRUE);

	value = g_string_new("sherry");
	pContact->SetFieldValue(ContactField_Name, value);
	g_string_free(value, TRUE);

	value = g_string_new("1");
	pContact->SetFieldValue(ContactField_Sex, value);
	g_string_free(value, TRUE);

	value = g_string_new("sherry.co@163.com");
	pContact->SetFieldValue(ContactField_EmailPref, value);
	g_string_free(value, TRUE);

	value = g_string_new("15921961907");
	pContact->SetFieldValue(ContactField_PhonePref, value);
	g_string_free(value, TRUE);

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
	
	// 保存到数据库
	pContactDb->SaveEntity((CDbEntity*)pContact);
	//pContactDb->UpdateEntity((CDbEntity*)pContact);
	/****************************************************************************/
#endif

	/************************************ 4. 将 PhoneContact加入 group *****************/
	// IMPORT_C gint32 CreateContactGroupRelation(guint32 nContactId, guint32 nGroupId);
	pContactDb->CreateContactGroupRelation(1, 1);
	pContactDb->CreateContactGroupRelation(3, 1);
	pContactDb->CreateContactGroupRelation(1, 2);
	/****************************************************************************/

	/************************************ 5. 按照 group 获取 phonecontact *****************/
	// IMPORT_C gint32 GetAllContactsByGroup(guint32 nGroupId, gboolean onlyPref, CContactIterator ** ppContactIterator);
	pContactIterator = NULL;
	ret = pContactDb->GetAllContactsByGroup(1, TRUE, &pContactIterator);
	if (ret != 0)
	{
		printf("get group contacts error.\n");
		return;
	}
	
	pContact = NULL;
	hasNext = FALSE;

	if (ECode_No_Error == pContactIterator->Current((CDbEntity**)&pContact))
	{
		// 打印phone contact 不带扩展通讯信息的各字段名
		GPtrArray * pFieldsName = NULL;
		pContact->GetFieldsName(&pFieldsName);
		
		g_ptr_array_foreach(pFieldsName, get_fields_name, NULL);  /* foreach callback */
		freeGStringArray(pFieldsName); 

	}
	printf("\n");

	// 输出 phone contact 不带扩展通讯信息的各字段值
	pFieldValue = NULL;
	for (int i=ContactField_Id; i<ContactField_EndFlag; i++)
	{
		pContact->GetFieldValue(i, &pFieldValue);
		printf(" %s", pFieldValue->str);
		g_string_free(pFieldValue, TRUE);
	}
	printf("\n");

	delete pContact;
	pContact = NULL;

	while (0 == pContactIterator->Next(&hasNext) && hasNext)
	{
		
		if (ECode_No_Error == pContactIterator->Current((CDbEntity**)&pContact))
		{
			GString * pFieldValue = NULL;
			for (int i=ContactField_Id; i<ContactField_EndFlag; i++)
			{
				pContact->GetFieldValue(i, &pFieldValue);
				printf(" %s", pFieldValue->str);
				g_string_free(pFieldValue, TRUE);
			}
			printf("\n");
		}

		delete pContact;
		pContact = NULL;
	}
	
	delete pContactIterator;
	pContactIterator = NULL;
	/****************************************************************************/

	/************************************ 6. 按照 姓名 获取 phonecontact *****************/
	//     IMPORT_C gint32 SearchContactsByName(guint32 nTagId, gchar* name, gboolean onlyPref, 
	//										CContactIterator ** ppContactIterator);

	printf("get contacts by name .\n\n");
	pContactIterator = NULL;
	ret = pContactDb->SearchContactsByName(1, "sh", TRUE, &pContactIterator);
	if (ret != 0)
	{
		printf("get contacts by name error.\n");
		return;
	}
	
	pContact = NULL;
	hasNext = FALSE;

	if (ECode_No_Error == pContactIterator->Current((CDbEntity**)&pContact))
	{
		// 打印phone contact 不带扩展通讯信息的各字段名
		GPtrArray * pFieldsName = NULL;
		pContact->GetFieldsName(&pFieldsName);
		
		g_ptr_array_foreach(pFieldsName, get_fields_name, NULL);  /* foreach callback */
		freeGStringArray(pFieldsName); 

	}
	printf("\n");

	// 输出 phone contact 不带扩展通讯信息的各字段值
	pFieldValue = NULL;
	for (int i=ContactField_Id; i<ContactField_EndFlag; i++)
	{
		pContact->GetFieldValue(i, &pFieldValue);
		printf(" %s", pFieldValue->str);
		g_string_free(pFieldValue, TRUE);
	}
	printf("\n");

	delete pContact;
	pContact = NULL;

	while (0 == pContactIterator->Next(&hasNext) && hasNext)
	{
		
		if (ECode_No_Error == pContactIterator->Current((CDbEntity**)&pContact))
		{
			GString * pFieldValue = NULL;
			for (int i=ContactField_Id; i<ContactField_EndFlag; i++)
			{
				pContact->GetFieldValue(i, &pFieldValue);
				printf(" %s", pFieldValue->str);
				g_string_free(pFieldValue, TRUE);
			}
			printf("\n");
		}

		delete pContact;
		pContact = NULL;
	}
	
	delete pContactIterator;
	pContactIterator = NULL;
	/****************************************************************************/

	delete pContactDb;
	pContactDb = NULL;
}


void  get_fields_name(gpointer data, gpointer user_data)
{
	GString * pFieldName = (GString*)data;
	printf(" %s", pFieldName->str);
}

void print_hashtable_value(gpointer key, gpointer value, gpointer user_data)
{
	printf("%x-%s, ", *(guint32*)key, (gchar*)value);
}

void print_address(gpointer data, gpointer user_data)
	{
	stAddress * pAddr = (stAddress*)data;
	printf("%x-%s,%s,%s,%s,%s,%s,%s", pAddr->atype,pAddr->block, pAddr->street,
		 pAddr->district, pAddr->city, pAddr->state, pAddr->country, pAddr->postcode);
	}