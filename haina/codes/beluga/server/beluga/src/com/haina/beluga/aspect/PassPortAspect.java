package com.haina.beluga.aspect;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.aspectj.lang.ProceedingJoinPoint;
import org.aspectj.lang.annotation.Around;
import org.aspectj.lang.annotation.Aspect;
import org.springframework.stereotype.Component;

import com.sihus.core.exception.AppException;
import com.sihus.core.util.StringUtils;

@Aspect
@Component
/**
 * passport 校验织入
 */
public class PassPortAspect {
	/** Logger available to subclasses */
	protected final Log logger = LogFactory.getLog(getClass());
	
	@Around("execution(* com.haina.beluga.webservice.priController.*.*(..))") 
	public Object inspect(ProceedingJoinPoint joinPoint) throws Throwable{
		if (logger.isInfoEnabled()) {
			logger.info("[Args:" + joinPoint.getArgs()+ "]");
			logger.info("[Target:" + joinPoint.getTarget()+ "]");
		}
		// 第一个参数为passport
		String passport = (String) joinPoint.getArgs()[0];
		if (StringUtils.isNull(passport)) {
			throw new AppException(ExceptionIDs.passport_Null);
		}
		// 验证过期
		boolean isExpired = PassportManager.isExpiredPassport(passport);
		if (isExpired) {
			// 过期需要客户端在此登陆生成新的passport
			throw new AppException(ExceptionIDs.passport_isExpired);
		}
		return joinPoint.proceed();
	}

}
