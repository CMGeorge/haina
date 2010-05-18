package com.sihus.core.util;

import java.io.InputStream;
import java.util.Properties;

public class PropertiesUtils {
	 private static Properties properties = new Properties(); 
	 
	 static{
		 try { InputStream is = PropertiesUtils.class.getClassLoader().getResourceAsStream( "init.properties");
	 	   		properties.load(is);
	 } catch (Exception e) {
		 e.printStackTrace();
		 } 
	 }
	 
	 public static String getProperty(String key) { 
		 return properties.getProperty(key).trim();
		 
	 }
}
