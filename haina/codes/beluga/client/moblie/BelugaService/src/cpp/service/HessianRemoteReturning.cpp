#include "service/HessianRemoteReturning.h"

namespace service {

const std::string KEY_statusCode("statusCode");
const std::string KEY_statusText("statusText");
const std::string KEY_operationCode("operationCode");
const std::string KEY_value("value");

hessian::HessianInputStream&
operator>> (hessian::HessianInputStream& in, HessianRemoteReturning& object)
{
    in.beginObject();
    while (in.peek() != 'z') {
        std::string key;
        in >> key;
        
        if (key == KEY_statusCode) {
            in >> object.statusCode;
        } else if (key == KEY_statusText) {
            in >> object.statusText;
        } else if (key == KEY_operationCode) {
            in >> object.operationCode;
        } else if (key == KEY_value) {
            in >> object.value;
        } else {
            in.throwUnknownPropertyException(key);
        }
    }
    in.endObject();
    return in;
}

hessian::HessianOutputStream&
operator<< (hessian::HessianOutputStream& out, const HessianRemoteReturning& object)
{
    static char TYPE_NAME[] = "com.haina.beluga.webservice.data.hessian.HessianRemoteReturning";
    out.beginObject(TYPE_NAME, sizeof(TYPE_NAME) - 1);
    out << KEY_statusCode << object.statusCode;
    out << KEY_statusText << object.statusText;
    out << KEY_operationCode << object.operationCode;
    out << KEY_value << object.value;
    out.endObject();
    return out;
}

}//namespace service
