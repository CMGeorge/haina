package com.sihus.core.dao;

import java.io.Serializable;
import java.lang.reflect.ParameterizedType;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

import org.hibernate.LockMode;
import org.hibernate.Query;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.criterion.DetachedCriteria;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.orm.hibernate3.support.HibernateDaoSupport;

import com.sihus.core.model.IModel;
import com.sihus.core.util.PagingData;

/**
 * Hibernate implementation of GenericDao A typesafe implementation of CRUD and
 * finder methods based on Hibernate and Spring AOP The finders are implemented
 * through the executeFinder method. Normally called by the
 * FinderIntroductionInterceptor
 */
@SuppressWarnings("unchecked")
public class BaseDao<T extends IModel, PK extends Serializable> extends
		HibernateDaoSupport implements IBaseDao<T, PK> {
	
	@Autowired
	@Qualifier("sessionFactory")
	public SessionFactory sessionFactory;

	private Class<T> clazz;
	
	public BaseDao() {
		clazz =  (Class<T>) ((ParameterizedType) this.getClass().getGenericSuperclass()).getActualTypeArguments()[0];
	}
	
	// private T type;
	
	@Override
	public PK create(T o) {
		return (PK) getHibernateTemplate().save(o);
	}
	@Override
	public T read(PK id) {
		return (T) getHibernateTemplate().get(clazz, id);
	}
	
	@Override
	public T loadForUpdate(PK id) {
		return (T) getHibernateTemplate().get(clazz, id, LockMode.UPGRADE);
	}

	@Override
	public T readForUpdate(PK id) {
		return (T) getHibernateTemplate().load(clazz, id, LockMode.UPGRADE);
	}

	@Override
	/**
	 * 走缓存
	 */
	public T load(PK id) {
		return (T) getHibernateTemplate().load(clazz, id);
	}
	
	@Override
	public void update(T o) {
		getHibernateTemplate().update(o);
	}
	
	@Override
	public void delete(T o) {
		getHibernateTemplate().delete(o);
	}

	// public Session getSession() {
	// boolean allowCreate = true;
	// return SessionFactoryUtils.getSession(sessionFactory, allowCreate);
	// }

	@Override
	public void saveOrUpdate(T newInstance) {
		getHibernateTemplate().saveOrUpdate(newInstance);
	}

	@Override
	public void deleteById(PK id) {
		getHibernateTemplate().delete(read(id));
	}

	/*@Autowired(required = true)
	@Override
	public void setBaseModel(T t) {
		type = t;

	}

	@Override
	public T getBaseModel() {
		return type;
	}*/

	@Override
	public List<T> getModels() {
//		return (List<T>) getResultByHQLAndParam("from "+ type.getClass().getSimpleName());
		getHibernateTemplate().setCacheQueries(true);
		return getHibernateTemplate().loadAll(clazz);
	}

	@Override
	public Long getModelSize() {
		String hql = "select count(*) from " + clazz.getSimpleName();
		List list= getResultByHQLAndParam(hql);
		return ((Long) list.get(0)).longValue();
	}

	
	public void setSessionFactory1(SessionFactory sessionFactory) {
		super.setSessionFactory(sessionFactory);
	}
	@Override
	public List<T> getModelByPage(T exampleEntity, int begin, int count) {
		int first=begin;
		int size=count;
		if(first<0) {
			first=1;
		}
		if(size<0) {
			size=1;
		}
		List<T> ret = null;
		if (exampleEntity == null) {
			try {
				exampleEntity = clazz.newInstance();
			} catch (InstantiationException e) {
				e.printStackTrace();
			} catch (IllegalAccessException e) {
				e.printStackTrace();
			}
		}
		getHibernateTemplate().setCacheQueries(true);
		ret = getHibernateTemplate().findByExample(exampleEntity, first, size);
		return ret;
	}

	@Override
	public List<T> getModelByHibernateCriteria(DetachedCriteria criteria) {
		List<T> list=null;
		if(criteria!=null) {
			list=this.getHibernateTemplate().findByCriteria(criteria);
		}
		return list;
	}

	@Override
	public List<T> getModelByHibernateCriteria(DetachedCriteria criteria,int begin, int count) {
		int first=begin;
		int size=count;
		if(first<0) {
			first=1;
		}
		if(size<0) {
			size=1;
		}
		List<T> list=null;
		if(criteria!=null) {
			list=this.getHibernateTemplate().findByCriteria(criteria, first, size);
		}
		return list;
	}
	/**
     * 通过HQL和参数查出结果集.
     * @param hql.
     * @return List.
     */
	@Override
    public List<?> getResultByHQLAndParam(String hql){
		getHibernateTemplate().setCacheQueries(true);
    	return getHibernateTemplate().find(hql);
    }
	@Override
    public List<?> getResultByHQLAndParam(String hql,Object object){
		getHibernateTemplate().setCacheQueries(true);
    	return getHibernateTemplate().find(hql,object);
    }
	@Override
    public List<?> getResultByHQLAndParam(String hql,Object[] object){
		getHibernateTemplate().setCacheQueries(true);
    	return getHibernateTemplate().find(hql,object);
    }
	
	@Override
	public List<?> getResultByHQLAndParamForUpdate(String hql, String alias) {
		Session session = this.getSession();
		Query query = session.createQuery(hql);
		query.setLockMode(alias, LockMode.UPGRADE);
		List<?> resultList = query.list();
		this.releaseSession(session);
		return resultList;
	}
	
	@Override
	public List<?> getResultByHQLAndParamForUpdate(String hql, Object[] object, String alias) {
		Session session = this.getSession();
		Query query = session.createQuery(hql);
		prepareQuery(query, object);
		query.setLockMode(alias, LockMode.UPGRADE);
		List<?> resultList = query.list();
		this.releaseSession(session);
		return resultList;
	}
	
	/**
     * 通过HQL和参数查出结果集的Iterator.
     * @param hql.
     * @return List.
     *
	 * 走缓存
	 */
	@Override
    public Iterator<?> getIteratorByHQLAndParam(String hql){
    	return getHibernateTemplate().iterate(hql);
    }
	@Override
    public Iterator<?> getIteratorByHQLAndParam(String hql,Object object){
    	return getHibernateTemplate().iterate(hql,object);
    }
	@Override
    public Iterator<?> getIteratorByHQLAndParam(String hql,Object[] object){
    	return getHibernateTemplate().iterate(hql,object);
    }
//	@Override
//	@Deprecated
//	public List<?> getResultBySQLAndParam(String hql, Object[] object, PagingData page) {
//		Session session = this.getSession();
//		// 查询总件数
//		StringBuffer counterHql = new StringBuffer();
////		int fromIndex = hql.toUpperCase().indexOf("FROM");
//		counterHql.append("SELECT count(*) from(").append(hql).append(")cc");
//		Query counterQuery = session.createSQLQuery(counterHql.toString());
////		counterQuery.setCacheable(true);
//		prepareQuery(counterQuery, object);
//		BigInteger counter = (BigInteger) counterQuery.list().get(0);
//		
//		// 更新分页信息
//		page.setRowsCount(counter.intValue());
//		page.setPagesCount();
//		
//		// 执行查询
//		Query query = session.createSQLQuery(hql);
////		query.setCacheable(true);
//		prepareQuery(query, object);
//		query.setFirstResult(page.getCurrentPage() * page.getRowsPerPage());
//		query.setMaxResults(page.getRowsPerPage());
//		List<?> resultList = query.list();
//		
//		this.releaseSession(session);
//		return resultList;
//	}
	@Override
	public Iterator<?> getIteratorByHQLAndParam(String hql, Object[] object, PagingData page) {
		Session session = this.getSession();
		// 查询总件数
		StringBuffer counterHql = new StringBuffer();
		int fromIndex = hql.toUpperCase().indexOf("FROM");
		counterHql.append("SELECT count(*) ").append(hql.substring(fromIndex)).append("");
		Query counterQuery = session.createQuery(counterHql.toString());
		counterQuery.setCacheable(true);
		prepareQuery(counterQuery, object);
		Long counter = (Long) counterQuery.iterate().next();
		
		// 更新分页信息
		if(page!=null) {
			page.setRowsCount(counter.intValue());
			page.setPagesCount();
		}
		
		// 执行查询
		Query query = session.createQuery(hql);
		query.setCacheable(true);
		prepareQuery(query, object);
		if(page!=null) {
			query.setFirstResult(page.getCurrentPage() * page.getRowsPerPage()-page.getShift());
			query.setMaxResults(page.getRowsPerPage());
		}
		Iterator<?> resultList = query.iterate();
		
		this.releaseSession(session);
		return resultList;
	}
	
	@Override
	public List<?> getResultByHQLAndParam(String hql, Object[] object, PagingData page) {
		Session session = this.getSession();
		// 查询总件数
		StringBuffer counterHql = new StringBuffer();
		int fromIndex = hql.toUpperCase().indexOf("FROM");
		counterHql.append("SELECT count(*) ").append(hql.substring(fromIndex)).append("");
		Query counterQuery = session.createQuery(counterHql.toString());
		counterQuery.setCacheable(true);
		prepareQuery(counterQuery, object);
		Long counter = (Long) counterQuery.iterate().next();
		
		// 更新分页信息
		if(page!=null) {
			page.setRowsCount(counter.intValue());
			page.setPagesCount();
		}
		
		// 执行查询
		Query query = session.createQuery(hql);
		query.setCacheable(true);
		prepareQuery(query, object);
		if(page!=null) {
			query.setFirstResult(page.getCurrentPage() * page.getRowsPerPage());
			query.setMaxResults(page.getRowsPerPage());
		}
		List<?> resultList = query.list();
		
		this.releaseSession(session);
		return resultList;
	}
	
	@Override
	public Iterator<?> getIteratorByHQLAndParamNoUpdate(String hql, Object[] object, PagingData page) {
		Session session = this.getSession();
		
		// 执行查询
		Query query = session.createQuery(hql);
		query.setCacheable(true);
		prepareQuery(query, object);
		if(page!=null) {
			query.setFirstResult(page.getCurrentPage() * page.getRowsPerPage());
			query.setMaxResults(page.getRowsPerPage());
		}
		Iterator<?> resultList = query.iterate();
		
		this.releaseSession(session);
		return resultList;
	}

	@Override
	public List<?> getResultByHQLAndParamNoUpdate(String hql, Object[] object, PagingData page) {
		Session session = this.getSession();
		
		// 执行查询
		Query query = session.createQuery(hql);
		query.setCacheable(true);
		prepareQuery(query, object);
		if(page!=null) {
			query.setFirstResult(page.getCurrentPage() * page.getRowsPerPage());
			query.setMaxResults(page.getRowsPerPage());
		}
		List<?> resultList = query.list();
		
		this.releaseSession(session);
		return resultList;
	}

	@Override
	public Iterator<?> getIteratorByHQLAndParam(String hql, Map<String,Object> args, PagingData page) {
		Session session = this.getSession();
		// 查询总件数
		StringBuffer counterHql = new StringBuffer();
		int fromIndex = hql.toUpperCase().indexOf("FROM");
		counterHql.append("SELECT count(*) ").append(hql.substring(fromIndex)).append("");
		Query counterQuery = session.createQuery(counterHql.toString());
		counterQuery.setCacheable(true);
		prepareQuery(counterQuery, args);
		Long counter = (Long) counterQuery.iterate().next();
		
		// 更新分页信息
		if(page!=null) {
			page.setRowsCount(counter.intValue());
			page.setPagesCount();
		}
		
		// 执行查询
		Query query = session.createQuery(hql);
		query.setCacheable(true);
		prepareQuery(query, args);
		if(page!=null) {
			query.setFirstResult(page.getCurrentPage() * page.getRowsPerPage()-page.getShift());
			query.setMaxResults(page.getRowsPerPage());
		}
		Iterator<?> resultList = query.iterate();
		
		this.releaseSession(session);
		return resultList;
	}
	
	@Override
	public List<?> getResultByHQLAndParam(String hql, Map<String,Object> args, PagingData page) {
		Session session = this.getSession();
		// 查询总件数
		StringBuffer counterHql = new StringBuffer();
		int fromIndex = hql.toUpperCase().indexOf("FROM");
		counterHql.append("SELECT count(*) ").append(hql.substring(fromIndex)).append("");
		Query counterQuery = session.createQuery(counterHql.toString());
		counterQuery.setCacheable(true);
		prepareQuery(counterQuery, args);
		Long counter = (Long) counterQuery.iterate().next();
		
		// 更新分页信息
		if(page!=null) {
			page.setRowsCount(counter.intValue());
			page.setPagesCount();
		}
		
		// 执行查询
		Query query = session.createQuery(hql);
		query.setCacheable(true);
		prepareQuery(query, args);
		if(page!=null) {
			query.setFirstResult(page.getCurrentPage() * page.getRowsPerPage());
			query.setMaxResults(page.getRowsPerPage());
		}
		List<?> resultList = query.list();
		
		this.releaseSession(session);
		return resultList;
	}
	
	@Override
	public Iterator<?> getIteratorByHQLAndParamNoUpdate(String hql, Map<String,Object> args, PagingData page) {
		Session session = this.getSession();
		
		// 执行查询
		Query query = session.createQuery(hql);
		query.setCacheable(true);
		prepareQuery(query, args);
		if(page!=null) {
			query.setFirstResult(page.getCurrentPage() * page.getRowsPerPage());
			query.setMaxResults(page.getRowsPerPage());
		}
		Iterator<?> resultList = query.iterate();
		
		this.releaseSession(session);
		return resultList;
	}
	
	@Override
	public List<?> getResultByHQLAndParamNoUpdate(String hql, Map<String,Object> args, PagingData page) {
		Session session = this.getSession();
		
		// 执行查询
		Query query = session.createQuery(hql);
		query.setCacheable(true);
		prepareQuery(query, args);
		if(page!=null) {
			query.setFirstResult(page.getCurrentPage() * page.getRowsPerPage());
			query.setMaxResults(page.getRowsPerPage());
		}
		List<?> resultList = query.list();
		
		this.releaseSession(session);
		return resultList;
	}
	
	/**
	 * 填充参数
	 * @param query 查询
	 * @param args 参数
	 */
	private void prepareQuery(Query query, Object[] args) {
		if(args != null && args.length>0 && query!=null) {
			for (int i=0;i<args.length;i++) {
				query.setParameter(i, args[i]);
			}
		}
	}

	/**
	 * 填充参数
	 * @param query 查询
	 * @param args 参数
	 */
	private void prepareQuery(Query query, Map<String,Object> args) {
		if(args != null && args.size()>0 && query!=null) {
			Iterator<String> keys=args.keySet().iterator();
			String key=null;
			while(keys.hasNext()) {
				key=keys.next();
				query.setParameter(key, args.get(key));
			}
		}
	}
	
	@Override
	public Session getCurrentSession() {
		return getSession();
	}
}