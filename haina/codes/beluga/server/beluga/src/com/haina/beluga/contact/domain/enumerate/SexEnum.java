package com.haina.beluga.contact.domain.enumerate;

import com.sihus.core.enumerate.IntegerEnumAbbr;


/**
 * 性别枚举类型。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-07-01
 */
public enum SexEnum implements IntegerEnumAbbr {
	
	/*分别表示未知，男性，女性。*/
	unknown(0),male(1),famale(2);
	
	private Integer sex;
	
	private SexEnum(Integer sex) {
		this.sex=sex;
	}

	@Override
	public Integer getEnumAbbreviation() {
		return sex;
	}
}
