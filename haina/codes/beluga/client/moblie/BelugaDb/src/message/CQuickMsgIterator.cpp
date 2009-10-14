/*
 ============================================================================
 Name		: CQuickMsgIterator.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Quick Msg Iterator
 ============================================================================
 */

#include "Beluga.h"
#include "CQuickMsgIterator.h"
#include "CQuickMsg.h"
#include "CMsgDb.h"


EXPORT_C gint32 CQuickMsgIterator::Current(CDbEntity ** ppEntity)
	{
	if (ppEntity == NULL)
		return ERROR(ESide_Client, EModule_Db, ECode_Invalid_Param);
	
	*ppEntity = NULL;
	if (m_dbQuery->eof())
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	
	 /* fill quick msg table fields */
	*ppEntity = new CQuickMsg(m_pEntityDb);
	if (NULL == *ppEntity)
		{
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
	
	CQuickMsg * pQuickMsg = (CQuickMsg*)(*ppEntity);
	for (int i=0; i<QuickMsgField_EndFlag; i++)
		{
		GString * fieldValue = g_string_new(m_dbQuery->fieldValue(i));
		pQuickMsg->SetFieldValue(i, fieldValue);	
		g_string_free(fieldValue, TRUE);
		}
	
	return 0;
	}
 
EXPORT_C gint32 CQuickMsgIterator::Next(gboolean * pSuccess)
	{
	*pSuccess = !m_dbQuery->eof();
	if (FALSE == *pSuccess)
		return ERROR(ESide_Client, EModule_Db, ECode_End_Of_Row);
	
	m_dbQuery->nextRow();
	return 0; 
	}

EXPORT_C gint32 CQuickMsgIterator::Prev(gboolean * pSuccess)
	{
	*pSuccess = FALSE;
	return ERROR(ESide_Client, EModule_Db, ECode_Not_Implemented);
	}
