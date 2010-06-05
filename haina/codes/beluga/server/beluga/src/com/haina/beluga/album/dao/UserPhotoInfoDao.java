package com.haina.beluga.album.dao;

import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import org.springframework.stereotype.Repository;

import com.haina.beluga.album.domain.UserPhotoComment;
import com.haina.beluga.album.domain.UserPhotoCommentView;
import com.haina.beluga.album.domain.UserPhotoInfo;
import com.haina.beluga.album.domain.UserPhotoInfoView;
import com.haina.beluga.album.domain.enumerate.UserPhotoSizeEnum;
import com.sihus.core.dao.BaseDao;
import com.sihus.core.util.PagingData;
import com.sihus.core.util.StringUtils;

/**
 * 用户相片Dao访问接口实现类。<br/>
 * @author huangyongqiang
 * @since 2010-05-22
 */
@Repository(value="userPhotoInfoDao")
public class UserPhotoInfoDao extends BaseDao<UserPhotoInfo, String> implements
		IUserPhotoInfoDao {

	@SuppressWarnings("unchecked")
	@Override
	public UserPhotoInfo getFirstPhotoOfUserAlbum(String userAlbumInfoId) {
		UserPhotoInfo ret=null;
		if(!StringUtils.isNull(userAlbumInfoId)) {
			String hql="select up from UserPhotoInfo up where up.userAlbumInfoId = : userAlbumInfoId order by up.createTime desc ";
			Map<String, Object> params=new HashMap<String, Object>();
			params.put("userAlbumInfoId", userAlbumInfoId.trim());
			PagingData page=new PagingData();
			page.setRowsPerPage(1);
			page.setCurrentPage(1);
			List<UserPhotoInfo> photos=(List<UserPhotoInfo>)getResultByHQLAndParamNoUpdate(hql, params, page);
			if(photos!=null && photos.size()>0) {
				ret=photos.get(0);
			}
		}
		return ret;
	}

	@SuppressWarnings("unchecked")
	@Override
	public UserPhotoInfo getCoverPhotoOfUserAlbum(String userAlbumInfoId) {
		UserPhotoInfo ret=null;
		if(!StringUtils.isNull(userAlbumInfoId)) {
			String hql="select up from UserPhotoInfo up where up.userAlbumInfoId = : userAlbumInfoId and up.coverFlag = true order by up.createTime desc ";
			Map<String, Object> params=new HashMap<String, Object>();
			params.put("userAlbumInfoId", userAlbumInfoId.trim());
			PagingData page=new PagingData();
			page.setRowsPerPage(1);
			page.setCurrentPage(1);
			List<UserPhotoInfo> photos=(List<UserPhotoInfo>)getResultByHQLAndParamNoUpdate(hql, params, page);
			if(photos!=null && photos.size()>0) {
				ret=photos.get(0);
			}
		}
		return ret;
	}

	@SuppressWarnings("unchecked")
	@Override
	public Collection<UserPhotoInfoView> getPhotosOfUserAlbum(
			String userAlbumInfoId, String email, UserPhotoSizeEnum photoSize,
			PagingData pagingData) {
		Collection<UserPhotoInfoView> ret=null;
		if(!StringUtils.isNull(userAlbumInfoId)) {
			StringBuilder hql=new StringBuilder("select new com.haina.beluga.album.domain.UserPhotoInfoView(upi.id, upi.photoName, upi.photoDescription, upi.createTime, upi.filePath) "
					+" from UserPhotoInfo upi, UserAlbumInfo uai ");
			Map<String,Object> params=new HashMap<String, Object>();
			boolean validEmail=false;
			if(!StringUtils.isNull(email)) {
				validEmail=true;
				hql.append(",  ContactUser cu ");
			}
			
			hql.append(" where uai.deleteFlag = false and upi.deleteFlag = false and uai.id = upi.userAlbumInfoId "
					+" and upi.userAlbumInfoId = :userAlbumInfoId ");
			params.put("userAlbumInfoId", userAlbumInfoId.trim());
			if(photoSize!=null) {
				hql.append("and upi.photoSize = :photoSize ");
				params.put("photoSize", photoSize);
			}
			params.put("userAlbumInfoId", userAlbumInfoId.trim());
			if(validEmail) {
				hql.append(" and cu.loginName = :loginName ");
				params.put("loginName", email.trim());
			}
			hql.append(" order by upi.createTime desc ");
			ret=(Collection<UserPhotoInfoView>)getResultByHQLAndParam(hql.toString(), params, pagingData);
		}
		return ret;
	}

	@Override
	public UserPhotoInfo getUserPhotoInfoById(String id) {
		return this.getUserPhotoInfoById(id, null);
	}

	@Override
	public UserPhotoInfo getUserPhotoInfoById(String photoId, String albumId) {
		UserPhotoInfo ret=null;
		ret=this.loadForUpdate(photoId);
		if(ret!=null && ret.getDeleteFlag()) {
			ret=null;
		}
		if(!StringUtils.isNull(albumId)) {
			if(!ret.getUserAlbumInfoId().equals(albumId.trim())) {
				ret=null;
			}
		}
		return ret;
	}

	@SuppressWarnings("unchecked")
	@Override
	public Collection<UserPhotoCommentView> getUserPhotoComments(String photoId,
			PagingData pagingData) {
		Collection<UserPhotoCommentView> ret=null;
		if(!StringUtils.isNull(photoId)) {
			String hql="select new com.haina.beluga.album.domain.UserPhotoCommentView(c.id, c.content,c.title, c.createTime, u.loginName, u.mobile) "
				+ " from UserPhotoComment c, ContactUser u, UserPhotoInfo p "
				+ " where u.id = c.createUserId and u.validFlag= true and c.userPhotoInfoId = p.id and p.deleteFlag = false and p.id = :userPhotoInfoId ";
			Map<String, Object> params=new HashMap<String, Object>();
			params.put("userPhotoInfoId", photoId);
			ret=(Collection<UserPhotoCommentView>)getResultByHQLAndParamNoUpdate(hql, params, pagingData);
		}
		return ret;
	}

	@Override
	public int deleteUserPhotoInfoByIds(String[] ids, String albumId,
			String deleteUserId) {
		int ret=0;
		if(null!=ids && ids.length>0 && !StringUtils.isNull(deleteUserId)) {
			Collection<String> idList=new ArrayList<String>();
			for(int i=0;i<ids.length;i++) {
				if(!StringUtils.isNull(ids[i])) {
					idList.add(ids[i].trim());
				}
			}
			if(idList.size()>0) {
				StringBuffer hql=new StringBuffer();
				Set<Object> params=new LinkedHashSet<Object>();
				
				/*$1 删除相片本身*/
				params.add(idList);
				params.add(deleteUserId);
				hql.append("update UserPhotoInfo upi set upi.deleteFlag = true where upi.deleteFlag = false and upi.id in( ? ) and upi.createUserId = ? ");
				
				if(StringUtils.isNull(albumId)) {
					hql.append(" and upi.userAlbumInfoId = ? ");
					params.add(albumId);
				}
				
				ret=this.getHibernateTemplate().bulkUpdate(hql.toString(),
						params.toArray(new Object[]{}));
				
				/*2 删除相片评论*/
				getHibernateTemplate().bulkUpdate("delete from UserPhotoComment where createUserId = ? and userPhotoInfoId in ( ? ) ",
						new Object[]{deleteUserId, idList});
				
				/*3 删除相片的图片，由于相片是逻辑删除，该功能暂不实现 */
				//TODO
			}
		}
		return ret;
	}

	@Override
	public boolean addUserPhotoComment(UserPhotoComment userPhotoComment) {
		if(null==userPhotoComment || !userPhotoComment.isNew()) {
			return false;
		}
		this.getHibernateTemplate().save(userPhotoComment);
		return true;
	}

	@Override
	public int deleteUserPhotoComment(String[] ids, String photoId,
			String deleteUserId) {
		int ret=0;
		if(ids!=null && ids.length>0
				&& !StringUtils.isNull(deleteUserId)) {
			Collection<String> idList=new HashSet<String>();
			for(int i=0;i<ids.length;i++) {
				if(!StringUtils.isNull(ids[i])) {
					idList.add(ids[i].trim());
				}
			}
			if(idList.size()>0) {
				Set<Object> params=new LinkedHashSet<Object>();
				StringBuilder hql=new StringBuilder("delete from UserPhotoComment where id in ( ? )  and createUserId = ? ");
				params.add(idList);
				params.add(deleteUserId);
				if(!StringUtils.isNull(photoId)) {
					hql.append(" and userPhotoInfoId = ? ");
					params.add(photoId);
				}
				ret=getHibernateTemplate().bulkUpdate(hql.toString(), params);
			}
		}
		return ret;
	}

}