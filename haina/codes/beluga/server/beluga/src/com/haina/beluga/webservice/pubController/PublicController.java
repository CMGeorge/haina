package com.haina.beluga.webservice.pubController;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.ModelMap;
import org.springframework.web.bind.annotation.RequestMapping;

import com.haina.beluga.contact.service.IIMService;
import com.haina.beluga.contact.service.IPhoneDistrictService;
import com.haina.beluga.contact.service.IWeatherService;
import com.haina.beluga.webservice.Constant;
import com.haina.beluga.webservice.data.Returning;

@Controller
@RequestMapping(value={"/pub.do"})
public class PublicController {

	
	@Autowired(required = true)
	private IIMService iMservice;
	@Autowired(required = true)
	private IWeatherService weatherService;
	@Autowired(required = true)
	private IPhoneDistrictService phoneDistrictService;
	
	/**
	 * 状态包括：
	 * 1.成功 statusCode=0
	 * 2.网络异常没有数据 statusCode = 10004
	 * 
	 * value值为：
	 * WeatherDto对象.
	 * 
	 */
	@RequestMapping(params = "method=getLiveWeather")
	public String getLiveWeather(String cityCode, ModelMap model){
		Returning r =  weatherService.get7Weatherdatas(cityCode);
		model.addAttribute(r);
		return Constant.JSON_VIEW;
	}
	/**
	 * 状态包括：
	 * 1.成功 statusCode=0
	 * 
	 * value值为：
	 * List集合，集合对象为WeatherDto.
	 * 
	 */
	@RequestMapping(params = "method=get7Weatherdatas")
	public String get7Weatherdatas(String cityCode, ModelMap model){
		Returning r = weatherService.getLiveWeather(cityCode);
		model.addAttribute(r);
		return Constant.JSON_VIEW;
	}
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
	@RequestMapping(params = "method=getQQStatus")
	public String getQQStatus(String qqCode, ModelMap model){
		Returning r =  iMservice.getQQStatus(qqCode);
		model.addAttribute(r);
		return Constant.JSON_VIEW;
	}
	/**
	 * 状态包括：
	 * 1.成功 statusCode=0
	 * 
	 * value值为：
	 * List集合，集合对象为PhoneDistrictDto.
	 * 
	 */
	@RequestMapping(params = "method=getOrUpdatePD")
	public String getOrUpdatePD(int updateFlg, int begin,
			int count, ModelMap model){
		Returning r = phoneDistrictService.getOrUpdatePhoneDistricts(updateFlg, begin,
				count);
		model.addAttribute(r);
		return Constant.JSON_VIEW;
	}
}
