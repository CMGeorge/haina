package com.haina.beluga.contact.service;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import org.springframework.stereotype.Service;

import com.haina.beluga.contact.dao.IContactUserDao;
import com.haina.beluga.contact.domain.ContactUser;
import com.haina.beluga.contact.domain.UserProfile;
import com.haina.beluga.contact.domain.enumerate.SexEnum;
import com.haina.beluga.contact.dto.ContactUserDto;
import com.haina.beluga.webservice.IStatusCode;
import com.haina.beluga.webservice.data.hessian.HessianRemoteReturning;
import com.sihus.core.service.BaseSerivce;
import com.sihus.core.util.DESUtil;
import com.sihus.core.util.DateUtil;
import com.sihus.core.util.StringUtils;

/**
 * 联系人用户hessian协议业务处理接口实现类。<br/>
 * 
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-07-05
 */

@Service(value="contactUserHessianService")
public class ContactUserHessianService extends BaseSerivce<IContactUserDao,ContactUser,String> implements
	IContactUserHessianService {

	
	@Override
	public HessianRemoteReturning addContactUser(String loginName, String password,
			String mobile,Integer userStatus,String userIp, Date time) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		if(StringUtils.isNull(loginName)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		if(StringUtils.isNull(mobile)) {
			ret.setStatusCode(IStatusCode.INVALID_MOBILE);
			return ret;
		}
		List<ContactUser> list=getBaseDao().getUserByMobileOrLoginName(mobile, loginName);
		ContactUser contactUser=null;
		if(null==list || list.size()<1) {
			/*不存在同名的用户。*/
			Integer status=(userStatus!=null ? userStatus : ContactUser.USER_STATUS_OFFLINE);
			Date addedTime=(time!=null ? time : new Date());
			
			contactUser=new ContactUser();
			contactUser.setLoginName(loginName.trim());
			contactUser.setPassword(DESUtil.encrypt(password.trim()));
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
			ret.setStatusCode(IStatusCode.SUCCESS);
			return ret;
		} else {
			for(ContactUser user : list) {
				if(user.getValidFlag()) {
					/*有效用户。*/
					/*说明登录名或手机号码重复。*/
					ret.setStatusCode(IStatusCode.LOGINNAME_OR_MOBILE_EXISTENT);
					return ret;
				} else {
					/*无效用户。*/
					if(user.getLoginName().equals(loginName)) {
						/*老用户。*/
						return this.editContactUserToValid(loginName, password, mobile, userStatus, userIp);
					}
				}
			}
		}
		ret.setStatusCode(IStatusCode.LOGINNAME_OR_MOBILE_EXISTENT);
		return ret;
	}

	@Override
	public HessianRemoteReturning editContactUserToValid(ContactUser contactUser) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		if(null==contactUser || contactUser.isNew()) {
			ret.setStatusCode(IStatusCode.INVALID_CONTACT_USER);
			return ret;
		}
		if(StringUtils.isNull(contactUser.getLoginName()) 
				|| StringUtils.isNull(contactUser.getPassword())) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME_OR_PASSWORD);
			return ret;
		}
		ContactUser tempContactUser=getBaseDao().read(contactUser.getId());
		if(!tempContactUser.getLoginName().equals(contactUser.getLoginName()) 
				|| !tempContactUser.getPassword().equals(DESUtil.encrypt(contactUser.getPassword()))) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME_OR_PASSWORD);
		}
		if(!contactUser.getValidFlag()) {
			contactUser.setValidFlag(Boolean.TRUE);
			getBaseDao().update(contactUser);
		}
		ret.setStatusCode(IStatusCode.SUCCESS);
		return ret;
	}
	
	@Override
	public HessianRemoteReturning editContactUserToValid(String loginName, String password,
			String mobile,Integer userStatus,String userIp) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		if(StringUtils.isNull(loginName) || StringUtils.isNull(password) ||
				StringUtils.isNull(mobile)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME_OR_PASSWORD);
			return ret;
		}
		ContactUser contactUser=getBaseDao().getInvalidUserByPwdAndLoginName(encryptPassword(password),loginName);
		if(null==contactUser) {
			ret.setStatusCode(IStatusCode.INVALID_CONTACT_USER);
			return ret;
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
		ret.setStatusCode(IStatusCode.SUCCESS);
		return ret;
	}
	
	@Override
	public HessianRemoteReturning getContactUser(int first, int maxSize) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		List<ContactUserDto> userDtoList=new ArrayList<ContactUserDto>();
		List<ContactUser> list=getBaseDao().getModelByPage(null, first, maxSize);
		if(null==list || list.isEmpty()) {
			for(ContactUser user:list) {
				ContactUserDto userDto=new ContactUserDto(user.getLoginName(), user.getPassword(),
						user.getMobile(), DateUtil.formatDateTime("yyyy-MM-dd HH:mm:ss", user.getLastLoginTime()),
						user.getLastLoginIp(), user.getDescription());
				userDtoList.add(userDto);
			}
		}
		ret.setStatusCode(IStatusCode.SUCCESS);
		ret.setValue(userDtoList);
		return ret;
	}

	@Override
	public HessianRemoteReturning getContactUserById(String userId) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		ContactUser contactUser=getBaseDao().read(userId);
		if(null!=contactUser) {
			ret.setStatusCode(IStatusCode.SUCCESS);
			ContactUserDto userDto=new ContactUserDto(contactUser.getLoginName(), contactUser.getPassword(),
					contactUser.getMobile(), DateUtil.formatDateTime("yyyy-MM-dd HH:mm:ss", contactUser.getLastLoginTime()),
					contactUser.getLastLoginIp(), contactUser.getDescription());
			ret.setValue(userDto);
		} else {
			ret.setStatusCode(IStatusCode.INVALID_CONTACT_USER);
		}
		return ret;
	}

	@Override
	public HessianRemoteReturning getContactUserByLoginName(String loginName) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		ContactUser contactUser=getBaseDao().getValidUserByLoginName(loginName);
		if(null!=contactUser) {
			ret.setStatusCode(IStatusCode.SUCCESS);
			ContactUserDto userDto=new ContactUserDto(contactUser.getLoginName(), contactUser.getPassword(),
					contactUser.getMobile(), DateUtil.formatDateTime("yyyy-MM-dd HH:mm:ss", contactUser.getLastLoginTime()),
					contactUser.getLastLoginIp(), contactUser.getDescription());
			ret.setValue(userDto);
		} else {
			ret.setStatusCode(IStatusCode.INVALID_CONTACT_USER);
		}
		return ret;
	}

	@Override
	public HessianRemoteReturning getInvalidContactUser(int first, int maxSize) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		ContactUser contactUser=new ContactUser();
		contactUser.setValidFlag(Boolean.FALSE);
		List<ContactUserDto> userDtoList=new ArrayList<ContactUserDto>();
		List<ContactUser> list=getBaseDao().getModelByPage(contactUser, first, maxSize);
		if(null==list || list.isEmpty()) {
			for(ContactUser user:list) {
				ContactUserDto userDto=new ContactUserDto(user.getLoginName(), user.getPassword(),
						user.getMobile(), DateUtil.formatDateTime("yyyy-MM-dd HH:mm:ss", user.getLastLoginTime()),
						user.getLastLoginIp(), user.getDescription());
				userDtoList.add(userDto);
			}
		}
		ret.setStatusCode(IStatusCode.SUCCESS);
		ret.setValue(userDtoList);
		return ret;
	}

	@Override
	public HessianRemoteReturning getValidContactUser(int first, int maxSize) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		ContactUser contactUser=new ContactUser();
		contactUser.setValidFlag(Boolean.TRUE);
		List<ContactUserDto> userDtoList=new ArrayList<ContactUserDto>();
		List<ContactUser> list=getBaseDao().getModelByPage(contactUser, first, maxSize);
		if(null==list || list.isEmpty()) {
			for(ContactUser user:list) {
				ContactUserDto userDto=new ContactUserDto(user.getLoginName(), user.getPassword(),
						user.getMobile(), DateUtil.formatDateTime("yyyy-MM-dd HH:mm:ss", user.getLastLoginTime()),
						user.getLastLoginIp(), user.getDescription());
				userDtoList.add(userDto);
			}
		}
		ret.setStatusCode(IStatusCode.SUCCESS);
		ret.setValue(userDtoList);
		return ret;
	}

	@Override
	public HessianRemoteReturning editContactUserToOffline(ContactUser contactUser) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		ContactUser user=contactUser;
		int result=0;
		if(null!=user && user.isValid()) {
			result=this.getBaseDao().editToOffline(contactUser.getLoginName());
		}
		if(result<1) {
			ret.setStatusCode(IStatusCode.INVALID_CONTACT_USER);
			return ret;
		}
		ret.setStatusCode(IStatusCode.SUCCESS);
		return ret;
	}

	@Override
	public HessianRemoteReturning editContactUserToOnline(String loginName, String password,
			String userLoginIp,Date onlineTime) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		if(StringUtils.isNull(loginName) || StringUtils.isNull(password)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME_OR_PASSWORD);
			return ret;
		}
		int result=getBaseDao().editToOnline(loginName, DESUtil.encrypt(password), userLoginIp,onlineTime);
		if(result<1) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME_OR_PASSWORD);
			return ret;
		}
		ret.setStatusCode(IStatusCode.SUCCESS);
		return ret;
	}

	@Override
	public HessianRemoteReturning editContactUserToOffline(String loginName) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		if(StringUtils.isNull(loginName)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		int result=getBaseDao().editToOffline(loginName);
		if(result<1) {
			ret.setStatusCode(IStatusCode.INVALID_CONTACT_USER);
			return ret;
		}
		ret.setStatusCode(IStatusCode.SUCCESS);
		return ret;
	}
	
	@Override
	public HessianRemoteReturning editContactUserToOffline(List<String> loginNames) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		if(null==loginNames || loginNames.isEmpty()) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		int result=getBaseDao().editToOffline(loginNames);
		if(result<1) {
			ret.setStatusCode(IStatusCode.INVALID_CONTACT_USER);
			return ret;
		}
		ret.setStatusCode(IStatusCode.SUCCESS);
		return ret;
	}
	
	@Override
	public HessianRemoteReturning addContactUserLoginNumber(ContactUser contactUser) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		ContactUser user=contactUser;
		if(null!=user && user.isValid()) {
			user.setLoginNumber(user.getLoginNumber()!=null ? user.getLoginNumber()+1 : 1);
			this.getBaseDao().update(user);
		}
		ret.setStatusCode(IStatusCode.SUCCESS);
		return ret;
	}

	@Override
	public HessianRemoteReturning editLoginName(String loginName, String newLoginName) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		if(StringUtils.isNull(loginName) || StringUtils.isNull(newLoginName)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		ContactUser contactUser=getBaseDao().getValidUserByLoginName(loginName);
		if(null==contactUser) {
			ret.setStatusCode(IStatusCode.INVALID_CONTACT_USER);
			return ret;
		}
		contactUser.setLoginName(newLoginName);
		contactUser.setLastLoginTime(new Date());
		contactUser.getUserProfile().setEmailPref(newLoginName);
		getBaseDao().update(contactUser);
		ContactUserDto userDto=new ContactUserDto(contactUser.getLoginName(), contactUser.getPassword(),
				contactUser.getMobile(), DateUtil.formatDateTime("yyyy-MM-dd HH:mm:ss", contactUser.getLastLoginTime()),
				contactUser.getLastLoginIp(), contactUser.getDescription());
		ret.setStatusCode(IStatusCode.SUCCESS);
		ret.setValue(userDto);
		return ret;
	}

	@Override
	public HessianRemoteReturning editMobile(String loginName, String neoMobile) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		if(StringUtils.isNull(loginName) || StringUtils.isNull(neoMobile)) {
			ret.setStatusCode(IStatusCode.INVALID_MOBILE);
			return ret;
		}
		ContactUser contactUser=getBaseDao().getValidUserByLoginName(loginName);
		if(null==contactUser) {
			ret.setStatusCode(IStatusCode.INVALID_CONTACT_USER);
			return ret;
		}
		contactUser.setMobile(neoMobile);
		contactUser.setLastLoginTime(new Date());
		contactUser.getUserProfile().setTelPref(neoMobile);
		getBaseDao().update(contactUser);
		ContactUserDto userDto=new ContactUserDto(contactUser.getLoginName(), contactUser.getPassword(),
				contactUser.getMobile(), DateUtil.formatDateTime("yyyy-MM-dd HH:mm:ss", contactUser.getLastLoginTime()),
				contactUser.getLastLoginIp(), contactUser.getDescription());
		ret.setStatusCode(IStatusCode.SUCCESS);
		ret.setValue(userDto);
		return ret;
	}

	@Override
	public HessianRemoteReturning editPassword(String loginName, String oldPassword, String neoPassword) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		if(StringUtils.isNull(loginName) || StringUtils.isNull(oldPassword) 
				|| StringUtils.isNull(neoPassword)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME_OR_PASSWORD);
			return ret;
		}
		ContactUser contactUser=getBaseDao().getValidUserByPwdAndLoginName(encryptPassword(oldPassword), loginName);
		if(null==contactUser) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME_OR_PASSWORD);
			return ret;
		}
		contactUser.setPassword(encryptPassword(neoPassword));
		contactUser.setLastLoginTime(new Date());
		getBaseDao().update(contactUser);
		ContactUserDto userDto=new ContactUserDto(contactUser.getLoginName(), contactUser.getPassword(),
				contactUser.getMobile(), DateUtil.formatDateTime("yyyy-MM-dd HH:mm:ss", contactUser.getLastLoginTime()),
				contactUser.getLastLoginIp(), contactUser.getDescription());
		ret.setStatusCode(IStatusCode.SUCCESS);
		ret.setValue(userDto);
		return ret;
	}

//	private ContactUser encryptPassword(ContactUser contactUser) {
//		ContactUser user=contactUser;
//		if(user!=null && !StringUtils.isNull(user.getPassword())) {
//			user.setPassword(DESUtil.encrypt(user.getPassword().trim()));
//		}
//		return user;
//	}
	
	private String encryptPassword(String password) {
		if(!StringUtils.isNull(password)) {
			return DESUtil.encrypt(password.trim());
		} else {
			return null;
		}
	}

	@Override
	public HessianRemoteReturning editUserProfile(String loginName,
			String nickName, String name, Integer age, Integer sex,
			byte[] photo, String identification, Date brithday, String url,
			String signature, String emailPref, String telPref, String imPref,
			byte[] ring, String org, String title, String note) {
		HessianRemoteReturning ret=new HessianRemoteReturning();
		if(StringUtils.isNull(loginName)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		ContactUser contactUser=getBaseDao().getValidUserByLoginName(loginName);
		if(null==contactUser) {
			ret.setStatusCode(IStatusCode.INVALID_CONTACT_USER);
			return ret;
		}
		UserProfile userProfile=contactUser.getUserProfile();
		return ret;
	}
}
