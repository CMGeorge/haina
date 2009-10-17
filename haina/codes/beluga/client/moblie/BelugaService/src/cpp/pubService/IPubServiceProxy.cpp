// THIS FILE WAS AUTOMATICALLY GENERATED.  DO NOT EDIT.
#include "pubService/IPubServiceProxy.h"
#include <string>
#include "hessian/Call.h"

namespace pubService {
    
std::string
IPubServiceProxy::getLiveWeather (const std::string& param1)
{
    static char METHOD_NAME[] = "getLiveWeather";
    hessian::Call<std::string > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

std::string
IPubServiceProxy::get7Weatherdatas (const std::string& param1)
{
    static char METHOD_NAME[] = "get7Weatherdatas";
    hessian::Call<std::string > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

std::string
IPubServiceProxy::getQQStatus (const std::string& param1)
{
    static char METHOD_NAME[] = "getQQStatus";
    hessian::Call<std::string > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

std::string
IPubServiceProxy::getMSNStatus (const std::string& param1)
{
    static char METHOD_NAME[] = "getMSNStatus";
    hessian::Call<std::string > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

std::string
IPubServiceProxy::getOrUpdatePD (const std::string& param1)
{
    static char METHOD_NAME[] = "getOrUpdatePD";
    hessian::Call<std::string > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

std::string
IPubServiceProxy::testCN (const std::string& param1)
{
    static char METHOD_NAME[] = "testCN";
    hessian::Call<std::string > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

}//namespace pubService
