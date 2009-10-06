#ifndef SERVICE_OBJECT_H
#define SERVICE_OBJECT_H

#include "hessian/types.h"
#include "hessian/HessianInputStream.h"
#include "hessian/HessianOutputStream.h"

namespace service {

struct Object
{
    std::string toString;
};

hessian::HessianInputStream& operator>>(hessian::HessianInputStream& in, Object& object);
hessian::HessianOutputStream& operator<<(hessian::HessianOutputStream& out, const Object& object);

}//namespace service
#endif
