package com.haina.beluga.contact.domain;

import java.util.Date;

import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;
import org.springframework.stereotype.Component;

import com.haina.beluga.contact.domain.enumerate.SexEnum;
import com.haina.core.model.VersionalModel;

/**
 * 用户详细信息表的领域模型类。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-07-01
 * @hibernate.class table="UserProfile" optimistic-lock="version"
 * @hibernate.cache usage="read-write"
 */
@Component
public class UserProfile extends VersionalModel {

	private static final long serialVersionUID = -5866939969778808915L;
	
	private String nickName;
	
	private String name;
	
	private Integer age;
	
	private SexEnum sex;
	
	/*头像存储路径。*/
	private String photo;
	
	private String identification;
	
	private Date brithday;
	
	/*个人主页或博客。*/
	private String url;
	
	private String signature;
	
	/*首选email，默认是登录名。*/
	private String emailPref;
	
	private String telPref;
	
	private String imPref;
	
	/*个性铃声存储路径。*/
	private String ring;
	
	/*组织名称。*/
	private String org;
	
	/*职位名称。*/
	private String title;
	
	/*个人说明。*/
	private String note;
	
	/*详细信息所属用户。*/
	private ContactUser contactUser;
	
	/*数据库手动修改记录。*/
	private String remark;

	/**
	 * @hibernate.id unsaved-value="null" type = "string" length="32"
	 * @hibernate.column name="id" sql-type="char(32)"
	 * @hibernate.generator class="uuid.hex"
	 */
	@Override
	public String getId() {
		return id;
	}
	
	/**
	 * @hibernate.version
	 * @hibernate.column name="version" type="long"
	 */
	@Override
	public long getVersion() {
		return this.version;
	}

	/**
	 * @hibernate.property column="nickName" length="64" type="string"
	 */
	public String getNickName() {
		return nickName;
	}

	public void setNickName(String nickName) {
		this.nickName = nickName;
	}

	/**
	 * @hibernate.property column="name" length="64" type="string"
	 */
	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	/**
	 * @hibernate.property column="age" type="integer"
	 */
	public Integer getAge() {
		return age;
	}

	public void setAge(Integer age) {
		this.age = age;
	}

	/**
	 * @hibernate.property column="photo" length="128" type="string"
	 */
	public String getPhoto() {
		return photo;
	}

	public void setPhoto(String photo) {
		this.photo = photo;
	}

	/**
	 * @hibernate.property column="identification" length="128" type="string"
	 */
	public String getIdentification() {
		return identification;
	}

	public void setIdentification(String identification) {
		this.identification = identification;
	}

	/**
	 * @hibernate.property type="timestamp"
	 * @hibernate.column name="brithday" sql-type="timestamp"
	 */
	public Date getBrithday() {
		return brithday;
	}

	public void setBrithday(Date brithday) {
		this.brithday = brithday;
	}

	/**
	 * @hibernate.property column="url" length="2000" type="string"
	 */
	public String getUrl() {
		return url;
	}

	public void setUrl(String url) {
		this.url = url;
	}

	/**
	 * @hibernate.property column="signature" length="128" type="string"
	 */
	public String getSignature() {
		return signature;
	}

	public void setSignature(String signature) {
		this.signature = signature;
	}

	/**
	 * @hibernate.property column="emailPref" length="256" type="string"
	 */
	public String getEmailPref() {
		return emailPref;
	}

	public void setEmailPref(String emailPref) {
		this.emailPref = emailPref;
	}

	/**
	 * @hibernate.property column="telPref" length="80" type="string"
	 */
	public String getTelPref() {
		return telPref;
	}

	public void setTelPref(String telPref) {
		this.telPref = telPref;
	}

	/**
	 * @hibernate.property column="imPref" length="256" type="string"
	 */
	public String getImPref() {
		return imPref;
	}

	public void setImPref(String imPref) {
		this.imPref = imPref;
	}

	/**
	 * @hibernate.property column="ring" length="128" type="string"
	 */
	public String getRing() {
		return ring;
	}

	public void setRing(String ring) {
		this.ring = ring;
	}

	/**
	 * @hibernate.property column="org" length="128" type="string"
	 */
	public String getOrg() {
		return org;
	}

	public void setOrg(String org) {
		this.org = org;
	}

	/**
	 * @hibernate.property column="title" length="128" type="string"
	 */
	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = title;
	}

	/**
	 * @hibernate.property column="note" length="1000" type="string"
	 */
	public String getNote() {
		return note;
	}

	public void setNote(String note) {
		this.note = note;
	}

	/**
	 * @hibernate.many-to-one name="contactUser" cascade="save-update" class="com.haina.beluga.contact.domain.ContactUser" unique="true"
	 * @hibernate.column name="userId" sql-type="char(32)" not-null="true"
	 * @return
	 */
	public ContactUser getContactUser() {
		return contactUser;
	}

	public void setContactUser(ContactUser contactUser) {
		this.contactUser = contactUser;
	}

	/**
	 * @hibernate.property column="remark" length="1000" type="string"
	 */
	public String getRemark() {
		return remark;
	}
	
	public void setRemark(String remark) {
		this.logger.warn("remark column is used for database log only,it can not be set.");
	}

	/**
	 * @hibernate.property type="com.haina.beluga.contact.domain.enumerate.SexEnumHibernateType"
	 * @hibernate.column name="sex" sql-type="smallint"
	 */
	public SexEnum getSex() {
		return sex;
	}

	public void setSex(SexEnum sex) {
		this.sex = sex;
	}

	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof UserProfile)) {
			return false;
		}
		UserProfile rhs = (UserProfile) object;
		return new EqualsBuilder().append(
				this.ring, rhs.ring).append(this.sex, rhs.sex).append(this.org, rhs.org).append(
				this.emailPref, rhs.emailPref).append(this.photo, rhs.photo)
				.append(this.url, rhs.url)
				.append(this.title, rhs.title).append(this.nickName,
						rhs.nickName).append(this.identification,
						rhs.identification).append(this.name, rhs.name).append(
						this.age, rhs.age).append(this.brithday, rhs.brithday)
				.append(this.telPref, rhs.telPref).append(this.note, rhs.note)
				.append(this.signature, rhs.signature).append(this.contactUser,
						rhs.contactUser).append(this.imPref, rhs.imPref)
				.isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(-844575309, -2140226785).append(this.ring).append(this.sex)
				.append(this.org).append(this.emailPref).append(
				this.photo).append(this.url).append(
				this.title).append(this.nickName).append(this.identification)
				.append(this.name).append(this.age).append(this.brithday)
				.append(this.telPref).append(this.note).append(this.signature)
				.append(this.contactUser).append(this.imPref).toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("note", this.note).append(
				"emailPref", this.emailPref)
				.append("age", this.age).append("nickName", this.nickName)
				.append("contactUser", this.contactUser)
				.append("org", this.org).append("ring", this.ring).append(
						"imPref", this.imPref).append("signature",
						this.signature).append("photo", this.photo)
				.append("brithday", this.brithday).append("url", this.url)
				.append("name", this.name).append("title", this.title).append(
						"telPref", this.telPref).append("sex", this.sex)
				.append("identification", this.identification).append("id",
						this.getId()).toString();
	}

	
}
