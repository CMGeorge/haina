package com.haina.beluga.contact.dao;

import java.sql.SQLException;
import java.util.Iterator;
import java.util.List;

import org.hibernate.HibernateException;
import org.hibernate.Query;
import org.hibernate.Session;
import org.springframework.orm.hibernate3.HibernateCallback;
import org.springframework.stereotype.Component;

import com.haina.beluga.contact.domain.PhoneDistrict;
import com.haina.core.dao.BaseDao;

@Component
@SuppressWarnings("unchecked")
public class PhoneDistrictDao extends BaseDao<PhoneDistrict, String> implements
		IPhoneDistrictDao {

	@Override
	public String[] getWeatherCityCodes() {
		return (String[]) getHibernateTemplate().execute(
				new HibernateCallback() {

					@Override
					public Object doInHibernate(Session session)
							throws HibernateException, SQLException {
						Query query = session
								.createQuery("select distinct p.weatherCityCode from PhoneDistrict p");
						query.setCacheable(true);
						query
								.setCacheRegion("com.haina.beluga.contact.domain.PhoneDistrict");
						return query.list().toArray(new String[] {});
					}

				});
		// String hql ="select distinct p.weatherCityCode from PhoneDistrict p";
		// List<String> list = (List<String>) getResultByHQLAndParam(hql);
		// return list.toArray(new String[]{});
	}

	@Override
	public Iterator<PhoneDistrict> getPhoneDistrictsByUpdateFlg(int updateFlg) {
		String hql = "from PhoneDistrict where updateFlg > ?";
		return (Iterator<PhoneDistrict>) getIteratorByHQLAndParam(hql,
				updateFlg);
	}

	@Override
	public Iterator<PhoneDistrict> getPhoneDistrictsByUpdateFlg(int updateFlg,
			int begin, int count) {
		String hql = "from PhoneDistrict where updateFlg > " + updateFlg;
		Query query = getSession().createQuery(hql);
		query.setFirstResult(begin);
		query.setMaxResults(count);
		return (Iterator<PhoneDistrict>) query.iterate();
	}

	@Override
	public Long getPhoneDistrictsByUpdateFlgCount(int updateFlg) {
		String hql = "select count(*) from PhoneDistrict where updateFlg > ?";
		List list = getResultByHQLAndParam(hql, updateFlg);
		return ((Long) list.get(0)).longValue();
	}

}
