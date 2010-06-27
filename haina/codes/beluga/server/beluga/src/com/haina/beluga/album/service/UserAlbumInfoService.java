package com.haina.beluga.album.service;

import java.util.Collection;
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.haina.beluga.album.dao.IUserAlbumInfoDao;
import com.haina.beluga.album.domain.UserAlbumInfo;
import com.haina.beluga.album.domain.UserAlbumInfoView;
import com.haina.beluga.album.dto.UserAlbumCoverDto;
import com.haina.beluga.album.dto.UserAlbumInfoDto;
import com.haina.beluga.album.dto.UserAlbumInfoListDto;
import com.haina.beluga.common.service.IToolService;
import com.haina.beluga.webservice.IStatusCode;
import com.haina.beluga.webservice.data.AbstractRemoteReturning;
import com.haina.beluga.webservice.data.Returning;
import com.sihus.core.service.BaseSerivce;
import com.sihus.core.util.DateUtil;
import com.sihus.core.util.PagingData;
import com.sihus.core.util.StringUtils;


/**
 * 用户相册远程调用方式业务接口实现类。<br/>
 * @author huangyongqiang
 * @since 2010-05-23
 */
@Service(value="userAlbumInfoService")
public class UserAlbumInfoService extends BaseSerivce<IUserAlbumInfoDao, UserAlbumInfo, String>
		implements IUserAlbumInfoService {
	
	@Autowired(required=true)
	private IToolService toolService;

	@SuppressWarnings("unchecked")
	@Override
	public AbstractRemoteReturning addUserAlbumInfo(String email,
			String albumName, String description) {
		AbstractRemoteReturning ret=new Returning();
		if(StringUtils.isNull(email)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		if(StringUtils.isNull(albumName)) {
			ret.setStatusCode(IStatusCode.INVALID_USER_ALBUM_NAME);
			return ret;
		}
		try {
			Map<String,Object> params=new HashMap<String, Object>();
			params.put("loginName", email);
			List<String> idList=(List<String>)getBaseDao().getResultByHQLAndParam(
					"select id from ContactUser where loginName = :loginName and validFlag = true ", 
					params, null);
			String userId=null;
			if(idList==null || idList.size()<1) {
				ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
				return ret;
			}
			userId=idList.get(0);
			if(StringUtils.isNull(userId)) {
				ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
				return ret;
			}
			UserAlbumInfo album=new UserAlbumInfo();
			album.setAlbumName(albumName);
			album.setCreateUserId(userId);
			album.setDescription(description);
			album.setLastUpdateUserId(userId);
			this.getBaseDao().create(album);
//			ContactUser contactUser=contactUserDao.getValidUserByLoginName(email);
//			if(null==contactUser) {
//				if(StringUtils.isNull(email)) {
//					ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
//					return ret;
//				}
//			}
//			UserAlbumInfo album=new UserAlbumInfo();
//			album.setAlbumName(albumName);
//			album.setCreateUserId(contactUser.getId());
//			album.setDescription(description);
//			album.setLastUpdateUserId(contactUser.getId());
//			this.getBaseDao().create(album);
			return ret;
		} catch (Throwable t) {
			this.log.error("新增相册出错", t);
			ret.setStatusText("新增相册出现异常，请稍候再试");
			ret.setStatusCode(IStatusCode.ADD_USER_ALBUM_ERROR);
			return ret;
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public AbstractRemoteReturning deleteUserAlbumInfo(String[] albumIds,
			String email, String deteleEmail) {
		AbstractRemoteReturning ret=new Returning();
		if(StringUtils.isNull(deteleEmail)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		if(null==albumIds || albumIds.length<1) {
			ret.setStatusCode(IStatusCode.INVALID_USER_ALBUM_ID);
			return ret;
		}
		try {
			Map<String,Object> params=new HashMap<String, Object>();
			params.put("loginName", deteleEmail);
			List<String> idList=(List<String>)getBaseDao().getResultByHQLAndParam(
					"select id from ContactUser where loginName = :loginName and validFlag = true ", 
					params, null);
			String userId=null;
			if(idList==null || idList.size()<1) {
				ret.setStatusCode(IStatusCode.INVALID_USER_ALBUM_ID);
				return ret;
			}
			userId=idList.get(0);
			if(StringUtils.isNull(userId)) {
				ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
				return ret;
			}
			int i=this.getBaseDao().deleteUserAlbumInfoByIds(albumIds, userId);
			if(i<albumIds.length) {
				ret.setStatusCode(IStatusCode.INVALID_PARTIAL_USER_ALBUM_ID);
				ret.setStatusText("部分相册id无效，可能无法删除");
				return ret;
			}
			return ret;
		} catch (Throwable t) {
			this.log.error("删除相册出错", t);
			ret.setStatusText("删除相册出现异常，请稍候再试");
			ret.setStatusCode(IStatusCode.ADD_USER_ALBUM_ERROR);
			return ret;
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public AbstractRemoteReturning editUserAlbumInfo(String albumId,
			String albumName, String description, String email) {
		AbstractRemoteReturning ret=new Returning();
		if(StringUtils.isNull(email)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		if(StringUtils.isNull(albumId)) {
			ret.setStatusCode(IStatusCode.INVALID_USER_ALBUM_ID);
			return ret;
		}
		if(StringUtils.isNull(albumName)) {
			ret.setStatusCode(IStatusCode.INVALID_USER_ALBUM_NAME);
			return ret;
		}
		try {
			Map<String,Object> params=new HashMap<String, Object>();
			params.put("loginName", email);
			List<String> idList=(List<String>)getBaseDao().getResultByHQLAndParam(
					"select id from ContactUser where loginName = :loginName and validFlag = true ", 
					params, null);
			String userId=null;
			if(idList==null || idList.size()<1) {
				ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
				return ret;
			}
			userId=idList.get(0);
			
			UserAlbumInfo album=this.getBaseDao().getUserAlbumInfoById(albumId);
			if(null==album) {
				ret.setStatusCode(IStatusCode.INVALID_USER_ALBUM_ID);
				return ret;
			}
			album.setLastUpdateTime(new Date());
			album.setLastUpdateUserId(userId);
			album.setAlbumName(albumName);
			album.setDescription(description);
			this.getBaseDao().update(album);
			return ret;
		} catch (Throwable t) {
			this.log.error("编辑相册出错", t);
			ret.setStatusText("编辑相册出现异常，请稍候再试");
			ret.setStatusCode(IStatusCode.EDIT_USER_ALBUM_ERROR);
			return ret;
		}
	}

	@Override
	public AbstractRemoteReturning getUserAlbumInfoList(String email,
			String mobile, int curPage, int pageSize) {
		AbstractRemoteReturning ret=new Returning();
		Collection<UserAlbumInfoDto> albumDtos=null;
		PagingData pagingData=new PagingData();
		pagingData.setCurrentPage(curPage);
		pagingData.setRowsPerPage(pageSize);
		UserAlbumInfoListDto userAlbumInfoListDto=new UserAlbumInfoListDto(); 
		try {
			Collection<UserAlbumInfoView> albums=this.getBaseDao().getUserAlbumInfos(email, mobile, pagingData);
			if(pagingData!=null) {
				userAlbumInfoListDto.setRowsCount(pagingData.getRowsCount());
				userAlbumInfoListDto.setPageCount(pagingData.getPagesCount());
			}
			if(albums!=null && albums.size()>0) {
				UserAlbumInfoDto albumDto=null;
				UserAlbumCoverDto coverDto=null;
				byte[] fileData=null;
				albumDtos=new LinkedHashSet<UserAlbumInfoDto>(albums.size());
				for(UserAlbumInfoView albumView:albums) {
					if(albumView!=null) {
						albumDto=new UserAlbumInfoDto();
						albumDto.setAlbumName(albumView.getAlbumName());
						albumDto.setCreateTime(DateUtil.toDefaultString(albumView.getCreateTime()));
						albumDto.setId(albumView.getAlbumId());
						albumDto.setPhotoAmount(albumView.getPhotoAmount());
						
						coverDto=new UserAlbumCoverDto();
						fileData=toolService.getLocalFileForByte(albumView.getCoverFilePath());
						coverDto.setCoverFile(fileData);
						albumDto.setCover(coverDto);
						albumDtos.add(albumDto);
					}
				}
				userAlbumInfoListDto.setAlbums(albumDtos);
			}
		} catch (Throwable t) {
			this.log.error("获取相册列表出错", t);
			ret.setStatusText("获取相册列表出现异常，请稍候再试");
			ret.setStatusCode(IStatusCode.REQUEST_USER_ALBUM_LIST_ERROR);
			return ret;
		}
		ret.setValue(userAlbumInfoListDto);
		return ret;
	}
}