/*
 ============================================================================
 Name		: CPhoneContact.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Phone Contact Entity
 ============================================================================
 */



#include "CPhoneContact.h"


static ECommType eCommInfoType;
static guint32	 nTotality = 0;
static guint32 * pHashKey = NULL;

GLREF_C void freeAddressArray(GPtrArray * pArray);

static void fill_hashtable (gpointer key, gpointer value, gpointer user_data)
	{
	pHashKey[*((guint32*)key)] = *((guint32*)key);
	g_hash_table_insert((GHashTable*)user_data, &pHashKey[*((guint32*)key)], g_strdup((gchar*)value));
	}

static void fill_hashtable_by_phonetype(gpointer key, gpointer value, gpointer user_data)
	{
	if (*((guint32*)key) & CommType_Phone)
		g_hash_table_insert((GHashTable*)user_data, key, g_strdup((gchar*)value));
	}

static void fill_hashtable_by_emailtype(gpointer key, gpointer value, gpointer user_data)
	{
	if (*((guint32*)key) & CommType_Email)
		g_hash_table_insert((GHashTable*)user_data, key, g_strdup((gchar*)value));
	}

static void fill_hashtable_by_imtype(gpointer key, gpointer value, gpointer user_data)
	{
	if (*((guint32*)key) & CommType_IM)
		g_hash_table_insert((GHashTable*)user_data, key, g_strdup((gchar*)value));
	}

static void get_totality(gpointer key, gpointer value, gpointer user_data)
	{
	if (*((guint32*)key) & (*(ECommType*)user_data)) nTotality++;
	}

static void delete_comminfo_by_phonetype(gpointer key, gpointer value, gpointer user_data)
	{
	if (*((guint32*)key) & CommType_Phone) 
		g_hash_table_remove((GHashTable*)user_data, key);
	}

static void delete_comminfo_by_emailtype(gpointer key, gpointer value, gpointer user_data)
	{
	if (*((guint32*)key) & CommType_Email) 
		g_hash_table_remove((GHashTable*)user_data, key);
	}

static void delete_comminfo_by_imtype(gpointer key, gpointer value, gpointer user_data)
	{
	if (*((guint32*)key) & CommType_IM) 
		g_hash_table_remove((GHashTable*)user_data, key);
	}

static gboolean find_commtype(gpointer key, gpointer value, gpointer user_data)
	{
	if (!g_ascii_strcasecmp((const char*)value, (const char*)user_data))
		{
		eCommInfoType = *(ECommType*)key;
		return TRUE;
		}
	else 
		return FALSE;
	}

static void copy_address(gpointer data, gpointer user_data)
	{
	stAddress * pAddr = (stAddress*)g_malloc0(sizeof(stAddress));
	pAddr->aid = ((stAddress*)data)->aid;
	pAddr->atype = ((stAddress*)data)->atype;
	g_stpcpy(pAddr->block, ((stAddress*)data)->block);
	g_stpcpy(pAddr->street, ((stAddress*)data)->street);
	g_stpcpy(pAddr->district, ((stAddress*)data)->district);
	g_stpcpy(pAddr->city, ((stAddress*)data)->city);
	g_stpcpy(pAddr->state, ((stAddress*)data)->state);
	g_stpcpy(pAddr->country, ((stAddress*)data)->country);
	g_stpcpy(pAddr->postcode, ((stAddress*)data)->postcode);
	g_ptr_array_add((GPtrArray*)user_data, pAddr);
	}


EXPORT_C CPhoneContact::CPhoneContact(CEntityDb * pEntityDb, gboolean init) :
    CContact(pEntityDb)
	{
	m_hCommInfoHashTable = NULL;
	m_aAddressArray = NULL;
	memset(m_aHashKey, 0, sizeof(m_aHashKey));
	pHashKey = m_aHashKey;

	if (init)
		{
		m_hCommInfoHashTable = g_hash_table_new_full(g_int_hash, g_int_equal, NULL, g_free);
		m_aAddressArray = g_ptr_array_new();
		}
	}

EXPORT_C CPhoneContact::~CPhoneContact()
	{
	if (m_hCommInfoHashTable)
		{
		g_hash_table_destroy(m_hCommInfoHashTable);
		m_hCommInfoHashTable = NULL;
		}
	
	if (m_aAddressArray)
		{
		freeAddressArray(m_aAddressArray);
		m_aAddressArray = NULL;
		}
	}

