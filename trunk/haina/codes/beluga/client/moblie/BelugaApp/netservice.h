/* 
*  this class encapsulate the server api
*/

#ifndef NETSERVICE_H
#define NETSERVICE_H

#include <hessian/HttpConnection.h> 
#include <pubService/IPubServiceProxy.h> 
#include <dto/WeatherDto.h> 
#include <json/json.h> 
#include <sstream>
#include <glib.h>

using namespace std;
using namespace hessian;
using namespace pubService;
using namespace dto;


class NetService 
{
	public:
	/* read contact read contact from mobile platform */
		NetService();
		~NetService();
		
		void setServerURL(string & url);
		gint32 getQQStatus(GString * qq);
		WeatherDto * getWeather(GString * cityCode);
		gboolean updatePhoneDistrict(guint32 updateFlag);

	private:
		string m_serverUrl;
};


#endif



