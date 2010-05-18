package com.haina.beluga.contact.dto;

import com.haina.beluga.contact.domain.Weather;
import com.sihus.core.dto.IDto;
import com.sihus.core.util.BeanUtil;

public class WeatherDto implements IDto{

	/**
	 * 
	 */
	private static final long serialVersionUID = -4673967747869811297L;
	
	private String date;/*天气日期*/ 
	private String weatherCityCode; /*城市代码*/
	private String weatherType; /*天气类型*/
	private String wind; /*风速*/
	private String temperature;/*实时温度*/
	private String icon;/*天气图片URI*/
	
//	private boolean isNight;/*是否夜里*/
	private int high; /*最高气温*/
	private int low;/*最低气温*/
	private String issuetime;/*发布时间*/
	
	public String getDate() {
		return date;
	}
	public void setDate(String date) {
		this.date = date;
	}
	public String getWeatherCityCode() {
		return weatherCityCode;
	}
	public void setWeatherCityCode(String weatherCityCode) {
		this.weatherCityCode = weatherCityCode;
	}
	public String getWeatherType() {
		return weatherType;
	}
	public void setWeatherType(String weatherType) {
		this.weatherType = weatherType;
	}
	public int getHigh() {
		return high;
	}
	public void setHigh(int high) {
		this.high = high;
	}
	public int getLow() {
		return low;
	}
	public void setLow(int low) {
		this.low = low;
	} 
	
	
	public String getWind() {
		return wind;
	}
	public void setWind(String wind) {
		this.wind = wind;
	}
	public String getTemperature() {
		return temperature;
	}
	public void setTemperature(String temperature) {
		this.temperature = temperature;
	}
	
	public String getIcon() {
		return icon;
	}
	public void setIcon(String icon) {
		this.icon = icon;
	}
//	@JSON(include=false)
//	public boolean isNight() {
//		return isNight;
//	}
//	public void setNight(boolean isNight) {
//		this.isNight = isNight;
//	}
	
	public String getIssuetime() {
		return issuetime;
	}
	public void setIssuetime(String issuetime) {
		this.issuetime = issuetime;
	}
	public static WeatherDto valueof(Weather w){
		WeatherDto dto = new WeatherDto();
		BeanUtil.copyPropertie(w, dto);
		return dto;
	}
	

}