EXPORT_C gint32 CPhoneContact::GetAllPhones(GHashTable ** hPhones)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}	
	
	*hPhones = g_hash_table_new_full(g_int_hash, g_int_equal, NULL, g_free);
	g_hash_table_foreach(m_hCommInfoHashTable, fill_hashtable_by_phonetype, *hPhones);
	return 0;
	}

EXPORT_C gint32 CPhoneContact::GetAllEmails(GHashTable ** hEmails)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}	
	
	*hEmails = g_hash_table_new_full(g_int_hash, g_int_equal, NULL, g_free);
	g_hash_table_foreach(m_hCommInfoHashTable, fill_hashtable_by_emailtype, *hEmails);
	return 0;
	}

EXPORT_C gint32 CPhoneContact::GetAllAddresses(GPtrArray ** hAddresses)
	{
	if (NULL == m_aAddressArray)
		{
		GetAllCommInfo();
		}	
	
	*hAddresses = g_ptr_array_new();
	g_ptr_array_foreach(m_aAddressArray, copy_address, *hAddresses);
	return 0;
	}

EXPORT_C gint32 CPhoneContact::GetAllIMs(GHashTable ** hIMs)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}	
	
	*hIMs = g_hash_table_new_full(g_int_hash, g_int_equal, NULL, g_free);
	g_hash_table_foreach(m_hCommInfoHashTable, fill_hashtable_by_imtype, *hIMs);
	return 0;
	}

EXPORT_C gint32 CPhoneContact::SetPhones(GHashTable * hPhones)
	{
/*	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		} */
	if (NULL == m_hCommInfoHashTable)
		m_hCommInfoHashTable = g_hash_table_new_full(g_int_hash, g_int_equal, NULL, g_free);
	
	g_hash_table_foreach(hPhones, fill_hashtable, m_hCommInfoHashTable);
	return 0;
	}

EXPORT_C gint32 CPhoneContact::SetEmails(GHashTable * hEmails)
	{
	/*	if (NULL == m_hCommInfoHashTable)
			{
			GetAllCommInfo();
			} */
	if (NULL == m_hCommInfoHashTable)
		m_hCommInfoHashTable = g_hash_table_new_full(g_int_hash, g_int_equal, NULL, g_free);
			
	g_hash_table_foreach(hEmails, fill_hashtable, m_hCommInfoHashTable);
	return 0;
	}

EXPORT_C gint32 CPhoneContact::SetAddresses(GPtrArray * hAddresses)
	{
	guint32 i, j;
/*	if (NULL == m_aAddressArray)
		{
		GetAllCommInfo();
		}*/
	if (NULL == m_aAddressArray)
		m_aAddressArray = g_ptr_array_new();
	
	for (i=0; i<hAddresses->len; i++)
		{
		for (j=0; j<m_aAddressArray->len; j++)
			{
			stAddress * srcAddr = (stAddress*)g_ptr_array_index(hAddresses, i);
			stAddress * DestAddr = (stAddress*)g_ptr_array_index(m_aAddressArray, j);
			
			if (srcAddr->aid == DestAddr->aid)
				{
				DestAddr->atype = srcAddr->atype;
				g_stpcpy(DestAddr->block, srcAddr->block);
				g_stpcpy(DestAddr->street, srcAddr->street);
				g_stpcpy(DestAddr->district, srcAddr->district);
				g_stpcpy(DestAddr->city, srcAddr->city);
				g_stpcpy(DestAddr->state, srcAddr->state);
				g_stpcpy(DestAddr->country, srcAddr->country);
				g_stpcpy(DestAddr->postcode, srcAddr->postcode);
				break;
				}
			}

		if (j == m_aAddressArray->len)
			{
			stAddress * srcAddr = (stAddress*)g_ptr_array_index(hAddresses, i);
			stAddress * DestAddr = (stAddress*)g_malloc0(sizeof(stAddress));
	
			DestAddr->aid = srcAddr->aid;
			DestAddr->atype = srcAddr->atype;
			g_stpcpy(DestAddr->block, srcAddr->block);
			g_stpcpy(DestAddr->street, srcAddr->street);
			g_stpcpy(DestAddr->district, srcAddr->district);
			g_stpcpy(DestAddr->city, srcAddr->city);
			g_stpcpy(DestAddr->state, srcAddr->state);
			g_stpcpy(DestAddr->country, srcAddr->country);
			g_stpcpy(DestAddr->postcode, srcAddr->postcode);
			g_ptr_array_add(m_aAddressArray, DestAddr);
			}
		}

	return 0;
	}

