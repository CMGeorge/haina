#ifndef __WEATHERDTO_H__
#define __WEATHERDTO_H__
#include <string>

using namespace std;
#ifdef BELUGA_DLL_BUILD
#define BELUGA_API __declspec(dllexport)
#else
#define BELUGA_API __declspec(dllimport)
#endif
class BELUGA_API WeatherDto
{
public:
	void	setDate(string aDate);
	string	getDate();
	
	void	setWeatherCityCode(string aCityCode);
	string	getWeatherCityCode();

	void	setWeatherType(string aWeatherType);
	string	getWeatherType();

	void	setWind(string aWind);
	string	getWind();

	void	setTemperature(string aTemperature);
	string	getTemperature();

	void	setIcon(string aIconPath);
	string	getIcon();

	/*
	void	setIsNight(bool aIsNight);
	bool	getIsNight();
	*/


	void	setHigh(int aHigh);
	int		getHigh();

	void	setLow(int aLow);
	int		getLow();

	void	setIssuetime(string aIssuetime);
	string	getIssuetime();

private:
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
};

#endif /* __WEATHERDTO_H__ */
