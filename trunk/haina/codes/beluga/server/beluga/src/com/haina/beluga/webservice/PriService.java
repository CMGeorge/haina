package com.haina.beluga.webservice;

import java.util.Date;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import com.haina.beluga.aspect.LoginPassport;
import com.haina.beluga.aspect.PassportManager;
import com.haina.beluga.contact.domain.ContactUser;
import com.haina.beluga.contact.service.IContactUserHessianService;
import com.haina.beluga.webservice.data.Returning;
import com.sihus.core.util.DESUtil;
import com.sihus.core.util.StringUtils;

/**
 * 个人服务Api接口实现类。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-06-29
 */

@Component(value="priService")
public class PriService implements IPriService {
	
	private static final long serialVersionUID = 4943053100272442973L;
	
//	private static final Log LOG=LogFactory.getLog(PriService.class);
	
	//@Autowired(required=true)
	//private AuthenticationService authenticationService;
	
	@Autowired(required=true)
	private IContactUserHessianService contactUserHessianService;
	
	//@Autowired(required=true)
	//private ILocaleMessageService localeMessageService;
	
//	@Autowired(required=true)
//	private IPassportService passportService;
	
//	/*监控执行标志。*/
//	private boolean monitoringFlag=true;
//	
//	/*监控执行周期。默认604800000毫秒，即一周。*/
//	private Long monitoringCycle=604800000l;
//	
//	private PassportMonitor passportMonitor=new PassportMonitor();
//	
//	private Thread passportMonitorThread=new Thread(passportMonitor);

	@Override
	public Returning editLoginName(String passport, String newLoginName) {
		Returning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(passport)) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		if(StringUtils.isNull(newLoginName)) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		
//		//$2 修改登录名
//		LoginPassport loginPassport=passportService.getLoginPassport(passport);
//		if(null==loginPassport) {
//			ret = new HessianRemoteReturning();
//			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
//			return ret;
//		}
		LoginPassport loginPassport = PassportManager.getLoginPassport(passport);
		ret=contactUserHessianService.editLoginName(loginPassport.getLoginName(), newLoginName);
		return ret;
	}

	@Override
	public Returning editMobile(String passport,String neoMobile) {
		Returning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(passport)) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		if(StringUtils.isNull(neoMobile)) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_MOBILE);
			return ret;
		}
		
		//$2 修改手机号码
		LoginPassport loginPassport=PassportManager.getLoginPassport(passport);
		if(null==loginPassport) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		ret=contactUserHessianService.editMobile(loginPassport.getLoginName(), neoMobile);
		return ret;
	}

	@Override
	public Returning editPassword(String passport, String oldPassword, String neoPassword) {
		Returning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(passport)) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		if(StringUtils.isNull(oldPassword) || StringUtils.isNull(neoPassword)) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_PASSWORD);
			return ret;
		}
		//$2 修改密码
		LoginPassport loginPassport=PassportManager.getLoginPassport(passport);
		if(null==loginPassport) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		if(oldPassword.equals(neoPassword)) {
			/*旧密码与新密码一样，直接返回修改成功。*/
			ret = new Returning();
			ret.setStatusCode(IStatusCode.SUCCESS);
			return ret;
		}
		ret=contactUserHessianService.editPassword(loginPassport.getLoginName(),oldPassword,neoPassword);
		return ret;
	}

	@Override
	public Returning login(String loginName, String password) {
		Returning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(loginName) || StringUtils.isNull(password)) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME_OR_PASSWORD);
			return ret;
		}
		
		//$2 取得目前有效的护照
		LoginPassport loginPassport=null;
		loginPassport=PassportManager.getLoginPassportByLoginNameAndPwd(loginName, DESUtil.encrypt(password));
		if(loginPassport!=null) {
			ret = new Returning();
			ret.setValue(loginPassport.getPassport());
			ret.setStatusCode(IStatusCode.SUCCESS);
			return ret;
		}
		
		//$3 设置用户在线状态
		Date now=new Date();
		ret=contactUserHessianService.editContactUserToOnline(loginName, password, null,now);
		if(ret.getStatusCode() == IStatusCode.SUCCESS) {
			//$4 生成护照
			loginPassport=PassportManager.addPassport(loginName,password,now);
			ret.setValue(loginPassport.getPassport());
		}		
		return ret;
	}

	@Override
	public Returning logoutByPsssport(String passport) {
		Returning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(passport)) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		LoginPassport loginPassport=PassportManager.getLoginPassport(passport);
		if(null==loginPassport) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		
		//$2 设置用户离线状态
		ret=contactUserHessianService.editContactUserToOffline(loginPassport.getLoginName());
		
		//$3 登录失效，清除护照
		if(ret.getStatusCode() == IStatusCode.SUCCESS) {
			PassportManager.expireLogin(passport);
		}
		return ret;
	}

	@Override
	public Returning logoutByLoginName(String loginName) {
		Returning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(loginName)) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		LoginPassport loginPassport=PassportManager.getLoginPassportByLoginName(loginName);
		if(null==loginPassport) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		
		//$2 设置用户离线状态
		ret=contactUserHessianService.editContactUserToOffline(loginPassport.getLoginName());
		
		//$3 登录失效，清除护照
		if(ret.getStatusCode() == IStatusCode.SUCCESS) {
			PassportManager.expireLogin(loginName);
		}
		return ret;
	}
	
	@Override
	public Returning register(String loginName, String password, String mobile) {
		Returning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(loginName) || StringUtils.isNull(password)) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME_OR_PASSWORD);
			return ret;
		}
		if(StringUtils.isNull(mobile)) {
			ret = new Returning();
			ret.setStatusCode(IStatusCode.INVALID_MOBILE);
			return ret;
		}
		//$2添加用户
		Date now=new Date();
		ret=contactUserHessianService.addContactUser(
				loginName, password, mobile, ContactUser.USER_STATUS_ONLINE,null,now);
		if(ret.getStatusCode() == IStatusCode.SUCCESS) {
			//$3 生成护照
			LoginPassport loginPassport=PassportManager.addPassport(loginName,password,now);
			ret.setValue(loginPassport.getPassport());
		}
		return ret;
	}

	@Override
	public Returning editPassportToKeepHeart(String passport) {
		Returning ret = new Returning();
		//$1 验证合法性
		if(StringUtils.isNull(passport)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		
		//$2 保持护照的心跳
		LoginPassport loginPassport=PassportManager.keepPassport(passport);
		if(null==loginPassport) {
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		ret.setStatusCode(IStatusCode.SUCCESS);
		ret.setValue(loginPassport.getPassport());
		return ret;
	}

}
