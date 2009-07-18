#include "HttpInternet.h"
#include <iostream>


using namespace std;

CHttpInternet::CHttpInternet():http_header(_T("Content-Type: application/x-www-form-urlencoded"))
{

}

CHttpInternet::~CHttpInternet()
{

}
	
bool CHttpInternet::SetHostName(LPCTSTR aHostName,int aHttp_Port)
{
//	i_Internet = InternetOpen(NULL, INTERNET_OPEN_TYPE_PRECONFIG, NULL, NULL, INTERNET_FLAG_ASYNC);
	i_Internet = InternetOpen(NULL, 
							  INTERNET_OPEN_TYPE_PRECONFIG,
							  NULL,
							  NULL,
							  NULL);

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
	i_HttpRequest = HttpOpenRequest(i_Connect,_T("POST"),aUrl,HTTP_VERSION,NULL,0,INTERNET_FLAG_DONT_CACHE,0);
	if (i_HttpRequest == NULL)
	{
		return NULL;
	}

	std::cout << "HttpSendRequest start " << __TIME__ << endl;
	if (!HttpSendRequest(i_HttpRequest,http_header,_tcslen(http_header),(LPVOID)aData.c_str(),aData.size()))
	{
		return NULL;    
	}
	std::cout << "HttpSendRequest end " << __TIME__ << endl;


	DWORD	qdwSize;     
	if(!InternetQueryDataAvailable(i_HttpRequest,&qdwSize,0,0))  
	{  
		//cout << "InternetQueryDataAvailable error" << endl;
		return NULL;  
	}  
	
	return ReadNetFile();
}
	
void CHttpInternet::AsyncPostData()
{

}


std::string CHttpInternet::ReadNetFile()
{
	std::cout << "ReadNetFile start " << GetTickCount() << endl;
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
	std::cout << "ReadNetFile end " << GetTickCount() << endl;

	return httpRevData;
}