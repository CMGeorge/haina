package com.haina.beluga.contact.service;

import java.rmi.RemoteException;
import java.util.HashMap;
import java.util.Map;
import java.util.StringTokenizer;

import javax.xml.rpc.holders.BooleanHolder;
import javax.xml.rpc.holders.StringHolder;

import junit.framework.TestCase;
import net.wxbug.api.ApiLocationData;
import net.wxbug.api.WeatherBugWebServicesLocator;
import net.wxbug.api.WeatherBugWebServicesSoap;

import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;
import org.springframework.orm.hibernate3.SessionFactoryUtils;
import org.springframework.orm.hibernate3.SessionHolder;
import org.springframework.transaction.support.TransactionSynchronizationManager;

import com.haina.beluga.contact.domain.PhoneDistrict;
import com.showji.api.Locating.MobileLocator;
import com.showji.api.Locating.MobileSoap;
import com.sihus.core.util.FileUtil;

/**
 * Simple test of the PersonDao
 */
public class JunitGeneratePhoneDistrictDB extends TestCase
{
    private ApplicationContext factory;
    public final static String ACode="A3432136345";
	public static WeatherBugWebServicesSoap   weatherBugWebServicesSoap ;
	public static MobileSoap moblieSoap;
	public static DefaultCnToSpell spell = new DefaultCnToSpell();
	public static Map<String,String> cityCodeCache = new HashMap<String,String>();
	static{
		try{
		WeatherBugWebServicesLocator   weatherBugWebServicesLocator   =   new   WeatherBugWebServicesLocator();  
        System.out.println("the Web Service:"+weatherBugWebServicesLocator.getWeatherBugWebServicesSoapAddress());  
           weatherBugWebServicesSoap  =  weatherBugWebServicesLocator.getWeatherBugWebServicesSoap(); 
           MobileLocator mobileLocator = new MobileLocator();
           System.out.println("the Web Service:"+mobileLocator.getMobileSoapAddress());  
           moblieSoap = mobileLocator.getMobileSoap();
		}catch(Exception   e){  
	        System.out.println(e);  
	        }  
	}
	public JunitGeneratePhoneDistrictDB(String s)
    {
        super(s);
        factory = new ClassPathXmlApplicationContext("test/test-applicationContext.xml");
    }
	public  String getWeather_city_code(String searchString){
		
		String code = cityCodeCache.get(searchString);
		if(code!=null){
			System.out.println(searchString+":from cache.");
			return code;
		}
        ApiLocationData[] apiLocationData = null;
		try {
			apiLocationData = weatherBugWebServicesSoap.getLocationList(searchString, ACode);
		} catch (RemoteException e) {
//			e.printStackTrace();
//			FileUtil.write("F:\\123.txt", e.getMessage());
		} 
        if(apiLocationData==null ||apiLocationData.length ==0){
//        	System.out.println("error:"+searchString+":没有城市天气代码!");
//        	FileUtil.write("F:\\123.txt", "error:"+searchString+":没有城市天气代码!");
        	cityCodeCache.put(searchString,"");
        	return null;
        }
       for(int i = 0 ; i < apiLocationData.length; i ++ ){
    	   if(apiLocationData[i].getCity().endsWith(searchString)&&apiLocationData[i].getCountry().equals("China")){
    		   String _code =  apiLocationData[i].getCityCode();
    		   cityCodeCache.put(searchString,_code);
    		   return _code;
    		   
    	   }
       }
//       System.out.println("error:"+searchString+":没有城市天气代码!");
//       FileUtil.write("F:\\123.txt", "error:"+searchString+":没有城市天气代码!");
       return null;
         
	}
	public static PhoneAdrModel getStrByNet(Integer phoneCode){
		 try{
			 StringHolder province =  new StringHolder();
			 StringHolder city =  new StringHolder();
			 StringHolder areaCode =  new StringHolder();
			 StringHolder postCode =  new StringHolder();
			 StringHolder corp =  new StringHolder();
			 StringHolder card =  new StringHolder();
			
			 moblieSoap.query(String.valueOf(phoneCode), new BooleanHolder(true), province, city, areaCode, postCode, corp, card);
			 PhoneAdrModel pm = new PhoneAdrModel();
			 pm.setProvince(province.value);
			 pm.setCity(city.value);
			 pm.setAreaCode(areaCode.value);
			 pm.setCorp(corp.value);
			 pm.setCard(card.value);
			 return pm;
		 }catch (Exception e) {
			 e.printStackTrace();
			 try {
				Thread.sleep(3000);
			} catch (InterruptedException e1) {
				// TODO Auto-generated catch block
				//e1.printStackTrace();
			}
			System.out.println("reload:"+phoneCode);
			FileUtil.write("F:\\124.txt", "reload:"+phoneCode);
			return getStrByNet(phoneCode);
			 
		}
			 // return null;
	}
	
