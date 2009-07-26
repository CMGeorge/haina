#pragma once

#ifdef BELUGA_MANAGE_DLL_BUILD
#define BELUGA_MANAGE_API __declspec(dllexport)
#else
#define BELUGA_MANAGE_API __declspec(dllimport)
#endif
typedef struct _WeatherInfo
{
	string	date;				/*天气日期*/ 				
	string	weatherCityCode;	/*城市代码*/				
	string	weatherType;		/*天气类型*/				
	string	wind;				/*风速*/				
	string	temperature;		/*实时温度*/				
	string	icon;				/*天气图片URI*/				
	//	bool	isNight;			/*是否夜里*/				
	int		high;				/*最高气温*/				
	int		low;				/*最低气温*/				
	string	issuetime;			/*发布时间*/
}WeatherInfo;
#ifdef BELUGA_MANAGE_DLL_BUILD
	class CacNetEngine;
	class CContactDb;
#endif
class BELUGA_MANAGE_API beluga_manage
{
public:
	beluga_manage(void);
public:
	~beluga_manage(void);
public:
	//string GetQQStatusByID(string qqId);
	bool setNetHost(LPCTSTR aHostName,int aHttp_Port);
	//WeatherInfo GetLiveWeatherByCityCode(string cityCode);
	//WeatherInfo* Get7WeatherDatas(string cityCode);
	bool Register(string loginName, string password, string mobile);
	//bool Login(string loginName, string password);
	bool Logout(void);
	//int GetErrCode(void);
	//bool GetOrUpdatePD(string updatePD);
	void GetPhoneContactList(void);
#ifdef BELUGA_MANAGE_DLL_BUILD
private:

	CacNetEngine	m_ane;
	string			m_passport;
	CContactDb * pContactDb;
#endif
};
