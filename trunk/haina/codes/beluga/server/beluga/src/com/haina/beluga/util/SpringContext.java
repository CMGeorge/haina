package com.haina.beluga.util;

import java.util.Locale;

import javax.annotation.Resource;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.InitializingBean;
import org.springframework.context.ApplicationContext;
import org.springframework.stereotype.Component;

import com.sihus.core.util.StringUtils;

/**
 * Spring上下文实用类。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-06-29
 */

@Component(value="springContext")
public class SpringContext implements InitializingBean {

	private static final transient Log LOG = LogFactory.getLog(SpringContext.class);
	
	private static SpringContext instance;

	@Resource
	private ApplicationContext context;
	
	//private ResourceBundleMessageSource messageSource;

	private SpringContext() {
		//this.appContext = new ClassPathXmlApplicationContext("configs/beluga-application.xml");
	}

	public ApplicationContext getContext() {
		return context;
	}

	@Override
	public void afterPropertiesSet() throws Exception {
		instance=this;
		//this.messageSource=(ResourceBundleMessageSource)this.getBean("messageSource");
		LOG.info(">>>>>>>>>>>>>>>>initialize SpringContext successfully>>>>>>>>>>>>>>>>>>>");
		//LOG.info(MessageFormat.format(">>>>>>>>>>>>>>>>spring context {0}>>>>>>>>>>>>>>>>>>>", context));
	}

	public synchronized static SpringContext getInstance() {
		return instance;
	}
	
	public Object getBean(String name) {
		Object object=null;
		if(!StringUtils.isNull(name)) {
			object=this.context.getBean(name);
			//LOG.info(MessageFormat.format(">>>>>>>>>>>>>>>>object class {0}>>>>>>>>>>>>>>>>>>>",object.getClass()));
		}
		return object;
	}
	
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
			message=this.context.getMessage(key, parameters, messageLocale);
		}
		return message;
	}
}
