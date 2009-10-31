/**
* @author north0808@gmail.com
* @version 1.0
*/
#ifndef PUBSERVICE_IPUBSERVICE_H
#define PUBSERVICE_IPUBSERVICE_H

#include "hessian/types.h"
#include "pubService/HessianRemoteReturning.h"
#include <string>

namespace pubService {

class IPubService
{
public:
    virtual pubService::HessianRemoteReturning getLiveWeather(const std::string& param1) = 0;
    virtual pubService::HessianRemoteReturning get7Weatherdatas(const std::string& param1) = 0;
    virtual pubService::HessianRemoteReturning getQQStatus(const std::string& param1) = 0;
    virtual pubService::HessianRemoteReturning getOrUpdatePD(const std::string& param1) = 0;
    virtual pubService::HessianRemoteReturning testCN(const std::string& param1) = 0;
    
    virtual ~IPubService ()
    { }
};

}//namespace pubService
#endif
