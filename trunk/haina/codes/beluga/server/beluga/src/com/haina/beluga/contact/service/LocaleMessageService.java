package com.haina.beluga.contact.service;

import java.util.Locale;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.support.ResourceBundleMessageSource;
import org.springframework.stereotype.Service;

import com.sihus.core.util.StringUtils;

/**
 * 本地化信息业务接口实现类。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-07-05
 */

@Service(value="localeMessageService")
public class LocaleMessageService implements ILocaleMessageService {
	
	@Autowired(required=true)
	private ResourceBundleMessageSource resourceBundleMessageSource;

	public String getI18NMessage(String key) {
		return getI18NMessage(key,null,null);
	}
	
	public String getI18NMessage(String key,Locale locale) {
		return getI18NMessage(key,null,locale);
	}
	
	public String getI18NMessage(String key,Object[] parameters,Locale locale) {
		String message=null;
		if(!StringUtils.isNull(key)) {
			Locale messageLocale=locale;
			if(null==messageLocale) {
				messageLocale=Locale.getDefault();
			}
			message=resourceBundleMessageSource.getMessage(key, parameters, messageLocale);
		}
		return message;
	}

}
