package com.haina.base.domain;

import java.util.Date;

import com.sihus.core.model.VersionalModel;

/**
 * 抽象帖子类，所有发帖，评论相关类的基类。<br/>
 * @author huangyongqiang
 * @since 2010-05-21
 */
public abstract class AbstractPostsComment extends VersionalModel {

	private static final long serialVersionUID = -541750882292437393L;
	
	/**
	 * 内容
	 */
	protected String content;
	
	/**
	 * 标题
	 */
	protected String title;
	
	/**
	 * 创建时间
	 */
	protected Date createTime;
	
	/**
	 * 最后更新时间
	 */
	protected Date lastUpdateTime;
	
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
