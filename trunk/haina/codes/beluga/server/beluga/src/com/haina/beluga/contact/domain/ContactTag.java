package com.haina.beluga.contact.domain;

import java.util.Date;

import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;

import com.sihus.core.model.VersionalModel;

/**
 * 联系人标签的领域模型类。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-06-26
 * @hibernate.class table="ContactTag" optimistic-lock="version"
 * @hibernate.cache usage="read-write"
 */

public class ContactTag extends VersionalModel {

	private static final long serialVersionUID = 7124161663842810931L;
	
	private String name;
	
	/*标签图标存储路径。*/
	private String logo;
	
//	/*创建人登录名。*/
//	private String ceateUser;
	
	private Date createTime;
	
	/*ceateUser所有标签中的排列顺序。*/
	private Integer tagOrder;
	
	/*是否可以被删除。*/
	private Boolean deleteFlag;
	
	/*标签所属用户。*/
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
		return this.id;
	}

	
	/**
	 * @hibernate.version column="version" type="long"
	 */
	@Override
	public long getVersion() {
		return this.version;
	}
	
	/**
	 * @hibernate.property column="name" type = "java.lang.String" length="64" not-null="true"
	 */
	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	/**
	 * 标签图标存储路径。<br/>
	 * @hibernate.property column="logo" type="java.lang.String" length="128" not-null="true"
	 */
	public String getLogo() {
		return logo;
	}

	public void setLogo(String logo) {
		this.logo = logo;
	}

//	/**
//	 * 创建人登录名。<br/>
//	 * @hibernate.property column="ceateUser" type="java.lang.String" length="64" not-null="true"
//	 */
//	public String getCeateUser() {
//		return ceateUser;
//	}
//
//	public void setCeateUser(String ceateUser) {
//		this.ceateUser = ceateUser;
//	}

	/**
	 * @hibernate.property type="java.util.Date" not-null="true"
	 * @hibernate.column name="createTime" sql-type="timestamp"
	 */
	public Date getCreateTime() {
		return createTime;
	}

	public void setCreateTime(Date createTime) {
		this.createTime = createTime;
	}

	/**
	 * ceateUser所有标签中的排列顺序。<br/>
	 * @hibernate.property column="tagOrder" type="java.lang.Integer" not-null="true"
	 */
	public Integer getTagOrder() {
		return tagOrder;
	}

	public void setTagOrder(Integer tagOrder) {
		this.tagOrder = tagOrder;
	}

	/**
	 * 是否可以被删除。<br/>
	 * @hibernate.property column="deleteFlag" type="java.lang.Boolean" not-null="true"
	 */
	public Boolean getDeleteFlag() {
		return deleteFlag;
	}

	public void setDeleteFlag(Boolean deleteFlag) {
		this.deleteFlag = deleteFlag;
	}

	/**
	 * 标签所属用户。<br/>
	 * @hibernate.many-to-one not-null="true" insert="true" update="false" cascade="save-update" lazy="false" outer-join="true"
	 * @hibernate.column name="createUser" sql-type="char(32)"
	 * @return
	 */
	public ContactUser getContactUser() {
		return contactUser;
	}

	public void setContactUser(ContactUser contactUser) {
		this.contactUser = contactUser;
	}
	
	/**
	 * 数据库手动修改记录。<br/>
	 * @hibernate.property column="remark" length="2000" type = "java.lang.String"
	 */
	public String getRemark() {
		return remark;
	}

	public void setRemark(String remark) {
		this.logger.warn("remark column is used for database log only,it can not be set.");
	}

	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof ContactTag)) {
			return false;
		}
		ContactTag rhs = (ContactTag) object;
		return new EqualsBuilder().append(this.id, rhs.id).append(
				this.createTime, rhs.createTime).append(this.logo, rhs.logo)
				.append(this.contactUser,rhs.contactUser).append(this.name, rhs.name).append(
						this.tagOrder, rhs.tagOrder).append(this.deleteFlag,
						rhs.deleteFlag)
				.isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(-995632337, -1369080159).append(this.createTime).append(this.logo)
				.append(this.contactUser).append(this.name).append(this.id)
				.append(this.tagOrder).append(this.deleteFlag).toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("logo", this.logo).append(
				"tagOrder", this.tagOrder).append(
				"createTime", this.createTime).append("name", this.name)
				.append("ceateUser", this.contactUser).append("deleteFlag",
						this.deleteFlag).append("id", this.getId()).toString();
	}
}
