package com.haina.beluga.webservice;

import com.haina.beluga.aspect.LoginPassport;
import com.haina.beluga.aspect.PassportManager;
import com.sihus.core.util.StringUtils;

/**
 * 所有控制器的基类。<br/>
 * @author huangyongqiang
 * @since 2010-06-12
 */
public class BaseController {

	/**
	 * 通过登录令牌取得登录用户名即email
	 * @param passport 登录令牌
	 * @return
	 */
	protected String getLoginNameOfPassport(String passport) {
		if(StringUtils.isNull(passport)) {
			return null;
		}
		LoginPassport loginPassport = PassportManager.getLoginPassport(passport);
		if(null==loginPassport) {
			return null;
		}
		return loginPassport.getLoginName();
	}
	
}
