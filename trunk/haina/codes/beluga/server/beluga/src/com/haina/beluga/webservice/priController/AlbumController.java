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
		ret=userAlbumInfoService.getUserAlbumInfoList(loginName, null, curPage, pageSize);
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
	
	/**
	 * 删除用户相册
	 * @param albumIds 相册id
	 * @param deteleEmail 删除用户的email
	 * @param passport 登录令牌
	 * @param model
	 * @return
	 */
	@RequestMapping(params = "method=deleteUserAlbumInfo")
	public String deleteUserAlbumInfo(@RequestParam(required=true,value="albumIds") String[] albumIds,	
			@RequestParam(required=false,value="deteleEmail") String deteleEmail, 
			@RequestParam(required=true,value="passport") String passport, ModelMap model) {
		AbstractRemoteReturning ret=null;
		String loginName = this.getLoginNameOfPassport(passport);
		ret=userAlbumInfoService.deleteUserAlbumInfo(albumIds, loginName, deteleEmail);
		model.addAttribute(ret);
		return Constant.JSON_VIEW;
	}
	
	/**
	 * 添加用户相册
	 * @param albumName 相册名称
	 * @param description 相册描述
	 * @param passport 登录令牌
	 * @param model
	 * @return
	 */
	@RequestMapping(params = "method=addUserAlbumInfo")
	public String addUserAlbumInfo(@RequestParam(required=true,value="albumName") String albumName,
			@RequestParam(required=true,value="description") String description,
			@RequestParam(required=true,value="passport") String passport, ModelMap model) {
		AbstractRemoteReturning ret=null;
		String loginName = this.getLoginNameOfPassport(passport);
		ret=userAlbumInfoService.addUserAlbumInfo(loginName, albumName, description);
		model.addAttribute(ret);
		return Constant.JSON_VIEW;
	}
	
	/**
	 * 取得用户相册的相片列表
	 * @param curPage 当前页
	 * @param pageSize 每页显示记录数
	 * @param albumId 相册id
	 * @param passport 登录令牌
	 * @param model
	 * @return
	 */
	@RequestMapping(params = "method=getUserPhotoInfoList")
	public String getUserPhotoInfoList(@RequestParam(required=false,value="curPage") int curPage,
			@RequestParam(required=false,value="pageSize") int pageSize,
			@RequestParam(required=true,value="albumId") String albumId,
			@RequestParam(required=true,value="passport") String passport, ModelMap model) {
		AbstractRemoteReturning ret=null;
		String loginName = this.getLoginNameOfPassport(passport);
		ret=userPhotoInfoService.getUserPhotoInfoList(albumId, loginName, curPage, pageSize);
		model.addAttribute(ret);
		return Constant.JSON_VIEW;
	}
	
	/**
	 * 获取用户相片信息
	 * @param curPage 相片评论的当前页
	 * @param pageSize 相片评论的每页显示记录数
	 * @param albumId 相册id
	 * @param photoId 相片id
	 * @param model
	 * @return
	 */
	@RequestMapping(params = "method=getUserPhotoInfo")
	public String getUserPhotoInfo(@RequestParam(required=false,value="curPage") int curPage,
			@RequestParam(required=false,value="pageSize") int pageSize,
			@RequestParam(required=false,value="albumId") String albumId,
			@RequestParam(required=true,value="photoId") String photoId,ModelMap model) {
		AbstractRemoteReturning ret=null;
		ret=userPhotoInfoService.getUserPhotoInfo(photoId, albumId, curPage, pageSize);
		model.addAttribute(ret);
		return Constant.JSON_VIEW;
	}
	
	/**
	 * 添加用户相片
	 * @param photoDescription 相片描述
	 * @param photoName 相片名称
	 * @param albumId 相片id
	 * @param mime 相片文件的MIME类型
	 * @param oriFileName 相片文件的原始文件名
	 * @param photoData 相片文件的数据
	 * @param passport 登录令牌
	 * @param model
	 * @return
	 */
	@RequestMapping(params = "method=addUserPhotoInfo")
	public String addUserPhotoInfo(@RequestParam(required=false,value="photoDescription") String photoDescription,
			@RequestParam(required=true,value="photoName") String photoName,
			@RequestParam(required=true,value="albumId") String albumId,
			@RequestParam(required=true,value="mime") String mime,
			@RequestParam(required=true,value="oriFileName") String oriFileName,
			@RequestParam(required=true,value="photoData") byte[] photoData,
			@RequestParam(required=true,value="passport") String passport, ModelMap model) {
		AbstractRemoteReturning ret=null;
		String loginName = this.getLoginNameOfPassport(passport);
		ret=userPhotoInfoService.addUserPhotoInfo(albumId, loginName, null, 
				photoName, photoDescription, mime, oriFileName, photoData);
		model.addAttribute(ret);
		return Constant.JSON_VIEW;
	}
	
	/**
	 * 删除用户相片
	 * @param photoIds 相片id
	 * @param albumId 相册id
	 * @param passport 登录令牌
	 * @param model
	 * @return
	 */
	@RequestMapping(params = "method=deleteUserPhotoInfo")
	public String deleteUserPhotoInfo(@RequestParam(required=true,value="photoIds") String[] photoIds,
			@RequestParam(required=false,value="albumId") String albumId,
			@RequestParam(required=true,value="passport") String passport, ModelMap model) {
		AbstractRemoteReturning ret=null;
		String loginName = this.getLoginNameOfPassport(passport);
		ret=userPhotoInfoService.deleteUserPhotoInfo(photoIds, albumId, loginName);
		model.addAttribute(ret);
		return Constant.JSON_VIEW;
	}
	
	/**
	 * 添加用户相片评论
	 * @param photoId 相片id
	 * @param commentContent 评论内容
	 * @param commentTime 评论时间
	 * @param passport 登录令牌
	 * @param model
	 * @return
	 */
	@RequestMapping(params = "method=addUserPhotoComment")
	public String addUserPhotoComment(@RequestParam(required=true,value="photoId") String photoId,
			@RequestParam(required=true,value="commentContent") String commentContent,
			@RequestParam(required=true,value="commentTime") String commentTime,
			@RequestParam(required=true,value="passport") String passport, ModelMap model) {
		AbstractRemoteReturning ret=null;
		String loginName = this.getLoginNameOfPassport(passport);
		ret=userPhotoInfoService.addUserPhotoComment(commentContent, null, photoId, loginName);
		model.addAttribute(ret);
		return Constant.JSON_VIEW;
	}
	
	/**
	 * 删除用户相片评论
	 * @param photoId 相片id
	 * @param commentIds 评论id
	 * @param passport 登录令牌
	 * @param model
	 * @return
	 */
	@RequestMapping(params = "method=deleteUserPhotoComment")
	public String deleteUserPhotoComment(@RequestParam(required=false,value="photoId") String photoId,
			@RequestParam(required=true,value="commentIds") String[] commentIds,
			@RequestParam(required=true,value="passport") String passport, ModelMap model) {
		AbstractRemoteReturning ret=null;
		String loginName = this.getLoginNameOfPassport(passport);
		ret=userPhotoInfoService.deleteUserPhotoComment(commentIds, photoId, loginName);
		model.addAttribute(ret);
		return Constant.JSON_VIEW;
	}
}
