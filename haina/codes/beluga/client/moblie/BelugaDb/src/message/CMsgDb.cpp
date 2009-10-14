/*
 ============================================================================
 Name		: CMsgDb.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Msg Database
 ============================================================================
 */

#include <string.h>
#include <stdlib.h>
#include "Beluga.h"
#include "CMsgDb.h"


EXPORT_C CMsgDb::CMsgDb()
	{
	}
    
EXPORT_C CMsgDb::~CMsgDb()
	{
	}
    
EXPORT_C gint32 CMsgDb::InitEntityDb(gchar* dbName) /* fill fields name */
	{
	strcpy(m_dbName, dbName);
	OpenDatabase();

	/* init msg table fields */
	CppSQLite3Table msgTable = m_dbBeluga.getTable("select * from message limit 1;");
	m_pMsgTableFieldsName = g_ptr_array_sized_new(msgTable.numFields());
	if (!m_pMsgTableFieldsName)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}
			
	for (int i=0; i<msgTable.numFields(); i++)
		g_ptr_array_add(m_pMsgTableFieldsName, g_string_new((gchar*)msgTable.fieldName(i)));
	
	/* init quick_msg table fields */
	CppSQLite3Table quickmsgTable = m_dbBeluga.getTable("select * from quick_msg limit 1;");
	m_pQuickMsgTableFieldsName = g_ptr_array_sized_new(quickmsgTable.numFields());
	if (!m_pQuickMsgTableFieldsName)
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
	}

	for (int i=0; i<quickmsgTable.numFields(); i++)
		g_ptr_array_add(m_pQuickMsgTableFieldsName, g_string_new((gchar*)quickmsgTable.fieldName(i)));
	
	/* init msg_face table fields */
	CppSQLite3Table msgfaceTable = m_dbBeluga.getTable("select * from msg_face limit 1;");
	m_pMsgFaceTableFieldsName = g_ptr_array_sized_new(msgfaceTable.numFields());
	if (!m_pMsgFaceTableFieldsName)
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
	}

	for (int i=0; i<msgfaceTable.numFields(); i++)
		g_ptr_array_add(m_pMsgFaceTableFieldsName, g_string_new((gchar*)msgfaceTable.fieldName(i)));
	
	/* init signature table fields */
	CppSQLite3Table signatureTable = m_dbBeluga.getTable("select * from signature limit 1;");
	m_pSignatureTableFieldsName = g_ptr_array_sized_new(signatureTable.numFields());
	if (!m_pSignatureTableFieldsName)
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
	}

	for (int i=0; i<signatureTable.numFields(); i++)
		g_ptr_array_add(m_pSignatureTableFieldsName, g_string_new((gchar*)signatureTable.fieldName(i)));

	CloseDatabase();
	return 0;
	}
    
    
EXPORT_C gint32 CMsgDb::saveMsg(CMsg * pMsg)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();
	try 
		{
		/* insert msg content entity first */
		char szMcId[10] = {0};

		gint32 mcId = pMsg->SaveMsgContent(); /* insert msg content */
		_itoa(mcId, szMcId, 10);
		GString * mc_Id= g_string_new(szMcId);
		pMsg->SetFieldValue(MsgField_ContentId, mc_Id);
		g_string_free(mc_Id, TRUE);

		/* insert message entity */
		strcpy(sql, "insert into message values(?");
		for (i=0; i<MsgField_EndFlag - 1; i++)
			strcat(sql, ", ?");
		strcat(sql, ");");
		
		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);
		for (i=0; i<MsgField_EndFlag; i++)
			{
			GString * fieldValue = NULL;
			if (ECode_No_Error == pMsg->GetFieldValue(i, &fieldValue))
				{
				statement.bind(i, fieldValue->str);
				g_string_free(fieldValue, TRUE);
				}
			else
				statement.bindNull(i); 
			}
		statement.execDML();
		statement.reset();
				
		delete pMsg;
		pMsg = NULL;
		CloseDatabase();
		return 0;
		}
	catch(CppSQLite3Exception& e)
		{
		delete pMsg;
		pMsg = NULL;
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Insert_Failed);
		}
	
	return 0;
	}
    
    
