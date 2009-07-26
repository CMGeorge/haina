#include "HttpInternet.h"
#include <iostream>
#include <mmsystem.h>

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
	i_HttpRequest = HttpOpenRequest(i_Connect,_T("POST"),aUrl,HTTP_VERSION,NULL,0,INTERNET_FLAG_DONT_CACHE,0);
	if (i_HttpRequest == NULL)
	{
		return "";
	}

	if (!HttpSendRequest(i_HttpRequest,szHead/*http_header*/,_tcslen(szHead)/*_tcslen(http_header)*/,(LPVOID)aData.c_str(),aData.size()))
	{
		DWORD dwTmp=GetLastError();
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