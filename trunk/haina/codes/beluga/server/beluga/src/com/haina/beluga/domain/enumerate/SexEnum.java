package com.haina.beluga.domain.enumerate;

import com.haina.beluga.core.enumerate.IntegerEnumAbbr;

/**
 * 性别枚举类型。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-07-01
 */
public enum SexEnum implements IntegerEnumAbbr {
	
	/*分别表示男性，女性。*/
	male(1),famale(2);
	
	private Integer sex;
	
	private SexEnum(Integer sex) {
		this.sex=sex;
	}

	@Override
	public Integer getEnumAbbreviation() {
		return sex;
	}
}
