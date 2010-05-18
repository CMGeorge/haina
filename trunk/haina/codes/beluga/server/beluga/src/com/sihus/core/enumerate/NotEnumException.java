package com.sihus.core.enumerate;

/**
 * 枚举类型异常。 <br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-05-20
 */
public class NotEnumException extends RuntimeException {

	private static final long serialVersionUID = -2691464091448202175L;
	
	public NotEnumException() {
		
	}
	
	public NotEnumException(String msg) {
		super(msg);
	}

}
