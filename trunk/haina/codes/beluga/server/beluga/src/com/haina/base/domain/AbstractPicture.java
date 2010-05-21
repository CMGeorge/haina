package com.haina.base.domain;

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
	
	@Override
	public long getVersion() {
		return this.version;
	}

	@Override
	public String getId() {
		return this.id;
	}

	public String getFilePath() {
		return filePath;
	}

	public void setFilePath(String filePath) {
		this.filePath = filePath;
	}

	public String getMime() {
		return mime;
	}

	public void setMime(String mime) {
		this.mime = mime;
	}

	public String getOriFileName() {
		return oriFileName;
	}

	public void setOriFileName(String oriFileName) {
		this.oriFileName = oriFileName;
	}

	
}
