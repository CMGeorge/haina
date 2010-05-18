package com.sihus.core.dao;

import java.io.Serializable;
import java.util.Iterator;
import java.util.List;

import org.hibernate.Session;
import org.hibernate.criterion.DetachedCriteria;

import com.sihus.core.model.IModel;
import com.sihus.core.util.PagingData;

/**
 * The basic GenericDao interface with CRUD methods Finders are added with
 * interface inheritance and AOP introductions for concrete implementations
 * 
 * Extended interfaces may declare methods starting with find... list...
 * iterate... or scroll... They will execute a preconfigured query that is
 * looked up based on the rest of the method name
 */
public interface IBaseDao<T extends IModel, PK extends Serializable> {

	public PK create(T newInstance);

	public T read(PK id) ;
	
	public T readForUpdate(PK id);

	// Load from cache
	public T load(PK id);
	
	public T loadForUpdate(PK id);

	public void saveOrUpdate(T newInstance);

	public void update(T transientObject);

	public void delete( T persistentObject);

	public void deleteById(PK id);

	public List<T> getModels();

	public Long getModelSize();
	
	/**
	 * 领域模型分页查询方法。<br/>
	 * @param exampleEntity 存放查询条件的领域对象
	 * @param begin 开始位置
	 * @param count 查询的数量
	 * @return
	 */
	public List<T> getModelByPage(T exampleEntity, int begin, int count);
	
	/**
	 * 根据Hibernate的Criteria属性查找模型类。<br/>
	 * @param criteria
	 * @return
	 */
	public List<T> getModelByHibernateCriteria(DetachedCriteria criteria);
	
	/**
	 * 根据Hibernate的Criteria属性查找模型类。<br/>
	 * @param criteria
	 * @param begin
	 * @param count
	 * @return
	 */
	public List<T> getModelByHibernateCriteria(DetachedCriteria criteria,int begin, int count);
	/**
     * 通过HQL和参数查出结果集.
     * @param hql.
     * @return List.
     */
	public List<?> getResultByHQLAndParam(String hql);
	
	public List<?> getResultByHQLAndParamForUpdate(String hql, String alias);
	
	public List<?> getResultByHQLAndParam(String hql,Object object);
	
	public List<?> getResultByHQLAndParam(String hql,Object[] object);
	
	public List<?> getResultByHQLAndParamForUpdate(String hql, Object[] object, String alias);
	
	public List<?> getResultByHQLAndParam(String hql, Object[] object, PagingData page);
	
	public List<?> getResultByHQLAndParamNoUpdate(String hql, Object[] object, PagingData page);
	
	public Iterator<?> getIteratorByHQLAndParam(String hql, Object[] object, PagingData page);
	
	public Iterator<?> getIteratorByHQLAndParamNoUpdate(String hql, Object[] object, PagingData page);
//	@Deprecated
//	public List<?> getResultBySQLAndParam(String hql, Object[] object, PagingData page) ;
	
	public Iterator<?> getIteratorByHQLAndParam(String hql);
	
	public Iterator<?> getIteratorByHQLAndParam(String hql,Object object);
	
	public Iterator<?> getIteratorByHQLAndParam(String hql,Object[] object);
	
	public Session getCurrentSession();
}
