package com.sihus.core.util;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class SafeMapUtil {
	
	private static ThreadLocal<Map<String, Object>> threadLocal = new ThreadLocal<Map<String, Object>>();

	@SuppressWarnings("unchecked")
	public static Map<String, Object> getThreadLocalMap() {
		if(null ==threadLocal.get()){
			threadLocal.set(new ConcurrentHashMap<String, Object>()) ;
		}
		Map map = (Map) threadLocal.get();
		return map;
	}
	
	public static void set(String key,Object value){
		getThreadLocalMap().put(key, value);
	}
	
	public static Object get(String key){
		return getThreadLocalMap().get(key);
	}
}
