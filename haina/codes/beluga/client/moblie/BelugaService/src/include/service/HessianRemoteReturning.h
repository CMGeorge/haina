#ifndef SERVICE_HESSIANREMOTERETURNING_H
#define SERVICE_HESSIANREMOTERETURNING_H

#include "hessian/types.h"
#include "service/Object.h"
#include <string>
#include "hessian/HessianInputStream.h"
#include "hessian/HessianOutputStream.h"

namespace service {

struct HessianRemoteReturning
{
    hessian::Int statusCode;
    std::string statusText;
    hessian::Int operationCode;
    std::string value;
};

hessian::HessianInputStream& operator>>(hessian::HessianInputStream& in, HessianRemoteReturning& object);
hessian::HessianOutputStream& operator<<(hessian::HessianOutputStream& out, const HessianRemoteReturning& object);

}//namespace service
#endif
