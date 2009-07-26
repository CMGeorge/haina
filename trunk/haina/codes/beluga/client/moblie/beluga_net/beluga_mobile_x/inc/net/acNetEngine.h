#ifndef	__ACNETENGINE_H__
#define	__ACNETENGINE_H__

#include <string>
#include <vector>
#include <WTypes.h>
#include "HttpInternet.h"
#include "PhoneDistrictDto.h"
#include "WeatherDto.h"

#include "glib/glib.h"


#include "json/json.h"


#ifdef BELUGA_DLL_BUILD
#include "hessian_output.h"
#include "hessian_input.h"
#include "string_input_stream.h"
#include "HessianRemoteReturning.h"
#define BELUGA_API __declspec(dllexport)
#else
#define BELUGA_API __declspec(dllimport)
#endif
using namespace std;

class BELUGA_API CacNetEngine
{
public:
	CacNetEngine();
	CacNetEngine(LPCTSTR aHostName,int aHttp_Port=INTERNET_DEFAULT_HTTP_PORT);
	~CacNetEngine();
public:
	bool	setNetHost(LPCTSTR aHostName,int aHttp_Port=INTERNET_DEFAULT_HTTP_PORT);

public:
	int			getErrCode();
	int			getQQStatus(string aQQId);
	WeatherDto*	getLiveWeather(string aCityCode);
	GPtrArray*	get7WeatherDatas(string aCityCode);
	GPtrArray*	getOrUpdatePD(string aFlag);
 	string		registerx(string loginName, string password, string mobile);
 	string		login(string loginName, string password);
 	bool		logoutByPsssport(string passport);
	
#ifdef BELUGA_DLL_BUILD
private:
	string getHessianString(string aHessianStr);
	HessianRemoteReturning	parse_json(string& aJson_string,Json::Value& jsonValue);
private:
	int		iErrCode;
	hessian::hessian_output	ihes_output;
//	hessian::hessian_input	ihes_input;


	string			iSendHes;
	CHttpInternet	iHttpNet;
#endif
};

#endif	/* __ACNETENGINE_H__ */