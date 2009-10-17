// THIS FILE WAS AUTOMATICALLY GENERATED.  DO NOT EDIT.
#ifndef PUBSERVICE_IPUBSERVICEPROXY_H
#define PUBSERVICE_IPUBSERVICEPROXY_H

#include "hessian/types.h"
#include "pubService/IPubService.h"
#include <string>
#include "hessian/Connection.h"
#include "BelugaService.h"

namespace pubService {

class BELUGASERVICE_API IPubServiceProxy: public IPubService
{
protected:
    hessian::Connection& connection_;
    
public:
    IPubServiceProxy (hessian::Connection& connection):
        connection_(connection)
    { }
    
    std::string getLiveWeather(const std::string& param1);
    std::string get7Weatherdatas(const std::string& param1);
    std::string getQQStatus(const std::string& param1);
    std::string getMSNStatus(const std::string& param1);
    std::string getOrUpdatePD(const std::string& param1);
    std::string testCN(const std::string& param1);
};

}//namespace pubService
#endif
