package com.haina.beluga.album.domain;

import java.util.Date;

import com.sihus.core.model.VersionalModel;
import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;

/**
 * 用户相册信息类。<br/>
 * @author huangyongqiang
 * @since 2010-05-21
 * @hibernate.class table="UserAlbumInfo" optimistic-lock="version"
 * @hibernate.cache usage="read-write"
 */
public class UserAlbumInfo extends VersionalModel {

	private static final long serialVersionUID = -7434821470400725350L;

	/**
	 * 相册名称
	 */
	private String albumName;
	
	/**
	 * 相册描述
	 */
	private String description;
	
	/**
	 * 创建用户
	 */
	private String createUserId;
	
	/**
	 * 创建时间
	 */
	private Date createTime;
	
	/**
	 * 最后更新用户
	 */
	private String lastUpdateUserId;
	
	/**
	 * 最后更新时间
	 */
	private Date lastUpdateTime;
	
	public UserAlbumInfo() {
		Date now=new Date();
		this.createTime=now;
		this.lastUpdateTime=now;
		this.deleteFlag=Boolean.FALSE;
		this.description="";
	}

	/**
	 * @hibernate.version
	 * @hibernate.column name="version" type="long"
	 */
	public long getVersion() {
		return this.version;
	}
	
	/**
	 * 
	 * @hibernate.id unsaved-value="null" type = "java.lang.String" length="32"
	 * @hibernate.column name="id" sql-type="char(32)"
	 * @hibernate.generator class="uuid.hex"
	 */
	public String getId() {
		return this.id;
	}
	
	/**
	 * @hibernate.property not-null="true" type = "string"
	 * @hibernate.column name="albumName" sql-type="varchar(128) default ''"
	 */
	public String getAlbumName() {
		return albumName;
	}

	public void setAlbumName(String albumName) {
		this.albumName = albumName;
	}

	/**
	 * @hibernate.property not-null="false" type = "string"
	 * @hibernate.column name="description" sql-type="varchar(2000)"
	 */
	public String getDescription() {
		return description;
	}

	public void setDescription(String description) {
		this.description = description;
	}

	/**
	 * @hibernate.property not-null="true" type = "string"
	 * @hibernate.column name="createUser_id" sql-type="char(32)"
	 */
	public String getCreateUserId() {
		return createUserId;
	}

	public void setCreateUserId(String createUserId) {
		this.createUserId = createUserId;
	}

	/**
	 * @hibernate.property not-null="true"
	 * @hibernate.column name="createTime"
	 */
	public Date getCreateTime() {
		return createTime;
	}

	public void setCreateTime(Date createTime) {
		this.createTime = createTime;
	}

	/**
	 * @hibernate.property not-null="true" type = "string"
	 * @hibernate.column name="lastUpdateUser_id" sql-type="char(32)"
	 */
	public String getLastUpdateUserId() {
		return lastUpdateUserId;
	}

	public void setLastUpdateUserId(String lastUpdateUserId) {
		this.lastUpdateUserId = lastUpdateUserId;
	}

	/**
	 * @hibernate.property not-null="true"
	 * @hibernate.column name="lastUpdateTime"
	 */
	public Date getLastUpdateTime() {
		return lastUpdateTime;
	}

	public void setLastUpdateTime(Date lastUpdateTime) {
		this.lastUpdateTime = lastUpdateTime;
	}
	
	/**
	 * @hibernate.property column="deleteFlag" type="java.lang.Boolean" not-null="true"
	 * @return
	 */
	public Boolean getDeleteFlag() {
		return this.deleteFlag;
	}
	
	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof UserAlbumInfo)) {
			return false;
		}
		UserAlbumInfo rhs = (UserAlbumInfo) object;
		return new EqualsBuilder().append(
				this.createTime, rhs.createTime).append(this.createUserId,
				rhs.createUserId).append(this.albumName, rhs.albumName).append(
				this.lastUpdateTime, rhs.lastUpdateTime)
				.append(this.lastUpdateUserId, rhs.lastUpdateUserId)
				.append(this.deleteFlag, rhs.deleteFlag)
				.append(this.description, rhs.description).isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(-704096379, 1002631951).append(
				this.createTime).append(this.createUserId).append(this.albumName)
				.append(this.lastUpdateTime).append(this.description)
				.append(this.lastUpdateUserId).append(this.deleteFlag).toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("lastUpdateUser",
				this.lastUpdateUserId).append(
				"createTime", this.createTime)
				.append("lastUpdateTime", this.lastUpdateTime).append(
						"createUserId", this.createUserId).append("albumName",
						this.albumName).append("id",this.getId())
				.append("deleteFlag", this.deleteFlag)
				.append("description", this.description).toString();
	}
}