package com.haina.beluga.album.dto;

import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;

import com.sihus.core.dto.IDto;

/**
 * 用户相册信息数据传输类。<br/>
 * @author huangyongqiang
 * @since 2010-05-23
 */
public class UserAlbumInfoDto implements IDto {

	private static final long serialVersionUID = 2991476535487940219L;
	
	/**
	 * 相册id
	 */
	private String id;
	
	/**
	 * 相册名称
	 */
	private String albumName;
	
	/**
	 * 创建时间
	 */
	private String createTime;
	
	/**
	 * 相片数量
	 */
	private long photoAmount;
	
	/**
	 * 相册封面
	 */
	private UserAlbumCoverDto cover;

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public String getAlbumName() {
		return albumName;
	}

	public void setAlbumName(String albumName) {
		this.albumName = albumName;
	}

	public String getCreateTime() {
		return createTime;
	}

	public void setCreateTime(String createTime) {
		this.createTime = createTime;
	}

	public long getPhotoAmount() {
		return photoAmount;
	}

	public void setPhotoAmount(long photoAmount) {
		this.photoAmount = photoAmount;
	}

	public UserAlbumCoverDto getCover() {
		return cover;
	}

	public void setCover(UserAlbumCoverDto cover) {
		this.cover = cover;
	}

	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof UserAlbumInfoDto)) {
			return false;
		}
		UserAlbumInfoDto rhs = (UserAlbumInfoDto) object;
		return new EqualsBuilder().appendSuper(super.equals(object)).append(
				this.id, rhs.id).append(this.createTime, rhs.createTime)
				.append(this.cover, rhs.cover).append(this.albumName,
						rhs.albumName)
				.append(this.photoAmount, rhs.photoAmount).isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(-236233859, 563357151).appendSuper(
				super.hashCode()).append(this.id).append(this.createTime)
				.append(this.cover).append(this.albumName).append(
						this.photoAmount).toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("createTime", this.createTime)
				.append("cover", this.cover).append("photoAmount",
						this.photoAmount).append("albumName", this.albumName)
				.append("id", this.id).toString();
	}
	
	
}