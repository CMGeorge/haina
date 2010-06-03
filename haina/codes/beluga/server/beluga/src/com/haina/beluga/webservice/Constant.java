package com.haina.beluga.webservice;
/**
 * 基本常量以正整数开始.
 * 调用成功的状态码均为0.
 * @author Administrator
 *
 */
public class Constant {
	
	/*QQ不在线返回的图像宽度*/
	public static final int QQ_OFFLINE_WIDTH = 16;
	/*QQ在线*/
	public static final int QQ_ONLINE = 10000;
	/*QQ不在线*/
	public static final int QQ_OFFLINE = 10001;
	/*QQ参数错误*/
	public static final int QQ_ARG_ERROE = 10002;
	/*网络错误*/
	public static final int QQ_NETWORK_ERROR = 10003;
	/*无法获得Live天气数据*/
	public static final int NO_LIVEDATA_REMOTEERR = 10004;
	
	
	/***
	 * 
	 */
	public static final String Monday = "Monday";
	public static final String Tuesday = "Tuesday";
	public static final String Wednesday = "Wednesday";
	public static final String Thursday = "Thursday";
	public static final String Friday = "Friday";
	public static final String Saturday = "Saturday";
	public static final String Sunday = "Sunday";
	/***
	 * Json跳转View
	 */
	public static final String JSON_VIEW = "jsonView";
	
}
