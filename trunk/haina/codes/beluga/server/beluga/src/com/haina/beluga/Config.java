package com.haina.beluga;

import java.util.ResourceBundle;

import com.haina.core.util.StringUtils;

/**
 * 全局配置类。
 * @author huangyongqiang
 *
 */
public final class Config {

	private final static ResourceBundle GLOBAL_CONFIG =ResourceBundle.getBundle("configs.global-config");
	
	
	public static String getGlobalConfig(String name) {
		String ret=null;
		if(!StringUtils.isNull(name)) {
			ret=GLOBAL_CONFIG.getString(name);
		}
		return ret;
	}
	
	public static void main(String[] args) {
		System.out.println(GLOBAL_CONFIG.getObject("com.haina.beluga.global.file.base.path"));
	}
}
