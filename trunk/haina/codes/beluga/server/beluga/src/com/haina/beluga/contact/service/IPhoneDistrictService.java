package com.haina.beluga.contact.service;

import com.haina.beluga.contact.dao.IPhoneDistrictDao;
import com.haina.beluga.contact.domain.PhoneDistrict;
import com.haina.beluga.webservice.data.hessian.HessianRemoteReturning;
import com.sihus.core.service.IBaseSerivce;

public interface IPhoneDistrictService extends
		IBaseSerivce<IPhoneDistrictDao, PhoneDistrict, String> {

	public HessianRemoteReturning getOrUpdatePhoneDistricts(int updateFlg);

	/**
	 * @author north0808@gmail.com
	 * @description 根据标识符、偏移量和长度获取要更新的手机号段和城市对应关系表中的记录
	 * @version 1.0
	 * @param updateFlg
	 * @param begin
	 * @param count
	 * @return HessianRemoteReturning
	 */
	public HessianRemoteReturning getOrUpdatePhoneDistricts(int updateFlg,
			int begin, int count);

	/**
	 * @author north0808@gmail.com
	 * @description 根据标识符获取要更新的手机号段和城市对应关系表中的记录数
	 * @version 1.0
	 * @param updateFlg
	 * @return HessianRemoteReturning
	 */
	public HessianRemoteReturning getOrUpdatePhoneDistrictsCount(int updateFlg);
}
