package com.sihus.core.enumerate;

import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

import com.sihus.core.model.LabelValue;
import com.sihus.core.util.LocaleUtil;
import com.sihus.core.util.StringUtils;

/**
 * Integer枚举辅助类。 <br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-05-20
 */
public class IntegerEnumHelper {

	public static final IntegerEnumAbbr valueOf(Class<? extends IntegerEnumAbbr> clazz,String name) {
		IntegerEnumAbbr result=null;
		if(clazz!=null && !StringUtils.isNull(name)) {
			if(Enum.class.isAssignableFrom(clazz)) {
				if(IntegerEnumAbbr.class.isAssignableFrom(clazz)) {
					Object[] objs =clazz.getEnumConstants();
					for(Object o:objs) {
						if(o.toString().equals(name.trim())) {
							result=(IntegerEnumAbbr)o;
							break;
						}
					}
					//throw new IllegalArgumentException("没有找到枚举类型 "+ clazz+"."+name);
				}
			} else {
				throw new NotEnumException();
			}
		}
		return result;
	}
	
	public static final IntegerEnumAbbr valueOfAbbr(Class<? extends IntegerEnumAbbr> clazz,String abbr) {
		IntegerEnumAbbr result=null;
		if(clazz!=null && !StringUtils.isNull(abbr)) {
			if(Enum.class.isAssignableFrom(clazz)) {
				if(IntegerEnumAbbr.class.isAssignableFrom(clazz)) {
					Object[] objs =clazz.getEnumConstants();
					for(Object o:objs) {
						if(((IntegerEnumAbbr)o).getEnumAbbreviation().equals(abbr.trim())) {
							result=(IntegerEnumAbbr)o;
							break;
						}
					}
					//throw new IllegalArgumentException("没有找到枚举类型 "+ clazz+"."+name);
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
				//throw new NotEnumException();
			}
		}
		return result;
	}
	
	public static final String getDescription(Enum<? extends IntegerEnumAbbr> eum,Locale locale) {
		String ret=null;
		if(null!=eum) {
			ret=LocaleUtil.getLocaleText(eum.getClass().getName(), locale,eum.name());
		}
		return ret;
	}
	
	public static final String getAbbrDescription(Enum<? extends IntegerEnumAbbr> eum,Locale locale) {
		IntegerEnumAbbr enumAbbr=null;
		if(eum!=null) {
			enumAbbr=(IntegerEnumAbbr)eum;
		}
		return LocaleUtil.getLocaleText(eum.getClass().getName()+"Abbr", locale,enumAbbr.getEnumAbbreviation().toString());
	}
	
	public static LabelValue wrap(Enum<? extends IntegerEnumAbbr> enumeration, Locale locale) {
		return new LabelValue(enumeration.name(), getDescription(enumeration, locale));

	}

	public static LabelValue[] wrap(Enum<? extends IntegerEnumAbbr>[] enumerations, Locale locale) {
		LabelValue[] result = null;
		if(enumerations!=null && enumerations.length>0) {
			result = new LabelValue[enumerations.length];
			for (int i = 0; i < enumerations.length; i++) {
				Enum<? extends IntegerEnumAbbr> enumeration = enumerations[i];
				result[i] = new LabelValue(getDescription(enumeration, locale),enumeration.toString());
			}
		}
		return result;
	}

	/**
	 * 把枚举对应的显示资源文字包装成数组。
	 * @param enumerations
	 * @param locale
	 * @return
	 */
	public static String[] wrap2Array(Enum<? extends IntegerEnumAbbr>[] enumerations, Locale locale) {
		String[] result = null;
		if(enumerations!=null && enumerations.length>0) {
			List<String> list=new ArrayList<String>(enumerations.length);
			for (int i = 0; i < enumerations.length; i++) {
				list.add(getDescription(enumerations[i], locale));
			}
			if(list.size()>0) {
				result=list.toArray(new String[0]);
			}
		}
		return result;
	}
}