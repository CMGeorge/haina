/*
 ============================================================================
 Name		: CMsg.h
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Msg Entity
 ============================================================================
 */

#ifndef __CMSG_H__
#define __CMSG_H__

#include "CDbEntity.h"

enum MsgField
{
    MsgField_Id = 0,
    MsgField_Type,
    MsgField_Status,
    MsgField_Time,
    MsgField_From,
    MsgField_To,
    MsgField_GroupId,
    MsgField_ContentId,
    MsgField_Subject,
    MsgField_Cc,
    MsgField_Bcc,
    MsgField_EndFlag
};

#define  MSG_TEXT_CONTENT_LEN		1024
#define	 MSG_ATTACH_NAME_LEN		64
#define	 MSG_ATTACH_POSTFIX_LEN		5


#define  MSG_FROM_LEN				256
#define  MSG_TO_LEN					256
#define  MSG_SUBJECT_LEN			256
#define  MSG_CC_LEN					256
#define  MSG_BCC_LEN				256

#define  QUICK_MSG_CONTENT_LEN		512
#define  SIGNATURE_CONTENT_LEN		512

#define  MSG_FACE_SYMBOL_LEN		64
#define  MSG_FACE_PICTURE_LEN		256


enum MsgType
{
	MsgType_QQ = 1,
	MsgType_MSN = 2,
	MsgType_Email = 3,
	MsgType_SMS = 4,
	MsgType_MMS = 5
};

enum MsgStatus 
{
	MsgStatus_Sent = 1,
	MsgStatus_FailToSend = 2,
	MsgStatus_Received_UnRead = 3,
	MsgStatus_Received_Read = 4
};


class CMsg : public CDbEntity
{
public:
	IMPORT_C CMsg(CEntityDb * pEntityDb): CDbEntity(pEntityDb)
	{
	m_pFieldsIndex = g_array_sized_new(FALSE, TRUE, sizeof(guint32), MsgField_EndFlag);
	m_pFieldsValue = g_ptr_array_sized_new(MsgField_EndFlag);
	for (int i=MsgField_Id; i<MsgField_EndFlag; i++)
		{
		m_nFieldsIndex[i] = i;
		g_array_append_val(m_pFieldsIndex, m_nFieldsIndex[i]);
		g_ptr_array_add(m_pFieldsValue, g_string_new(""));
		}
	}
	
	IMPORT_C ~CMsg();

    IMPORT_C gint32 SetText(gchar * textMsg);
    IMPORT_C gint32 SetAttachment(gchar * attachName);

    IMPORT_C gint32 GetText(GString ** textMsg);
    IMPORT_C gint32 GetAttachment(gchar * path);  /* save attachment file to the path */

	IMPORT_C gint32 DeleteAttachment();

	IMPORT_C gint32 SaveMsgContent();
	IMPORT_C gint32 GetMsgContent(guint32 mcId);

private:
	guint32 m_nFieldsIndex[MsgField_EndFlag];

	gchar   m_szAttachName[MSG_ATTACH_NAME_LEN + 1];
	gchar   m_szAttachPostfix[MSG_ATTACH_POSTFIX_LEN + 1];
	gchar   m_szTextContent[MSG_TEXT_CONTENT_LEN + 1];
	gchar   m_szLocalPath[256 + 1];
};

#endif

