package com.haina.beluga.contact.domain.enumerate;

import com.sihus.core.enumerate.hibernate.EnumIntegerType;


/**
 * 性别枚举的Hibernate映射类型。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-07-01
 */
public class SexEnumHibernateType extends EnumIntegerType {

	@Override
	public Class getEnumClass() {
		return SexEnum.class;
	}

}
