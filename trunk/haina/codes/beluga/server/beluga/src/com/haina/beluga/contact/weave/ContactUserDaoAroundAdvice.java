package com.haina.beluga.contact.weave;

import java.text.MessageFormat;
import java.util.List;
import java.util.Map;

import org.aopalliance.intercept.MethodInterceptor;
import org.aopalliance.intercept.MethodInvocation;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import com.haina.beluga.contact.domain.ContactUser;
import com.sihus.core.util.DESUtil;
import com.sihus.core.util.StringUtils;

/**
 * 用户密码编码解码AOP拦截器。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-06-15
 */
@Deprecated
public class ContactUserDaoAroundAdvice implements MethodInterceptor {
	private static final Log LOG=LogFactory.getLog(ContactUserDaoAroundAdvice.class);

	/*需要对对密码编码的方法。*/
	private Map<String,String> encryptPasswordMethod;
	
	/*需要对对密码编码的方法。*/
	private Map<String,String> decryptPasswordMethod;
	
	public void setEncryptPasswordMethod(Map<String, String> encryptPasswordMethod) {
		this.encryptPasswordMethod = encryptPasswordMethod;
	}


	public void setDecryptPasswordMethod(Map<String, String> decryptPasswordMethod) {
		this.decryptPasswordMethod = decryptPasswordMethod;
	}
	
	@SuppressWarnings("unchecked")
	@Override
	public Object invoke(MethodInvocation invocation) throws Throwable {
		String methodName=invocation.getMethod().getName();
		Object retObj=invocation.proceed();
		boolean encryptd=this.encryptPassword(methodName,invocation.getArguments());
		if(!encryptd) {
			/*没有进行密码的编码。*/
			this.decryptPassword(methodName, retObj);
		}
		return retObj;
	}
	
	
	/**
	 * 对密码解码。<br/>
	 */
	@SuppressWarnings("unchecked")
	private boolean decryptPassword(String methodName, Object retObj) {
		if(decryptPasswordMethod!=null && decryptPasswordMethod.size()>0
				&& !StringUtils.isNull(methodName) && retObj!=null) {
			if(decryptPasswordMethod.containsKey(methodName)) {
				LOG.info(MessageFormat.format(">>>>>>>>>>>>>>>>>>>decryptPasswordMethod:  {0} >>>>>>>>>>>>>>>>>>", methodName));
				if(retObj.getClass().equals(List.class)) {
					List<ContactUser> list=(List<ContactUser>)retObj;
					if(list!=null && !list.isEmpty()) {
						for(ContactUser user:list) {
							LOG.info(MessageFormat.format(">>>>>>>>>>>>>>>>>>>password before decrypted:  {0} >>>>>>>>>>>>>>>>>>", user.getPassword()));
							user.setPassword(DESUtil.decrypt(user.getPassword()));
							LOG.info(MessageFormat.format(">>>>>>>>>>>>>>>>>>>password after decrypted:  {0} >>>>>>>>>>>>>>>>>>", user.getPassword()));
						}
					}
				} else if(retObj.getClass().equals(ContactUser.class)) {
					ContactUser contactUser=(ContactUser)retObj;
					LOG.info(MessageFormat.format(">>>>>>>>>>>>>>>>>>>password before decrypted:  {0} >>>>>>>>>>>>>>>>>>", contactUser.getPassword()));
					contactUser.setPassword(DESUtil.decrypt(contactUser.getPassword()));
					LOG.info(MessageFormat.format(">>>>>>>>>>>>>>>>>>>password after decrypted:  {0} >>>>>>>>>>>>>>>>>>", contactUser.getPassword()));
				}
				return true;
			}
		}
		return false;
	}
	
	/**
	 * 对密码编码。<br/>
	 */
	public boolean encryptPassword(String methodName, Object[] args) {
		if(encryptPasswordMethod!=null && encryptPasswordMethod.size()>0
				&& !StringUtils.isNull(methodName) && args!=null
				&& args.length>0) {
			if(encryptPasswordMethod.containsKey(methodName)) {
				LOG.info(MessageFormat.format(">>>>>>>>>>>>>>>>>>>encryptPasswordMethod:  {0} >>>>>>>>>>>>>>>>>>", methodName));
				for(Object o:args) {
					if(o!=null && o.getClass().equals(ContactUser.class)) {
						ContactUser contactUser=(ContactUser)o;
						LOG.info(MessageFormat.format(">>>>>>>>>>>>>>>>>>>password before ecrypted:  {0} >>>>>>>>>>>>>>>>>>", contactUser.getPassword()));
						contactUser.setPassword(DESUtil.encrypt(contactUser.getPassword()));
						LOG.info(MessageFormat.format(">>>>>>>>>>>>>>>>>>>password after ecrypted:  {0} >>>>>>>>>>>>>>>>>>", contactUser.getPassword()));
						return true;
					}
				}
			}
		}
		return false;
	}
}
