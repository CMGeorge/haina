#ifndef		__XCONFIG__H__
#define		__XCONFIG__H__

//----------------------- host info ---------------------------------------------------
#define		KHostNamePort			8079
#define		KHostName				_T("yanzisoft.oicp.net")					//������ip������
//#define		KHostName				_T("192.168.3.105")					//������ip������
//#define		KHostName				_T("58.24.49.13")					//������ip������
//#define		KHostName				_T("192.168.0.1")					//������ip������


//----------------------- public services ---------------------------------------------------
#define		KTestHessianUrl			_T("/myapp/TestHessianServlet")
#define		KTestCN					_T("/beluga/pub?call=testCN")				//���Ĳ��Ե�ַ
#define		KGetQQStatusUrl			_T("/beluga/pub?call=getQQStatus")			//��ȡQQ����״̬
#define		KGetOrUpdatePDUrl		_T("/beluga/pub?call=getOrUpdatePD")		//��ȡ����������
#define		KGetLiveWeatherUrl		_T("/beluga/pub?call=getLiveWeather")		//��ȡ�����������
#define		KGet7WeatherUrl			_T("/beluga/pub?call=get7Weatherdatas")		//��ȡ�������������


//----------------------- private services ---------------------------------------------------
#define		KRegisterUrl			_T("/beluga/pri?call=register")	
#define		KLoginUrl				_T("/beluga/pri?call=login")	
#define		KLogoutByPsssportUrl	_T("/beluga/pri?call=logoutByPsssport")	



//----------------------- other ---------------------------------------------------


#endif		/* __XCONFIG__H__ */
