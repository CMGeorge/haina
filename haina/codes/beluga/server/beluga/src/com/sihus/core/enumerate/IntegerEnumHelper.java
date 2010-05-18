package com.sihus.core.enumerate;

import java.util.Locale;

import com.sihus.core.util.LocaleUtil;

/**
 * Integer枚举辅助类。 <br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-05-20
 */
public class IntegerEnumHelper {

	public static final Object valueOf(Class<?> clazz,String name) {
		Object result=null;
		if(clazz!=null && name!=null) {
			if(Enum.class.isAssignableFrom(clazz)) {
				if(IntegerEnumAbbr.class.isAssignableFrom(clazz)) {
					Object[] objs =clazz.getEnumConstants();
					for(Object o:objs) {
						if(((IntegerEnumAbbr)o).getEnumAbbreviation().equals(Integer.valueOf(name))) {
							result=o;
							break;
						}
					}
					if(result == null)
						throw new IllegalArgumentException("没有找到枚举类型 "+ clazz+"."+name);
				}
			} else {
				throw new NotEnumException();
			}
		}
		return result;
	}
	
	public static final Integer getEnumName(Object enumeration) {
		Integer result=null;
		if(enumeration!=null) {
			if(Enum.class.isAssignableFrom(enumeration.getClass())) {
				if(IntegerEnumAbbr.class.isAssignableFrom(enumeration.getClass())) {
					result=((IntegerEnumAbbr)enumeration).getEnumAbbreviation();
				}
			} else {
				throw new NotEnumException();
			}
		}
		return result;
	}
	
	public static final String getDescription(Enum<?> eum,Locale locale) {
		return LocaleUtil.getLocaleText(eum.getClass().getName(), locale,eum.name());
	}
}