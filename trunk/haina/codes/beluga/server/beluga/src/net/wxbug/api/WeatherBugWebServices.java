/**
 * WeatherBugWebServices.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package net.wxbug.api;

public interface WeatherBugWebServices extends javax.xml.rpc.Service {
    public java.lang.String getWeatherBugWebServicesSoap12Address();

    public net.wxbug.api.WeatherBugWebServicesSoap getWeatherBugWebServicesSoap12() throws javax.xml.rpc.ServiceException;

    public net.wxbug.api.WeatherBugWebServicesSoap getWeatherBugWebServicesSoap12(java.net.URL portAddress) throws javax.xml.rpc.ServiceException;
    public java.lang.String getWeatherBugWebServicesSoapAddress();

    public net.wxbug.api.WeatherBugWebServicesSoap getWeatherBugWebServicesSoap() throws javax.xml.rpc.ServiceException;

    public net.wxbug.api.WeatherBugWebServicesSoap getWeatherBugWebServicesSoap(java.net.URL portAddress) throws javax.xml.rpc.ServiceException;
}
