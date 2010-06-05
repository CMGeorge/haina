package com.haina.beluga.album.dao;

import java.util.Collection;

import com.haina.beluga.album.domain.UserAlbumInfo;
import com.haina.beluga.album.domain.UserAlbumInfoView;
import com.sihus.core.dao.IBaseDao;
import com.sihus.core.util.PagingData;

/**
 * 用户相册信息Dao访问接口。<br/>
 * @author huangyongqiang
 * @since 2010-05-22
 */
public interface IUserAlbumInfoDao extends IBaseDao<UserAlbumInfo, String> {

	/**
	 * 获取用户相册信息
	 * @param email 用户邮箱
	 * @param mobile 用户手机号码
	 * @param pagingData 分页数据
	 * @return
	 */
	Collection<UserAlbumInfoView> getUserAlbumInfos(String email,String mobile,
			PagingData pagingData);
	
	/**
	 * 通过id获取相册
	 * @param id 相册id
	 * @return
	 */
	UserAlbumInfo getUserAlbumInfoById(String id);
	
	/**
	 * 通过id删除单个相册
	 * @param id 相册id
	 * @param deleteUserId 删除用户的id
	 */
	int deleteUserAlbumInfoById(String id,String deleteUserId);
	
	/**
	 * 通过id批量删除相册
	 * @param ids 相册id
	 * @param deleteUserId 删除用户的id
	 */
	int deleteUserAlbumInfoByIds(String[] ids,String deleteUserId);
}
