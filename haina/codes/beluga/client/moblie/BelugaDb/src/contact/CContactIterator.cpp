/*
 ============================================================================
 Name		: CContactIterator.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Contact Iterator
 ============================================================================
 */

#include "CContactIterator.h"
#include "CContact.h"
#include "CPhoneContact.h"
#include "CIMContact.h"


EXPORT_C gint32 CContactIterator::Current(CDbEntity ** ppEntity)
	{
	if (ppEntity == NULL)
		return ERROR(ESide_Client, EModule_Db, ECode_Invalid_Param);
	
	*ppEntity = NULL;
	if (m_dbQuery->eof() && !m_bAlreadyNext)
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	
	if (m_bAlreadyNext)
		m_bAlreadyNext = FALSE;
	
	m_nSameContactId = m_dbQuery->getIntField(ContactField_Id);
	guint8 contactType = m_dbQuery->getIntField(ContactField_Type);
	
	/* fill contact table fields */
	if (ContactType_Phone == contactType)
		*ppEntity = new CPhoneContact(m_pEntityDb);
	else	
		*ppEntity = new CIMContact(m_pEntityDb);
	if (NULL == *ppEntity)
		{
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
	
	CContact * pContact = (CContact*)(*ppEntity);
	for (int i=0; i<ContactField_EndFlag; i++)
		{
		const gchar * fieldValue = m_dbQuery->fieldValue(i);
		GString * field = g_string_new(fieldValue);
		pContact->SetFieldValue(i, field);	
		g_string_free(field, TRUE);
		
		if (ContactType_Phone == contactType)
			{
			if (i == ContactField_PhonePref && strlen(fieldValue))
				((CPhoneContact*)pContact)->SetPhone((ECommType)(CommType_Pref | CommType_Phone), (gchar*)fieldValue);
			else if (i == ContactField_EmailPref)
				((CPhoneContact*)pContact)->SetEmail((ECommType)(CommType_Pref | CommType_Email), (gchar*)fieldValue);
			else if (i == ContactField_IMPref)
				((CPhoneContact*)pContact)->SetIM((ECommType)(CommType_Pref | CommType_IM), (gchar*)fieldValue);
			}
		}
	
	/* fill contact_ext table and address table fields */
	if (ContactType_Phone == contactType && FALSE == m_bOnlyPref)
		{
		CPhoneContact * phoneContact = (CPhoneContact*)pContact;
		do 	
			{
			guint32 nContactId = m_dbQuery->getIntField(ContactField_Id);
			if (m_nSameContactId != nContactId)  /* has moved to the next contact */ 
				{ 
				m_bAlreadyNext = TRUE;  
				break;
				}
			else /* fill contact_ext table and address table fields */
				{
				ECommType commType = (ECommType)m_dbQuery->getIntField(ContactField_EndFlag + 1); /* contact_ext comm_key field */ 
				switch(commType & 0xF0) 
					{
					case CommType_Phone:
						phoneContact->SetPhone(commType, (gchar*)m_dbQuery->getStringField(ContactField_EndFlag + 2));	
						break;
					case CommType_Email:
						phoneContact->SetEmail(commType, (gchar*)m_dbQuery->getStringField(ContactField_EndFlag + 2));
						break;
					case CommType_Address:
						{
						stAddress addr;
						addr.aid = m_dbQuery->getIntField(ContactField_EndFlag + 2);
						addr.atype = commType;
						g_stpcpy(addr.block, m_dbQuery->getStringField(ContactField_EndFlag + 3));
						g_stpcpy(addr.street, m_dbQuery->getStringField(ContactField_EndFlag + 4));
						g_stpcpy(addr.district, m_dbQuery->getStringField(ContactField_EndFlag + 5));
						g_stpcpy(addr.city, m_dbQuery->getStringField(ContactField_EndFlag + 6));
						g_stpcpy(addr.state, m_dbQuery->getStringField(ContactField_EndFlag + 7));
						g_stpcpy(addr.country, m_dbQuery->getStringField(ContactField_EndFlag + 8));
						g_stpcpy(addr.postcode, m_dbQuery->getStringField(ContactField_EndFlag + 9));
						
						phoneContact->SetAddress(commType, &addr);
						}
						break;
					case CommType_IM:
						phoneContact->SetIM(commType, (gchar*)m_dbQuery->getStringField(ContactField_EndFlag + 2));
						break;
					default:
						break;
					}
				}
			
			m_dbQuery->nextRow();
			} while (!m_dbQuery->eof());
		}
	
	return 0;
	}
 
EXPORT_C gint32 CContactIterator::Next(gboolean * pSuccess)
	{
	if (m_bAlreadyNext)
		{
		*pSuccess = TRUE;
		return 0;
		}
	
	m_dbQuery->nextRow();
	*pSuccess = !m_dbQuery->eof();
	if (FALSE == *pSuccess)
		return ERROR(ESide_Client, EModule_Db, ECode_End_Of_Row);

	return 0; 
	}

EXPORT_C gint32 CContactIterator::Prev(gboolean * pSuccess)
	{
	*pSuccess = FALSE;
	return ERROR(ESide_Client, EModule_Db, ECode_Not_Implemented);
	}
