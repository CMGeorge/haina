package com.sihus.core.util;

import java.beans.BeanInfo;
import java.beans.IntrospectionException;
import java.beans.Introspector;
import java.beans.PropertyDescriptor;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.lang.reflect.Modifier;

public class BeanUtil { 
	
	 public static boolean isEmpty(final String value) {
	        return value == null || value.trim().length() == 0 || "null".endsWith(value);
	    }
	
/** 实现将源类属性拷贝到目标类中
 * @param source 
 * @param target
 */
	public static void copyPropertie(Object source, Object target) {
	 try {
	      //获取目标类的属性信息
	      BeanInfo targetbean = Introspector.getBeanInfo(target.getClass());
	      PropertyDescriptor[] propertyDescriptors = targetbean.getPropertyDescriptors();
	      //对每个目标类的属性查找set方法，并进行处理
	      for (int i = 0; i < propertyDescriptors.length; i++) {
	           PropertyDescriptor pro = propertyDescriptors[i];
	           Method wm = pro.getWriteMethod();
	           if (wm != null) {//当目标类的属性具有set方法时，查找源类中是否有相同属性的get方法
	               BeanInfo sourceBean = Introspector.getBeanInfo(source.getClass());
	               PropertyDescriptor[] sourcepds = sourceBean.getPropertyDescriptors();
	               for (int j = 0; j < sourcepds.length; j++) {
	                    if (sourcepds[j].getName().equals(pro.getName())) { //匹配
	                         Method rm = sourcepds[j].getReadMethod();
	                         //如果方法不可访问(get方法是私有的或不可达),则抛出SecurityException
	                         if (!Modifier.isPublic(rm.getDeclaringClass().getModifiers())) {
	                              rm.setAccessible(true);
	                         }
	                        //获取对应属性get所得到的值
	                        Object value = rm.invoke(source,new Object[0]);
	                        if (!Modifier.isPublic(wm.getDeclaringClass().getModifiers())) {
	                             wm.setAccessible(true);
	                        }
	                        //调用目标类对应属性的set方法对该属性进行填充
	                        wm.invoke((Object) target, new Object[] { value });
	                        break;
	                    }
	               }
	            }
	        }
	 } catch (IntrospectionException e) {
	     e.printStackTrace();
	 } catch (IllegalArgumentException e) {
	     e.printStackTrace();
	 } catch (IllegalAccessException e) {
	     e.printStackTrace();
	} catch (InvocationTargetException e) {
	    e.printStackTrace();
	}
	}

}
