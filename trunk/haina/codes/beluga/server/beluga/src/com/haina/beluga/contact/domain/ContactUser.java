package com.haina.beluga.contact.domain;

import java.util.Date;
import java.util.Set;

import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;
import org.springframework.stereotype.Component;

import com.sihus.core.model.VersionalModel;

/**
 * 联系人用户表的领域模型类。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-06-26
 * @hibernate.class table="ContactUser" optimistic-lock="version"
 * @hibernate.cache usage="read-write"
 */
public class ContactUser extends VersionalModel {

	private static final long serialVersionUID = 6223217962960596222L;
	
	/*用户在线状态。*/
	public static final Integer USER_STATUS_ONLINE=1;
	
	/*用户离线状态。*/
	public static final Integer USER_STATUS_OFFLINE=0;

	private String loginName;
	
	private String password;
	
	private Integer userStatus;
	
	private Date registerTime;
	
	private String mobile;
	
	private Date lastLoginTime;
	/*最后修改日期*/
	private Date lastUpdateTime;
	/*登录次数*/
	private Integer loginNumber;
	
	private String lastLoginIp;
	
	/*是否有效。*/
	private Boolean validFlag;
	
	private String description;
	
	/*用户的联系人标签。*/
	private Set<ContactTag> contactTags;
	
	/*详细信息。*/
	private UserProfile userProfile;
	
	/*详细扩展信息。*/
	private Set<UserProfileExt> userProfileExts;
	
	/*数据库手动修改记录。*/
	private String remark;
	
	/**
	 * @hibernate.id unsaved-value="null" type = "java.lang.String" length="32"
	 * @hibernate.column name="id" sql-type="char(32)"
	 * @hibernate.generator class="uuid.hex"
	 */
	@Override
	public String getId() {
		return id;
	}

	/**
	 * @hibernate.property column="loginName" length="64" not-null="true" type = "java.lang.String" unique = "true"
	 */
	public String getLoginName() {
		return loginName;
	}

	public void setLoginName(String loginName) {
		this.loginName = loginName;
	}

