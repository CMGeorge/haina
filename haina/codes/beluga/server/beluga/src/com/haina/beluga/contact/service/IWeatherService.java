package com.haina.beluga.contact.service;

import com.haina.beluga.contact.dao.IWeatherDao;
import com.haina.beluga.contact.domain.Weather;
import com.haina.beluga.webservice.data.hessian.HessianRemoteReturning;
import com.sihus.core.service.IBaseSerivce;

public interface IWeatherService extends IBaseSerivce<IWeatherDao,Weather,String> {

	public void loadWeatherDatasByApi();
	public HessianRemoteReturning getLiveWeather(String cityCode);
	public HessianRemoteReturning get7Weatherdatas(String cityCode);
	void loadLiveDatasByApi();
}
