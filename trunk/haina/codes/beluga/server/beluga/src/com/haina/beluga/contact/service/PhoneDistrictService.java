package com.haina.beluga.contact.service;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

import org.springframework.stereotype.Component;

import com.haina.beluga.contact.dao.IPhoneDistrictDao;
import com.haina.beluga.contact.domain.PhoneDistrict;
import com.haina.beluga.contact.dto.PhoneDistrictDto;
import com.haina.beluga.webservice.data.Returning;
import com.sihus.core.service.BaseSerivce;

@Component
public class PhoneDistrictService extends
		BaseSerivce<IPhoneDistrictDao, PhoneDistrict, String> implements
		IPhoneDistrictService {

	@Override
	public Returning getOrUpdatePhoneDistricts(int updateFlg) {
		List<PhoneDistrictDto> list = new ArrayList<PhoneDistrictDto>();
		Iterator<PhoneDistrict> iterator = getBaseDao()
				.getPhoneDistrictsByUpdateFlg(updateFlg);
		while (iterator.hasNext()) {
			PhoneDistrict w = iterator.next();
			list.add(PhoneDistrictDto.valueof(w));
		}
		Returning hrr = new Returning(list);
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
	public Returning getOrUpdatePhoneDistricts(int updateFlg,
			int begin, int count) {
		List<PhoneDistrictDto> list = new ArrayList<PhoneDistrictDto>();
		Iterator<PhoneDistrict> iterator = getBaseDao()
				.getPhoneDistrictsByUpdateFlg(updateFlg, begin, count);
		while (iterator.hasNext()) {
			PhoneDistrict w = iterator.next();
			list.add(PhoneDistrictDto.valueof(w));
		}
		Returning hrr = new Returning(list);
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
	public Returning getOrUpdatePhoneDistrictsCount(int updateFlg) {
		Long count = getBaseDao().getPhoneDistrictsByUpdateFlgCount(updateFlg);
		Returning hrr = new Returning(count);
		hrr.setValue(count);
		return hrr;
	}

}
