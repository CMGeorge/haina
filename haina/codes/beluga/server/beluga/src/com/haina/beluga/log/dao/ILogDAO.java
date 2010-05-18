package com.haina.beluga.log.dao;

import java.util.List;

import com.haina.beluga.log.domain.Log;
import com.sihus.core.dao.IBaseDao;

/**
 * @author:付翔.
 * @createDate:2007-7-16.
 * @classInfo:
 */

public interface ILogDAO extends IBaseDao<Log, String> {
	
	public List<Log> findlogbyParams( String[] param,int startIndex,int rowCount);
	public Long getSizebyParams( String[] param);
}
