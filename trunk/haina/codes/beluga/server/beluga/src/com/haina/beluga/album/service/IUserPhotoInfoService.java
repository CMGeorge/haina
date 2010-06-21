package com.haina.beluga.album.service;

import com.haina.beluga.album.dao.IUserPhotoInfoDao;
import com.haina.beluga.album.domain.UserPhotoInfo;
import com.haina.beluga.webservice.data.AbstractRemoteReturning;
import com.sihus.core.service.IBaseSerivce;

/**
 * 用户相册远程调用方式业务接口。<br/>
 * @author huangyongqiang
 * @since 2010-05-27
 */
public interface IUserPhotoInfoService extends IBaseSerivce<IUserPhotoInfoDao, UserPhotoInfo, String> {

	/**
	 * 获取用户相册的相片列表
	 * @param albumId 相册id
	 * @param email 相册所属用户登录邮箱
	 * @param curPage 当前页数
	 * @param pageSize 每页显示的数量
	 * @return
	 */
	AbstractRemoteReturning getUserPhotoInfoList(String albumId, String email, int curPage, int pageSize);
	
	/**
	 * 获取用户相片信息
	 * @param photoId 相片id
	 * @param albumId 相册id
	 * @param curPage 评论当前页数
	 * @param pageSize 评论每页显示的数量
	 * @return
	 */
	AbstractRemoteReturning getUserPhotoInfo(String photoId, String albumId, int curPage, int pageSize);
	
	/**
	 * 添加相片
	 * @param albumId 相片所属相册id
	 * @param email 相片所属用户登录邮箱
	 * @param createTime 添加时间
	 * @param photoName 相片名称
	 * @param photoDescription 相片描述
	 * @param mime 相片的MIME类型
	 * @param oriFileName 相片原始文件名称
	 * @param photoData 相片数据
	 * @return
	 */
	AbstractRemoteReturning addUserPhotoInfo(String albumId, String email,
			String createTime, String photoName,
			String photoDescription, String mime, String oriFileName, byte[] photoData);
	
	/**
	 * 删除用户相片
	 * @param photoIds 相片id
	 * @param albumId 相册id
	 * @param email 删除用户登录邮箱
	 * @return
	 */
	AbstractRemoteReturning deleteUserPhotoInfo(String[] photoIds, String albumId,String email);
	
	/**
	 * 添加用户相片的评论
	 * @param commentContent 评论内容
	 * @param commentTime 评论标题
	 * @param photoId 评论所属相片id
	 * @param email 添加评论用户的登录邮箱
	 * @return
	 */
	AbstractRemoteReturning addUserPhotoComment(String commentContent,String commentTime, String photoId,
			String email);
	
	/**
	 * 删除相片评论
	 * @param commentIds 评论id
	 * @param photoId 相片id
	 * @param email 删除用户的登录邮箱
	 * @return
	 */
	AbstractRemoteReturning deleteUserPhotoComment(String[] commentIds, String photoId, String email);
}
