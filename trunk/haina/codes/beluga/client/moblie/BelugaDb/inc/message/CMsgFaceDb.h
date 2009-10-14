/*
============================================================================
Name		: CMsgFaceDb.h
Author	  : shaochuan.yang
Copyright   : haina
Description : Msg Face Database
============================================================================
*/

#ifndef __CCONFIGDB_H__
#define __CCONFIGDB_H__

#include <glib.h>
#include "CEntityDb.h"
#include "CMsgFace.h"

class CMsgFaceIterator;

class CMsgFaceDb : public CEntityDb
{
public:
	IMPORT_C CMsgFaceDb();
	IMPORT_C ~CMsgFaceDb();

	IMPORT_C gint32 InitEntityDb(gchar* dbName);
	IMPORT_C gint32 GetMaxId(guint32 * nMaxId);
	IMPORT_C gint32 GetEntityById(guint32 nId, CDbEntity** ppEntity);

	IMPORT_C gint32 SaveEntity(CDbEntity * pEntity);
	IMPORT_C gint32 DeleteEntity(guint32 nEntityId);
	IMPORT_C gint32 UpdateEntity(CDbEntity * pEntity);

	IMPORT_C gint32 GetConfigByName(gchar * configName, CConfig** pConfig);
	IMPORT_C gint32 GetConfigByKey(guint32 configKey, CConfig** pConfig);

	IMPORT_C gint32 DeleteAllConfigs();

	IMPORT_C gint32 GetAllConfigs(CConfigIterator ** ppConfigIterator); 

	IMPORT_C gint32 GetConfigsTotality(guint32 * totality);
};

#endif
