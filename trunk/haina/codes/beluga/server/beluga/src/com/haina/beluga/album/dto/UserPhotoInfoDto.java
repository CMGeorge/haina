package com.haina.beluga.album.dto;

import com.sihus.core.dto.IDto;

/**
 * 用户相片信息数据传输类。<br/>
 * @author huangyongqiang
 * @since 2010-05-27
 */
public class UserPhotoInfoDto implements IDto {

	private static final long serialVersionUID = 8948583764701706889L;
	
	/**
	 * 相片id
	 */
	private String id;
	
	/**
	 * 相片名称
	 */
	private String photoName;
	
	/**
	 * 创建时间
	 */
	private String createTime;
	
	/**
	 * 相片描述
	 */
	private String photoDescription;
	
	/**
	 * 相片图片文件
	 */
	private byte[] photoFile;
	
	/**
	 * 相片的评论列表
	 */
	private UserPhotoCommentListDto commentList;

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public String getPhotoName() {
		return photoName;
	}

	public void setPhotoName(String photoName) {
		this.photoName = photoName;
	}

	public String getCreateTime() {
		return createTime;
	}

	public void setCreateTime(String createTime) {
		this.createTime = createTime;
	}

	public String getPhotoDescription() {
		return photoDescription;
	}

	public void setPhotoDescription(String photoDescription) {
		this.photoDescription = photoDescription;
	}

	public byte[] getPhotoFile() {
		return photoFile;
	}

	public void setPhotoFile(byte[] photoFile) {
		this.photoFile = photoFile;
	}

	public UserPhotoCommentListDto getCommentList() {
		return commentList;
	}

	public void setCommentList(UserPhotoCommentListDto commentList) {
		this.commentList = commentList;
	}
}