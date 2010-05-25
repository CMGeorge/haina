/**
 * WeatherBugWebServicesLocator.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package net.wxbug.api;

public class WeatherBugWebServicesLocator extends org.apache.axis.client.Service implements net.wxbug.api.WeatherBugWebServices {

    public WeatherBugWebServicesLocator() {
    }


    public WeatherBugWebServicesLocator(org.apache.axis.EngineConfiguration config) {
        super(config);
    }

    public WeatherBugWebServicesLocator(java.lang.String wsdlLoc, javax.xml.namespace.QName sName) throws javax.xml.rpc.ServiceException {
        super(wsdlLoc, sName);
    }

    // Use to get a proxy class for WeatherBugWebServicesSoap12
    private java.lang.String WeatherBugWebServicesSoap12_address = "http://api.wxbug.net/webservice-v1.asmx";

    public java.lang.String getWeatherBugWebServicesSoap12Address() {
        return WeatherBugWebServicesSoap12_address;
    }

    // The WSDD service name defaults to the port name.
    private java.lang.String WeatherBugWebServicesSoap12WSDDServiceName = "WeatherBugWebServicesSoap12";

    public java.lang.String getWeatherBugWebServicesSoap12WSDDServiceName() {
        return WeatherBugWebServicesSoap12WSDDServiceName;
    }

    public void setWeatherBugWebServicesSoap12WSDDServiceName(java.lang.String name) {
        WeatherBugWebServicesSoap12WSDDServiceName = name;
    }

    public net.wxbug.api.WeatherBugWebServicesSoap getWeatherBugWebServicesSoap12() throws javax.xml.rpc.ServiceException {
       java.net.URL endpoint;
        try {
            endpoint = new java.net.URL(WeatherBugWebServicesSoap12_address);
        }
        catch (java.net.MalformedURLException e) {
            throw new javax.xml.rpc.ServiceException(e);
        }
        return getWeatherBugWebServicesSoap12(endpoint);
    }

    public net.wxbug.api.WeatherBugWebServicesSoap getWeatherBugWebServicesSoap12(java.net.URL portAddress) throws javax.xml.rpc.ServiceException {
        try {
            net.wxbug.api.WeatherBugWebServicesSoap12Stub _stub = new net.wxbug.api.WeatherBugWebServicesSoap12Stub(portAddress, this);
            _stub.setPortName(getWeatherBugWebServicesSoap12WSDDServiceName());
            return _stub;
        }
        catch (org.apache.axis.AxisFault e) {
            return null;
        }
    }

    public void setWeatherBugWebServicesSoap12EndpointAddress(java.lang.String address) {
        WeatherBugWebServicesSoap12_address = address;
    }


    // Use to get a proxy class for WeatherBugWebServicesSoap
    private java.lang.String WeatherBugWebServicesSoap_address = "http://api.wxbug.net/webservice-v1.asmx";

    public java.lang.String getWeatherBugWebServicesSoapAddress() {
        return WeatherBugWebServicesSoap_address;
    }

    // The WSDD service name defaults to the port name.
    private java.lang.String WeatherBugWebServicesSoapWSDDServiceName = "WeatherBugWebServicesSoap";

    public java.lang.String getWeatherBugWebServicesSoapWSDDServiceName() {
        return WeatherBugWebServicesSoapWSDDServiceName;
    }

    public void setWeatherBugWebServicesSoapWSDDServiceName(java.lang.String name) {
        WeatherBugWebServicesSoapWSDDServiceName = name;
    }

    public net.wxbug.api.WeatherBugWebServicesSoap getWeatherBugWebServicesSoap() throws javax.xml.rpc.ServiceException {
       java.net.URL endpoint;
        try {
            endpoint = new java.net.URL(WeatherBugWebServicesSoap_address);
        }
        catch (java.net.MalformedURLException e) {
            throw new javax.xml.rpc.ServiceException(e);
        }
        return getWeatherBugWebServicesSoap(endpoint);
    }

    public net.wxbug.api.WeatherBugWebServicesSoap getWeatherBugWebServicesSoap(java.net.URL portAddress) throws javax.xml.rpc.ServiceException {
        try {
            net.wxbug.api.WeatherBugWebServicesSoapStub _stub = new net.wxbug.api.WeatherBugWebServicesSoapStub(portAddress, this);
            _stub.setPortName(getWeatherBugWebServicesSoapWSDDServiceName());
            return _stub;
        }
        catch (org.apache.axis.AxisFault e) {
            return null;
        }
    }

    public void setWeatherBugWebServicesSoapEndpointAddress(java.lang.String address) {
        WeatherBugWebServicesSoap_address = address;
    }

    /**
     * For the given interface, get the stub implementation.
     * If this service has no port for the given interface,
     * then ServiceException is thrown.
     * This service has multiple ports for a given interface;
     * the proxy implementation returned may be indeterminate.
     */
    public java.rmi.Remote getPort(Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        try {
            if (net.wxbug.api.WeatherBugWebServicesSoap.class.isAssignableFrom(serviceEndpointInterface)) {
                net.wxbug.api.WeatherBugWebServicesSoap12Stub _stub = new net.wxbug.api.WeatherBugWebServicesSoap12Stub(new java.net.URL(WeatherBugWebServicesSoap12_address), this);
                _stub.setPortName(getWeatherBugWebServicesSoap12WSDDServiceName());
                return _stub;
            }
            if (net.wxbug.api.WeatherBugWebServicesSoap.class.isAssignableFrom(serviceEndpointInterface)) {
                net.wxbug.api.WeatherBugWebServicesSoapStub _stub = new net.wxbug.api.WeatherBugWebServicesSoapStub(new java.net.URL(WeatherBugWebServicesSoap_address), this);
                _stub.setPortName(getWeatherBugWebServicesSoapWSDDServiceName());
                return _stub;
            }
        }
        catch (java.lang.Throwable t) {
            throw new javax.xml.rpc.ServiceException(t);
        }
        throw new javax.xml.rpc.ServiceException("There is no stub implementation for the interface:  " + (serviceEndpointInterface == null ? "null" : serviceEndpointInterface.getName()));
    }

    /**
     * For the given interface, get the stub implementation.
     * If this service has no port for the given interface,
     * then ServiceException is thrown.
     */
    public java.rmi.Remote getPort(javax.xml.namespace.QName portName, Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        if (portName == null) {
            return getPort(serviceEndpointInterface);
        }
        java.lang.String inputPortName = portName.getLocalPart();
        if ("WeatherBugWebServicesSoap12".equals(inputPortName)) {
            return getWeatherBugWebServicesSoap12();
        }
        else if ("WeatherBugWebServicesSoap".equals(inputPortName)) {
            return getWeatherBugWebServicesSoap();
        }
        else  {
            java.rmi.Remote _stub = getPort(serviceEndpointInterface);
            ((org.apache.axis.client.Stub) _stub).setPortName(portName);
            return _stub;
        }
    }

    public javax.xml.namespace.QName getServiceName() {
        return new javax.xml.namespace.QName("http://api.wxbug.net/", "WeatherBugWebServices");
    }

    private java.util.HashSet ports = null;

    public java.util.Iterator getPorts() {
        if (ports == null) {
            ports = new java.util.HashSet();
            ports.add(new javax.xml.namespace.QName("http://api.wxbug.net/", "WeatherBugWebServicesSoap12"));
            ports.add(new javax.xml.namespace.QName("http://api.wxbug.net/", "WeatherBugWebServicesSoap"));
        }
        return ports.iterator();
    }

    /**
    * Set the endpoint address for the specified port name.
    */
    public void setEndpointAddress(java.lang.String portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        
if ("WeatherBugWebServicesSoap12".equals(portName)) {
            setWeatherBugWebServicesSoap12EndpointAddress(address);
        }
        else 
if ("WeatherBugWebServicesSoap".equals(portName)) {
            setWeatherBugWebServicesSoapEndpointAddress(address);
        }
        else 
{ // Unknown Port Name
            throw new javax.xml.rpc.ServiceException(" Cannot set Endpoint Address for Unknown Port" + portName);
        }
    }

    /**
    * Set the endpoint address for the specified port name.
    */
    public void setEndpointAddress(javax.xml.namespace.QName portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        setEndpointAddress(portName.getLocalPart(), address);
    }

}
