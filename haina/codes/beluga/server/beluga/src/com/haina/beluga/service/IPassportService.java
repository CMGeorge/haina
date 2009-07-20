package com.haina.beluga.service;

import java.util.Date;
import java.util.List;

import com.haina.beluga.domain.ContactUser;

/**
 * 用户验证护照业务处理接口。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @data 2009-07-11
 *
 */
public interface IPassportService {

	/**
	 * 添加护照。<br/>
	 * @param contactUser 联系人用户
	 * @return
	 */
	public LoginPassport addPassport(ContactUser contactUser);
	
	/**
	 * 添加护照。<br/>
	 * @param loginName 登录名
	 * @param password 密码
	 * @param loginTime 登录时间
	 * @return
	 */
	public LoginPassport addPassport(String loginName,String password,Date loginTime);
	
	/**
	 * 更新护照。<br/>
	 * @param passport 护照
	 * @return
	 */
	public LoginPassport updatePassport(String passport);
	
	/**
	 * 保持护照。<br/>
	 * 护照的保持心跳功能。
	 * @param passport 护照
	 * @return
	 */
	public LoginPassport keepPassport(String passport);
	
	/**
	 * 取得登录护照对象。<br/>
	 * @param passport 护照
	 * @return
	 */
	public LoginPassport getLoginPassport(String passport);
	
	/**
	 * 是否过期的护照。<br/>
	 * @param passport 护照
	 * @return
	 */
	public boolean isExpiredPassport(String passport);
	
	/**
	 * 是否过期的护照。<br/>
	 * @param loginPassport 护照对象
	 * @return
	 */
	public boolean isExpiredPassport(LoginPassport loginPassport); 
	
	/**
	 * 删除单个护照。<br/>
	 * @param passport 护照
	 * @return
	 */
	public boolean removePassport(String passport);
	
	/**
	 * 删除所有护照。<br/>
	 * @return
	 */
	public boolean removeAllPassport();
	
	/**
	 * 是否过期的登录。<br/>
	 * @param passport 登录名
	 * @return
	 */
	public boolean isExpiredLogin(String passport);
	
	/**
	 * 使登录失效。<br/>
	 * @param passport
	 * @return
	 */
	public boolean expireLogin(String passport);
	
	/**
	 * 通过登录名取得登录护照对象。<br/>
	 * @param loginName
	 * @return
	 */
	public LoginPassport getLoginPassportByLoginName(String loginName);
	
	/**
	 * 通过登录名和密码取得登录护照对象。<br/>
	 * @param loginName
	 * @return
	 */
	public LoginPassport getLoginPassportByLoginNameAndPwd(String loginName,String password);
	
	/**
	 * 取得目前的护照数量。<br/>
	 * @return
	 */
	public int getPassportQuantity();
	
	/**
	 * 清楚超期的护照。<br/>
	 * @return 被清除了护照的登录名
	 */
	public List<String> clearExpiredPassport();
}
