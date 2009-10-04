/*
 ============================================================================
 Name		: CContactDb.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Contact Database
 ============================================================================
 */

#include <string.h>
#include <stdlib.h>
#include <time.h>
#include "CContact.h"
#include "CGroup.h"
#include "CPhoneContact.h"
#include "CIMContact.h"
#include "CContactDb.h"
#include "CContactIterator.h"

GLREF_C void GetLocalTime(tm*);

static void insert_contact_ext_row(gpointer key, gpointer value, gpointer user_data)
	{
	char sql[128] = {0};
	guint32 nContactId = 0;
	CPhoneContact * contact = (CPhoneContact*)user_data;
	contact->GetEntityDb()->GetMaxId(&nContactId);
	
	sprintf(sql, "insert into contact_ext values(NULL, %d, %d, '%s');", 
			nContactId, *((guint32*)key), (char*)value);
	
	contact->GetEntityDb()->GetDatabase()->execDML(sql);
	}

static void update_contact_ext_row(gpointer key, gpointer value, gpointer user_data)
	{
	char sql[128] = {0};
	GString * fieldValue = NULL;
	CPhoneContact * contact = (CPhoneContact*)user_data;
	contact->GetFieldValue(ContactField_Id, &fieldValue);
	
	sprintf(sql, "insert into contact_ext values(NULL, %d, %d, '%s');", 
			atoi(fieldValue->str), *((guint32*)key), (char*)value);
	g_string_free(fieldValue, TRUE);
	contact->GetEntityDb()->GetDatabase()->execDML(sql);
	}


EXPORT_C CContactDb::CContactDb()
	{
	m_nDbErrCode = ECode_No_Error;
	}
    
EXPORT_C CContactDb::~CContactDb()
	{
	}
    
EXPORT_C gint32 CContactDb::InitEntityDb(gchar* dbName) /* fill fields name */
	{
	strcpy(m_dbName, dbName);
	OpenDatabase();
	CppSQLite3Table contactTable = m_dbBeluga.getTable("select * from contact limit 1;");
	m_pFieldsName = g_ptr_array_sized_new(contactTable.numFields());
	if (!m_pFieldsName)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
			
	for (int i=0; i<contactTable.numFields(); i++)
		g_ptr_array_add(m_pFieldsName, g_string_new((gchar*)contactTable.fieldName(i)));
	
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CContactDb::GetMaxId(guint32 * nMaxId)
	{
	OpenDatabase();
	*nMaxId = m_dbBeluga.execScalar("select max(cid) from contact;");
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CContactDb::GetEntityById(guint32 nId, CDbEntity** ppEntity)
	{
	char sql[128] = {0};
	*ppEntity = NULL;
	
	OpenDatabase();
	sprintf(sql, "select * from contact where cid = %d;", nId);
	CppSQLite3Query query = m_dbBeluga.execQuery(sql);
	
	if (query.eof())
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
		}
	
	if (ContactType_Phone == query.getIntField(ContactField_Type))
		*ppEntity = new CPhoneContact(this, FALSE);
	else
		*ppEntity = new CIMContact(this);
	if (NULL == *ppEntity)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
	
	for (int i=0; i<query.numFields(); i++)
		{
		GString * fieldValue = g_string_new(query.fieldValue(i));
		(*ppEntity)->SetFieldValue(i, fieldValue);
		g_string_free(fieldValue, TRUE);
		}
		
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CContactDb::SaveEntity(CDbEntity * pEntity)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();
	try 
		{
		m_dbBeluga.execDML("begin transaction;");
		/* insert contact entity */
		strcpy(sql, "insert into contact values(NULL");
		for (i=0; i<ContactField_EndFlag - 1; i++)
			strcat(sql, ", ?");
		strcat(sql, ");");
		
		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);
		for (i=1; i<ContactField_EndFlag; i++)
			{
			GString * fieldValue = NULL;
			if (ECode_No_Error == pEntity->GetFieldValue(i, &fieldValue))
				{
				statement.bind(i, fieldValue->str);
				g_string_free(fieldValue, TRUE);
				}
			else
				statement.bindNull(i);
			}
		statement.execDML();
		statement.reset();
		
		/* insert contact_ext entity */
		CContact * contact = (CContact*)pEntity;
		GString * fieldValue = NULL;
		contact->GetFieldValue(ContactField_Type, &fieldValue);
		if (ContactType_Phone == atoi(fieldValue->str))
			{
			CPhoneContact * phonecontact = (CPhoneContact*)pEntity;
			/* insert phones */
			GHashTable * phones = NULL;
			phonecontact->GetAllPhones(&phones);
			g_hash_table_foreach(phones, insert_contact_ext_row, phonecontact);
			g_hash_table_destroy(phones);
			
			/* insert emails */
			GHashTable * emails = NULL;
			phonecontact->GetAllEmails(&emails);
			g_hash_table_foreach(emails, insert_contact_ext_row, phonecontact);
			g_hash_table_destroy(emails);
			
			/* insert ims */
			GHashTable * ims = NULL;
			phonecontact->GetAllIMs(&ims);
			g_hash_table_foreach(ims, insert_contact_ext_row, phonecontact);
			g_hash_table_destroy(ims);
			
			/* insert addresses */
			GPtrArray * addresses = NULL;
			phonecontact->GetAllAddresses(&addresses);
			for (guint32 j=0; j<addresses->len; j++)
				{
				guint32 nContactId = 0;
				memset(sql, 0, sizeof(sql));
				GetMaxId(&nContactId);
				stAddress * addr = (stAddress*)g_ptr_array_index(addresses, j);
				sprintf(sql, "insert into address values(NULL, %d, '%s', '%s', '%s', '%s', '%s', '%s');", 
									addr->atype, addr->block, addr->street, addr->district,
									addr->city, addr->state, addr->country, addr->postcode);
				m_dbBeluga.execDML(sql);
				
				guint32 nAddrId = m_dbBeluga.execScalar("select max(aid) from address;");
				memset(sql, 0, sizeof(sql));
				sprintf(sql, "insert into contact_ext values(NULL, %d, %d, %d);", 
									nContactId, addr->atype, nAddrId);
				m_dbBeluga.execDML(sql);
				}
			freeAddressArray(addresses);
			}
		g_string_free(fieldValue, TRUE);		
		
		m_dbBeluga.execDML("commit transaction;");
		delete pEntity;
		pEntity = NULL;
		CloseDatabase();
		return 0;
		}
	catch(CppSQLite3Exception& e)
		{
		m_dbBeluga.execDML("rollback transaction;");
		delete pEntity;
		pEntity = NULL;
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Insert_Failed);
		}
	
	return 0;
	}
    
