#include "acNetEngine.h"

#include "xconfig.h"

using namespace hessian;



CacNetEngine::CacNetEngine()
{

}

CacNetEngine::CacNetEngine(LPCTSTR aHostName,int aHttp_Port)
{
	setNetHost(aHostName,aHttp_Port);
}

CacNetEngine::~CacNetEngine()
{

}

/************************************************************************/
// ������setNetHost
// ���ܣ����÷���������������IP��ַ
// ������aHostName��������������IP
// ������aHttp_Port�����ʶ˿ڣ�Ĭ��Ϊ80�˿�
// ���أ��޷���
/************************************************************************/
void CacNetEngine::setNetHost(LPCTSTR aHostName,int aHttp_Port)
{
	iHttpNet.SetHostName(aHostName,aHttp_Port);
}


/************************************************************************/
// ������getQQStatus
// ���ܣ���ȡQQ����״̬
// ������QQ����
// ���أ�����(10000) ����(10001) ����(-1)
/************************************************************************/
int CacNetEngine::getQQStatus(string aQQId)
{
	iSendHes.clear();
 	ihes_output.write_string(iSendHes,aQQId);
 	string retData = iHttpNet.SyncPostData(KGetQQStatusUrl,iSendHes);

	Json::Value jsonValue;
	string retstring = getHessianString(retData);
	HessianRemoteReturning hes_return = parse_json(retstring,jsonValue);

	if(hes_return.getStatusCode() != 0)
		return -1;

	int online = hes_return.getValue().asInt();
	return online;
}


/************************************************************************/
// ������getLiveWeather
// ���ܣ���ȡ��������������
// �����������ر��еĳ���ID
// ���أ��������ݣ�WeatherDto��
/************************************************************************/
WeatherDto CacNetEngine::getLiveWeather(string aCityCode)
{
	WeatherDto weatherDto;

	iSendHes.clear();
	ihes_output.write_string(iSendHes,aCityCode);
	string retData = iHttpNet.SyncPostData(KGetLiveWeatherUrl,iSendHes);

	Json::Value jsonValue;
	string retstring = getHessianString(retData);
	HessianRemoteReturning hes_return = parse_json(retstring,jsonValue);

	if(hes_return.getStatusCode() != 0)
	{

	}
	else
	{
		Json::Value jVal = hes_return.getValue();
		weatherDto.setDate(jVal["date"].asString());
		weatherDto.setHigh(jVal["high"].asInt());
		weatherDto.setIcon(jVal["icon"].asString());
		weatherDto.setIssuetime(jVal["issuetime"].asString());
		weatherDto.setLow(jVal["low"].asInt());
		weatherDto.setTemperature(jVal["temperature"].asString());
		weatherDto.setWeatherCityCode(jVal["weatherCityCode"].asString());
		weatherDto.setWeatherType(jVal["weatherType"].asString());
		weatherDto.setWind(jVal["wind"].asString());
	}

	return weatherDto;
}


/************************************************************************/
// ������get7WeatherDatas
// ���ܣ���ȡδ��7�����������
// �����������ر��еĳ���ID
// ���أ������������飨GPtrArray��
/************************************************************************/
GPtrArray* CacNetEngine::get7WeatherDatas(string aCityCode)
{
	GPtrArray* weather7_list = g_ptr_array_new();

	iSendHes.clear();
	ihes_output.write_string(iSendHes,aCityCode);
	string retData = iHttpNet.SyncPostData(KGet7WeatherUrl,iSendHes);

	Json::Value jsonValue;
	string retstring = getHessianString(retData);
	HessianRemoteReturning hes_return = parse_json(retstring,jsonValue);

	if(hes_return.getStatusCode() != 0)
	{
		
	}
	else
	{
		Json::Value jVal = hes_return.getValue();
		if(jVal.isArray())
		{
			int valSize = jVal.size();
			for (int i = 0;i < valSize;i++)
			{
				WeatherDto* weatherDto = new WeatherDto();
				weatherDto->setDate(jVal[i]["date"].asString());
				weatherDto->setHigh(jVal[i]["high"].asInt());
				weatherDto->setIcon(jVal[i]["icon"].asString());
				weatherDto->setIssuetime(jVal[i]["issuetime"].asString());
				weatherDto->setLow(jVal[i]["low"].asInt());
				weatherDto->setTemperature(jVal[i]["temperature"].asString());
				weatherDto->setWeatherCityCode(jVal[i]["weatherCityCode"].asString());
				weatherDto->setWeatherType(jVal[i]["weatherType"].asString());
				weatherDto->setWind(jVal[i]["wind"].asString());

				g_ptr_array_add(weather7_list,weatherDto);
			}
		}
	}
	
	return weather7_list;
}


