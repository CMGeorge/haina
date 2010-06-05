package com.haina.beluga.album.domain.enumerate;

import com.sihus.core.enumerate.IntegerEnumAbbr;
import com.sihus.core.enumerate.hibernate.EnumIntegerType;

/**
 * 用户相片大小枚举的Hibernate映射类型。<br/>
 * @author huangyongqiang
 * @since 21009-05-22
 */
public class UserPhotoSizeEnumHibernateType extends EnumIntegerType {

	@Override
	public Class<? extends IntegerEnumAbbr> getEnumClass() {
		return UserPhotoSizeEnum.class;
	}

}
