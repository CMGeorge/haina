#include "HttpInternet.h"
#include <iostream>
#include <mmsystem.h>
#include "connmgr.h"
using namespace std;

CHttpInternet::CHttpInternet():http_header(_T("Content-Type: application/x-www-form-urlencoded"))
{

}

CHttpInternet::~CHttpInternet()
{
	InternetCloseHandle(i_Connect);
	InternetCloseHandle(i_Internet);
}
	
bool CHttpInternet::SetHostName(LPCTSTR aHostName,int aHttp_Port)
{
//	i_Internet = InternetOpen(NULL, INTERNET_OPEN_TYPE_PRECONFIG, NULL, NULL, INTERNET_FLAG_ASYNC);
	//i_Internet = InternetOpen(NULL, 
	//						  INTERNET_OPEN_TYPE_PRECONFIG,
	//						  NULL,
	//						  NULL,
	//						  NULL);
	AutoDial_Connect();
	BYTE	ab=0x00;
	DWORD dwFlags=0;
	DWORD dwResult = INTERNET_ERROR_OPEN;
	CHAR strAgent[64]={0};	
	InternetGetConnectedState(&dwFlags,0);  
	if(LOBYTE(dwFlags) ==(BYTE)INTERNET_CONNECTION_OFFLINE)//take appropriate steps
	{
		return false;
	}
	sprintf(strAgent, "Agent%ld", timeGetTime());

	i_Internet = InternetOpenA(strAgent, INTERNET_OPEN_TYPE_DIRECT, NULL,NULL, 0);
	if(i_Internet == NULL)
		return false;
							  
							  
	i_Connect = InternetConnect(i_Internet,
								aHostName,
								aHttp_Port,
								NULL,
								NULL,
								INTERNET_SERVICE_HTTP,
								NULL,
								NULL);

	if(i_Connect == NULL)
		return false;

	return true;
}
	
std::string CHttpInternet::SyncPostData(LPCTSTR aUrl,string aData)
{
	LPCTSTR szHead = _T("Accept: */*\r\n\r\n");
	DWORD timeout=9000;
	i_HttpRequest = HttpOpenRequest(i_Connect,_T("POST"),aUrl,HTTP_VERSION,NULL,0,INTERNET_FLAG_RELOAD|INTERNET_FLAG_DONT_CACHE,0);
	if (i_HttpRequest == NULL)
	{
		return "";
	}
	InternetSetOption(i_HttpRequest,INTERNET_OPTION_SEND_TIMEOUT,&timeout,sizeof(DWORD));
	if (!HttpSendRequest(i_HttpRequest,http_header,_tcslen(http_header),(LPVOID)aData.c_str(),aData.size()))
	{
		DWORD dwTmp=GetLastError();//INTERNET_ERROR_BASE+2
		return "";    
	}

	DWORD	qdwSize;     
	if(!InternetQueryDataAvailable(i_HttpRequest,&qdwSize,0,0))  
	{  
		//cout << "InternetQueryDataAvailable error" << endl;
		return "";  
	}  
	
	return ReadNetFile();
}
	
void CHttpInternet::AsyncPostData()
{

}


