package com.haina.base.domain;

import java.util.Date;

import com.sihus.core.model.VersionalModel;

/**
 * 抽象图片类，所有图片图像相关类的基类。<br/>
 * @author huangyongqiang
 * @since 2010-05-20
 */
public abstract class AbstractPicture extends VersionalModel {

	private static final long serialVersionUID = -6322581764569919711L;

	/**
	 * 存放的文件路径
	 */
	protected String filePath;
	
	/**
	 * 图片的mime类型
	 */
	protected String mime;
	
	/**
	 * 图片原始文件名
	 */
	protected String oriFileName;
	
	/**
	 * 创建时间
	 */
	protected Date createTime;
	
	/**
	 * @hibernate.property not-null="true" type = "string"
	 * @hibernate.column name="mime" sql-type="varchar(64)"
	 */
	public String getMime() {
		return mime;
	}

	public void setMime(String mime) {
		this.mime = mime;
	}

	/**
	 * @hibernate.property not-null="true" type = "string"
	 * @hibernate.column name="oriFileName" sql-type="varchar(512)"
	 */
	public String getOriFileName() {
		return oriFileName;
	}

	public void setOriFileName(String oriFileName) {
		this.oriFileName = oriFileName;
	}
	
	/**
	 * @hibernate.property not-null="true" type = "string"
	 * @hibernate.column name="filePath" sql-type="varchar(2000)"
	 */
	public String getFilePath() {
		return filePath;
	}

	public void setFilePath(String filePath) {
		this.filePath = filePath;
	}
	
	/**
	 * @hibernate.property not-null="true"
	 * @hibernate.column name="createTime"
	 */
	public Date getCreateTime() {
		return createTime;
	}

	public void setCreateTime(Date createTime) {
		this.createTime = createTime;
	}	
}