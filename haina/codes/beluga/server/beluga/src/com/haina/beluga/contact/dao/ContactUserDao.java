package com.haina.beluga.contact.dao;

import java.util.Date;
import java.util.List;

import org.hibernate.criterion.DetachedCriteria;
import org.hibernate.criterion.Restrictions;
import org.springframework.stereotype.Repository;

import com.haina.beluga.contact.domain.ContactUser;
import com.sihus.core.dao.BaseDao;
import com.sihus.core.util.StringUtils;

/**
 * 联系人用户领域模型类Dao访问接口实现类。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-07-03
 */

@Repository(value="contactUserDao")
public class ContactUserDao extends BaseDao<ContactUser,String> implements IContactUserDao {

	@Override
	public ContactUser getValidUserByLoginName(String loginName) {
		ContactUser contactUser=null;
		if(!StringUtils.isNull(loginName)) {
			DetachedCriteria detachedCriteria=DetachedCriteria.forClass(ContactUser.class);
			detachedCriteria.add(Restrictions.and(
					Restrictions.eq("loginName", loginName), Restrictions.eq("validFlag", Boolean.TRUE)));
			List<ContactUser> list=this.getUserByHibernateCriteria(detachedCriteria);
//			this.getHibernateTemplate().findByNamedParam(
//					"from ContactUser u where u.loginName = :loginName and u.validFlag = :validFlag", 
//					new String[]{"loginName","validFlag"}, new Object[]{loginName,Boolean.TRUE});
			if(list!=null && list.size()>0) {
				contactUser=list.get(0);
			}
		}
		return contactUser;
	}

	
	@Override
	public ContactUser getValidUserByMobile(String mobile) {
		ContactUser contactUser=null;
		if(!StringUtils.isNull(mobile)) {
			DetachedCriteria detachedCriteria=DetachedCriteria.forClass(ContactUser.class);
			detachedCriteria.add(Restrictions.and(
					Restrictions.eq("mobile", mobile), Restrictions.eq("validFlag", Boolean.TRUE)));
			List<ContactUser> list=this.getUserByHibernateCriteria(detachedCriteria);
//			this.getHibernateTemplate().findByNamedParam(
//					"from ContactUser u where u.mobile = :mobile and u.validFlag = :validFlag", 
//					new String[]{"mobile","validFlag"}, new Object[]{mobile,Boolean.TRUE});
			if(list!=null && list.size()>0) {
				contactUser=list.get(0);
			}
		}
		return contactUser;
	}

	
	@Override
	public ContactUser getValidUserByPwdAndLoginName(String password,String loginName) {
		ContactUser contactUser=null;
		if(!StringUtils.isNull(loginName) && !StringUtils.isNull(password)) {
			ContactUser exampleUser=new ContactUser();
			exampleUser.setLoginName(loginName);
			exampleUser.setPassword(password);
			exampleUser.setValidFlag(Boolean.TRUE);
			List<ContactUser> list=this.getUserByExample(exampleUser);
//			this.getHibernateTemplate().findByNamedParam(
//					"from ContactUser u where u.loginName = :loginName and u.password = :password and u.validFlag = :validFlag", 
//					new String[]{"loginName","password","validFlag"}, new Object[]{loginName,password,Boolean.TRUE});
			if(list!=null && list.size()>0) {
				contactUser=list.get(0);
			}
		}
		return contactUser;
	}
	
	
	@Override
	public ContactUser getInvalidUserByPwdAndLoginName(String password,String loginName) {
		ContactUser contactUser=null;
		if(!StringUtils.isNull(loginName) && !StringUtils.isNull(password)) {
			ContactUser exampleUser=new ContactUser();
			exampleUser.setLoginName(loginName);
			exampleUser.setPassword(password);
			exampleUser.setValidFlag(Boolean.FALSE);
			List<ContactUser> list=this.getUserByExample(exampleUser);
//			this.getHibernateTemplate().findByNamedParam(
//					"from ContactUser u where u.loginName = :loginName and u.password = :password and u.validFlag = :validFlag", 
//					new String[]{"loginName","password","validFlag"}, new Object[]{loginName,password,Boolean.FALSE});
			if(list!=null && list.size()>0) {
				contactUser=list.get(0);
			}
		}
		return contactUser;
	}
	