EXPORT_C gint32 CPhoneContact::SetIMs(GHashTable * hIMs)
	{
	/*	if (NULL == m_hCommInfoHashTable)
			{
			GetAllCommInfo();
			} */
	if (NULL == m_hCommInfoHashTable)
		m_hCommInfoHashTable = g_hash_table_new_full(g_int_hash, g_int_equal, NULL, g_free);
	
	g_hash_table_foreach(hIMs, fill_hashtable, m_hCommInfoHashTable);
	return 0;
	}


EXPORT_C gint32 CPhoneContact::GetPhone(ECommType ePhoneType, gchar ** sPhone)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}	
	
	if (NULL == sPhone)
		return ERROR(ESide_Client, EModule_Db, ECode_Invalid_Param);
	
	*sPhone = NULL;
	*sPhone = g_strdup((gchar*)g_hash_table_lookup(m_hCommInfoHashTable, &ePhoneType));
	if (NULL == *sPhone)
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	
	return 0;
	}

EXPORT_C gint32 CPhoneContact::GetEmail(ECommType eEmailType, gchar ** sEmail)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}
	
	if (NULL == sEmail)
		return ERROR(ESide_Client, EModule_Db, ECode_Invalid_Param);
	
	*sEmail = NULL;
	*sEmail = g_strdup((gchar*)g_hash_table_lookup(m_hCommInfoHashTable, &eEmailType));
	if (NULL == *sEmail)
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	
	return 0;
	}

EXPORT_C gint32 CPhoneContact::GetAddress(ECommType eAddrType, stAddress ** sAddress)
	{
	if (NULL == m_aAddressArray)
		{
		GetAllCommInfo();
		}
	
	if (NULL == sAddress)
		return ERROR(ESide_Client, EModule_Db, ECode_Invalid_Param);
	
	*sAddress = NULL;
	for (guint32 j=0; j<m_aAddressArray->len; j++)
		{
		stAddress * srcAddr = (stAddress*)g_ptr_array_index(m_aAddressArray, j);
		if (eAddrType == srcAddr->atype)
			{
				*sAddress = (stAddress*)g_malloc0(sizeof(stAddress));
				if (NULL == *sAddress)
					return ERROR(ESide_Client, EModule_Sys, ECode_No_Memory);
								
				(*sAddress)->aid = srcAddr->aid;
				(*sAddress)->atype = srcAddr->atype;
				g_stpcpy((*sAddress)->block, srcAddr->block);
				g_stpcpy((*sAddress)->street, srcAddr->street);
				g_stpcpy((*sAddress)->district, srcAddr->district);
				g_stpcpy((*sAddress)->city, srcAddr->city);
				g_stpcpy((*sAddress)->state, srcAddr->state);
				g_stpcpy((*sAddress)->country, srcAddr->country);
				g_stpcpy((*sAddress)->postcode, srcAddr->postcode);
			}
		}
	return 0;
	}

EXPORT_C gint32 CPhoneContact::GetIM(ECommType eIMType, gchar ** sIM)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}
	
	if (NULL == sIM)
		return ERROR(ESide_Client, EModule_Db, ECode_Invalid_Param);
	
	*sIM = NULL;
	*sIM = g_strdup((gchar*)g_hash_table_lookup(m_hCommInfoHashTable, &eIMType));
	if (NULL == *sIM)
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	
	return 0;
	}

EXPORT_C gint32 CPhoneContact::SetPhone(ECommType ePhoneType, gchar * sPhone)
	{
	if (NULL == m_hCommInfoHashTable)
		m_hCommInfoHashTable = g_hash_table_new_full(g_int_hash, g_int_equal, NULL, g_free);
	
	m_aHashKey[(guint32)ePhoneType] = (guint32)ePhoneType;
	g_hash_table_insert(m_hCommInfoHashTable, &m_aHashKey[(guint32)ePhoneType], g_strdup((gchar*)sPhone));
	return 0;
	}