	/**
	 * @hibernate.property column="password" length="256" not-null="true" type = "string"
	 */
	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}
	
	/**
	 * @hibernate.property column="userStatus" not-null="true" type = "integer"
	 */
	public Integer getUserStatus() {
		return userStatus;
	}

	public void setUserStatus(Integer userStatus) {
		Integer status=userStatus;
		if(null==status || !status.equals(USER_STATUS_OFFLINE) 
				&& !status.equals(USER_STATUS_ONLINE)) {
			status=USER_STATUS_OFFLINE;
		}
		this.userStatus=status;
	}
	
	/**
	 * @hibernate.property not-null="true" type="timestamp"
	 * @hibernate.column name="registerTime" sql-type="timestamp"
	 */
	public Date getRegisterTime() {
		return registerTime;
	}

	public void setRegisterTime(Date registerTime) {
		this.registerTime = registerTime;
	}

	/**
	 * @hibernate.property column="mobile" length="64" not-null="true" type = "java.lang.String" unique = "true"
	 */
	public String getMobile() {
		return mobile;
	}

	public void setMobile(String mobile) {
		this.mobile = mobile;
	}

	/**
	 * @hibernate.property type="timestamp"
	 * @hibernate.column name="lastLoginTime" sql-type="timestamp"
	 */
	public Date getLastLoginTime() {
		return lastLoginTime;
	}

	public void setLastLoginTime(Date lastLoginTime) {
		this.lastLoginTime = lastLoginTime;
	}
	/**
	 * @hibernate.property type="timestamp"
	 * @hibernate.column name="lastUpdateTime" sql-type="timestamp"
	 */
	public Date getLastUpdateTime() {
		return lastUpdateTime;
	}

	public void setLastUpdateTime(Date lastUpdateTime) {
		this.lastUpdateTime = lastUpdateTime;
	}
	/**
	 * @hibernate.property
	 * @hibernate.column name="loginNumber" sql-type="int default 0"
	 * return Integer
	 */
	public Integer getLoginNumber() {
		return loginNumber;
	}

	public void setLoginNumber(Integer loginNumber) {
		this.loginNumber = loginNumber;
	}

	/**
	 * @hibernate.property column="lastLoginIp" length="64" type = "java.lang.String"
	 */
	public String getLastLoginIp() {
		return lastLoginIp;
	}

	public void setLastLoginIp(String lastLoginIp) {
		this.lastLoginIp = lastLoginIp;
	}

	/**
	 * @hibernate.property column="validFlag" type="java.lang.Boolean" not-null="true"
	 * @return
	 */
	public Boolean getValidFlag() {
		return validFlag;
	}

	public void setValidFlag(Boolean validFlag) {
		this.validFlag = validFlag;
	}
	
	/**
	 * @hibernate.property column="description" length="2000" type = "java.lang.String"
	 */
	public String getDescription() {
		return description;
	}

	public void setDescription(String description) {
		this.description = description;
	}
	
	/**
	 * @hibernate.version
	 * @hibernate.column name="version" type="long"
	 */
	@Override
	public long getVersion() {
		return version;
	}

	/**
	 * 用户的联系人标签。<br/>
	 * @hibernate.set name="contactTags" table="ContactTag" lazy="true" outer-join="false" inverse="false" cascade="all" order-by="tagOrder"
	 * @hibernate.key column="createUser"
	 * @hibernate.one-to-many class="com.haina.beluga.contact.domain.ContactTag"
	 * @return
	 */
	public Set<ContactTag> getContactTags() {
		return contactTags;
	}

	public void setContactTags(Set<ContactTag> contactTags) {
		this.contactTags = contactTags;
	}
	
	/**
	 * @hibernate.one-to-one name="userProfile" class="com.haina.beluga.contact.domain.UserProfile" property-ref="contactUser" cascade="all" constrained="false"
	 * @return
	 */
	public UserProfile getUserProfile() {
		return userProfile;
	}

	public void setUserProfile(UserProfile userProfile) {
		this.userProfile = userProfile;
	}
	
	/**
	 * @hibernate.set name="userProfileExts" table="UserProfileExt" lazy="true" outer-join="false" inverse="false" cascade="all" order-by="commKey"
	 * @hibernate.key column="userId"
	 * @hibernate.one-to-many class="com.haina.beluga.contact.domain.UserProfileExt"
	 * @return
	 */
	public Set<UserProfileExt> getUserProfileExts() {
		return userProfileExts;
	}

	public void setUserProfileExts(Set<UserProfileExt> userProfileExts) {
		this.userProfileExts = userProfileExts;
	}
	
	/**
	 * 数据库手动修改记录。
	 * @hibernate.property column="remark" length="2000" type = "java.lang.String"
	 */
	public String getRemark() {
		return remark;
	}
	
	public void setRemark(String remark) {
		this.logger.warn("remark column is used for database log only,it can not be set.");
	}

	/**
	 * 是否有效。<br/>
	 * @return
	 */
	public boolean isValid() {
		return (!this.isNew() && this.getValidFlag()!=null && this.getValidFlag());
	}
	
	/**
	 * 是否在线。<br/>
	 * @return
	 */
	public boolean isOnline() {
		return (!this.isNew() && this.getValidFlag()!=null && this.getValidFlag() 
				&& this.getUserStatus().equals(ContactUser.USER_STATUS_ONLINE));
	}
	
	/**
	 * 是否离线。<br/>
	 * @return
	 */
	public boolean isOffline() {
		return (!this.isNew() && this.getValidFlag()!=null && this.getValidFlag() 
				&& this.getUserStatus().equals(ContactUser.USER_STATUS_OFFLINE));
	}
	
	/**
	 * @see java.lang.Object#equals(Object)
	 */
	@Override
	public boolean equals(Object object) {
		if (!(object instanceof ContactUser)) {
			return false;
		}
		ContactUser rhs = (ContactUser) object;
		return new EqualsBuilder().append(
				this.lastLoginIp, rhs.lastLoginIp).append(this.lastLoginTime,
				rhs.lastLoginTime).append(this.registerTime, rhs.registerTime)
				.append(this.loginName, rhs.loginName).append(this.mobile, rhs.mobile)
				.append(this.id, rhs.id).append(this.password, rhs.password)
				.append(this.description, rhs.description)
				.append(this.userStatus, rhs.userStatus).isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	@Override
	public int hashCode() {
		return new HashCodeBuilder(1134122519, -1554226803).append(this.lastLoginIp).append(
				this.lastLoginTime).append(this.registerTime).append(
				this.loginName).append(this.mobile).append(this.id).append(this.password)
				.append(this.userStatus).append(this.description).toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	@Override
	public String toString() {
		return new ToStringBuilder(this)
				.append("logingName", this.loginName).append("mobile", this.mobile).append(
						"lastLoginIp", this.lastLoginIp).append(
						"lastLoginTime", this.lastLoginTime).append(
						"registerTime", this.registerTime).append("id",
						this.getId()).append("description",this.getDescription()).
						append("userStatus",this.getUserStatus()).
						append("password",this.getPassword()).toString();
	}
}
