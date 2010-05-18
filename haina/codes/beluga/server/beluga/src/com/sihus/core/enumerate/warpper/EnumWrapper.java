package com.sihus.core.enumerate.warpper;

import java.util.Locale;

import com.sihus.core.model.LabelValue;

/**
 * 枚举包装接口。 <br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-05-20
 */
public interface EnumWrapper {

	/**
	 * 取得枚举的本地化信息。
	 * @param enumeration
	 * @param locale
	 * @return
	 */
	LabelValue doWarp(Enum<?> enumeration,Locale locale);
	
	/**
	 * 取得多个枚举的本地化信息。
	 * @param enumeration
	 * @param locale
	 * @return
	 */
	LabelValue[] doWarp(Enum<?>[] enumerations,Locale locale);
}
