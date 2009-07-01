package com.haina.beluga.domain.enumerate;

import com.haina.beluga.core.enumerate.hibernate.EnumIntegerType;

/**
 * 性别枚举的Hibernate映射类型。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-07-01
 */
public class SexEnumHibernateType extends EnumIntegerType {

	@Override
	public Class getEnumClass() {
		return SexEnum.class;
	}

}
