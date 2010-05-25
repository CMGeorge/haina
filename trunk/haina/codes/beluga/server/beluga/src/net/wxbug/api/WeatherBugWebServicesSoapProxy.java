package net.wxbug.api;

public class WeatherBugWebServicesSoapProxy implements net.wxbug.api.WeatherBugWebServicesSoap {
  private String _endpoint = null;
  private net.wxbug.api.WeatherBugWebServicesSoap weatherBugWebServicesSoap = null;
  
  public WeatherBugWebServicesSoapProxy() {
    _initWeatherBugWebServicesSoapProxy();
  }
  
  public WeatherBugWebServicesSoapProxy(String endpoint) {
    _endpoint = endpoint;
    _initWeatherBugWebServicesSoapProxy();
  }
  
  private void _initWeatherBugWebServicesSoapProxy() {
    try {
      weatherBugWebServicesSoap = (new net.wxbug.api.WeatherBugWebServicesLocator()).getWeatherBugWebServicesSoap();
      if (weatherBugWebServicesSoap != null) {
        if (_endpoint != null)
          ((javax.xml.rpc.Stub)weatherBugWebServicesSoap)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
        else
          _endpoint = (String)((javax.xml.rpc.Stub)weatherBugWebServicesSoap)._getProperty("javax.xml.rpc.service.endpoint.address");
      }
      
    }
    catch (javax.xml.rpc.ServiceException serviceException) {}
  }
  
  public String getEndpoint() {
    return _endpoint;
  }
  
  public void setEndpoint(String endpoint) {
    _endpoint = endpoint;
    if (weatherBugWebServicesSoap != null)
      ((javax.xml.rpc.Stub)weatherBugWebServicesSoap)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
    
  }
  
  public net.wxbug.api.WeatherBugWebServicesSoap getWeatherBugWebServicesSoap() {
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap;
  }
  
  public net.wxbug.api.ApiLocationData[] getLocationList(java.lang.String searchString, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getLocationList(searchString, ACode);
  }
  
  public net.wxbug.api.LocationUSWorldCityData getUSWorldCityByLatLong(java.math.BigDecimal latitude, java.math.BigDecimal longitude, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getUSWorldCityByLatLong(latitude, longitude, ACode);
  }
  
  public net.wxbug.api.ApiStationData[] getStationListByCityCode(java.lang.String cityCode, net.wxbug.api.UnitType unitType, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getStationListByCityCode(cityCode, unitType, ACode);
  }
  
  public net.wxbug.api.ApiStationData[] getStationListByUSZipCode(java.lang.String zipCode, net.wxbug.api.UnitType unitType, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getStationListByUSZipCode(zipCode, unitType, ACode);
  }
  
  public net.wxbug.api.LiveCompactWeatherData getLiveCompactWeatherByCityCode(java.lang.String cityCode, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getLiveCompactWeatherByCityCode(cityCode, unittype, ACode);
  }
  
  public net.wxbug.api.LiveCompactWeatherData getLiveCompactWeatherByUSZipCode(java.lang.String zipCode, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getLiveCompactWeatherByUSZipCode(zipCode, unittype, ACode);
  }
  
  public net.wxbug.api.LiveCompactWeatherData getLiveCompactWeatherByStationID(java.lang.String stationid, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getLiveCompactWeatherByStationID(stationid, unittype, ACode);
  }
  
  public net.wxbug.api.LiveWeatherData getLiveWeatherByCityCode(java.lang.String cityCode, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getLiveWeatherByCityCode(cityCode, unittype, ACode);
  }
  
  public net.wxbug.api.LiveWeatherData getLiveWeatherByUSZipCode(java.lang.String zipCode, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getLiveWeatherByUSZipCode(zipCode, unittype, ACode);
  }
  
  public net.wxbug.api.LiveWeatherData getLiveWeatherByStationID(java.lang.String stationid, net.wxbug.api.UnitType unittype, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getLiveWeatherByStationID(stationid, unittype, ACode);
  }
  
  public net.wxbug.api.ApiAlertData[] getAlertsDataList(java.lang.String zipCode, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getAlertsDataList(zipCode, ACode);
  }
  
  public net.wxbug.api.ApiAlertIssueData getAlertsIssueData(java.lang.String zipCode, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getAlertsIssueData(zipCode, ACode);
  }
  
  public net.wxbug.api.ApiForecastIssueData getForecastIssueDetailsByCityCode(java.lang.String cityCode, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getForecastIssueDetailsByCityCode(cityCode, ACode);
  }
  
  public net.wxbug.api.ApiForecastIssueData getForecastIssueDetailsByUSZipCode(java.lang.String zipCode, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getForecastIssueDetailsByUSZipCode(zipCode, ACode);
  }
  
  public net.wxbug.api.ApiForecastData[] getForecastByCityCode(java.lang.String cityCode, net.wxbug.api.UnitType unitType, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getForecastByCityCode(cityCode, unitType, ACode);
  }
  
  public net.wxbug.api.ApiForecastData[] getForecastByUSZipCode(java.lang.String zipCode, net.wxbug.api.UnitType unitType, java.lang.String ACode) throws java.rmi.RemoteException{
    if (weatherBugWebServicesSoap == null)
      _initWeatherBugWebServicesSoapProxy();
    return weatherBugWebServicesSoap.getForecastByUSZipCode(zipCode, unitType, ACode);
  }
  
  
}