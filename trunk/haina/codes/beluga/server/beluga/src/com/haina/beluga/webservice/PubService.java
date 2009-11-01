package com.haina.beluga.webservice;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import com.haina.beluga.contact.service.IIMService;
import com.haina.beluga.contact.service.IPhoneDistrictService;
import com.haina.beluga.contact.service.IWeatherService;
import com.haina.beluga.webservice.data.hessian.HessianRemoteReturning;

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
	public HessianRemoteReturning get7Weatherdatas(String cityCode) {
		return weatherService.get7Weatherdatas(cityCode);
	}

	@Override
	public HessianRemoteReturning getLiveWeather(String cityCode) {
		return weatherService.getLiveWeather(cityCode);
	}

	@Override
	public HessianRemoteReturning getQQStatus(String qqCode) {
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
	public HessianRemoteReturning getOrUpdatePD(int updateFlg, int begin,
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
	public HessianRemoteReturning getOrUpdatePDCount(int updateFlg) {
		return phoneDistrictService.getOrUpdatePhoneDistrictsCount(updateFlg);
	}

	@Override
	public HessianRemoteReturning testCN(String cn) {
		HessianRemoteReturning hr = new HessianRemoteReturning();
		hr.setValue("好" + cn);
		return hr;
	}

}