EXPORT_C gint32 CMsgDb::updateMsg(CMsg * pMsg)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();
	
	try 
		{
		/* delete old msg content record and insert a new */
		GString * mc_Id = NULL;
		pMsg->GetFieldValue(MsgField_ContentId, &mc_Id);
		sprintf(sql, "delete from msg_content where mc_id = %s;", mc_Id->str);
		g_string_free(mc_Id, TRUE);
		m_dbBeluga.execDML(sql);
		
		/* insert the new msg content entity first */
		char szMcId[10] = {0};
		gint32 mcId = pMsg->SaveMsgContent(); /* insert msg content */
		_itoa(mcId, szMcId, 10);
		mc_Id= g_string_new(szMcId);
		pMsg->SetFieldValue(MsgField_ContentId, mc_Id);
		g_string_free(mc_Id, TRUE);
		
		/* update message entity */
		strcpy(sql, "update message set "); 
		for (i=1; i<MsgField_EndFlag; i++)
			{
			GString * fieldName = (GString*)g_ptr_array_index(m_pFieldsName, i);
			strcat(sql, fieldName->str);
			strcat(sql, " = ?");
			if (i != MsgField_EndFlag - 1)
				strcat(sql, ", ");
			}
		strcat(sql, " where mid = ?;");
		
		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);

		GString * idValue = NULL;
		if (ECode_No_Error == pMsg->GetFieldValue(0, &idValue))
			{
			statement.bind(MsgField_EndFlag, idValue->str);
			g_string_free(idValue, TRUE);
			}
		else
			statement.bindNull(MsgField_EndFlag);

		for (i=1; i<MsgField_EndFlag; i++)
			{
			GString * fieldValue = NULL;
			if (ECode_No_Error == pMsg->GetFieldValue(i, &fieldValue))
				{
				statement.bind(i, fieldValue->str);
				g_string_free(fieldValue, TRUE);
				}
			else
				statement.bindNull(i);
			}
		statement.execDML();
		statement.reset();
		
		delete pMsg;
		pMsg = NULL;
		CloseDatabase();
		return 0;
		}
	catch(CppSQLite3Exception& e)
		{
		delete pMsg;
		pMsg = NULL;
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Update_Failed);
		}
		
	return 0;
	}
    

EXPORT_C gint32 CMsgDb::getMaxMsgId(guint32 * pMaxMsgId)
	{
	OpenDatabase();
	*pMaxMsgId = m_dbBeluga.execScalar("select max(mid) from message;");
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::getMaxQuickMsgId(guint32 * pMaxQuickMsgId)
	{
	OpenDatabase();
	*pMaxQuickMsgId = m_dbBeluga.execScalar("select max(qm_id) from quick_msg;");
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::getMaxSignatureId(guint32 * pMaxSigId)
	{
	OpenDatabase();
	*pMaxSigId = m_dbBeluga.execScalar("select max(sid) from signature;");
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::getMaxMsgFaceId(guint32 * pMaxFaceId)
	{
	OpenDatabase();
	*pMaxFaceId = m_dbBeluga.execScalar("select max(mf_id) from msg_face;");
	CloseDatabase();
	return 0;
	}	

EXPORT_C gint32 CMsgDb::getMsgById(guint32 nMsgId, CMsg** ppMsg)
	{
	guint32 mcId = 0;
	char sql[128] = {0};
	*ppMsg = NULL;

	OpenDatabase();
	sprintf(sql, "select * from message where mid = %d;", nMsgId);
	CppSQLite3Query query = m_dbBeluga.execQuery(sql);

	if (query.eof())
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	}

	*ppMsg = new CMsg(this);
	if (NULL == *ppMsg)
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
	}

	for (int i=0; i<query.numFields(); i++)
	{
		if (i == MsgField_ContentId)
			mcId = atoi(query.fieldValue(i));

		GString * fieldValue = g_string_new(query.fieldValue(i));
		(*ppMsg)->SetFieldValue(i, fieldValue);	
		g_string_free(fieldValue, TRUE);
	}

	if (mcId != 0)
		(*ppMsg)->GetMsgContent(mcId);

	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::getQuickMsgById(guint32 nQuickMsgId, CQuickMsg** ppMsg)
	{
	char sql[128] = {0};
	*ppMsg = NULL;

	OpenDatabase();
	sprintf(sql, "select * from quick_msg where qm_id = %d;", nQuickMsgId);
	CppSQLite3Query query = m_dbBeluga.execQuery(sql);

	if (query.eof())
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	}

	*ppMsg = new CQuickMsg(this);
	if (NULL == *ppMsg)
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
	}

	for (int i=0; i<query.numFields(); i++)
	{
		GString * fieldValue = g_string_new(query.fieldValue(i));
		(*ppMsg)->SetFieldValue(i, fieldValue);	
		g_string_free(fieldValue, TRUE);
	}

	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::getSignatureById(guint32 nSignatureId, CSignature** ppSig)
	{
	char sql[128] = {0};
	*ppSig = NULL;

	OpenDatabase();
	sprintf(sql, "select * from signature where sid = %d;", nSignatureId);
	CppSQLite3Query query = m_dbBeluga.execQuery(sql);

	if (query.eof())
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	}

	*ppSig = new CSignature(this);
	if (NULL == *ppSig)
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
	}

	for (int i=0; i<query.numFields(); i++)
	{
		GString * fieldValue = g_string_new(query.fieldValue(i));
		(*ppSig)->SetFieldValue(i, fieldValue);	
		g_string_free(fieldValue, TRUE);
	}

	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::getMsgFaceById(guint32 nMsgFaceId, CMsgFace** ppFace)
	{
	char sql[128] = {0};
	*ppFace = NULL;

	OpenDatabase();
	sprintf(sql, "select * from msg_face where mf_id = %d;", nMsgFaceId);
	CppSQLite3Query query = m_dbBeluga.execQuery(sql);

	if (query.eof())
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	}

	*ppFace = new CMsgFace(this);
	if (NULL == *ppFace)
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
	}

	for (int i=0; i<query.numFields(); i++)
	{
		GString * fieldValue = g_string_new(query.fieldValue(i));
		(*ppFace)->SetFieldValue(i, fieldValue);	
		g_string_free(fieldValue, TRUE);
	}

	CloseDatabase();
	return 0;
	}
	

