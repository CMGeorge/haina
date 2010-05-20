package com.haina.beluga.contact.dao;

import java.util.List;

import com.haina.beluga.contact.domain.ContactUplink;
import com.sihus.core.dao.IBaseDao;

/**
 * 联系人上行领域模型类Dao访问接口。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-07-28
 */
public interface IContactUplinkDao extends IBaseDao<ContactUplink,String> {

	/**
	 * 通过手机号码得到联系人上行信息。<br/>
	 * @param mobile
	 * @return
	 */
	public ContactUplink getUplinkByMobile(String mobile);
	
	/**
	 * 通过所有者用户的Id得到该用户的联系人上行信息。<br/>
	 * @param ownerId
	 * @return
	 */
	public List<ContactUplink> getUplinkByOwner(String ownerId);
	
	/**
	 * 得到所有已经注册的联系人上行信息。<br/>
	 * @param mobile
	 * @return
	 */
	public List<ContactUplink> getRegisteredUplink();
	
	/**
	 * 得到所有没有注册的联系人上行信息。<br/>
	 * @param mobile
	 * @return
	 */
	public List<ContactUplink> getUnregisteredUplink();
	
	/**
	 * 通过所有者用户的Id删除该用户的联系人上行信息。<br/>
	 * @param ownerId
	 * @return
	 */
	public void deleteUplinkByOwner(String ownerId);
}
