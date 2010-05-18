package com.haina.beluga.contact.service;

import java.util.ArrayList;
import java.util.Date;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.haina.beluga.contact.domain.ContactUser;
import com.sihus.core.util.DESUtil;
import com.sihus.core.util.StringUtils;

/**
 * 用户验证护照业务处理接口实现类。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @data 2009-07-11
 *
 */
//@Service(value="passportService")
public class PassportService implements IPassportService {
	
	private static final Log LOG=LogFactory.getLog(PassportService.class);

	/*用户护照的存储池。*/
	private Map<String,LoginPassport> passportPool=new ConcurrentHashMap<String,LoginPassport>();
	
	/*登录超期时间。默认604800000毫秒，即一周。*/
	private Long loginExpiry;
	
	/*登录超期时间偏移，考虑到网络传输时延。默认10000毫秒，即十秒。*/
	private Long loginExpiryTimeOff;
	
	/*护照超期时间。默认1800000毫秒，即半小时。*/
	private Long passportExpiry;
	
	/*护照超期时间偏移，考虑到网络传输时延。默认30000毫秒，即半分钟。*/
	private Long passportExpiryTimeOff;
	
	/*监控执行周期。默认604800000毫秒，即一周。*/
//	private Long monitoringCycle=604800000l;
	
	/*监控执行标志。*/
//	private boolean monitoringFlag=true;
	
//	private PassportMonitor passportMonitor=new PassportMonitor();
//	
//	private Thread passportMonitorThread=new Thread(passportMonitor);

	public PassportService() {
//		startMonitoring();
	}
	
	
	@Override
	public LoginPassport addPassport(ContactUser contactUser) {
		LoginPassport loginPassport=null;
		if(null!=contactUser) {
			loginPassport=new LoginPassport();
			loginPassport.setLoginName(contactUser.getLoginName().trim());
			loginPassport.setPassword(DESUtil.encrypt(contactUser.getPassword().trim()));
			loginPassport.setLoginExpiry(loginExpiry);
			Long time=contactUser.getLastLoginTime().getTime();
			loginPassport.setLoginTime(time);
			String passport=generatePassport();
			loginPassport.setPassport(passport);
			loginPassport.setPassportTime(time);
			loginPassport.setPassportExpiry(passportExpiry);
			passportPool.put(passport, loginPassport);
		}
		return loginPassport;
	}

	@Override
	public LoginPassport addPassport(String loginName,String password,Date loginTime) {
		LoginPassport loginPassport=null;
		if(!StringUtils.isNull(loginName) 
				&& !StringUtils.isNull(password) && loginTime!=null) {
			loginPassport=new LoginPassport();
			loginPassport.setLoginName(loginName.trim());
			loginPassport.setPassword(DESUtil.encrypt(password.trim()));
			loginPassport.setLoginExpiry(loginExpiry);
			Long time=loginTime.getTime();
			loginPassport.setLoginTime(time);
			String passport=generatePassport();
			loginPassport.setPassport(passport);
			loginPassport.setPassportTime(time);
			loginPassport.setPassportExpiry(passportExpiry);
			passportPool.put(passport, loginPassport);
		}
		return loginPassport;
	}
	
	@Override
	public LoginPassport updatePassport(String passport) {
		if(!StringUtils.isNull(passport) && passportPool.containsKey(passport)) {
			LoginPassport loginPassport=passportPool.get(passport);
			Long now=(new Date()).getTime();
			if(loginPassport!=null && isExpiredPassport(loginPassport)) {
				loginPassport.setPassport(generatePassport());
				loginPassport.setPassportTime(now);
				return loginPassport;
			}
		}
		return null;
	}
	
	@Override
	public LoginPassport getLoginPassport(String passport) {
		LoginPassport loginPassport=null;
		if(passport!=null && passportPool.containsKey(passport)) {
			loginPassport=passportPool.get(passport);
			if(isExpiredPassport(loginPassport)) {
				this.removePassport(loginPassport.getPassport());
				loginPassport=null;
			}
		}
		return loginPassport;
	}
	
	@Override
	public LoginPassport getLoginPassportByLoginName(String loginName) {
		LoginPassport loginPassport=null;
		if(loginName!=null) {
			Iterator<String> keys=passportPool.keySet().iterator();
			while(keys.hasNext()) {
				LoginPassport value=passportPool.get(keys.next());
				if(!isExpiredPassport(value)) {
					if(value.getLoginName().equals(loginName)) {
						loginPassport=value;
						break;
					}
				}
			}
		}
		return loginPassport;
	}

	@Override
	public boolean isExpiredPassport(String passport) {
//		if(isExpiredLogin(passport)) {
//		return true;
//	}
		if(StringUtils.isNull(passport) || !passportPool.containsKey(passport)) {
			return true;
		}
		LoginPassport loginPassport=passportPool.get(passport);
		return isExpiredPassport(loginPassport);
	}
	
	@Override
	public boolean isExpiredPassport(LoginPassport loginPassport) {
//		if(isExpiredLogin(loginPassport)) {
//			return true;
//		}
		if(null==loginPassport) {
			return true;
		}
		Long now=(new Date()).getTime();
		return loginPassport.getPassportTime()+loginPassport.getPassportExpiry()>=now-passportExpiryTimeOff;
	}

