package com.haina.beluga.webservice.data;

import java.io.Serializable;


/**
 * 远程调用参数的基类。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-06-17
 */
public abstract class AbstractRemoteParameter extends AbstractRemoteData {

	/*参数名称。*/
	protected String name;
	
	/*参数值*/
	protected Serializable value;

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public Serializable getValue() {
		return value;
	}

	public void setValue(Serializable value) {
		this.value = value;
	}
}
