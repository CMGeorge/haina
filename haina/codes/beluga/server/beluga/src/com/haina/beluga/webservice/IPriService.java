package com.haina.beluga.webservice;

import java.io.Serializable;

import com.haina.beluga.webservice.data.Returning;

/**
 * 个人服务Api接口。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-06-29
 */

public interface IPriService extends Serializable {
	
	/**
	 * 联系人用户登录。<br/>
	 * @param loginName 用户的登录名
	 * @param password 密码
	 * @return 状态包括：
	 * 		1.成功 statusCode=0
	 * 		2.登录名或密码无效 statusCode = 1002
	 * 		value值为护照，只有成功才有
	 */
	Returning login(String loginName, String password);
	
	/**
	 * 联系人用户注册。<br/>
	 * 注册成功后的用户自动登录。
	 * @param loginName 用户的登录名
	 * @param password 密码
	 * @param mobile 移动电话号码
	 * @return 状态包括：
	 * 		1.成功 statusCode=0
	 * 		2.无效的手机号码 statusCode = 1010
	 * 		value:护照，只有成功才有
	 */
	Returning register(String loginName, String password, String mobile);
	
	/**
	 * 联系人用户通过护照退出。<br/>
	 * @param passport 登录护照
	 * @return 状态包括：
	 * 		1.成功 statusCode=0
	 * 		2.登录护照无效或已过期 statusCode = 1008
	 * 		3.无效的登录名称 statusCode = 1009
	 * 		4.用户不存在或用户未激活 statusCode = 1007
	 * 		5.登录名或手机号码已存在 statusCode = 1007
	 */
	Returning logoutByPsssport(String passport);
	
	/**
	 * 联系人用户通过登录名退出。<br/>
	 * @param loginName 登录名
	 * @return 状态包括：
	 * 		1.成功 statusCode=0
	 * 		2.无效的登录名称 statusCode = 1009
	 * 		4.用户不存在或用户未激活 statusCode = 1007
	 */
	Returning logoutByLoginName(String loginName);
	
	/**
	 * 修改密码。<br/>
	 * 
	 * @param passport 登录护照
	 * @param oldPassword 旧密码
	 * @param neoPassword 新密码
	 */
	Returning editPassword(String passport, String oldPassword, String neoPassword);

	/**
	 * 修改手机号码。<br/>
	 * 
	 * @param passport 登录护照
	 * @param neoMobile 新手机号码
	 */
	Returning editMobile(String passport, String neoMobile);
	
	/**
	 * 修改登录名（同时修改电子邮件）。<br/>
	 * @param passport 登录护照
	 * @param neoEmail 新的登录名
	 * @return 操作结果
	 */
	Returning editLoginName(String passport, String newLoginName);
	
	/**
	 * 护照的保持心跳功能。<br/>
	 * @param passport 登录护照
	 * @return 操作结果
	 */
	Returning editPassportToKeepHeart(String passport);
}
