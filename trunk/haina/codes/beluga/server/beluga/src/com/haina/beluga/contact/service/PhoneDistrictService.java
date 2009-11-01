package com.haina.beluga.contact.service;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

import org.springframework.stereotype.Component;

import com.haina.beluga.contact.dao.IPhoneDistrictDao;
import com.haina.beluga.contact.domain.PhoneDistrict;
import com.haina.beluga.contact.dto.PhoneDistrictDto;
import com.haina.beluga.webservice.data.hessian.HessianRemoteReturning;
import com.haina.core.service.BaseSerivce;

@Component
public class PhoneDistrictService extends
		BaseSerivce<IPhoneDistrictDao, PhoneDistrict, String> implements
		IPhoneDistrictService {

	@Override
	public HessianRemoteReturning getOrUpdatePhoneDistricts(int updateFlg) {
		List<PhoneDistrictDto> list = new ArrayList<PhoneDistrictDto>();
		Iterator<PhoneDistrict> iterator = getBaseDao()
				.getPhoneDistrictsByUpdateFlg(updateFlg);
		while (iterator.hasNext()) {
			PhoneDistrict w = iterator.next();
			list.add(PhoneDistrictDto.valueof(w));
		}
		HessianRemoteReturning hrr = new HessianRemoteReturning(list);
		hrr.setValue(list);
		return hrr;

	}

	/*
	 * (non-Javadoc)
	 * 
	 * @seecom.haina.beluga.contact.service.IPhoneDistrictService#
	 * getOrUpdatePhoneDistricts(int, int, int)
	 */
	@Override
	public HessianRemoteReturning getOrUpdatePhoneDistricts(int updateFlg,
			int begin, int count) {
		List<PhoneDistrictDto> list = new ArrayList<PhoneDistrictDto>();
		Iterator<PhoneDistrict> iterator = getBaseDao()
				.getPhoneDistrictsByUpdateFlg(updateFlg, begin, count);
		while (iterator.hasNext()) {
			PhoneDistrict w = iterator.next();
			list.add(PhoneDistrictDto.valueof(w));
		}
		HessianRemoteReturning hrr = new HessianRemoteReturning(list);
		hrr.setValue(list);
		return hrr;
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @seecom.haina.beluga.contact.service.IPhoneDistrictService#
	 * getOrUpdatePhoneDistrictsCount(int)
	 */
	@Override
	public HessianRemoteReturning getOrUpdatePhoneDistrictsCount(int updateFlg) {
		Long count = getBaseDao().getPhoneDistrictsByUpdateFlgCount(updateFlg);
		HessianRemoteReturning hrr = new HessianRemoteReturning(count);
		hrr.setValue(count);
		return hrr;
	}

}
