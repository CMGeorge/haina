/*
 ============================================================================
 Name		: CMsg.cpp
 Author	  : shaochuan.yang
 Copyright   : haina
 Description : Message Entity
 ============================================================================
 */

#include <stdio.h>
#include <stdlib.h>
#include "CMsg.h"
#include "CMsgDb.h"

extern void deleteFile(gchar* file);


EXPORT_C CMsg::~CMsg()
{
	FILE * fp_r = fopen(m_szAttachName, "rb");
	if (fp_r != NULL) /* some attach file exist */
	{
		fclose(fp_r);
		deleteFile(m_szLocalPath);
	}
}

EXPORT_C gint32 CMsg::SetText(gchar * textMsg)
{
	memset(textMsg, 0, sizeof(m_szTextContent));
	strncmp(m_szTextContent, textMsg, MSG_TEXT_CONTENT_LEN);
	return 0;
}

EXPORT_C gint32 CMsg::SetAttachment(gchar * attachName)
{
	guint8 buffer[1024] = {0};

	FILE * fp_r = fopen(m_szLocalPath, "rb");
	if (fp_r != NULL) /* some attach file exist */
	{
		fclose(fp_r);
		fp_r = NULL;
		deleteFile(m_szLocalPath);
	}

	fp_r = fopen(attachName, "rb");
	if (fp_r == NULL)
	{
		return ERROR(ESide_Client, EModule_Db, ECode_Open_File_Failed);
	}
	
	memset(m_szAttachName, 0, sizeof(m_szAttachName));
	gchar * attach = strrchr(attachName, '/');
	if (!attach)
		attach = strrchr(attachName, '\\');
	if (attach)
		strncmp(m_szAttachName, attach + 1, MSG_ATTACH_POSTFIX_LEN);

	memset(m_szAttachPostfix, 0, sizeof(m_szAttachPostfix));
	gchar * postfix = strrchr(attachName, '.');
	if (postfix)
		strncmp(m_szAttachPostfix, postfix + 1, MSG_ATTACH_POSTFIX_LEN);

	memset(m_szLocalPath, 0, sizeof(m_szLocalPath));
	sprintf(m_szLocalPath, "%s/%s.%s", ((CMsgDb*)m_pEntityDb)->getDbPath(), m_szAttachName, m_szAttachPostfix);

	FILE * fp_w = fopen(m_szLocalPath, "w+b");
	if (fp_w == NULL)
	{
		fclose(fp_r);
		return ERROR(ESide_Client, EModule_Db, ECode_Open_File_Failed);
	}

	while (!feof(fp_r))
	{
		guint16 readLen = fread(buffer, sizeof(guint8), 1024, fp_r);
		fwrite(buffer, sizeof(guint8), readLen, fp_w);
		memset(buffer, 0 , 1024);
	}

	fclose(fp_r);
	fclose(fp_w);

	return 0;
}

EXPORT_C gint32 CMsg::GetText(GString ** textMsg)
{
	if (NULL == textMsg)
		return ERROR(ESide_Client, EModule_Db, ECode_Invalid_Param);
	
	*textMsg = g_string_new(m_szTextContent);
	if (NULL == *textMsg)
		return ERROR(ESide_Client, EModule_Db, ECode_No_Memory);

	return 0;
}

EXPORT_C gint32 CMsg::GetAttachment(gchar * path) /* save attachment file to the path */
{
	gchar szAttachFile[256] = {0};
	guint8 buffer[1024] = {0};

	sprintf(szAttachFile, "%s/%s.%s", path, m_szAttachName, m_szAttachPostfix);
	FILE * fp_r = fopen(m_szLocalPath, "rb");
	if (fp_r == NULL)
	{
		return ERROR(ESide_Client, EModule_Db, ECode_Open_File_Failed);
	}

	FILE * fp_w = fopen(szAttachFile, "w+b");
	if (fp_w == NULL)
	{
		fclose(fp_r);
		return ERROR(ESide_Client, EModule_Db, ECode_Open_File_Failed);
	}
	
	while (!feof(fp_r))
	{
		guint16 readLen = fread(buffer, sizeof(guint8), 1024, fp_r);
		fwrite(buffer, sizeof(guint8), readLen, fp_w);
		memset(buffer, 0 , 1024);
	}
	
	fclose(fp_r);
	fclose(fp_w);

	return 0;
}

