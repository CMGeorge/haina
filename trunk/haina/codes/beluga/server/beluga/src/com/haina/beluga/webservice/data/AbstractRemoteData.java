package com.haina.beluga.webservice.data;

import com.haina.beluga.webservice.IStatusCode;
import com.haina.beluga.webservice.flexjson.JSON;
import com.sihus.core.dto.IDto;


/**
 * 远程调用传递数据的基类。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-06-17
 */
public abstract class AbstractRemoteData implements IDto {
	
	/*状态码。*/
	protected int statusCode;
	
	/*状态文字。*/
	protected String statusText;
	
	/*操作码。*/
	protected int operationCode;
	
	public Integer getStatusCode() {
		return statusCode;
	}

	public void setStatusCode(Integer statusCode) {
		this.statusCode = statusCode;
	}

	public Integer getOperationCode() {
		return operationCode;
	}

	public void setOperationCode(Integer operationCode) {
		this.operationCode = operationCode;
	}

	public String getStatusText() {
		return statusText;
	}

	public void setStatusText(String statusText) {
		this.statusText = statusText;
	}
	
	/**
	 * 是否是成功状态。<br/>
	 * @return
	 */
	@JSON(include=false)
	public boolean isSuccessStatus() {
		return this.statusCode==IStatusCode.SUCCESS;
	}
}
