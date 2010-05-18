package com.haina.beluga.contact.domain;

import org.springframework.stereotype.Component;

import com.sihus.core.model.VersionalModel;

/**
 * 联系人扩展信息的领域模型类。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-07-02
 * @hibernate.class table="UserProfileExt" optimistic-lock="version"
 * @hibernate.cache usage="read-write"
 */

public class UserProfileExt extends VersionalModel {

	private static final long serialVersionUID = 2938237837519242469L;
	
	/*通讯类型，1x表示电话、2x表示email、3x表示地址、4x表示IM；x1表示家庭、x2表示工作、x3表示其他。comm_key是类型组合值。*/
	private Integer commKey;
	
	/*通讯信息，包括电话号码、email、联系地址id、IM账号。*/
	private String commValue;
	
	private ContactUser contactUser;
	
	/*数据库手动修改记录。*/
	private String remark;

	/**
	 * @hibernate.id unsaved-value="null" type = "string" length="32"
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
		return this.version;
	}

	/**
	 * @hibernate.property type="integer" not-null="true"
	 * @hibernate.column name="commKey" sql-type="smallint"
	 */
	public Integer getCommKey() {
		return commKey;
	}

	public void setCommKey(Integer commKey) {
		this.commKey = commKey;
	}

	/**
	 * @hibernate.property type="string" not-null="true" length="128"
	 * @hibernate.column name="commValue" sql-type="varchar(256)"
	 */
	public String getCommValue() {
		return commValue;
	}

	public void setCommValue(String commValue) {
		this.commValue = commValue;
	}
	
	/** 
	 * @hibernate.many-to-one not-null="true" insert="true" update="false" cascade="save-update" lazy="false" outer-join="true"
	 * @hibernate.column name="userId" sql-type="char(32)"
	 * @return
	 */
	public ContactUser getContactUser() {
		return contactUser;
	}

	public void setContactUser(ContactUser contactUser) {
		this.contactUser = contactUser;
	}
	
	/**
	 * @hibernate.property column="remark" length="1000" type="string"
	 */
	public String getRemark() {
		return remark;
	}
	
	public void setRemark(String remark) {
		this.logger.warn("remark column is used for database log only,it can not be set.");
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

	
}
