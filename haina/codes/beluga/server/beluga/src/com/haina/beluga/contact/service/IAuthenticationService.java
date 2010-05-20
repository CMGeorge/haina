package com.haina.beluga.contact.service;

/**
 * 认证业务处理接口。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-07-05
 */
public interface IAuthenticationService {
	
	public String getProxyLoginUrl();

	public String getProxyRegisterUrl();

	public String getLoginUrl();

	public String getRegisterUrl();

	public String getProxyLogoutUrl();

	public String getLogoutUrl();

	public String getAppCode();
	
	public void proxyRegister(String email, String password, String mobile,
			String description, String registerIp,String lang);
}