EXPORT_C gint32 CMsgDb::saveQuickMsg(CQuickMsg * pMsg)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();
	try 
		{
		/* insert quick msg entity */
		strcpy(sql, "insert into quick_msg values(NULL");
		for (i=0; i<QuickMsgField_EndFlag - 1; i++)
			strcat(sql, ", ?");
		strcat(sql, ");");

		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);
		for (i=1; i<QuickMsgField_EndFlag; i++)
			{
			GString * fieldValue = NULL;
			if (ECode_No_Error == pMsg->GetFieldValue(i, &fieldValue))
				{
				statement.bind(i, fieldValue->str);
				g_string_free(fieldValue, TRUE);
				}
			else
				statement.bindNull(i);
			}
		statement.execDML();
		statement.reset();

		delete pMsg;
		pMsg = NULL;
		CloseDatabase();
		return 0;
		}
	catch(CppSQLite3Exception& e)
		{
		delete pMsg;
		pMsg = NULL;
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Insert_Failed);
		}

	return 0;
	}

EXPORT_C gint32 CMsgDb::saveSignature(CSignature * pSig)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();
	try 
	{
		/* insert signature entity */
		strcpy(sql, "insert into signature values(NULL");
		for (i=0; i<SignatureField_EndFlag - 1; i++)
			strcat(sql, ", ?");
		strcat(sql, ");");

		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);
		for (i=1; i<SignatureField_EndFlag; i++)
		{
			GString * fieldValue = NULL;
			if (ECode_No_Error == pSig->GetFieldValue(i, &fieldValue))
			{
				statement.bind(i, fieldValue->str);
				g_string_free(fieldValue, TRUE);
			}
			else
				statement.bindNull(i);
		}
		statement.execDML();
		statement.reset();

		delete pSig;
		pSig = NULL;
		CloseDatabase();
		return 0;
	}
	catch(CppSQLite3Exception& e)
	{
		delete pSig;
		pSig = NULL;
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Insert_Failed);
	}

	return 0;
	}

EXPORT_C gint32 CMsgDb::saveMsgFace(CMsgFace * pFace)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();
	try 
	{
		/* insert msg face entity */
		strcpy(sql, "insert into msg_face values(NULL");
		for (i=0; i<MsgFaceField_EndFlag - 1; i++)
			strcat(sql, ", ?");
		strcat(sql, ");");

		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);
		for (i=1; i<MsgFaceField_EndFlag; i++)
		{
			GString * fieldValue = NULL;
			if (ECode_No_Error == pFace->GetFieldValue(i, &fieldValue))
			{
				statement.bind(i, fieldValue->str);
				g_string_free(fieldValue, TRUE);
			}
			else
				statement.bindNull(i);
		}
		statement.execDML();
		statement.reset();

		delete pFace;
		pFace = NULL;
		CloseDatabase();
		return 0;
	}
	catch(CppSQLite3Exception& e)
	{
		delete pFace;
		pFace = NULL;
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Insert_Failed);
	}

	return 0;
	}


