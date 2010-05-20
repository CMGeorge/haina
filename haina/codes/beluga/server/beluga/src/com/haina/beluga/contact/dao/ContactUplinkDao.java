package com.haina.beluga.contact.dao;

import java.util.List;

import org.springframework.stereotype.Repository;

import com.haina.beluga.contact.domain.ContactUplink;
import com.sihus.core.dao.BaseDao;
import com.sihus.core.util.StringUtils;

/**
 * 联系人上行领域模型类Dao访问接口实现类。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-07-30
 */

@Repository(value="contactUplinkDao")
public class ContactUplinkDao extends BaseDao<ContactUplink,String> implements IContactUplinkDao {

	@Override
	public void deleteUplinkByOwner(String ownerId) {
		if(!StringUtils.isNull(ownerId)) {
			this.getHibernateTemplate().bulkUpdate("delete from ContactUplink cu where cu.owner = ?", ownerId);
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<ContactUplink> getRegisteredUplink() {
		ContactUplink contactUplink=new ContactUplink();
		contactUplink.setRegisterFlag(Boolean.TRUE);
		return this.getHibernateTemplate().findByExample(contactUplink);
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<ContactUplink> getUnregisteredUplink() {
		ContactUplink contactUplink=new ContactUplink();
		contactUplink.setRegisterFlag(Boolean.FALSE);
		return this.getHibernateTemplate().findByExample(contactUplink);
	}

	@SuppressWarnings("unchecked")
	@Override
	public ContactUplink getUplinkByMobile(String mobile) {
		ContactUplink ret=null;
		if(!StringUtils.isNull(mobile)) {
			ContactUplink contactUplink=new ContactUplink();
			contactUplink.setMobile(mobile);
			List<ContactUplink> list=this.getHibernateTemplate().findByExample(contactUplink);
			if(list!=null  && list.size()>0) {
				ret=list.get(0);
			}
		}
		return ret;
	}

	@SuppressWarnings("unchecked")
	@Override
	public List<ContactUplink> getUplinkByOwner(String ownerId) {
		List<ContactUplink> ret=null;
		if(!StringUtils.isNull(ownerId)) {
			ContactUplink contactUplink=new ContactUplink();
			contactUplink.setOwner(ownerId);
			ret=this.getHibernateTemplate().findByExample(contactUplink);
		}
		return ret;
	}
	
}
