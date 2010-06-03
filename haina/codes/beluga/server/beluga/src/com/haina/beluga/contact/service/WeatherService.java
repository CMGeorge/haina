package com.haina.beluga.contact.service;

import java.rmi.RemoteException;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

import net.wxbug.api.ApiForecastData;
import net.wxbug.api.LiveWeatherData;
import net.wxbug.api.UnitType;
import net.wxbug.api.WeatherBugWebServicesLocator;
import net.wxbug.api.WeatherBugWebServicesSoap;

import org.apache.log4j.Logger;
import org.springframework.beans.factory.InitializingBean;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import com.haina.beluga.contact.dao.IPhoneDistrictDao;
import com.haina.beluga.contact.dao.IWeatherDao;
import com.haina.beluga.contact.domain.Weather;
import com.haina.beluga.contact.dto.WeatherDto;
import com.haina.beluga.webservice.Constant;
import com.haina.beluga.webservice.data.Returning;
import com.sihus.core.service.BaseSerivce;
import com.sihus.core.util.MfDate;
import com.sihus.core.util.MfTime;
@Component
public class WeatherService extends BaseSerivce<IWeatherDao,Weather,String> implements IWeatherService,InitializingBean {
	@Autowired(required=true)
	private IPhoneDistrictDao phoneDistrictDao;
	private final static String ACode="A3432136345";//，A3432136345
	private static Map<String,WeatherDto> liveWeatherPool = new HashMap<String,WeatherDto>(); 
	 static Logger logger = Logger.getLogger(WeatherService.class.getName());
	 private static WeatherBugWebServicesSoap   weatherBugWebServicesSoap ;
		static{
			try{
			WeatherBugWebServicesLocator   weatherBugWebServicesLocator   =   new   WeatherBugWebServicesLocator();  
			logger.info("the Web Service:"+weatherBugWebServicesLocator.getWeatherBugWebServicesSoapAddress());  
	           weatherBugWebServicesSoap  =  weatherBugWebServicesLocator.getWeatherBugWebServicesSoap(); 
			}catch(Exception   e){  
				logger.error(e);  
		    }  
		}
	
	



	@Override
	public void loadWeatherDatasByApi() {
		long t1 = System.currentTimeMillis();
		String[] codes = phoneDistrictDao.getWeatherCityCodes();
		List<Weather> weatherList = new ArrayList<Weather>();
		logger.info("loadWeatherDatasByApiSize:"+codes.length);
		int i = 0;
		for(String cityCode:codes){
			loadWDbyCityCode(cityCode,weatherList,0);
			logger.info("loadWeatherDatas at :"+(i++));
		}
		getBaseDao().saveAll(weatherList);
		//所有更新后的天气加载到缓存
		findAll();
		long t2 = System.currentTimeMillis();
		logger.info("loadWeatherDatasByApi-complete:"+(t2-t1));
	}
	@Override
	public void loadLiveDatasByApi() {
		long t1 = System.currentTimeMillis();
		String[] codes = phoneDistrictDao.getWeatherCityCodes();
		for(String cityCode:codes){
			try {
				initLivePoolByApi(cityCode);
			} catch (RemoteException e) {
				logger.error(e.getMessage());
			}
		}
		
		long t2 = System.currentTimeMillis();
		logger.info("loadLiveDatasByApi-complete:"+(t2-t1));
	}
	@Override
	public Returning getLiveWeather(String cityCode) {
		Returning hrr = new Returning();
		WeatherDto livePoolData = liveWeatherPool.get(cityCode);
		if(livePoolData!=null){
			hrr.setValue(livePoolData);
			return hrr;
		}
		try {
			WeatherDto dto =  initLivePoolByApi(cityCode);
			hrr.setValue(dto);
		} catch (RemoteException e) {
			// TODO Auto-generated catch block
			logger.error(e.getMessage());
			hrr.setStatusCode(Constant.NO_LIVEDATA_REMOTEERR);
		}
		return hrr;
	}
	public WeatherDto initLivePoolByApi(String cityCode) throws RemoteException{
		LiveWeatherData livedata = weatherBugWebServicesSoap.getLiveWeatherByCityCode(cityCode, UnitType.Metric, ACode);
		WeatherDto dto = new WeatherDto();
		dto.setWeatherType(livedata.getCurrDesc());
//		dto.setLow(Integer.valueOf(livedata.getTemperatureLow()));
//		dto.setHigh(Integer.valueOf(livedata.getTemperatureHigh()));
		dto.setTemperature(livedata.getTemperature());
		dto.setWeatherCityCode(cityCode);
		dto.setIssuetime(MfTime.toNow());
		dto.setIcon(livedata.getCurrIcon());
		dto.setIssuetime(MfTime.toNow());
		liveWeatherPool.put(cityCode, dto);
		return dto;
	}
	@Override
	public Returning get7Weatherdatas(String cityCode) {
		List<WeatherDto> list = new ArrayList<WeatherDto>();
		Iterator<Weather> iterator = getBaseDao().get7WeatherDatas(cityCode);
		while(iterator.hasNext()){
			Weather w = iterator.next();
			list.add(WeatherDto.valueof(w));
		}
		Returning hrr = new Returning(list);
			hrr.setValue(list);
		return hrr;
	}
	
