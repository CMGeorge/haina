#ifndef	__HTTPINTERNET_H__
#define	__HTTPINTERNET_H__

#include <Windows.h>
#include <string>
#include <WinInet.h>
#include <tchar.h>

class CHttpInternet
{
public:
	CHttpInternet();
	~CHttpInternet();
	
public:
	bool SetHostName(LPCTSTR aHostName,int aHttp_Port=INTERNET_DEFAULT_HTTP_PORT);
	
public:
	std::string	SyncPostData(LPCTSTR aUrl,std::string aData);
	void	AsyncPostData();

private:
	std::string ReadNetFile();

private:
	TCHAR* http_header;
	HINTERNET i_Internet;
	HINTERNET i_Connect;
	HINTERNET i_HttpRequest;
};

#endif /* __HTTPINTERNET_H__ */