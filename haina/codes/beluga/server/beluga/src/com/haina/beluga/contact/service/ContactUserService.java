package com.haina.beluga.contact.service;

import java.util.Date;
import java.util.List;
import java.util.Set;

import org.springframework.stereotype.Service;

import com.haina.beluga.contact.dao.IContactUserDao;
import com.haina.beluga.contact.domain.ContactUser;
import com.haina.beluga.contact.domain.UserProfile;
import com.haina.beluga.contact.domain.UserProfileExt;
import com.haina.beluga.contact.domain.enumerate.SexEnum;
import com.sihus.core.service.BaseSerivce;
import com.sihus.core.util.StringUtils;

/**
 * 联系人用户业务处理接口实现类。<br/>
 * 
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-07-05
 */

@Service(value="contactUserService")
public class ContactUserService extends BaseSerivce<IContactUserDao,ContactUser,String> implements
		IContactUserService {

//	private IContactUserDao contactUserDao=this.getBaseDao();
	
	@Override
	public ContactUser addContactUser(String loginName, String password,
			String mobile,Integer userStatus,String userIp, Date time) {
		if(StringUtils.isNull(loginName) || StringUtils.isNull(mobile)) {
			return null;
		}
		List<ContactUser> list=getBaseDao().getUserByMobileOrLoginName(mobile, loginName);
		ContactUser contactUser=null;
		if(null==list || list.size()<1) {
			/*不存在同名的用户。*/
			Integer status=(userStatus!=null ? userStatus : ContactUser.USER_STATUS_OFFLINE);
			Date addedTime=(time!=null ? time : new Date());
			
			contactUser=new ContactUser();
			contactUser.setLoginName(loginName.trim());
			contactUser.setPassword(password.trim());
			contactUser.setMobile(mobile);
			contactUser.setRegisterTime(addedTime);
			contactUser.setValidFlag(Boolean.TRUE);
			contactUser.setUserStatus(status);
			if(status.equals(ContactUser.USER_STATUS_ONLINE)) {
				contactUser.setLastLoginIp(userIp);
				contactUser.setLoginNumber(1);
				contactUser.setLastLoginTime(addedTime);
			}
			UserProfile userProfile=new UserProfile();
			userProfile.setTelPref(mobile);
			userProfile.setEmailPref(loginName);
			//TODO:setSex.
			userProfile.setSex(SexEnum.unknown);
			
			contactUser.setUserProfile(userProfile);
			userProfile.setContactUser(contactUser);
			getBaseDao().create(contactUser);
			return contactUser;
		} else {
			for(ContactUser user : list) {
				if(user.getValidFlag()) {
					/*有效用户。*/
					/*说明登录名或手机号码重复。*/
					return user;
				} else {
					/*无效用户。*/
					if(user.getLoginName().equals(loginName)) {
						/*老用户。*/
						contactUser=this.editContactUserToValid(loginName, password, mobile, userStatus, userIp);
						if(contactUser!=null) {
							return contactUser;
						}
					}
				}
			}
		}
		return null;
	}

	@Override
	public ContactUser editContactUserToValid(ContactUser contactUser) {
		if(null==contactUser) {
			return null;
		}
		if(contactUser.isNew()) {
			return null;
		}
		ContactUser tempContactUser=getBaseDao().read(contactUser.getId());
		if(!tempContactUser.getPassword().equals(contactUser.getPassword())) {
			return null;
		}
		if(!contactUser.getValidFlag()) {
			contactUser.setValidFlag(Boolean.TRUE);
			getBaseDao().update(contactUser);
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
		ContactUser contactUser=getBaseDao().getInvalidUserByPwdAndLoginName(password,loginName);
		if(null==contactUser) {
			return null;
		}
		Integer status=(userStatus!=null ? userStatus : ContactUser.USER_STATUS_OFFLINE);
		contactUser.setMobile(mobile);
		contactUser.setValidFlag(Boolean.TRUE);
		contactUser.setUserStatus(status);
		Date now=new Date();
		if(status.equals(ContactUser.USER_STATUS_ONLINE)) {
			contactUser.setLastLoginIp(userIp);
			contactUser.setLastLoginTime(now);
			contactUser.setLoginNumber(contactUser.getLoginNumber()!=null ? contactUser.getLoginNumber()+1 : 1);
		}
		contactUser.setLastUpdateTime(now);
		contactUser.getUserProfile().setTelPref(mobile);
		contactUser.getUserProfile().setEmailPref(loginName);
		getBaseDao().update(contactUser);
		return contactUser;
	}
	
	@Override
	public List<ContactUser> getContactUser(int first, int maxSize) {
		return getBaseDao().getModelByPage(null, first, maxSize);
	}

	@Override
	public ContactUser getContactUserById(String userId) {
		return getBaseDao().read(userId);
	}

	@Override
	public ContactUser getContactUserByLoginName(String loginName) {
		return getBaseDao().getValidUserByLoginName(loginName);
	}

	@Override
	public List<ContactUser> getInvalidContactUser(int first, int maxSize) {
		ContactUser contactUser=new ContactUser();
		contactUser.setValidFlag(Boolean.FALSE);
		return getBaseDao().getModelByPage(contactUser, first, maxSize);
	}

	@Override
	public UserProfile getUserProfileById(String id) {
		ContactUser contactUser=getBaseDao().read(id);
		return contactUser!=null ? contactUser.getUserProfile() : null;
	}

	@Override
	public UserProfile getUserProfileByLoginName(String loginName) {
		ContactUser contactUser=getBaseDao().getValidUserByLoginName(loginName);
		return contactUser!=null ? contactUser.getUserProfile() : null;
	}

	@Override
	public Set<UserProfileExt> getUserProfileExtById(String id) {
		ContactUser contactUser=getBaseDao().read(id);
		return contactUser!=null ? contactUser.getUserProfileExts() : null;
	}

	@Override
	public Set<UserProfileExt> getUserProfileExtByLoginName(String loginName) {
		ContactUser contactUser=getBaseDao().getValidUserByLoginName(loginName);
		return contactUser!=null ? contactUser.getUserProfileExts() : null;
	}

	@Override
	public List<ContactUser> getValidContactUser(int first, int maxSize) {
		ContactUser contactUser=new ContactUser();
		contactUser.setValidFlag(Boolean.TRUE);
		return getBaseDao().getModelByPage(contactUser, first, maxSize);
	}

	@Override
	public ContactUser editContactUserToOffline(ContactUser contactUser) {
		ContactUser user=contactUser;
		if(null!=user && user.isValid()) {
			user.setUserStatus(ContactUser.USER_STATUS_OFFLINE);
		}
		this.getBaseDao().update(user);
		return user;
	}

	@Override
	public ContactUser editContactUserToOnline(String loginName, String password,
			String userLoginIp,Date onlineTime) {
		if(StringUtils.isNull(loginName) || StringUtils.isNull(password)) {
			return null;
		}
		ContactUser contactUser=getBaseDao().getValidUserByPwdAndLoginName(password,loginName);
		if(null==contactUser) {
			return null;
		}
		if(contactUser.getUserStatus().equals(ContactUser.USER_STATUS_OFFLINE)) {
			contactUser.setUserStatus(ContactUser.USER_STATUS_ONLINE);
			contactUser.setLastLoginIp(userLoginIp);
			contactUser.setLastLoginTime(onlineTime!=null ? onlineTime : new Date());
			contactUser.setLoginNumber(contactUser.getLoginNumber()!=null ? contactUser.getLoginNumber()+1 : 1);
			getBaseDao().update(contactUser);
		}
		return contactUser;
	}

	@Override
	public ContactUser editContactUserToOffline(String loginName) {
		if(StringUtils.isNull(loginName)) {
			return null;
		}
		ContactUser contactUser=getBaseDao().getValidUserByLoginName(loginName);
		if(null==contactUser) {
			return null;
		}
		if(contactUser.getUserStatus().equals(ContactUser.USER_STATUS_ONLINE)) {
			contactUser.setUserStatus(ContactUser.USER_STATUS_OFFLINE);
			getBaseDao().update(contactUser);
		}
		return contactUser;
	}
	
	@Override
	public ContactUser addContactUserLoginNumber(ContactUser contactUser) {
		ContactUser user=contactUser;
		if(null!=user && user.isValid()) {
			user.setLoginNumber(user.getLoginNumber()!=null ? user.getLoginNumber()+1 : 1);
			this.getBaseDao().update(user);
		}
		return user;
	}

	@Override
	public ContactUser editLoginName(String loginName, String newLoginName) {
		if(StringUtils.isNull(loginName) || StringUtils.isNull(newLoginName)) {
			return null;
		}
		ContactUser contactUser=getBaseDao().getValidUserByLoginName(loginName);
		if(null==contactUser) {
			return null;
		}
		contactUser.setLoginName(newLoginName);
		contactUser.setLastLoginTime(new Date());
		contactUser.getUserProfile().setEmailPref(newLoginName);
		getBaseDao().update(contactUser);
		return contactUser;
	}

	@Override
	public ContactUser editMobile(String loginName, String neoMobile) {
		if(StringUtils.isNull(loginName) || StringUtils.isNull(neoMobile)) {
			return null;
		}
		ContactUser contactUser=getBaseDao().getValidUserByLoginName(loginName);
		if(null==contactUser) {
			return null;
		}
		contactUser.setMobile(neoMobile);
		contactUser.setLastLoginTime(new Date());
		contactUser.getUserProfile().setTelPref(neoMobile);
		getBaseDao().update(contactUser);
		return contactUser;
	}

	@Override
	public ContactUser editPassword(String loginName, String oldPassword, String neoPassword) {
		if(StringUtils.isNull(loginName) || StringUtils.isNull(oldPassword) 
				|| StringUtils.isNull(neoPassword)) {
			return null;
		}
		ContactUser contactUser=getBaseDao().getValidUserByLoginName(loginName);
		if(null==contactUser) {
			return null;
		}
		if(!contactUser.getPassword().equals(oldPassword)) {
			/*旧密码不正确，返回用户对象，用于调用方法判断。*/
			return contactUser;
		}
		contactUser.setPassword(neoPassword);
		contactUser.setLastLoginTime(new Date());
		getBaseDao().update(contactUser);
		return contactUser;
	}

}
