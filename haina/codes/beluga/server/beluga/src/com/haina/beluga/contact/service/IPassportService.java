package com.haina.beluga.contact.service;

import java.util.Date;
import java.util.List;

import com.haina.beluga.contact.domain.ContactUser;

/**
 * 用户验证护照业务处理接口。<br/>
 * @author huangyongqiang
 * //@Version 1.0
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
	LoginPassport addPassport(ContactUser contactUser);
	
	/**
	 * 添加护照。<br/>
	 * @param loginName 登录名
	 * @param password 密码
	 * @param loginTime 登录时间
	 * @return
	 */
	LoginPassport addPassport(String loginName,String password,Date loginTime);
	
	/**
	 * 更新护照。<br/>
	 * @param passport 护照
	 * @return
	 */
	LoginPassport updatePassport(String passport);
	
	/**
	 * 保持护照。<br/>
	 * 护照的保持心跳功能。
	 * @param passport 护照
	 * @return
	 */
	LoginPassport keepPassport(String passport);
	
	/**
	 * 取得登录护照对象。<br/>
	 * @param passport 护照
	 * @return
	 */
	LoginPassport getLoginPassport(String passport);
	
	/**
	 * 是否过期的护照。<br/>
	 * @param passport 护照
	 * @return
	 */
	boolean isExpiredPassport(String passport);
	
	/**
	 * 是否过期的护照。<br/>
	 * @param loginPassport 护照对象
	 * @return
	 */
	boolean isExpiredPassport(LoginPassport loginPassport); 
	
	/**
	 * 删除单个护照。<br/>
	 * @param passport 护照
	 * @return
	 */
	boolean removePassport(String passport);
	
	/**
	 * 删除所有护照。<br/>
	 * @return
	 */
	boolean removeAllPassport();
	
	/**
	 * 是否过期的登录。<br/>
	 * @param passport 登录名
	 * @return
	 */
	boolean isExpiredLogin(String passport);
	
	/**
	 * 使登录失效。<br/>
	 * @param passport
	 * @return
	 */
	boolean expireLogin(String passport);
	
	/**
	 * 通过登录名取得登录护照对象。<br/>
	 * @param loginName
	 * @return
	 */
	LoginPassport getLoginPassportByLoginName(String loginName);
	
	/**
	 * 通过登录名和密码取得登录护照对象。<br/>
	 * @param loginName
	 * @return
	 */
	LoginPassport getLoginPassportByLoginNameAndPwd(String loginName,String password);
	
	/**
	 * 取得目前的护照数量。<br/>
	 * @return
	 */
	int getPassportQuantity();
	
	/**
	 * 清楚超期的护照。<br/>
	 * @return 被清除了护照的登录名
	 */
	List<String> clearExpiredPassport();
}
