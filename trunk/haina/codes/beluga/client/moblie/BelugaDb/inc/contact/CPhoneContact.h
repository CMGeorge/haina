/*
 ============================================================================
 Name		: CPhoneContact.h
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Phone Contact Entity
 ============================================================================
 */

#ifndef __CPHONECONTACT_H__
#define __CPHONECONTACT_H__


#include "CContact.h"
#include "CContactDb.h"

class CPhoneContact : public CContact
{
public:
    IMPORT_C CPhoneContact(CEntityDb * pEntityDb, gboolean init = TRUE);
    IMPORT_C ~CPhoneContact();
    
    IMPORT_C gint32 GetAllPhones(GHashTable ** hPhones);
    IMPORT_C gint32 GetAllEmails(GHashTable ** hEmails);
    IMPORT_C gint32 GetAllAddresses(GPtrArray ** pAddresses);
    IMPORT_C gint32 GetAllIMs(GHashTable ** hIMs);

    IMPORT_C gint32 SetPhones(GHashTable * hPhones);
    IMPORT_C gint32 SetEmails(GHashTable * hEmails);
    IMPORT_C gint32 SetAddresses(GPtrArray * pAddresses);
    IMPORT_C gint32 SetIMs(GHashTable * hIMs);
    
    IMPORT_C gint32 GetPhone(ECommType ePhoneType, gchar ** sPhone);
    IMPORT_C gint32 GetEmail(ECommType eEmailType, gchar ** sEmail);
    IMPORT_C gint32 GetAddress(ECommType eAddrType, stAddress ** sAddress);
    IMPORT_C gint32 GetIM(ECommType eIMType, gchar ** sIM);

    IMPORT_C gint32 SetPhone(ECommType ePhoneType, gchar * sPhone);
    IMPORT_C gint32 SetEmail(ECommType eEmailType, gchar * sEmail);
    IMPORT_C gint32 SetAddress(ECommType eAddrType, stAddress * sAddress);
    IMPORT_C gint32 SetIM(ECommType eIMType, gchar * sIM);

    IMPORT_C gint32 GetPhoneType(gchar * sPhone, ECommType * ePhoneType);
    IMPORT_C gint32 GetEmailType(gchar * sEmail, ECommType * eEmailType);
    IMPORT_C gint32 GetAddressType(stAddress * sAddress, ECommType * eAddrType);
    IMPORT_C gint32 GetIMType(gchar * sIM, ECommType * eIMType);

    IMPORT_C gint32 GetPhoneTotality(guint32 * totality);
    IMPORT_C gint32 GetEmailTotality(guint32 * totality);
    IMPORT_C gint32 GetAddressTotality(guint32 * totality);
    IMPORT_C gint32 GetIMTotality(guint32 * totality);

    IMPORT_C gint32 DeletePhone(ECommType ePhoneType);
    IMPORT_C gint32 DeleteEmail(ECommType eEmailType);
    IMPORT_C gint32 DeleteAddress(ECommType eAddrType);
    IMPORT_C gint32 DeleteIM(ECommType eIMType);

    IMPORT_C gint32 DeleteAllPhones();
    IMPORT_C gint32 DeleteAllEmails();
    IMPORT_C gint32 DeleteAllAddresses();
    IMPORT_C gint32 DeleteAllIMs();
    
    GHashTable * getCommInfoHashTable();
    GPtrArray * getAddressPtrArray();

private:
	gint32  GetAllCommInfo();
	
private:
	GHashTable	  * m_hCommInfoHashTable;
	GPtrArray	  * m_aAddressArray;
	guint32			m_aHashKey[0x46 + 1];
};

#endif
