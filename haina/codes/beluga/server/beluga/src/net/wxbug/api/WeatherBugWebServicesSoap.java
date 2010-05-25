/**
 * WeatherBugWebServicesSoap.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package net.wxbug.api;

public interface WeatherBugWebServicesSoap extends java.rmi.Remote {

    /**
     * Returns US/World locations based on US zipcode or any search
     * string.
     */
    public net.wxbug.api.ApiLocationData[] getLocationList(java.lang.String searchString, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Return the location data of US/world city that is nearest to
     * the input Lat, Long.
     */
    public net.wxbug.api.LocationUSWorldCityData getUSWorldCityByLatLong(java.math.BigDecimal latitude, java.math.BigDecimal longitude, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Returns station list (US or International) based on citycode.
     */
    public net.wxbug.api.ApiStationData[] getStationListByCityCode(java.lang.String cityCode, net.wxbug.api.UnitType unitType, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Returns station list based on US zipcode.
     */
    public net.wxbug.api.ApiStationData[] getStationListByUSZipCode(java.lang.String zipCode, net.wxbug.api.UnitType unitType, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Live Compact Weather for international cities (including US
     * cities) based on citycode.
     */
    public net.wxbug.api.LiveCompactWeatherData getLiveCompactWeatherByCityCode(java.lang.String cityCode, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Live Compact Weather for US cities based on zipcode.
     */
    public net.wxbug.api.LiveCompactWeatherData getLiveCompactWeatherByUSZipCode(java.lang.String zipCode, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Live Compact Weather for US and International locations based
     * on StationID.
     */
    public net.wxbug.api.LiveCompactWeatherData getLiveCompactWeatherByStationID(java.lang.String stationid, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Live Weather for international cities (including US cities)
     * based on citycode.
     */
    public net.wxbug.api.LiveWeatherData getLiveWeatherByCityCode(java.lang.String cityCode, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Live Weather for US cities based on zipcode.
     */
    public net.wxbug.api.LiveWeatherData getLiveWeatherByUSZipCode(java.lang.String zipCode, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Live Weather for US and International locations based on StationID.
     */
    public net.wxbug.api.LiveWeatherData getLiveWeatherByStationID(java.lang.String stationid, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Get Severe Weather Alerts based on US zipcodes.
     */
    public net.wxbug.api.ApiAlertData[] getAlertsDataList(java.lang.String zipCode, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Get Alerts location details based on US zipcode.
     */
    public net.wxbug.api.ApiAlertIssueData getAlertsIssueData(java.lang.String zipCode, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Get forecast location details based on citycode.
     */
    public net.wxbug.api.ApiForecastIssueData getForecastIssueDetailsByCityCode(java.lang.String cityCode, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Get forecast location details based on US zipcode.
     */
    public net.wxbug.api.ApiForecastIssueData getForecastIssueDetailsByUSZipCode(java.lang.String zipCode, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Get forecast based on citycode.
     */
    public net.wxbug.api.ApiForecastData[] getForecastByCityCode(java.lang.String cityCode, net.wxbug.api.UnitType unitType, java.lang.String ACode) throws java.rmi.RemoteException;

    /**
     * Get forecast based on US zipcode.
     */
    public net.wxbug.api.ApiForecastData[] getForecastByUSZipCode(java.lang.String zipCode, net.wxbug.api.UnitType unitType, java.lang.String ACode) throws java.rmi.RemoteException;
}
