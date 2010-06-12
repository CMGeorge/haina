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
	
	/**
	 * 取得用户注册列表
	 * @param passport 登录令牌
	 * @param curPage 当前页
	 * @param pageSize 每页显示记录数
	 * @param model
	 * @return
	 */
	@RequestMapping(params = "method=getUserAlbumInfoList")
	public String getUserAlbumInfoList(@RequestParam(required=false,value="curPage") int curPage,
			@RequestParam(required=false,value="pageSize") int pageSize,
			@RequestParam(required=true,value="passport") String passport, ModelMap model) {
		AbstractRemoteReturning ret=null;
		String loginName = this.getLoginNameOfPassport(passport);
		ret=userAlbumInfoService.getUserAlbumInfos(loginName, null, curPage, pageSize);
		model.addAttribute(ret);
		return Constant.JSON_VIEW;
	}
	
	/**
	 * 编辑用户相册
	 * @param albumId 相册id
	 * @param albumName 相册名称
	 * @param description 相册描述
	 * @param passport 登录令牌
	 * @param model
	 * @return
	 */
	@RequestMapping(params = "method=getUserAlbumInfoList")
	public String editUserAlbumInfo(@RequestParam(required=true,value="albumId") String albumId,
			@RequestParam(required=true,value="albumName") String albumName,	
			@RequestParam(required=false,value="description") String description, 
			@RequestParam(required=true,value="passport") String passport, ModelMap model) {
		AbstractRemoteReturning ret=null;
		String loginName = this.getLoginNameOfPassport(passport);
		ret=userAlbumInfoService.editUserAlbumInfo(albumId, albumName, description, loginName);
		model.addAttribute(ret);
		return Constant.JSON_VIEW;
	}
}
