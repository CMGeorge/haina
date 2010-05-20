package com.haina.beluga.contact.service;

import java.util.Date;
import java.util.List;
import java.util.Set;

import com.haina.beluga.contact.dao.IContactUserDao;
import com.haina.beluga.contact.domain.ContactUser;
import com.haina.beluga.contact.domain.UserProfile;
import com.haina.beluga.contact.domain.UserProfileExt;
import com.sihus.core.service.IBaseSerivce;

/**
 * 联系人用户业务处理接口。<br/>
 * 
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-07-03
 */
public interface IContactUserService extends IBaseSerivce<IContactUserDao,ContactUser,String> {

	/**
	 * 添加联系人用户。<br/>
	 * @param loginName 登录名称。
	 * @param password 密码
	 * @param mobile 手机号码
	 * @param userStatus 用户状态
	 * @param userIp 用户使用的IP地址
	 * @param time 添加的时间
	 * @return
	 */
	public ContactUser addContactUser(String loginName, String password, String mobile,Integer userStatus
			,String userIp, Date time);
	
	/**
	 * 设置联系人用户有效。<br/>
	 * @param contactUser 用户对象
	 * @return
	 */
	public ContactUser editContactUserToValid(ContactUser contactUser);
	
	/**
	 * 设置联系人用户有效。<br/>
	 * @param loginName 登录名
	 * @param password 密码
	 * @param mobile 手机号码
	 * @param userStatus 用户状态
	 * @param userIp 用户IP地址
	 * @return
	 */
	public ContactUser editContactUserToValid(String loginName, String password,
			String mobile,Integer userStatus,String userIp);
	
	/**
	 * 通过Id取得联系人用户信息。<br/>
	 * @param userId
	 * @return
	 */
	public ContactUser getContactUserById(String userId);
	
	/**
	 * 通过登录名称取得联系人用户信息。<br/>
	 * @param loginName
	 * @return
	 */
	public ContactUser getContactUserByLoginName(String loginName);
	
	/**
	 * 取得一定范围内的联系人用户。<br/>
	 * @param first 起始位置
	 * @param maxSize 最大数量
	 * @return
	 */
	public List<ContactUser> getContactUser(int first, int maxSize);
	
	/**
	 * 取得一定范围内的有效联系人用户。<br/>
	 * @param first 起始位置
	 * @param maxSize 最大数量
	 * @return
	 */
	public List<ContactUser> getValidContactUser(int first, int maxSize);
	
	/**
	 * 取得一定范围内的无效联系人用户。<br/>
	 * @param first 起始位置
	 * @param maxSize 最大数量
	 * @return
	 */
	public List<ContactUser> getInvalidContactUser(int first, int maxSize);
	
	/**
	 * 通过登录名得到用户详细信息。<br/>
	 * @param loginName
	 * @return
	 */
	public UserProfile getUserProfileByLoginName(String loginName);
	
	/**
	 * 通过登录名得到用户详细扩展信息。<br/>
	 * @param loginName
	 * @return
	 */
	public Set<UserProfileExt> getUserProfileExtByLoginName(String loginName);
	
	/**
	 * 通过用户Id得到用户详细信息。<br/>
	 * @param id
	 * @return
	 */
	public UserProfile getUserProfileById(String id);
	
	/**
	 * 通过用户Id得到用户详细扩展信息。<br/>
	 * @param id
	 * @return
	 */
	public Set<UserProfileExt> getUserProfileExtById(String id);
	
	/**
	 * 设置用户为离线状态。<br/>
	 * @param contactUser
	 * @return
	 */
	public ContactUser editContactUserToOffline(ContactUser contactUser);
	
	/**
	 * 设置用户为离线状态。<br/>
	 * @param loginName
	 * @return
	 */
	public ContactUser editContactUserToOffline(String loginName);
	
	/**
	 * 设置用户为在线状态。<br/>
	 * @param loginName
	 * @param password
	 * @param userLoginIp
	 * @param onlineTime
	 * @return
	 */
	public ContactUser editContactUserToOnline(String loginName, String password, String userLoginIp,Date onlineTime);
	
	/**
	 * 用户登录次数增加一次。<br/>
	 * @param contactUser
	 * @return
	 */
	public ContactUser addContactUserLoginNumber(ContactUser contactUser);
	
	/**
	 * 修改登录名称即修改电子邮件。<br/>
	 * @param loginName 现在登录名
	 * @param newLoginName 新的登录名
	 * @return
	 */
	public ContactUser editLoginName(String loginName, String newLoginName);
	
	/**
	 * 修改手机号码。<br/>
	 * @param loginName 登录名
	 * @param newMobile 新的手机号码
	 * @return
	 */
	public ContactUser editMobile(String loginName, String neoMobile);
	
	/**
	 * 修改密码。<br/>
	 * @param loginName 登录名
	 * @param oldPassword 旧密码
	 * @param neoPassword 新的密码
	 * @return
	 */
	public ContactUser editPassword(String loginName, String oldPassword, String neoPassword);
	
	
}
