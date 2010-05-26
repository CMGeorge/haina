package test;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.List;

import javax.imageio.ImageIO;

import com.sihus.core.util.MfTime;
import com.sun.syndication.feed.synd.SyndEntryImpl;
import com.sun.syndication.feed.synd.SyndFeed;
import com.sun.syndication.io.FeedException;
import com.sun.syndication.io.SyndFeedInput;

public class RomeTest {
	
	public static void main(String[] args){
//		for(int i =0;i<=176;i++)
//			getIconWeather(i);
		System.out.println(MfTime.toNow());;
		testQQstatus(25310890);
	}
	/**
	 * 10000<QQ<=1425000000;
	 * @param a
	 * @return
	 */
	public static int testQQstatus(int a){
		URL feedUrl;
		try {
			feedUrl = new URL("http://wpa.qq.com/pa?p=1:"+a+":4");
			java.awt.image.BufferedImage bi = javax.imageio.ImageIO.read(feedUrl);

//			bi.getWidth(); //获得 宽度

//			bi.getHeight() ;
			
//			byte[] bytes = new byte[100];
//			int length = 0;
//			while ((length = pi.read(bytes)) != -1) {
//				os.write(bytes, 0, length);
//			}
//			os.close();
//			os.flush();
//			pi.close();
			System.out.println(a+":"+bi.getWidth());
		} catch (MalformedURLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IllegalArgumentException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return 0;
	}
	/**
	 * 10000<QQ<=1425000000;
	 * @param a
	 * @return
	 */
	public static int getIconWeather(int a){
		String b = null;
		if(a<10){
			b = "00"+a;
		}else
		if(10<=a && a<100){
			b = "0"+a;
		}else
			b=a+"";
		URL feedUrl;
		try {
			feedUrl = new URL("http://deskwx.weatherbug.com/images/Forecast/icons/cond"+b+".gif");
			java.awt.image.BufferedImage bi = javax.imageio.ImageIO.read(feedUrl);
			File file = new File("F:/icon/cond"+b+".gif");
			FileOutputStream out = new FileOutputStream(file);
			ImageIO.write(bi, "gif", out);
			out.close();
		} catch (MalformedURLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IllegalArgumentException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return 0;
	}
	public void testRome(){
		URL feedUrl;
		try {
			feedUrl = new URL("http://news.baidu.com/n?cmd=7&loc=0&name=上海&tn=rss");
			SyndFeedInput input = new SyndFeedInput();
			SyndFeed feed = input.build(new InputStreamReader(feedUrl.openStream()));
			List<SyndEntryImpl> list = feed.getEntries();
			for(SyndEntryImpl syndFeed:list){
				System.out.println(syndFeed.getTitle());
			}
//			System.out.println(feed.getEntries());
		} catch (MalformedURLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IllegalArgumentException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (FeedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}
