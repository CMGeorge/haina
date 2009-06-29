package com.haina.beluga.domain;

import java.util.Date;

import org.springframework.stereotype.Component;

import com.haina.beluga.core.model.VersionalModel;
import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;

/**
 * 联系人标签的领域模型类。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-06-26
 * @hibernate.class table="ContactTag"
 * @hibernate.cache usage="read-write"
 */

@Component
public class ContactTag extends VersionalModel {

	private static final long serialVersionUID = 7124161663842810931L;
	
	private String name;
	
	/*标签图标存储路径。*/
	private String logo;
	
	/*创建人登录名。*/
	private String ceateUser;
	
	private Date createTime;
	
	/*ceateUser所有标签中的排列顺序。*/
	private Integer tagOrder;
	
	/*是否可以被删除。*/
	private Boolean deleteFlag;
	
	/*数据库手动修改记录。*/
	private String remark;

	/**
	 * @hibernate.version column="version" type = "java.lang.Long"
	 */
	@Override
	public Long getVersion() {
		// TODO Auto-generated method stub
		return null;
	}

	/**
	 * @hibernate.id column="id"  unsaved-value="null" type = "java.lang.String" length="32"
	 * @hibernate.generator class="uuid.hex"
	 */
	@Override
	public String getId() {
		return this.id;
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

	/**
	 * 创建人登录名。<br/>
	 * @hibernate.property column="ceateUser" type="java.lang.String" length="64" not-null="true"
	 */
	public String getCeateUser() {
		return ceateUser;
	}

	public void setCeateUser(String ceateUser) {
		this.ceateUser = ceateUser;
	}

	/**
	 * @hibernate.property column="createTime" type="java.util.Date" not-null="true"
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
	 * 数据库手动修改记录。<br/>
	 * @hibernate.property column="remark" length="2000" type = "java.lang.String"
	 */
	public String getRemark() {
		return remark;
	}

	public void setRemark(String remark) {
		this.remark = remark;
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
				.append(this.ceateUser,rhs.ceateUser).append(this.name, rhs.name).append(
						this.tagOrder, rhs.tagOrder).append(this.deleteFlag,
						rhs.deleteFlag)
				.isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(-995632337, -1369080159).append(this.createTime).append(this.logo)
				.append(this.ceateUser).append(this.name).append(this.id)
				.append(this.tagOrder).append(this.deleteFlag).toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("logo", this.logo).append(
				"tagOrder", this.tagOrder).append(
				"createTime", this.createTime).append("name", this.name)
				.append("ceateUser", this.ceateUser).append("deleteFlag",
						this.deleteFlag).append("id", this.getId()).toString();
	}
	
	
}
