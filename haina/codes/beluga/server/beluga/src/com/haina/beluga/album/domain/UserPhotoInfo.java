package com.haina.beluga.album.domain;

import java.util.Date;

import com.haina.base.domain.AbstractPicture;
import com.haina.beluga.album.domain.enumerate.UserPhotoSizeEnum;

import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;

/**
 * 用户相片信息类。<br/>
 * @author huangyongqiang
 * @since 2010-05-21
 * @hibernate.class table="UserPhotoInfo" optimistic-lock="version"
 * @hibernate.cache usage="read-write"
 */
public class UserPhotoInfo extends AbstractPicture {

	private static final long serialVersionUID = -9003532042110865414L;
	
	/**
	 * 相片名称
	 */
	private String photoName;
	
	/**
	 * 相片描述
	 */
	private String photoDescription;
	
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
	 * 所属相册id
	 */
	private String userAlbumInfoId;
	
	/**
	 * 相片在相册内的顺序号码
	 */
	private Integer seqNumber;
	
	/**
	 * 相片大小类型
	 */
	private UserPhotoSizeEnum photoSize;
	
	/**
	 * 是否是封面
	 */
	private Boolean coverFlag;
	
	public UserPhotoInfo() {
		this.deleteFlag=Boolean.FALSE;
		this.photoSize=UserPhotoSizeEnum.normal;
		this.coverFlag=Boolean.FALSE;
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
	 * @hibernate.property not-null="true" type = "string"
	 * @hibernate.column name="photoName" sql-type="varchar(128)"
	 */
	public String getPhotoName() {
		return photoName;
	}

	public void setPhotoName(String photoName) {
		this.photoName = photoName;
	}

	/**
	 * @hibernate.property not-null="false" type = "string"
	 * @hibernate.column name="photoName" sql-type="varchar(128)"
	 */
	public String getPhotoDescription() {
		return photoDescription;
	}

	public void setPhotoDescription(String photoDescription) {
		this.photoDescription = photoDescription;
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
	 * @hibernate.property not-null="true" type = "string"
	 * @hibernate.column name="userAlbumInfo_id" sql-type="char(32)"  unique-key="uk_userAlbumInfoId_seqNumber"
	 */
	public String getUserAlbumInfoId() {
		return userAlbumInfoId;
	}

	public void setUserAlbumInfoId(String userAlbumInfoId) {
		this.userAlbumInfoId = userAlbumInfoId;
	}

	/**
	 * @hibernate.property type="java.lang.Boolean" not-null="true"
	 * @hibernate.column name="deleteFlag" sql-type="int"
	 * @return
	 */
	public Boolean getDeleteFlag() {
		return this.deleteFlag;
	}
	
	/**
	 * @hibernate.property
	 * @hibernate.column name="seqNumber" sql-type="int default 1" unique-key="uk_userAlbumInfoId_seqNumber"
	 */
	public Integer getSeqNumber() {
		return seqNumber;
	}

	public void setSeqNumber(Integer seqNumber) {
		this.seqNumber = seqNumber;
	}

	/**
	 * @hibernate.property not-null="true" type="com.haina.beluga.album.domain.enumerate.UserPhotoSizeEnumHibernateType"
	 * @hibernate.column name="photoSize" sql-type="smallint"
	 */
	public UserPhotoSizeEnum getPhotoSize() {
		return photoSize;
	}
	
	public void setPhotoSize(UserPhotoSizeEnum photoSize) {
		this.photoSize = photoSize;
	}

	/**
	 * @hibernate.property type="java.lang.Boolean" not-null="true"
	 * @hibernate.column name="coverFlag"
	 * @return
	 */
	public Boolean getCoverFlag() {
		return coverFlag;
	}

	public void setCoverFlag(Boolean coverFlag) {
		this.coverFlag = (coverFlag!=null?coverFlag:Boolean.FALSE);
	}

	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof UserPhotoInfo)) {
			return false;
		}
		UserPhotoInfo rhs = (UserPhotoInfo) object;
		return new EqualsBuilder().append(this.id, rhs.id)
		.append(this.deleteFlag, rhs.deleteFlag).append(
				this.createTime, rhs.createTime).append(this.lastUpdateUserId,
				rhs.lastUpdateUserId).append(this.filePath, rhs.filePath)
				.append(this.lastUpdateTime, rhs.lastUpdateTime).append(
						this.photoName, rhs.photoName).append(this.mime,
						rhs.mime).append(this.oriFileName, rhs.oriFileName)
				.append(this.photoDescription, rhs.photoDescription).append(
						this.seqNumber, rhs.seqNumber).append(
						this.userAlbumInfoId, rhs.userAlbumInfoId).append(
						this.createUserId, rhs.createUserId)
		.append(this.photoSize, rhs.photoSize).append(this.picUrl, rhs.picUrl)
		.append(this.coverFlag, rhs.coverFlag).isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(-1705946853, 393743399).append(this.id)
		.append(this.createTime).append(this.lastUpdateUserId).append(this.filePath).append(
				this.lastUpdateTime).append(this.photoName).append(this.mime)
				.append(this.oriFileName).append(this.photoDescription).append(
						this.seqNumber).append(this.userAlbumInfoId).append(
						this.createUserId).append(this.deleteFlag).append(this.photoSize)
				.append(this.picUrl).append(this.coverFlag).toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("createUserId",
				this.createUserId).append("oriFileName", this.oriFileName)
				.append("seqNumber", this.seqNumber)
				.append("lastUpdateTime",
						this.lastUpdateTime).append("userAlbumInfoId",
						this.userAlbumInfoId).append("filePath", this.filePath)
				.append("lastUpdateUserId", this.lastUpdateUserId).append(
						"deleteFlag", this.getDeleteFlag()).append("photoName",
						this.photoName).append("photoDescription",
						this.photoDescription)
				.append("createTime", this.createTime).append("id",
						this.getId()).append("mime", this.mime)
				.append("photoSize", this.photoSize)
				.append("picUrl", this.picUrl)
				.append("coverFlag", this.coverFlag).toString();
	}
}