EXPORT_C gint32 CMsgDb::updateQuickMsg(CQuickMsg * pMsg)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();

	try 
	{
		/* update quick msg entity */
		strcpy(sql, "update quick_msg set "); 
		for (i=1; i<QuickMsgField_EndFlag; i++)
		{
			GString * fieldName = (GString*)g_ptr_array_index(m_pFieldsName, i);
			strcat(sql, fieldName->str);
			strcat(sql, " = ?");
			if (i != QuickMsgField_EndFlag - 1)
				strcat(sql, ", ");
		}
		strcat(sql, "where qm_id = ?;");

		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);

		GString * idValue = NULL;
		if (ECode_No_Error == pMsg->GetFieldValue(0, &idValue))
		{
			statement.bind(QuickMsgField_EndFlag, idValue->str);
			g_string_free(idValue, TRUE);
		}
		else
			statement.bindNull(QuickMsgField_EndFlag);

		for (i=1; i<QuickMsgField_EndFlag; i++)
		{
			GString * fieldValue = NULL;
			if (ECode_No_Error == pMsg->GetFieldValue(i, &fieldValue))
			{
				statement.bind(i, fieldValue->str);
				g_string_free(fieldValue, TRUE);
			}
			else
				statement.bindNull(i);
		}
		statement.execDML();
		statement.reset();

		delete pMsg;
		pMsg = NULL;
		CloseDatabase();
		return 0;
	}
	catch(CppSQLite3Exception& e)
	{
		delete pMsg;
		pMsg = NULL;
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Update_Failed);
	}

	return 0;
	}

EXPORT_C gint32 CMsgDb::updateSignature(CSignature * pSig)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();

	try 
	{
		/* update signature entity */
		strcpy(sql, "update signature set "); 
		for (i=1; i<SignatureField_EndFlag; i++)
		{
			GString * fieldName = (GString*)g_ptr_array_index(m_pFieldsName, i);
			strcat(sql, fieldName->str);
			strcat(sql, " = ?");
			if (i != SignatureField_EndFlag - 1)
				strcat(sql, ", ");
		}
		strcat(sql, "where sid = ?;");

		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);

		GString * idValue = NULL;
		if (ECode_No_Error == pSig->GetFieldValue(0, &idValue))
		{
			statement.bind(QuickMsgField_EndFlag, idValue->str);
			g_string_free(idValue, TRUE);
		}
		else
			statement.bindNull(SignatureField_EndFlag);

		for (i=1; i<SignatureField_EndFlag; i++)
		{
			GString * fieldValue = NULL;
			if (ECode_No_Error == pSig->GetFieldValue(i, &fieldValue))
			{
				statement.bind(i, fieldValue->str);
				g_string_free(fieldValue, TRUE);
			}
			else
				statement.bindNull(i);
		}
		statement.execDML();
		statement.reset();

		delete pSig;
		pSig = NULL;
		CloseDatabase();
		return 0;
	}
	catch(CppSQLite3Exception& e)
	{
		delete pSig;
		pSig = NULL;
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Update_Failed);
	}

	return 0;
	}

EXPORT_C gint32 CMsgDb::updateMsgFace(CMsgFace * pFace)
	{
	int i;
	char sql[256] = {0};
	OpenDatabase();

	try 
	{
		/* update msg face entity */
		strcpy(sql, "update msg_face set "); 
		for (i=1; i<MsgFaceField_EndFlag; i++)
		{
			GString * fieldName = (GString*)g_ptr_array_index(m_pFieldsName, i);
			strcat(sql, fieldName->str);
			strcat(sql, " = ?");
			if (i != MsgFaceField_EndFlag - 1)
				strcat(sql, ", ");
		}
		strcat(sql, "where mf_id = ?;");

		CppSQLite3Statement statement = m_dbBeluga.compileStatement(sql);

		GString * idValue = NULL;
		if (ECode_No_Error == pFace->GetFieldValue(0, &idValue))
		{
			statement.bind(MsgFaceField_EndFlag, idValue->str);
			g_string_free(idValue, TRUE);
		}
		else
			statement.bindNull(MsgFaceField_EndFlag);

		for (i=1; i<MsgFaceField_EndFlag; i++)
		{
			GString * fieldValue = NULL;
			if (ECode_No_Error == pFace->GetFieldValue(i, &fieldValue))
			{
				statement.bind(i, fieldValue->str);
				g_string_free(fieldValue, TRUE);
			}
			else
				statement.bindNull(i);
		}
		statement.execDML();
		statement.reset();

		delete pFace;
		pFace = NULL;
		CloseDatabase();
		return 0;
	}
	catch(CppSQLite3Exception& e)
	{
		delete pFace;
		pFace = NULL;
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_Update_Failed);
	}

	return 0;
	}

