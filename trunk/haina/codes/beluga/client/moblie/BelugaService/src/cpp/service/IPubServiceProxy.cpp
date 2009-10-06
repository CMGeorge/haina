/**
* Author:JackyHo.dev@gmail.com
* Date:2009.9.30
*/
#include "service/HessianRemoteReturning.h"
#include "service/IPubServiceProxy.h"
#include <string>
#include "hessian/Call.h"

namespace service {
    
service::HessianRemoteReturning
IPubServiceProxy::getLiveWeather (const std::string& param1)
{
    static char METHOD_NAME[] = "getLiveWeather";
    hessian::Call<service::HessianRemoteReturning > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

service::HessianRemoteReturning
IPubServiceProxy::get7Weatherdatas (const std::string& param1)
{
    static char METHOD_NAME[] = "get7Weatherdatas";
    hessian::Call<service::HessianRemoteReturning > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

service::HessianRemoteReturning
IPubServiceProxy::getQQStatus (const std::string& param1)
{
    static char METHOD_NAME[] = "getQQStatus";
    hessian::Call<service::HessianRemoteReturning > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

service::HessianRemoteReturning
IPubServiceProxy::getMSNStatus (const std::string& param1)
{
    static char METHOD_NAME[] = "getMSNStatus";
    hessian::Call<service::HessianRemoteReturning > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

service::HessianRemoteReturning
IPubServiceProxy::getOrUpdatePD (const std::string& param1)
{
    static char METHOD_NAME[] = "getOrUpdatePD";
    hessian::Call<service::HessianRemoteReturning > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

service::HessianRemoteReturning
IPubServiceProxy::testCN (const std::string& param1)
{
    static char METHOD_NAME[] = "testCN";
    hessian::Call<service::HessianRemoteReturning > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

}//namespace service
