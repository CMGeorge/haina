package com.haina.beluga.album.domain;

import com.haina.base.domain.AbstractPicture;

import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;

/**
 * 用户相册封面类。<br/>
 * @author huangyongqiang
 * @since 2010-05-21
 * @hibernate.class table="UserAlbumCover" optimistic-lock="version"
 * @hibernate.cache usage="read-write"
 */
public class UserAlbumCover extends AbstractPicture {

	private static final long serialVersionUID = 1502713771536161551L;
	
	/**
	 * 所属用户相册id
	 */
	private String userAlbumInfoId;
	
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
	 * @hibernate.column name="userAlbumInfo_id" sql-type="char(32)"
	 */
	public String getUserAlbumInfoId() {
		return userAlbumInfoId;
	}

	public void setUserAlbumInfoId(String userAlbumInfoId) {
		this.userAlbumInfoId = userAlbumInfoId;
	}

	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof UserAlbumCover)) {
			return false;
		}
		UserAlbumCover rhs = (UserAlbumCover) object;
		return new EqualsBuilder().append(this.id, rhs.id).append(
				this.filePath, rhs.filePath).append(this.mime, rhs.mime)
				.append(this.oriFileName, rhs.oriFileName).append(
						this.userAlbumInfoId, rhs.userAlbumInfoId)
				.append(this.picUrl, rhs.picUrl).isEquals();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this)
				.append("oriFileName", this.oriFileName).append(
						"userAlbumInfoId", this.userAlbumInfoId)
						.append("filePath", this.filePath).append(
						"mime", this.mime).append("id", this.getId())
				.append("picUrl",this.picUrl).toString();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(128578997, 548760465).append(this.id)
		.append(this.filePath).append(this.mime)
				.append(this.oriFileName).append(this.userAlbumInfoId)
				.append(this.picUrl).toHashCode();
	}
}