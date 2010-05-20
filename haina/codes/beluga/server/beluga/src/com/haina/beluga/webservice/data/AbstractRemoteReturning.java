package com.haina.beluga.webservice.data;

import com.haina.beluga.webservice.flexjson.JSONSerializer;

/**
 * 远程调用返回值的基类。<br/>
 * 
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-06-17
 */
public abstract class AbstractRemoteReturning extends AbstractRemoteData {

	/* 返回值。 */
	protected String value;

	public String getValue() {
		return value;
	}

	public void setValue(Object value) {
		this.value = new JSONSerializer().deepSerialize(value);
	}
}
