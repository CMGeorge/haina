#pragma once

#ifdef BELUGA_MANAGE_DLL_BUILD
#define BELUGA_MANAGE_API __declspec(dllexport)
#else
#define BELUGA_MANAGE_API __declspec(dllimport)
#endif
typedef struct _WeatherInfo
{
	string	date;				/*��������*/ 				
	string	weatherCityCode;	/*���д���*/				
	string	weatherType;		/*��������*/				
	string	wind;				/*����*/				
	string	temperature;		/*ʵʱ�¶�*/				
	string	icon;				/*����ͼƬURI*/				
	//	bool	isNight;			/*�Ƿ�ҹ��*/				
	int		high;				/*�������*/				
	int		low;				/*�������*/				
	string	issuetime;			/*����ʱ��*/
}WeatherInfo;
#ifdef BELUGA_MANAGE_DLL_BUILD
	class CacNetEngine;
	class CContactDb;
#endif
class BELUGA_MANAGE_API beluga_manage
{
private:
	beluga_manage(void);
public:
	~beluga_manage(void);
public:
	string GetQQStatusByID(string qqId);
	static beluga_manage *GetInstance(LPCTSTR aHostName,int aHttp_Port);
	WeatherInfo GetLiveWeatherByCityCode(string cityCode);
	WeatherInfo* Get7WeatherDatas(string cityCode);
	bool Register(string loginName, string password, string mobile);
	bool Login(string loginName, string password);
	bool Logout(void);
	int GetErrCode(void);
	bool GetOrUpdatePD(string updatePD);
	void GetPhoneContactList(void);
#ifdef BELUGA_MANAGE_DLL_BUILD
private:
	static beluga_manage *m_pbm;
	CacNetEngine	m_ane;
	string			m_passport;
	CContactDb * pContactDb;
#endif
};