/**
 * ApiAlertData.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package net.wxbug.api;

public class ApiAlertData  implements java.io.Serializable {
    private java.util.Calendar expiryDateTime;

    private java.lang.String expiryTimeZone;

    private java.lang.String ID;

    private java.util.Calendar issueDateTime;

    private java.lang.String issueTimeZone;

    private java.lang.String message;

    private java.lang.String title;

    private java.lang.String type;

    private java.lang.String zipCode;

    private java.lang.String webUrl;

    public ApiAlertData() {
    }

    public ApiAlertData(
           java.util.Calendar expiryDateTime,
           java.lang.String expiryTimeZone,
           java.lang.String ID,
           java.util.Calendar issueDateTime,
           java.lang.String issueTimeZone,
           java.lang.String message,
           java.lang.String title,
           java.lang.String type,
           java.lang.String zipCode,
           java.lang.String webUrl) {
           this.expiryDateTime = expiryDateTime;
           this.expiryTimeZone = expiryTimeZone;
           this.ID = ID;
           this.issueDateTime = issueDateTime;
           this.issueTimeZone = issueTimeZone;
           this.message = message;
           this.title = title;
           this.type = type;
           this.zipCode = zipCode;
           this.webUrl = webUrl;
    }


    /**
     * Gets the expiryDateTime value for this ApiAlertData.
     * 
     * @return expiryDateTime
     */
    public java.util.Calendar getExpiryDateTime() {
        return expiryDateTime;
    }


    /**
     * Sets the expiryDateTime value for this ApiAlertData.
     * 
     * @param expiryDateTime
     */
    public void setExpiryDateTime(java.util.Calendar expiryDateTime) {
        this.expiryDateTime = expiryDateTime;
    }


    /**
     * Gets the expiryTimeZone value for this ApiAlertData.
     * 
     * @return expiryTimeZone
     */
    public java.lang.String getExpiryTimeZone() {
        return expiryTimeZone;
    }


    /**
     * Sets the expiryTimeZone value for this ApiAlertData.
     * 
     * @param expiryTimeZone
     */
    public void setExpiryTimeZone(java.lang.String expiryTimeZone) {
        this.expiryTimeZone = expiryTimeZone;
    }


    /**
     * Gets the ID value for this ApiAlertData.
     * 
     * @return ID
     */
    public java.lang.String getID() {
        return ID;
    }


    /**
     * Sets the ID value for this ApiAlertData.
     * 
     * @param ID
     */
    public void setID(java.lang.String ID) {
        this.ID = ID;
    }


    /**
     * Gets the issueDateTime value for this ApiAlertData.
     * 
     * @return issueDateTime
     */
    public java.util.Calendar getIssueDateTime() {
        return issueDateTime;
    }


    /**
     * Sets the issueDateTime value for this ApiAlertData.
     * 
     * @param issueDateTime
     */
    public void setIssueDateTime(java.util.Calendar issueDateTime) {
        this.issueDateTime = issueDateTime;
    }


    /**
     * Gets the issueTimeZone value for this ApiAlertData.
     * 
     * @return issueTimeZone
     */
    public java.lang.String getIssueTimeZone() {
        return issueTimeZone;
    }


    /**
     * Sets the issueTimeZone value for this ApiAlertData.
     * 
     * @param issueTimeZone
     */
    public void setIssueTimeZone(java.lang.String issueTimeZone) {
        this.issueTimeZone = issueTimeZone;
    }


    /**
     * Gets the message value for this ApiAlertData.
     * 
     * @return message
     */
    public java.lang.String getMessage() {
        return message;
    }


    /**
     * Sets the message value for this ApiAlertData.
     * 
     * @param message
     */
    public void setMessage(java.lang.String message) {
        this.message = message;
    }


    /**
     * Gets the title value for this ApiAlertData.
     * 
     * @return title
     */
    public java.lang.String getTitle() {
        return title;
    }


    /**
     * Sets the title value for this ApiAlertData.
     * 
     * @param title
     */
    public void setTitle(java.lang.String title) {
        this.title = title;
    }


    /**
     * Gets the type value for this ApiAlertData.
     * 
     * @return type
     */
    public java.lang.String getType() {
        return type;
    }


    /**
     * Sets the type value for this ApiAlertData.
     * 
     * @param type
     */
    public void setType(java.lang.String type) {
        this.type = type;
    }


    /**
     * Gets the zipCode value for this ApiAlertData.
     * 
     * @return zipCode
     */
    public java.lang.String getZipCode() {
        return zipCode;
    }


    /**
     * Sets the zipCode value for this ApiAlertData.
     * 
     * @param zipCode
     */
    public void setZipCode(java.lang.String zipCode) {
        this.zipCode = zipCode;
    }


    /**
     * Gets the webUrl value for this ApiAlertData.
     * 
     * @return webUrl
     */
    public java.lang.String getWebUrl() {
        return webUrl;
    }


    /**
     * Sets the webUrl value for this ApiAlertData.
     * 
     * @param webUrl
     */
    public void setWebUrl(java.lang.String webUrl) {
        this.webUrl = webUrl;
    }

    private java.lang.Object __equalsCalc = null;
    public synchronized boolean equals(java.lang.Object obj) {
        if (!(obj instanceof ApiAlertData)) return false;
        ApiAlertData other = (ApiAlertData) obj;
        if (obj == null) return false;
        if (this == obj) return true;
        if (__equalsCalc != null) {
            return (__equalsCalc == obj);
        }
        __equalsCalc = obj;
        boolean _equals;
        _equals = true && 
            ((this.expiryDateTime==null && other.getExpiryDateTime()==null) || 
             (this.expiryDateTime!=null &&
              this.expiryDateTime.equals(other.getExpiryDateTime()))) &&
            ((this.expiryTimeZone==null && other.getExpiryTimeZone()==null) || 
             (this.expiryTimeZone!=null &&
              this.expiryTimeZone.equals(other.getExpiryTimeZone()))) &&
            ((this.ID==null && other.getID()==null) || 
             (this.ID!=null &&
              this.ID.equals(other.getID()))) &&
            ((this.issueDateTime==null && other.getIssueDateTime()==null) || 
             (this.issueDateTime!=null &&
              this.issueDateTime.equals(other.getIssueDateTime()))) &&
            ((this.issueTimeZone==null && other.getIssueTimeZone()==null) || 
             (this.issueTimeZone!=null &&
              this.issueTimeZone.equals(other.getIssueTimeZone()))) &&
            ((this.message==null && other.getMessage()==null) || 
             (this.message!=null &&
              this.message.equals(other.getMessage()))) &&
            ((this.title==null && other.getTitle()==null) || 
             (this.title!=null &&
              this.title.equals(other.getTitle()))) &&
            ((this.type==null && other.getType()==null) || 
             (this.type!=null &&
              this.type.equals(other.getType()))) &&
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
        if (getExpiryDateTime() != null) {
            _hashCode += getExpiryDateTime().hashCode();
        }
        if (getExpiryTimeZone() != null) {
            _hashCode += getExpiryTimeZone().hashCode();
        }
        if (getID() != null) {
            _hashCode += getID().hashCode();
        }
        if (getIssueDateTime() != null) {
            _hashCode += getIssueDateTime().hashCode();
        }
        if (getIssueTimeZone() != null) {
            _hashCode += getIssueTimeZone().hashCode();
        }
        if (getMessage() != null) {
            _hashCode += getMessage().hashCode();
        }
        if (getTitle() != null) {
            _hashCode += getTitle().hashCode();
        }
        if (getType() != null) {
            _hashCode += getType().hashCode();
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
        new org.apache.axis.description.TypeDesc(ApiAlertData.class, true);

    static {
        typeDesc.setXmlType(new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiAlertData"));
        org.apache.axis.description.ElementDesc elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("expiryDateTime");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ExpiryDateTime"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "dateTime"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("expiryTimeZone");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ExpiryTimeZone"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("ID");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ID"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("issueDateTime");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "IssueDateTime"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "dateTime"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("issueTimeZone");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "IssueTimeZone"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("message");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Message"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("title");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Title"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("type");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Type"));
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
