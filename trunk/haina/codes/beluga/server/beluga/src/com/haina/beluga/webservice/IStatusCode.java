package com.haina.beluga.webservice;

/**
 * 状态码常量接口。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-06-21
 */
public interface IStatusCode {

	/**
	 * 成功。<br/>
	 */
	int SUCCESS=0;
	
	/**
	 * 未知错误。<br/>
	 */
	int UNKNOW_ERROR=1000;
	
	/**
	 * 网络错误。<br/>
	 */
	int NETWORK_ERROR=1001;
	
	/**
	 * 登录名或密码无效。<br/>
	 */
	int INVALID_LOGINNAME_OR_PASSWORD=1002;
	
	/**
	 * 手机号码无效。<br/>
	 */
	int MOBILE_INVALID=1003;
	
	/**
	 * 用户已存在。<br/>
	 */
	int LOGINNAME_EXISTENT=1004;
	
	/**
	 * 注册成功，但暂时不能生成登录护照。<br/>
	 */
	int REGISTER_SUCCESS_PASSPORT_FAILD=1005;
	
	/**
	 * 登录失败，暂时不能生成登录护照。<br/>
	 */
	int LOGIN_PASSPORT_FAILD=1006;
	
	/**
	 * 用户不存在或用户未激活。<br/>
	 */
	int INVALID_CONTACT_USER=1007;
	
	/**
	 * 登录护照无效或已过期。<br/>
	 */
	int INVALID_LOGIN_PASSPORT=1008;
	
	/**
	 * 无效的登录名称。<br/>
	 */
	int INVALID_LOGINNAME=1009;
	
	/**
	 * 无效的手机号码。<br/>
	 */
	int INVALID_MOBILE=1010;
	
	/**
	 * 无效的密码。<br/>
	 */
	int INVALID_PASSWORD=1011;
	
	/**
	 * 错误的旧密码。<br/>
	 */
	int WRONG_OLD_PASSWORD=1012;
	
	/**
	 * 登录名或手机号码已存在。<br/>
	 */
	int LOGINNAME_OR_MOBILE_EXISTENT=1013;
	
	/**
	 * 无效的登录名称或手机号码。<br/>
	 */
	int INVALID_LOGINNAME_OR_MOBILE=1014;
	
	/**
	 * 请求相册列表出现异常。<br/>
	 */
	int REQUEST_USER_ALBUM_LIST_ERROR=3000;
	
	/**
	 * 无效的相册名称。<br/>
	 */
	int INVALID_ALBUM=3001;
}
