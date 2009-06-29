package com.haina.beluga.webservice;


import java.io.IOException;
import java.lang.reflect.Method;

import org.apache.log4j.Logger;

import com.caucho.hessian.micro.MicroHessianInput;
import com.caucho.hessian.micro.MicroHessianOutput;
import com.caucho.services.server.AbstractSkeleton;

import flexjson.JSONDeserializer;
import flexjson.JSONSerializer;

public class OUSkeleton extends AbstractSkeleton {
	 
	private Object service;
	private Class<?> intface;
	static Logger logger = Logger.getLogger(OUSkeleton.class.getName());
	
	protected OUSkeleton(Class<?> apiClass) {
		super(apiClass);
	}
	
	public OUSkeleton(Object service, Class<?> apiClass){
	    super(apiClass);
	    this.service = service;
	    this.intface = apiClass;
	    if (! apiClass.isAssignableFrom(service.getClass()))
	    	logger.error("Service " + service + " must be an instance of " + apiClass.getName(),new IllegalArgumentException());
	      //throw new IllegalArgumentException("Service " + service + " must be an instance of " + apiClass.getName());
	  }
	
	 private Method getMothodByClass(Class<?> clazz,String methodName){
		 Method m[] = clazz.getDeclaredMethods();
         for (int i = 0; i < m.length; i++){
        	 if (m[i].getName().equals(methodName))
        		 return m[i];
         }
         return null;
             
	 }
	@SuppressWarnings("unchecked")
	public void invoke(String method, MicroHessianInput in, MicroHessianOutput out) throws IOException{
    
	    Method _method = getMothodByClass(intface,method);
	
	    if (_method == null) {
	    	logger.error("NoSuchMethodException:" + method,new NoSuchMethodException());
	     // return;
	    }
	
	    Class<?>[] args = _method.getParameterTypes();
	    Object[] values = null; 
	    if(args.length > 0 ){
	    	logger.info(args.length + ":Parameters in "+_method.getName());
		    values = new Object[args.length];
		    for (int i = 0; i < args.length; i++) {
		    	values[i] =  new JSONDeserializer().use(null, args[i])
					.deserialize(in.readString());
		    	logger.info(i +":"+values[i].getClass());
		    }
	    }
	    try {
	    	long t1 = System.currentTimeMillis();
	    	Object result = _method.invoke(service, values);
			long t2 = System.currentTimeMillis();
			String rs=new JSONSerializer().deepSerialize(result);
			out.writeString(rs);
			logger.info("JSON:"+rs);
			logger.info(_method.getName() + ":" + (t2 - t1));
//			System.out.println(rs);
	    } catch (Throwable e) {
	    	logger.error(e);
	      return;
	    }
   
  }
}