EXPORT_C gint32 CPhoneContact::SetEmail(ECommType eEmailType, gchar * sEmail)
	{
	if (NULL == m_hCommInfoHashTable)
		m_hCommInfoHashTable = g_hash_table_new_full(g_int_hash, g_int_equal, NULL, g_free);
	
	m_aHashKey[(guint32)eEmailType] = (guint32)eEmailType;
	g_hash_table_insert(m_hCommInfoHashTable, &m_aHashKey[(guint32)eEmailType], g_strdup((gchar*)sEmail));
	return 0;
	}

EXPORT_C gint32 CPhoneContact::SetAddress(ECommType eAddrType, stAddress * sAddress)
	{
	if (NULL == m_aAddressArray)
		m_aAddressArray = g_ptr_array_new();
	
	if (NULL == sAddress)
		return ERROR(ESide_Client, EModule_Db, ECode_Invalid_Param);
	
	guint32 j = 0;
	for (j=0; j<m_aAddressArray->len; j++)
		if (eAddrType == ((stAddress*)g_ptr_array_index(m_aAddressArray, j))->atype) break;
	
	stAddress * addr;
	if (j < m_aAddressArray->len)
		addr = (stAddress*)g_ptr_array_index(m_aAddressArray, j);
	else
		addr = (stAddress*)g_malloc0(sizeof(stAddress));
	
	addr->aid = sAddress->aid;
	addr->atype = sAddress->atype;
	g_stpcpy(addr->block, sAddress->block);
	g_stpcpy(addr->street, sAddress->street);
	g_stpcpy(addr->district, sAddress->district);
	g_stpcpy(addr->city, sAddress->city);
	g_stpcpy(addr->state, sAddress->state);
	g_stpcpy(addr->country, sAddress->country);
	g_stpcpy(addr->postcode, sAddress->postcode);
	
	if (j >= m_aAddressArray->len)
		g_ptr_array_add(m_aAddressArray, addr);
		
	return 0;
	}

EXPORT_C gint32 CPhoneContact::SetIM(ECommType eIMType, gchar * sIM)
	{
	if (NULL == m_hCommInfoHashTable)
		m_hCommInfoHashTable = g_hash_table_new_full(g_int_hash, g_int_equal, NULL, g_free);	
	
	m_aHashKey[(guint32)eIMType] = (guint32)eIMType;
	g_hash_table_insert(m_hCommInfoHashTable, &m_aHashKey[(guint32)eIMType], g_strdup((gchar*)sIM));
	return 0;
	}

EXPORT_C gint32 CPhoneContact::GetPhoneType(gchar * sPhone, ECommType * ePhoneType)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}
	
	if (g_hash_table_find(m_hCommInfoHashTable, find_commtype, sPhone))
		{
		*ePhoneType = eCommInfoType;
		return 0;
		}
	else
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	}

EXPORT_C gint32 CPhoneContact::GetEmailType(gchar * sEmail, ECommType * eEmailType)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}	
	
	if (g_hash_table_find(m_hCommInfoHashTable, find_commtype, sEmail))
		{
		*eEmailType = eCommInfoType;
		return 0;
		}
	else
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	}

EXPORT_C gint32 CPhoneContact::GetAddressType(stAddress * sAddress, ECommType * eAddrType)
	{
	if (NULL == m_aAddressArray)
		{
		GetAllCommInfo();
		}	
	
	for (guint32 i=0; i<m_aAddressArray->len; i++)
		if (sAddress->aid == ((stAddress*)g_ptr_array_index(m_aAddressArray, i))->aid)
			{
			*eAddrType = (ECommType)((stAddress*)g_ptr_array_index(m_aAddressArray, i))->atype;
			return 0;
			}
	
	return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	}

EXPORT_C gint32 CPhoneContact::GetIMType(gchar * sIM, ECommType * eIMType)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}	
	
	if (g_hash_table_find(m_hCommInfoHashTable, find_commtype, sIM))
		{
		*eIMType = eCommInfoType;
		return 0;
		}
	else
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	}

EXPORT_C gint32 CPhoneContact::GetPhoneTotality(guint32 * totality)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}
	nTotality = 0;
	ECommType eType = CommType_Phone;
	g_hash_table_foreach(m_hCommInfoHashTable, get_totality, &eType);
	*totality = nTotality; 
	return 0;
	}