/************************************************************************/
// ������getOrUpdatePD
// ���ܣ���ȡ����������
// ��������ʶλ����
// ���أ��������������飨GPtrArray��
/************************************************************************/
GPtrArray* CacNetEngine::getOrUpdatePD(string aFlag)
{
	GPtrArray* pd_list = g_ptr_array_new();

	iSendHes.clear();
	ihes_output.write_string(iSendHes,aFlag);
	string retData = iHttpNet.SyncPostData(KGetOrUpdatePDUrl,iSendHes);

	Json::Value jsonValue;
	string retstring = getHessianString(retData);
	HessianRemoteReturning hes_return = parse_json(retstring,jsonValue);

	if(hes_return.getStatusCode() != 0)
	{

	}
	else
	{
		Json::Value jVal = hes_return.getValue();
		if(jVal.isArray())
		{
			int valSize = jVal.size();
			for (int i = 0;i < valSize;i++)
			{
				PhoneDistrictDto* phoneDistrict = new PhoneDistrictDto();


// 				WeatherDto* weatherDto = new WeatherDto();
// 				weatherDto->setDate(jVal[i]["date"].asString());
// 				weatherDto->setHigh(jVal[i]["high"].asInt());
// 				weatherDto->setIcon(jVal[i]["icon"].asString());
// 				weatherDto->setIssuetime(jVal[i]["issuetime"].asString());
// 				weatherDto->setLow(jVal[i]["low"].asInt());
// 				weatherDto->setTemperature(jVal[i]["temperature"].asString());
// 				weatherDto->setWeatherCityCode(jVal[i]["weatherCityCode"].asString());
// 				weatherDto->setWeatherType(jVal[i]["weatherType"].asString());
// 				weatherDto->setWind(jVal[i]["wind"].asString());

				g_ptr_array_add(pd_list,phoneDistrict);
			}
		}
	}

	return pd_list;
}




/************************************************************************/
// ������getHessianString
// ���ܣ�ת��Hessian��ʽ������Ϊ�����ַ���(Json��ʽ)
// ������Hessian��ʽ����
// ���أ������ַ���(Json��ʽ)
/************************************************************************/
string CacNetEngine::getHessianString(string aHessianStr)
{
	auto_ptr<input_stream> i_stream(new string_input_stream(aHessianStr));
	hessian_input hes_in(i_stream);
	return hes_in.read_string();
}


/************************************************************************/
// ������parse_json
// ���ܣ�����Json��ʽ�ַ�����ת��HessianRemoteRutrning����
// ������aJson_string��Json��ʽ�ַ���	
// ������jsonValue���������Json����
// ���أ������ַ���(Json��ʽ)
/************************************************************************/
HessianRemoteReturning CacNetEngine::parse_json(string& aJson_string,Json::Value& jsonValue)
{
	HessianRemoteReturning remoteRet;

	Json::Reader	reader;
	if(reader.parse(aJson_string,jsonValue))
	{
 		remoteRet.setStatusCode(jsonValue["statusCode"].asInt());
 		remoteRet.setStatusText(jsonValue["statusText"].asString());
 		remoteRet.setOperationCode(jsonValue["operationCode"].asInt());
//		remoteRet.setValue(&(jsonValue["value"]));
 		remoteRet.setValue(jsonValue["value"]);
	}
	return remoteRet;
}