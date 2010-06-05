package com.haina.beluga.webservice.pubController;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.ModelMap;
import org.springframework.web.bind.annotation.RequestMapping;

import com.haina.beluga.webservice.Constant;
import com.haina.beluga.webservice.IPriService;
import com.haina.beluga.webservice.data.Returning;
/**
 * 登录注册控制器
 * @author fuxiang
 *
 */
@Controller
@RequestMapping(value={"/login.do"})
public class LoginController {
	
	@Autowired(required = true)
	private IPriService priService;

	@RequestMapping(params = "method=login")
	public String login(String loginName, String password, ModelMap model) {
		Returning h = priService.login(loginName, password);
		model.addAttribute(h);
		return Constant.JSON_VIEW;
	}
	@RequestMapping(params = "method=register")
	public String register(String loginName, String password, String mobile,
			ModelMap model) {
		Returning h = priService.register(loginName, password, mobile);
		model.addAttribute(h);
		return Constant.JSON_VIEW;
	}



}
