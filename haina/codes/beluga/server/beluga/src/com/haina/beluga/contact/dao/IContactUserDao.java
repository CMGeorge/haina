package com.haina.beluga.contact.dao;

import java.util.Date;
import java.util.List;

import org.hibernate.criterion.DetachedCriteria;

import com.haina.beluga.contact.domain.ContactUser;
import com.sihus.core.dao.IBaseDao;

/**
 * 联系人用户领域模型类Dao访问接口。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-06-29
 */
public interface IContactUserDao extends IBaseDao<ContactUser,String> {
	
	/**
	 * 通过登录名得到有效用户。<br/>
	 * @param loginName
	 * @return
	 */
	public ContactUser getValidUserByLoginName(String loginName);
	
	/**
	 * 通过登录名得到无效用户。<br/>
	 * @param loginName
	 * @return
	 */
	public ContactUser getInvalidUserByLoginName(String loginName);
	
	/**
	 * 通过手机号码得到有效用户。<br/>
	 * @param mobile
	 * @return
	 */
	public ContactUser getValidUserByMobile(String mobile);
	
	/**
	 * 通过密码和登录名得到有效用户。<br/>
	 * 密码必须放第一位。
	 * @param password
	 * @param loginName
	 * @return
	 */
	public ContactUser getValidUserByPwdAndLoginName(String password,String loginName);
	
	/**
	 * 通过密码和登录名得到无效用户。<br/>
	 * 密码必须放第一位。
	 * @param password
	 * @param loginName
	 * @return
	 */
	public ContactUser getInvalidUserByPwdAndLoginName(String password,String loginName);
	
	/**
	 * 设置用户为无效，即逻辑删除。<br/>
	 * @param contactUser
	 */
	public void deleteToInvalid(ContactUser contactUser);

	
	/**
	 * 通过手机号码或登录名得到用户。<br/>
	 * @param mobile
	 * @param loginName
	 * @return
	 */
	public List<ContactUser> getUserByMobileOrLoginName(String mobile,String loginName);
	
	/**
	 * 通过属性表达式查找用户。<br/>
	 * @param criteria
	 * @param begin
	 * @param count
	 * @return
	 */
	public List<ContactUser> getUserByHibernateCriteria(DetachedCriteria criteria,int begin, int count);
	
	/**
	 * 通过属性表达式查找用户。<br/>
	 * @param criteria
	 * @return
	 */
	public List<ContactUser> getUserByHibernateCriteria(DetachedCriteria criteria);
	
	/**
	 * 通过手机号码或登录名取得用户。<br/>
	 * @param mobile
	 * @param loginName
	 * @return
	 */
	public List<ContactUser> getInvalidUserByMobileOrLoginName(String mobile, String loginName);
	
	/**
	 * 通过参考用户查找用户。<br/>
	 * @param contactUser
	 * @return
	 */
	public List<ContactUser> getUserByExample(ContactUser contactUser);
	
	/**
	 * 设置多个用户离线。<br/>
	 * @param loginNames 登录名
	 * @return
	 */
	public int editToOffline(final List<String> loginNames);
	
	/**
	 * 设置用户离线。<br/>
	 * @param loginName 登录名
	 * @return
	 */
	public int editToOffline(String loginName);
	
	/**
	 * 设置用户在线。<br/>
	 * @param loginName 登录名
	 * @param password 密码
	 * @param userLoginIp 用户IP地址
	 * @param onlineTime 上线的时间
	 * @return
	 */
	public int editToOnline(String loginName, String password,String userLoginIp,Date onlineTime);
	
}
