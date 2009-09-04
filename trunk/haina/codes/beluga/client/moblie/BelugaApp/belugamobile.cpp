
/* 
*  this class encapsulate the platform api
*/

#include "belugamobile.h"


BelugaWinMobile::Contact::Contact()
{
	m_pContact = new CContactDll();
	if (NULL != m_pContact)
	{
		m_pContact->init();
	}
}

BelugaWinMobile::Contact::~Contact()
{
	if (m_pContact)
		delete m_pContact;
	m_pContact = NULL;
}

guint32 BelugaWinMobile::Contact::getContactCount()
{
	return m_pContact->totalCount;
}

gboolean BelugaWinMobile::Contact::getContacts(GList** pContacts, guint32 offset, guint32 limit)
{
	return m_pContact->getContact(pContacts, offset, limit);
}
