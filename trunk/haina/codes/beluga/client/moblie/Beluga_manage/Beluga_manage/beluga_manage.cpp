
#include <iostream> 
#include <glib.h>
using namespace std;
#include <WeatherDto.h>
#include <PhoneDistrictDto.h>
#include <acNetEngine.h>
#include <myinc.h>
#include "CContactDb.h"
#include "CContactIterator.h"
#include "CPhoneContact.h"
#include "CIMContact.h"
#include "beluga_manage.h"
void  get_fields_name(gpointer data, gpointer user_data);

beluga_manage::beluga_manage(void)
{
	m_passport="";
	pContactDb = new CContactDb();
	if (NULL == pContactDb)
	{
		printf("create contactdb instance error.\n");
		return;
	}
	pContactDb->InitEntityDb("\\Program Files\\beluga_manage_t\\beluga.db"); /* 初始化联系人数据库 */

}

beluga_manage::~beluga_manage(void)
{
	if (!m_passport.empty())
	{
		m_passport.clear();
	}
}
void beluga_manage::GetPhoneContactList(void)
{
	gint32 ret;
	/************************************ 1. 获取PhoneContact 列表 *****************/
	// IMPORT_C gint32 GetAllContactsByTag(guint32 nTagId, gboolean onlyPref, CContactIterator ** ppContactIterator);
	CContactIterator * pContactIterator = NULL;
	ret = pContactDb->GetAllContactsByTag(ContactType_Phone, TRUE, &pContactIterator);
	if (ret != 0)
	{
		printf("get all phone contacts error.\n");
		return;
	}

	CPhoneContact * pContact = NULL;
	gboolean hasNext = FALSE;

	if (ECode_No_Error == pContactIterator->Current((CDbEntity**)&pContact))
	{
		// 打印phone contact 不带扩展通讯信息的各字段名
		GPtrArray * pFieldsName = NULL;
		pContact->GetFieldsName(&pFieldsName);

		g_ptr_array_foreach(pFieldsName, get_fields_name, NULL);  /* foreach callback */
		freeGStringArray(pFieldsName); 

	}
	printf("\n");

	// 输出 phone contact 不带扩展通讯信息的各字段值
	GString * pFieldValue = NULL;
	for (int i=ContactField_Id; i<ContactField_EndFlag; i++)
	{
		pContact->GetFieldValue(i, &pFieldValue);
		printf(" %s", pFieldValue->str);
		g_string_free(pFieldValue, TRUE);
	}
	printf("\n");

	delete pContact;
	pContact = NULL;

	while (0 == pContactIterator->Next(&hasNext) && hasNext)
	{

		if (ECode_No_Error == pContactIterator->Current((CDbEntity**)&pContact))
		{
			GString * pFieldValue = NULL;
			for (int i=ContactField_Id; i<ContactField_EndFlag; i++)
			{
				pContact->GetFieldValue(i, &pFieldValue);
				printf(" %s", pFieldValue->str);
				g_string_free(pFieldValue, TRUE);
			}
			printf("\n");
		}

		delete pContact;
		pContact = NULL;
	}

	delete pContactIterator;
	pContactIterator = NULL;
}
typedef enum QQStatus{ONLINE=10000,OFFLINE,ERROR};
static string QQStatusStr[3]={"online","offline","error"};
static beluga_manage *m_pbm=NULL;

//string beluga_manage::GetQQStatusByID(string qqId)
//{
//	
//	int iResult=m_ane.getQQStatus(qqId);
//	if (iResult!=ONLINE && iResult!=OFFLINE)
//	{
//		iResult=ERROR;
//	}
//	return QQStatusStr[iResult-ONLINE];
//}
bool beluga_manage::setNetHost(LPCTSTR aHostName,int aHttp_Port)
{
		//m_pbm=new beluga_manage();
		return m_ane.setNetHost(aHostName,aHttp_Port);
}
//
//WeatherInfo beluga_manage::GetLiveWeatherByCityCode(string cityCode)
//{
//	WeatherInfo wi={0};
//	WeatherDto *wd=m_ane.getLiveWeather(cityCode);
//
//	wi.date=wd->getDate();
//	wi.high=wd->getHigh();
//	wi.icon=wd->getIcon();
//	wi.issuetime=wd->getIcon();
//	wi.low=wd->getLow();
//	wi.temperature=wd->getTemperature();
//	wi.weatherCityCode=wd->getWeatherCityCode();
//	wi.weatherType=wd->getWeatherType();
//	wi.wind=wd->getWind();
//	return wi;
//}
//WeatherInfo* beluga_manage::Get7WeatherDatas(string cityCode)
//{
//	GPtrArray* weather7_list = m_ane.get7WeatherDatas(cityCode);
//	WeatherInfo * wi=new WeatherInfo[7];
//	WeatherInfo * pwi=wi;
//	if(weather7_list == NULL)
//	{
//		return NULL;
//	}
//	for (int i=0;i<weather7_list->len;i++,pwi++)
//	{
//		WeatherDto* wd = (WeatherDto*)g_ptr_array_index(weather7_list,i);
//		pwi->date=wd->getDate();
//		pwi->high=wd->getHigh();
//		pwi->icon=wd->getIcon();
//		pwi->issuetime=wd->getIcon();
//		pwi->low=wd->getLow();
//		pwi->temperature=wd->getTemperature();
//		pwi->weatherCityCode=wd->getWeatherCityCode();
//		pwi->weatherType=wd->getWeatherType();
//		pwi->wind=wd->getWind();
//	}
//	freeGPtrArray(weather7_list);
//	return wi;
//}
bool beluga_manage::Register(string loginName, string password, string mobile)
{
	m_passport= m_ane.registerx(loginName,password,mobile);

	return !m_passport.empty();
}
//bool beluga_manage::Login(string loginName, string password)
//{
//	m_passport= m_ane.login(loginName,password);
//	return !m_passport.empty();
//}
bool beluga_manage::Logout(void)
{
	return m_ane.logoutByPsssport(m_passport);
}
//int beluga_manage::GetErrCode(void)
//{
//	return m_ane.getErrCode();
//}
//bool beluga_manage::GetOrUpdatePD(string updatePD)
//{
//	GPtrArray* pd_list = m_ane.getOrUpdatePD(updatePD/*"1"*/);
//	if(pd_list == NULL)
//	{
//		return false;
//	}
//	for(int i = 0; i < pd_list->len; i++)
//	{
//		PhoneDistrictDto* phoneDistrict = (PhoneDistrictDto*)g_ptr_array_index(pd_list,i);
//		cout << "========== " << i << " ==========" << endl;
//		cout << "DistrictCity:\t" << phoneDistrict->getDistrictCity() << endl;
//		cout << "DistrictNumber:\t" << phoneDistrict->getDistrictNumber() << endl;
//		cout << "DistrictProvince:\t" << phoneDistrict->getDistrictProvince() << endl;
//		cout << "WeatherCityCode:\t" << phoneDistrict->getWeatherCityCode() << endl;
//		// 		cout << "Range:\t" << phoneDistrict->getRange() << endl;
//		cout << "FeeType:\t" << phoneDistrict->getFeeType() << endl;
//		cout << "UpdateFlg:\t" << phoneDistrict->getUpdateFlg() << endl;
//		cout << endl;
//	}
//	freeGPtrArray(pd_list);
//	return true;
//}
void  get_fields_name(gpointer data, gpointer user_data)
{
	GString * pFieldName = (GString*)data;
	printf(" %s", pFieldName->str);
}