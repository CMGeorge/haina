package com.haina.beluga.contact.service;

import java.util.Locale;

/**
 * 本地化信息业务接口。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-07-05
 */
public interface ILocaleMessageService {

	public String getI18NMessage(String key);
	
	public String getI18NMessage(String key,Locale locale);
	
	public String getI18NMessage(String key,Object[] parameters,Locale locale);
}
