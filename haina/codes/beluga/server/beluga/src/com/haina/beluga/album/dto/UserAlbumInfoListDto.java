package com.haina.beluga.album.dto;

import java.util.Collection;

import com.sihus.core.dto.IDto;
import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;

/**
 * 用户相册信息列表数据传输类。<br/>
 * @author huangyongqiang
 * @since 2010-05-27
 */
public class UserAlbumInfoListDto implements IDto {

	private static final long serialVersionUID = 4174869449839905474L;
	
	/**
	 * 总页数
	 */
	private long pageCount;
	
	/**
	 * 总条数
	 */
	private long rowsCount;
	
	/**
	 * 当前显示页的相册
	 */
	private Collection<UserAlbumInfoDto> albums;

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

	public Collection<UserAlbumInfoDto> getAlbums() {
		return albums;
	}

	public void setAlbums(Collection<UserAlbumInfoDto> albums) {
		this.albums = albums;
	}

	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof UserAlbumInfoListDto)) {
			return false;
		}
		UserAlbumInfoListDto rhs = (UserAlbumInfoListDto) object;
		return new EqualsBuilder().appendSuper(super.equals(object)).append(
				this.rowsCount, rhs.rowsCount).append(this.pageCount,
				rhs.pageCount).append(this.albums, rhs.albums).isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(1562790713, 2017714333).appendSuper(
				super.hashCode()).append(this.rowsCount).append(this.pageCount)
				.append(this.albums).toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("pageCount", this.pageCount)
				.append("albums", this.albums).append("rowsCount",
						this.rowsCount).toString();
	}
}