/**
 * ApiForecastData.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package net.wxbug.api;

public class ApiForecastData  implements java.io.Serializable {
    private java.lang.String conditionID;

    private java.lang.String description;

    private java.lang.String icon;

    private java.lang.String image;

    private boolean isNight;

    private java.lang.String prediction;

    private java.lang.String shortPrediction;

    private java.lang.String shortTitle;

    private java.lang.String tempHigh;

    private java.lang.String tempLow;

    private java.lang.String tempUnit;

    private java.lang.String title;

    private java.lang.String webUrl;

    public ApiForecastData() {
    }

    public ApiForecastData(
           java.lang.String conditionID,
           java.lang.String description,
           java.lang.String icon,
           java.lang.String image,
           boolean isNight,
           java.lang.String prediction,
           java.lang.String shortPrediction,
           java.lang.String shortTitle,
           java.lang.String tempHigh,
           java.lang.String tempLow,
           java.lang.String tempUnit,
           java.lang.String title,
           java.lang.String webUrl) {
           this.conditionID = conditionID;
           this.description = description;
           this.icon = icon;
           this.image = image;
           this.isNight = isNight;
           this.prediction = prediction;
           this.shortPrediction = shortPrediction;
           this.shortTitle = shortTitle;
           this.tempHigh = tempHigh;
           this.tempLow = tempLow;
           this.tempUnit = tempUnit;
           this.title = title;
           this.webUrl = webUrl;
    }


    /**
     * Gets the conditionID value for this ApiForecastData.
     * 
     * @return conditionID
     */
    public java.lang.String getConditionID() {
        return conditionID;
    }


    /**
     * Sets the conditionID value for this ApiForecastData.
     * 
     * @param conditionID
     */
    public void setConditionID(java.lang.String conditionID) {
        this.conditionID = conditionID;
    }


    /**
     * Gets the description value for this ApiForecastData.
     * 
     * @return description
     */
    public java.lang.String getDescription() {
        return description;
    }


    /**
     * Sets the description value for this ApiForecastData.
     * 
     * @param description
     */
    public void setDescription(java.lang.String description) {
        this.description = description;
    }


    /**
     * Gets the icon value for this ApiForecastData.
     * 
     * @return icon
     */
    public java.lang.String getIcon() {
        return icon;
    }


    /**
     * Sets the icon value for this ApiForecastData.
     * 
     * @param icon
     */
    public void setIcon(java.lang.String icon) {
        this.icon = icon;
    }


    /**
     * Gets the image value for this ApiForecastData.
     * 
     * @return image
     */
    public java.lang.String getImage() {
        return image;
    }


    /**
     * Sets the image value for this ApiForecastData.
     * 
     * @param image
     */
    public void setImage(java.lang.String image) {
        this.image = image;
    }


    /**
     * Gets the isNight value for this ApiForecastData.
     * 
     * @return isNight
     */
    public boolean isIsNight() {
        return isNight;
    }


    /**
     * Sets the isNight value for this ApiForecastData.
     * 
     * @param isNight
     */
    public void setIsNight(boolean isNight) {
        this.isNight = isNight;
    }


    /**
     * Gets the prediction value for this ApiForecastData.
     * 
     * @return prediction
     */
    public java.lang.String getPrediction() {
        return prediction;
    }


    /**
     * Sets the prediction value for this ApiForecastData.
     * 
     * @param prediction
     */
    public void setPrediction(java.lang.String prediction) {
        this.prediction = prediction;
    }


    /**
     * Gets the shortPrediction value for this ApiForecastData.
     * 
     * @return shortPrediction
     */
    public java.lang.String getShortPrediction() {
        return shortPrediction;
    }


    /**
     * Sets the shortPrediction value for this ApiForecastData.
     * 
     * @param shortPrediction
     */
    public void setShortPrediction(java.lang.String shortPrediction) {
        this.shortPrediction = shortPrediction;
    }


    /**
     * Gets the shortTitle value for this ApiForecastData.
     * 
     * @return shortTitle
     */
    public java.lang.String getShortTitle() {
        return shortTitle;
    }


    /**
     * Sets the shortTitle value for this ApiForecastData.
     * 
     * @param shortTitle
     */
    public void setShortTitle(java.lang.String shortTitle) {
        this.shortTitle = shortTitle;
    }


    /**
     * Gets the tempHigh value for this ApiForecastData.
     * 
     * @return tempHigh
     */
    public java.lang.String getTempHigh() {
        return tempHigh;
    }


    /**
     * Sets the tempHigh value for this ApiForecastData.
     * 
     * @param tempHigh
     */
    public void setTempHigh(java.lang.String tempHigh) {
        this.tempHigh = tempHigh;
    }


    /**
     * Gets the tempLow value for this ApiForecastData.
     * 
     * @return tempLow
     */
    public java.lang.String getTempLow() {
        return tempLow;
    }


    /**
     * Sets the tempLow value for this ApiForecastData.
     * 
     * @param tempLow
     */
    public void setTempLow(java.lang.String tempLow) {
        this.tempLow = tempLow;
    }


    /**
     * Gets the tempUnit value for this ApiForecastData.
     * 
     * @return tempUnit
     */
    public java.lang.String getTempUnit() {
        return tempUnit;
    }


    /**
     * Sets the tempUnit value for this ApiForecastData.
     * 
     * @param tempUnit
     */
    public void setTempUnit(java.lang.String tempUnit) {
        this.tempUnit = tempUnit;
    }


    /**
     * Gets the title value for this ApiForecastData.
     * 
     * @return title
     */
    public java.lang.String getTitle() {
        return title;
    }


    /**
     * Sets the title value for this ApiForecastData.
     * 
     * @param title
     */
    public void setTitle(java.lang.String title) {
        this.title = title;
    }


    /**
     * Gets the webUrl value for this ApiForecastData.
     * 
     * @return webUrl
     */
    public java.lang.String getWebUrl() {
        return webUrl;
    }


    /**
     * Sets the webUrl value for this ApiForecastData.
     * 
     * @param webUrl
     */
    public void setWebUrl(java.lang.String webUrl) {
        this.webUrl = webUrl;
    }

    private java.lang.Object __equalsCalc = null;
    public synchronized boolean equals(java.lang.Object obj) {
        if (!(obj instanceof ApiForecastData)) return false;
        ApiForecastData other = (ApiForecastData) obj;
        if (obj == null) return false;
        if (this == obj) return true;
        if (__equalsCalc != null) {
            return (__equalsCalc == obj);
        }
        __equalsCalc = obj;
        boolean _equals;
        _equals = true && 
            ((this.conditionID==null && other.getConditionID()==null) || 
             (this.conditionID!=null &&
              this.conditionID.equals(other.getConditionID()))) &&
            ((this.description==null && other.getDescription()==null) || 
             (this.description!=null &&
              this.description.equals(other.getDescription()))) &&
            ((this.icon==null && other.getIcon()==null) || 
             (this.icon!=null &&
              this.icon.equals(other.getIcon()))) &&
            ((this.image==null && other.getImage()==null) || 
             (this.image!=null &&
              this.image.equals(other.getImage()))) &&
            this.isNight == other.isIsNight() &&
            ((this.prediction==null && other.getPrediction()==null) || 
             (this.prediction!=null &&
              this.prediction.equals(other.getPrediction()))) &&
            ((this.shortPrediction==null && other.getShortPrediction()==null) || 
             (this.shortPrediction!=null &&
              this.shortPrediction.equals(other.getShortPrediction()))) &&
            ((this.shortTitle==null && other.getShortTitle()==null) || 
             (this.shortTitle!=null &&
              this.shortTitle.equals(other.getShortTitle()))) &&
            ((this.tempHigh==null && other.getTempHigh()==null) || 
             (this.tempHigh!=null &&
              this.tempHigh.equals(other.getTempHigh()))) &&
            ((this.tempLow==null && other.getTempLow()==null) || 
             (this.tempLow!=null &&
              this.tempLow.equals(other.getTempLow()))) &&
            ((this.tempUnit==null && other.getTempUnit()==null) || 
             (this.tempUnit!=null &&
              this.tempUnit.equals(other.getTempUnit()))) &&
            ((this.title==null && other.getTitle()==null) || 
             (this.title!=null &&
              this.title.equals(other.getTitle()))) &&
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
        if (getConditionID() != null) {
            _hashCode += getConditionID().hashCode();
        }
        if (getDescription() != null) {
            _hashCode += getDescription().hashCode();
        }
        if (getIcon() != null) {
            _hashCode += getIcon().hashCode();
        }
        if (getImage() != null) {
            _hashCode += getImage().hashCode();
        }
        _hashCode += (isIsNight() ? Boolean.TRUE : Boolean.FALSE).hashCode();
        if (getPrediction() != null) {
            _hashCode += getPrediction().hashCode();
        }
        if (getShortPrediction() != null) {
            _hashCode += getShortPrediction().hashCode();
        }
        if (getShortTitle() != null) {
            _hashCode += getShortTitle().hashCode();
        }
        if (getTempHigh() != null) {
            _hashCode += getTempHigh().hashCode();
        }
        if (getTempLow() != null) {
            _hashCode += getTempLow().hashCode();
        }
        if (getTempUnit() != null) {
            _hashCode += getTempUnit().hashCode();
        }
        if (getTitle() != null) {
            _hashCode += getTitle().hashCode();
        }
        if (getWebUrl() != null) {
            _hashCode += getWebUrl().hashCode();
        }
        __hashCodeCalc = false;
        return _hashCode;
    }

    // Type metadata
    private static org.apache.axis.description.TypeDesc typeDesc =
        new org.apache.axis.description.TypeDesc(ApiForecastData.class, true);

    static {
        typeDesc.setXmlType(new javax.xml.namespace.QName("http://api.wxbug.net/", "ApiForecastData"));
        org.apache.axis.description.ElementDesc elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("conditionID");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ConditionID"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("description");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Description"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("icon");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Icon"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("image");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Image"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("isNight");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "IsNight"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "boolean"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("prediction");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Prediction"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("shortPrediction");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ShortPrediction"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("shortTitle");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ShortTitle"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("tempHigh");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "TempHigh"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("tempLow");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "TempLow"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("tempUnit");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "TempUnit"));
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
