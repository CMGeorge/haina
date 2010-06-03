package com.haina.beluga.contact.service;

import java.util.Date;

import com.haina.beluga.contact.dao.ContactUplinkDao;
import com.haina.beluga.contact.domain.ContactUplink;
import com.haina.beluga.webservice.data.Returning;
import com.sihus.core.service.IBaseSerivce;

/**
 * 联系人上行信息hessian协议业务处理接口。<br/>
 * 
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-08-07
 */
public interface IContactUplinkHessianService extends IBaseSerivce<ContactUplinkDao,ContactUplink,String> {

	/**
	 * 添加一个联系人上行信息。<br/>
	 * @param owner 所有者id
	 * @param mobile 手机号码
	 * @param name 姓名
	 * @param age 年龄
	 * @param sex 性别
	 * @param brithday 生日
	 * @param url 个人主页或博客
	 * @param emailPref 首选email
	 * @param telPref 首先电话号码
	 * @param imPref 首选IM
	 * @param org 组织名称
	 * @param title 职位
	 * @return
	 */
	public Returning addContactUplink(String owner,String mobile, String name,Integer age, Integer sex, Date brithday,
			String url,String emailPref, String telPref, String imPref, String org, String title);
}
