package com.haina.beluga.log.dao;

import java.util.List;

import org.hibernate.Query;
import org.hibernate.Session;
import org.springframework.stereotype.Component;

import com.haina.beluga.log.domain.Log;
import com.sihus.core.dao.BaseDao;
import com.sihus.core.util.StringUtils;
/**
 * @author:付翔.
 * @createDate:2007-7-16.
 * @classInfo:
 * 使用查询缓存时，需用hql对象查询，以便hibernate可以访问控制对象，不可用原生态SqlQuery。
 */
@Component
public class LogDAO extends BaseDao<Log,String> implements ILogDAO{

	@SuppressWarnings("unchecked")
	public List<Log> findlogbyParams( String[] param,final int startIndex,final int rowCount) {
		final String hqlStr = toHql(param,false);
		  Session session = getSession();
		  Query query = session.createQuery("from Log");  
//          query.setFirstResult(startIndex).setMaxResults(rowCount);   
          query.setCacheable(true);
          query.setCacheRegion("com.haina.beluga.log.domain.Log");
          return query.list(); 
		 
	}


	public Long getSizebyParams(String[] param) {
		final String hqlStr = toHql(param,true);
		Session session = getSession();
		  Query query = session.createSQLQuery(hqlStr);   
        return (Long) query.list().get(0);
	}
	private String toHql(String[] param,boolean isGetSize){
		StringBuffer hql = null;
		if( ! isGetSize)
			hql = new StringBuffer("select * from Log log where 1=1 ");
		else
			hql = new StringBuffer("select count(*) from Log log where 1=1 ");
//		if(!"".equals(param[0])){
//			if( ! isGetSize)
//				hql = new StringBuffer("select log from Log log,Account at where log.account=at.id and at.username='"+param[0]+"' ");
//			else
//				hql = new StringBuffer("select count(*) from Log log,Account at where log.account=at.id and at.username='"+param[0]+"' ");
//			
//		}
		if(!StringUtils.isNull(param[1])){
			hql.append("and log.remark>="+param[1]+" ");
		}
		if(!StringUtils.isNull(param[2])){
			hql.append("and log.ip >='"+param[2]+"' ");
		}
		if(!StringUtils.isNull(param[3])){
			hql.append("and log.ip <='"+param[3]+"' ");
		}
		if(!StringUtils.isNull(param[4])){
			hql.append("and log.logTime >='"+param[4]+"' ");
		}
		if(!StringUtils.isNull(param[5])){
			hql.append("and log.logTime <='"+param[5]+"'");
		}
		hql.append(" order by log.logTime desc");
		return hql.toString();
	}
}