	@Override
	public void afterPropertiesSet() throws Exception {
		findAll();
		
	}

	public void setPhoneDistrictDao(IPhoneDistrictDao phoneDistrictDao) {
		this.phoneDistrictDao = phoneDistrictDao;
	}
	private String getWindByStr(String str){
		return str.split("Winds")[1].split(".  ")[0];
	}
	private String getIcon(String str){
		return str.split("/")[6];
	}
	private String week2num(String week){
		MfDate mfDate = new MfDate();
		int day = 0;
		if(Constant.Monday.equals(week)){
			 day = Calendar.MONDAY;
		}else if(Constant.Tuesday.equals(week)){
			day = Calendar.TUESDAY;
		}else if(Constant.Wednesday.equals(week)){
			day = Calendar.WEDNESDAY;
		}else if(Constant.Thursday.equals(week)){
			day = Calendar.THURSDAY;
		}else if(Constant.Friday.equals(week)){
			day = Calendar.FRIDAY;
		}else if(Constant.Saturday.equals(week)){
			day = Calendar.SATURDAY;
		}else {
			day = Calendar.SUNDAY;
		}
		if(day >= mfDate.getWeek())
			mfDate.addDays(day-mfDate.getWeek());
		else
			mfDate.addDays(day+7-mfDate.getWeek());
		return mfDate.toString();
	}
	private void loadWDbyCityCode(String cityCode, List<Weather> weatherList,int no){
		try {
			ApiForecastData[] forecastDatas = weatherBugWebServicesSoap.getForecastByCityCode(cityCode, UnitType.Metric, ACode);
			if(forecastDatas.length == 7)
				for(ApiForecastData afd:forecastDatas){
					Weather weather = new Weather();
					weather.setDate(week2num(afd.getTitle()));
					weather.setHigh(Integer.valueOf(afd.getTempHigh()));
					weather.setLow(Integer.valueOf(afd.getTempLow()));
					weather.setWeatherCityCode(cityCode);
					weather.setWeatherType(afd.getDescription());
					weather.setWind(getWindByStr(afd.getPrediction()));
					weather.setIcon(getIcon(afd.getImage()));
					weather.setNight(afd.isIsNight());
					weather.setIssuetime(MfTime.toNow());
					weatherList.add(weather);
				}
			else
				for(int i = 1 ; i < forecastDatas.length; i=i+2){
					Weather weather = new Weather();
					weather.setDate(week2num(forecastDatas[i].getTitle()));
					weather.setHigh(Integer.valueOf(forecastDatas[i].getTempHigh()));
					weather.setLow(Integer.valueOf(forecastDatas[i].getTempLow()));
					weather.setWeatherCityCode(cityCode);
					weather.setWeatherType(forecastDatas[i].getDescription());
					weather.setWind(getWindByStr(forecastDatas[i].getPrediction()));
					weather.setIcon(getIcon(forecastDatas[i].getImage()));
					weather.setNight(forecastDatas[i].isIsNight());
					weather.setIssuetime(MfTime.toNow());
					weatherList.add(weather);
				}
		} catch (RemoteException e) {
			if(no < 3){
				loadWDbyCityCode(cityCode,weatherList,no++);
				logger.info(cityCode+":reload... "+no);
			}
		}
	}
	
}
