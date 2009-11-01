package com.haina.beluga.webservice;

import java.io.Serializable;

import com.haina.beluga.webservice.data.hessian.HessianRemoteReturning;
/**
 * 公共服务api.
 * @author Administrator
 * 客户端每次调用，都是返回HessianRemoteReturning对象。
 * 先判断状态是否成功(statusCode=0)，后去value值。
 */
public interface IPubService extends Serializable {
	/**
	 * 状态包括：
	 * 1.成功 statusCode=0
	 * 2.网络异常没有数据 statusCode = 10004
	 * 
	 * value值为：
	 * WeatherDto对象.
	 * 
	 */
	public HessianRemoteReturning getLiveWeather(String cityCode);
	/**
	 * 状态包括：
	 * 1.成功 statusCode=0
	 * 
	 * value值为：
	 * List集合，集合对象为WeatherDto.
	 * 
	 */
	public HessianRemoteReturning get7Weatherdatas(String cityCode);
	/**
	 * 状态包括：
	 * 1.成功 statusCode=0
	 * 2.QQ参数错误 statusCode = 10002
	 * 3.网络错误 statusCode= 10003 
	 * 
	 * value值为：
	 * 1.在线 value=10000
	 * 2.不在线 value=10001
	 * 
	 */
	public HessianRemoteReturning getQQStatus(String qqCode);
	/**
	 * 状态包括：
	 * 1.成功 statusCode=0
	 * 
	 * value值为：
	 * List集合，集合对象为PhoneDistrictDto.
	 * 
	 */
	// public HessianRemoteReturning getOrUpdatePD(String updateFlg);
	/**
	 * 状态包括：
	 * 1.成功 statusCode=0
	 * 
	 * value值为：
	 * List集合，集合对象为PhoneDistrictDto.
	 * 
	 */
	public HessianRemoteReturning getOrUpdatePD(int updateFlg, int begin,
			int count);
	/**
	 * 状态包括：
	 * 1.成功 statusCode=0
	 * 
	 * value值为：Long
	 * 
	 */
	public HessianRemoteReturning getOrUpdatePDCount(int updateFlg);
	
	public HessianRemoteReturning testCN(String cn);
	

}
