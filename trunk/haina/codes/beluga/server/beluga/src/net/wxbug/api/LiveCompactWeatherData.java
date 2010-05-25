/**
 * LiveCompactWeatherData.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package net.wxbug.api;

public class LiveCompactWeatherData  implements java.io.Serializable {
    private java.lang.String city;

    private java.lang.String cityCode;

    private java.lang.String country;

    private java.lang.String currIcon;

    private java.lang.String currDesc;

    private java.lang.String gustWindSpeed;

    private java.lang.String gustWindSpeedUnit;

    private java.lang.String gustWindDirectionString;

    private java.lang.String inputLocationUrl;

    private double latitude;

    private double longitude;

    private java.lang.String obDate;

    private java.util.Calendar obDateTime;

    private java.lang.String rainToday;

    private java.lang.String rainUnit;

    private java.lang.String state;

    private java.lang.String stationIDRequested;

    private java.lang.String stationIDReturned;

    private java.lang.String stationName;

    private java.lang.String temperature;

    private java.lang.String temperatureUnit;

    private java.lang.String timeZone;

    private double timeZoneOffset;

    private java.lang.String webUrl;

    private java.lang.String windDirection;

    private java.lang.String windSpeed;

    private java.lang.String windSpeedUnit;

    private java.lang.String zipCode;

    public LiveCompactWeatherData() {
    }

    public LiveCompactWeatherData(
           java.lang.String city,
           java.lang.String cityCode,
           java.lang.String country,
           java.lang.String currIcon,
           java.lang.String currDesc,
           java.lang.String gustWindSpeed,
           java.lang.String gustWindSpeedUnit,
           java.lang.String gustWindDirectionString,
           java.lang.String inputLocationUrl,
           double latitude,
           double longitude,
           java.lang.String obDate,
           java.util.Calendar obDateTime,
           java.lang.String rainToday,
           java.lang.String rainUnit,
           java.lang.String state,
           java.lang.String stationIDRequested,
           java.lang.String stationIDReturned,
           java.lang.String stationName,
           java.lang.String temperature,
           java.lang.String temperatureUnit,
           java.lang.String timeZone,
           double timeZoneOffset,
           java.lang.String webUrl,
           java.lang.String windDirection,
           java.lang.String windSpeed,
           java.lang.String windSpeedUnit,
           java.lang.String zipCode) {
           this.city = city;
           this.cityCode = cityCode;
           this.country = country;
           this.currIcon = currIcon;
           this.currDesc = currDesc;
           this.gustWindSpeed = gustWindSpeed;
           this.gustWindSpeedUnit = gustWindSpeedUnit;
           this.gustWindDirectionString = gustWindDirectionString;
           this.inputLocationUrl = inputLocationUrl;
           this.latitude = latitude;
           this.longitude = longitude;
           this.obDate = obDate;
           this.obDateTime = obDateTime;
           this.rainToday = rainToday;
           this.rainUnit = rainUnit;
           this.state = state;
           this.stationIDRequested = stationIDRequested;
           this.stationIDReturned = stationIDReturned;
           this.stationName = stationName;
           this.temperature = temperature;
           this.temperatureUnit = temperatureUnit;
           this.timeZone = timeZone;
           this.timeZoneOffset = timeZoneOffset;
           this.webUrl = webUrl;
           this.windDirection = windDirection;
           this.windSpeed = windSpeed;
           this.windSpeedUnit = windSpeedUnit;
           this.zipCode = zipCode;
    }


    /**
     * Gets the city value for this LiveCompactWeatherData.
     * 
     * @return city
     */
    public java.lang.String getCity() {
        return city;
    }


    /**
     * Sets the city value for this LiveCompactWeatherData.
     * 
     * @param city
     */
    public void setCity(java.lang.String city) {
        this.city = city;
    }


    /**
     * Gets the cityCode value for this LiveCompactWeatherData.
     * 
     * @return cityCode
     */
    public java.lang.String getCityCode() {
        return cityCode;
    }


    /**
     * Sets the cityCode value for this LiveCompactWeatherData.
     * 
     * @param cityCode
     */
    public void setCityCode(java.lang.String cityCode) {
        this.cityCode = cityCode;
    }


    /**
     * Gets the country value for this LiveCompactWeatherData.
     * 
     * @return country
     */
    public java.lang.String getCountry() {
        return country;
    }


    /**
     * Sets the country value for this LiveCompactWeatherData.
     * 
     * @param country
     */
    public void setCountry(java.lang.String country) {
        this.country = country;
    }


    /**
     * Gets the currIcon value for this LiveCompactWeatherData.
     * 
     * @return currIcon
     */
    public java.lang.String getCurrIcon() {
        return currIcon;
    }


    /**
     * Sets the currIcon value for this LiveCompactWeatherData.
     * 
     * @param currIcon
     */
    public void setCurrIcon(java.lang.String currIcon) {
        this.currIcon = currIcon;
    }


    /**
     * Gets the currDesc value for this LiveCompactWeatherData.
     * 
     * @return currDesc
     */
    public java.lang.String getCurrDesc() {
        return currDesc;
    }


    /**
     * Sets the currDesc value for this LiveCompactWeatherData.
     * 
     * @param currDesc
     */
    public void setCurrDesc(java.lang.String currDesc) {
        this.currDesc = currDesc;
    }


    /**
     * Gets the gustWindSpeed value for this LiveCompactWeatherData.
     * 
     * @return gustWindSpeed
     */
    public java.lang.String getGustWindSpeed() {
        return gustWindSpeed;
    }


    /**
     * Sets the gustWindSpeed value for this LiveCompactWeatherData.
     * 
     * @param gustWindSpeed
     */
    public void setGustWindSpeed(java.lang.String gustWindSpeed) {
        this.gustWindSpeed = gustWindSpeed;
    }


    /**
     * Gets the gustWindSpeedUnit value for this LiveCompactWeatherData.
     * 
     * @return gustWindSpeedUnit
     */
    public java.lang.String getGustWindSpeedUnit() {
        return gustWindSpeedUnit;
    }


    /**
     * Sets the gustWindSpeedUnit value for this LiveCompactWeatherData.
     * 
     * @param gustWindSpeedUnit
     */
    public void setGustWindSpeedUnit(java.lang.String gustWindSpeedUnit) {
        this.gustWindSpeedUnit = gustWindSpeedUnit;
    }


    /**
     * Gets the gustWindDirectionString value for this LiveCompactWeatherData.
     * 
     * @return gustWindDirectionString
     */
    public java.lang.String getGustWindDirectionString() {
        return gustWindDirectionString;
    }


    /**
     * Sets the gustWindDirectionString value for this LiveCompactWeatherData.
     * 
     * @param gustWindDirectionString
     */
    public void setGustWindDirectionString(java.lang.String gustWindDirectionString) {
        this.gustWindDirectionString = gustWindDirectionString;
    }


    /**
     * Gets the inputLocationUrl value for this LiveCompactWeatherData.
     * 
     * @return inputLocationUrl
     */
    public java.lang.String getInputLocationUrl() {
        return inputLocationUrl;
    }


    /**
     * Sets the inputLocationUrl value for this LiveCompactWeatherData.
     * 
     * @param inputLocationUrl
     */
    public void setInputLocationUrl(java.lang.String inputLocationUrl) {
        this.inputLocationUrl = inputLocationUrl;
    }


    /**
     * Gets the latitude value for this LiveCompactWeatherData.
     * 
     * @return latitude
     */
    public double getLatitude() {
        return latitude;
    }


    /**
     * Sets the latitude value for this LiveCompactWeatherData.
     * 
     * @param latitude
     */
    public void setLatitude(double latitude) {
        this.latitude = latitude;
    }


    /**
     * Gets the longitude value for this LiveCompactWeatherData.
     * 
     * @return longitude
     */
    public double getLongitude() {
        return longitude;
    }


    /**
     * Sets the longitude value for this LiveCompactWeatherData.
     * 
     * @param longitude
     */
    public void setLongitude(double longitude) {
        this.longitude = longitude;
    }


    /**
     * Gets the obDate value for this LiveCompactWeatherData.
     * 
     * @return obDate
     */
    public java.lang.String getObDate() {
        return obDate;
    }


    /**
     * Sets the obDate value for this LiveCompactWeatherData.
     * 
     * @param obDate
     */
    public void setObDate(java.lang.String obDate) {
        this.obDate = obDate;
    }


    /**
     * Gets the obDateTime value for this LiveCompactWeatherData.
     * 
     * @return obDateTime
     */
    public java.util.Calendar getObDateTime() {
        return obDateTime;
    }


    /**
     * Sets the obDateTime value for this LiveCompactWeatherData.
     * 
     * @param obDateTime
     */
    public void setObDateTime(java.util.Calendar obDateTime) {
        this.obDateTime = obDateTime;
    }


    /**
     * Gets the rainToday value for this LiveCompactWeatherData.
     * 
     * @return rainToday
     */
    public java.lang.String getRainToday() {
        return rainToday;
    }


    /**
     * Sets the rainToday value for this LiveCompactWeatherData.
     * 
     * @param rainToday
     */
    public void setRainToday(java.lang.String rainToday) {
        this.rainToday = rainToday;
    }


    /**
     * Gets the rainUnit value for this LiveCompactWeatherData.
     * 
     * @return rainUnit
     */
    public java.lang.String getRainUnit() {
        return rainUnit;
    }


    /**
     * Sets the rainUnit value for this LiveCompactWeatherData.
     * 
     * @param rainUnit
     */
    public void setRainUnit(java.lang.String rainUnit) {
        this.rainUnit = rainUnit;
    }


    /**
     * Gets the state value for this LiveCompactWeatherData.
     * 
     * @return state
     */
    public java.lang.String getState() {
        return state;
    }


    /**
     * Sets the state value for this LiveCompactWeatherData.
     * 
     * @param state
     */
    public void setState(java.lang.String state) {
        this.state = state;
    }


    /**
     * Gets the stationIDRequested value for this LiveCompactWeatherData.
     * 
     * @return stationIDRequested
     */
    public java.lang.String getStationIDRequested() {
        return stationIDRequested;
    }


    /**
     * Sets the stationIDRequested value for this LiveCompactWeatherData.
     * 
     * @param stationIDRequested
     */
    public void setStationIDRequested(java.lang.String stationIDRequested) {
        this.stationIDRequested = stationIDRequested;
    }


    /**
     * Gets the stationIDReturned value for this LiveCompactWeatherData.
     * 
     * @return stationIDReturned
     */
    public java.lang.String getStationIDReturned() {
        return stationIDReturned;
    }


    /**
     * Sets the stationIDReturned value for this LiveCompactWeatherData.
     * 
     * @param stationIDReturned
     */
    public void setStationIDReturned(java.lang.String stationIDReturned) {
        this.stationIDReturned = stationIDReturned;
    }


    /**
     * Gets the stationName value for this LiveCompactWeatherData.
     * 
     * @return stationName
     */
    public java.lang.String getStationName() {
        return stationName;
    }


    /**
     * Sets the stationName value for this LiveCompactWeatherData.
     * 
     * @param stationName
     */
    public void setStationName(java.lang.String stationName) {
        this.stationName = stationName;
    }


    /**
     * Gets the temperature value for this LiveCompactWeatherData.
     * 
     * @return temperature
     */
    public java.lang.String getTemperature() {
        return temperature;
    }


    /**
     * Sets the temperature value for this LiveCompactWeatherData.
     * 
     * @param temperature
     */
    public void setTemperature(java.lang.String temperature) {
        this.temperature = temperature;
    }


    /**
     * Gets the temperatureUnit value for this LiveCompactWeatherData.
     * 
     * @return temperatureUnit
     */
    public java.lang.String getTemperatureUnit() {
        return temperatureUnit;
    }


    /**
     * Sets the temperatureUnit value for this LiveCompactWeatherData.
     * 
     * @param temperatureUnit
     */
    public void setTemperatureUnit(java.lang.String temperatureUnit) {
        this.temperatureUnit = temperatureUnit;
    }


    /**
     * Gets the timeZone value for this LiveCompactWeatherData.
     * 
     * @return timeZone
     */
    public java.lang.String getTimeZone() {
        return timeZone;
    }


    /**
     * Sets the timeZone value for this LiveCompactWeatherData.
     * 
     * @param timeZone
     */
    public void setTimeZone(java.lang.String timeZone) {
        this.timeZone = timeZone;
    }


    /**
     * Gets the timeZoneOffset value for this LiveCompactWeatherData.
     * 
     * @return timeZoneOffset
     */
    public double getTimeZoneOffset() {
        return timeZoneOffset;
    }


    /**
     * Sets the timeZoneOffset value for this LiveCompactWeatherData.
     * 
     * @param timeZoneOffset
     */
    public void setTimeZoneOffset(double timeZoneOffset) {
        this.timeZoneOffset = timeZoneOffset;
    }


    /**
     * Gets the webUrl value for this LiveCompactWeatherData.
     * 
     * @return webUrl
     */
    public java.lang.String getWebUrl() {
        return webUrl;
    }


    /**
     * Sets the webUrl value for this LiveCompactWeatherData.
     * 
     * @param webUrl
     */
    public void setWebUrl(java.lang.String webUrl) {
        this.webUrl = webUrl;
    }


    /**
     * Gets the windDirection value for this LiveCompactWeatherData.
     * 
     * @return windDirection
     */
    public java.lang.String getWindDirection() {
        return windDirection;
    }


    /**
     * Sets the windDirection value for this LiveCompactWeatherData.
     * 
     * @param windDirection
     */
    public void setWindDirection(java.lang.String windDirection) {
        this.windDirection = windDirection;
    }


    /**
     * Gets the windSpeed value for this LiveCompactWeatherData.
     * 
     * @return windSpeed
     */
    public java.lang.String getWindSpeed() {
        return windSpeed;
    }


    /**
     * Sets the windSpeed value for this LiveCompactWeatherData.
     * 
     * @param windSpeed
     */
    public void setWindSpeed(java.lang.String windSpeed) {
        this.windSpeed = windSpeed;
    }


    /**
     * Gets the windSpeedUnit value for this LiveCompactWeatherData.
     * 
     * @return windSpeedUnit
     */
    public java.lang.String getWindSpeedUnit() {
        return windSpeedUnit;
    }


    /**
     * Sets the windSpeedUnit value for this LiveCompactWeatherData.
     * 
     * @param windSpeedUnit
     */
    public void setWindSpeedUnit(java.lang.String windSpeedUnit) {
        this.windSpeedUnit = windSpeedUnit;
    }


    /**
     * Gets the zipCode value for this LiveCompactWeatherData.
     * 
     * @return zipCode
     */
    public java.lang.String getZipCode() {
        return zipCode;
    }


    /**
     * Sets the zipCode value for this LiveCompactWeatherData.
     * 
     * @param zipCode
     */
    public void setZipCode(java.lang.String zipCode) {
        this.zipCode = zipCode;
    }

    private java.lang.Object __equalsCalc = null;
    public synchronized boolean equals(java.lang.Object obj) {
        if (!(obj instanceof LiveCompactWeatherData)) return false;
        LiveCompactWeatherData other = (LiveCompactWeatherData) obj;
        if (obj == null) return false;
        if (this == obj) return true;
        if (__equalsCalc != null) {
            return (__equalsCalc == obj);
        }
        __equalsCalc = obj;
        boolean _equals;
        _equals = true && 
            ((this.city==null && other.getCity()==null) || 
             (this.city!=null &&
              this.city.equals(other.getCity()))) &&
            ((this.cityCode==null && other.getCityCode()==null) || 
             (this.cityCode!=null &&
              this.cityCode.equals(other.getCityCode()))) &&
            ((this.country==null && other.getCountry()==null) || 
             (this.country!=null &&
              this.country.equals(other.getCountry()))) &&
            ((this.currIcon==null && other.getCurrIcon()==null) || 
             (this.currIcon!=null &&
              this.currIcon.equals(other.getCurrIcon()))) &&
            ((this.currDesc==null && other.getCurrDesc()==null) || 
             (this.currDesc!=null &&
              this.currDesc.equals(other.getCurrDesc()))) &&
            ((this.gustWindSpeed==null && other.getGustWindSpeed()==null) || 
             (this.gustWindSpeed!=null &&
              this.gustWindSpeed.equals(other.getGustWindSpeed()))) &&
            ((this.gustWindSpeedUnit==null && other.getGustWindSpeedUnit()==null) || 
             (this.gustWindSpeedUnit!=null &&
              this.gustWindSpeedUnit.equals(other.getGustWindSpeedUnit()))) &&
            ((this.gustWindDirectionString==null && other.getGustWindDirectionString()==null) || 
             (this.gustWindDirectionString!=null &&
              this.gustWindDirectionString.equals(other.getGustWindDirectionString()))) &&
            ((this.inputLocationUrl==null && other.getInputLocationUrl()==null) || 
             (this.inputLocationUrl!=null &&
              this.inputLocationUrl.equals(other.getInputLocationUrl()))) &&
            this.latitude == other.getLatitude() &&
            this.longitude == other.getLongitude() &&
            ((this.obDate==null && other.getObDate()==null) || 
             (this.obDate!=null &&
              this.obDate.equals(other.getObDate()))) &&
            ((this.obDateTime==null && other.getObDateTime()==null) || 
             (this.obDateTime!=null &&
              this.obDateTime.equals(other.getObDateTime()))) &&
            ((this.rainToday==null && other.getRainToday()==null) || 
             (this.rainToday!=null &&
              this.rainToday.equals(other.getRainToday()))) &&
            ((this.rainUnit==null && other.getRainUnit()==null) || 
             (this.rainUnit!=null &&
              this.rainUnit.equals(other.getRainUnit()))) &&
            ((this.state==null && other.getState()==null) || 
             (this.state!=null &&
              this.state.equals(other.getState()))) &&
            ((this.stationIDRequested==null && other.getStationIDRequested()==null) || 
             (this.stationIDRequested!=null &&
              this.stationIDRequested.equals(other.getStationIDRequested()))) &&
            ((this.stationIDReturned==null && other.getStationIDReturned()==null) || 
             (this.stationIDReturned!=null &&
              this.stationIDReturned.equals(other.getStationIDReturned()))) &&
            ((this.stationName==null && other.getStationName()==null) || 
             (this.stationName!=null &&
              this.stationName.equals(other.getStationName()))) &&
            ((this.temperature==null && other.getTemperature()==null) || 
             (this.temperature!=null &&
              this.temperature.equals(other.getTemperature()))) &&
            ((this.temperatureUnit==null && other.getTemperatureUnit()==null) || 
             (this.temperatureUnit!=null &&
              this.temperatureUnit.equals(other.getTemperatureUnit()))) &&
            ((this.timeZone==null && other.getTimeZone()==null) || 
             (this.timeZone!=null &&
              this.timeZone.equals(other.getTimeZone()))) &&
            this.timeZoneOffset == other.getTimeZoneOffset() &&
            ((this.webUrl==null && other.getWebUrl()==null) || 
             (this.webUrl!=null &&
              this.webUrl.equals(other.getWebUrl()))) &&
            ((this.windDirection==null && other.getWindDirection()==null) || 
             (this.windDirection!=null &&
              this.windDirection.equals(other.getWindDirection()))) &&
            ((this.windSpeed==null && other.getWindSpeed()==null) || 
             (this.windSpeed!=null &&
              this.windSpeed.equals(other.getWindSpeed()))) &&
            ((this.windSpeedUnit==null && other.getWindSpeedUnit()==null) || 
             (this.windSpeedUnit!=null &&
              this.windSpeedUnit.equals(other.getWindSpeedUnit()))) &&
            ((this.zipCode==null && other.getZipCode()==null) || 
             (this.zipCode!=null &&
              this.zipCode.equals(other.getZipCode())));
        __equalsCalc = null;
        return _equals;
    }

    private boolean __hashCodeCalc = false;
    public synchronized int hashCode() {
        if (__hashCodeCalc) {
            return 0;
        }
        __hashCodeCalc = true;
        int _hashCode = 1;
        if (getCity() != null) {
            _hashCode += getCity().hashCode();
        }
        if (getCityCode() != null) {
            _hashCode += getCityCode().hashCode();
        }
        if (getCountry() != null) {
            _hashCode += getCountry().hashCode();
        }
        if (getCurrIcon() != null) {
            _hashCode += getCurrIcon().hashCode();
        }
        if (getCurrDesc() != null) {
            _hashCode += getCurrDesc().hashCode();
        }
        if (getGustWindSpeed() != null) {
            _hashCode += getGustWindSpeed().hashCode();
        }
        if (getGustWindSpeedUnit() != null) {
            _hashCode += getGustWindSpeedUnit().hashCode();
        }
        if (getGustWindDirectionString() != null) {
            _hashCode += getGustWindDirectionString().hashCode();
        }
        if (getInputLocationUrl() != null) {
            _hashCode += getInputLocationUrl().hashCode();
        }
        _hashCode += new Double(getLatitude()).hashCode();
        _hashCode += new Double(getLongitude()).hashCode();
        if (getObDate() != null) {
            _hashCode += getObDate().hashCode();
        }
        if (getObDateTime() != null) {
            _hashCode += getObDateTime().hashCode();
        }
        if (getRainToday() != null) {
            _hashCode += getRainToday().hashCode();
        }
        if (getRainUnit() != null) {
            _hashCode += getRainUnit().hashCode();
        }
        if (getState() != null) {
            _hashCode += getState().hashCode();
        }
        if (getStationIDRequested() != null) {
            _hashCode += getStationIDRequested().hashCode();
        }
        if (getStationIDReturned() != null) {
            _hashCode += getStationIDReturned().hashCode();
        }
        if (getStationName() != null) {
            _hashCode += getStationName().hashCode();
        }
        if (getTemperature() != null) {
            _hashCode += getTemperature().hashCode();
        }
        if (getTemperatureUnit() != null) {
            _hashCode += getTemperatureUnit().hashCode();
        }
        if (getTimeZone() != null) {
            _hashCode += getTimeZone().hashCode();
        }
        _hashCode += new Double(getTimeZoneOffset()).hashCode();
        if (getWebUrl() != null) {
            _hashCode += getWebUrl().hashCode();
        }
        if (getWindDirection() != null) {
            _hashCode += getWindDirection().hashCode();
        }
        if (getWindSpeed() != null) {
            _hashCode += getWindSpeed().hashCode();
        }
        if (getWindSpeedUnit() != null) {
            _hashCode += getWindSpeedUnit().hashCode();
        }
        if (getZipCode() != null) {
            _hashCode += getZipCode().hashCode();
        }
        __hashCodeCalc = false;
        return _hashCode;
    }

    // Type metadata
    private static org.apache.axis.description.TypeDesc typeDesc =
        new org.apache.axis.description.TypeDesc(LiveCompactWeatherData.class, true);

    static {
        typeDesc.setXmlType(new javax.xml.namespace.QName("http://api.wxbug.net/", "LiveCompactWeatherData"));
        org.apache.axis.description.ElementDesc elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("city");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "City"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("cityCode");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "CityCode"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("country");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Country"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("currIcon");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "CurrIcon"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("currDesc");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "CurrDesc"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("gustWindSpeed");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GustWindSpeed"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("gustWindSpeedUnit");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GustWindSpeedUnit"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("gustWindDirectionString");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GustWindDirectionString"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("inputLocationUrl");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "InputLocationUrl"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("latitude");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Latitude"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "double"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("longitude");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Longitude"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "double"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("obDate");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ObDate"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("obDateTime");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ObDateTime"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "dateTime"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("rainToday");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "RainToday"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("rainUnit");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "RainUnit"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("state");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "State"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("stationIDRequested");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "StationIDRequested"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("stationIDReturned");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "StationIDReturned"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("stationName");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "StationName"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("temperature");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Temperature"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("temperatureUnit");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "TemperatureUnit"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("timeZone");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "TimeZone"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("timeZoneOffset");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "TimeZoneOffset"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "double"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("webUrl");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "WebUrl"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("windDirection");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "WindDirection"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("windSpeed");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "WindSpeed"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("windSpeedUnit");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "WindSpeedUnit"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("zipCode");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ZipCode"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
    }

    /**
     * Return type metadata object
     */
    public static org.apache.axis.description.TypeDesc getTypeDesc() {
        return typeDesc;
    }

    /**
     * Get Custom Serializer
     */
    public static org.apache.axis.encoding.Serializer getSerializer(
           java.lang.String mechType, 
           java.lang.Class _javaType,  
           javax.xml.namespace.QName _xmlType) {
        return 
          new  org.apache.axis.encoding.ser.BeanSerializer(
            _javaType, _xmlType, typeDesc);
    }

    /**
     * Get Custom Deserializer
     */
    public static org.apache.axis.encoding.Deserializer getDeserializer(
           java.lang.String mechType, 
           java.lang.Class _javaType,  
           javax.xml.namespace.QName _xmlType) {
        return 
          new  org.apache.axis.encoding.ser.BeanDeserializer(
            _javaType, _xmlType, typeDesc);
    }

}