EXPORT_C gint32 CPhoneContact::GetEmailTotality(guint32 * totality)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}	
	nTotality = 0;
	ECommType eType = CommType_Email;
	g_hash_table_foreach(m_hCommInfoHashTable, get_totality, &eType);
	*totality = nTotality; 
	return 0;
	}

EXPORT_C gint32 CPhoneContact::GetAddressTotality(guint32 * totality)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}	
	nTotality = 0;
	for (guint32 i=0; i<m_aAddressArray->len; i++)
		if (CommType_Address & ((stAddress*)g_ptr_array_index(m_aAddressArray, i))->atype)
			nTotality++;
	*totality = nTotality;
	return 0;
	}

EXPORT_C gint32 CPhoneContact::GetIMTotality(guint32 * totality)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}	
	nTotality = 0;
	ECommType eType = CommType_IM;
	g_hash_table_foreach(m_hCommInfoHashTable, get_totality, &eType);
	*totality = nTotality; 
	return 0;
	}

EXPORT_C gint32 CPhoneContact::DeletePhone(ECommType ePhoneType)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}
	g_hash_table_remove(m_hCommInfoHashTable, &ePhoneType);
	return 0;
	}

EXPORT_C gint32 CPhoneContact::DeleteEmail(ECommType eEmailType)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}	
	g_hash_table_remove(m_hCommInfoHashTable, &eEmailType);
	return 0;
	}

EXPORT_C gint32 CPhoneContact::DeleteAddress(ECommType eAddrType)
	{
	if (NULL == m_aAddressArray)
		{
		GetAllCommInfo();
		}	
	for (guint32 i=0; i<m_aAddressArray->len; i++)
		if (eAddrType == ((stAddress*)g_ptr_array_index(m_aAddressArray, i))->atype)
			{
			stAddress * addr = (stAddress*)g_ptr_array_remove_index(m_aAddressArray, i);
			g_free(addr);
			}
	return 0;
	}

EXPORT_C gint32 CPhoneContact::DeleteIM(ECommType eIMType)
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}	
	g_hash_table_remove(m_hCommInfoHashTable, &eIMType);
	return 0;
	}

EXPORT_C gint32 CPhoneContact::DeleteAllPhones()
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}
	g_hash_table_foreach(m_hCommInfoHashTable, delete_comminfo_by_phonetype, m_hCommInfoHashTable);	
	return 0;
	}

EXPORT_C gint32 CPhoneContact::DeleteAllEmails()
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}	
	g_hash_table_foreach(m_hCommInfoHashTable, delete_comminfo_by_emailtype, m_hCommInfoHashTable);
	return 0;
	}

EXPORT_C gint32 CPhoneContact::DeleteAllAddresses()
	{
	if (NULL == m_aAddressArray)
		{
		GetAllCommInfo();
		}	
	for (guint32 i=0; i<m_aAddressArray->len; i++)
		if (CommType_Address & ((stAddress*)g_ptr_array_index(m_aAddressArray, i))->atype)
			{
			stAddress * addr = (stAddress*)g_ptr_array_remove_index(m_aAddressArray, i);
			g_free(addr);
			}
	return 0;
	}

EXPORT_C gint32 CPhoneContact::DeleteAllIMs()
	{
	if (NULL == m_hCommInfoHashTable)
		{
		GetAllCommInfo();
		}	
	g_hash_table_foreach(m_hCommInfoHashTable, delete_comminfo_by_imtype, m_hCommInfoHashTable);
	return 0;
	}

gint32 CPhoneContact::GetAllCommInfo()
	{
	if (NULL == m_hCommInfoHashTable)
		m_hCommInfoHashTable = g_hash_table_new_full(g_int_hash, g_int_equal, NULL, g_free);
	if (NULL == m_aAddressArray)
		m_aAddressArray = g_ptr_array_new();
	
		return ((CContactDb*)m_pEntityDb)->GetContactCommInfo(this);
	}

GHashTable * CPhoneContact::getCommInfoHashTable()
	{
	return m_hCommInfoHashTable;
	}

GPtrArray * CPhoneContact::getAddressPtrArray()
	{
	return m_aAddressArray;
	}

