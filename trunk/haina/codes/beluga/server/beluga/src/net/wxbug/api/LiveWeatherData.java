/**
 * LiveWeatherData.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package net.wxbug.api;

public class LiveWeatherData  implements java.io.Serializable {
    private java.lang.String auxTemperature;

    private java.lang.String auxTemperatureRate;

    private java.lang.String city;

    private java.lang.String cityCode;

    private java.lang.String country;

    private java.lang.String currIcon;

    private java.lang.String currDesc;

    private java.lang.String dewPoint;

    private int elevation;

    private java.lang.String elevationUnit;

    private java.lang.String feelsLike;

    private java.util.Calendar gustTime;

    private java.lang.String gustWindSpeed;

    private java.lang.String gustWindSpeedUnit;

    private java.lang.String gustWindDirectionString;

    private java.lang.String gustWindDirectionDegrees;

    private java.lang.String humidity;

    private java.lang.String humidityUnit;

    private java.lang.String humidityHigh;

    private java.lang.String humidityLow;

    private java.lang.String humidityRate;

    private java.lang.String inputLocationUrl;

    private int moonPhase;

    private java.lang.String moonPhaseImage;

    private java.lang.String pressure;

    private java.lang.String pressureUnit;

    private java.lang.String pressureHigh;

    private java.lang.String pressureLow;

    private java.lang.String pressureRate;

    private java.lang.String pressureRateUnit;

    private java.lang.String light;

    private java.lang.String lightRate;

    private java.lang.String indoorTemperature;

    private java.lang.String indoorTemperatureRate;

    private double latitude;

    private double longitude;

    private java.lang.String obDate;

    private java.util.Calendar obDateTime;

    private java.lang.String rainMonth;

    private java.lang.String rainRate;

    private java.lang.String rainRateMax;

    private java.lang.String rainRateUnit;

    private java.lang.String rainToday;

    private java.lang.String rainUnit;

    private java.lang.String rainYear;

    private java.lang.String state;

    private java.lang.String stationIDRequested;

    private java.lang.String stationIDReturned;

    private java.lang.String stationName;

    private java.lang.String stationURL;

    private java.util.Calendar sunrise;

    private java.util.Calendar sunset;

    private java.lang.String temperature;

    private java.lang.String temperatureHigh;

    private java.lang.String temperatureLow;

    private java.lang.String temperatureRate;

    private java.lang.String temperatureRateUnit;

    private java.lang.String temperatureUnit;

    private java.lang.String timeZone;

    private double timeZoneOffset;

    private java.lang.String webUrl;

    private java.lang.String wetBulb;

    private java.lang.String windDirection;

    private java.lang.String windDirectionAvg;

    private java.lang.String windDirectionDegrees;

    private java.lang.String windSpeed;

    private java.lang.String windSpeedAvg;

    private java.lang.String windSpeedUnit;

    private java.lang.String zipCode;

    public LiveWeatherData() {
    }

    public LiveWeatherData(
           java.lang.String auxTemperature,
           java.lang.String auxTemperatureRate,
           java.lang.String city,
           java.lang.String cityCode,
           java.lang.String country,
           java.lang.String currIcon,
           java.lang.String currDesc,
           java.lang.String dewPoint,
           int elevation,
           java.lang.String elevationUnit,
           java.lang.String feelsLike,
           java.util.Calendar gustTime,
           java.lang.String gustWindSpeed,
           java.lang.String gustWindSpeedUnit,
           java.lang.String gustWindDirectionString,
           java.lang.String gustWindDirectionDegrees,
           java.lang.String humidity,
           java.lang.String humidityUnit,
           java.lang.String humidityHigh,
           java.lang.String humidityLow,
           java.lang.String humidityRate,
           java.lang.String inputLocationUrl,
           int moonPhase,
           java.lang.String moonPhaseImage,
           java.lang.String pressure,
           java.lang.String pressureUnit,
           java.lang.String pressureHigh,
           java.lang.String pressureLow,
           java.lang.String pressureRate,
           java.lang.String pressureRateUnit,
           java.lang.String light,
           java.lang.String lightRate,
           java.lang.String indoorTemperature,
           java.lang.String indoorTemperatureRate,
           double latitude,
           double longitude,
           java.lang.String obDate,
           java.util.Calendar obDateTime,
           java.lang.String rainMonth,
           java.lang.String rainRate,
           java.lang.String rainRateMax,
           java.lang.String rainRateUnit,
           java.lang.String rainToday,
           java.lang.String rainUnit,
           java.lang.String rainYear,
           java.lang.String state,
           java.lang.String stationIDRequested,
           java.lang.String stationIDReturned,
           java.lang.String stationName,
           java.lang.String stationURL,
           java.util.Calendar sunrise,
           java.util.Calendar sunset,
           java.lang.String temperature,
           java.lang.String temperatureHigh,
           java.lang.String temperatureLow,
           java.lang.String temperatureRate,
           java.lang.String temperatureRateUnit,
           java.lang.String temperatureUnit,
           java.lang.String timeZone,
           double timeZoneOffset,
           java.lang.String webUrl,
           java.lang.String wetBulb,
           java.lang.String windDirection,
           java.lang.String windDirectionAvg,
           java.lang.String windDirectionDegrees,
           java.lang.String windSpeed,
           java.lang.String windSpeedAvg,
           java.lang.String windSpeedUnit,
           java.lang.String zipCode) {
           this.auxTemperature = auxTemperature;
           this.auxTemperatureRate = auxTemperatureRate;
           this.city = city;
           this.cityCode = cityCode;
           this.country = country;
           this.currIcon = currIcon;
           this.currDesc = currDesc;
           this.dewPoint = dewPoint;
           this.elevation = elevation;
           this.elevationUnit = elevationUnit;
           this.feelsLike = feelsLike;
           this.gustTime = gustTime;
           this.gustWindSpeed = gustWindSpeed;
           this.gustWindSpeedUnit = gustWindSpeedUnit;
           this.gustWindDirectionString = gustWindDirectionString;
           this.gustWindDirectionDegrees = gustWindDirectionDegrees;
           this.humidity = humidity;
           this.humidityUnit = humidityUnit;
           this.humidityHigh = humidityHigh;
           this.humidityLow = humidityLow;
           this.humidityRate = humidityRate;
           this.inputLocationUrl = inputLocationUrl;
           this.moonPhase = moonPhase;
           this.moonPhaseImage = moonPhaseImage;
           this.pressure = pressure;
           this.pressureUnit = pressureUnit;
           this.pressureHigh = pressureHigh;
           this.pressureLow = pressureLow;
           this.pressureRate = pressureRate;
           this.pressureRateUnit = pressureRateUnit;
           this.light = light;
           this.lightRate = lightRate;
           this.indoorTemperature = indoorTemperature;
           this.indoorTemperatureRate = indoorTemperatureRate;
           this.latitude = latitude;
           this.longitude = longitude;
           this.obDate = obDate;
           this.obDateTime = obDateTime;
           this.rainMonth = rainMonth;
           this.rainRate = rainRate;
           this.rainRateMax = rainRateMax;
           this.rainRateUnit = rainRateUnit;
           this.rainToday = rainToday;
           this.rainUnit = rainUnit;
           this.rainYear = rainYear;
           this.state = state;
           this.stationIDRequested = stationIDRequested;
           this.stationIDReturned = stationIDReturned;
           this.stationName = stationName;
           this.stationURL = stationURL;
           this.sunrise = sunrise;
           this.sunset = sunset;
           this.temperature = temperature;
           this.temperatureHigh = temperatureHigh;
           this.temperatureLow = temperatureLow;
           this.temperatureRate = temperatureRate;
           this.temperatureRateUnit = temperatureRateUnit;
           this.temperatureUnit = temperatureUnit;
           this.timeZone = timeZone;
           this.timeZoneOffset = timeZoneOffset;
           this.webUrl = webUrl;
           this.wetBulb = wetBulb;
           this.windDirection = windDirection;
           this.windDirectionAvg = windDirectionAvg;
           this.windDirectionDegrees = windDirectionDegrees;
           this.windSpeed = windSpeed;
           this.windSpeedAvg = windSpeedAvg;
           this.windSpeedUnit = windSpeedUnit;
           this.zipCode = zipCode;
    }


    /**
     * Gets the auxTemperature value for this LiveWeatherData.
     * 
     * @return auxTemperature
     */
    public java.lang.String getAuxTemperature() {
        return auxTemperature;
    }


    /**
     * Sets the auxTemperature value for this LiveWeatherData.
     * 
     * @param auxTemperature
     */
    public void setAuxTemperature(java.lang.String auxTemperature) {
        this.auxTemperature = auxTemperature;
    }


    /**
     * Gets the auxTemperatureRate value for this LiveWeatherData.
     * 
     * @return auxTemperatureRate
     */
    public java.lang.String getAuxTemperatureRate() {
        return auxTemperatureRate;
    }


    /**
     * Sets the auxTemperatureRate value for this LiveWeatherData.
     * 
     * @param auxTemperatureRate
     */
    public void setAuxTemperatureRate(java.lang.String auxTemperatureRate) {
        this.auxTemperatureRate = auxTemperatureRate;
    }


    /**
     * Gets the city value for this LiveWeatherData.
     * 
     * @return city
     */
    public java.lang.String getCity() {
        return city;
    }


    /**
     * Sets the city value for this LiveWeatherData.
     * 
     * @param city
     */
    public void setCity(java.lang.String city) {
        this.city = city;
    }


    /**
     * Gets the cityCode value for this LiveWeatherData.
     * 
     * @return cityCode
     */
    public java.lang.String getCityCode() {
        return cityCode;
    }


    /**
     * Sets the cityCode value for this LiveWeatherData.
     * 
     * @param cityCode
     */
    public void setCityCode(java.lang.String cityCode) {
        this.cityCode = cityCode;
    }


    /**
     * Gets the country value for this LiveWeatherData.
     * 
     * @return country
     */
    public java.lang.String getCountry() {
        return country;
    }


    /**
     * Sets the country value for this LiveWeatherData.
     * 
     * @param country
     */
    public void setCountry(java.lang.String country) {
        this.country = country;
    }


    /**
     * Gets the currIcon value for this LiveWeatherData.
     * 
     * @return currIcon
     */
    public java.lang.String getCurrIcon() {
        return currIcon;
    }


    /**
     * Sets the currIcon value for this LiveWeatherData.
     * 
     * @param currIcon
     */
    public void setCurrIcon(java.lang.String currIcon) {
        this.currIcon = currIcon;
    }


    /**
     * Gets the currDesc value for this LiveWeatherData.
     * 
     * @return currDesc
     */
    public java.lang.String getCurrDesc() {
        return currDesc;
    }


    /**
     * Sets the currDesc value for this LiveWeatherData.
     * 
     * @param currDesc
     */
    public void setCurrDesc(java.lang.String currDesc) {
        this.currDesc = currDesc;
    }


    /**
     * Gets the dewPoint value for this LiveWeatherData.
     * 
     * @return dewPoint
     */
    public java.lang.String getDewPoint() {
        return dewPoint;
    }


    /**
     * Sets the dewPoint value for this LiveWeatherData.
     * 
     * @param dewPoint
     */
    public void setDewPoint(java.lang.String dewPoint) {
        this.dewPoint = dewPoint;
    }


    /**
     * Gets the elevation value for this LiveWeatherData.
     * 
     * @return elevation
     */
    public int getElevation() {
        return elevation;
    }


    /**
     * Sets the elevation value for this LiveWeatherData.
     * 
     * @param elevation
     */
    public void setElevation(int elevation) {
        this.elevation = elevation;
    }


    /**
     * Gets the elevationUnit value for this LiveWeatherData.
     * 
     * @return elevationUnit
     */
    public java.lang.String getElevationUnit() {
        return elevationUnit;
    }


    /**
     * Sets the elevationUnit value for this LiveWeatherData.
     * 
     * @param elevationUnit
     */
    public void setElevationUnit(java.lang.String elevationUnit) {
        this.elevationUnit = elevationUnit;
    }


    /**
     * Gets the feelsLike value for this LiveWeatherData.
     * 
     * @return feelsLike
     */
    public java.lang.String getFeelsLike() {
        return feelsLike;
    }


    /**
     * Sets the feelsLike value for this LiveWeatherData.
     * 
     * @param feelsLike
     */
    public void setFeelsLike(java.lang.String feelsLike) {
        this.feelsLike = feelsLike;
    }


    /**
     * Gets the gustTime value for this LiveWeatherData.
     * 
     * @return gustTime
     */
    public java.util.Calendar getGustTime() {
        return gustTime;
    }


    /**
     * Sets the gustTime value for this LiveWeatherData.
     * 
     * @param gustTime
     */
    public void setGustTime(java.util.Calendar gustTime) {
        this.gustTime = gustTime;
    }


    /**
     * Gets the gustWindSpeed value for this LiveWeatherData.
     * 
     * @return gustWindSpeed
     */
    public java.lang.String getGustWindSpeed() {
        return gustWindSpeed;
    }


    /**
     * Sets the gustWindSpeed value for this LiveWeatherData.
     * 
     * @param gustWindSpeed
     */
    public void setGustWindSpeed(java.lang.String gustWindSpeed) {
        this.gustWindSpeed = gustWindSpeed;
    }


    /**
     * Gets the gustWindSpeedUnit value for this LiveWeatherData.
     * 
     * @return gustWindSpeedUnit
     */
    public java.lang.String getGustWindSpeedUnit() {
        return gustWindSpeedUnit;
    }


    /**
     * Sets the gustWindSpeedUnit value for this LiveWeatherData.
     * 
     * @param gustWindSpeedUnit
     */
    public void setGustWindSpeedUnit(java.lang.String gustWindSpeedUnit) {
        this.gustWindSpeedUnit = gustWindSpeedUnit;
    }


    /**
     * Gets the gustWindDirectionString value for this LiveWeatherData.
     * 
     * @return gustWindDirectionString
     */
    public java.lang.String getGustWindDirectionString() {
        return gustWindDirectionString;
    }


    /**
     * Sets the gustWindDirectionString value for this LiveWeatherData.
     * 
     * @param gustWindDirectionString
     */
    public void setGustWindDirectionString(java.lang.String gustWindDirectionString) {
        this.gustWindDirectionString = gustWindDirectionString;
    }


    /**
     * Gets the gustWindDirectionDegrees value for this LiveWeatherData.
     * 
     * @return gustWindDirectionDegrees
     */
    public java.lang.String getGustWindDirectionDegrees() {
        return gustWindDirectionDegrees;
    }


    /**
     * Sets the gustWindDirectionDegrees value for this LiveWeatherData.
     * 
     * @param gustWindDirectionDegrees
     */
    public void setGustWindDirectionDegrees(java.lang.String gustWindDirectionDegrees) {
        this.gustWindDirectionDegrees = gustWindDirectionDegrees;
    }


    /**
     * Gets the humidity value for this LiveWeatherData.
     * 
     * @return humidity
     */
    public java.lang.String getHumidity() {
        return humidity;
    }


    /**
     * Sets the humidity value for this LiveWeatherData.
     * 
     * @param humidity
     */
    public void setHumidity(java.lang.String humidity) {
        this.humidity = humidity;
    }


    /**
     * Gets the humidityUnit value for this LiveWeatherData.
     * 
     * @return humidityUnit
     */
    public java.lang.String getHumidityUnit() {
        return humidityUnit;
    }


    /**
     * Sets the humidityUnit value for this LiveWeatherData.
     * 
     * @param humidityUnit
     */
    public void setHumidityUnit(java.lang.String humidityUnit) {
        this.humidityUnit = humidityUnit;
    }


    /**
     * Gets the humidityHigh value for this LiveWeatherData.
     * 
     * @return humidityHigh
     */
    public java.lang.String getHumidityHigh() {
        return humidityHigh;
    }


    /**
     * Sets the humidityHigh value for this LiveWeatherData.
     * 
     * @param humidityHigh
     */
    public void setHumidityHigh(java.lang.String humidityHigh) {
        this.humidityHigh = humidityHigh;
    }


    /**
     * Gets the humidityLow value for this LiveWeatherData.
     * 
     * @return humidityLow
     */
    public java.lang.String getHumidityLow() {
        return humidityLow;
    }


    /**
     * Sets the humidityLow value for this LiveWeatherData.
     * 
     * @param humidityLow
     */
    public void setHumidityLow(java.lang.String humidityLow) {
        this.humidityLow = humidityLow;
    }


    /**
     * Gets the humidityRate value for this LiveWeatherData.
     * 
     * @return humidityRate
     */
    public java.lang.String getHumidityRate() {
        return humidityRate;
    }


    /**
     * Sets the humidityRate value for this LiveWeatherData.
     * 
     * @param humidityRate
     */
    public void setHumidityRate(java.lang.String humidityRate) {
        this.humidityRate = humidityRate;
    }


    /**
     * Gets the inputLocationUrl value for this LiveWeatherData.
     * 
     * @return inputLocationUrl
     */
    public java.lang.String getInputLocationUrl() {
        return inputLocationUrl;
    }


    /**
     * Sets the inputLocationUrl value for this LiveWeatherData.
     * 
     * @param inputLocationUrl
     */
    public void setInputLocationUrl(java.lang.String inputLocationUrl) {
        this.inputLocationUrl = inputLocationUrl;
    }


    /**
     * Gets the moonPhase value for this LiveWeatherData.
     * 
     * @return moonPhase
     */
    public int getMoonPhase() {
        return moonPhase;
    }


    /**
     * Sets the moonPhase value for this LiveWeatherData.
     * 
     * @param moonPhase
     */
    public void setMoonPhase(int moonPhase) {
        this.moonPhase = moonPhase;
    }


    /**
     * Gets the moonPhaseImage value for this LiveWeatherData.
     * 
     * @return moonPhaseImage
     */
    public java.lang.String getMoonPhaseImage() {
        return moonPhaseImage;
    }


    /**
     * Sets the moonPhaseImage value for this LiveWeatherData.
     * 
     * @param moonPhaseImage
     */
    public void setMoonPhaseImage(java.lang.String moonPhaseImage) {
        this.moonPhaseImage = moonPhaseImage;
    }


    /**
     * Gets the pressure value for this LiveWeatherData.
     * 
     * @return pressure
     */
    public java.lang.String getPressure() {
        return pressure;
    }


    /**
     * Sets the pressure value for this LiveWeatherData.
     * 
     * @param pressure
     */
    public void setPressure(java.lang.String pressure) {
        this.pressure = pressure;
    }


    /**
     * Gets the pressureUnit value for this LiveWeatherData.
     * 
     * @return pressureUnit
     */
    public java.lang.String getPressureUnit() {
        return pressureUnit;
    }


    /**
     * Sets the pressureUnit value for this LiveWeatherData.
     * 
     * @param pressureUnit
     */
    public void setPressureUnit(java.lang.String pressureUnit) {
        this.pressureUnit = pressureUnit;
    }


    /**
     * Gets the pressureHigh value for this LiveWeatherData.
     * 
     * @return pressureHigh
     */
    public java.lang.String getPressureHigh() {
        return pressureHigh;
    }


    /**
     * Sets the pressureHigh value for this LiveWeatherData.
     * 
     * @param pressureHigh
     */
    public void setPressureHigh(java.lang.String pressureHigh) {
        this.pressureHigh = pressureHigh;
    }


    /**
     * Gets the pressureLow value for this LiveWeatherData.
     * 
     * @return pressureLow
     */
    public java.lang.String getPressureLow() {
        return pressureLow;
    }


    /**
     * Sets the pressureLow value for this LiveWeatherData.
     * 
     * @param pressureLow
     */
    public void setPressureLow(java.lang.String pressureLow) {
        this.pressureLow = pressureLow;
    }


    /**
     * Gets the pressureRate value for this LiveWeatherData.
     * 
     * @return pressureRate
     */
    public java.lang.String getPressureRate() {
        return pressureRate;
    }


    /**
     * Sets the pressureRate value for this LiveWeatherData.
     * 
     * @param pressureRate
     */
    public void setPressureRate(java.lang.String pressureRate) {
        this.pressureRate = pressureRate;
    }


    /**
     * Gets the pressureRateUnit value for this LiveWeatherData.
     * 
     * @return pressureRateUnit
     */
    public java.lang.String getPressureRateUnit() {
        return pressureRateUnit;
    }


    /**
     * Sets the pressureRateUnit value for this LiveWeatherData.
     * 
     * @param pressureRateUnit
     */
    public void setPressureRateUnit(java.lang.String pressureRateUnit) {
        this.pressureRateUnit = pressureRateUnit;
    }


    /**
     * Gets the light value for this LiveWeatherData.
     * 
     * @return light
     */
    public java.lang.String getLight() {
        return light;
    }


    /**
     * Sets the light value for this LiveWeatherData.
     * 
     * @param light
     */
    public void setLight(java.lang.String light) {
        this.light = light;
    }


    /**
     * Gets the lightRate value for this LiveWeatherData.
     * 
     * @return lightRate
     */
    public java.lang.String getLightRate() {
        return lightRate;
    }


    /**
     * Sets the lightRate value for this LiveWeatherData.
     * 
     * @param lightRate
     */
    public void setLightRate(java.lang.String lightRate) {
        this.lightRate = lightRate;
    }


    /**
     * Gets the indoorTemperature value for this LiveWeatherData.
     * 
     * @return indoorTemperature
     */
    public java.lang.String getIndoorTemperature() {
        return indoorTemperature;
    }


    /**
     * Sets the indoorTemperature value for this LiveWeatherData.
     * 
     * @param indoorTemperature
     */
    public void setIndoorTemperature(java.lang.String indoorTemperature) {
        this.indoorTemperature = indoorTemperature;
    }


    /**
     * Gets the indoorTemperatureRate value for this LiveWeatherData.
     * 
     * @return indoorTemperatureRate
     */
    public java.lang.String getIndoorTemperatureRate() {
        return indoorTemperatureRate;
    }


    /**
     * Sets the indoorTemperatureRate value for this LiveWeatherData.
     * 
     * @param indoorTemperatureRate
     */
    public void setIndoorTemperatureRate(java.lang.String indoorTemperatureRate) {
        this.indoorTemperatureRate = indoorTemperatureRate;
    }


    /**
     * Gets the latitude value for this LiveWeatherData.
     * 
     * @return latitude
     */
    public double getLatitude() {
        return latitude;
    }


    /**
     * Sets the latitude value for this LiveWeatherData.
     * 
     * @param latitude
     */
    public void setLatitude(double latitude) {
        this.latitude = latitude;
    }


    /**
     * Gets the longitude value for this LiveWeatherData.
     * 
     * @return longitude
     */
    public double getLongitude() {
        return longitude;
    }


    /**
     * Sets the longitude value for this LiveWeatherData.
     * 
     * @param longitude
     */
    public void setLongitude(double longitude) {
        this.longitude = longitude;
    }


    /**
     * Gets the obDate value for this LiveWeatherData.
     * 
     * @return obDate
     */
    public java.lang.String getObDate() {
        return obDate;
    }


    /**
     * Sets the obDate value for this LiveWeatherData.
     * 
     * @param obDate
     */
    public void setObDate(java.lang.String obDate) {
        this.obDate = obDate;
    }


    /**
     * Gets the obDateTime value for this LiveWeatherData.
     * 
     * @return obDateTime
     */
    public java.util.Calendar getObDateTime() {
        return obDateTime;
    }


    /**
     * Sets the obDateTime value for this LiveWeatherData.
     * 
     * @param obDateTime
     */
    public void setObDateTime(java.util.Calendar obDateTime) {
        this.obDateTime = obDateTime;
    }


    /**
     * Gets the rainMonth value for this LiveWeatherData.
     * 
     * @return rainMonth
     */
    public java.lang.String getRainMonth() {
        return rainMonth;
    }


    /**
     * Sets the rainMonth value for this LiveWeatherData.
     * 
     * @param rainMonth
     */
    public void setRainMonth(java.lang.String rainMonth) {
        this.rainMonth = rainMonth;
    }


    /**
     * Gets the rainRate value for this LiveWeatherData.
     * 
     * @return rainRate
     */
    public java.lang.String getRainRate() {
        return rainRate;
    }


    /**
     * Sets the rainRate value for this LiveWeatherData.
     * 
     * @param rainRate
     */
    public void setRainRate(java.lang.String rainRate) {
        this.rainRate = rainRate;
    }


    /**
     * Gets the rainRateMax value for this LiveWeatherData.
     * 
     * @return rainRateMax
     */
    public java.lang.String getRainRateMax() {
        return rainRateMax;
    }


    /**
     * Sets the rainRateMax value for this LiveWeatherData.
     * 
     * @param rainRateMax
     */
    public void setRainRateMax(java.lang.String rainRateMax) {
        this.rainRateMax = rainRateMax;
    }


    /**
     * Gets the rainRateUnit value for this LiveWeatherData.
     * 
     * @return rainRateUnit
     */
    public java.lang.String getRainRateUnit() {
        return rainRateUnit;
    }


    /**
     * Sets the rainRateUnit value for this LiveWeatherData.
     * 
     * @param rainRateUnit
     */
    public void setRainRateUnit(java.lang.String rainRateUnit) {
        this.rainRateUnit = rainRateUnit;
    }


    /**
     * Gets the rainToday value for this LiveWeatherData.
     * 
     * @return rainToday
     */
    public java.lang.String getRainToday() {
        return rainToday;
    }


    /**
     * Sets the rainToday value for this LiveWeatherData.
     * 
     * @param rainToday
     */
    public void setRainToday(java.lang.String rainToday) {
        this.rainToday = rainToday;
    }


    /**
     * Gets the rainUnit value for this LiveWeatherData.
     * 
     * @return rainUnit
     */
    public java.lang.String getRainUnit() {
        return rainUnit;
    }


    /**
     * Sets the rainUnit value for this LiveWeatherData.
     * 
     * @param rainUnit
     */
    public void setRainUnit(java.lang.String rainUnit) {
        this.rainUnit = rainUnit;
    }


    /**
     * Gets the rainYear value for this LiveWeatherData.
     * 
     * @return rainYear
     */
    public java.lang.String getRainYear() {
        return rainYear;
    }


    /**
     * Sets the rainYear value for this LiveWeatherData.
     * 
     * @param rainYear
     */
    public void setRainYear(java.lang.String rainYear) {
        this.rainYear = rainYear;
    }


    /**
     * Gets the state value for this LiveWeatherData.
     * 
     * @return state
     */
    public java.lang.String getState() {
        return state;
    }


    /**
     * Sets the state value for this LiveWeatherData.
     * 
     * @param state
     */
    public void setState(java.lang.String state) {
        this.state = state;
    }


    /**
     * Gets the stationIDRequested value for this LiveWeatherData.
     * 
     * @return stationIDRequested
     */
    public java.lang.String getStationIDRequested() {
        return stationIDRequested;
    }


    /**
     * Sets the stationIDRequested value for this LiveWeatherData.
     * 
     * @param stationIDRequested
     */
    public void setStationIDRequested(java.lang.String stationIDRequested) {
        this.stationIDRequested = stationIDRequested;
    }


    /**
     * Gets the stationIDReturned value for this LiveWeatherData.
     * 
     * @return stationIDReturned
     */
    public java.lang.String getStationIDReturned() {
        return stationIDReturned;
    }


    /**
     * Sets the stationIDReturned value for this LiveWeatherData.
     * 
     * @param stationIDReturned
     */
    public void setStationIDReturned(java.lang.String stationIDReturned) {
        this.stationIDReturned = stationIDReturned;
    }


    /**
     * Gets the stationName value for this LiveWeatherData.
     * 
     * @return stationName
     */
    public java.lang.String getStationName() {
        return stationName;
    }


    /**
     * Sets the stationName value for this LiveWeatherData.
     * 
     * @param stationName
     */
    public void setStationName(java.lang.String stationName) {
        this.stationName = stationName;
    }


    /**
     * Gets the stationURL value for this LiveWeatherData.
     * 
     * @return stationURL
     */
    public java.lang.String getStationURL() {
        return stationURL;
    }


    /**
     * Sets the stationURL value for this LiveWeatherData.
     * 
     * @param stationURL
     */
    public void setStationURL(java.lang.String stationURL) {
        this.stationURL = stationURL;
    }


    /**
     * Gets the sunrise value for this LiveWeatherData.
     * 
     * @return sunrise
     */
    public java.util.Calendar getSunrise() {
        return sunrise;
    }


    /**
     * Sets the sunrise value for this LiveWeatherData.
     * 
     * @param sunrise
     */
    public void setSunrise(java.util.Calendar sunrise) {
        this.sunrise = sunrise;
    }


    /**
     * Gets the sunset value for this LiveWeatherData.
     * 
     * @return sunset
     */
    public java.util.Calendar getSunset() {
        return sunset;
    }


    /**
     * Sets the sunset value for this LiveWeatherData.
     * 
     * @param sunset
     */
    public void setSunset(java.util.Calendar sunset) {
        this.sunset = sunset;
    }


    /**
     * Gets the temperature value for this LiveWeatherData.
     * 
     * @return temperature
     */
    public java.lang.String getTemperature() {
        return temperature;
    }


    /**
     * Sets the temperature value for this LiveWeatherData.
     * 
     * @param temperature
     */
    public void setTemperature(java.lang.String temperature) {
        this.temperature = temperature;
    }


    /**
     * Gets the temperatureHigh value for this LiveWeatherData.
     * 
     * @return temperatureHigh
     */
    public java.lang.String getTemperatureHigh() {
        return temperatureHigh;
    }


    /**
     * Sets the temperatureHigh value for this LiveWeatherData.
     * 
     * @param temperatureHigh
     */
    public void setTemperatureHigh(java.lang.String temperatureHigh) {
        this.temperatureHigh = temperatureHigh;
    }


    /**
     * Gets the temperatureLow value for this LiveWeatherData.
     * 
     * @return temperatureLow
     */
    public java.lang.String getTemperatureLow() {
        return temperatureLow;
    }


    /**
     * Sets the temperatureLow value for this LiveWeatherData.
     * 
     * @param temperatureLow
     */
    public void setTemperatureLow(java.lang.String temperatureLow) {
        this.temperatureLow = temperatureLow;
    }


    /**
     * Gets the temperatureRate value for this LiveWeatherData.
     * 
     * @return temperatureRate
     */
    public java.lang.String getTemperatureRate() {
        return temperatureRate;
    }


    /**
     * Sets the temperatureRate value for this LiveWeatherData.
     * 
     * @param temperatureRate
     */
    public void setTemperatureRate(java.lang.String temperatureRate) {
        this.temperatureRate = temperatureRate;
    }


    /**
     * Gets the temperatureRateUnit value for this LiveWeatherData.
     * 
     * @return temperatureRateUnit
     */
    public java.lang.String getTemperatureRateUnit() {
        return temperatureRateUnit;
    }


    /**
     * Sets the temperatureRateUnit value for this LiveWeatherData.
     * 
     * @param temperatureRateUnit
     */
    public void setTemperatureRateUnit(java.lang.String temperatureRateUnit) {
        this.temperatureRateUnit = temperatureRateUnit;
    }


    /**
     * Gets the temperatureUnit value for this LiveWeatherData.
     * 
     * @return temperatureUnit
     */
    public java.lang.String getTemperatureUnit() {
        return temperatureUnit;
    }


    /**
     * Sets the temperatureUnit value for this LiveWeatherData.
     * 
     * @param temperatureUnit
     */
    public void setTemperatureUnit(java.lang.String temperatureUnit) {
        this.temperatureUnit = temperatureUnit;
    }


    /**
     * Gets the timeZone value for this LiveWeatherData.
     * 
     * @return timeZone
     */
    public java.lang.String getTimeZone() {
        return timeZone;
    }


    /**
     * Sets the timeZone value for this LiveWeatherData.
     * 
     * @param timeZone
     */
    public void setTimeZone(java.lang.String timeZone) {
        this.timeZone = timeZone;
    }


    /**
     * Gets the timeZoneOffset value for this LiveWeatherData.
     * 
     * @return timeZoneOffset
     */
    public double getTimeZoneOffset() {
        return timeZoneOffset;
    }


    /**
     * Sets the timeZoneOffset value for this LiveWeatherData.
     * 
     * @param timeZoneOffset
     */
    public void setTimeZoneOffset(double timeZoneOffset) {
        this.timeZoneOffset = timeZoneOffset;
    }


    /**
     * Gets the webUrl value for this LiveWeatherData.
     * 
     * @return webUrl
     */
    public java.lang.String getWebUrl() {
        return webUrl;
    }


    /**
     * Sets the webUrl value for this LiveWeatherData.
     * 
     * @param webUrl
     */
    public void setWebUrl(java.lang.String webUrl) {
        this.webUrl = webUrl;
    }


    /**
     * Gets the wetBulb value for this LiveWeatherData.
     * 
     * @return wetBulb
     */
    public java.lang.String getWetBulb() {
        return wetBulb;
    }


    /**
     * Sets the wetBulb value for this LiveWeatherData.
     * 
     * @param wetBulb
     */
    public void setWetBulb(java.lang.String wetBulb) {
        this.wetBulb = wetBulb;
    }


    /**
     * Gets the windDirection value for this LiveWeatherData.
     * 
     * @return windDirection
     */
    public java.lang.String getWindDirection() {
        return windDirection;
    }


    /**
     * Sets the windDirection value for this LiveWeatherData.
     * 
     * @param windDirection
     */
    public void setWindDirection(java.lang.String windDirection) {
        this.windDirection = windDirection;
    }


    /**
     * Gets the windDirectionAvg value for this LiveWeatherData.
     * 
     * @return windDirectionAvg
     */
    public java.lang.String getWindDirectionAvg() {
        return windDirectionAvg;
    }


    /**
     * Sets the windDirectionAvg value for this LiveWeatherData.
     * 
     * @param windDirectionAvg
     */
    public void setWindDirectionAvg(java.lang.String windDirectionAvg) {
        this.windDirectionAvg = windDirectionAvg;
    }


    /**
     * Gets the windDirectionDegrees value for this LiveWeatherData.
     * 
     * @return windDirectionDegrees
     */
    public java.lang.String getWindDirectionDegrees() {
        return windDirectionDegrees;
    }


    /**
     * Sets the windDirectionDegrees value for this LiveWeatherData.
     * 
     * @param windDirectionDegrees
     */
    public void setWindDirectionDegrees(java.lang.String windDirectionDegrees) {
        this.windDirectionDegrees = windDirectionDegrees;
    }


    /**
     * Gets the windSpeed value for this LiveWeatherData.
     * 
     * @return windSpeed
     */
    public java.lang.String getWindSpeed() {
        return windSpeed;
    }


    /**
     * Sets the windSpeed value for this LiveWeatherData.
     * 
     * @param windSpeed
     */
    public void setWindSpeed(java.lang.String windSpeed) {
        this.windSpeed = windSpeed;
    }


    /**
     * Gets the windSpeedAvg value for this LiveWeatherData.
     * 
     * @return windSpeedAvg
     */
    public java.lang.String getWindSpeedAvg() {
        return windSpeedAvg;
    }


    /**
     * Sets the windSpeedAvg value for this LiveWeatherData.
     * 
     * @param windSpeedAvg
     */
    public void setWindSpeedAvg(java.lang.String windSpeedAvg) {
        this.windSpeedAvg = windSpeedAvg;
    }


    /**
     * Gets the windSpeedUnit value for this LiveWeatherData.
     * 
     * @return windSpeedUnit
     */
    public java.lang.String getWindSpeedUnit() {
        return windSpeedUnit;
    }


    /**
     * Sets the windSpeedUnit value for this LiveWeatherData.
     * 
     * @param windSpeedUnit
     */
    public void setWindSpeedUnit(java.lang.String windSpeedUnit) {
        this.windSpeedUnit = windSpeedUnit;
    }


    /**
     * Gets the zipCode value for this LiveWeatherData.
     * 
     * @return zipCode
     */
    public java.lang.String getZipCode() {
        return zipCode;
    }


    /**
     * Sets the zipCode value for this LiveWeatherData.
     * 
     * @param zipCode
     */
    public void setZipCode(java.lang.String zipCode) {
        this.zipCode = zipCode;
    }

    private java.lang.Object __equalsCalc = null;
    public synchronized boolean equals(java.lang.Object obj) {
        if (!(obj instanceof LiveWeatherData)) return false;
        LiveWeatherData other = (LiveWeatherData) obj;
        if (obj == null) return false;
        if (this == obj) return true;
        if (__equalsCalc != null) {
            return (__equalsCalc == obj);
        }
        __equalsCalc = obj;
        boolean _equals;
        _equals = true && 
            ((this.auxTemperature==null && other.getAuxTemperature()==null) || 
             (this.auxTemperature!=null &&
              this.auxTemperature.equals(other.getAuxTemperature()))) &&
            ((this.auxTemperatureRate==null && other.getAuxTemperatureRate()==null) || 
             (this.auxTemperatureRate!=null &&
              this.auxTemperatureRate.equals(other.getAuxTemperatureRate()))) &&
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
            ((this.dewPoint==null && other.getDewPoint()==null) || 
             (this.dewPoint!=null &&
              this.dewPoint.equals(other.getDewPoint()))) &&
            this.elevation == other.getElevation() &&
            ((this.elevationUnit==null && other.getElevationUnit()==null) || 
             (this.elevationUnit!=null &&
              this.elevationUnit.equals(other.getElevationUnit()))) &&
            ((this.feelsLike==null && other.getFeelsLike()==null) || 
             (this.feelsLike!=null &&
              this.feelsLike.equals(other.getFeelsLike()))) &&
            ((this.gustTime==null && other.getGustTime()==null) || 
             (this.gustTime!=null &&
              this.gustTime.equals(other.getGustTime()))) &&
            ((this.gustWindSpeed==null && other.getGustWindSpeed()==null) || 
             (this.gustWindSpeed!=null &&
              this.gustWindSpeed.equals(other.getGustWindSpeed()))) &&
            ((this.gustWindSpeedUnit==null && other.getGustWindSpeedUnit()==null) || 
             (this.gustWindSpeedUnit!=null &&
              this.gustWindSpeedUnit.equals(other.getGustWindSpeedUnit()))) &&
            ((this.gustWindDirectionString==null && other.getGustWindDirectionString()==null) || 
             (this.gustWindDirectionString!=null &&
              this.gustWindDirectionString.equals(other.getGustWindDirectionString()))) &&
            ((this.gustWindDirectionDegrees==null && other.getGustWindDirectionDegrees()==null) || 
             (this.gustWindDirectionDegrees!=null &&
              this.gustWindDirectionDegrees.equals(other.getGustWindDirectionDegrees()))) &&
            ((this.humidity==null && other.getHumidity()==null) || 
             (this.humidity!=null &&
              this.humidity.equals(other.getHumidity()))) &&
            ((this.humidityUnit==null && other.getHumidityUnit()==null) || 
             (this.humidityUnit!=null &&
              this.humidityUnit.equals(other.getHumidityUnit()))) &&
            ((this.humidityHigh==null && other.getHumidityHigh()==null) || 
             (this.humidityHigh!=null &&
              this.humidityHigh.equals(other.getHumidityHigh()))) &&
            ((this.humidityLow==null && other.getHumidityLow()==null) || 
             (this.humidityLow!=null &&
              this.humidityLow.equals(other.getHumidityLow()))) &&
            ((this.humidityRate==null && other.getHumidityRate()==null) || 
             (this.humidityRate!=null &&
              this.humidityRate.equals(other.getHumidityRate()))) &&
            ((this.inputLocationUrl==null && other.getInputLocationUrl()==null) || 
             (this.inputLocationUrl!=null &&
              this.inputLocationUrl.equals(other.getInputLocationUrl()))) &&
            this.moonPhase == other.getMoonPhase() &&
            ((this.moonPhaseImage==null && other.getMoonPhaseImage()==null) || 
             (this.moonPhaseImage!=null &&
              this.moonPhaseImage.equals(other.getMoonPhaseImage()))) &&
            ((this.pressure==null && other.getPressure()==null) || 
             (this.pressure!=null &&
              this.pressure.equals(other.getPressure()))) &&
            ((this.pressureUnit==null && other.getPressureUnit()==null) || 
             (this.pressureUnit!=null &&
              this.pressureUnit.equals(other.getPressureUnit()))) &&
            ((this.pressureHigh==null && other.getPressureHigh()==null) || 
             (this.pressureHigh!=null &&
              this.pressureHigh.equals(other.getPressureHigh()))) &&
            ((this.pressureLow==null && other.getPressureLow()==null) || 
             (this.pressureLow!=null &&
              this.pressureLow.equals(other.getPressureLow()))) &&
            ((this.pressureRate==null && other.getPressureRate()==null) || 
             (this.pressureRate!=null &&
              this.pressureRate.equals(other.getPressureRate()))) &&
            ((this.pressureRateUnit==null && other.getPressureRateUnit()==null) || 
             (this.pressureRateUnit!=null &&
              this.pressureRateUnit.equals(other.getPressureRateUnit()))) &&
            ((this.light==null && other.getLight()==null) || 
             (this.light!=null &&
              this.light.equals(other.getLight()))) &&
            ((this.lightRate==null && other.getLightRate()==null) || 
             (this.lightRate!=null &&
              this.lightRate.equals(other.getLightRate()))) &&
            ((this.indoorTemperature==null && other.getIndoorTemperature()==null) || 
             (this.indoorTemperature!=null &&
              this.indoorTemperature.equals(other.getIndoorTemperature()))) &&
            ((this.indoorTemperatureRate==null && other.getIndoorTemperatureRate()==null) || 
             (this.indoorTemperatureRate!=null &&
              this.indoorTemperatureRate.equals(other.getIndoorTemperatureRate()))) &&
            this.latitude == other.getLatitude() &&
            this.longitude == other.getLongitude() &&
            ((this.obDate==null && other.getObDate()==null) || 
             (this.obDate!=null &&
              this.obDate.equals(other.getObDate()))) &&
            ((this.obDateTime==null && other.getObDateTime()==null) || 
             (this.obDateTime!=null &&
              this.obDateTime.equals(other.getObDateTime()))) &&
            ((this.rainMonth==null && other.getRainMonth()==null) || 
             (this.rainMonth!=null &&
              this.rainMonth.equals(other.getRainMonth()))) &&
            ((this.rainRate==null && other.getRainRate()==null) || 
             (this.rainRate!=null &&
              this.rainRate.equals(other.getRainRate()))) &&
            ((this.rainRateMax==null && other.getRainRateMax()==null) || 
             (this.rainRateMax!=null &&
              this.rainRateMax.equals(other.getRainRateMax()))) &&
            ((this.rainRateUnit==null && other.getRainRateUnit()==null) || 
             (this.rainRateUnit!=null &&
              this.rainRateUnit.equals(other.getRainRateUnit()))) &&
            ((this.rainToday==null && other.getRainToday()==null) || 
             (this.rainToday!=null &&
              this.rainToday.equals(other.getRainToday()))) &&
            ((this.rainUnit==null && other.getRainUnit()==null) || 
             (this.rainUnit!=null &&
              this.rainUnit.equals(other.getRainUnit()))) &&
            ((this.rainYear==null && other.getRainYear()==null) || 
             (this.rainYear!=null &&
              this.rainYear.equals(other.getRainYear()))) &&
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
            ((this.stationURL==null && other.getStationURL()==null) || 
             (this.stationURL!=null &&
              this.stationURL.equals(other.getStationURL()))) &&
            ((this.sunrise==null && other.getSunrise()==null) || 
             (this.sunrise!=null &&
              this.sunrise.equals(other.getSunrise()))) &&
            ((this.sunset==null && other.getSunset()==null) || 
             (this.sunset!=null &&
              this.sunset.equals(other.getSunset()))) &&
            ((this.temperature==null && other.getTemperature()==null) || 
             (this.temperature!=null &&
              this.temperature.equals(other.getTemperature()))) &&
            ((this.temperatureHigh==null && other.getTemperatureHigh()==null) || 
             (this.temperatureHigh!=null &&
              this.temperatureHigh.equals(other.getTemperatureHigh()))) &&
            ((this.temperatureLow==null && other.getTemperatureLow()==null) || 
             (this.temperatureLow!=null &&
              this.temperatureLow.equals(other.getTemperatureLow()))) &&
            ((this.temperatureRate==null && other.getTemperatureRate()==null) || 
             (this.temperatureRate!=null &&
              this.temperatureRate.equals(other.getTemperatureRate()))) &&
            ((this.temperatureRateUnit==null && other.getTemperatureRateUnit()==null) || 
             (this.temperatureRateUnit!=null &&
              this.temperatureRateUnit.equals(other.getTemperatureRateUnit()))) &&
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
            ((this.wetBulb==null && other.getWetBulb()==null) || 
             (this.wetBulb!=null &&
              this.wetBulb.equals(other.getWetBulb()))) &&
            ((this.windDirection==null && other.getWindDirection()==null) || 
             (this.windDirection!=null &&
              this.windDirection.equals(other.getWindDirection()))) &&
            ((this.windDirectionAvg==null && other.getWindDirectionAvg()==null) || 
             (this.windDirectionAvg!=null &&
              this.windDirectionAvg.equals(other.getWindDirectionAvg()))) &&
            ((this.windDirectionDegrees==null && other.getWindDirectionDegrees()==null) || 
             (this.windDirectionDegrees!=null &&
              this.windDirectionDegrees.equals(other.getWindDirectionDegrees()))) &&
            ((this.windSpeed==null && other.getWindSpeed()==null) || 
             (this.windSpeed!=null &&
              this.windSpeed.equals(other.getWindSpeed()))) &&
            ((this.windSpeedAvg==null && other.getWindSpeedAvg()==null) || 
             (this.windSpeedAvg!=null &&
              this.windSpeedAvg.equals(other.getWindSpeedAvg()))) &&
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
        if (getAuxTemperature() != null) {
            _hashCode += getAuxTemperature().hashCode();
        }
        if (getAuxTemperatureRate() != null) {
            _hashCode += getAuxTemperatureRate().hashCode();
        }
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
        if (getDewPoint() != null) {
            _hashCode += getDewPoint().hashCode();
        }
        _hashCode += getElevation();
        if (getElevationUnit() != null) {
            _hashCode += getElevationUnit().hashCode();
        }
        if (getFeelsLike() != null) {
            _hashCode += getFeelsLike().hashCode();
        }
        if (getGustTime() != null) {
            _hashCode += getGustTime().hashCode();
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
        if (getGustWindDirectionDegrees() != null) {
            _hashCode += getGustWindDirectionDegrees().hashCode();
        }
        if (getHumidity() != null) {
            _hashCode += getHumidity().hashCode();
        }
        if (getHumidityUnit() != null) {
            _hashCode += getHumidityUnit().hashCode();
        }
        if (getHumidityHigh() != null) {
            _hashCode += getHumidityHigh().hashCode();
        }
        if (getHumidityLow() != null) {
            _hashCode += getHumidityLow().hashCode();
        }
        if (getHumidityRate() != null) {
            _hashCode += getHumidityRate().hashCode();
        }
        if (getInputLocationUrl() != null) {
            _hashCode += getInputLocationUrl().hashCode();
        }
        _hashCode += getMoonPhase();
        if (getMoonPhaseImage() != null) {
            _hashCode += getMoonPhaseImage().hashCode();
        }
        if (getPressure() != null) {
            _hashCode += getPressure().hashCode();
        }
        if (getPressureUnit() != null) {
            _hashCode += getPressureUnit().hashCode();
        }
        if (getPressureHigh() != null) {
            _hashCode += getPressureHigh().hashCode();
        }
        if (getPressureLow() != null) {
            _hashCode += getPressureLow().hashCode();
        }
        if (getPressureRate() != null) {
            _hashCode += getPressureRate().hashCode();
        }
        if (getPressureRateUnit() != null) {
            _hashCode += getPressureRateUnit().hashCode();
        }
        if (getLight() != null) {
            _hashCode += getLight().hashCode();
        }
        if (getLightRate() != null) {
            _hashCode += getLightRate().hashCode();
        }
        if (getIndoorTemperature() != null) {
            _hashCode += getIndoorTemperature().hashCode();
        }
        if (getIndoorTemperatureRate() != null) {
            _hashCode += getIndoorTemperatureRate().hashCode();
        }
        _hashCode += new Double(getLatitude()).hashCode();
        _hashCode += new Double(getLongitude()).hashCode();
        if (getObDate() != null) {
            _hashCode += getObDate().hashCode();
        }
        if (getObDateTime() != null) {
            _hashCode += getObDateTime().hashCode();
        }
        if (getRainMonth() != null) {
            _hashCode += getRainMonth().hashCode();
        }
        if (getRainRate() != null) {
            _hashCode += getRainRate().hashCode();
        }
        if (getRainRateMax() != null) {
            _hashCode += getRainRateMax().hashCode();
        }
        if (getRainRateUnit() != null) {
            _hashCode += getRainRateUnit().hashCode();
        }
        if (getRainToday() != null) {
            _hashCode += getRainToday().hashCode();
        }
        if (getRainUnit() != null) {
            _hashCode += getRainUnit().hashCode();
        }
        if (getRainYear() != null) {
            _hashCode += getRainYear().hashCode();
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
        if (getStationURL() != null) {
            _hashCode += getStationURL().hashCode();
        }
        if (getSunrise() != null) {
            _hashCode += getSunrise().hashCode();
        }
        if (getSunset() != null) {
            _hashCode += getSunset().hashCode();
        }
        if (getTemperature() != null) {
            _hashCode += getTemperature().hashCode();
        }
        if (getTemperatureHigh() != null) {
            _hashCode += getTemperatureHigh().hashCode();
        }
        if (getTemperatureLow() != null) {
            _hashCode += getTemperatureLow().hashCode();
        }
        if (getTemperatureRate() != null) {
            _hashCode += getTemperatureRate().hashCode();
        }
        if (getTemperatureRateUnit() != null) {
            _hashCode += getTemperatureRateUnit().hashCode();
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
        if (getWetBulb() != null) {
            _hashCode += getWetBulb().hashCode();
        }
        if (getWindDirection() != null) {
            _hashCode += getWindDirection().hashCode();
        }
        if (getWindDirectionAvg() != null) {
            _hashCode += getWindDirectionAvg().hashCode();
        }
        if (getWindDirectionDegrees() != null) {
            _hashCode += getWindDirectionDegrees().hashCode();
        }
        if (getWindSpeed() != null) {
            _hashCode += getWindSpeed().hashCode();
        }
        if (getWindSpeedAvg() != null) {
            _hashCode += getWindSpeedAvg().hashCode();
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
        new org.apache.axis.description.TypeDesc(LiveWeatherData.class, true);

    static {
        typeDesc.setXmlType(new javax.xml.namespace.QName("http://api.wxbug.net/", "LiveWeatherData"));
        org.apache.axis.description.ElementDesc elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("auxTemperature");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "AuxTemperature"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("auxTemperatureRate");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "AuxTemperatureRate"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
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
        elemField.setFieldName("dewPoint");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "DewPoint"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("elevation");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Elevation"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "int"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("elevationUnit");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "ElevationUnit"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("feelsLike");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "FeelsLike"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("gustTime");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GustTime"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "dateTime"));
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
        elemField.setFieldName("gustWindDirectionDegrees");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "GustWindDirectionDegrees"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("humidity");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Humidity"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("humidityUnit");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "HumidityUnit"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("humidityHigh");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "HumidityHigh"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("humidityLow");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "HumidityLow"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("humidityRate");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "HumidityRate"));
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
        elemField.setFieldName("moonPhase");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "MoonPhase"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "int"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("moonPhaseImage");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "MoonPhaseImage"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("pressure");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Pressure"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("pressureUnit");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "PressureUnit"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("pressureHigh");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "PressureHigh"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("pressureLow");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "PressureLow"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("pressureRate");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "PressureRate"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("pressureRateUnit");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "PressureRateUnit"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("light");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Light"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("lightRate");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "LightRate"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("indoorTemperature");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "IndoorTemperature"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("indoorTemperatureRate");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "IndoorTemperatureRate"));
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
        elemField.setFieldName("rainMonth");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "RainMonth"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("rainRate");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "RainRate"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("rainRateMax");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "RainRateMax"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("rainRateUnit");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "RainRateUnit"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
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
        elemField.setFieldName("rainYear");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "RainYear"));
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
        elemField.setFieldName("stationURL");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "StationURL"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("sunrise");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Sunrise"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "dateTime"));
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("sunset");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "Sunset"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "dateTime"));
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
        elemField.setFieldName("temperatureHigh");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "TemperatureHigh"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("temperatureLow");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "TemperatureLow"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("temperatureRate");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "TemperatureRate"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("temperatureRateUnit");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "TemperatureRateUnit"));
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
        elemField.setFieldName("wetBulb");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "WetBulb"));
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
        elemField.setFieldName("windDirectionAvg");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "WindDirectionAvg"));
        elemField.setXmlType(new javax.xml.namespace.QName("http://www.w3.org/2001/XMLSchema", "string"));
        elemField.setMinOccurs(0);
        elemField.setNillable(false);
        typeDesc.addFieldDesc(elemField);
        elemField = new org.apache.axis.description.ElementDesc();
        elemField.setFieldName("windDirectionDegrees");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "WindDirectionDegrees"));
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
        elemField.setFieldName("windSpeedAvg");
        elemField.setXmlName(new javax.xml.namespace.QName("http://api.wxbug.net/", "WindSpeedAvg"));
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
