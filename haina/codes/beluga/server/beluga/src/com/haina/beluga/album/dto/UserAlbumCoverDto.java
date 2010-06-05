package com.haina.beluga.album.dto;

import com.sihus.core.dto.IDto;
import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;

/**
 * 用户相册封面数据传输类。<br/>
 * @author huangyongqiang
 * @since 2010-05-23
 */
public class UserAlbumCoverDto implements IDto {

	private static final long serialVersionUID = -193245549097529393L;
	
	/**
	 * 封面图片显示的url
	 */
	private String coverUrl;
	
	/**
	 * 封面图片文件
	 */
	private byte[] coverFile;

	public String getCoverUrl() {
		return coverUrl;
	}

	public void setCoverUrl(String coverUrl) {
		this.coverUrl = (coverUrl!=null?coverUrl:"");
	}

	public byte[] getCoverFile() {
		return coverFile;
	}

	public void setCoverFile(byte[] coverFile) {
		this.coverFile = coverFile;
	}

	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof UserAlbumCoverDto)) {
			return false;
		}
		UserAlbumCoverDto rhs = (UserAlbumCoverDto) object;
		return new EqualsBuilder().appendSuper(super.equals(object)).append(
				this.coverUrl, rhs.coverUrl).append(this.coverFile,
				rhs.coverFile).isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(525102969, -664513091).appendSuper(
				super.hashCode()).append(this.coverUrl).append(this.coverFile)
				.toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("coverUrl", this.coverUrl)
				.append("coverFile", this.coverFile).toString();
	}
}