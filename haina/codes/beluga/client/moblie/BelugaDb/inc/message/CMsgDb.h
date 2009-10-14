/*
 ============================================================================
 Name		: CMsgDb.h
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Msg Database
 ============================================================================
 */

#ifndef __CMSGDB_H__
#define __CMSGDB_H__


#include <time.h>
#include "CEntityDb.h"

#include "CMsg.h"
#include "CMsgIterator.h"
#include "CQuickMsg.h"
#include "CQuickMsgIterator.h"
#include "CMsgFace.h"
#include "CMsgFaceIterator.h"
#include "CSignature.h"
#include "CSignatureIterator.h"

class CMsgDb : public CEntityDb
{
public:
    IMPORT_C CMsgDb();
    IMPORT_C ~CMsgDb();

	IMPORT_C gint32 InitEntityDb(gchar* dbName);
    IMPORT_C gint32 getMaxMsgId(guint32 * pMaxMsgId);
    IMPORT_C gint32 getMaxQuickMsgId(guint32 * pMaxQuickMsgId);
    IMPORT_C gint32 getMaxSignatureId(guint32 * pMaxSigId);
    IMPORT_C gint32 getMaxMsgFaceId(guint32 * pMaxFaceId);

    IMPORT_C gint32 getMsgById(guint32 nMsgId, CMsg** ppMsg);
    IMPORT_C gint32 getQuickMsgById(guint32 nQuickMsgId, CQuickMsg** ppMsg);
    IMPORT_C gint32 getSignatureById(guint32 nSignatureId, CSignature** ppSig);
    IMPORT_C gint32 getMsgFaceById(guint32 nMsgFaceId, CMsgFace** ppFace);

    IMPORT_C gint32 saveMsg(CMsg * ppMsg);
    IMPORT_C gint32 saveQuickMsg(CQuickMsg * pMsg);
    IMPORT_C gint32 saveSignature(CSignature * pSig);
    IMPORT_C gint32 saveMsgFace(CMsgFace * pFace);

    IMPORT_C gint32 updateMsg(CMsg * pMsg);
    IMPORT_C gint32 updateQuickMsg(CQuickMsg * pMsg);
    IMPORT_C gint32 updateSignature(CSignature * pSig);
    IMPORT_C gint32 updateMsgFace(CMsgFace * pFace);

    IMPORT_C gint32 deleteMsg(guint32 nMsgId);
    IMPORT_C gint32 deleteQuickMsg(guint32 nMsgId);
    IMPORT_C gint32 deleteSignature(guint32 nSigId);
    IMPORT_C gint32 deleteMsgFace(guint32 nFaceId);

	/* delete all msgs sent to and received from a contact */
    IMPORT_C gint32 deleteAllMsgsByContact(guint32 nContactId); 
    IMPORT_C gint32 deleteAllMsgs();

    IMPORT_C gint32 getMsgsTotality(guint32 &totality);
    IMPORT_C gint32 getMsgsTotalityByContact(guint32 nContactId, guint32 &totality); 
    /* msgs totality from the time on */
    IMPORT_C gint32 getMsgsTotalityByContact(guint32 nContactId, tm time, guint32 &totality);  
    IMPORT_C gint32 getMsgsTotalityByGroup(guint32 nGroupId, guint32 &totality);

    IMPORT_C gint32 getMsgFacesTotality(guint32 &totality);
    IMPORT_C gint32 getSignaturesTotality(guint32 &totality);
    IMPORT_C gint32 getQuickMsgsTotality(guint32 &totality);

	IMPORT_C gint32 getAllMsgFaces(CMsgFaceIterator ** ppMsgFaceIterator);
	IMPORT_C gint32 getAllSignatures(CSignatureIterator ** ppSigIterator);
	IMPORT_C gint32 getAllQuickMsgs(CQuickMsgIterator ** ppQuickMsgIterator);

    IMPORT_C gint32 searchMsgs(GArray * fieldsIndex, GPtrArray * fieldsValue, guint32 limit, guint32 offset, CMsgIterator ** ppMsgIterator); 
	/* search msgs sent to and received from a contact */
    IMPORT_C gint32 searchMsgsByContact(guint32 nContactId,  guint32 limit, guint32 offset, CMsgIterator ** ppMsgIterator); 
	/* search msgs sent to and received from a contact base on time */
    IMPORT_C gint32 searchMsgsByContact(guint32 nContactId, tm time, guint32 limit, guint32 offset, CMsgIterator ** ppMsgIterator);
    /* search msgs sent to and received from a group */
    IMPORT_C gint32 searchMsgsByGroup(guint32 nGroupId, guint32 limit, guint32 offset, CMsgIterator ** ppMsgIterator); 
    /* search msgs sent to and received from a group base on time */
    IMPORT_C gint32 searchMsgsByGroup(guint32 nGroupId, tm time, guint32 limit, guint32 offset, CMsgIterator ** ppMsgIterator); 
	
	IMPORT_C gchar * getDbPath();


private:
	GPtrArray	  * m_pMsgTableFieldsName;
	GPtrArray	  * m_pQuickMsgTableFieldsName;
	GPtrArray	  * m_pMsgFaceTableFieldsName;
	GPtrArray	  * m_pSignatureTableFieldsName;
};

#endif

