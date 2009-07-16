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


/*
vector<WeatherDto> CacNetEngine::get7WeatherDatas(string aCityCode)
{
	return ;
}
vector<PhoneDistrictDto> CacNetEngine::getOrUpdatePD(int aFlag)
{
	return ;
}
*/




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