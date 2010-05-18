package com.haina.beluga.log.service;

import java.util.List;

import com.haina.beluga.log.dao.ILogDAO;
import com.haina.beluga.log.domain.Log;
import com.haina.beluga.log.mvc.LogCommand;
import com.sihus.core.service.IBaseSerivce;

/**
 * @author:付翔.
 * @createDate:2007-7-17.
 * @classInfo:
 */

public interface ILogService extends IBaseSerivce<ILogDAO,Log,String> {
	
	/*分页*/
	public List<Log> findByPaginate(LogCommand logc,int startIndex,int rowCount);
	public Long getSizeByPaginate(LogCommand logc);

}
