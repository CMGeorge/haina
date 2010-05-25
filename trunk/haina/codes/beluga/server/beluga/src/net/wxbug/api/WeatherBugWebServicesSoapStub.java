/**
 * WeatherBugWebServicesSoapStub.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package net.wxbug.api;

public class WeatherBugWebServicesSoapStub extends org.apache.axis.client.Stub implements net.wxbug.api.WeatherBugWebServicesSoap {
    private java.util.Vector cachedSerClasses = new java.util.Vector();
    private java.util.Vector cachedSerQNames = new java.util.Vector();
    private java.util.Vector cachedSerFactories = new java.util.Vector();
    private java.util.Vector cachedDeserFactories = new java.util.Vector();

    static org.apache.axis.description.OperationDesc [] _operations;

    static {
        _operations = new org.apache.axis.description.OperationDesc[16];
        _initOperationDesc1();
        _initOperationDesc2();
    }

    private static void _initOperationDesc1(){
        org.apache.axis.description.OperationDesc oper;
        org.apache.axis.description.ParameterDesc param;
        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetLocationList");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "searchString"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "ArrayOfApiLocationData"));
        oper.setReturnClass(net.wxbug.api.ApiLocationData[].class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLocationListResult"));
        param = oper.getReturnParamDesc();
        param.setItemQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiLocationData"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[0] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetUSWorldCityByLatLong");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "latitude"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "decimal"), java.math.BigDecimal.class, false, false);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "longitude"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "decimal"), java.math.BigDecimal.class, false, false);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "LocationUSWorldCityData"));
        oper.setReturnClass(net.wxbug.api.LocationUSWorldCityData.class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetUSWorldCityByLatLongResult"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[1] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetStationListByCityCode");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "cityCode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "unitType"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://api.wxbug.net/", "UnitType"), net.wxbug.api.UnitType.class, false, false);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "ArrayOfApiStationData"));
        oper.setReturnClass(net.wxbug.api.ApiStationData[].class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetStationListByCityCodeResult"));
        param = oper.getReturnParamDesc();
        param.setItemQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiStationData"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[2] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetStationListByUSZipCode");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "zipCode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "unitType"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://api.wxbug.net/", "UnitType"), net.wxbug.api.UnitType.class, false, false);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "ArrayOfApiStationData"));
        oper.setReturnClass(net.wxbug.api.ApiStationData[].class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetStationListByUSZipCodeResult"));
        param = oper.getReturnParamDesc();
        param.setItemQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiStationData"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[3] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetLiveCompactWeatherByCityCode");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "cityCode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "unittype"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://api.wxbug.net/", "UnitType"), net.wxbug.api.UnitType.class, false, false);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "LiveCompactWeatherData"));
        oper.setReturnClass(net.wxbug.api.LiveCompactWeatherData.class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLiveCompactWeatherByCityCodeResult"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[4] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetLiveCompactWeatherByUSZipCode");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "zipCode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "unittype"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://api.wxbug.net/", "UnitType"), net.wxbug.api.UnitType.class, false, false);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "LiveCompactWeatherData"));
        oper.setReturnClass(net.wxbug.api.LiveCompactWeatherData.class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLiveCompactWeatherByUSZipCodeResult"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[5] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetLiveCompactWeatherByStationID");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "stationid"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "unittype"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://api.wxbug.net/", "UnitType"), net.wxbug.api.UnitType.class, false, false);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "LiveCompactWeatherData"));
        oper.setReturnClass(net.wxbug.api.LiveCompactWeatherData.class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLiveCompactWeatherByStationIDResult"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[6] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetLiveWeatherByCityCode");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "cityCode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "unittype"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://api.wxbug.net/", "UnitType"), net.wxbug.api.UnitType.class, false, false);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "LiveWeatherData"));
        oper.setReturnClass(net.wxbug.api.LiveWeatherData.class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLiveWeatherByCityCodeResult"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[7] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetLiveWeatherByUSZipCode");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "zipCode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "unittype"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://api.wxbug.net/", "UnitType"), net.wxbug.api.UnitType.class, false, false);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "LiveWeatherData"));
        oper.setReturnClass(net.wxbug.api.LiveWeatherData.class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLiveWeatherByUSZipCodeResult"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[8] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetLiveWeatherByStationID");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "stationid"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "unittype"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://api.wxbug.net/", "UnitType"), net.wxbug.api.UnitType.class, false, false);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "LiveWeatherData"));
        oper.setReturnClass(net.wxbug.api.LiveWeatherData.class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLiveWeatherByStationIDResult"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[9] = oper;

    }

    private static void _initOperationDesc2(){
        org.apache.axis.description.OperationDesc oper;
        org.apache.axis.description.ParameterDesc param;
        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetAlertsDataList");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "zipCode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "ArrayOfApiAlertData"));
        oper.setReturnClass(net.wxbug.api.ApiAlertData[].class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetAlertsDataListResult"));
        param = oper.getReturnParamDesc();
        param.setItemQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiAlertData"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[10] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetAlertsIssueData");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "zipCode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiAlertIssueData"));
        oper.setReturnClass(net.wxbug.api.ApiAlertIssueData.class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetAlertsIssueDataResult"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[11] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetForecastIssueDetailsByCityCode");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "cityCode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiForecastIssueData"));
        oper.setReturnClass(net.wxbug.api.ApiForecastIssueData.class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetForecastIssueDetailsByCityCodeResult"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[12] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetForecastIssueDetailsByUSZipCode");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "zipCode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiForecastIssueData"));
        oper.setReturnClass(net.wxbug.api.ApiForecastIssueData.class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetForecastIssueDetailsByUSZipCodeResult"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[13] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetForecastByCityCode");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "cityCode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "unitType"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://api.wxbug.net/", "UnitType"), net.wxbug.api.UnitType.class, false, false);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "ArrayOfApiForecastData"));
        oper.setReturnClass(net.wxbug.api.ApiForecastData[].class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetForecastByCityCodeResult"));
        param = oper.getReturnParamDesc();
        param.setItemQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiForecastData"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[14] = oper;

        oper = new org.apache.axis.description.OperationDesc();
        oper.setName("GetForecastByUSZipCode");
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "zipCode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "unitType"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://api.wxbug.net/", "UnitType"), net.wxbug.api.UnitType.class, false, false);
        oper.addParameter(param);
        param = new org.apache.axis.description.ParameterDesc(new javax.xml.namespace.QName("http://api.wxbug.net/", "ACode"), org.apache.axis.description.ParameterDesc.IN, new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"), java.lang.String.class, false, false);
        param.setOmittable(true);
        oper.addParameter(param);
        oper.setReturnType(new javax.xml.namespace.QName("http://api.wxbug.net/", "ArrayOfApiForecastData"));
        oper.setReturnClass(net.wxbug.api.ApiForecastData[].class);
        oper.setReturnQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetForecastByUSZipCodeResult"));
        param = oper.getReturnParamDesc();
        param.setItemQName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiForecastData"));
        oper.setStyle(org.apache.axis.constants.Style.WRAPPED);
        oper.setUse(org.apache.axis.constants.Use.LITERAL);
        _operations[15] = oper;

    }

    public WeatherBugWebServicesSoapStub() throws org.apache.axis.AxisFault {
         this(null);
    }

    public WeatherBugWebServicesSoapStub(java.net.URL endpointURL, javax.xml.rpc.Service service) throws org.apache.axis.AxisFault {
         this(service);
         super.cachedEndpoint = endpointURL;
    }

    public WeatherBugWebServicesSoapStub(javax.xml.rpc.Service service) throws org.apache.axis.AxisFault {
        if (service == null) {
            super.service = new org.apache.axis.client.Service();
        } else {
            super.service = service;
        }
        ((org.apache.axis.client.Service)super.service).setTypeMappingVersion("1.2");
            java.lang.Class cls;
            javax.xml.namespace.QName qName;
            javax.xml.namespace.QName qName2;
            java.lang.Class beansf = org.apache.axis.encoding.ser.BeanSerializerFactory.class;
            java.lang.Class beandf = org.apache.axis.encoding.ser.BeanDeserializerFactory.class;
            java.lang.Class enumsf = org.apache.axis.encoding.ser.EnumSerializerFactory.class;
            java.lang.Class enumdf = org.apache.axis.encoding.ser.EnumDeserializerFactory.class;
            java.lang.Class arraysf = org.apache.axis.encoding.ser.ArraySerializerFactory.class;
            java.lang.Class arraydf = org.apache.axis.encoding.ser.ArrayDeserializerFactory.class;
            java.lang.Class simplesf = org.apache.axis.encoding.ser.SimpleSerializerFactory.class;
            java.lang.Class simpledf = org.apache.axis.encoding.ser.SimpleDeserializerFactory.class;
            java.lang.Class simplelistsf = org.apache.axis.encoding.ser.SimpleListSerializerFactory.class;
            java.lang.Class simplelistdf = org.apache.axis.encoding.ser.SimpleListDeserializerFactory.class;
            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiAlertData");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.ApiAlertData.class;
            cachedSerClasses.add(cls);
            cachedSerFactories.add(beansf);
            cachedDeserFactories.add(beandf);

            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiAlertIssueData");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.ApiAlertIssueData.class;
            cachedSerClasses.add(cls);
            cachedSerFactories.add(beansf);
            cachedDeserFactories.add(beandf);

            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiForecastData");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.ApiForecastData.class;
            cachedSerClasses.add(cls);
            cachedSerFactories.add(beansf);
            cachedDeserFactories.add(beandf);

            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiForecastIssueData");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.ApiForecastIssueData.class;
            cachedSerClasses.add(cls);
            cachedSerFactories.add(beansf);
            cachedDeserFactories.add(beandf);

            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiLocationData");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.ApiLocationData.class;
            cachedSerClasses.add(cls);
            cachedSerFactories.add(beansf);
            cachedDeserFactories.add(beandf);

            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiStationData");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.ApiStationData.class;
            cachedSerClasses.add(cls);
            cachedSerFactories.add(beansf);
            cachedDeserFactories.add(beandf);

            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ArrayOfApiAlertData");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.ApiAlertData[].class;
            cachedSerClasses.add(cls);
            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiAlertData");
            qName2 = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiAlertData");
            cachedSerFactories.add(new org.apache.axis.encoding.ser.ArraySerializerFactory(qName, qName2));
            cachedDeserFactories.add(new org.apache.axis.encoding.ser.ArrayDeserializerFactory());

            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ArrayOfApiForecastData");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.ApiForecastData[].class;
            cachedSerClasses.add(cls);
            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiForecastData");
            qName2 = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiForecastData");
            cachedSerFactories.add(new org.apache.axis.encoding.ser.ArraySerializerFactory(qName, qName2));
            cachedDeserFactories.add(new org.apache.axis.encoding.ser.ArrayDeserializerFactory());

            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ArrayOfApiLocationData");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.ApiLocationData[].class;
            cachedSerClasses.add(cls);
            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiLocationData");
            qName2 = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiLocationData");
            cachedSerFactories.add(new org.apache.axis.encoding.ser.ArraySerializerFactory(qName, qName2));
            cachedDeserFactories.add(new org.apache.axis.encoding.ser.ArrayDeserializerFactory());

            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ArrayOfApiStationData");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.ApiStationData[].class;
            cachedSerClasses.add(cls);
            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiStationData");
            qName2 = new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiStationData");
            cachedSerFactories.add(new org.apache.axis.encoding.ser.ArraySerializerFactory(qName, qName2));
            cachedDeserFactories.add(new org.apache.axis.encoding.ser.ArrayDeserializerFactory());

            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "LiveCompactWeatherData");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.LiveCompactWeatherData.class;
            cachedSerClasses.add(cls);
            cachedSerFactories.add(beansf);
            cachedDeserFactories.add(beandf);

            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "LiveWeatherData");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.LiveWeatherData.class;
            cachedSerClasses.add(cls);
            cachedSerFactories.add(beansf);
            cachedDeserFactories.add(beandf);

            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "LocationUSWorldCityData");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.LocationUSWorldCityData.class;
            cachedSerClasses.add(cls);
            cachedSerFactories.add(beansf);
            cachedDeserFactories.add(beandf);

            qName = new javax.xml.namespace.QName("http://api.wxbug.net/", "UnitType");
            cachedSerQNames.add(qName);
            cls = net.wxbug.api.UnitType.class;
            cachedSerClasses.add(cls);
            cachedSerFactories.add(enumsf);
            cachedDeserFactories.add(enumdf);

    }

    protected org.apache.axis.client.Call createCall() throws java.rmi.RemoteException {
        try {
            org.apache.axis.client.Call _call = super._createCall();
            if (super.maintainSessionSet) {
                _call.setMaintainSession(super.maintainSession);
            }
            if (super.cachedUsername != null) {
                _call.setUsername(super.cachedUsername);
            }
            if (super.cachedPassword != null) {
                _call.setPassword(super.cachedPassword);
            }
            if (super.cachedEndpoint != null) {
                _call.setTargetEndpointAddress(super.cachedEndpoint);
            }
            if (super.cachedTimeout != null) {
                _call.setTimeout(super.cachedTimeout);
            }
            if (super.cachedPortName != null) {
                _call.setPortName(super.cachedPortName);
            }
            java.util.Enumeration keys = super.cachedProperties.keys();
            while (keys.hasMoreElements()) {
                java.lang.String key = (java.lang.String) keys.nextElement();
                _call.setProperty(key, super.cachedProperties.get(key));
            }
            // All the type mapping information is registered
            // when the first call is made.
            // The type mapping information is actually registered in
            // the TypeMappingRegistry of the service, which
            // is the reason why registration is only needed for the first call.
            synchronized (this) {
                if (firstCall()) {
                    // must set encoding style before registering serializers
                    _call.setEncodingStyle(null);
                    for (int i = 0; i < cachedSerFactories.size(); ++i) {
                        java.lang.Class cls = (java.lang.Class) cachedSerClasses.get(i);
                        javax.xml.namespace.QName qName =
                                (javax.xml.namespace.QName) cachedSerQNames.get(i);
                        java.lang.Object x = cachedSerFactories.get(i);
                        if (x instanceof Class) {
                            java.lang.Class sf = (java.lang.Class)
                                 cachedSerFactories.get(i);
                            java.lang.Class df = (java.lang.Class)
                                 cachedDeserFactories.get(i);
                            _call.registerTypeMapping(cls, qName, sf, df, false);
                        }
                        else if (x instanceof javax.xml.rpc.encoding.SerializerFactory) {
                            org.apache.axis.encoding.SerializerFactory sf = (org.apache.axis.encoding.SerializerFactory)
                                 cachedSerFactories.get(i);
                            org.apache.axis.encoding.DeserializerFactory df = (org.apache.axis.encoding.DeserializerFactory)
                                 cachedDeserFactories.get(i);
                            _call.registerTypeMapping(cls, qName, sf, df, false);
                        }
                    }
                }
            }
            return _call;
        }
        catch (java.lang.Throwable _t) {
            throw new org.apache.axis.AxisFault("Failure trying to get the Call object", _t);
        }
    }

    public net.wxbug.api.ApiLocationData[] getLocationList(java.lang.String searchString, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[0]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetLocationList");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLocationList"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {searchString, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.ApiLocationData[]) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.ApiLocationData[]) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.ApiLocationData[].class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.LocationUSWorldCityData getUSWorldCityByLatLong(java.math.BigDecimal latitude, java.math.BigDecimal longitude, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[1]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetUSWorldCityByLatLong");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetUSWorldCityByLatLong"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {latitude, longitude, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.LocationUSWorldCityData) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.LocationUSWorldCityData) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.LocationUSWorldCityData.class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.ApiStationData[] getStationListByCityCode(java.lang.String cityCode, net.wxbug.api.UnitType unitType, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[2]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetStationListByCityCode");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetStationListByCityCode"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {cityCode, unitType, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.ApiStationData[]) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.ApiStationData[]) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.ApiStationData[].class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.ApiStationData[] getStationListByUSZipCode(java.lang.String zipCode, net.wxbug.api.UnitType unitType, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[3]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetStationListByUSZipCode");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetStationListByUSZipCode"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {zipCode, unitType, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.ApiStationData[]) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.ApiStationData[]) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.ApiStationData[].class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.LiveCompactWeatherData getLiveCompactWeatherByCityCode(java.lang.String cityCode, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[4]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetLiveCompactWeatherByCityCode");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLiveCompactWeatherByCityCode"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {cityCode, unittype, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.LiveCompactWeatherData) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.LiveCompactWeatherData) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.LiveCompactWeatherData.class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.LiveCompactWeatherData getLiveCompactWeatherByUSZipCode(java.lang.String zipCode, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[5]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetLiveCompactWeatherByUSZipCode");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLiveCompactWeatherByUSZipCode"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {zipCode, unittype, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.LiveCompactWeatherData) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.LiveCompactWeatherData) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.LiveCompactWeatherData.class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.LiveCompactWeatherData getLiveCompactWeatherByStationID(java.lang.String stationid, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[6]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetLiveCompactWeatherByStationID");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLiveCompactWeatherByStationID"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {stationid, unittype, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.LiveCompactWeatherData) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.LiveCompactWeatherData) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.LiveCompactWeatherData.class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.LiveWeatherData getLiveWeatherByCityCode(java.lang.String cityCode, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[7]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetLiveWeatherByCityCode");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLiveWeatherByCityCode"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {cityCode, unittype, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.LiveWeatherData) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.LiveWeatherData) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.LiveWeatherData.class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.LiveWeatherData getLiveWeatherByUSZipCode(java.lang.String zipCode, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[8]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetLiveWeatherByUSZipCode");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLiveWeatherByUSZipCode"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {zipCode, unittype, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.LiveWeatherData) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.LiveWeatherData) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.LiveWeatherData.class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.LiveWeatherData getLiveWeatherByStationID(java.lang.String stationid, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[9]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetLiveWeatherByStationID");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetLiveWeatherByStationID"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {stationid, unittype, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.LiveWeatherData) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.LiveWeatherData) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.LiveWeatherData.class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.ApiAlertData[] getAlertsDataList(java.lang.String zipCode, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[10]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetAlertsDataList");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetAlertsDataList"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {zipCode, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.ApiAlertData[]) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.ApiAlertData[]) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.ApiAlertData[].class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.ApiAlertIssueData getAlertsIssueData(java.lang.String zipCode, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[11]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetAlertsIssueData");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetAlertsIssueData"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {zipCode, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.ApiAlertIssueData) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.ApiAlertIssueData) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.ApiAlertIssueData.class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.ApiForecastIssueData getForecastIssueDetailsByCityCode(java.lang.String cityCode, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[12]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetForecastIssueDetailsByCityCode");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetForecastIssueDetailsByCityCode"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {cityCode, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.ApiForecastIssueData) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.ApiForecastIssueData) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.ApiForecastIssueData.class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.ApiForecastIssueData getForecastIssueDetailsByUSZipCode(java.lang.String zipCode, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[13]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetForecastIssueDetailsByUSZipCode");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetForecastIssueDetailsByUSZipCode"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {zipCode, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.ApiForecastIssueData) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.ApiForecastIssueData) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.ApiForecastIssueData.class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.ApiForecastData[] getForecastByCityCode(java.lang.String cityCode, net.wxbug.api.UnitType unitType, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[14]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetForecastByCityCode");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetForecastByCityCode"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {cityCode, unitType, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.ApiForecastData[]) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.ApiForecastData[]) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.ApiForecastData[].class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

    public net.wxbug.api.ApiForecastData[] getForecastByUSZipCode(java.lang.String zipCode, net.wxbug.api.UnitType unitType, java.lang.String ACode) throws java.rmi.RemoteException {
        if (super.cachedEndpoint == null) {
            throw new org.apache.axis.NoEndPointException();
        }
        org.apache.axis.client.Call _call = createCall();
        _call.setOperation(_operations[15]);
        _call.setUseSOAPAction(true);
        _call.setSOAPActionURI("http://api.wxbug.net/GetForecastByUSZipCode");
        _call.setEncodingStyle(null);
        _call.setProperty(org.apache.axis.client.Call.SEND_TYPE_ATTR, Boolean.FALSE);
        _call.setProperty(org.apache.axis.AxisEngine.PROP_DOMULTIREFS, Boolean.FALSE);
        _call.setSOAPVersion(org.apache.axis.soap.SOAPConstants.SOAP11_CONSTANTS);
        _call.setOperationName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GetForecastByUSZipCode"));

        setRequestHeaders(_call);
        setAttachments(_call);
 try {        java.lang.Object _resp = _call.invoke(new java.lang.Object[] {zipCode, unitType, ACode});

        if (_resp instanceof java.rmi.RemoteException) {
            throw (java.rmi.RemoteException)_resp;
        }
        else {
            extractAttachments(_call);
            try {
                return (net.wxbug.api.ApiForecastData[]) _resp;
            } catch (java.lang.Exception _exception) {
                return (net.wxbug.api.ApiForecastData[]) org.apache.axis.utils.JavaUtils.convert(_resp, net.wxbug.api.ApiForecastData[].class);
            }
        }
  } catch (org.apache.axis.AxisFault axisFaultException) {
  throw axisFaultException;
}
    }

}
