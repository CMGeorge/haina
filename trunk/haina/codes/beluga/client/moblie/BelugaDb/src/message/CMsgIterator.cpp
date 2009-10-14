/*
 ============================================================================
 Name		: CMsgIterator.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Msg Iterator
 ============================================================================
 */

#include "Beluga.h"
#include "CMsgIterator.h"
#include "CMsg.h"
#include "CMsgDb.h"


EXPORT_C gint32 CMsgIterator::Current(CDbEntity ** ppEntity)
	{
	guint32 mcId = 0;
	if (ppEntity == NULL)
		return ERROR(ESide_Client, EModule_Db, ECode_Invalid_Param);
	
	*ppEntity = NULL;
	if (m_dbQuery->eof())
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	
	 /* fill msg table fields */
	*ppEntity = new CMsg(m_pEntityDb);
	if (NULL == *ppEntity)
		{
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
	
	CMsg * pMsg = (CMsg*)(*ppEntity);
	for (int i=0; i<MsgField_EndFlag; i++)
		{
		if (i == MsgField_ContentId)
			mcId = atoi(m_dbQuery->fieldValue(i));

		GString * fieldValue = g_string_new(m_dbQuery->fieldValue(i));
		pMsg->SetFieldValue(i, fieldValue);	
		g_string_free(fieldValue, TRUE);
		}
	
	if (mcId != 0)
		pMsg->GetMsgContent(mcId);

	return 0;
	}
 
EXPORT_C gint32 CMsgIterator::Next(gboolean * pSuccess)
	{
	*pSuccess = !m_dbQuery->eof();
	if (FALSE == *pSuccess)
		return ERROR(ESide_Client, EModule_Db, ECode_End_Of_Row);
	
	m_dbQuery->nextRow();
	return 0; 
	}

EXPORT_C gint32 CMsgIterator::Prev(gboolean * pSuccess)
	{
	*pSuccess = FALSE;
	return ERROR(ESide_Client, EModule_Db, ECode_Not_Implemented);
	}