EXPORT_C gint32 CMsg::DeleteAttachment()
{
	deleteFile(m_szLocalPath);
	memset(m_szLocalPath, 0, sizeof(m_szLocalPath));
	memset(m_szAttachName, 0, sizeof(m_szAttachName));
	memset(m_szAttachPostfix, 0, sizeof(m_szAttachPostfix));
	return 0;
}

EXPORT_C gint32 CMsg::SaveMsgContent()
{
	char sql[64] = {0};

	/* insert msg content entity */
	strcpy(sql, "insert into msg_content values(NULL, ?, ?, ?, ?);");
	CppSQLite3Statement statement = m_pEntityDb->GetDatabase()->compileStatement(sql);
	statement.bind(1, m_szTextContent);
	statement.bind(3, m_szAttachPostfix);
	statement.bind(4, m_szAttachName);

	FILE * fp_r = fopen(m_szLocalPath, "rb");
	if (fp_r != NULL)
	{
		fseek(fp_r, 0, SEEK_END);
		guint32 len = ftell(fp_r);
		fseek(fp_r, 0, SEEK_SET);
		
		guint8 * content = (guint8*)malloc(len * sizeof(guint8));
		if (content)
		{
			fread(content, sizeof(guint8), len, fp_r);
			statement.bind(2, content, len);
		}
		else
		{
			statement.bindNull(2);
		}

		fclose(fp_r);
	}
	else
	{
		printf("save attachment error!");
	}

	statement.execDML();
	statement.reset();

	return m_pEntityDb->GetDatabase()->execScalar("select max(mc_id) from msg_content;");
}

EXPORT_C gint32 CMsg::GetMsgContent(guint32 mcId)
{
	char sql[64] = {0};
	
	sprintf(sql, "select * from msg_content where mc_id = %d;", mcId);
	CppSQLite3Query query = m_pEntityDb->GetDatabase()->execQuery(sql);

	if (query.eof())
	{
		return ERROR(ESide_Client, EModule_Db, ECode_Not_Exist);
	}
	
	/* get text content */
	GString * fieldValue = g_string_new(query.fieldValue(1));
	SetText(fieldValue->str);
	g_string_free(fieldValue, TRUE);
	fieldValue = NULL;
	
	/* get attach postfix name */
	fieldValue = g_string_new(query.fieldValue(3));
	strncpy(m_szAttachPostfix, fieldValue->str, MSG_ATTACH_POSTFIX_LEN);
	g_string_free(fieldValue, TRUE);
	fieldValue = NULL;

	/* get attach name */
	fieldValue = g_string_new(query.fieldValue(4));
	strncpy(m_szAttachName, fieldValue->str, MSG_ATTACH_NAME_LEN);
	g_string_free(fieldValue, TRUE);
	fieldValue = NULL;

	/* get attach content */
	int contentLen = 0;
	gchar * content = (gchar*)query.getBlobField(2, contentLen);

	memset(m_szLocalPath, 0, sizeof(m_szLocalPath));
	sprintf(m_szLocalPath, "%s/%s.%s", ((CMsgDb*)m_pEntityDb)->getDbPath(), m_szAttachName, m_szAttachPostfix);
	FILE * fp_w = fopen(m_szLocalPath, "w+b");
	if (fp_w == NULL)
	{
		return ERROR(ESide_Client, EModule_Db, ECode_Open_File_Failed);
	}

	fwrite(content, sizeof(guint8), contentLen, fp_w);
	fclose(fp_w);

	return 0;
}