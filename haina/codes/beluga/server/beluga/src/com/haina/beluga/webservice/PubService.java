package com.haina.beluga.webservice;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import com.haina.beluga.contact.service.IIMService;
import com.haina.beluga.contact.service.IPhoneDistrictService;
import com.haina.beluga.contact.service.IWeatherService;
import com.haina.beluga.webservice.data.Returning;

@Component(value = "pubService")
public class PubService implements IPubService {

	/**
	 * 
	 */
	private static final long serialVersionUID = -1083499059060975666L;
	@Autowired(required = true)
	private IIMService iMservice;
	@Autowired(required = true)
	private IWeatherService weatherService;
	@Autowired(required = true)
	private IPhoneDistrictService phoneDistrictService;

	@Override
	public Returning get7Weatherdatas(String cityCode) {
		return weatherService.get7Weatherdatas(cityCode);
	}

	@Override
	public Returning getLiveWeather(String cityCode) {
		return weatherService.getLiveWeather(cityCode);
	}

	@Override
	public Returning getQQStatus(String qqCode) {
		return iMservice.getQQStatus(qqCode);
	}

	// @Override
	// public HessianRemoteReturning getOrUpdatePD(String updateFlg) {
	// return phoneDistrictService.getOrUpdatePhoneDistricts(Integer
	// .valueOf(updateFlg));
	// }

	/*
	 * (non-Javadoc)
	 * 
	 * @see com.haina.beluga.webservice.IPubService#getOrUpdatePD(int, int, int)
	 */
	@Override
	public Returning getOrUpdatePD(int updateFlg, int begin,
			int count) {
		return phoneDistrictService.getOrUpdatePhoneDistricts(updateFlg, begin,
				count);
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see com.haina.beluga.webservice.IPubService#getOrUpdatePDCount(int)
	 */
	@Override
	public Returning getOrUpdatePDCount(int updateFlg) {
		return phoneDistrictService.getOrUpdatePhoneDistrictsCount(updateFlg);
	}

	@Override
	public Returning testCN(String cn) {
		Returning hr = new Returning();
		hr.setValue("好" + cn);
		return hr;
	}

}
