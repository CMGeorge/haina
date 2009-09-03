/*
 ============================================================================
 Name		: CConfigDb.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Config Database
 ============================================================================
 */

#include <string.h>
#include <stdlib.h>
#include "Beluga.h"
#include "CConfigIterator.h"
#include "CConfig.h"


EXPORT_C CConfigDb::CConfigDb()
	{
	}
    
EXPORT_C CConfigDb::~CConfigDb()
	{
	}
    
EXPORT_C gint32 CConfigDb::InitEntityDb(gchar* dbName) /* fill fields name */
	{
	strcpy(m_dbName, dbName);
	OpenDatabase();
	CppSQLite3Table configTable = m_dbBeluga.getTable("select * from config limit 1;");
	m_pFieldsName = g_ptr_array_sized_new(configTable.numFields());
	if (!m_pFieldsName)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
			
	for (int i=0; i<configTable.numFields(); i++)
		g_ptr_array_add(m_pFieldsName, g_string_new((gchar*)configTable.fieldName(i)));
	
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CConfigDb::GetMaxId(guint32 * nMaxId)
	{
	OpenDatabase();
	*nMaxId = m_dbBeluga.execScalar("select max(cid) from config;");
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CConfigDb::GetEntityById(guint32 nId, CDbEntity** ppEntity)
	{
	char sql[128] = {0};
	*ppEntity = NULL;
	
	OpenDatabase();
	sprintf(sql, "select * from config where cid = %d;", nId);
	CppSQLite3Query query = m_dbBeluga.execQuery(sql);
	
	if (query.eof())
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
		}
	
	*ppEntity = new CConfig(this);
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
    
EXPORT_C gint32 CConfigDb::SaveEntity(CDbEntity * pEntity)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();
	try 
		{
		/* insert config entity */
		strcpy(sql, "insert into config values(?");
		for (i=0; i<ConfigField_EndFlag - 1; i++)
			strcat(sql, ", ?");
		strcat(sql, ");");
		
		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);
		for (i=0; i<ConfigField_EndFlag; i++)
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
    
EXPORT_C gint32 CConfigDb::DeleteEntity(guint32 nEntityId)
	{
	char sql[128] = {0};
	OpenDatabase();
	sprintf(sql, "delete from config where cid = %d;", nEntityId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CConfigDb::UpdateEntity(CDbEntity * pEntity)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();
	
	try 
		{
		/* update config entity */
		strcpy(sql, "update config set "); 
		for (i=1; i<ConfigField_EndFlag; i++)
			{
			GString * fieldName = (GString*)g_ptr_array_index(m_pFieldsName, i);
			strcat(sql, fieldName->str);
			strcat(sql, " = ?");
			if (i != ConfigField_EndFlag - 1)
				strcat(sql, ", ");
			}
		strcat(sql, " where cid = ?;");
		
		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);

		GString * idValue = NULL;
		if (ECode_No_Error == pEntity->GetFieldValue(0, &idValue))
			{
			statement.bind(ConfigField_EndFlag, idValue->str);
			g_string_free(idValue, TRUE);
			}
		else
			statement.bindNull(ConfigField_EndFlag);

		for (i=1; i<ConfigField_EndFlag; i++)
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

EXPORT_C gint32 CConfigDb::DeleteAllConfigs()
	{
	char sql[64] = {0};
	OpenDatabase();
	strcpy(sql, "delete from config;");
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}
 
EXPORT_C gint32 CConfigDb::GetConfigByName(gchar * configName, CConfig** pConfig)
	{
	char sql[256] = {0};
	OpenDatabase();
	
	sprintf(sql, "select * from config where cfg_name like '%s';", configName);
	CppSQLite3Query query = m_dbBeluga.execQuery(sql);	
	if (query.eof())
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
		}
	
	*pConfig = new CConfig(this);
	if (NULL == *pConfig)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
	
	for (int i=0; i<query.numFields(); i++)
		(*pConfig)->SetFieldValue(i, g_string_new(query.fieldValue(i)));
		
	CloseDatabase();
	
	return 0;
	}

EXPORT_C gint32 CConfigDb::GetConfigByKey(guint32 configKey, CConfig** pConfig)
	{
		char sql[256] = {0};
		OpenDatabase();

		sprintf(sql, "select * from config where cfg_key = %d;", configKey);
		CppSQLite3Query query = m_dbBeluga.execQuery(sql);	
		if (query.eof())
		{
			CloseDatabase();
			return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
		}

		*pConfig = new CConfig(this);
		if (NULL == *pConfig)
		{
			CloseDatabase();
			return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

		for (int i=0; i<query.numFields(); i++)
			(*pConfig)->SetFieldValue(i, g_string_new(query.fieldValue(i)));

		CloseDatabase();

		return 0;
	}

EXPORT_C gint32 CConfigDb::GetConfigsTotality(guint32 *totality)
	{
	char sql[64] = {0};
	*totality = 0;
	OpenDatabase();
	
	strcpy(sql, "select count(*) from config;");
	*totality = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	return 0;
	}
    
EXPORT_C gint32 CConfigDb::GetAllConfigs(CConfigIterator ** ppConfigIterator)
	{
	char sql[128] = {0};
	OpenDatabase();
	
	strcpy(sql, "select * from config;");	
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppConfigIterator = NULL;
	*ppConfigIterator = new CConfigIterator(this);
	if (NULL == *ppConfigIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;	
	}
