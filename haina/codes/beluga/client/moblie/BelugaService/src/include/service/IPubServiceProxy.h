/**
* Author:JackyHo.dev@gmail.com
* Date:2009.9.30
*/
#ifndef SERVICE_IPUBSERVICEPROXY_H
#define SERVICE_IPUBSERVICEPROXY_H

#include "hessian/types.h"
#include "service/HessianRemoteReturning.h"
#include "service/IPubService.h"
#include <string>
#include "hessian/Connection.h"
#include "BelugaService.h"

namespace service {

class BELUGASERVICE_API IPubServiceProxy: public IPubService
{
protected:
    hessian::Connection& connection_;
    
public:
    IPubServiceProxy (hessian::Connection& connection):
        connection_(connection)
    { }
    
    service::HessianRemoteReturning getLiveWeather(const std::string& param1);
    service::HessianRemoteReturning get7Weatherdatas(const std::string& param1);
    service::HessianRemoteReturning getQQStatus(const std::string& param1);
    service::HessianRemoteReturning getMSNStatus(const std::string& param1);
    service::HessianRemoteReturning getOrUpdatePD(const std::string& param1);
    service::HessianRemoteReturning testCN(const std::string& param1);
};

}//namespace service
#endif
