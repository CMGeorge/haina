/*
 ============================================================================
 Name		: CContact.h
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Contact Entity
 ============================================================================
 */

#ifndef __CCONTACT_H__
#define __CCONTACT_H__

#include "CDbEntity.h"

enum ContactField
{
    ContactField_Id = 0,
    ContactField_Type,
    ContactField_UserId,
    ContactField_Name,
    ContactField_NameSpell,
    ContactField_NickName,
    ContactField_NickNameSpell,
    ContactField_Sex,
    ContactField_Photo,
    ContactField_Signature,
    ContactField_PhonePref,
    ContactField_EmailPref,
    ContactField_IMPref,
    ContactField_Birthday,
    ContactField_Org,
    ContactField_Url,
    ContactField_CallRing,
	ContactField_MsgRing,
    ContactField_Title,
    ContactField_Note,
    ContactField_EndFlag
};

enum ContactType 
{
	ContactType_Phone = 1,
	ContactType_IM = 2
};


#define  CONTACT_UID_LEN		256
#define  CONTACT_NAME_LEN		64
#define  CONTACT_NAME_SPELL_LEN	64
#define  CONTACT_NICKNAME_LEN	128
#define  CONTACT_NICKNAME_SPELL_LEN	128
#define  CONTACT_PHOTO_LEN		256
#define  CONTACT_SIGNATURE_LEN	256
#define  CONTACT_PHONE_PREF_LEN	80
#define  CONTACT_EMAIL_PREF_LEN	256
#define  CONTACT_IM_PREF_LEN	256
#define  CONTACT_BIRTHDAY_LEN	10
#define  CONTACT_ORG_LEN		64
#define  CONTACT_URL_LEN		256
#define  CONTACT_RING_LEN		256
#define  CONTACT_TITLE_LEN		64
#define  CONTACT_NOTE_LEN		512


#define  ADDR_BLOCK_LEN			80
#define  ADDR_STREET_LEN		64
#define  ADDR_DISTRICT_LEN		64
#define  ADDR_CITY_LEN			64
#define  ADDR_STATE_LEN			64
#define  ADDR_COUNTRY_LEN		64
#define  ADDR_POSTCODE_LEN		32


struct stAddress
	{
	guint32 aid;
	guint32 atype;
	gchar block[ADDR_BLOCK_LEN];
	gchar street[ADDR_STREET_LEN];
	gchar district[ADDR_DISTRICT_LEN];
	gchar city[ADDR_CITY_LEN];
	gchar state[ADDR_STATE_LEN];
	gchar country[ADDR_COUNTRY_LEN];
	gchar postcode[ADDR_POSTCODE_LEN];
	};

class CContact : public CDbEntity
{
public:
	IMPORT_C CContact(CEntityDb * pEntityDb): CDbEntity(pEntityDb)
	{
	m_pFieldsIndex = g_array_sized_new(FALSE, TRUE, sizeof(guint32), ContactField_EndFlag);
	m_pFieldsValue = g_ptr_array_sized_new(ContactField_EndFlag);
	for (int i=ContactField_Id; i<ContactField_EndFlag; i++)
		{
		m_nFieldsIndex[i] = i;
		g_array_append_val(m_pFieldsIndex, m_nFieldsIndex[i]);
		g_ptr_array_add(m_pFieldsValue, g_string_new(""));
		}
	}
	
	IMPORT_C ~CContact()
		{
		
		}

protected:
	guint32 m_nFieldsIndex[ContactField_EndFlag];
};

#endif
