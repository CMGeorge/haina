package com.sihus.core.util;

import java.util.Locale;
import java.util.ResourceBundle;


/**
 * ��本地化实用类。 <br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-05-20
 */
public final class LocaleUtil {

	/**
	 * 获取本地化资源文字。 <br/>
	 * @param resourceBaseName 本地化资源的基本名称
	 * @param locale 本地化对象
	 * @param key 资源的key
	 * @return 显示文字
	 */
	public static String getLocaleText(String resourceBaseName,Locale locale,String key) {
		String text=null;
		if(resourceBaseName!=null && locale!=null && key!=null) {
			text=ResourceBundle.getBundle(resourceBaseName, locale).getString(key);
		}
		return text;
	}
	
	/**
	 * 取得可用的本地化信息。 <br/>
	 * @return 
	 */
	public static Locale[] getAvailableLocales() {
		return Locale.getAvailableLocales();
	}
}
