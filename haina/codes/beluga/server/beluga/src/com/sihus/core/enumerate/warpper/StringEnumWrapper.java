package com.sihus.core.enumerate.warpper;

import java.util.Locale;

import com.sihus.core.enumerate.StringEnumHelper;
import com.sihus.core.model.LabelValue;

/**
 * 枚举包装接口String枚举实现类。 <br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-05-20
 */
public class StringEnumWrapper implements EnumWrapper {

	public LabelValue doWarp(Enum<?> enumeration, Locale locale) {
		LabelValue labelValue=null;
		if(enumeration!=null && locale!=null) {
			labelValue=new LabelValue(enumeration.name(),StringEnumHelper.getDescription(enumeration, locale));
		}
		return labelValue;
	}

	public LabelValue[] doWarp(Enum<?>[] enumerations, Locale locale) {
		LabelValue[] labelValues=null;
		if(enumerations!=null && locale!=null && enumerations.length>0) {
			int size=enumerations.length;
			labelValues = new LabelValue[size];
			for(int i=0;i<size;i++) {
				labelValues[i]=new LabelValue(enumerations[i].name(),StringEnumHelper.getDescription(enumerations[i], locale));
			}
		}
		return labelValues;
	}

}
