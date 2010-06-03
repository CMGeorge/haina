package com.haina.beluga.webservice.data;

import java.io.Serializable;

import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;


/**
 * 基于Hessian协议的远程调用返回值类。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-06-17
 */
public class Returning extends AbstractRemoteReturning {

	private static final long serialVersionUID = 442552256316220328L;
	
	/*HTTP协议状态码。*/
//	protected Integer httpStatusCode;

	/*Hessian协议自身的状态码*/
//	protected Integer hessianStatusCode;
	
	
	public Returning(Integer statusCode,
			Serializable value) {
		super();
		this.statusCode=statusCode;
//		this.operationCode=operationCode;
		this.setValue(value);
//		this.httpStatusCode = httpStatusCode;
//		this.hessianStatusCode = hessianStatusCode;
	}
	
	public Returning(Integer statusCode,String statusText,
			Serializable value) {
		super();
		this.statusCode=statusCode;
		this.statusText=statusText;
//		this.operationCode=operationCode;
		this.setValue(value);
//		this.httpStatusCode = httpStatusCode;
//		this.hessianStatusCode = hessianStatusCode;
	}
	
	public Returning(Serializable value) {
		super();
		this.setValue(value);
//		this.httpStatusCode = httpStatusCode;
//		this.hessianStatusCode = hessianStatusCode;
	}
	
	
//	public HessianRemoteReturning(Integer httpStatusCode,
//			Integer hessianStatusCode) {
//		super();
////		this.httpStatusCode = httpStatusCode;
////		this.hessianStatusCode = hessianStatusCode;
//	}

	public Returning(Object value) {
		super();
		this.setValue(value);
	}
	public Returning() {
		super();
//		this.value=value;
	}
//	public Integer getHttpStatusCode() {
//		return httpStatusCode;
//	}

//	public void setHttpStatusCode(Integer httpStatusCode) {
//		this.httpStatusCode = httpStatusCode;
//	}
//
//	public Integer getHessianStatusCode() {
//		return hessianStatusCode;
//	}
//
//	public void setHessianStatusCode(Integer hessianStatusCode) {
//		this.hessianStatusCode = hessianStatusCode;
//	}

	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof Returning)) {
			return false;
		}
		Returning rhs = (Returning) object;
		return new EqualsBuilder().appendSuper(super.equals(object)).append(
				this.value, rhs.value)/*.append(this.hessianStatusCode,
				rhs.hessianStatusCode).append(this.httpStatusCode, rhs.httpStatusCode)*/
				.isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(478403855, -827019439).appendSuper(
				super.hashCode()).append(this.value)/*.append(
				this.hessianStatusCode).append(
				this.httpStatusCode)*/.toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("value", this.value)
			/*.append("statusText", this.getStatusText())
			.append("operationCode",this.getOperationCode())*/
			.append("statusCode",this.getStatusCode()).toString();
	}
	
	

}
