package com.haina.beluga.album.dto;

import java.util.Collection;

import com.sihus.core.dto.IDto;
import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;

/**
 * 用户相片信息列表数据传输类。<br/>
 * @author huangyongqiang
 * @since 2010-05-27
 */
public class UserPhotoInfoListDto implements IDto {

	private static final long serialVersionUID = -7867639836131285303L;
	
	/**
	 * 总页数
	 */
	private long pageCount;
	
	/**
	 * 总条数
	 */
	private long rowsCount;
	
	/**
	 * 当前显示页的相片
	 */
	private Collection<UserPhotoInfoDto> photos;

	public long getPageCount() {
		return pageCount;
	}

	public void setPageCount(long pageCount) {
		this.pageCount = pageCount;
	}

	public long getRowsCount() {
		return rowsCount;
	}

	public void setRowsCount(long rowsCount) {
		this.rowsCount = rowsCount;
	}

	public Collection<UserPhotoInfoDto> getPhotos() {
		return photos;
	}

	public void setPhotos(Collection<UserPhotoInfoDto> photos) {
		this.photos = photos;
	}

	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof UserPhotoInfoListDto)) {
			return false;
		}
		UserPhotoInfoListDto rhs = (UserPhotoInfoListDto) object;
		return new EqualsBuilder().appendSuper(super.equals(object)).append(
				this.photos, rhs.photos).append(this.rowsCount, rhs.rowsCount)
				.append(this.pageCount, rhs.pageCount).isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(1106411809, 715819081).appendSuper(
				super.hashCode()).append(this.photos).append(this.rowsCount)
				.append(this.pageCount).toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("pageCount", this.pageCount)
				.append("photos", this.photos).append("rowsCount",
						this.rowsCount).toString();
	}

}