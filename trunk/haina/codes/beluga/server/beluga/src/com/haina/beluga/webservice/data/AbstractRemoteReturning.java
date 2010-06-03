package com.haina.beluga.webservice.data;

import java.io.Serializable;

/**
 * 远程调用返回值的基类。<br/>
 * 
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-06-17
 */
public abstract class AbstractRemoteReturning extends AbstractRemoteData {

	/**
	 * 
	 */
	private static final long serialVersionUID = -3768217656560995261L;
	/* 返回值。 */
	protected Serializable value;

	public Serializable getValue() {
		return value;
	}

	public void setValue(Object value) {
		this.value = net.sf.json.JSONSerializer.toJSON(value);
	}
}
