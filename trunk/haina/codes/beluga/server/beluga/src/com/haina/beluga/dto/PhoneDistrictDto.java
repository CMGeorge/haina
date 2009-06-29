package com.haina.beluga.dto;

import com.haina.beluga.core.dto.IDto;
import com.haina.beluga.core.util.BeanUtil;
import com.haina.beluga.domain.PhoneDistrict;

public class PhoneDistrictDto implements IDto {

	/**
	 * 
	 */
	private static final long serialVersionUID = 7382562686004299711L;
	
	private String districtNumber;/*区号*/
	private String range;/*手机前7位范围*/
	private String feeType;/*资费类型*/
	private String districtCity;/*城市*/
	private String districtProvince;/*省份*/
	private Integer updateFlg;/*更新标志*/
//	private String pingyinCity;
	private String weatherCityCode;/*天气代码*/
	public String getDistrictNumber() {
		return districtNumber;
	}
	public void setDistrictNumber(String districtNumber) {
		this.districtNumber = districtNumber;
	}
	public String getRange() {
		return range;
	}
	public void setRange(String range) {
		this.range = range;
	}
	public String getFeeType() {
		return feeType;
	}
	public void setFeeType(String feeType) {
		this.feeType = feeType;
	}
	public String getDistrictCity() {
		return districtCity;
	}
	public void setDistrictCity(String districtCity) {
		this.districtCity = districtCity;
	}
	public String getDistrictProvince() {
		return districtProvince;
	}
	public void setDistrictProvince(String districtProvince) {
		this.districtProvince = districtProvince;
	}
	public Integer getUpdateFlg() {
		return updateFlg;
	}
	public void setUpdateFlg(Integer updateFlg) {
		this.updateFlg = updateFlg;
	}
	public String getWeatherCityCode() {
		return weatherCityCode;
	}
	public void setWeatherCityCode(String weatherCityCode) {
		this.weatherCityCode = weatherCityCode;
	}
	public static PhoneDistrictDto valueof(PhoneDistrict w) {
		PhoneDistrictDto dto = new PhoneDistrictDto();
		BeanUtil.copyPropertie(w, dto);
		return dto;
	}
	
	

}
