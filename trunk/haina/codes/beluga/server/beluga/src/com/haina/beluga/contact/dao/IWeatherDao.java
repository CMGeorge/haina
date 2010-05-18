package com.haina.beluga.contact.dao;

import java.util.Iterator;
import java.util.List;

import com.haina.beluga.contact.domain.Weather;
import com.sihus.core.dao.IBaseDao;

public interface IWeatherDao extends IBaseDao<Weather, String>{
	/**
	 * 清空过期天气。
	 */
	public void delAll();
	public void saveAll(List<Weather> list);
	public Iterator<Weather> get7WeatherDatas(String cityCode);

}
