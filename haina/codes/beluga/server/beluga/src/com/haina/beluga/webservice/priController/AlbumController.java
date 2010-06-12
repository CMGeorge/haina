package com.haina.beluga.webservice.priController;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.ModelMap;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

import com.haina.beluga.album.service.IUserAlbumInfoService;
import com.haina.beluga.album.service.IUserPhotoInfoService;
import com.haina.beluga.webservice.BaseController;
import com.haina.beluga.webservice.Constant;
import com.haina.beluga.webservice.data.AbstractRemoteReturning;

/**
 * 相册相关操作控制器。<br/>
 * @author huangyongqiang
 * @since 2010-06-12
 */
@Controller
@RequestMapping("/album.do")
public class AlbumController extends BaseController {

	@Autowired(required = true)
	private IUserAlbumInfoService userAlbumInfoService;
	
	@Autowired(required = true)
	private IUserPhotoInfoService userPhotoInfoService;
	
	@RequestMapping(params = "method=getUserAlbumInfoList")
	public String getUserAlbumInfoList(@RequestParam(required=true,value="passport") String passport, 
			@RequestParam(required=false,value="curPage") int curPage,
			@RequestParam(required=false,value="pageSize") int pageSize,
			ModelMap model) {
		AbstractRemoteReturning ret=null;
		String loginName = this.getLoginNameOfPassport(passport);
		ret=userAlbumInfoService.getUserAlbumInfos(loginName, null, curPage, pageSize);
		model.addAttribute(ret);
		return Constant.JSON_VIEW;
	}
	
}
