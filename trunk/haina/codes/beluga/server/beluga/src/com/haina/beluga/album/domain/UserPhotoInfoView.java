package com.haina.beluga.album.domain;

import java.util.Date;

import com.sihus.core.model.IModel;

/**
 * 用户相片信息视图类。<br/>
 * @author huangyongqiang
 * @since 2010-05-26
 */
public class UserPhotoInfoView implements IModel {

	private static final long serialVersionUID = -1527786487751572574L;
	
	/**
	 * 相片id
	 */
	private String photoId;
	
	/**
	 * 相片名称
	 */
	private String photoName;
	
	/**
	 * 相片描述
	 */
	private String photoDescription;
	
	/**
	 * 创建时间
	 */
	private Date createTime;
	
	/**
	 * 相片文件存储路径
	 */
	private String photoFilePath;

	public UserPhotoInfoView(String photoId, String photoName,
			String photoDescription, Date createTime, String photoFilePath) {
		super();
		this.photoId = photoId;
		this.photoName = photoName;
		this.photoDescription = photoDescription;
		this.createTime = createTime;
		this.photoFilePath = photoFilePath;
	}

	public String getPhotoId() {
		return photoId;
	}

	public void setPhotoId(String photoId) {
		this.photoId = photoId;
	}

	public String getPhotoName() {
		return photoName;
	}

	public void setPhotoName(String photoName) {
		this.photoName = photoName;
	}

	public String getPhotoDescription() {
		return photoDescription;
	}

	public void setPhotoDescription(String photoDescription) {
		this.photoDescription = photoDescription;
	}

	public Date getCreateTime() {
		return createTime;
	}

	public void setCreateTime(Date createTime) {
		this.createTime = createTime;
	}

	public String getPhotoFilePath() {
		return photoFilePath;
	}

	public void setPhotoFilePath(String photoFilePath) {
		this.photoFilePath = photoFilePath;
	}	
}