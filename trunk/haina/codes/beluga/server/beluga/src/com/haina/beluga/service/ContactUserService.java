package com.haina.beluga.service;

import java.util.Date;
import java.util.List;
import java.util.Set;

import org.springframework.stereotype.Service;

import com.haina.beluga.core.service.BaseSerivce;
import com.haina.beluga.core.util.StringUtils;
import com.haina.beluga.dao.IContactUserDao;
import com.haina.beluga.domain.ContactUser;
import com.haina.beluga.domain.UserProfile;
import com.haina.beluga.domain.UserProfileExt;

/**
 * 联系人用户业务处理接口实现类。<br/>
 * 
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-07-05
 */

@Service(value="contactUserService")
public class ContactUserService extends BaseSerivce<IContactUserDao,ContactUser,String> implements
		IContactUserService {

	private IContactUserDao contactUserDao=this.getBaseDao();
	
	@Override
	public ContactUser addContactUser(String loginName, String password,
			String mobile,Integer userStatus,String userIp) {
		if(StringUtils.isNull(loginName) || StringUtils.isNull(mobile)) {
			return null;
		}
		ContactUser contactUser=contactUserDao.getContactUserByLoginName(loginName);
		if(null==contactUser) {
			/*新用户。*/
			contactUser=contactUserDao.getContactUserByMobile(mobile);
			if(null!=contactUser && contactUser.getValidFlag()) {
				return null;
			}
			Date now=new Date();
			contactUser=new ContactUser();
			contactUser.setLoginName(loginName);
			contactUser.setPassword(password);
			contactUser.setMobile(mobile);
			contactUser.setRegisterTime(now);
			contactUser.setLastLoginTime(now);
			contactUser.setValidFlag(Boolean.TRUE);
			contactUser.setUserStatus(userStatus);
			if(userStatus!=null && userStatus.equals(ContactUser.USER_STATUS_ONLINE)) {
				contactUser.setLastLoginIp(userIp);
			}
			UserProfile userProfile=new UserProfile();
			userProfile.setTelPref(mobile);
			
			contactUser.setUserProfile(userProfile);
			userProfile.setContactUser(contactUser);
			contactUserDao.create(contactUser);
			return contactUser;
		} else {
			contactUser=this.editContactUserToValid(loginName, password, mobile, userStatus, userIp);
			return contactUser;
		}
	}

	@Override
	public ContactUser editContactUserToValid(ContactUser contactUser) {
		if(null==contactUser) {
			return null;
		}
		if(contactUser.isNew()) {
			return null;
		}
		ContactUser tempContactUser=contactUserDao.read(contactUser.getId());
		if(!tempContactUser.getPassword().equals(contactUser.getPassword())) {
			return null;
		}
		if(!contactUser.getValidFlag()) {
			contactUser.setValidFlag(Boolean.TRUE);
			contactUserDao.update(contactUser);
		}
		return contactUser;
	}
	
	@Override
	public ContactUser editContactUserToValid(String loginName, String password,
			String mobile,Integer userStatus,String userIp) {
		if(StringUtils.isNull(loginName) || StringUtils.isNull(password) ||
				StringUtils.isNull(mobile)) {
			return null;
		}
		ContactUser contactUser=contactUserDao.getContactUserByLoginName(loginName);
		if(null==contactUser || !contactUser.getPassword().equals(password)) {
			return null;
		}
		if(contactUser.getValidFlag()) {
			return contactUser;
		}
		contactUser.setMobile(mobile);
		contactUser.setValidFlag(Boolean.TRUE);
		contactUser.setUserStatus(userStatus);
		if(userStatus!=null && userStatus.equals(ContactUser.USER_STATUS_ONLINE)) {
			contactUser.setLastLoginIp(userIp);
			contactUser.setLastLoginTime(new Date());
		}
		contactUser.getUserProfile().setTelPref(mobile);
		contactUserDao.update(contactUser);
		return contactUser;
	}
	
	@Override
	public List<ContactUser> getContactUser(int first, int maxSize) {
		return contactUserDao.getModelByPage(null, first, maxSize);
	}

	@Override
	public ContactUser getContactUserById(String userId) {
		return contactUserDao.read(userId);
	}

	@Override
	public ContactUser getContactUserByLoginName(String loginName) {
		return contactUserDao.getContactUserByLoginName(loginName);
	}

	@Override
	public List<ContactUser> getInvalidContactUser(int first, int maxSize) {
		ContactUser contactUser=new ContactUser();
		contactUser.setValidFlag(Boolean.FALSE);
		return contactUserDao.getModelByPage(contactUser, first, maxSize);
	}

	@Override
	public UserProfile getUserProfileById(String id) {
		ContactUser contactUser=contactUserDao.read(id);
		return contactUser!=null ? contactUser.getUserProfile() : null;
	}

	@Override
	public UserProfile getUserProfileByLoginName(String loginName) {
		ContactUser contactUser=contactUserDao.getContactUserByLoginName(loginName);
		return contactUser!=null ? contactUser.getUserProfile() : null;
	}

	@Override
	public Set<UserProfileExt> getUserProfileExtById(String id) {
		ContactUser contactUser=contactUserDao.read(id);
		return contactUser!=null ? contactUser.getUserProfileExts() : null;
	}

	@Override
	public Set<UserProfileExt> getUserProfileExtByLoginName(String loginName) {
		ContactUser contactUser=contactUserDao.getContactUserByLoginName(loginName);
		return contactUser!=null ? contactUser.getUserProfileExts() : null;
	}

	@Override
	public List<ContactUser> getValidContactUser(int first, int maxSize) {
		ContactUser contactUser=new ContactUser();
		contactUser.setValidFlag(Boolean.TRUE);
		return contactUserDao.getModelByPage(contactUser, first, maxSize);
	}

	@Override
	public ContactUser editContactUserToOffline(ContactUser contactUser) {
		ContactUser user=contactUser;
		if(null!=user && user.isOnline()) {
			user.setUserStatus(ContactUser.USER_STATUS_OFFLINE);
		}
		this.contactUserDao.update(user);
		return user;
	}

	@Override
	public ContactUser editContactUserToOnline(String loginName, String password,
			String userLoginIp) {
		if(StringUtils.isNull(loginName) || StringUtils.isNull(password)) {
			return null;
		}
		ContactUser contactUser=contactUserDao.getContactUserByLoginName(loginName);
		if(null==contactUser) {
			return null;
		}
		if(!contactUser.getValidFlag()) {
			return contactUser;
		}
		if(contactUser.getUserStatus().equals(ContactUser.USER_STATUS_OFFLINE)) {
			contactUser.setUserStatus(ContactUser.USER_STATUS_ONLINE);
			contactUser.setLastLoginIp(userLoginIp);
			contactUser.setLastLoginTime(new Date());
			contactUserDao.update(contactUser);
		}
		return contactUser;
	}

	@Override
	public ContactUser editContactUserToOffline(String loginName) {
		if(StringUtils.isNull(loginName)) {
			return null;
		}
		ContactUser contactUser=contactUserDao.getContactUserByLoginName(loginName);
		if(null==contactUser) {
			return null;
		}
		if(!contactUser.getValidFlag()) {
			return contactUser;
		}
		if(contactUser.getUserStatus().equals(ContactUser.USER_STATUS_ONLINE)) {
			contactUser.setUserStatus(ContactUser.USER_STATUS_OFFLINE);
			contactUserDao.update(contactUser);
		}
		return contactUser;
	}

}
