/*
 ============================================================================
 Name		: CTagIterator.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Tag Iterator
 ============================================================================
 */

#include "Beluga.h"
#include "CTagIterator.h"
#include "CTag.h"


EXPORT_C gint32 CTagIterator::Current(CDbEntity ** ppEntity)
	{
	if (ppEntity == NULL)
		return ERROR(ESide_Client, EModule_Db, ECode_Invalid_Param);
	
	*ppEntity = NULL;
	if (m_dbQuery->eof())
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	
	 /* fill cgroup table fields */
	*ppEntity = new CTag(m_pEntityDb);
	if (NULL == *ppEntity)
		{
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
	
	CTag * pTag = (CTag*)(*ppEntity);
	for (int i=0; i<TagField_EndFlag; i++)
		{
		GString * fieldValue = g_string_new(m_dbQuery->fieldValue(i));
		pTag->SetFieldValue(i, fieldValue);	
		g_string_free(fieldValue, TRUE);
		}
	
	return 0;
	}
 
EXPORT_C gint32 CTagIterator::Next(gboolean * pSuccess)
	{
	m_dbQuery->nextRow();
	*pSuccess = !m_dbQuery->eof();
	if (FALSE == *pSuccess)
		return ERROR(ESide_Client, EModule_Db, ECode_End_Of_Row);

	return 0; 
	}

EXPORT_C gint32 CTagIterator::Prev(gboolean * pSuccess)
	{
	*pSuccess = FALSE;
	return ERROR(ESide_Client, EModule_Db, ECode_Not_Implemented);
	}