std::string CHttpInternet::ReadNetFile()
{
	int		allsize = 0;
	int		sindex = 0;
	string	httpRevData;
	DWORD	dwSizeOfRead;
	char	cTemp[1024];

	BOOL bContinue = true;
	while (bContinue)
	{
		memset(cTemp,0,1024);
		if (InternetReadFile(i_HttpRequest,&cTemp,1024,&dwSizeOfRead))
		{
			if (dwSizeOfRead == 0)
			{
				bContinue = false;
			}
			else
			{
				allsize += dwSizeOfRead;
				httpRevData.resize(allsize);

				const char *c = httpRevData.c_str();
				char *ch=const_cast<char *>(c);
				
				memcpy(ch+sindex,cTemp,dwSizeOfRead);
				sindex += dwSizeOfRead;
				
				/*
				int hesLength = dwSizeOfRead;
				for(int i = 0;i < hesLength;i++)
				{
					ch[sindex] = cTemp[i];
					sindex++;
				}
				*/
			}
			
		}
	}

	InternetCloseHandle(i_HttpRequest);
	return httpRevData;
}
BOOL CHttpInternet::AutoDial_Connect()
{
	BYTE id[] = { 0x44,0xF1,0x6E,0x43,0xFB,0xB4,0x63,0x48,
		0xA0,0x41,0x8F,0x90,0x5A,0x62,0xC5,0x72};

	GUID networkID ;
	memcpy(&networkID,id,sizeof(networkID));

	CONNMGR_CONNECTIONINFO ci = { 0 };
	BOOL bAvailable = FALSE;
	HANDLE hConnection = NULL;

	//set the CONNMGR_CONNECTIONINFO flags for testing network status.
	ci.cbSize           = sizeof(ci);
	ci.dwParams         = CONNMGR_PARAM_GUIDDESTNET
		| CONNMGR_PARAM_MAXCONNLATENCY;
	ci.dwFlags          = 0;
	ci.ulMaxConnLatency = 4000;         // 4 second
	ci.bDisabled        = TRUE;
	ci.dwPriority       = CONNMGR_PRIORITY_USERINTERACTIVE;
	ci.guidDestNet      = networkID;

	HRESULT hr;
	if (SUCCEEDED(hr=ConnMgrEstablishConnection(&ci, &hConnection)))
	{
		DWORD dwResult = WaitForSingleObject(hConnection, 4000);

		switch (dwResult)
		{
		case WAIT_OBJECT_0:
			{
				DWORD dwStatus;
				if( SUCCEEDED(ConnMgrConnectionStatus(hConnection, &dwStatus)) && 
					( (dwStatus == CONNMGR_STATUS_CONNECTED) || (dwStatus == CONNMGR_STATUS_CONNECTIONDISABLED) ))
				{
					hr=S_OK;
				}
				else
				{
					hr=S_FALSE;
				}
				break;
			}

		case WAIT_TIMEOUT:
			hr=E_FAIL;
			break;
		}
	}
	//hr==S_OK says that the connection already exist.
	if(hr==S_OK)
	{
		return TRUE;
	}

	//otherwise, continue to establish connection
	//reset the CONNMGR_CONNECTIONINFO structure 

	CONNMGR_CONNECTIONINFO ConnInfo={0};

	memset(&ci,0,sizeof(ci));

	ci.cbSize = sizeof(ConnInfo);
	ci.dwParams = CONNMGR_PARAM_GUIDDESTNET;
	ci.dwFlags = 0;
	ci.dwPriority = CONNMGR_PRIORITY_USERINTERACTIVE ;
	ci.guidDestNet = networkID;

	hr = ConnMgrEstablishConnection(&ci, &hConnection);

	BOOL bStop = FALSE;
	DWORD dwStatus;

	while( bStop == FALSE )
	{
		DWORD dwResult = WaitForSingleObject(hConnection, INFINITE); 

		if (dwResult == (WAIT_OBJECT_0))
		{ 
			hr=ConnMgrConnectionStatus(hConnection,&dwStatus);
			if( SUCCEEDED(hr))
			{
				if( dwStatus == CONNMGR_STATUS_CONNECTED ||
					dwStatus == CONNMGR_STATUS_CONNECTIONFAILED ||
					dwStatus == CONNMGR_STATUS_NOPATHTODESTINATION ||
					dwStatus == CONNMGR_STATUS_CONNECTIONDISABLED ||
					dwStatus == CONNMGR_STATUS_CONNECTIONCANCELED)
					bStop=TRUE;
			}
			else
			{
				bStop=TRUE;
			}
		}
		else // failures
		{
			bStop = TRUE;
		}
	}

	return (dwStatus == CONNMGR_STATUS_CONNECTED);
}
