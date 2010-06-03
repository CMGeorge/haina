package com.haina.beluga.contact.service;

import com.haina.beluga.contact.dao.IWeatherDao;
import com.haina.beluga.contact.domain.Weather;
import com.haina.beluga.webservice.data.Returning;
import com.sihus.core.service.IBaseSerivce;

public interface IWeatherService extends IBaseSerivce<IWeatherDao,Weather,String> {

	public void loadWeatherDatasByApi();
	public Returning getLiveWeather(String cityCode);
	public Returning get7Weatherdatas(String cityCode);
	void loadLiveDatasByApi();
}
