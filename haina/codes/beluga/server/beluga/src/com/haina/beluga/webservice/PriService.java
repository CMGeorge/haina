package com.haina.beluga.webservice;

import java.lang.Thread.State;
import java.util.Date;
import java.util.List;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.InitializingBean;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import com.haina.beluga.contact.domain.ContactUser;
import com.haina.beluga.contact.service.IContactUserHessianService;
import com.haina.beluga.contact.service.IPassportService;
import com.haina.beluga.contact.service.LoginPassport;
import com.haina.beluga.webservice.data.hessian.HessianRemoteReturning;
import com.haina.core.util.DESUtil;
import com.haina.core.util.StringUtils;

/**
 * 个人服务Api接口实现类。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-06-29
 */

@Component(value="priService")
public class PriService implements IPriService, InitializingBean {
	
	private static final long serialVersionUID = 4943053100272442973L;
	
	private static final Log LOG=LogFactory.getLog(PriService.class);
	
	//@Autowired(required=true)
	//private AuthenticationService authenticationService;
	
	@Autowired(required=true)
	private IContactUserHessianService contactUserHessianService;
	
	//@Autowired(required=true)
	//private ILocaleMessageService localeMessageService;
	
	@Autowired(required=true)
	private IPassportService passportService;
	
	/*监控执行标志。*/
	private boolean monitoringFlag=true;
	
	/*监控执行周期。默认604800000毫秒，即一周。*/
	private Long monitoringCycle=604800000l;
	
	private PassportMonitor passportMonitor=new PassportMonitor();
	
	private Thread passportMonitorThread=new Thread(passportMonitor);

	@Override
	public HessianRemoteReturning editLoginName(String passport, String newLoginName) {
		HessianRemoteReturning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(passport)) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		if(StringUtils.isNull(newLoginName)) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		
		//$2 修改登录名
		LoginPassport loginPassport=passportService.getLoginPassport(passport);
		if(null==loginPassport) {
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		ret=contactUserHessianService.editLoginName(loginPassport.getLoginName(), newLoginName);
		passportService.keepPassport(passport);
		return ret;
	}