	@Override
	public boolean removeAllPassport() {
		return false;
	}

	@Override
	public boolean removePassport(String passport) {
		if(!StringUtils.isNull(passport) && passportPool.containsValue(passport)) {
			passportPool.remove(passport);
		}
		return false;
	}

	@Override
	public boolean isExpiredLogin(String passport) {
		if(passport==null || !passportPool.containsKey(passport)) {
			return true;
		}
		LoginPassport loginPassport=passportPool.get(passport);
		return isExpiredLogin(loginPassport);
	}
	
	public boolean isExpiredLogin(LoginPassport loginPassport) {
		if(loginPassport==null) {
			return true;
		} else {
			Long now=(new Date()).getTime();
			if(!passportPool.containsValue(loginPassport)) {
				return true;
			}
			return loginPassport.getLoginTime()+loginPassport.getLoginExpiry()>=now-loginExpiryTimeOff;
		}
	}
	
//	@Override
//	public void afterPropertiesSet() throws Exception {
//		startMonitoring();
//	}

	public void setLoginExpiry(Long loginExpiry) {
		this.loginExpiry = loginExpiry;
	}

	public void setPassportExpiry(Long passportExpiry) {
		this.passportExpiry = passportExpiry;
	}

	public void setPassportExpiryTimeOff(Long passportExpiryTimeOff) {
		this.passportExpiryTimeOff = passportExpiryTimeOff;
	}

	public void setLoginExpiryTimeOff(Long loginExpiryTimeOff) {
		this.loginExpiryTimeOff = loginExpiryTimeOff;
	}
	
//	public void setMonitoringCycle(Long monitoringCycle) {
//		this.monitoringCycle = monitoringCycle;
//	}
	
	private String generatePassport() {
		return StringUtils.getRandom(8);//DESUtil.encrypt(UUID.randomUUID().toString().replace("-", ""));
	}
	
//	private void startMonitoring() {
//		if(passportMonitorThread.getState().equals(State.NEW)) {
//			passportMonitorThread.start();
//			LOG.info(">>>>>>>>>>>>>>>>>>>>>PassportMonitor started>>>>>>>>>>>>>>>>>>>");
//		}
//	}
	
	@Override
	public List<String> clearExpiredPassport() {
		List<String> expiredLoginName=new ArrayList<String>();
		if(passportPool.size()>0) {
			Iterator<String> keys=passportPool.keySet().iterator();
			while(keys.hasNext()) {
				String passport=keys.next();
				if(isExpiredPassport(passport)) {
					expiredLoginName.add(passportPool.get(passport).getLoginName());
					expireLogin(passport);
				}
			}
		}
		return expiredLoginName;
	}
	
//	/**
//	 * 用户验证护照监视器。<br/>
//	 * @author huangyongqiang
//	 * @version 1.0
//	 * @since 1.0
//	 * @data 2009-07-11
//	 *
//	 */
//	private class PassportMonitor implements Runnable {
//
//		@SuppressWarnings("static-access")
//		@Override
//		public void run() {
//			try {
//				while(monitoringFlag) {
//					LOG.info(">>>>>>>>>>>>>>>>>>PassportMonitor start clear expired login user>>>>>>>>>>>>>>>>>>>>>");
//					if(passportPool.size()>0) {
//						Iterator<String> keys=passportPool.keySet().iterator();
//						while(keys.hasNext()) {
//							String passport=keys.next();}
//							if(isExpiredPassport(passport)) {
//								expireLogin(passport);
//							}
//						}
//					}
//					Thread.currentThread().sleep(monitoringCycle);
//				}
//			} catch (Exception e) {
//				LOG.error(e);
//				e.printStackTrace();
//			}
//		}
//		
//	}

	@Override
	public boolean expireLogin(String passport) {
		if(StringUtils.isNull(passport) || !passportPool.containsKey(passport)) {
			return false;
		}
		passportPool.remove(passport);
		return true;
	}
	
	@Override
	public int getPassportQuantity() {
		return passportPool.size();
	}


	@Override
	public LoginPassport getLoginPassportByLoginNameAndPwd(String loginName,
			String password) {
		LoginPassport loginPassport=null;
		if(loginName!=null) {
			Iterator<String> keys=passportPool.keySet().iterator();
			while(keys.hasNext()) {
				LoginPassport value=passportPool.get(keys.next());
				if(!isExpiredPassport(value)) {
					if(value.getLoginName().equals(loginName) 
							&& value.getPassword().equals(password)) {					
						loginPassport=value;
						break;
					}
				}
			}
		}
		return loginPassport;
	}


	@Override
	public LoginPassport keepPassport(String passport) {
		if(!StringUtils.isNull(passport) && passportPool.containsKey(passport)) {
			LoginPassport loginPassport=passportPool.get(passport);
			Long now=(new Date()).getTime();
			if(loginPassport!=null && !isExpiredPassport(loginPassport)) {
				loginPassport.setPassportTime(now);
				return loginPassport;
			}
		}
		return null;
	}
}
