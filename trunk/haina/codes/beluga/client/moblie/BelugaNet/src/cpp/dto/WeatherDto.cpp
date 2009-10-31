#include "dto/WeatherDto.h"

namespace dto {

	void WeatherDto::setDate(string aDate)
	{
		date = aDate;
	}
	string WeatherDto::getDate()
	{
		return date;
	}

	void WeatherDto::setWeatherCityCode(string aCityCode)
	{
		weatherCityCode = aCityCode;
	}
	string WeatherDto::getWeatherCityCode()
	{
		return weatherCityCode;
	}

	void WeatherDto::setWeatherType(string aWeatherType)
	{
		weatherType = aWeatherType;
	}
	string WeatherDto::getWeatherType()
	{
		return weatherType;
	}

	void WeatherDto::setWind(string aWind)
	{
		wind = aWind;
	}
	string WeatherDto::getWind()
	{
		return wind;
	}

	void WeatherDto::setTemperature(string aTemperature)
	{
		temperature = aTemperature;
	}
	string WeatherDto::getTemperature()
	{
		return temperature;
	}

	void WeatherDto::setIcon(string aIconPath)
	{
		icon = aIconPath;
	}
	string WeatherDto::getIcon()
	{
		return icon;
	}

	/*
	void WeatherDto::setIsNight(bool aIsNight)
	{
	isNight = aIsNight;
	}
	bool WeatherDto::getIsNight()
	{
	return isNight;
	}*/


	void WeatherDto::setHigh(int aHigh)
	{
		high = aHigh;
	}
	int	 WeatherDto::getHigh()
	{
		return high;
	}

	void WeatherDto::setLow(int aLow)
	{
		low = aLow;
	}
	int	 WeatherDto::getLow()
	{
		return low;
	}

	void WeatherDto::setIssuetime(string aIssuetime)
	{
		issuetime = aIssuetime;
	}
	string WeatherDto::getIssuetime()
	{
		return issuetime;
	}

}//namespace dto
