package com.sihus.core.model;



/**
 * 具有版本同步特性的持久化模型类�。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-05-20
 * 
 */
public abstract class VersionalModel extends BaseModel {
	
	private static final long serialVersionUID = 4298693759262776965L;
	
	protected long version=0;	
	
	/**
	 * @hibernate.version
	 * @hibernate.column name="version" type="long"
	 */
	public long getVersion() {
		return this.version;
	}
	
	public void setVersion(long version) {
//		this.logger.warn("version column is used for database lock only,it can not be set.");
	}

}
