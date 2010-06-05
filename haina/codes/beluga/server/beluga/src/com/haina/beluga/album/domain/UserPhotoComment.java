package com.haina.beluga.album.domain;

import java.util.Date;

import com.haina.base.domain.AbstractPostsComment;
import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;

/**
 * 用户相片评论类。<br/>
 * @author huangyongqiang
 * @since 2010-05-22
 * @hibernate.class table="UserPhotoComment" optimistic-lock="version"
 * @hibernate.cache usage="read-write"
 */
public class UserPhotoComment extends AbstractPostsComment {

	private static final long serialVersionUID = 3253517222647875357L;

	/**
	 * 创建用户
	 */
	private String createUserId;
	
	/**
	 * 最后更新用户
	 */
	private String lastUpdateUserId;
	
	/**
	 * 最后更新时间
	 */
	private Date lastUpdateTime;
	
	/**
	 * 所属相片id
	 */
	private String userPhotoInfoId;
	
	public UserPhotoComment() {
		this.deleteFlag=Boolean.FALSE;
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
	 * @hibernate.version
	 * @hibernate.column name="version" type="long"
	 */
	public long getVersion() {
		return this.version;
	}

	/**
	 * @hibernate.property column="deleteFlag" type="java.lang.Boolean" not-null="true"
	 * @return
	 */
	public Boolean getDeleteFlag() {
		return this.deleteFlag;
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
		this.createTime = (createTime!=null?createTime:new Date());
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
		this.lastUpdateTime = (lastUpdateTime!=null?lastUpdateTime:new Date());
	}

	/**
	 * @hibernate.property not-null="true" type = "string"
	 * @hibernate.column name="userPhotoInfo_id" sql-type="char(32)"
	 */
	public String getUserPhotoInfoId() {
		return userPhotoInfoId;
	}

	public void setUserPhotoInfoId(String userPhotoInfoId) {
		this.userPhotoInfoId = userPhotoInfoId;
	}

	/**
	 * @hibernate.property not-null="true" type = "string"
	 * @hibernate.column name="content" sql-type="varchar(2000)"
	 */
	public String getContent() {
		return content;
	}

	public void setContent(String content) {
		this.content = content;
	}

	/**
	 * @hibernate.property not-null="true" type = "string"
	 * @hibernate.column name="content" sql-type="varchar(100)"
	 */
	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = title;
	}
	
	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof UserPhotoComment)) {
			return false;
		}
		UserPhotoComment rhs = (UserPhotoComment) object;
		return new EqualsBuilder().append(this.id, rhs.id).append(
				this.content, rhs.content).append(this.createTime,
				rhs.createTime).append(this.title, rhs.title).append(
				this.lastUpdateUserId, rhs.lastUpdateUserId).append(
				this.lastUpdateTime, rhs.lastUpdateTime).append(
				this.userPhotoInfoId, rhs.userPhotoInfoId).append(
				this.createUserId, rhs.createUserId)
				.append(this.deleteFlag, rhs.deleteFlag).isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(1352009999, 1694406983)
		.append(this.content).append(this.id).append(this.createTime)
				.append(this.title).append(this.lastUpdateUserId).append(
						this.lastUpdateTime).append(this.userPhotoInfoId)
				.append(this.createUserId).append(this.deleteFlag)
				.toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("createUserId", this.createUserId)
		.append("createTime",this.createTime).append("lastUpdateTime", this.lastUpdateTime)
		.append("lastUpdateUserId", this.lastUpdateUserId).append(
						"userPhotoInfoId", this.userPhotoInfoId).append(
						"deleteFlag", this.getDeleteFlag()).append("id",
						this.getId()).toString();
	}
}