	public static String getBigUp(String strSource){
	    String  pe;  
	    char ch[];       
	   StringTokenizer st=new StringTokenizer(strSource); 
	    while(st.hasMoreTokens())
	       {
	        pe=st.nextToken();    
	        ch= pe.toCharArray();
	        if(ch[0]>='a'&&ch[0]<='z')
	        {ch[0]=(char)(ch[0]-32);
	        } 
	      return new String(ch);
	       }
	    return null;
	}
	public  void generate(int start,int end) throws Exception {
		PhoneDistrictService phoneService = getPhoneDistrictService();
		String temp_AreaCode = null;
		boolean isSave= false;
		int temp_start = 0;
		for(int i=start; i < end; i ++ ){
			System.out.println(temp_start);
			PhoneAdrModel pm = getStrByNet(i);
			
			if(pm==null||pm.getCity().equals("")){
				System.out.println("error:"+i+"没找到记录");
//				FileUtil.write("F:\\123.txt", "error:"+i+"没找到记录");
				if(!isSave){
					pm = getStrByNet(temp_start);
					PhoneDistrict pd = new PhoneDistrict();
					String priv =pm.getProvince();
					String city_py = getBigUp(spell.cnToSpell(pm.getCity()));
					String city = pm.getCity();
					String fee = pm.getCorp()+pm.getCard();
					String weather_city_code = getWeather_city_code(city_py);
					pd.setDistrictCity(city);
					pd.setDistrictNumber(pm.getAreaCode());
					pd.setDistrictProvince(priv);
					pd.setFeeType(fee);
//					pd.setRangeStart(new Long(temp_start));
//					pd.setRangeEnd(new Long(i-1));
					pd.setPingyinCity(city_py);
					pd.setUpdateFlg(1);
					pd.setWeatherCityCode(weather_city_code);
					//save
					phoneService.create(pd);
					restartSession();
					isSave=true;
				   }
					//temp_start = 0;
					temp_AreaCode  = null;
					continue;
				
			}
			if(temp_AreaCode == null){
				temp_AreaCode  = pm.getAreaCode();
				temp_start = i;
				isSave=false;
				
			}else if(!temp_AreaCode.equals(pm.getAreaCode())){
				temp_AreaCode  = pm.getAreaCode();
//				System.out.println(i);
				pm = getStrByNet(temp_start);
				PhoneDistrict pd = new PhoneDistrict();
				String priv =pm.getProvince();
				String city_py = getBigUp(spell.cnToSpell(pm.getCity()));
				String city = pm.getCity();
				String fee = pm.getCorp()+pm.getCard();
				String weather_city_code = getWeather_city_code(city_py);
				pd.setDistrictCity(city);
				pd.setDistrictNumber(pm.getAreaCode());
				pd.setDistrictProvince(priv);
				pd.setFeeType(fee);
//				pd.setRangeStart(new Long(temp_start));
//				pd.setRangeEnd(new Long(i-1));
				pd.setPingyinCity(city_py);
				pd.setUpdateFlg(1);
				pd.setWeatherCityCode(weather_city_code);
				//save
				phoneService.create(pd);
				restartSession();
//				isSave=false;
				temp_start = i;
				
			}
			
			if(i==end-1){
//				System.out.println(i);
				pm = getStrByNet(temp_start);
				PhoneDistrict pd = new PhoneDistrict();
				String priv =pm.getProvince();
				String city_py = getBigUp(spell.cnToSpell(pm.getCity()));
				String city = pm.getCity();
				String fee = pm.getCorp()+pm.getCard();
				String weather_city_code = getWeather_city_code(city_py);
				pd.setDistrictCity(city);
				pd.setDistrictNumber(pm.getAreaCode());
				pd.setDistrictProvince(priv);
				pd.setFeeType(fee);
//				pd.setRangeStart(new Long(temp_start));
//				pd.setRangeEnd(new Long(i));
				pd.setPingyinCity(city_py);
				pd.setUpdateFlg(1);
				pd.setWeatherCityCode(weather_city_code);
				//save
				phoneService.create(pd);
				restartSession();
			}
			
		}
		//System.out.println(getWeather_city_code("Sihong"));
		//System.out.println(getStrByNet(138513669));
		
	}
   
    public void testGenerate130() throws Exception
    {
//    	generate(1333600,1400000);
//    	restartSession();
//    	generate(1534998,1540000);
//    	restartSession();
//    	generate(1550020,1570000);
//    	restartSession();
//    	generate(1580000,1600000);
//    	restartSession();
//    	generate(1882200,1900000);
//    	generate(1880210,1890000);
    }
    
//    public void testCache() throws Exception
//    {
////    	LogService logService = getLogService();
////    	logService.findAll(true);
////    	logService.findAll(true);
//    }
    
    protected void setUp() throws Exception
    {
    	
        openSession();
    }

    protected void tearDown() throws Exception
    {
        closeSession();
    }

    private void openSession()
    {
        SessionFactory sessionFactory = getSessionFactory();
        Session session = SessionFactoryUtils.getSession(sessionFactory, true);
        TransactionSynchronizationManager.bindResource(sessionFactory, new SessionHolder(session));
    }

    private void closeSession()
    {
        SessionFactory sessionFactory = getSessionFactory();
        SessionHolder sessionHolder = (SessionHolder) TransactionSynchronizationManager.unbindResource(sessionFactory);
        sessionHolder.getSession().flush();
        sessionHolder.getSession().close();
        SessionFactoryUtils.releaseSession(sessionHolder.getSession(), sessionFactory);
    }

    private void restartSession()
    {
        closeSession();
        openSession();
    }

    private SessionFactory getSessionFactory()
    {
        return (SessionFactory) factory.getBean("sessionFactory");
    }

    private PhoneDistrictService getPhoneDistrictService()
    {
    	PhoneDistrictService phoneDistrictService = (PhoneDistrictService) factory.getBean("phoneDistrictService");
    	
        return phoneDistrictService;
    }
}
