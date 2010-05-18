package com.haina.beluga.log.domain;

import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;

import com.sihus.core.model.VersionalModel;


/**
 * 只取角色名称不关联角色表，日志实时性考虑。
*
 * @hibernate.class table="LOG"
 * @hibernate.cache usage="read-write"
 */
public class Log extends VersionalModel{

	 // Fields    
	private static final long serialVersionUID = 712763106703674149L;
	
   // private Account account;
    private String ip;
    private String roleName;
    private String logTime;
    private String handle;
    private String infoClass;
    private String remark;

    
	 /**
	 * @hibernate.property column="HANDLE" length="20"
	 * @return String
	 */
	public String getHandle() {
		return handle;
	}
	
	public void setHandle(String handle) {
		this.handle = handle;
	}
	 /**
	 * @hibernate.property column="INFOCLASS" length="50"
	 * @return String
	 */
	public String getInfoClass() {
		return infoClass;
	}
	
	public void setInfoClass(String infoClass) {
		this.infoClass = infoClass;
	}
	 /**
	 * @hibernate.property column="IP" length="20"
	 * @return String
	 */
	public String getIp() {
		return ip;
	}
	
	public void setIp(String ip) {
		this.ip = ip;
	}
	 /**
	 * @hibernate.property column="LOGTIME" length="20"
	 * @return String
	 */
	public String getLogTime() {
		return logTime;
	}
	
	public void setLogTime(String logTime) {
		this.logTime = logTime;
	}
	 /**
	 * @hibernate.property column="REMARK" length="200"
	 * @return String
	 */
	public String getRemark() {
		return remark;
	}
	
	public void setRemark(String remark) {
		this.remark = remark;
	}
	/**
	 * @hibernate.property column="ROLENAME" length="20"
	 * @return String
	 */
	public String getRoleName() {
		return roleName;
	}
	
	public void setRoleName(String roleName) {
		this.roleName = roleName;
	}
	/**
	 * @hibernate.version column="VERSION"
	 * @return long
	 */
	public long getVersion() {
		return version;
	}
	
	@Override
	public String toString() {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	protected Object clone() throws CloneNotSupportedException {
		// TODO Auto-generated method stub
		return super.clone();
	}
	 /**
	 * @hibernate.id column="ID" generator-class="uuid"  unsaved-value="null"
	 * @return String
	 */
	public String getId() {
		// TODO Auto-generated method stub
		return this.id;
	}

	/**
	 * @see java.lang.Object#equals(Object)
	 */
	@Override
	public boolean equals(Object object) {
		if (!(object instanceof Log)) {
			return false;
		}
		Log rhs = (Log) object;
		return new EqualsBuilder().append(this.id, rhs.id).append(
				this.handle, rhs.handle).append(this.roleName, rhs.roleName)
				.append(this.logTime, rhs.logTime).append(this.infoClass,
						rhs.infoClass).append(this.remark, rhs.remark).append(
						this.ip, rhs.ip).isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	@Override
	public int hashCode() {
		return new HashCodeBuilder(-1712230407, 1374111269).append(
				this.id).append(this.handle).append(this.roleName)
				.append(this.logTime).append(this.infoClass)
				.append(this.remark).append(this.ip)
				.toHashCode();
	}
	
	
}
