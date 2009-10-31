/**
* @author north0808@gmail.com
* @version 1.0
*/
#include "beluganettest.h"
//user add
#include <hessian/HttpConnection.h>//hessian
#include <pubService/IPubServiceProxy.h>//hessian
#include <dto/WeatherDto.h>//hessian
#include <json/json.h>//json
#include <windows.h>//cursor
#include <contact/CContactDb.h>//db
#include <sstream>

using namespace std;
using namespace hessian;
using namespace pubService;
using namespace dto;

string URL("http://61.129.70.8:8080/beluga/pub");

BelugaNetTest::BelugaNetTest(QWidget *parent, Qt::WFlags flags)
: QMainWindow(parent, flags)
{
	ui.setupUi(this);
	QObject::connect(ui.pushButtonQQSubmit,SIGNAL(clicked()),this,SLOT(pushButtonQQSubmitClick()));
	QObject::connect(ui.pushButtonCitySubmit,SIGNAL(clicked()),this,SLOT(pushButtonCitySubmitClick()));
	QObject::connect(ui.pushButtonWeatherSubmit,SIGNAL(clicked()),this,SLOT(pushButtonWeatherSubmitClick()));
}

BelugaNetTest::~BelugaNetTest()
{

}

void BelugaNetTest::pushButtonQQSubmitClick()
{
	SetCursor(LoadCursor(NULL,IDC_WAIT)); //设定鼠标忙碌状态
	ui.labelQQStatus->setText("checking...");
	string qq(ui.lineEditQQ->text().toAscii());
	HttpConnection connection(URL);
	IPubServiceProxy *service = new IPubServiceProxy(connection);
	try
	{
		QApplication::processEvents();
		//在hessian中传输含有中文的字符串时要以utf8编码方式传输
		HessianRemoteReturning ret = service->getQQStatus(qq);//默认30秒超时
		/*
		* 状态包括：
		* 1.成功 statusCode=0
		* 2.QQ参数错误 statusCode = 10002
		* 3.网络错误 statusCode= 10003 
		* 
		* value值为：
		* 1.在线 value=10000
		* 2.不在线 value=10001
		*/
		if(ret.statusCode == 0&&ret.value.compare("10000")==0)
		{
			ui.labelQQStatus->setText("online");
		}
		else
		{
			ui.labelQQStatus->setText("offline or error");
		}
	}
	catch(...)
	{
		//30秒超时，网络问题
		ui.labelQQStatus->setText("problem");
	}
	SetCursor(NULL);//恢复鼠标状态	
}


void BelugaNetTest::pushButtonWeatherSubmitClick()
{
	SetCursor(LoadCursor(NULL,IDC_WAIT)); //设定鼠标忙碌状态
	ui.plainTextEditCity->setPlainText("checking...");
	string weatherCityCode(ui.lineEditWeatherCityCode->text().toUtf8());
	string url(URL);
	HttpConnection connection(url);
	IPubServiceProxy *service = new IPubServiceProxy(connection);
	try
	{
		QApplication::processEvents();
		//在hessian中传输含有中文的字符串时要以utf8编码方式传输
		HessianRemoteReturning ret = service->getLiveWeather(weatherCityCode);//默认30秒超时
		/**
		* 状态包括：
		* 1.成功 statusCode=0
		* 2.网络异常没有数据 statusCode = 10004
		* 
		* value值为：
		* WeatherDto对象.
		* 
		*/
		if(ret.statusCode == 0)
		{
			QString str = QString::fromUtf8(ret.value.c_str());
			Json::Reader reader;
			Json::Value jsonValue;
			WeatherDto weatherDto;
			if(reader.parse(string(str.toAscii()),jsonValue))
			{
				/*
				按顺序读取
				date:天气日期
				high:最高气温
				icon:天气图片URI
				issuetime:发布时间
				low:最低气温
				temperature:实时温度
				weatherCityCode:城市代码
				weatherType:天气类型
				wind:风速
				*/
				weatherDto.setDate(jsonValue["date"].asString());
				weatherDto.setHigh(jsonValue["high"].asInt());
				weatherDto.setIcon(jsonValue["icon"].asString());
				weatherDto.setIssuetime(jsonValue["issuetime"].asString());
				weatherDto.setLow(jsonValue["low"].asInt());
				weatherDto.setTemperature(jsonValue["temperature"].asString());
				weatherDto.setWeatherCityCode(jsonValue["weatherCityCode"].asString());
				weatherDto.setWeatherType(jsonValue["weatherType"].asString());
				weatherDto.setWind(jsonValue["wind"].asString());
			}
			char msg[256] = {0};
			sprintf(msg,"Date:%s\nHigh:%d\nIcon:%s\nIssuetime:%s\nLow:%d\nTemperature:%s\nWeatherCityCode:%s\nWeatherType:%s\nWind:%s"
				, weatherDto.getDate().c_str(), weatherDto.getHigh(), weatherDto.getIcon().c_str(), weatherDto.getIssuetime().c_str(), weatherDto.getLow(), weatherDto.getTemperature().c_str(), weatherDto.getWeatherCityCode().c_str(), weatherDto.getWeatherType().c_str(), weatherDto.getWind().c_str());
			ui.plainTextEditCity->setPlainText(QString(msg));
		}
		else
		{
			ui.plainTextEditCity->setPlainText("failure");
		}
	}
	catch(...)
	{
		//30秒超时，网络问题
		ui.plainTextEditCity->setPlainText("problem");
	}
	SetCursor(NULL);//恢复鼠标状态	
}


