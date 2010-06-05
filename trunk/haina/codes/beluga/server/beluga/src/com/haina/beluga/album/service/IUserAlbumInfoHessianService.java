package com.haina.beluga.album.service;

import com.haina.beluga.album.dao.IUserAlbumInfoDao;
import com.haina.beluga.album.domain.UserAlbumInfo;
import com.haina.beluga.webservice.data.AbstractRemoteReturning;
import com.sihus.core.service.IBaseSerivce;

/**
 * 用户相册远程调用方式业务接口。<br/>
 * @author huangyongqiang
 * @since 2010-05-23
 */
public interface IUserAlbumInfoHessianService extends IBaseSerivce<IUserAlbumInfoDao, UserAlbumInfo, String> {

	/**
	 * 获取用户相册
	 * @param email 用户的登录邮箱
	 * @param mobile 用户的手机号码
	 * @param curPage 当前页数
	 * @param pageSize 每页显示的数量
	 * @return
	 */
	AbstractRemoteReturning getUserAlbumInfos(String email,String mobile,int curPage,
			int pageSize);
	
	/**
	 * 编辑用户相册
	 * @param albumId 相册id
	 * @param albumName 相册名称
	 * @param description  相册的描述
	 * @param email 用户的登录邮箱
	 * @return
	 */
	AbstractRemoteReturning editUserAlbumInfo(String albumId, String albumName,	
			String description, String email);
	
	/**
	 * 删除相册，自持批量删除
	 * @param albumIds 相册的id
	 * @param email 相册所属用户登录邮箱
	 * @param deteleEmail 删除者的登录邮箱
	 * @return
	 */
	AbstractRemoteReturning deleteUserAlbumInfo(String[] albumIds, String email, String deteleEmail);
	
	/**
	 * 添加相册
	 * @param email 相册所属用户登录邮箱
	 * @param albumName 相册名称
	 * @param description 相册的描述
	 * @return
	 */
	AbstractRemoteReturning addUserAlbumInfo(String email, String albumName, String description);
}
