/**
* @author north0808@gmail.com
* @version 1.0
*/
#include "pubService/HessianRemoteReturning.h"
#include "pubService/IPubServiceProxy.h"
#include <string>
#include "hessian/Call.h"

namespace pubService {
    
pubService::HessianRemoteReturning
IPubServiceProxy::getLiveWeather (const std::string& param1)
{
    static char METHOD_NAME[] = "getLiveWeather";
    hessian::Call<pubService::HessianRemoteReturning > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

pubService::HessianRemoteReturning
IPubServiceProxy::get7Weatherdatas (const std::string& param1)
{
    static char METHOD_NAME[] = "get7Weatherdatas";
    hessian::Call<pubService::HessianRemoteReturning > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

pubService::HessianRemoteReturning
IPubServiceProxy::getQQStatus (const std::string& param1)
{
    static char METHOD_NAME[] = "getQQStatus";
    hessian::Call<pubService::HessianRemoteReturning > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

pubService::HessianRemoteReturning
IPubServiceProxy::getMSNStatus (const std::string& param1)
{
    static char METHOD_NAME[] = "getMSNStatus";
    hessian::Call<pubService::HessianRemoteReturning > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

pubService::HessianRemoteReturning
IPubServiceProxy::getOrUpdatePD (hessian::Int param1, hessian::Int param2, hessian::Int param3)
{
    static char METHOD_NAME[] = "getOrUpdatePD";
    hessian::Call<pubService::HessianRemoteReturning > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    call << param2;
    call << param3;
    return call.invoke(connection_);
}

pubService::HessianRemoteReturning
IPubServiceProxy::getOrUpdatePDCount (hessian::Int param1)
{
    static char METHOD_NAME[] = "getOrUpdatePDCount";
    hessian::Call<pubService::HessianRemoteReturning > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

pubService::HessianRemoteReturning
IPubServiceProxy::testCN (const std::string& param1)
{
    static char METHOD_NAME[] = "testCN";
    hessian::Call<pubService::HessianRemoteReturning > call(METHOD_NAME, sizeof(METHOD_NAME) - 1);
    call << param1;
    return call.invoke(connection_);
}

}//namespace pubService
