package com.haina.beluga.album.service;

import java.text.MessageFormat;
import java.util.Collection;
import java.util.Date;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Properties;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.stereotype.Service;

import com.haina.beluga.album.dao.IUserPhotoInfoDao;
import com.haina.beluga.album.domain.UserPhotoComment;
import com.haina.beluga.album.domain.UserPhotoCommentView;
import com.haina.beluga.album.domain.UserPhotoInfo;
import com.haina.beluga.album.domain.UserPhotoInfoView;
import com.haina.beluga.album.domain.enumerate.UserPhotoSizeEnum;
import com.haina.beluga.album.dto.UserPhotoCommentDto;
import com.haina.beluga.album.dto.UserPhotoCommentListDto;
import com.haina.beluga.album.dto.UserPhotoInfoDto;
import com.haina.beluga.album.dto.UserPhotoInfoListDto;
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
 * @since 2010-05-27
 */
@Service(value="userPhotoInfoHessianService")
public class UserPhotoInfoHessianService extends BaseSerivce<IUserPhotoInfoDao, UserPhotoInfo, String> implements
		IUserPhotoInfoHessianService {

	@Autowired(required=true)
	private IToolService toolService;
	
	@Autowired(required=true)
	@Qualifier(value="userConfig")
	private Properties userConfig;
	
	@Autowired(required=true)
	@Qualifier(value="userPhotoFileType")
	private Properties fileTypesMap;
	
	@Override
	public AbstractRemoteReturning getPhotosOfUserAlbum(String albumId,
			String email, int curPage, int pageSize) {
		AbstractRemoteReturning ret=new Returning();
		if(StringUtils.isNull(albumId)) {
			ret.setStatusCode(IStatusCode.INVALID_USER_ALBUM_ID);
			return ret;
		}
		PagingData pagingData=new PagingData();
		pagingData.setCurrentPage(curPage);
		pagingData.setRowsPerPage(pageSize);
		UserPhotoInfoListDto userPhotoInfoListDto=new UserPhotoInfoListDto(); 
		try {
			Collection<UserPhotoInfoView> photoList=getBaseDao().getPhotosOfUserAlbum(albumId, email, 
					UserPhotoSizeEnum.genMini, pagingData);
			if(pagingData!=null) {
				userPhotoInfoListDto.setRowsCount(pagingData.getRowsCount());
				userPhotoInfoListDto.setPageCount(pagingData.getPagesCount());
			}
			if(photoList!=null && photoList.size()>0) {
				Collection<UserPhotoInfoDto> photos=new HashSet<UserPhotoInfoDto>(photoList.size());
				UserPhotoInfoDto dto=null;
				byte[] fileData=null;
				for(UserPhotoInfoView photoView:photoList) {
					if(photoView!=null) {
						dto=new UserPhotoInfoDto();
						dto.setCreateTime(DateUtil.toDefaultString(photoView.getCreateTime()));
						dto.setId(photoView.getPhotoId());
						dto.setPhotoDescription(photoView.getPhotoDescription());
						fileData=toolService.getLocalFileForByte(photoView.getPhotoFilePath());
						dto.setPhotoFile(fileData);
						dto.setPhotoName(photoView.getPhotoName());
						photos.add(dto);
					}
				}
			}
		} catch(Throwable t) {
			this.log.error("获取相片列表出错", t);
			ret.setStatusText("获取相片列表出现异常，请稍候再试");
			ret.setStatusCode(IStatusCode.REQUEST_USER_PHOTO_LIST_ERROR);
			return ret;
		}
		ret.setValue(userPhotoInfoListDto);
		return ret;
	}

	@Override
	public AbstractRemoteReturning getUserPhotos(String photoId, String albumId,
			int curPage, int pageSize) {
		AbstractRemoteReturning ret=new Returning();
		if(StringUtils.isNull(photoId)) {
			ret.setStatusCode(IStatusCode.INVALID_USER_PHOTO_ID);
			return ret;
		}
		PagingData pagingData=new PagingData();
		pagingData.setCurrentPage(curPage);
		pagingData.setRowsPerPage(pageSize);
		UserPhotoInfoDto userPhotoInfoDto=null;
		try {
			UserPhotoInfo userPhotoInfo=this.getBaseDao().getUserPhotoInfoById(photoId, albumId);
			if(null==userPhotoInfo) {
				ret.setStatusCode(IStatusCode.INVALID_USER_PHOTO_ID);
				return ret;
			}
			userPhotoInfoDto=new UserPhotoInfoDto();
			userPhotoInfoDto.setCreateTime(DateUtil.toDefaultString(userPhotoInfo.getCreateTime()));
			userPhotoInfoDto.setId(userPhotoInfo.getId());
			userPhotoInfoDto.setPhotoDescription(userPhotoInfo.getPhotoDescription());
			userPhotoInfoDto.setPhotoName(userPhotoInfo.getPhotoName());
			userPhotoInfoDto.setPhotoFile(toolService.getLocalFileForByte(userPhotoInfo.getFilePath()));
			Collection<UserPhotoCommentView> comments=this.getBaseDao().getUserPhotoComments(userPhotoInfo.getId(), pagingData);
			UserPhotoCommentListDto commentList=new UserPhotoCommentListDto();
			if(comments!=null && comments.size()>0) {
				commentList.setRowsCount(pagingData.getRowsCount());
				commentList.setPageCount(pagingData.getPagesCount());
				Collection<UserPhotoCommentDto> commentDtos=new HashSet<UserPhotoCommentDto>();
				UserPhotoCommentDto commentDto=null;
				for(UserPhotoCommentView c:comments) {
					if(c!=null) {
						commentDto=new UserPhotoCommentDto();
						commentDto.setContent(c.getContent());
						commentDto.setCreateTime(DateUtil.toDefaultString(c.getCreateTime()));
						commentDto.setCreateUserEmail(c.getCreateUserEmail());
						commentDto.setId(c.getCommentId());
						commentDto.setTitle(c.getTitle());
						commentDtos.add(commentDto);
					}
				}
				commentList.setComments(commentDtos);
			}
		} catch(Throwable t) {
			this.log.error("获取相片及其评论列表出错", t);
			ret.setStatusText("获取相片及其评论列表出现异常，请稍候再试");
			ret.setStatusCode(IStatusCode.REQUEST_USER_PHOTO_ERROR);
			return ret;
		}
		ret.setValue(userPhotoInfoDto);
		return ret;
	}

	@SuppressWarnings("unchecked")
	@Override
	public AbstractRemoteReturning addUserPhotoInfo(String albumId, String email, 
			String createTime, String photoName, String photoDescription, 
			String mime, String oriFileName, byte[] photoData) {
		AbstractRemoteReturning ret=new Returning();
		if(StringUtils.isNull(albumId)) {
			ret.setStatusCode(IStatusCode.INVALID_USER_ALBUM_ID);
			return ret;
		}
		if(StringUtils.isNull(email)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		if(StringUtils.isNull(photoName)) {
			ret.setStatusCode(IStatusCode.INVALID_USER_PHOTO_NAME);
			return ret;
		}
		if(StringUtils.isNull(mime)) {
			ret.setStatusCode(IStatusCode.INVALID_USER_PHOTO_MIME);
			return ret;
		}
		String extName=fileTypesMap.getProperty(mime.trim());
		if(StringUtils.isNull(extName)) {
			/*不合法的MIME，没有对应的扩展名*/
			ret.setStatusCode(IStatusCode.INVALID_USER_PHOTO_MIME);
			return ret;
		}
		if(StringUtils.isNull(oriFileName)) {
			ret.setStatusCode(IStatusCode.INVALID_USER_PHOTO_NAME);
			return ret;
		}
		if(null==photoData || photoData.length<1) {
			ret.setStatusCode(IStatusCode.INVALID_USER_PHOTO_DATA);
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
			params.clear();
			params.put(albumId, albumId);
			idList=(List<String>)getBaseDao().getResultByHQLAndParam(
					"select id from UserAlbumInfo where id = :albumId and deleteFlag = false ", 
					params, null);
			if(idList==null || idList.size()<1) {
				ret.setStatusCode(IStatusCode.INVALID_USER_ALBUM_ID);
				return ret;
			}
			Date time=StringUtils.isNull(createTime)?new Date():DateUtil.parse(DateUtil.DEFAULT_DATE_FORMAT, createTime);
			
			/*$1生成相册本身文件和缩略图*/
			String fileName=MessageFormat.format(userConfig.getProperty("com.haina.beluga.album.photo.file.name.format"),
					StringUtils.getRandom(32),extName);
			String filePath=MessageFormat.format(userConfig.getProperty("com.haina.beluga.album.photo.file.path"),
					fileName);
			String photoUrl=MessageFormat.format(userConfig.getProperty("com.haina.beluga.album.photo.url.format"), 
					fileName);
			
			String fileName2=MessageFormat.format(userConfig.getProperty("com.haina.beluga.album.photo.file.name.format"),
					StringUtils.getRandom(32),extName);
			String filePath2=MessageFormat.format(userConfig.getProperty("com.haina.beluga.album.photo.file.path"),
					fileName2);
			String photoUrl2=MessageFormat.format(userConfig.getProperty("com.haina.beluga.album.photo.url.format"), 
					fileName2);
			int thumbnailWidth=Integer.valueOf(
					userConfig.getProperty("com.haina.beluga.album.photo.thumbnail.width"));/*缩略图宽度*/
			int thumbnailHeight=Integer.valueOf(
					userConfig.getProperty("com.haina.beluga.album.photo.thumbnail.height"));/*缩略图高度*/
			
			this.toolService.createLocalFile(filePath, photoData);
			this.toolService.createLocalThumbnailFile(photoData, filePath2, thumbnailWidth, thumbnailHeight);
			
			/*$2相册本身信息插入数据库*/
			int seqNum=this.getBaseDao().getMaxPhotoSeqNumberOfUserAlbum(albumId)+1;
			UserPhotoInfo userPhotoInfo=new UserPhotoInfo();
			userPhotoInfo.setCreateTime(time);
			userPhotoInfo.setCreateUserId(userId);
			userPhotoInfo.setFilePath(filePath);
			userPhotoInfo.setLastUpdateTime(time);
			userPhotoInfo.setLastUpdateUserId(userId);
			userPhotoInfo.setMime(mime);
			userPhotoInfo.setOriFileName(oriFileName);
			userPhotoInfo.setPhotoDescription(photoDescription);
			userPhotoInfo.setPhotoSize(UserPhotoSizeEnum.normal);
			userPhotoInfo.setPicUrl(photoUrl);
			userPhotoInfo.setSeqNumber(seqNum);
			userPhotoInfo.setUserAlbumInfoId(albumId);
			
			/*$3相册缩略图信息插入数据库*/
			UserPhotoInfo userPhotoInfo2=new UserPhotoInfo();
			userPhotoInfo2.setCreateTime(time);
			userPhotoInfo2.setCreateUserId(userId);
			userPhotoInfo2.setFilePath(filePath2);
			userPhotoInfo2.setLastUpdateTime(time);
			userPhotoInfo2.setLastUpdateUserId(userId);
			userPhotoInfo2.setMime(mime);
			userPhotoInfo2.setOriFileName(oriFileName);
			userPhotoInfo2.setPhotoDescription(photoDescription);
			userPhotoInfo2.setPhotoSize(UserPhotoSizeEnum.genMini);
			userPhotoInfo2.setPicUrl(photoUrl2);
			userPhotoInfo2.setSeqNumber(seqNum);
			userPhotoInfo2.setUserAlbumInfoId(albumId);
			
			this.getBaseDao().create(userPhotoInfo2);
			this.getBaseDao().create(userPhotoInfo);
			return ret;
		} catch (Throwable t) {
			this.log.error("添加相片出错", t);
			ret.setStatusText("添加相片出现异常，请稍候再试");
			ret.setStatusCode(IStatusCode.ADD_USER_PHOTO_ERROR);
			return ret;
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public AbstractRemoteReturning deleteUserPhotoInfo(String[] photoIds,
			String albumId, String email) {
		AbstractRemoteReturning ret=new Returning();
		if(null==photoIds || photoIds.length<1) {
			ret.setStatusCode(IStatusCode.INVALID_USER_PHOTO_ID);
			return ret;
		}
		if(StringUtils.isNull(email)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
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
			int result=getBaseDao().deleteUserPhotoInfoByIds(photoIds, albumId, userId);
			if(result<photoIds.length) {
				ret.setStatusCode(IStatusCode.INVALID_PARTIAL_USER_PHOTO_ID);
				ret.setStatusText("部分用户相片id无效，可能无法删除");
				return ret;
			}
			return ret;
		} catch (Throwable t) {
			this.log.error("删除相片出错", t);
			ret.setStatusText("删除相片出现异常，请稍候再试");
			ret.setStatusCode(IStatusCode.DELETE_USER_PHOTO_ERROR);
			return ret;
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public AbstractRemoteReturning addUserPhotoComment(String commentContent,
			String commentTime, String photoId, String email) {
		AbstractRemoteReturning ret=new Returning();
		if(StringUtils.isNull(email)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
			return ret;
		}
		if(StringUtils.isNull(commentContent)) {
			ret.setStatusCode(IStatusCode.INVALID_USER_PHOTO_COMMENT_CONTENT);
			return ret;
		}
		Date time=null;
		time=DateUtil.parse(DateUtil.DEFAULT_DATE_FORMAT, commentTime);
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
			UserPhotoInfo photoInfo=getBaseDao().getUserPhotoInfoById(photoId);
			if(null==photoInfo) {
				ret.setStatusCode(IStatusCode.INVALID_USER_PHOTO_ID);
				return ret;
			}
			UserPhotoComment userPhotoComment=new UserPhotoComment();
			userPhotoComment.setContent(commentContent);
			userPhotoComment.setCreateTime(time);
			userPhotoComment.setCreateUserId(userId);
			userPhotoComment.setLastUpdateTime(time);
			userPhotoComment.setLastUpdateUserId(userId);
			userPhotoComment.setUserPhotoInfoId(photoId);
			userPhotoComment.setTitle("");
			boolean result=getBaseDao().addUserPhotoComment(userPhotoComment);
			if(!result) {
				ret.setStatusCode(IStatusCode.INVALID_USER_PHOTO_COMMENT);
				return ret;
			}
			return ret;
		} catch (Throwable t) {
			this.log.error("添加相片评论出错", t);
			ret.setStatusText("添加相片评论出现异常，请稍候再试");
			ret.setStatusCode(IStatusCode.ADD_USER_PHOTO_COMMENT_ERROR);
			return ret;
		}
	}

	@SuppressWarnings("unchecked")
	@Override
	public AbstractRemoteReturning deleteUserPhotoComment(String[] commentIds,
			String photoId, String email) {
		AbstractRemoteReturning ret=new Returning();
		if(null==commentIds || commentIds.length<1) {
			ret.setStatusCode(IStatusCode.INVALID_PHOTO_COMMENT_ID);
			return ret;
		}
		if(StringUtils.isNull(email)) {
			ret.setStatusCode(IStatusCode.INVALID_LOGINNAME);
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
			int result=this.getBaseDao().deleteUserPhotoComment(commentIds, photoId, userId);
			if(result<commentIds.length) {
				ret.setStatusCode(IStatusCode.INVALID_PARTIAL_USER_PHOTO_COMMENT_ID);
				ret.setStatusText("部分用户相片评论id无效，可能无法删除");
				return ret;
			}
			return ret;
		} catch (Throwable t) {
			this.log.error("删除相片评论出错", t);
			ret.setStatusText("删除相片评论出现异常，请稍候再试");
			ret.setStatusCode(IStatusCode.DELETE_USER_PHOTO_COMMENT_ERROR);
			return ret;
		}
	}
}