EXPORT_C gint32 CMsgDb::deleteMsg(guint32 nMsgId)
	{
	char sql[64] = {0};
	OpenDatabase();

	sprintf(sql, "delete from message where mid = %d;", nMsgId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::deleteQuickMsg(guint32 nMsgId)
	{
	char sql[64] = {0};
	OpenDatabase();

	sprintf(sql, "delete from quick_msg where qm_id = %d;", nMsgId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::deleteSignature(guint32 nSigId)
	{
	char sql[64] = {0};
	OpenDatabase();

	sprintf(sql, "delete from signature where sid = %d;", nSigId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::deleteMsgFace(guint32 nFaceId)
	{
	char sql[64] = {0};
	OpenDatabase();

	sprintf(sql, "delete from msg_face where mf_id = %d;", nFaceId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}

/* delete all msgs sent to and received from a contact */
EXPORT_C gint32 CMsgDb::deleteAllMsgsByContact(guint32 nContactId)
	{
	char sql[256] = {0};
	OpenDatabase();

	sprintf(sql, "delete from message where "\
		"fromc in (select comm_value from contact_ext where cid = %d) "\
		"or toc in (select comm_value from contact_ext where cid = %d);", nContactId, nContactId);
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::deleteAllMsgs()
	{
	char sql[64] = {0};
	OpenDatabase();

	strcpy(sql, "delete from message;");
	m_dbBeluga.execDML(sql);
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::getMsgsTotality(guint32 &totality)
	{
	char sql[64] = {0};
	totality = 0;
	OpenDatabase();

	strcpy(sql, "select count(*) from message;");
	totality = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::getMsgsTotalityByContact(guint32 nContactId, guint32 &totality)
	{
	char sql[256] = {0};
	totality = 0;
	OpenDatabase();

	sprintf(sql, "select count(*) from message where "\
				"fromc in (select comm_value from contact_ext where cid = %d) "\
				"or toc in (select comm_value from contact_ext where cid = %d);", nContactId, nContactId);
	totality = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	return 0;
	}

/* msgs totality from the time on */
EXPORT_C gint32 CMsgDb::getMsgsTotalityByContact(guint32 nContactId, tm time, guint32 &totality)
	{
	char sql[512] = {0};
	totality = 0;
	OpenDatabase();

	sprintf(sql, "select count(*) from message where "\
		"(fromc in (select comm_value from contact_ext where cid = %d) "\
		"or toc in (select comm_value from contact_ext where cid = %d)) "\
		"and time >= '%d-%d-%d %02d:%02d:%02d';", nContactId, nContactId, 
		time.tm_year + 1900, time.tm_mon, time.tm_mday, time.tm_hour, time.tm_min, time.tm_sec);

	totality = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::getMsgsTotalityByGroup(guint32 nGroupId, guint32 &totality)
	{
	char sql[64] = {0};
	totality = 0;
	OpenDatabase();

	sprintf(sql, "select count(*) from message where gid = %d;", nGroupId);
	totality = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::getMsgFacesTotality(guint32 &totality)
	{
	char sql[64] = {0};
	totality = 0;
	OpenDatabase();

	strcpy(sql, "select count(*) from msg_face;");
	totality = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::getSignaturesTotality(guint32 &totality)
	{
	char sql[64] = {0};
	totality = 0;
	OpenDatabase();

	strcpy(sql, "select count(*) from signature;");
	totality = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::getQuickMsgsTotality(guint32 &totality)
	{
	char sql[64] = {0};
	totality = 0;
	OpenDatabase();

	strcpy(sql, "select count(*) from quick_msg;");
	totality = m_dbBeluga.execScalar(sql);
	CloseDatabase();
	return 0;
	}

EXPORT_C gint32 CMsgDb::getAllMsgFaces(CMsgFaceIterator ** ppMsgFaceIterator)
{
	char sql[128] = {0};
	OpenDatabase();

	strcpy(sql, "select * from msg_face;");	
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppMsgFaceIterator = NULL;
	*ppMsgFaceIterator = new CMsgFaceIterator(this);
	if (NULL == *ppMsgFaceIterator)
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
	}

	CloseDatabase();
	return 0;
}

EXPORT_C gint32 CMsgDb::getAllSignatures(CSignatureIterator ** ppSigIterator)
{
	char sql[128] = {0};
	OpenDatabase();

	strcpy(sql, "select * from signature;");	
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppSigIterator = NULL;
	*ppSigIterator = new CSignatureIterator(this);
	if (NULL == *ppSigIterator)
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
	}

	CloseDatabase();
	return 0;
}

EXPORT_C gint32 CMsgDb::getAllQuickMsgs(CQuickMsgIterator ** ppQuickMsgIterator)
{
	char sql[128] = {0};
	OpenDatabase();

	strcpy(sql, "select * from quick_msg;");	
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppQuickMsgIterator = NULL;
	*ppQuickMsgIterator = new CQuickMsgIterator(this);
	if (NULL == *ppQuickMsgIterator)
	{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
	}

	CloseDatabase();
	return 0;
}

EXPORT_C gint32 CMsgDb::searchMsgs(GArray * fieldsIndex, GPtrArray * fieldsValue, guint32 limit, guint32 offset, CMsgIterator ** ppMsgIterator)
	{
	return 0;
	}

/* search msgs sent to and received from a contact */
EXPORT_C gint32 CMsgDb::searchMsgsByContact(guint32 nContactId,  guint32 limit, guint32 offset, CMsgIterator ** ppMsgIterator)
	{
	char sql[256] = {0};
	OpenDatabase();

	sprintf(sql, "select * from message where "\
		"fromc in (select comm_value from contact_ext where cid = %d) "\
		"or toc in (select comm_value from contact_ext where cid = %d) limit %d offset %d order by time asc;", nContactId, nContactId, limit, offset);
	
	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppMsgIterator = NULL;
	*ppMsgIterator = new CMsgIterator(this);
	if (NULL == *ppMsgIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;
	}

/* search msgs sent to and received from a contact base on time */
EXPORT_C gint32 CMsgDb::searchMsgsByContact(guint32 nContactId, tm time, guint32 limit, guint32 offset, CMsgIterator ** ppMsgIterator)
	{
	char sql[512] = {0};
	OpenDatabase();

	sprintf(sql, "select * from message where "\
		"(fromc in (select comm_value from contact_ext where cid = %d) "\
		"or toc in (select comm_value from contact_ext where cid = %d)) "\
		"and time >= '%d-%d-%d %02d:%02d:%02d' limit %d offset %d order by time asc;", nContactId, nContactId, 
		time.tm_year + 1900, time.tm_mon, time.tm_mday, time.tm_hour, time.tm_min, time.tm_sec, limit, offset);

	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppMsgIterator = NULL;
	*ppMsgIterator = new CMsgIterator(this);
	if (NULL == *ppMsgIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;
	}

/* search msgs sent to and received from a group */
EXPORT_C gint32 CMsgDb::searchMsgsByGroup(guint32 nGroupId, guint32 limit, guint32 offset, CMsgIterator ** ppMsgIterator)
	{
	char sql[256] = {0};
	OpenDatabase();

	sprintf(sql, "select * from message where gid = %d"\
		" limit %d offset %d order by time asc;", nGroupId, limit, offset);

	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppMsgIterator = NULL;
	*ppMsgIterator = new CMsgIterator(this);
	if (NULL == *ppMsgIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;
	}

/* search msgs sent to and received from a group base on time */
EXPORT_C gint32 CMsgDb::searchMsgsByGroup(guint32 nGroupId, tm time, guint32 limit, guint32 offset, CMsgIterator ** ppMsgIterator)
	{
	char sql[256] = {0};
	OpenDatabase();

	sprintf(sql, "select * from message where gid = %d"\
		" and time >= '%d-%d-%d %02d:%02d:%02d' limit %d offset %d order by time asc;", 
		nGroupId, time.tm_year + 1900, time.tm_mon, time.tm_mday, time.tm_hour, time.tm_min, time.tm_sec, limit, offset);

	m_dbQuery = m_dbBeluga.execQuery(sql);
	*ppMsgIterator = NULL;
	*ppMsgIterator = new CMsgIterator(this);
	if (NULL == *ppMsgIterator)
		{
		CloseDatabase();
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);
		}

	CloseDatabase();
	return 0;
	}


EXPORT_C gchar * CMsgDb::getDbPath()
	{
	gchar * dbPath = strrchr(m_dbName, '/');
	if (!dbPath)
		dbPath = strrchr(m_dbName, '\\');

	if (dbPath)
		return (dbPath + 1);

	return NULL;
	}