package test;

import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.junit.After;
import org.junit.Before;
import org.junit.Ignore;
import org.junit.Test;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;
import org.springframework.orm.hibernate3.SessionFactoryUtils;
import org.springframework.orm.hibernate3.SessionHolder;
import org.springframework.transaction.support.TransactionSynchronizationManager;

import com.haina.beluga.album.service.IUserAlbumInfoService;
import com.haina.beluga.webservice.PriService;
import com.haina.beluga.webservice.data.AbstractRemoteReturning;
import com.haina.beluga.webservice.data.Returning;

/**
 * Simple test of the PersonDao
 */
public class TestPriService {
	private ApplicationContext factory;

//	public TestPriService(String s) {
//		super(s);
//		factory = new ClassPathXmlApplicationContext(
//				"test/test-applicationContext.xml");
//	}

	public TestPriService() {
		factory = new ClassPathXmlApplicationContext(
				"test/test-applicationContext.xml");
	}
	
	/**
	 * 测试注册
	 * @throws Exception
	 */
	@Ignore(value="已经测试过")
	@Test(expected=Throwable.class)
	public void testRegister() throws Exception {
		Returning h = getPriService().register("paul008@163.com", "123456", "13503280999");
		System.out.println(h);
	}
	
	/**
	 * 测试注册
	 * @throws Exception
	 */
	@Ignore(value="已经测试过")
	@Test(expected=Throwable.class)
	public void testRegister1() throws Exception {
		Returning h = getPriService().register("luke_huang@163.com", "123456", "13503280900");
		System.out.println(h);
	}
	
	/**
	 * 测试添加用户相册
	 * @throws Exception
	 */
	@Ignore(value="已经测试过")
	@Test(expected=Throwable.class)
	public void testAddUserAlbumInfo() throws Exception {
		AbstractRemoteReturning h=this.getUserAlbumInfoService().addUserAlbumInfo("paul008@163.com", "测试相册名1", "测试相册描述1");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名2", "测试相册描述2");
		System.out.println(h);
		
		 h=this.getUserAlbumInfoService().addUserAlbumInfo("paul008@163.com", "测试相册名3", "测试相册描述3");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名4", "测试相册描述4");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("paul008@163.com", "测试相册名5", "测试相册描述5");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名6", "测试相册描述6");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("paul008@163.com", "测试相册名7", "测试相册描述7");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名8", "测试相册描述8");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("paul008@163.com", "测试相册名9", "测试相册描述9");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名10", "测试相册描述10");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名11", "测试相册描述11");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("paul008@163.com", "测试相册名12", "测试相册描述12");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名13", "测试相册描述13");
		System.out.println(h);
		
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名14", "测试相册描述14");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名15", "测试相册描述15");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名16", "测试相册描述16");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名17", "测试相册描述17");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名18", "测试相册描述18");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名19", "测试相册描述19");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名20", "测试相册描述20");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名21", "测试相册描述21");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名22", "测试相册描述22");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名23", "测试相册描述23");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名24", "测试相册描述24");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名25", "测试相册描述25");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名26", "测试相册描述26");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名27", "测试相册描述27");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名28", "测试相册描述28");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名29", "测试相册描述29");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名30", "测试相册描述30");
		System.out.println(h);
		h=this.getUserAlbumInfoService().addUserAlbumInfo("luke_huang@163.com", "测试相册名31", "测试相册描述31");
		System.out.println(h);
	}
	
	/**
	 * 测试获取相册列表
	 * @throws Exception
	 */
	@Ignore(value="已经测试过")
	@Test()
	public void testGetUserAlbumInfoList() throws Exception {
		AbstractRemoteReturning h=this.getUserAlbumInfoService().getUserAlbumInfoList("luke_huang@163.com", null, 1, 11);
		System.out.println(h);
	}
	
	@Ignore(value="已经测试过")
	@Test(expected=Throwable.class)
	public void testDeleteUserAlbumInfo() throws Exception {
		String[] ids=new String[]{"402881e7297776ca01297776dcde0003","402881e7297776ca01297776dcde0004"};
		AbstractRemoteReturning h=this.getUserAlbumInfoService().deleteUserAlbumInfo(ids, "luke_huang@163.com", "luke_huang@163.com");
		System.out.println(h);
	}
	
	@Ignore(value="")
	@Test(expected=Throwable.class)
	public void testLogin() throws Exception {
		AbstractRemoteReturning h = getPriService().login("testuser6", "12345");
		System.out.println(h);
	}
	
	@Ignore(value="")
	@Test(expected=Throwable.class)
	public void testLogin1() throws Exception {
		AbstractRemoteReturning h = getPriService().login("testuser6", "123456");
		System.out.println(h);
		Returning h2 = getPriService().login("testuser6", "123456");
		System.out.println(h2);
	}
	
//	public void testLogout() throws Exception {
//		HessianRemoteReturning h = getPriService().logoutByPsssport("testuser6");
//		System.out.println(h);
//	}
	
//	public void testLogin() throws Exception {
//		HessianRemoteReturning h = getPriService().login("hyqtest", "123456");
//		System.out.println(h);
//	}

	@Before
	public void setUp() throws Exception {
		openSession();
	}

	@After
	public void tearDown() throws Exception {
		closeSession();
	}

	private void openSession() {
		SessionFactory sessionFactory = getSessionFactory();
		Session session = SessionFactoryUtils.getSession(sessionFactory, true);
		TransactionSynchronizationManager.bindResource(sessionFactory,
				new SessionHolder(session));
	}

	private void closeSession() {
		SessionFactory sessionFactory = getSessionFactory();
		SessionHolder sessionHolder = (SessionHolder) TransactionSynchronizationManager
				.unbindResource(sessionFactory);
		sessionHolder.getSession().flush();
		sessionHolder.getSession().close();
		SessionFactoryUtils.releaseSession(sessionHolder.getSession(),
				sessionFactory);
	}

	private void restartSession() {
		closeSession();
		openSession();
	}

	private SessionFactory getSessionFactory() {
		return (SessionFactory) factory.getBean("sessionFactory");
	}

	private PriService getPriService() {
		PriService priService = (PriService) factory.getBean("priService");

		return priService;
	}
	
	private IUserAlbumInfoService getUserAlbumInfoService() {
		IUserAlbumInfoService userAlbumInfoService = (IUserAlbumInfoService)factory.getBean("userAlbumInfoService");
		return userAlbumInfoService;
	}

}
