/*
 ============================================================================
 Name		: CTagDb.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Tag Database
 ============================================================================
 */

#include <string.h>
#include <stdlib.h>
#include "Beluga.h"
#include "CTagIterator.h"
#include "CTag.h"


EXPORT_C CTagDb::CTagDb()
	{
	}
    
EXPORT_C CTagDb::~CTagDb()
	{
	}
    
EXPORT_C gint32 CTagDb::InitEntityDb(gchar* dbName) /* fill fields name */
	{
	strcpy(m_dbName, dbName);
	OpenDatabase();
	CppSQLite3Table tagTable = m_dbBeluga.getTable("select * from tag limit 1;");
	m_pFieldsName = g_ptr_array_sized_new(tagTable.numFields());
	if (!m_pFieldsName)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
			
	for (int i=0; i<tagTable.numFields(); i++)
		g_ptr_array_add(m_pFieldsName, g_string_new((gchar*)tagTable.fieldName(i)));
	
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CTagDb::GetMaxId(guint32 * nMaxId)
	{
	OpenDatabase();
	*nMaxId = m_dbBeluga.execScalar("select max(tid) from tag;");
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CTagDb::GetEntityById(guint32 nId, CDbEntity** ppEntity)
	{
	char sql[128] = {0};
	*ppEntity = NULL;
	
	OpenDatabase();
	sprintf(sql, "select * from tag where tid = %d;", nId);
	CppSQLite3Query query = m_dbBeluga.execQuery(sql);
	
	if (query.eof())
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
		}
	
	*ppEntity = new CTag(this);
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
    
EXPORT_C gint32 CTagDb::SaveEntity(CDbEntity * pEntity)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();
	try 
		{
		/* insert tag entity */
		strcpy(sql, "insert into tag values(?");
		for (i=0; i<TagField_EndFlag - 1; i++)
			strcat(sql, ", ?");
		strcat(sql, ");");
		
		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);
		for (i=0; i<TagField_EndFlag; i++)
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
    
EXPORT_C gint32 CTagDb::DeleteEntity(guint32 nEntityId)
	{
	char sql[128] = {0};
	OpenDatabase();
	sprintf(sql, "delete from tag where tid = %d;", nEntityId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CTagDb::UpdateEntity(CDbEntity * pEntity)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();
	
	try 
		{
		/* update tag entity */
		strcpy(sql, "update tag set "); 
		for (i=1; i<TagField_EndFlag; i++)
			{
			GString * fieldName = (GString*)g_ptr_array_index(m_pFieldsName, i);
			strcat(sql, fieldName->str);
			strcat(sql, " = ?");
			if (i != TagField_EndFlag - 1)
				strcat(sql, ", ");
			}
		strcat(sql, "where tid = ?;");
		
		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);
		
		GString * idValue = NULL;
		if (ECode_No_Error == pEntity->GetFieldValue(0, &idValue))
		{
			statement.bind(TagField_EndFlag, idValue->str);
			g_string_free(idValue, TRUE);
		}
		else
			statement.bindNull(TagField_EndFlag);

		for (i=1; i<TagField_EndFlag; i++)
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

IMPORT_C gint32 CTagDb::DeleteAllTags()
	{
	char sql[128] = {0};
	OpenDatabase();
	strcpy(sql, "delete from tag;");
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}

IMPORT_C gint32 CTagDb::GetAllTags(CTagIterator ** ppTagIterator)
	{
	char sql[128] = {0};
	OpenDatabase();
	
	strcpy(sql, "select * from tag order by tag_order asc;");	
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppTagIterator = NULL;
	*ppTagIterator = new CTagIterator(this);
	if (NULL == *ppTagIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;	
	}

IMPORT_C gint32 CTagDb::GetTagsTotality(guint32 * totality)
	{
	char sql[128] = {0};
	*totality = 0;
	OpenDatabase();
	
	strcpy(sql, "select count(*) from tag;");
	*totality = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	
	return 0;
	}

IMPORT_C gint32 CTagDb::CheckTagNameConflict(gchar * tagName, gboolean * bConflict)
	{
	char sql[128] = {0};
	guint32 count = 0;
	OpenDatabase();
	
	sprintf(sql, "select count(*) from tag where name like '%s';", tagName);
	count = m_dbBeluga.execScalar(sql);
	
	*bConflict = (count ? TRUE : FALSE); 
	CloseDatabase();
	
	return 0;  	
	}
