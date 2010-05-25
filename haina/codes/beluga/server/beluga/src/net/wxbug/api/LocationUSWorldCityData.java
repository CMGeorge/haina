/**
 * LocationUSWorldCityData.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package net.wxbug.api;

public class LocationUSWorldCityData  implements java.io.Serializable {
    private java.lang.String cityName;

    private java.lang.String cityCode;

    private java.lang.String countryCode;

    private boolean isUSCity;

    private java.math.BigDecimal latitude;

    private java.math.BigDecimal longitude;

    private java.lang.String province;

    private java.lang.String state;

    private java.math.BigDecimal timeZone;

    private java.lang.String zipCode;

    public LocationUSWorldCityData() {
    }

    public LocationUSWorldCityData(
           java.lang.String cityName,
           java.lang.String cityCode,
           java.lang.String countryCode,
           boolean isUSCity,
           java.math.BigDecimal latitude,
           java.math.BigDecimal longitude,
           java.lang.String province,
           java.lang.String state,
           java.math.BigDecimal timeZone,
           java.lang.String zipCode) {
           this.cityName = cityName;
           this.cityCode = cityCode;
           this.countryCode = countryCode;
           this.isUSCity = isUSCity;
           this.latitude = latitude;
           this.longitude = longitude;
           this.province = province;
           this.state = state;
           this.timeZone = timeZone;
           this.zipCode = zipCode;
    }


    /**
     * Gets the cityName value for this LocationUSWorldCityData.
     * 
     * @return cityName
     */
    public java.lang.String getCityName() {
        return cityName;
    }


    /**
     * Sets the cityName value for this LocationUSWorldCityData.
     * 
     * @param cityName
     */
    public void setCityName(java.lang.String cityName) {
        this.cityName = cityName;
    }


    /**
     * Gets the cityCode value for this LocationUSWorldCityData.
     * 
     * @return cityCode
     */
    public java.lang.String getCityCode() {
        return cityCode;
    }


    /**
     * Sets the cityCode value for this LocationUSWorldCityData.
     * 
     * @param cityCode
     */
    public void setCityCode(java.lang.String cityCode) {
        this.cityCode = cityCode;
    }


    /**
     * Gets the countryCode value for this LocationUSWorldCityData.
     * 
     * @return countryCode
     */
    public java.lang.String getCountryCode() {
        return countryCode;
    }


    /**
     * Sets the countryCode value for this LocationUSWorldCityData.
     * 
     * @param countryCode
     */
    public void setCountryCode(java.lang.String countryCode) {
        this.countryCode = countryCode;
    }


    /**
     * Gets the isUSCity value for this LocationUSWorldCityData.
     * 
     * @return isUSCity
     */
    public boolean isIsUSCity() {
        return isUSCity;
    }


    /**
     * Sets the isUSCity value for this LocationUSWorldCityData.
     * 
     * @param isUSCity
     */
    public void setIsUSCity(boolean isUSCity) {
        this.isUSCity = isUSCity;
    }


    /**
     * Gets the latitude value for this LocationUSWorldCityData.
     * 
     * @return latitude
     */
    public java.math.BigDecimal getLatitude() {
        return latitude;
    }


    /**
     * Sets the latitude value for this LocationUSWorldCityData.
     * 
     * @param latitude
     */
    public void setLatitude(java.math.BigDecimal latitude) {
        this.latitude = latitude;
    }


    /**
     * Gets the longitude value for this LocationUSWorldCityData.
     * 
     * @return longitude
     */
    public java.math.BigDecimal getLongitude() {
        return longitude;
    }


    /**
     * Sets the longitude value for this LocationUSWorldCityData.
     * 
     * @param longitude
     */
    public void setLongitude(java.math.BigDecimal longitude) {
        this.longitude = longitude;
    }


    /**
     * Gets the province value for this LocationUSWorldCityData.
     * 
     * @return province
     */
    public java.lang.String getProvince() {
        return province;
    }


    /**
     * Sets the province value for this LocationUSWorldCityData.
     * 
     * @param province
     */
    public void setProvince(java.lang.String province) {
        this.province = province;
    }


    /**
     * Gets the state value for this LocationUSWorldCityData.
     * 
     * @return state
     */
    public java.lang.String getState() {
        return state;
    }


    /**
     * Sets the state value for this LocationUSWorldCityData.
     * 
     * @param state
     */
    public void setState(java.lang.String state) {
        this.state = state;
    }


    /**
     * Gets the timeZone value for this LocationUSWorldCityData.
     * 
     * @return timeZone
     */
    public java.math.BigDecimal getTimeZone() {
        return timeZone;
    }


    /**
     * Sets the timeZone value for this LocationUSWorldCityData.
     * 
     * @param timeZone
     */
    public void setTimeZone(java.math.BigDecimal timeZone) {
        this.timeZone = timeZone;
    }


    /**
     * Gets the zipCode value for this LocationUSWorldCityData.
     * 
     * @return zipCode
     */
    public java.lang.String getZipCode() {
        return zipCode;
    }


    /**
     * Sets the zipCode value for this LocationUSWorldCityData.
     * 
     * @param zipCode
     */
    public void setZipCode(java.lang.String zipCode) {
        this.zipCode = zipCode;
    }

    private java.lang.Object __equalsCalc = null;
    public synchronized boolean equals(java.lang.Object obj) {
        if (!(obj instanceof LocationUSWorldCityData)) return false;
        LocationUSWorldCityData other = (LocationUSWorldCityData) obj;
        if (obj == null) return false;
        if (this == obj) return true;
        if (__equalsCalc != null) {
            return (__equalsCalc == obj);
        }
        __equalsCalc = obj;
        boolean _equals;
        _equals = true && 
            ((this.cityName==null && other.getCityName()==null) || 
             (this.cityName!=null &&
              this.cityName.equals(other.getCityName()))) &&
            ((this.cityCode==null && other.getCityCode()==null) || 
             (this.cityCode!=null &&
              this.cityCode.equals(other.getCityCode()))) &&
            ((this.countryCode==null && other.getCountryCode()==null) || 
             (this.countryCode!=null &&
              this.countryCode.equals(other.getCountryCode()))) &&
            this.isUSCity == other.isIsUSCity() &&
            ((this.latitude==null && other.getLatitude()==null) || 
             (this.latitude!=null &&
              this.latitude.equals(other.getLatitude()))) &&
            ((this.longitude==null && other.getLongitude()==null) || 
             (this.longitude!=null &&
              this.longitude.equals(other.getLongitude()))) &&
            ((this.province==null && other.getProvince()==null) || 
             (this.province!=null &&
              this.province.equals(other.getProvince()))) &&
            ((this.state==null && other.getState()==null) || 
             (this.state!=null &&
              this.state.equals(other.getState()))) &&
            ((this.timeZone==null && other.getTimeZone()==null) || 
             (this.timeZone!=null &&
              this.timeZone.equals(other.getTimeZone()))) &&
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
        if (getCityName() != null) {
            _hashCode += getCityName().hashCode();
        }
        if (getCityCode() != null) {
            _hashCode += getCityCode().hashCode();
        }
        if (getCountryCode() != null) {
            _hashCode += getCountryCode().hashCode();
        }
        _hashCode += (isIsUSCity() ? Boolean.TRUE : Boolean.FALSE).hashCode();
        if (getLatitude() != null) {
            _hashCode += getLatitude().hashCode();
        }
        if (getLongitude() != null) {
            _hashCode += getLongitude().hashCode();
        }
        if (getProvince() != null) {
            _hashCode += getProvince().hashCode();
        }
        if (getState() != null) {
            _hashCode += getState().hashCode();
        }
        if (getTimeZone() != null) {
            _hashCode += getTimeZone().hashCode();
        }
        if (getZipCode() != null) {
            _hashCode += getZipCode().hashCode();
        }
        __hashCodeCalc = false;
        return _hashCode;
    }

    // Type metadata
    private static org.apache.axis.description.TypeDesc typeDesc =
        new org.apache.axis.description.TypeDesc(LocationUSWorldCityData.class, true);

    static {
        typeDesc.setXmlType(new javax.xml.namespace.QName("http://api.wxbug.net/", "LocationUSWorldCityData"));
        org.apache.axis.description.ElementDesc elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("cityName");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "CityName"));
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
        elemField.setFieldName("countryCode");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "CountryCode"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("isUSCity");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "isUSCity"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "boolean"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("latitude");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Latitude"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "decimal"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("longitude");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Longitude"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "decimal"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("province");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Province"));
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
        elemField.setFieldName("timeZone");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "TimeZone"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "decimal"));
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
