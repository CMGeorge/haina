package com.haina.beluga.contact.dao;

import java.util.Iterator;
import java.util.List;

import org.hibernate.HibernateException;
import org.hibernate.Session;
import org.springframework.orm.hibernate3.HibernateCallback;
import org.springframework.stereotype.Component;

import com.haina.beluga.contact.domain.Weather;
import com.sihus.core.dao.BaseDao;
@Component
public class WeatherDao extends BaseDao<Weather,String> implements IWeatherDao {

	@Override
	public void delAll() {
//	     List<Weather> weathers =  getModels(true);
//	     for(Weather w:weathers){
//	    	 delete(w);
//	     }
		getHibernateTemplate().execute(new HibernateCallback() { 
			public Object doInHibernate(Session session) throws HibernateException {
				 session.createQuery("delete from Weather").executeUpdate();
				 return null;
				}
		});
//		getSession(true).createQuery("delete from Weather").executeUpdate();
	}

	@Override
	public Iterator<Weather> get7WeatherDatas(String cityCode) {
		String hql = "from Weather where weatherCityCode = ? ";
		return (Iterator<Weather>) getIteratorByHQLAndParam(hql, cityCode);
	}

	@Override
	public void saveAll(List<Weather> list) {
		
		Session session = getSession(true);
		session.createQuery("delete from Weather").executeUpdate();
		for(int i = 0 ; i < list.size(); i++){
			create(list.get(i));
//			if(i % 500 ==0){
//				session.flush();
//				session.clear();
//			}
		}
	}

	
}
