/**
* Author:JackyHo.dev@gmail.com
* Date:2009.9.30
*/
#ifndef SERVICE_IPUBSERVICE_H
#define SERVICE_IPUBSERVICE_H

#include "hessian/types.h"
#include "service/HessianRemoteReturning.h"
#include <string>

namespace service {

class IPubService
{
public:
    virtual service::HessianRemoteReturning getLiveWeather(const std::string& param1) = 0;
    virtual service::HessianRemoteReturning get7Weatherdatas(const std::string& param1) = 0;
    virtual service::HessianRemoteReturning getQQStatus(const std::string& param1) = 0;
    virtual service::HessianRemoteReturning getMSNStatus(const std::string& param1) = 0;
    virtual service::HessianRemoteReturning getOrUpdatePD(const std::string& param1) = 0;
    virtual service::HessianRemoteReturning testCN(const std::string& param1) = 0;
    
    virtual ~IPubService ()
    { }
};

}//namespace service
#endif