EXPORT_C gint32 CContactDb::DeleteEntity(guint32 nEntityId)
	{
	char sql[128] = {0};
	OpenDatabase();
	sprintf(sql, "delete from contact where cid = %d;", nEntityId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CContactDb::UpdateEntity(CDbEntity * pEntity)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();
	
	try 
		{
		m_dbBeluga.execDML("begin transaction;");
		/* update contact entity */
		strcpy(sql, "update contact set "); 
		for (i=1; i<ContactField_EndFlag; i++)
			{
			GString * fieldName = (GString*)g_ptr_array_index(m_pFieldsName, i);
			strcat(sql, fieldName->str);
			strcat(sql, " = ?");
			if (i != ContactField_EndFlag - 1)
				strcat(sql, ", ");
			}
		strcat(sql, "where cid = ?;");
		
		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);

		GString * idValue = NULL;
		if (ECode_No_Error == pEntity->GetFieldValue(0, &idValue))
		{
			statement.bind(ContactField_EndFlag, idValue->str);
			g_string_free(idValue, TRUE);
		}
		else
			statement.bindNull(ContactField_EndFlag);

		for (i=1; i<ContactField_EndFlag; i++)
			{
			GString * fieldValue = NULL;
			if (ECode_No_Error == pEntity->GetFieldValue(i, &fieldValue))
				{
				statement.bind(i, fieldValue->str);
				g_string_free(fieldValue, TRUE);
				}
			else
				statement.bindNull(i);
			}
		statement.execDML();
		statement.reset();
		
		/* update contact_ext entity */
		CContact * contact = (CContact*)pEntity;
		GString * fieldValue = NULL;
		contact->GetFieldValue(ContactField_Type, &fieldValue);
		if (ContactType_Phone == atoi(fieldValue->str))
			{
			CPhoneContact * phonecontact = (CPhoneContact*)pEntity;
			GString * fieldId = NULL;
			phonecontact->GetFieldValue(ContactField_Id, &fieldId);
			memset(sql, 0, sizeof(sql));
			sprintf(sql, "delete from contact_ext where cid = %d;", atoi(fieldId->str));
			m_dbBeluga.execDML(sql);
			
			/* insert phones */
			GHashTable * phones = NULL;
			phonecontact->GetAllPhones(&phones);
			g_hash_table_foreach(phones, update_contact_ext_row, phonecontact);
			g_hash_table_destroy(phones);
			
			/* insert emails */
			GHashTable * emails = NULL;
			phonecontact->GetAllEmails(&emails);
			g_hash_table_foreach(emails, update_contact_ext_row, phonecontact);
			g_hash_table_destroy(emails);
			
			/* insert ims */
			GHashTable * ims = NULL;
			phonecontact->GetAllIMs(&ims);
			g_hash_table_foreach(ims, update_contact_ext_row, phonecontact);
			g_hash_table_destroy(ims);
			
			/* insert addresses */
			GPtrArray * addresses = NULL;
			phonecontact->GetAllAddresses(&addresses);
			for (guint32 j=0; j<addresses->len; j++)
				{
				memset(sql, 0, sizeof(sql));
				stAddress * addr = (stAddress*)g_ptr_array_index(addresses, j);
				sprintf(sql, "insert into address values(NULL, %d, '%s', '%s', '%s', '%s', '%s', '%s');", 
									addr->atype, addr->block, addr->street, addr->district,
									addr->city, addr->state, addr->country, addr->postcode);
									
				guint32 nAddrId = m_dbBeluga.execScalar("select max(aid) from address;");
				memset(sql, 0, sizeof(sql));
				sprintf(sql, "insert into contact_ext values(NULL, %d, %d, %d);", 
									atoi(fieldId->str), addr->atype, nAddrId);
				m_dbBeluga.execDML(sql);
				}
			freeAddressArray(addresses);
			g_string_free(fieldId, TRUE);
			}
		g_string_free(fieldValue, TRUE);
		
		m_dbBeluga.execDML("commit transaction;");
		delete pEntity;
		pEntity = NULL;
		CloseDatabase();
		return 0;
		}
	catch(CppSQLite3Exception& e)
		{
		m_dbBeluga.execDML("rollback transaction;");
		delete pEntity;
		pEntity = NULL;
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Update_Failed);
		}
		
	return 0;
	}
    
