/*
 ============================================================================
 Name		: CMsgFaceIterator.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Msg Face Iterator
 ============================================================================
 */

#include "Beluga.h"
#include "CMsgFaceIterator.h"
#include "CMsgFace.h"
#include "CMsgDb.h"


EXPORT_C gint32 CMsgFaceIterator::Current(CDbEntity ** ppEntity)
	{
	if (ppEntity == NULL)
		return ERROR(ESide_Client, EModule_Db, ECode_Invalid_Param);
	
	*ppEntity = NULL;
	if (m_dbQuery->eof())
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	
	 /* fill config table fields */
	*ppEntity = new CMsgFace(m_pEntityDb);
	if (NULL == *ppEntity)
		{
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
	
	CMsgFace * pMsgFace = (CMsgFace*)(*ppEntity);
	for (int i=0; i<MsgFaceField_EndFlag; i++)
		{
		GString * fieldValue = g_string_new(m_dbQuery->fieldValue(i));
		pMsgFace->SetFieldValue(i, fieldValue);	
		g_string_free(fieldValue, TRUE);
		}
	
	return 0;
	}
 
EXPORT_C gint32 CMsgFaceIterator::Next(gboolean * pSuccess)
	{
	*pSuccess = !m_dbQuery->eof();
	if (FALSE == *pSuccess)
		return ERROR(ESide_Client, EModule_Db, ECode_End_Of_Row);
	
	m_dbQuery->nextRow();
	return 0; 
	}

EXPORT_C gint32 CMsgFaceIterator::Prev(gboolean * pSuccess)
	{
	*pSuccess = FALSE;
	return ERROR(ESide_Client, EModule_Db, ECode_Not_Implemented);
	}
