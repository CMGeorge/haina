package com.haina.beluga.album.dao;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.hibernate.HibernateException;
import org.hibernate.Query;
import org.hibernate.Session;
import org.springframework.orm.hibernate3.HibernateCallback;
import org.springframework.stereotype.Repository;

import com.haina.beluga.album.domain.UserAlbumInfo;
import com.haina.beluga.album.domain.UserAlbumInfoView;
import com.sihus.core.dao.BaseDao;
import com.sihus.core.util.PagingData;
import com.sihus.core.util.StringUtils;

/**
 * 用户相册Dao访问接口实现类。<br/>
 * @author huangyongqiang
 * @since 2010-05-22
 */
@Repository(value="userAlbumInfoDao")
public class UserAlbumInfoDao extends BaseDao<UserAlbumInfo, String> implements
		IUserAlbumInfoDao {

	@Override
	public int deleteUserAlbumInfoById(String id,String deleteUserId) {
		int ret=0;
		if(!StringUtils.isNull(id) && !StringUtils.isNull(deleteUserId)) {
			ret=this.deleteUserAlbumInfoByIds(new String[]{id}, deleteUserId);
		}
		return ret;
	}
	
	@SuppressWarnings("unchecked")
	@Override
	public int deleteUserAlbumInfoByIds(String[] ids, final String deleteUserId) {
		int ret=0;
		if(null!=ids && ids.length>0 && !StringUtils.isNull(deleteUserId)) {
			final List<String> idList=new ArrayList<String>();
			for(int i=0;i<ids.length;i++) {
				if(!StringUtils.isNull(ids[i])) {
					idList.add(ids[i]);
				}
			}
			if(idList.size()>0) {
				/*$1 删除相册本身*/
				ret=(Integer)this.getHibernateTemplate().execute(new HibernateCallback() {
					@Override
					public Object doInHibernate(Session session)
							throws HibernateException, SQLException {
						Query query=session.createQuery(
								"update UserAlbumInfo set deleteFlag = true where deleteFlag = false and createUserId = :userId and id in ( :ids )");
						query.setParameter("userId", deleteUserId);
						query.setParameter("ids", idList);
						int result=query.executeUpdate();
						return result;
					}
					
				});
				
//				ret=this.getHibernateTemplate().bulkUpdate(
//						"update UserAlbumInfo set deleteFlag = true where deleteFlag = false and createUserId = ? and id in ( ? ) ",
//						new Object[]{deleteUserId, idList});
				
				/*$2 删除相册的相片*/
				Map<String,Object> params=new HashMap<String, Object>();
				params.put("createUserId", deleteUserId);
				params.put("userAlbumInfoIds", idList);
				/*搜索相册里可以删除的相片id*/
				List<String> photoIdList=(List<String>)getResultByHQLAndParam("select id from UserPhotoInfo where upi.deleteFlag = false and upi.createUserId = :createUserId and userAlbumInfoId in ( :userAlbumInfoIds )", 
						params,null);
				if(photoIdList!=null && photoIdList.size()>0) {
					getHibernateTemplate().bulkUpdate("update UserPhotoInfo set deleteFlag = true where deleteFlag = false and id in ( ? ) ",
							new Object[]{deleteUserId, photoIdList});
					
					/*3 删除相片评论*/
					getHibernateTemplate().bulkUpdate("delete from UserPhotoComment where createUserId = ? and userPhotoInfoId in ( ? ) ",
							new Object[]{deleteUserId, photoIdList});
				}
			}
		}
		return ret;
	}
	
	@SuppressWarnings("unchecked")
	@Override
	public Collection<UserAlbumInfoView> getUserAlbumInfos(String email,
			String mobile, PagingData pagingData) {
		Collection<UserAlbumInfoView> ret=null;
		StringBuilder hql=new StringBuilder("select new com.haina.beluga.album.domain.UserAlbumInfoView(uai.id, uai.name, uai.createTime, count(upi.id), upi2.filePath) "
				+ " from UserAlbumInfo uai, UserPhotoInfo upi, UserPhotoInfo upi2 ");
		Map<String,Object> params=new HashMap<String, Object>();
		
		boolean validEmail=false;
		boolean validMobile=false;
		if(!StringUtils.isNull(email)) {
			validEmail=true;
		}
		if(!StringUtils.isNull(mobile)) {
			validMobile=true;
		}
		if(validEmail || validMobile) {
			hql.append(",  ContactUser cu ");
		}
		
		hql.append(" where uai.deleteFlag = false and upi.deleteFlag = false and upi2.deleteFlag = false ")
		.append(" and upi.userAlbumInfoId = uai.id and upi2.coverFlag = true ");
		if(validEmail || validMobile) {
			hql.append(" and uai.createUserId = cu.id ");
		}
		if(validEmail) {
			hql.append(" and cu.loginName = :loginName ");
			params.put("loginName", email.trim());
		}
		if(validMobile) {
			hql.append(" and cu.mobile = :mobile ");
			params.put("mobile", mobile.trim());
		}		
		hql.append(" group by uai.id, uai.name, uai.createTime, upi2.filePath order by uai.createTime desc ");
		ret=(Collection<UserAlbumInfoView>)getResultByHQLAndParam(hql.toString(), params, pagingData);
		return ret;
	}

	@Override
	public UserAlbumInfo getUserAlbumInfoById(String id) {
		UserAlbumInfo ret=this.loadForUpdate(id);
		if(ret!=null && ret.getDeleteFlag()) {
			ret=null;
		}
		return ret;
	}
}
