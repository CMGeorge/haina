/*
 ============================================================================
 Name		: CSignatureIterator.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Signature Iterator
 ============================================================================
 */

#include "Beluga.h"
#include "CSignatureIterator.h"
#include "CSignature.h"
#include "CMsgDb.h"


EXPORT_C gint32 CSignatureIterator::Current(CDbEntity ** ppEntity)
	{
	if (ppEntity == NULL)
		return ERROR(ESide_Client, EModule_Db, ECode_Invalid_Param);
	
	*ppEntity = NULL;
	if (m_dbQuery->eof())
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	
	 /* fill signature table fields */
	*ppEntity = new CSignature(m_pEntityDb);
	if (NULL == *ppEntity)
		{
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
	
	CSignature * pSignature = (CSignature*)(*ppEntity);
	for (int i=0; i<SignatureField_EndFlag; i++)
		{
		GString * fieldValue = g_string_new(m_dbQuery->fieldValue(i));
		pSignature->SetFieldValue(i, fieldValue);	
		g_string_free(fieldValue, TRUE);
		}
	
	return 0;
	}
 
EXPORT_C gint32 CSignatureIterator::Next(gboolean * pSuccess)
	{
	*pSuccess = !m_dbQuery->eof();
	if (FALSE == *pSuccess)
		return ERROR(ESide_Client, EModule_Db, ECode_End_Of_Row);
	
	m_dbQuery->nextRow();
	return 0; 
	}

EXPORT_C gint32 CSignatureIterator::Prev(gboolean * pSuccess)
	{
	*pSuccess = FALSE;
	return ERROR(ESide_Client, EModule_Db, ECode_Not_Implemented);
	}
