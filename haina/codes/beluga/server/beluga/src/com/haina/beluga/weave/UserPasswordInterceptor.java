package com.haina.beluga.weave;

import java.util.List;

import org.aspectj.lang.JoinPoint;
import org.aspectj.lang.annotation.AfterReturning;
//import org.aspectj.lang.annotation.Aspect;
import org.aspectj.lang.annotation.Before;
//import org.springframework.stereotype.Component;

import com.haina.beluga.core.util.DESUtil;
import com.haina.beluga.domain.ContactUser;


/**
 * 用户密码编码解码AOP拦截器。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-06-15
 */

//@Aspect
//@Component(value="userPasswordInterceptor")
@Deprecated
public class UserPasswordInterceptor {
	
	/**
	 * 新增或编辑前对ContactUser对象的密码编码。<br/>
	 * @param contactUser
	 */
	@Before(value="(execution(public java.lang.String com.haina.beluga.dao.IContactUserDao.create(com.haina.beluga.domain.ContactUser))"
		+ " || " + "execution(public void com.haina.beluga.dao.IContactUserDao.update(com.haina.beluga.domain.ContactUser))"
		+ " || " + "execution(public void com.haina.beluga.dao.IContactUserDao.saveOrUpdate(com.haina.beluga.domain.ContactUser)))"
		+ " && args(contactUser)" ,argNames="contactUser")
	public void encryptContactUserPwd(ContactUser contactUser) {
		if(null!=null) {
			contactUser.setPassword(DESUtil.encrypt(contactUser.getPassword()));
		}
	}

	
	/**
	 * 读取后对ContactUser对象的密码解码。<br/>
	 * @param jp
	 * @param retObj
	 */
	@AfterReturning(pointcut="(execution(public com.haina.beluga.domain.ContactUser com.haina.beluga.dao.IContactUserDao.read(java.lang.String))"
		+ " || " + "execution(public com.haina.beluga.domain.ContactUser com.haina.beluga.dao.IContactUserDao.load(java.lang.String)))",
		returning="retObj")
	public void decryptReadedContactUserPwd(JoinPoint jp,Object retObj) {
		if(null!=retObj && retObj.equals(ContactUser.class)) {
			ContactUser contactUser=(ContactUser)retObj;
			contactUser.setPassword(DESUtil.decrypt(contactUser.getPassword()));
		}
	}
	
	/**
	 * 读取后对ContactUser对象的密码解码。<br/>
	 * @param jp
	 * @param retObj
	 */
	@SuppressWarnings("unchecked")
	@AfterReturning(pointcut="(execution(public java.util.List com.haina.beluga.dao.IContactUserDao.getModels(boolean))"
		+ " || " + "execution(public com.haina.beluga.domain.ContactUser com.haina.beluga.dao.IContactUserDao.getContactUserByLoginName(java.lang.String))"
		+ " || " + "execution(public com.haina.beluga.domain.ContactUser com.haina.beluga.dao.IContactUserDao.getContactUserByMobile(java.lang.String)))",
		returning="retObj")
	public void decryptGotContactUsersPwd(JoinPoint jp, Object retObj) {
		if(null!=retObj) {
			if(retObj.getClass().equals(List.class)) {
				List<ContactUser> list=(List<ContactUser>)retObj;
				if(!list.isEmpty()) {
					for(ContactUser user:list) {
						user.setPassword(DESUtil.decrypt(user.getPassword()));
					}
				}
			} else if(retObj.getClass().equals(ContactUser.class)) {
				ContactUser contactUser=(ContactUser)retObj;
				contactUser.setPassword(DESUtil.decrypt(contactUser.getPassword()));
			}	
		}
	}
	
	public static void main(String[] args) {
		System.out.println("123456789012345678901234567890123456789012345678901234567890jklm".length());
		String s=DESUtil.encrypt("123abc");
		System.out.println(s);
		System.out.println(s.length());
		System.out.println(DESUtil.decrypt(s));
	}
}