void BelugaNetTest::pushButtonCitySubmitClick()
{
	SetCursor(LoadCursor(NULL,IDC_WAIT)); //设定鼠标忙碌状态
	ui.plainTextEditCity->setPlainText("checking...");
	HttpConnection connection(URL);
	IPubServiceProxy *service = new IPubServiceProxy(connection);
	try
	{
		QApplication::processEvents();
		//读取本地数据库城市信息总记录条数
		CContactDb *contactDb = new CContactDb();
		if (NULL == contactDb)
		{
			printf("Create contact db instance error.\n");
			return;
		}
		contactDb->InitEntityDb("\\Program Files\\Beluga\\beluga.db");
		guint16 updateFlag(0);
		try
		{
			contactDb->GetMaxPhoneDistrictUpdateFlag(&updateFlag);
		}
		catch(...){}
		if(updateFlag<0)
		{
			SetCursor(NULL);//恢复鼠标状态	
			ui.plainTextEditCity->setPlainText("local db read error");
			return;
		}
		updateFlag = 1;
		//int to string
		stringstream ss;
		ss << updateFlag;
		//在hessian中传输含有中文的字符串时要以utf8编码方式传输
		HessianRemoteReturning ret = service->getOrUpdatePD(ss.str());//默认30秒超时
		/**
		* 状态包括：
		* 1.成功 statusCode=0
		* 
		* value值为：
		* List集合，集合对象为PhoneDistrictDto.
		* 
		*/
		if(ret.statusCode == 0)
		{
			QString str = QString::fromUtf8(ret.value.c_str());
			Json::Reader reader;
			Json::Value jsonValue;
			WeatherDto weatherDto;
			if(reader.parse(string(str.toAscii()),jsonValue))
			{
				if(jsonValue.isArray())
				{
					int size = jsonValue.size();
					if(size<=0)
					{
						ui.plainTextEditCity->setPlainText("server response null");
					}
					for (int i = 0;i < size;i++)
					{
						Json::Value jsonValueTmp = jsonValue[i];
						stPhoneDistrict phoneDistrict;
						/*
						按顺序读取
						districtCity:城市
						districtNumber:区号
						districtProvince:省份
						feeType:资费类型
						range:手机前7位范围
						updateFlg:更新标志
						weatherCityCode:天气代码
						*/
						strcpy(phoneDistrict.districtName, jsonValueTmp["districtCity"].asString().c_str());
						strcpy(phoneDistrict.districtNumber, jsonValueTmp["districtNumber"].asString().c_str());
						strcpy(phoneDistrict.ownerState, jsonValueTmp["districtProvince"].asString().c_str());
						strcpy(phoneDistrict.feeType, jsonValueTmp["feeType"].asString().c_str());
						phoneDistrict.phoneRange = g_string_new(jsonValueTmp["range"].asString().c_str());
						phoneDistrict.updateFlag = jsonValueTmp["updateFlg"].asInt();
						strcpy(phoneDistrict.weatherCode, jsonValueTmp["weatherCityCode"].asString().c_str());
						
						contactDb->SavePhoneDistrict(&phoneDistrict);
						char msg[50]={0};
						sprintf(msg,"add:%d/%d",i+1,size);
						ui.plainTextEditCity->setPlainText(msg);
						QApplication::processEvents();
					}
				}
				else
				{
					ui.plainTextEditCity->setPlainText("server response is not an array");
				}
			}
			else
			{
				ui.plainTextEditCity->setPlainText("json parse error");
			}
		}
		else
		{
			ui.plainTextEditCity->setPlainText("failure");
		}
	}
	catch(...)
	{
		//30秒超时，网络问题
		ui.plainTextEditCity->setPlainText("problem");
	}
	SetCursor(NULL);//恢复鼠标状态	
}
