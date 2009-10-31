/**
* @author north0808@gmail.com
* @version 1.0
*/
#ifndef PUBSERVICE_IPUBSERVICEPROXY_H
#define PUBSERVICE_IPUBSERVICEPROXY_H

#include "hessian/types.h"
#include "pubService/HessianRemoteReturning.h"
#include "pubService/IPubService.h"
#include <string>
#include "hessian/Connection.h"
#include "BelugaNet.h"

namespace pubService {

class BELUGANET_API IPubServiceProxy: public IPubService
{
protected:
    hessian::Connection& connection_;
    
public:
    IPubServiceProxy (hessian::Connection& connection):
        connection_(connection)
    { }
    
    pubService::HessianRemoteReturning getLiveWeather(const std::string& param1);
    pubService::HessianRemoteReturning get7Weatherdatas(const std::string& param1);
    pubService::HessianRemoteReturning getQQStatus(const std::string& param1);
    pubService::HessianRemoteReturning getOrUpdatePD(hessian::Int param1, hessian::Int param2, hessian::Int param3);
    pubService::HessianRemoteReturning getOrUpdatePDCount(hessian::Int param1);
    pubService::HessianRemoteReturning testCN(const std::string& param1);
};

}//namespace pubService
#endif
