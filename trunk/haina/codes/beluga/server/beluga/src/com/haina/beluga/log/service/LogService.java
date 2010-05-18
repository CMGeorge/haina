package com.haina.beluga.log.service;

import java.util.List;

import org.springframework.stereotype.Component;

import com.haina.beluga.log.dao.ILogDAO;
import com.haina.beluga.log.domain.Log;
import com.haina.beluga.log.mvc.LogCommand;
import com.sihus.core.service.BaseSerivce;

/**
 * @author:付翔.
 * @createDate:2007-7-17.
 * @classInfo:
 */
@Component
public class LogService extends BaseSerivce<ILogDAO,Log,String> implements ILogService {

	//private ILogDao logdao;
	
	public List<Log> findByPaginate(LogCommand logc,int startIndex,int rowCount) {
//		List<Log>  list = new ArrayList<Log>();
		String[] parm = new String[]{logc.getUser(),logc.getRemark(),
				logc.getStartIp(),logc.getEndIp(),
				logc.getStartTime(),logc.getEndTime()};
		List<Log> dbList =  getBaseDao().findlogbyParams(parm,startIndex,rowCount);
//		if(dbList == null || dbList.size()<0)
//			return dbList;
//		IPSeeker ipSeeker = IPSeeker.getInstance();
//		for(int i = 0 ; i < 20 ; i ++ ){
//			Log log = new Log();
//			log.setId(i+"");
			
//			String ip = dbList.get(i).getIp();
//			dbList.get(i).setIp("("+ip+")"+ipSeeker.getAddress(ip));
//			list.add(log);
//		}
		return dbList;
	}
	

	public Long getSizeByPaginate(LogCommand logc) {
		String[] parm = new String[]{logc.getUser(),logc.getRemark(),
				logc.getStartIp(),logc.getEndIp(),
				logc.getStartTime(),logc.getEndTime()};
		return getBaseDao().getSizebyParams(parm);
	}

//	public void setLogdao(ILogDao logdao) {
//		this.logdao = logdao;
//	}


}
