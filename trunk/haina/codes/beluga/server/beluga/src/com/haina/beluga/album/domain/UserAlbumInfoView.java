package com.haina.beluga.album.domain;

import java.util.Date;

import com.sihus.core.model.IModel;
import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;

/**
 * 用户相册信息视图类。<br/>
 * @author huangyongqiang
 * @since 2010-05-23
 */
public class UserAlbumInfoView implements IModel {

	private static final long serialVersionUID = 4538896484166259186L;

	/**
	 * 相册id
	 */
	private String albumId;
	
	/**
	 * 相册名称
	 */
	private String albumName;
	
	/**
	 * 创建时间
	 */
	private Date createTime;
	
	/**
	 * 相片数量
	 */
	private long photoAmount;
	
	/**
	 * 相册封面文件存储路径
	 */
	private String coverFilePath;

	
	public UserAlbumInfoView(String albumId, String albumName, Date createTime,
			long photoAmount, String coverFilePath) {
		super();
		this.albumId = albumId;
		this.albumName = albumName;
		this.createTime = createTime;
		this.photoAmount = photoAmount;
		this.coverFilePath = coverFilePath;
	}

	public String getAlbumId() {
		return albumId;
	}

	public void setAlbumId(String albumId) {
		this.albumId = albumId;
	}

	public String getAlbumName() {
		return albumName;
	}

	public void setAlbumName(String albumName) {
		this.albumName = albumName;
	}

	public Date getCreateTime() {
		return createTime;
	}

	public void setCreateTime(Date createTime) {
		this.createTime = createTime;
	}

	public long getPhotoAmount() {
		return photoAmount;
	}

	public void setPhotoAmount(long photoAmount) {
		this.photoAmount = photoAmount;
	}
	
	public String getCoverFilePath() {
		return coverFilePath;
	}

	public void setCoverFilePath(String coverFilePath) {
		this.coverFilePath = coverFilePath;
	}

	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof UserAlbumInfoView)) {
			return false;
		}
		UserAlbumInfoView rhs = (UserAlbumInfoView) object;
		return new EqualsBuilder().appendSuper(super.equals(object)).append(
				this.createTime, rhs.createTime).append(this.albumName,
				rhs.albumName).append(this.albumId, rhs.albumId).append(
				this.photoAmount, rhs.photoAmount)
				.append(this.coverFilePath, rhs.coverFilePath).isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(370916227, -695696901).appendSuper(
				super.hashCode()).append(this.createTime)
				.append(this.albumName).append(this.albumId).append(
						this.photoAmount).append(this.coverFilePath).toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("albumId", this.albumId)
				.append("createTime", this.createTime).append("photoAmount",
						this.photoAmount).append("albumName", this.albumName)
				.append("coverFilePath", this.coverFilePath).toString();
	}
}