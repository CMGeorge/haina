package com.haina.beluga.contact.dto;

import java.util.Date;

import com.haina.beluga.contact.domain.enumerate.SexEnum;
import com.sihus.core.dto.IDto;

/**
 * 联系人信息传输类。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-07-18
 */
public class ContactUserDto implements IDto {

	private static final long serialVersionUID = 4195102353591092377L;
	
	private String loginName;
	
	private String password;
	
	private String registerTime;
	
	private String mobile;
	
	private String lastLoginTime;
	
	/*最后修改日期*/
	private String lastUpdateTime;
	
	private String lastLoginIp;
	
	private String description;
	
	private String nickName;
	
	private String name;
	
	private Integer age;
	
	private SexEnum sex;
	
	/*头像文件。*/
	private byte[] photo;
	
	private String identification;
	
	private Date brithday;
	
	/*个人主页或博客。*/
	private String url;
	
	private String signature;
	
	/*首选email，默认是登录名。*/
	private String emailPref;
	
	private String telPref;
	
	private String imPref;
	
	/*个性铃声文件。*/
	private byte[] ring;
	
	/*组织名称。*/
	private String org;
	
	/*职位名称。*/
	private String title;
	
	/*个人说明。*/
	private String note;
	
	public ContactUserDto(String loginName, String password,
			String mobile, String lastLoginTime,
			String lastLoginIp, String description) {
		super();
		this.loginName = loginName;
		this.password = password;
		this.mobile = mobile;
		this.lastLoginTime = lastLoginTime;
		this.lastLoginIp = lastLoginIp;
		this.description = description;
	}

	public String getLoginName() {
		return loginName;
	}

	public void setLoginName(String loginName) {
		this.loginName = loginName;
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

	public String getRegisterTime() {
		return registerTime;
	}

	public void setRegisterTime(String registerTime) {
		this.registerTime = registerTime;
	}

	public String getMobile() {
		return mobile;
	}

	public void setMobile(String mobile) {
		this.mobile = mobile;
	}

	public String getLastLoginTime() {
		return lastLoginTime;
	}

	public void setLastLoginTime(String lastLoginTime) {
		this.lastLoginTime = lastLoginTime;
	}

	public String getLastUpdateTime() {
		return lastUpdateTime;
	}

	public void setLastUpdateTime(String lastUpdateTime) {
		this.lastUpdateTime = lastUpdateTime;
	}

	public String getLastLoginIp() {
		return lastLoginIp;
	}

	public void setLastLoginIp(String lastLoginIp) {
		this.lastLoginIp = lastLoginIp;
	}

	public String getDescription() {
		return description;
	}

	public void setDescription(String description) {
		this.description = description;
	}

	public String getNickName() {
		return nickName;
	}

	public void setNickName(String nickName) {
		this.nickName = nickName;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public Integer getAge() {
		return age;
	}

	public void setAge(Integer age) {
		this.age = age;
	}

	public SexEnum getSex() {
		return sex;
	}

	public void setSex(SexEnum sex) {
		this.sex = sex;
	}

	public byte[] getPhoto() {
		return photo;
	}

	public void setPhoto(byte[] photo) {
		this.photo = photo;
	}

	public String getIdentification() {
		return identification;
	}

	public void setIdentification(String identification) {
		this.identification = identification;
	}

	public Date getBrithday() {
		return brithday;
	}

	public void setBrithday(Date brithday) {
		this.brithday = brithday;
	}

	public String getUrl() {
		return url;
	}

	public void setUrl(String url) {
		this.url = url;
	}

	public String getSignature() {
		return signature;
	}

	public void setSignature(String signature) {
		this.signature = signature;
	}

	public String getEmailPref() {
		return emailPref;
	}

	public void setEmailPref(String emailPref) {
		this.emailPref = emailPref;
	}

	public String getTelPref() {
		return telPref;
	}

	public void setTelPref(String telPref) {
		this.telPref = telPref;
	}

	public String getImPref() {
		return imPref;
	}

	public void setImPref(String imPref) {
		this.imPref = imPref;
	}

	public byte[] getRing() {
		return ring;
	}

	public void setRing(byte[] ring) {
		this.ring = ring;
	}

	public String getOrg() {
		return org;
	}

	public void setOrg(String org) {
		this.org = org;
	}

	public String getTitle() {
		return title;
	}

	public void setTitle(String title) {
		this.title = title;
	}

	public String getNote() {
		return note;
	}

	public void setNote(String note) {
		this.note = note;
	}

}
