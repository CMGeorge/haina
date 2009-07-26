#ifndef	__ACNETENGINE_H__
#define	__ACNETENGINE_H__

#ifdef BELUGA_DLL_BUILD
#define BELUGA_API __declspec(dllexport)
#else
#define BELUGA_API __declspec(dllimport)
#endif
using namespace std;

class BELUGA_API CacNetEngine
{
public:
	CacNetEngine();
	CacNetEngine(LPCTSTR aHostName,int aHttp_Port);
	~CacNetEngine();
public:
	bool	setNetHost(LPCTSTR aHostName,int aHttp_Port);

public:
	int			getErrCode();
	int			getQQStatus(string aQQId);
	WeatherDto*	getLiveWeather(string aCityCode);
	GPtrArray*	get7WeatherDatas(string aCityCode);
	GPtrArray*	getOrUpdatePD(string aFlag);
 	string		registerx(string loginName, string password, string mobile);
 	string		login(string loginName, string password);
 	bool		logoutByPsssport(string passport);
	
};

#endif	/* __ACNETENGINE_H__ */