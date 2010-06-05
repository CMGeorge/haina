package com.haina.beluga.album.domain;

import java.util.Date;

import com.sihus.core.model.IModel;

/**
 * 用户相片评论视图类。<br/>
 * @author huangyongqiang
 * @since 2010-05-31
 */
public class UserPhotoCommentView implements IModel {

	private static final long serialVersionUID = -4046803913104012029L;
	
	/**
	 * 评论id
	 */
	private String commentId;
	
	/**
	 * 评论内容
	 */
	private String content;
	
	/**
	 * 评论标题
	 */
	private String title;
	
	/**
	 * 创建时间
	 */
	private Date createTime;
	
	/**
	 * 创建用户登录邮箱
	 */
	private String createUserEmail;
	
	/**
	 * 创建用户手机号码
	 */
	private String createUserMobile;

	
	public UserPhotoCommentView(String commentId, String content, String title,
			Date createTime, String createUserEmail, String createUserMobile) {
		super();
		this.commentId = commentId;
		this.content = content;
		this.title = title;
		this.createTime = createTime;
		this.createUserEmail = createUserEmail;
		this.createUserMobile = createUserMobile;
	}

	public String getCommentId() {
		return commentId;
	}

	public void setCommentId(String commentId) {
		this.commentId = commentId;
	}

	public String getContent() {
		return content;
	}

	public void setContent(String content) {
		this.content = content;
	}

	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = title;
	}

	public Date getCreateTime() {
		return createTime;
	}

	public void setCreateTime(Date createTime) {
		this.createTime = createTime;
	}

	public String getCreateUserEmail() {
		return createUserEmail;
	}

	public void setCreateUserEmail(String createUserEmail) {
		this.createUserEmail = createUserEmail;
	}

	public String getCreateUserMobile() {
		return createUserMobile;
	}

	public void setCreateUserMobile(String createUserMobile) {
		this.createUserMobile = createUserMobile;
	}
}
