/*
 ============================================================================
 Name		: CGroupDb.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Group Database
 ============================================================================
 */

#include <string.h>
#include <stdlib.h>
#include "Beluga.h"
#include "CGroup.h"
#include "CGroupIterator.h"
#include "CGroupDb.h"


EXPORT_C CGroupDb::CGroupDb()
	{
	}
    
EXPORT_C CGroupDb::~CGroupDb()
	{
	}
    
EXPORT_C gint32 CGroupDb::InitEntityDb(gchar* dbName) /* fill fields name */
	{
	strcpy(m_dbName, dbName);
	OpenDatabase();
	CppSQLite3Table groupTable = m_dbBeluga.getTable("select * from cgroup limit 1;");
	m_pFieldsName = g_ptr_array_sized_new(groupTable.numFields());
	if (!m_pFieldsName)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
			
	for (int i=0; i<groupTable.numFields(); i++)
		g_ptr_array_add(m_pFieldsName, g_string_new((gchar*)groupTable.fieldName(i)));
	
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CGroupDb::GetMaxId(guint32 * nMaxId)
	{
	OpenDatabase();
	*nMaxId = m_dbBeluga.execScalar("select max(gid) from cgroup;");
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CGroupDb::GetEntityById(guint32 nId, CDbEntity** ppEntity)
	{
	char sql[128] = {0};
	*ppEntity = NULL;
	
	OpenDatabase();
	sprintf(sql, "select * from cgroup where gid = %d;", nId);
	CppSQLite3Query query = m_dbBeluga.execQuery(sql);
	
	if (query.eof())
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
		}
	
	*ppEntity = new CGroup(this);
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
    
EXPORT_C gint32 CGroupDb::SaveEntity(CDbEntity * pEntity)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();
	try 
		{
		/* insert group entity */
		strcpy(sql, "insert into cgroup values(NULL");
		for (i=0; i<GroupField_EndFlag - 1; i++)
			strcat(sql, ", ?");
		strcat(sql, ");");
		
		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);
		for (i=1; i<GroupField_EndFlag; i++)
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
				
		delete pEntity;
		pEntity = NULL;
		CloseDatabase();
		return 0;
		}
	catch(CppSQLite3Exception& e)
		{
		delete pEntity;
		pEntity = NULL;
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Insert_Failed);
		}
	
	return 0;
	}
    
EXPORT_C gint32 CGroupDb::DeleteEntity(guint32 nEntityId)
	{
	char sql[128] = {0};
	OpenDatabase();
	sprintf(sql, "delete from cgroup where gid = %d;", nEntityId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CGroupDb::UpdateEntity(CDbEntity * pEntity)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();
	
	try 
		{
		/* update group entity */
		strcpy(sql, "update group set "); 
		for (i=1; i<GroupField_EndFlag; i++)
			{
			GString * fieldName = (GString*)g_ptr_array_index(m_pFieldsName, i);
			strcat(sql, fieldName->str);
			strcat(sql, " = ?");
			if (i != GroupField_EndFlag - 1)
				strcat(sql, ", ");
			}
		strcat(sql, "where gid = ?;");
		
		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);

		GString * idValue = NULL;
		if (ECode_No_Error == pEntity->GetFieldValue(0, &idValue))
		{
			statement.bind(GroupField_EndFlag, idValue->str);
			g_string_free(idValue, TRUE);
		}
		else
			statement.bindNull(GroupField_EndFlag);

		for (i=1; i<GroupField_EndFlag; i++)
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
		
		delete pEntity;
		pEntity = NULL;
		CloseDatabase();
		return 0;
		}
	catch(CppSQLite3Exception& e)
		{
		delete pEntity;
		pEntity = NULL;
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Update_Failed);
		}
		
	return 0;
	}
   
gboolean CGroupDb::CheckGroupInTag(guint32 nGroupId, guint32 nTagId)
	{
	char sql[128] = {0};
	guint32 count = 0;
	OpenDatabase();
	
	sprintf(sql, "select count(*) from cgroup where tid = %d and gid = %d;", nTagId, nGroupId);
	count = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	
	return (count ? TRUE : FALSE); 
	}

EXPORT_C gint32 CGroupDb::DeleteAllGroupsByTag(guint32 nTagId)
	{
	char sql[128] = {0};
	OpenDatabase();
	sprintf(sql, "delete from cgroup where tid = %d;", nTagId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}
  
EXPORT_C gint32 CGroupDb::ReleaseGroupAllRelations(guint32 nGroupId)
	{
	char sql[128] = {0};
	OpenDatabase();
	sprintf(sql, "delete from r_contact_group where gid = %d;", nGroupId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CGroupDb::ReleaseGroupAllRelations(CGroup * pGroup)
	{
	GString * GroupId = NULL;
	pGroup->GetFieldValue(GroupField_Id, &GroupId);
	ReleaseGroupAllRelations(atoi(GroupId->str));
	g_string_free(GroupId, TRUE);
	return 0;
	}

EXPORT_C gint32 CGroupDb::GetContactRelationGroups(guint32 nContactId, CGroupIterator ** ppGroupIterator)
	{
	char sql[256] = {0};
	OpenDatabase();
	
	sprintf(sql, "select * from cgroup where gid in (select gid from r_contact_group where cid = %d) order by name_spell asc;", nContactId);
		
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppGroupIterator = NULL;
	*ppGroupIterator = new CGroupIterator(this);
	if (NULL == *ppGroupIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CGroupDb::GetAllGroupsByTag(guint32 nTagId, CGroupIterator ** ppGroupIterator)
	{
	char sql[512] = {0};
	OpenDatabase();
	
	sprintf(sql, "select * from cgroup where tid = %d order by name_spell asc;", nTagId);	
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppGroupIterator = NULL;
	*ppGroupIterator = new CGroupIterator(this);
	if (NULL == *ppGroupIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CGroupDb::GetGroupsTotalityByTag(guint32 nTagId, guint32 *totality)
	{
	char sql[128] = {0};
	*totality = 0;
	OpenDatabase();
	
	sprintf(sql, "select count(*) from cgroup where tid = %d;", nTagId);
	*totality = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	
	return 0;
	}

EXPORT_C gint32 CGroupDb::CheckGroupNameConflict(gchar * groupName, gboolean * bConflict)
	{
	char sql[128] = {0};
	guint32 count = 0;
	OpenDatabase();
	
	sprintf(sql, "select count(*) from cgroup where name like '%s';", groupName);
	count = m_dbBeluga.execScalar(sql);
	
	*bConflict = (count ? TRUE : FALSE); 
	CloseDatabase();
	
	return 0;  
	}
