package com.haina.beluga.album.domain.enumerate;

import com.sihus.core.enumerate.IntegerEnumAbbr;

/**
 * 用户相片大小枚举。<br/>
 * @author huangyongqiang
 *  @since 2010-05-22
 */
public enum UserPhotoSizeEnum implements IntegerEnumAbbr {
	
	/**
	 * 正常大小
	 */
	normal(0),
	
	/**
	 * 普通缩略图
	 */
	genMini(1);
	
	private int size;
	
	private UserPhotoSizeEnum(int size) {
		this.size=size;
	}
	
	@Override
	public Integer getEnumAbbreviation() {
		return size;
	}

}
