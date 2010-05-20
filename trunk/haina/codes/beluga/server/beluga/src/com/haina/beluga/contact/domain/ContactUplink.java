package com.haina.beluga.contact.domain;

import java.util.Date;

import org.springframework.stereotype.Component;

import com.haina.beluga.contact.domain.enumerate.SexEnum;
import com.haina.core.model.VersionalModel;

/**
 * 联系人上行表的领域模型类。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-07-23
 * @hibernate.class table="ContactUplink" optimistic-lock="version"
 * @hibernate.cache usage="read-write"
 */
@Component
public class ContactUplink extends VersionalModel {
	
	private static final long serialVersionUID = 484539818638173778L;

	/*所有人ID，即上传者的用户ID。*/
	private String owner;
	
	private String mobile;
	
	private String name;
	
	private Integer age;
	
	private SexEnum sex;
	
	private Date brithday;
	
	/*个人主页或博客。*/
	private String url;
	
	/*首选email。*/
	private String emailPref;
	
	private String telPref;
	
	private String imPref;
	
	/*组织名称。*/
	private String org;
	
	/*职位名称。*/
	private String title;
	
	/*是否已注册。*/
	private Boolean registerFlag;
	
	
	/*数据库手动修改记录。*/
	private String remark;
	
	/**
	 * @hibernate.id unsaved-value="null" type = "java.lang.String" length="32"
	 * @hibernate.column name="id" sql-type="char(32)"
	 * @hibernate.generator class="uuid.hex"
	 */
	@Override
	public String getId() {
		return id;
	}
	
	/**
	 * @hibernate.version
	 * @hibernate.column name="version" type="long"
	 */
	@Override
	public long getVersion() {
		return version;
	}

	/**
	 * @hibernate.property length="32" not-null="true" type = "string"
	 * @hibernate.column name="id" sql-type="char(32)"
	 */
	public String getOwner() {
		return owner;
	}

	public void setOwner(String owner) {
		this.owner = owner;
	}
	
	/**
	 * @hibernate.property column="mobile" length="64" not-null="true" type = "java.lang.String" unique = "true"
	 */
	public String getMobile() {
		return mobile;
	}

	public void setMobile(String mobile) {
		this.mobile = mobile;
	}
	
	/**
	 * @hibernate.property column="name" length="64" type="string"
	 */
	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	/**
	 * @hibernate.property column="age" type="integer"
	 */
	public Integer getAge() {
		return age;
	}

	public void setAge(Integer age) {
		this.age = age;
	}

	/**
	 * @hibernate.property type="timestamp"
	 * @hibernate.column name="brithday" sql-type="timestamp"
	 */
	public Date getBrithday() {
		return brithday;
	}

	public void setBrithday(Date brithday) {
		this.brithday = brithday;
	}
	
	/**
	 * @hibernate.property column="url" length="2000" type="string"
	 */
	public String getUrl() {
		return url;
	}

	public void setUrl(String url) {
		this.url = url;
	}
	

	/**
	 * @hibernate.property column="emailPref" length="256" type="string"
	 */
	public String getEmailPref() {
		return emailPref;
	}

	public void setEmailPref(String emailPref) {
		this.emailPref = emailPref;
	}

	/**
	 * @hibernate.property column="telPref" length="80" type="string"
	 */
	public String getTelPref() {
		return telPref;
	}

	public void setTelPref(String telPref) {
		this.telPref = telPref;
	}

	/**
	 * @hibernate.property column="imPref" length="256" type="string"
	 */
	public String getImPref() {
		return imPref;
	}

	public void setImPref(String imPref) {
		this.imPref = imPref;
	}

	/**
	 * @hibernate.property column="org" length="128" type="string"
	 */
	public String getOrg() {
		return org;
	}

	public void setOrg(String org) {
		this.org = org;
	}

	/**
	 * @hibernate.property column="title" length="128" type="string"
	 */
	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = title;
	}
	
	/**
	 * @hibernate.property type="com.haina.beluga.contact.domain.enumerate.SexEnumHibernateType"
	 * @hibernate.column name="sex" sql-type="smallint"
	 */
	public SexEnum getSex() {
		return sex;
	}

	public void setSex(SexEnum sex) {
		this.sex = sex;
	}
	
	@Override
	public boolean equals(Object object) {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public int hashCode() {
		// TODO Auto-generated method stub
		return 0;
	}

	@Override
	public String toString() {
		// TODO Auto-generated method stub
		return null;
	}

	/**
	 * 数据库手动修改记录。
	 * @hibernate.property column="remark" length="2000" type = "java.lang.String"
	 */
	public String getRemark() {
		return remark;
	}
	
	public void setRemark(String remark) {
		this.logger.warn("remark column is used for database log only,it can not be set.");
	}

	/**
	 * @hibernate.property column="registerFlag" type="boolean" not-null="true"
	 * @return
	 */
	public Boolean getRegisterFlag() {
		return registerFlag;
	}

	public void setRegisterFlag(Boolean registerFlag) {
		this.registerFlag = registerFlag;
	}
}
