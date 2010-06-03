package com.haina.beluga.log.weave;

import java.lang.reflect.Method;

import javax.servlet.http.HttpServletRequest;

import org.apache.log4j.Logger;
import org.springframework.aop.AfterReturningAdvice;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import com.haina.beluga.log.dao.ILogDAO;
import com.haina.beluga.log.domain.Log;
import com.sihus.core.util.DateUtil;


/**
 * @author:付翔.
 * @createDate:2007-7-16.
 * @classInfo:系统响应请求后，日志织入。 
 */
@Component
public class LogAfterAdvice implements AfterReturningAdvice {
	
	private String handleName="submit";
	@Autowired(required=true)
	private ILogDAO logdao;
	private String pattern = "yyyy-MM-dd HH:mm:ss";
	
	 protected final static  Logger logger = Logger.getLogger(LogAfterAdvice.class.getName());

	public void afterReturning(Object returnVale, Method method, Object[] args, Object thisClass) throws Throwable {
		
		String className = thisClass.getClass().getSimpleName();
		Log log = new Log();//getHandlerInternal
		if("getHandlerMethodName".equals(method.getName())){
			handleName=(String) returnVale;
			
		}
		logger.info(method.getName()+"--------------------"+className+"----"+handleName);
		if("handleRequest".equals(method.getName())){
			HttpServletRequest request = (HttpServletRequest) args[0];
//			Account account = (Account) request.getSession().getAttribute("account");
			log.setIp(request.getRemoteAddr());
			log.setHandle(handleName);
			//登陆系统成功时，若未登陆成功帐户角色都为空。
//			if(null!=account){
//				log.setAccount(account);
//				log.setRoleName(account.getRole().getRolename());
//			}
			log.setInfoClass(className);
			log.setLogTime(DateUtil.formatDateTime(pattern));
//			if(null == SafeMapUtil.get(Constants.STARTTIME)){
//				log.setRemark("unCheck");
//			}else{
//				long startTime = (Long) SafeMapUtil.get(Constants.STARTTIME);
//				long endTime = System.currentTimeMillis();
//				long useTime =  endTime - startTime;
//				log.setRemark(""+useTime);
//			}
			logdao.create(log);
			handleName="submit";
			//debugInfo
//			if(null == SafeMapUtil.get(Constants.STARTTIME)){
		
		
//				logger.info("----------It's unCheck----------");
//				logger.info("UserId:"+accoutdto.getUsername());
//				logger.info("UserIp:"+request.getRemoteAddr());
//				logger.info("Handle:"+handleName);
//				logger.info("Class:"+className);
//				logger.info("Role:"+accoutdto.getRole().getRolename());
//				logger.info("Status: Undesign.");
//				logger.info("----------It's unCheck----------");
//				return;
//			}
//			long startTime = (Long) SafeMapUtil.get(Constants.STARTTIME);
//			long endTime = System.currentTimeMillis();
//			long useTime =  endTime - startTime;
//			logger.info("----------"+"UseTime:"+useTime+"(ms)----------");
//			logger.info("UserId:"+accoutdto.getUsername());
//			logger.info("UserIp:"+request.getRemoteAddr());
//			logger.info("Handle:"+handleName);
//			logger.info("Class:"+className);
//			logger.info("Role:"+accoutdto.getRole().getRolename());
//			logger.info("Status: Undesign.");
//			logger.info("----------"+"UseTime:"+useTime+"(ms)----------");
		}
	}
}
