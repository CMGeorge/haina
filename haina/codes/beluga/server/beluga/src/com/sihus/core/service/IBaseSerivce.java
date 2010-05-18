package com.sihus.core.service;

import java.io.Serializable;
import java.util.List;

import com.sihus.core.dao.IBaseDao;
import com.sihus.core.model.IModel;
/**
 * 业务与管理分离,业务逻辑不处理异常.
 * 异常交与SpringAOP事务管理．
 * @author X_FU.
 *
 */
public interface IBaseSerivce<D extends IBaseDao<T,PK>,T extends IModel,PK extends Serializable> {
	
	public D getBaseDao();
	
	public void create(T model);
	public void saveOrUpdate(T model);
	public void update(T model); 
	public void delete(T model);
	public void deleteById(PK id);
	// load from cache
	public T load(PK id);
	public T read(PK id);
	public List<T> findAll();
	public Long findAllSize();
}
