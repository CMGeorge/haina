package com.haina.beluga.album.dto;

import java.util.Collection;

import com.sihus.core.dto.IDto;

/**
 * 用户相片评论列表数据传输类。<br/>
 * @author huangyongqiang
 * @since 2010-05-31
 */
public class UserPhotoCommentListDto implements IDto {

	private static final long serialVersionUID = -7791375410893386047L;
	
	/**
	 * 总页数
	 */
	private long pageCount;
	
	/**
	 * 总条数
	 */
	private long rowsCount;
	
	/**
	 * 当前显示页的相册评论
	 */
	private Collection<UserPhotoCommentDto> comments;

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

	public Collection<UserPhotoCommentDto> getComments() {
		return comments;
	}

	public void setComments(Collection<UserPhotoCommentDto> comments) {
		this.comments = comments;
	}
}
