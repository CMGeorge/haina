package com.haina.beluga.core.model;



/**
 * 具有版本同步特性的持久化模型类�。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-05-20
 * 
 */
public abstract class VersionalModel extends BaseModel {

	protected long version=0;
	
	public abstract long getVersion();
	

	private void setVersion(long version) {
		this.version = version;
	}

}
