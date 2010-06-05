package com.haina.beluga.album.dao;

import java.util.Collection;

import com.haina.beluga.album.domain.UserPhotoComment;
import com.haina.beluga.album.domain.UserPhotoCommentView;
import com.haina.beluga.album.domain.UserPhotoInfo;
import com.haina.beluga.album.domain.UserPhotoInfoView;
import com.haina.beluga.album.domain.enumerate.UserPhotoSizeEnum;
import com.sihus.core.dao.IBaseDao;
import com.sihus.core.util.PagingData;

/**
 * 用户相片信息Dao访问接口。<br/>
 * @author huangyongqiang
 * @since 2010-05-23
 */
public interface IUserPhotoInfoDao extends IBaseDao<UserPhotoInfo, String> {

	/**
	 * 取得相册内第一张相片。<br/>
	 * 按照创建的时间。
	 * @param userAlbumInfoId 相册id
	 * @return
	 */
	UserPhotoInfo getFirstPhotoOfUserAlbum(String userAlbumInfoId);
	
	/**
	 * 取得相册的封面相片
	 * @param userAlbumInfoId 相册id
	 * @return
	 */
	UserPhotoInfo getCoverPhotoOfUserAlbum(String userAlbumInfoId);
	
	/**
	 * 获取相册的相片列表
	 * @param userAlbumInfoId 相册id
	 * @param email 用户登录邮箱
	 * @param photoSize 相片大小类型
	 * @param pagingData 分页数据
	 * @return
	 */
	Collection<UserPhotoInfoView> getPhotosOfUserAlbum(String userAlbumInfoId, String email,
			UserPhotoSizeEnum photoSize, PagingData pagingData);
	
	/**
	 * 通过id取得用户相片
	 * @param id 相片id
	 * @return
	 */
	UserPhotoInfo getUserPhotoInfoById(String id);
	
	/**
	 * 通过相册id和相片id取得用户相册
	 * @param photoId 相片id
	 * @param albumId 相册id
	 * @return
	 */
	UserPhotoInfo getUserPhotoInfoById(String photoId,String albumId);
	
	/**
	 * 获取相片的评论列表
	 * @param photoId 相片id
	 * @param pagingData 分页数据
	 * @return
	 */
	Collection<UserPhotoCommentView> getUserPhotoComments(String photoId,PagingData pagingData);
	
	/**
	 * 通过id批量删除相片
	 * @param ids 相片id
	 * @param albumId 相册id
	 * @param deleteUserId 删除用户的id
	 */
	int deleteUserPhotoInfoByIds(String[] ids, String albumId, String deleteUserId);
	
	/**
	 * 添加用户评论
	 * @param userPhotoComment 评论对象
	 */
	boolean addUserPhotoComment(UserPhotoComment userPhotoComment);
	
	/**
	 * 删除相片评论
	 * @param ids 评论id
	 * @param photoId 相片id
	 * @param deleteUserId 删除用户的id
	 * @return
	 */
	int deleteUserPhotoComment(String[] ids, String photoId, String deleteUserId);
}
