package com.haina.beluga.album.dto;

import com.sihus.core.dto.IDto;
import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;

/**
 * 用户相片评论数据传输类。<br/>
 * @author huangyongqiang
 * @since 2010-05-31
 */
public class UserPhotoCommentDto implements IDto {

	private static final long serialVersionUID = 1684765649974829577L;

	/**
	 * 评论id
	 */
	private String id;
	
	/**
	 * 内容
	 */
	private String content;
	
	/**
	 * 标题
	 */
	private String title;
	
	/**
	 * 创建时间
	 */
	private String createTime;
	
	/**
	 * 创建用户登录邮箱
	 */
	private String createUserEmail;
	
	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public String getContent() {
		return content;
	}

	public void setContent(String content) {
		this.content = (content!=null?content:"");
	}

	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = (title!=null?title:"");
	}

	public String getCreateTime() {
		return createTime;
	}

	public void setCreateTime(String createTime) {
		this.createTime = createTime;
	}

	public String getCreateUserEmail() {
		return createUserEmail;
	}

	public void setCreateUserEmail(String createUserEmail) {
		this.createUserEmail = (createUserEmail!=null?createUserEmail:"");
	}

	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof UserPhotoCommentDto)) {
			return false;
		}
		UserPhotoCommentDto rhs = (UserPhotoCommentDto) object;
		return new EqualsBuilder().appendSuper(super.equals(object)).append(
				this.content, rhs.content).append(this.id, rhs.id).append(
				this.createTime, rhs.createTime).append(this.title, rhs.title)
				.append(this.createUserEmail, rhs.createUserEmail).isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(-1840702149, -1470917389).appendSuper(
				super.hashCode()).append(this.content).append(this.id).append(
				this.createTime).append(this.title)
				.append(this.createUserEmail).toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("content", this.content)
				.append("createTime", this.createTime).append("title",
						this.title).append("createUserEmail",
						this.createUserEmail).append("id", this.id).toString();
	}
}