	@Override
	public void deleteToInvalid(ContactUser contactUser) {
		if(contactUser!=null && !contactUser.isNew() 
				&& contactUser.getValidFlag()) {
			contactUser.setValidFlag(Boolean.FALSE);
			this.update(contactUser);
		}
	}

	
	@Override
	public ContactUser getInvalidUserByLoginName(String loginName) {
		ContactUser contactUser=null;
		if(!StringUtils.isNull(loginName)) {
			DetachedCriteria detachedCriteria=DetachedCriteria.forClass(ContactUser.class);
			detachedCriteria.add(Restrictions.and(
					Restrictions.eq("loginName", loginName), Restrictions.eq("validFlag", Boolean.TRUE)));
			List<ContactUser> list=this.getUserByHibernateCriteria(detachedCriteria);
//			this.getHibernateTemplate().findByNamedParam(
//					"from ContactUser u where u.loginName = :loginName and u.validFlag = :validFlag", 
//					new String[]{"loginName","validFlag"}, new Object[]{loginName,Boolean.FALSE});
			if(list!=null && list.size()>0) {
				contactUser=list.get(0);
			}
		}
		return contactUser;
	}

	
	@Override
	public List<ContactUser> getUserByMobileOrLoginName(String mobile, String loginName) {
		List<ContactUser> list=null;
		if(!StringUtils.isNull(mobile) &&  !StringUtils.isNull(loginName)) {
			DetachedCriteria detachedCriteria=DetachedCriteria.forClass(ContactUser.class);
			detachedCriteria.add(Restrictions.or(
					Restrictions.eq("mobile", mobile), Restrictions.eq("loginName", loginName)));
			list=this.getUserByHibernateCriteria(detachedCriteria);
//			this.getHibernateTemplate().findByNamedParam(
//					"from ContactUser u where u.loginName = :loginName or u.mobile = :mobile", 
//					new String[]{"loginName","mobile"}, new Object[]{loginName,mobile});
		}
		return list;
	}

	
	@Override
	public List<ContactUser> getInvalidUserByMobileOrLoginName(String mobile, String loginName) {
		List<ContactUser> list=null;
		if(!StringUtils.isNull(mobile) &&  !StringUtils.isNull(loginName)) {
			DetachedCriteria detachedCriteria=DetachedCriteria.forClass(ContactUser.class);
			detachedCriteria.add(Restrictions.and(Restrictions.eq("validFlag", Boolean.FALSE), Restrictions.or(
					Restrictions.eq("mobile", mobile), Restrictions.eq("loginName", loginName))));
			list=this.getUserByHibernateCriteria(detachedCriteria);
		}
		return list;
	}
	
	
	@Override
	public List<ContactUser> getUserByHibernateCriteria(DetachedCriteria criteria,int begin, int count) {
		int first=begin;
		int size=count;
		if(first<0) {
			first=1;
		}
		if(size<0) {
			size=1;
		}
		List<ContactUser> list=null;
		if(criteria!=null) {
			list=this.getModelByHibernateCriteria(criteria, begin, count);
		}
		return list;
	}
	
	
	@Override
	public List<ContactUser> getUserByHibernateCriteria(DetachedCriteria criteria) {
		List<ContactUser> list=null;
		if(criteria!=null) {
			list=this.getModelByHibernateCriteria(criteria);
		}
		return list;
	}

	
	@SuppressWarnings("unchecked")
	@Override
	public List<ContactUser> getUserByExample(ContactUser contactUser) {
		List<ContactUser> list=this.getHibernateTemplate().findByExample(contactUser);
		return list;
	}
	
	public int editToOffline(final List<String> loginNames) {
		int i=0;
		if(null!=loginNames && loginNames.isEmpty()) {
			i=this.getHibernateTemplate().bulkUpdate(
					"update ContactUser u set u.userStatus = ? where u.loginName in ( ? )",
					new Object[]{ContactUser.USER_STATUS_OFFLINE,loginNames});
		}
		return i;
	}

	@Override
	public int editToOffline(String loginName) {
		int i=0;
		if(!StringUtils.isNull(loginName)) {
			i=this.getHibernateTemplate().bulkUpdate(
					"update ContactUser u set u.userStatus = ? where u.loginName = ? ",
					new Object[]{ContactUser.USER_STATUS_OFFLINE,loginName});
		}
		return i;
	}

	@Override
	public int editToOnline(String loginName, String password,String userLoginIp,Date onlineTime) {
		int i=0;
		if(!StringUtils.isNull(loginName)) {

			i=this.getHibernateTemplate().bulkUpdate(
					"update ContactUser u set u.loginNumber = u.loginNumber+1,u.userStatus = ?,u.lastLoginIp=?,u.lastLoginTime=? where u.loginName = ? and u.password = ?",
					new Object[]{ContactUser.USER_STATUS_ONLINE,userLoginIp!=null ? userLoginIp : "",
							onlineTime!=null ? onlineTime : new Date(),loginName,password});
		}
		return i;
	}
	
//	private ContactUser encryptPassword(ContactUser contactUser) {
//		ContactUser user=contactUser;
//		if(user!=null && !StringUtils.isNull(user.getPassword())) {
//			user.setPassword(DESUtil.encrypt(user.getPassword().trim()));
//		}
//		return user;
//	}
//	
//	private ContactUser decryptPassword(ContactUser contactUser) {
//		ContactUser user=contactUser;
//		if(user!=null && !StringUtils.isNull(user.getPassword())) {
//			user.setPassword(DESUtil.decrypt(user.getPassword().trim()));
//		}
//		return user;
//	}
//	
//	private List<ContactUser> decryptPassword(List<ContactUser> contactUsers) {
//		List<ContactUser> list=contactUsers;
//		if(list!=null && list.size()>0) {
//			for(int i=0;i<list.size();i++) {
//				list.set(i, this.decryptPassword(list.get(i)));
//			}
//		}
//		return list;
//	}
}
