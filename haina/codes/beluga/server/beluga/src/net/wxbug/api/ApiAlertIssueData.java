/**
 * ApiAlertIssueData.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package net.wxbug.api;

public class ApiAlertIssueData  implements java.io.Serializable {
    private int alertCount;

    private java.lang.String county;

    private java.lang.String fips;

    private java.lang.String state;

    private java.lang.String zipCode;

    private java.lang.String webUrl;

    public ApiAlertIssueData() {
    }

    public ApiAlertIssueData(
           int alertCount,
           java.lang.String county,
           java.lang.String fips,
           java.lang.String state,
           java.lang.String zipCode,
           java.lang.String webUrl) {
           this.alertCount = alertCount;
           this.county = county;
           this.fips = fips;
           this.state = state;
           this.zipCode = zipCode;
           this.webUrl = webUrl;
    }


    /**
     * Gets the alertCount value for this ApiAlertIssueData.
     * 
     * @return alertCount
     */
    public int getAlertCount() {
        return alertCount;
    }


    /**
     * Sets the alertCount value for this ApiAlertIssueData.
     * 
     * @param alertCount
     */
    public void setAlertCount(int alertCount) {
        this.alertCount = alertCount;
    }


    /**
     * Gets the county value for this ApiAlertIssueData.
     * 
     * @return county
     */
    public java.lang.String getCounty() {
        return county;
    }


    /**
     * Sets the county value for this ApiAlertIssueData.
     * 
     * @param county
     */
    public void setCounty(java.lang.String county) {
        this.county = county;
    }


    /**
     * Gets the fips value for this ApiAlertIssueData.
     * 
     * @return fips
     */
    public java.lang.String getFips() {
        return fips;
    }


    /**
     * Sets the fips value for this ApiAlertIssueData.
     * 
     * @param fips
     */
    public void setFips(java.lang.String fips) {
        this.fips = fips;
    }


    /**
     * Gets the state value for this ApiAlertIssueData.
     * 
     * @return state
     */
    public java.lang.String getState() {
        return state;
    }


    /**
     * Sets the state value for this ApiAlertIssueData.
     * 
     * @param state
     */
    public void setState(java.lang.String state) {
        this.state = state;
    }


    /**
     * Gets the zipCode value for this ApiAlertIssueData.
     * 
     * @return zipCode
     */
    public java.lang.String getZipCode() {
        return zipCode;
    }


    /**
     * Sets the zipCode value for this ApiAlertIssueData.
     * 
     * @param zipCode
     */
    public void setZipCode(java.lang.String zipCode) {
        this.zipCode = zipCode;
    }


    /**
     * Gets the webUrl value for this ApiAlertIssueData.
     * 
     * @return webUrl
     */
    public java.lang.String getWebUrl() {
        return webUrl;
    }


    /**
     * Sets the webUrl value for this ApiAlertIssueData.
     * 
     * @param webUrl
     */
    public void setWebUrl(java.lang.String webUrl) {
        this.webUrl = webUrl;
    }

    private java.lang.Object __equalsCalc = null;
    public synchronized boolean equals(java.lang.Object obj) {
        if (!(obj instanceof ApiAlertIssueData)) return false;
        ApiAlertIssueData other = (ApiAlertIssueData) obj;
        if (obj == null) return false;
        if (this == obj) return true;
        if (__equalsCalc != null) {
            return (__equalsCalc == obj);
        }
        __equalsCalc = obj;
        boolean _equals;
        _equals = true && 
            this.alertCount == other.getAlertCount() &&
            ((this.county==null && other.getCounty()==null) || 
             (this.county!=null &&
              this.county.equals(other.getCounty()))) &&
            ((this.fips==null && other.getFips()==null) || 
             (this.fips!=null &&
              this.fips.equals(other.getFips()))) &&
            ((this.state==null && other.getState()==null) || 
             (this.state!=null &&
              this.state.equals(other.getState()))) &&
            ((this.zipCode==null && other.getZipCode()==null) || 
             (this.zipCode!=null &&
              this.zipCode.equals(other.getZipCode()))) &&
            ((this.webUrl==null && other.getWebUrl()==null) || 
             (this.webUrl!=null &&
              this.webUrl.equals(other.getWebUrl())));
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
        _hashCode += getAlertCount();
        if (getCounty() != null) {
            _hashCode += getCounty().hashCode();
        }
        if (getFips() != null) {
            _hashCode += getFips().hashCode();
        }
        if (getState() != null) {
            _hashCode += getState().hashCode();
        }
        if (getZipCode() != null) {
            _hashCode += getZipCode().hashCode();
        }
        if (getWebUrl() != null) {
            _hashCode += getWebUrl().hashCode();
        }
        __hashCodeCalc = false;
        return _hashCode;
    }

    // Type metadata
    private static org.apache.axis.description.TypeDesc typeDesc =
        new org.apache.axis.description.TypeDesc(ApiAlertIssueData.class, true);

    static {
        typeDesc.setXmlType(new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiAlertIssueData"));
        org.apache.axis.description.ElementDesc elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("alertCount");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "AlertCount"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "int"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("county");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "County"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("fips");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Fips"));
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
        elemField.setFieldName("zipCode");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ZipCode"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("webUrl");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "WebUrl"));
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
