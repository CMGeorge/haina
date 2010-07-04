package com.sihus.core.dao;

import java.io.Serializable;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

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

	PK create(T newInstance);

	T read(PK id) ;
	
	T readForUpdate(PK id);

	// Load from cache
	T load(PK id);
	
	T loadForUpdate(PK id);

	void saveOrUpdate(T newInstance);

	void update(T transientObject);

	void delete( T persistentObject);

	void deleteById(PK id);

	List<T> getModels();

	Long getModelSize();
	
	/**
	 * 领域模型分页查询方法。<br/>
	 * @param exampleEntity 存放查询条件的领域对象
	 * @param begin 开始位置
	 * @param count 查询的数量
	 * @return
	 */
	List<T> getModelByPage(T exampleEntity, int begin, int count);
	
	/**
	 * 根据Hibernate的Criteria属性查找模型类。<br/>
	 * @param criteria
	 * @return
	 */
	List<T> getModelByHibernateCriteria(DetachedCriteria criteria);
	
	/**
	 * 根据Hibernate的Criteria属性查找模型类。<br/>
	 * @param criteria
	 * @param begin
	 * @param count
	 * @return
	 */
	List<T> getModelByHibernateCriteria(DetachedCriteria criteria,int begin, int count);
	/**
     * 通过HQL和参数查出结果集.
     * @param hql.
     * @return List.
     */
	List<?> getResultByHQLAndParam(String hql);
	
	List<?> getResultByHQLAndParamForUpdate(String hql, String alias);
	
	List<?> getResultByHQLAndParam(String hql,Object object);
	
	List<?> getResultByHQLAndParam(String hql,Object[] object);
	
	List<?> getResultByHQLAndParamForUpdate(String hql, Object[] object, String alias);
	
	List<?> getResultByHQLAndParam(String hql, Object[] object, PagingData page);
	
	List<?> getResultByHQLAndParamNoUpdate(String hql, Object[] object, PagingData page);
	
	Iterator<?> getIteratorByHQLAndParam(String hql, Object[] object, PagingData page);
	
	Iterator<?> getIteratorByHQLAndParamNoUpdate(String hql, Object[] object, PagingData page);
//	@Deprecated
//	List<?> getResultBySQLAndParam(String hql, Object[] object, PagingData page) ;
	
	Iterator<?> getIteratorByHQLAndParam(String hql);
	
	Iterator<?> getIteratorByHQLAndParam(String hql,Object object);
	
	Iterator<?> getIteratorByHQLAndParam(String hql,Object[] object);
	
	Iterator<?> getIteratorByHQLAndParam(String hql, Map<String,Object> args, PagingData page);
	
	List<?> getResultByHQLAndParam(String hql, String countHql, Map<String,Object> args, PagingData page);
	
	Iterator<?> getIteratorByHQLAndParamNoUpdate(String hql, Map<String,Object> args, PagingData page);
	
	List<?> getResultByHQLAndParamNoUpdate(String hql, Map<String,Object> args, PagingData page);
	
	Session getCurrentSession();
}
