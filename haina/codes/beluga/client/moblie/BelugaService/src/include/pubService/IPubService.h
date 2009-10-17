// THIS FILE WAS AUTOMATICALLY GENERATED.  DO NOT EDIT.
#ifndef PUBSERVICE_IPUBSERVICE_H
#define PUBSERVICE_IPUBSERVICE_H

#include "hessian/types.h"
#include <string>

namespace pubService {

class IPubService
{
public:
    virtual std::string getLiveWeather(const std::string& param1) = 0;
    virtual std::string get7Weatherdatas(const std::string& param1) = 0;
    virtual std::string getQQStatus(const std::string& param1) = 0;
    virtual std::string getMSNStatus(const std::string& param1) = 0;
    virtual std::string getOrUpdatePD(const std::string& param1) = 0;
    virtual std::string testCN(const std::string& param1) = 0;
    
    virtual ~IPubService ()
    { }
};

}//namespace pubService
#endif
