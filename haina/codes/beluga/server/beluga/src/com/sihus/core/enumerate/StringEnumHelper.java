package com.sihus.core.enumerate;

import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

import com.sihus.core.model.LabelValue;
import com.sihus.core.util.LocaleUtil;
import com.sihus.core.util.StringUtils;

/**
 * String枚举辅助类。 <br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-05-20
 */
public class StringEnumHelper {

	public static final StringEnumAbbr valueOf(Class<? extends StringEnumAbbr> clazz,String name) {
		StringEnumAbbr result=null;
		if(clazz!=null && !StringUtils.isNull(name)) {
			if(Enum.class.isAssignableFrom(clazz)) {
				if(StringEnumAbbr.class.isAssignableFrom(clazz)) {
					Object[] objs =clazz.getEnumConstants();
					for(Object o:objs) {
						if(o.toString().equals(name.trim())) {
							result=(StringEnumAbbr)o;
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
	
	public static final StringEnumAbbr valueOfAbbr(Class<? extends StringEnumAbbr> clazz,String abbr) {
		StringEnumAbbr result=null;
		if(clazz!=null && !StringUtils.isNull(abbr)) {
			if(Enum.class.isAssignableFrom(clazz)) {
				if(StringEnumAbbr.class.isAssignableFrom(clazz)) {
					Object[] objs =clazz.getEnumConstants();
					for(Object o:objs) {
						if(((StringEnumAbbr)o).getEnumAbbreviation().equals(abbr.trim())) {
							result=(StringEnumAbbr)o;
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
	
	public static final String getEnumName(Object enumeration) {
		String result=null;
		if(enumeration!=null) {
			if(Enum.class.isAssignableFrom(enumeration.getClass())) {
				if(StringEnumAbbr.class.isAssignableFrom(enumeration.getClass())) {
					result=((StringEnumAbbr)enumeration).getEnumAbbreviation();
				}
			} else {
				throw new NotEnumException();
			}
		}
		return result;
	}
	
	public static final String getDescription(Enum<? extends StringEnumAbbr> eum,Locale locale) {
		String ret=null;
		if(null!=eum) {
			ret=LocaleUtil.getLocaleText(eum.getClass().getName(), locale,eum.name());
		}
		return ret;
	}
	
	public static final String getAbbrDescription(Enum<? extends StringEnumAbbr> eum,Locale locale) {
		StringEnumAbbr enumAbbr=null;
		if(eum!=null) {
			enumAbbr=(StringEnumAbbr)eum;
		}
		return LocaleUtil.getLocaleText(eum.getClass().getName()+"Abbr", locale,enumAbbr.getEnumAbbreviation());
	}
	
	public static LabelValue wrap(Enum<? extends StringEnumAbbr> enumeration, Locale locale) {
		return new LabelValue(enumeration.name(), getDescription(enumeration, locale));

	}

	public static LabelValue[] wrap(Enum<? extends StringEnumAbbr>[] enumerations, Locale locale) {
		LabelValue[] result = null;
		if(enumerations!=null && enumerations.length>0) {
			result = new LabelValue[enumerations.length];
			for (int i = 0; i < enumerations.length; i++) {
				Enum<? extends StringEnumAbbr> enumeration = enumerations[i];
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
	public static String[] wrap2Array(Enum<? extends StringEnumAbbr>[] enumerations, Locale locale) {
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