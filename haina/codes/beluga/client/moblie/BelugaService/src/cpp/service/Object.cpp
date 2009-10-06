// THIS FILE WAS AUTOMATICALLY GENERATED.  DO NOT EDIT.
#include "service/Object.h"

namespace service {


hessian::HessianInputStream&
operator>> (hessian::HessianInputStream& in, Object& object)
{
    in.beginObject();
    while (in.peek() != 'z') {
        std::string key;
        in >> key;
    }
    in.endObject();
    return in;
}

hessian::HessianOutputStream&
operator<< (hessian::HessianOutputStream& out, const Object& object)
{
    static char TYPE_NAME[] = "java.lang.Object";
    out.beginObject(TYPE_NAME, sizeof(TYPE_NAME) - 1);
    out.endObject();
    return out;
}

}//namespace service
