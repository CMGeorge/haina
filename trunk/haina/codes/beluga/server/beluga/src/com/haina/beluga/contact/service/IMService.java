package com.haina.beluga.contact.service;

import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URL;

import org.springframework.stereotype.Component;

import com.haina.beluga.webservice.Constant;
import com.haina.beluga.webservice.data.Returning;
@Component
public class IMService implements IIMService{

	public Returning getQQStatus(String qqCode){
		Returning hrr = new Returning();
		if(Integer.valueOf(qqCode) <= 10000 || Integer.valueOf(qqCode) >1425000000){
			hrr.setStatusCode(Constant.QQ_ARG_ERROE);
			return hrr;
		}
		URL feedUrl;
		try {
			feedUrl = new URL("http://wpa.qq.com/pa?p=1:"+qqCode+":4");
			java.awt.image.BufferedImage bi = javax.imageio.ImageIO.read(feedUrl);
			if(bi.getWidth()==Constant.QQ_OFFLINE_WIDTH)
				hrr.setValue(Constant.QQ_OFFLINE) ;
			else
				hrr.setValue(Constant.QQ_ONLINE) ;
		} catch (MalformedURLException e) {
			e.printStackTrace();
			hrr.setStatusCode(Constant.QQ_NETWORK_ERROR) ;
		} catch (IllegalArgumentException e) {
			e.printStackTrace();
			hrr.setStatusCode(Constant.QQ_NETWORK_ERROR) ;
		}catch (IOException e) {
			e.printStackTrace();
			hrr.setStatusCode(Constant.QQ_NETWORK_ERROR) ;
		}
		return hrr;
	}
}