EXPORT_C gint32 CContactDb::DeleteAllContactsByTag(guint32 nTagId)
	{
	char sql[256] = {0};
	OpenDatabase();
	sprintf(sql, "delete from contact where cid in (select cid from r_contact_group where gid in "\
				 "(select gid from cgroup where tid= %d)); ", nTagId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CContactDb::ReleaseContactGroupRelation(CContact * pContact, CGroup * pGroup)
	{
	GString * ContactId = NULL;
	GString * GroupId = NULL;
	pContact->GetFieldValue(ContactField_Id, &ContactId);
	pGroup->GetFieldValue(GroupField_Id, &GroupId);
	ReleaseContactGroupRelation(atoi(ContactId->str), atoi(GroupId->str));
	g_string_free(ContactId, TRUE);
	g_string_free(GroupId, TRUE);
	return 0;
	}
    
EXPORT_C gint32 CContactDb::ReleaseContactGroupRelation(guint32 nContactId, guint32 nGroupId)
	{
	char sql[128] = {0};
	OpenDatabase();
	sprintf(sql, "delete from r_contact_group where cid = %d and gid = %d;", nContactId, nGroupId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CContactDb::ReleaseContactAllRelations(guint32 nContactId)
	{
	char sql[128] = {0};
	OpenDatabase();
	sprintf(sql, "delete from r_contact_group where cid = %d;", nContactId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CContactDb::ReleaseContactAllRelations(CContact * pContact)
	{
	GString * ContactId = NULL;
	pContact->GetFieldValue(ContactField_Id, &ContactId);
	ReleaseContactAllRelations(atoi(ContactId->str));
	g_string_free(ContactId, TRUE);
	return 0;
	}

EXPORT_C gint32 CContactDb::CreateContactGroupRelation(CContact * pContact, CGroup * pGroup)
	{
	GString * ContactId = NULL;
	GString * GroupId = NULL;
	pContact->GetFieldValue(ContactField_Id, &ContactId);
	pGroup->GetFieldValue(GroupField_Id, &GroupId);
	CreateContactGroupRelation(atoi(ContactId->str), atoi(GroupId->str));
	g_string_free(ContactId, TRUE);
	g_string_free(GroupId, TRUE);
	return 0;
	}
    
EXPORT_C gint32 CContactDb::CreateContactGroupRelation(guint32 nContactId, guint32 nGroupId)
	{
	char sql[128] = {0};
	OpenDatabase();
	sprintf(sql, "insert into r_contact_group values(%d, %d);", nContactId, nGroupId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CContactDb::SearchContactsByTag(guint32 nTagId, GArray * fieldsIndex, GPtrArray * fieldsValue, 
							gboolean onlyPref, CContactIterator ** ppContactIterator)
	{
	char sql[512] = {0};
	OpenDatabase();
	
	if (onlyPref || nTagId != ContactType_Phone)
		strcpy(sql, "select * from contact c where ");
	else
		strcpy(sql, "select c.*, ext.* from contact c "\
					"left join (select ce.cid ,ce.comm_key, ce.comm_value, a.* from contact_ext ce "\
					"left join address a on ce.comm_value = a.aid) ext "\
					"on c.cid = ext.cid where ");
		
	for (guint32 i=0; i<fieldsIndex->len; i++)
		for (guint32 j=ContactField_UserId; j<ContactField_EndFlag; j++)
			{
			if (g_array_index(fieldsIndex, guint32, i) == j)
				{
				strcat(sql, ((GString*)g_ptr_array_index(m_pFieldsName, j))->str);
				strcat(sql, " like '%");
				strcat(sql, ((GString*)g_ptr_array_index(fieldsValue, i))->str);
				strcat(sql, "%' and");				
				}
			}
	
	strcat(sql, "c.cid >= 0 ");
	
	if (-1 != m_nSortFieldIndex)
		{
		strcat(sql, "order by c.");
		strcat(sql, ((GString*)g_ptr_array_index(m_pFieldsName, m_nSortFieldIndex))->str);
		strcat(sql, " asc;");
		}
		
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppContactIterator = NULL;
	*ppContactIterator = new CContactIterator(this, onlyPref);
	if (NULL == *ppContactIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CContactDb::SearchPhoneContactsByPhoneOrEmail(gchar * commValue, 
							gboolean onlyPref, CContactIterator ** ppContactIterator)
	{
	char sql[512] = {0};
	OpenDatabase();
	
	if (onlyPref)
		strcpy(sql, "select * from contact c where cid in (select cid from contact_ext where comm_value like '%");
	else
		strcpy(sql, "select c.*, ext.* from contact c "\
					"left join (select ce.cid ,ce.comm_key, ce.comm_value, a.* from contact_ext ce "\
					"left join address a on ce.comm_value = a.aid) ext "\
					"on c.cid = ext.cid where c.cid in (select cid from contact_ext where comm_value like '%");
	
	strcat(sql, commValue);
	strcat(sql, "%') order by c.name_spell asc;");				
		
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppContactIterator = NULL;
	*ppContactIterator = new CContactIterator(this, onlyPref);
	if (NULL == *ppContactIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CContactDb::SearchContactsByName(guint32 nTagId, gchar* name, gboolean onlyPref, 
											CContactIterator ** ppContactIterator)
	{
	char sql[512] = {0};
	OpenDatabase();
	
	if (nTagId != ContactType_Phone)
		sprintf(sql, "select * from contact where nickname_spell like '%%%s%%'and type = %d order by nickname_spell asc;", name, nTagId);
	else if (onlyPref)
		sprintf(sql, "select * from contact where name_spell like '%%%s%%'and type = %d order by name_spell asc;", name, nTagId);
	else
		sprintf(sql, "select c.*, ext.* from contact c "\
					"left join (select ce.cid ,ce.comm_key, ce.comm_value, a.* from contact_ext ce "\
					"left join address a on ce.comm_value = a.aid) ext "\
					"on c.cid = ext.cid where name_spell like '%%%s%%'and type = %d order by name_spell asc;", name, nTagId);
							
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppContactIterator = NULL;
	*ppContactIterator = new CContactIterator(this, onlyPref);
	if (NULL == *ppContactIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;	
	}
/*
EXPORT_C gint32 CContactDb::SearchContactsByWordsFirstLetter(guint32 nTagId, gchar* nameLetters, gboolean onlyPref, 
    											CContactIterator ** ppContactIterator)
	{
	char sql[512] = {0};
	OpenDatabase();
	
	if (nTagId != ContactType_Phone)
		sprintf(sql, "select * from contact where nickname_spell like '%%%s%%' order by nickname_spell asc;", nameLetters);
	else if (onlyPref)
		sprintf(sql, "select * from contact where name_spell like '%%%s%%' order by name_spell asc;", nameLetters);
	else
		sprintf(sql, "select c.*, ext.* from contact c "\
					"left join (select ce.cid ,ce.comm_key, ce.comm_value, a.* from contact_ext ce "\
					"left join address a on ce.comm_value = a.aid) ext "\
					"on c.cid = ext.cid where name_spell like '%%%s%%' order by name_spell asc;", nameLetters);
		
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppContactIterator = NULL;
	*ppContactIterator = new CContactIterator(this, onlyPref);
	if (NULL == *ppContactIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;
	}   
    
EXPORT_C gint32 CContactDb::GetMostMatchingContactByTag(guint32 nTagId, GArray * fieldsIndex, GPtrArray * fieldsValue, 
									gboolean onlyPref, CContact ** ppContact)
	{
	char sql[512] = {0};
	OpenDatabase();
	
	if (onlyPref || nTagId != ContactType_Phone)
		strcpy(sql, "select * from contact where ");
	else
		strcpy(sql, "select c.*, ext.* from contact c "\
					"left join (select ce.cid ,ce.comm_key, ce.comm_value, a.* from contact_ext ce "\
					"left join address a on ce.comm_value = a.aid) ext "\
					"on c.cid = ext.cid where ");
		
	for (int i=0; i<fieldsIndex->len; i++)
		for (int j=ContactField_UserId; j<ContactField_EndFlag; j++)
			{
			if (g_array_index(fieldsIndex, guint32, i) == j)
				{
				strcat(sql, g_ptr_array_index(m_pFieldsName, j)->str));
				strcat(sql, " like '%");
				strcat(sql, g_ptr_array_index(fieldsValue, i)->str));
				strcat(sql, "%' and");				
				}
			}
	
	strcat(sql, "c.cid >= 0 ");
	
	if (-1 != m_nSortFieldIndex)
		{
		strcat(sql, "order by c.");
		strcat(sql, g_ptr_array_index(m_pFieldsName, m_nSortFieldIndex)->str));
		strcat(sql, " asc;");
		}
		
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppContactIterator = NULL;
	*ppContactIterator = new CContactIterator(this, onlyPref);
	if (NULL == *ppContactIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;	
	}
*/
EXPORT_C gint32 CContactDb::GetMostMatchingPhoneContactByPhoneOrEmail(gchar * commValue, 
									CContact ** ppContact)
	{
	gint32 ret;
	guint32 nContactId;
	guint32 nMostMatchingId = (guint32)-1;
	guint8 nBaseLen = 7, nLimitLen = 12;
	guint8 nMatchingBit = CONTACT_PHONE_PREF_LEN;
	char sql[512] = {0};
	
	*ppContact = NULL;
	OpenDatabase();
	
	char * email = strrchr(commValue, '@'); /* simply check commValue is email */
	if (email != NULL) /* email */
		{
		strcpy(sql, "select c.cid, ext.comm_value from contact c left join contact_ext ext on c.cid = ext.cid "\
						"where ext.comm_key & 32 = 32 and ext.comm_value like '");
		strcat(sql, commValue);
		strcat(sql, "' order by c.name_spell asc;");	
		}
	else   /* phone */
		{
		guint8 nPhoneLen = strlen(commValue);
		strcpy(sql, "select c.cid, ext.comm_value from contact c left join contact_ext ext on c.cid = ext.cid "\
						"where (ext.comm_key & 16 = 16) and (ext.comm_value like '");
		if (nPhoneLen < nBaseLen)
			strcat(sql, commValue);
		else
			{
			guint8 i = 0;
			strcat(sql, "%");

			gchar* tmp = commValue;
			if (nPhoneLen > nLimitLen) 
				{
				tmp = commValue + nPhoneLen - nLimitLen;
				nPhoneLen = nLimitLen;
				}

			strcat(sql, tmp + i);
			i++;

			while (i < nPhoneLen - nBaseLen) 
				{
				strcat(sql, "' or ext.comm_value like '%");
				strcat(sql, tmp + i);
				i++;
				}
			}
		strcat(sql, "') order by c.name_spell asc;");
		}
			
	CppSQLite3Query query = m_dbBeluga.execQuery(sql);
	while (!query.eof())
		{
		nContactId = query.getIntField(0);
		const gchar * fieldValue = query.getStringField(1);
		
		if (NULL == fieldValue)  /* query error */
			break; 
		
		/* get the most matching contact */
		char * email = strrchr(commValue, '@'); /* simply check commValue is email */
		if (email != NULL) /* email */
			{
			if (0 == strcmp(commValue, fieldValue)) /* email equal */
				{
				nMostMatchingId = nContactId;
				break;
				}
			}
		else   /* phone */
			{
			guint32 i = strlen(commValue);
			guint32 j = strlen(fieldValue);
			
			while (i && j && (commValue[i - 1] == fieldValue[j - 1]) )
				{
				i--; j--;
				
				if (0 == j && 0 == i) /* perfect same phone .*/
					{ 
					nMostMatchingId = nContactId;
					break;
					}
				
				if (i < nMatchingBit) /* i left less, matching most */
					{ 
					nMatchingBit = i;
					nMostMatchingId = nContactId; /* record the most matching contact id */
					}
				}
			
				if (0 == j && 0 == i) /* perfect same tel */
					break; 
			}			        
		
		query.nextRow();
		}
	
	CDbEntity * pEntity = NULL;
	ret = GetEntityById(nMostMatchingId, &pEntity);
	if (0 == ret)
		*ppContact = (CContact*)pEntity;

	CloseDatabase();	
	return ret;
	}
    
EXPORT_C gint32 CContactDb::GetAllContactsByTag(guint32 nTagId, gboolean onlyPref, CContactIterator ** ppContactIterator)
	{
	char sql[512] = {0};
	OpenDatabase();
	
	if (onlyPref || nTagId != ContactType_Phone)
		sprintf(sql, "select * from contact c where c.type = %d ", nTagId);
	else
		sprintf(sql, "select c.*, ext.* from contact c "\
					"left join (select ce.cid ,ce.comm_key, ce.comm_value, a.* from contact_ext ce "\
					"left join address a on ce.comm_value = a.aid) ext "\
					"on c.cid = ext.cid where c.type = %d ", nTagId);
	
	if (-1 != m_nSortFieldIndex)
		{
		strcat(sql, "order by c.");
		strcat(sql, ((GString*)g_ptr_array_index(m_pFieldsName, m_nSortFieldIndex))->str);
		strcat(sql, " asc;");
		}
		
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppContactIterator = NULL;
	*ppContactIterator = new CContactIterator(this, onlyPref);
	if (NULL == *ppContactIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CContactDb::GetAllContactsNotInGroupByTag(guint32 nTagId, gboolean onlyPref, CContactIterator ** ppContactIterator)
	{
	char sql[512] = {0};
	OpenDatabase();
	
	if (onlyPref || nTagId != ContactType_Phone)
		sprintf(sql, "select * from contact c where c.cid not in (select cid from r_contact_group) and c.type = %d ", nTagId);
	else
		sprintf(sql, "select c.*, ext.* from contact c "\
					"left join (select ce.cid ,ce.comm_key, ce.comm_value, a.* from contact_ext ce "\
					"left join address a on ce.comm_value = a.aid) ext "\
					"on c.cid = ext.cid where c.cid not in (select cid from r_contact_group) and c.type = %d ", nTagId);
		
	if (-1 != m_nSortFieldIndex)
		{
		strcat(sql, "order by c.");
		strcat(sql, ((GString*)g_ptr_array_index(m_pFieldsName, m_nSortFieldIndex))->str);
		strcat(sql, " asc;");
		}
		
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppContactIterator = NULL;
	*ppContactIterator = new CContactIterator(this, onlyPref);
	if (NULL == *ppContactIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CContactDb::GetAllContactsByGroup(guint32 nGroupId, gboolean onlyPref, CContactIterator ** ppContactIterator)
	{
	char sql[512] = {0};
	OpenDatabase();
	
	if (onlyPref || !CheckGroupInTag(nGroupId, ContactType_Phone))
		sprintf(sql, "select * from contact c, r_contact_group r where r.gid = %d and ", nGroupId);
	else
		sprintf(sql, "select c.*, ext.* from contact c, r_contact_group r "\
					"left join (select ce.cid ,ce.comm_key, ce.comm_value, a.* from contact_ext ce "\
					"left join address a on ce.comm_value = a.aid) ext "\
					"on c.cid = ext.cid where r.gid = %d and ", nGroupId);
	
	strcat(sql, "c.cid = r.cid ");
	if (-1 != m_nSortFieldIndex)
		{
		strcat(sql, "order by c.");
		strcat(sql, ((GString*)g_ptr_array_index(m_pFieldsName, m_nSortFieldIndex))->str);
		strcat(sql, " asc;");
		}
		
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppContactIterator = NULL;
	*ppContactIterator = new CContactIterator(this, onlyPref);
	if (NULL == *ppContactIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CContactDb::GetContactsTotalityByTag(guint32 nTagId, guint32 * totality)
	{
	char sql[128] = {0};
	*totality = 0;
	OpenDatabase();
	
	sprintf(sql, "select count(*) from contact where type = %d;", nTagId);
	*totality = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	
	return 0;
	}
    
EXPORT_C gint32 CContactDb::GetContactsTotalityByGroup(guint32 nGroupId, guint32 * totality)
	{
	char sql[128] = {0};
	*totality = 0;
	OpenDatabase();
	
	sprintf(sql, "select count(cid) from r_contact_group where gid = %d;", nGroupId);
	*totality = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	
	return 0;
	}
    
EXPORT_C gint32 CContactDb::GetPhoneDistrict(gchar* phoneNumber,  stPhoneDistrict * phoneDistrict)
	{
	char sql[256] = {0};
	OpenDatabase();
	
	phoneDistrict->phoneRange = NULL;
	sprintf(sql, "select * from phone_district where phone_range like '%%%s%%';", phoneNumber);
	CppSQLite3Query query = m_dbBeluga.execQuery(sql);
	if (query.eof())
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	}

	strcpy(phoneDistrict->districtNumber, query.getStringField(1));
	strcpy(phoneDistrict->districtName, query.getStringField(2));
	strcpy(phoneDistrict->ownerState, query.getStringField(3));
	strcpy(phoneDistrict->feeType, query.getStringField(5));
	strcpy(phoneDistrict->weatherCode, query.getStringField(7));
	phoneDistrict->updateFlag = query.getIntField(6);
		
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CContactDb::SavePhoneDistrict(stPhoneDistrict * phoneDistrict)
	{
	GString * sql = NULL;
	OpenDatabase();
	
	sql = g_string_new_len("", 256);
	g_string_printf(sql, "insert into phone_district values(null, '%s', '%s', '%s', '%s', '%s', %d, '%s');", 
				phoneDistrict->districtNumber, phoneDistrict->districtName, phoneDistrict->ownerState,
				phoneDistrict->phoneRange->str, phoneDistrict->feeType, phoneDistrict->updateFlag, 
				phoneDistrict->weatherCode);

	m_dbBeluga.execDML(sql->str);	
	CloseDatabase();
	g_string_free(sql, TRUE);

	return 0;
	}

EXPORT_C gint32 CContactDb::GetMaxPhoneDistrictUpdateFlag(guint16 * updateFlag)
	{
	*updateFlag = m_dbBeluga.execScalar("select max(update_flag) from phone_district;");
	return 0;
	}

EXPORT_C gint32 CContactDb::GetRecentContacts(GPtrArray ** pContacts)
	{
	char sql[64] = {0};
	char times[20] = {0};
//	time_t t;
	
//	time(&t); 
	*pContacts = NULL;
	*pContacts = g_ptr_array_new();
	if (*pContacts == NULL)
		{
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
	
	OpenDatabase();
	strcpy(sql, "select * from recent_contact;");
	CppSQLite3Query query = m_dbBeluga.execQuery(sql);
	while (!query.eof())
		{
		stRecentContact * recentContact = (stRecentContact*)g_malloc0(sizeof(stRecentContact));
		if (recentContact == NULL)
			{
			CloseDatabase();
			return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
			}
		
		recentContact->nContactId = query.getIntField(1);
		recentContact->event = (EContactEvent)query.getIntField(2);
		strcpy(recentContact->eventCommInfo, query.getStringField(3));
		strcpy(times, query.getStringField(4));  /* exp: 2009-6-30 21:51:23 */
 		//recentContact->time = localtime(&t); 
		GetLocalTime(&recentContact->time);
		char * tmp = strrchr(times, '-');
		recentContact->time.tm_mon = atoi(tmp+1);
		tmp = strrchr(tmp, '-');
		recentContact->time.tm_mday = atoi(tmp+1);
		tmp = strrchr(tmp, ' ');
		recentContact->time.tm_hour = atoi(tmp+1);
		tmp = strrchr(tmp, ':');
		recentContact->time.tm_min = atoi(tmp+1);
		tmp = strrchr(tmp, ':');
		recentContact->time.tm_sec = atoi(tmp+1);
		
		g_ptr_array_add(*pContacts, recentContact);
		query.nextRow();
		}
	
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CContactDb::SaveRecentContact(stRecentContact * contact)
	{
	char sql[256] = {0};
	OpenDatabase();
	if (m_dbBeluga.execScalar("select count(*) from recent_contact;") > MAX_RECENT_CONTACT_NUM)
		{
		m_dbBeluga.execDML("delete from recent_contact where time = (select min(time) from recent_contact);");
		}
	
	sprintf(sql, "select count(*) from recent_contact where cid = %d;", contact->nContactId);
	if (m_dbBeluga.execScalar(sql) != 0)
		{
		memset(sql, 0, 256);
		sprintf(sql, "update recent_contact set event = %d, event_comm = '%s', time = '%d-%d-%d %02d:%02d:%02d' where cid = %d;", 
			contact->event, contact->eventCommInfo,
			contact->time.tm_year, contact->time.tm_mon, contact->time.tm_mday,
			contact->time.tm_hour, contact->time.tm_min, contact->time.tm_sec, contact->nContactId);
		}
		else
		{
		memset(sql, 0, 256);
		sprintf(sql, "insert into recent_contact values(null, %d, %d, '%s', %d-%d-%d %02d:%02d:%02d);", 
			contact->nContactId, contact->event, contact->eventCommInfo,
			contact->time.tm_year, contact->time.tm_mon, contact->time.tm_mday,
			contact->time.tm_hour, contact->time.tm_min, contact->time.tm_sec);
		}

	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}

gboolean CContactDb::IsInRecentContact(guint32 nContactId)
	{
	char sql[256] = {0};
	OpenDatabase();
	sprintf(sql, "select count(*) from recent_contact where cid = %d;", nContactId);
	if (m_dbBeluga.execScalar(sql) == 0)
		{
		CloseDatabase();
		return FALSE;
		}
	else
		{
		CloseDatabase();
		return TRUE;
		}
	}

tm & CContactDb::GetAarliestContactTime(guint32 nContactId)
	{
	tm stTime;
	char sql[64] = {0};
	char times[20] = {0};
	OpenDatabase();

	if (nContactId == -1)
		sprintf(sql, "select min(time) from recent_contact;");
	else
		sprintf(sql, "select time from recent_contact where cid = %d;", nContactId);

	CppSQLite3Query query = m_dbBeluga.execQuery(sql);
	
	GetLocalTime(&stTime);
	if (query.eof())
		{
		CloseDatabase();
		return stTime;
		}
	
	strcpy(times, query.getStringField(0));  /* exp: 2009-6-30 21:51:23 */
	if (strlen(times) != 0)
		{	
		char * tmp = strrchr(times, '-');
		stTime.tm_mon = atoi(tmp+1);
		tmp = strrchr(tmp, '-');
		stTime.tm_mday = atoi(tmp+1);
		tmp = strrchr(tmp, ' ');
		stTime.tm_hour = atoi(tmp+1);
		tmp = strrchr(tmp, ':');
		stTime.tm_min = atoi(tmp+1);
		tmp = strrchr(tmp, ':');
		stTime.tm_sec = atoi(tmp+1);
		CloseDatabase();
		}
	return stTime;
	}


gint32 CContactDb::GetContactCommInfo(CPhoneContact * pContact)
	{
	char sql[128] = {0};
	GHashTable * hashTable = pContact->getCommInfoHashTable();
	GPtrArray * ptrArray = pContact->getAddressPtrArray();
	
	GString * CidField = NULL;
	OpenDatabase();
	
	/* fill pref comm info to hashtable */
	GString * fieldValue = NULL;
	pContact->GetFieldValue(ContactField_PhonePref, &fieldValue);
	if (fieldValue->len) 
		pContact->SetPhone((ECommType)(CommType_Pref | CommType_Phone), fieldValue->str);
	g_string_free(fieldValue, TRUE);
	
	pContact->GetFieldValue(ContactField_EmailPref, &fieldValue);
	if (fieldValue->len) 
		pContact->SetEmail((ECommType)(CommType_Pref | CommType_Email), fieldValue->str);
	g_string_free(fieldValue, TRUE);
	
	pContact->GetFieldValue(ContactField_IMPref, &fieldValue);
	if (fieldValue->len) 
		pContact->SetIM((ECommType)(CommType_Pref | CommType_IM), fieldValue->str);
	g_string_free(fieldValue, TRUE);
	
	pContact->GetFieldValue(ContactField_Id, &CidField);
	sprintf(sql, "select ce.comm_key, ce.comm_value, a.* from contact_ext ce "\
					"left join address a on ce.comm_value = a.aid where ce.cid = %s;", CidField->str);
	g_string_free(CidField, TRUE);
	CppSQLite3Query query = m_dbBeluga.execQuery(sql);
	while (!query.eof())
		{
		ECommType commType = (ECommType)query.getIntField(0); /* comm_key field */ 
		switch(commType & 0xF0) 
			{
			case CommType_Phone:
				pContact->SetPhone(commType, (gchar*)query.getStringField(1));	
				break;
			case CommType_Email:
				pContact->SetEmail(commType, (gchar*)query.getStringField(1));
				break;
			case CommType_Address:
				{
				stAddress addr;
				addr.aid = query.getIntField(1);
				addr.atype = commType;
				g_stpcpy(addr.block, query.getStringField(3));
				g_stpcpy(addr.street, query.getStringField(4));
				g_stpcpy(addr.district, query.getStringField(5));
				g_stpcpy(addr.city, query.getStringField(6));
				g_stpcpy(addr.state, query.getStringField(7));
				g_stpcpy(addr.country, query.getStringField(8));
				g_stpcpy(addr.postcode, query.getStringField(9));
				
				pContact->SetAddress(commType, &addr);
				}
				break;
			case CommType_IM:
				pContact->SetIM(commType, (gchar*)query.getStringField(1));
				break;
			default:
				break;
			}
				
		query.nextRow();
		}
	
	CloseDatabase();
	return 0;
	}

gint32 CContactDb::GetMaxContactCommId(guint32 * pMaxCommId)
	{
	*pMaxCommId = 0;
	OpenDatabase();
	*pMaxCommId = m_dbBeluga.execScalar("select max(ce_id) from contact_ext;");
	CloseDatabase();
	return 0;
	}
   
gboolean CContactDb::CheckGroupInTag(guint32 nGroupId, guint32 nTagId)
	{
	char sql[128] = {0};
	guint32 count = 0;
	OpenDatabase();
	
	sprintf(sql, "select count(*) from cgroup where tid = %d and gid = %d;", nTagId, nGroupId);
	count = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	
	return (count ? TRUE : FALSE); 
	}