	@Override
	public HessianRemoteReturning editMobile(String passport,String neoMobile) {
		HessianRemoteReturning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(passport)) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		if(StringUtils.isNull(neoMobile)) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_MOBILE);
			return ret;
		}
		
		//$2 修改手机号码
		LoginPassport loginPassport=passportService.getLoginPassport(passport);
		if(null==loginPassport) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		ret=contactUserHessianService.editMobile(loginPassport.getLoginName(), neoMobile);
		passportService.keepPassport(passport);
		return ret;
	}

	@Override
	public HessianRemoteReturning editPassword(String passport, String oldPassword, String neoPassword) {
		HessianRemoteReturning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(passport)) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		if(StringUtils.isNull(oldPassword) || StringUtils.isNull(neoPassword)) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_PASSWORD);
			return ret;
		}
		//$2 修改密码
		LoginPassport loginPassport=passportService.getLoginPassport(passport);
		if(null==loginPassport) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		if(oldPassword.equals(neoPassword)) {
			/*旧密码与新密码一样，直接返回修改成功。*/
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.SUCCESS);
			passportService.keepPassport(passport);
			return ret;
		}
		ret=contactUserHessianService.editPassword(loginPassport.getLoginName(),oldPassword,neoPassword);
		passportService.keepPassport(passport);
		return ret;
	}

	@Override
	public HessianRemoteReturning login(String loginName, String password) {
		HessianRemoteReturning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(loginName) || StringUtils.isNull(password)) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME_OR_PASSWORD);
			return ret;
		}
		
		//$2 取得目前有效的护照
		LoginPassport loginPassport=null;
		loginPassport=passportService.getLoginPassportByLoginNameAndPwd(loginName, DESUtil.encrypt(password));
		if(loginPassport!=null) {
			ret = new HessianRemoteReturning();
			ret.setValue(loginPassport.getPassport());
			ret.setStatusCode(IStatusCode.SUCCESS);
			return ret;
		}
		
		//$3 设置用户在线状态
		Date now=new Date();
		ret=contactUserHessianService.editContactUserToOnline(loginName, password, null,now);
		if(ret.isSuccessStatus()) {
			//$4 生成护照
			loginPassport=passportService.addPassport(loginName,password,now);
			ret.setValue(loginPassport.getPassport());
		}		
		return ret;
	}

	@Override
	public HessianRemoteReturning logoutByPsssport(String passport) {
		HessianRemoteReturning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(passport)) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		LoginPassport loginPassport=passportService.getLoginPassport(passport);
		if(null==loginPassport) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		
		//$2 设置用户离线状态
		ret=contactUserHessianService.editContactUserToOffline(loginPassport.getLoginName());
		
		//$3 登录失效，清除护照
		if(ret.isSuccessStatus()) {
			passportService.expireLogin(passport);
		}
		return ret;
	}

	@Override
	public HessianRemoteReturning logoutByLoginName(String loginName) {
		HessianRemoteReturning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(loginName)) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		LoginPassport loginPassport=passportService.getLoginPassportByLoginName(loginName);
		if(null==loginPassport) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		
		//$2 设置用户离线状态
		ret=contactUserHessianService.editContactUserToOffline(loginPassport.getLoginName());
		
		//$3 登录失效，清除护照
		if(ret.isSuccessStatus()) {
			passportService.expireLogin(loginName);
		}
		return ret;
	}
	
	@Override
	public HessianRemoteReturning register(String loginName, String password, String mobile) {
		HessianRemoteReturning ret = null;
		//$1 验证合法性
		if(StringUtils.isNull(loginName) || StringUtils.isNull(password)) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME_OR_PASSWORD);
			return ret;
		}
		if(StringUtils.isNull(mobile)) {
			ret = new HessianRemoteReturning();
			ret.setStatusCode(IStatusCode.INVALID_MOBILE);
			return ret;
		}
		//$2添加用户
		Date now=new Date();
		ret=contactUserHessianService.addContactUser(
				loginName, password, mobile, ContactUser.USER_STATUS_ONLINE,null,now);
		if(ret.isSuccessStatus()) {
			//$3 生成护照
			LoginPassport loginPassport=passportService.addPassport(loginName,password,now);
			ret.setValue(loginPassport.getPassport());
		}
		return ret;
	}

	@Override
	public void afterPropertiesSet() throws Exception {
		startMonitoring();		
	}
	
	private void startMonitoring() {
		if(passportMonitorThread.getState().equals(State.NEW)) {
			passportMonitorThread.start();
			LOG.info(">>>>>>>>>>>>>>>>>>>>>PassportMonitor started>>>>>>>>>>>>>>>>>>>");
		}
	}
	
	private void removeExpiredLoginUser() {
		LOG.info(">>>>>>>>>>>>>>>>>>PassportMonitor start clear expired login user>>>>>>>>>>>>>>>>>>>>>");
		List<String> expiredLoginName=passportService.clearExpiredPassport();
		/*暂时不修改数据库状态。*/
		//this.contactUserHessianService.editContactUserToOffline(expiredLoginName);
	}

	/**
	 * 用户验证护照监视器。<br/>
	 * @author huangyongqiang
	 * @version 1.0
	 * @since 1.0
	 * @data 2009-07-11
	 *
	 */
	private class PassportMonitor implements Runnable {

		@SuppressWarnings("static-access")
		@Override
		public void run() {
			try {
				while(monitoringFlag) {
					removeExpiredLoginUser();
					Thread.currentThread().sleep(monitoringCycle);
				}
			} catch (Exception e) {
				LOG.error(e);
				e.printStackTrace();
			}
		}
		
	}

	@Override
	public HessianRemoteReturning editPassportToKeepHeart(String passport) {
		HessianRemoteReturning ret = new HessianRemoteReturning();
		//$1 验证合法性
		if(StringUtils.isNull(passport)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		
		//$2 保持护照的心跳
		LoginPassport loginPassport=passportService.keepPassport(passport);
		if(null==loginPassport) {
			ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
			return ret;
		}
		ret.setStatusCode(IStatusCode.SUCCESS);
		ret.setValue(loginPassport.getPassport());
		return ret;
	}
}
