/*
 ============================================================================
 Name		: CGroupIterator.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Group Iterator
 ============================================================================
 */

#include "Beluga.h"
#include "CGroupIterator.h"
#include "CGroup.h"


EXPORT_C gint32 CGroupIterator::Current(CDbEntity ** ppEntity)
	{
	if (ppEntity == NULL)
		return ERROR(ESide_Client, EModule_Db, ECode_Invalid_Param);
	
	*ppEntity = NULL;
	if (m_dbQuery->eof())
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	
	 /* fill cgroup table fields */
	*ppEntity = new CGroup(m_pEntityDb);
	if (NULL == *ppEntity)
		{
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
	
	CGroup * pGroup = (CGroup*)(*ppEntity);
	for (int i=0; i<GroupField_EndFlag; i++)
		{
		GString * fieldValue = g_string_new(m_dbQuery->fieldValue(i));
		pGroup->SetFieldValue(i, fieldValue);	
		g_string_free(fieldValue, TRUE);
		}
	
	return 0;
	}
 
EXPORT_C gint32 CGroupIterator::Next(gboolean * pSuccess)
	{
	m_dbQuery->nextRow();
	*pSuccess = !m_dbQuery->eof();
	if (FALSE == *pSuccess)
		return ERROR(ESide_Client, EModule_Db, ECode_End_Of_Row);

	return 0; 
	}

EXPORT_C gint32 CGroupIterator::Prev(gboolean * pSuccess)
	{
	*pSuccess = FALSE;
	return ERROR(ESide_Client, EModule_Db, ECode_Not_Implemented);
	}
