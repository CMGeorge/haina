package com.haina.beluga.contact.domain;

import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;
import org.springframework.stereotype.Component;

import com.sihus.core.model.VersionalModel;

/**
 * 
 * @hibernate.class table="Weather"
 * @hibernate.cache usage="read-write"
 */
public class Weather extends VersionalModel {

	private static final long serialVersionUID = -326286128859607526L;
	private String date;/* 天气日期 */
	private String weatherCityCode;
	private String weatherType;
	private Boolean isNight;
	private Integer high;
	private Integer low;
	private String wind;
	private String icon;
	private String issuetime;/* 发布时间 */

	/**
	 * @hibernate.property column="date" length="20"
	 * @return String
	 */
	public String getDate() {
		return date;
	}

	public void setDate(String date) {
		this.date = date;
	}

	/**
	 * @hibernate.property column="weatherCityCode" length="20"
	 * @return String
	 */
	public String getWeatherCityCode() {
		return weatherCityCode;
	}

	public void setWeatherCityCode(String weatherCityCode) {
		this.weatherCityCode = weatherCityCode;
	}

	/**
	 * @hibernate.property column="weatherType" length="50"
	 * @return String
	 */
	public String getWeatherType() {
		return weatherType;
	}

	public void setWeatherType(String weatherType) {
		this.weatherType = weatherType;
	}

	/**
	 * @hibernate.property column="high"
	 * @return Integer
	 */
	public Integer getHigh() {
		return high;
	}

	public void setHigh(Integer high) {
		this.high = high;
	}

	/**
	 * @hibernate.property column="low"
	 * @return Integer
	 */
	public int getLow() {
		return low;
	}

	public void setLow(int low) {
		this.low = low;
	}

	/**
	 * @hibernate.property column="wind" length="30"
	 * @return String
	 */
	public String getWind() {
		return wind;
	}

	public void setWind(String wind) {
		this.wind = wind;
	}

	/**
	 * @hibernate.property column="icon" length="30"
	 * @return String
	 */
	public String getIcon() {
		return icon;
	}

	public void setIcon(String icon) {
		this.icon = icon;
	}

	/**
	 * @hibernate.property column="isNight"
	 * @return boolean
	 */
	public Boolean getNight() {
		return isNight;
	}

	public void setNight(Boolean isNight) {
		this.isNight = isNight;
	}

	/**
	 * @hibernate.property column="issuetime" length="10"
	 * @return String
	 */
	public String getIssuetime() {
		return issuetime;
	}

	public void setIssuetime(String issuetime) {
		this.issuetime = issuetime;
	}

	/**
	 * @hibernate.version column="VERSION" type="long"
	 * @return long
	 */
	@Override
	public long getVersion() {
		// TODO Auto-generated method stub
		return version;
	}

	/**
	 * @hibernate.id unsaved-value="null" length="32"
	 * @hibernate.column name="id" sql-type="char(32)"
	 * @hibernate.generator class="uuid.hex"
	 * @return String
	 */
	@Override
	public String getId() {
		return id;
	}

	/**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof Weather)) {
			return false;
		}
		Weather rhs = (Weather) object;
		return new EqualsBuilder().append(this.icon, rhs.icon).append(
				this.wind, rhs.wind).append(this.isNight, rhs.isNight).append(
				this.weatherCityCode, rhs.weatherCityCode).append(this.high,
				rhs.high).append(this.weatherType, rhs.weatherType).append(
				this.issuetime, rhs.issuetime).append(this.low, rhs.low)
				.append(this.date, rhs.date).isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(1239287031, 192974521).append(this.icon)
				.append(this.wind).append(this.isNight).append(
						this.weatherCityCode).append(this.high).append(
						this.weatherType).append(this.issuetime).append(
						this.low).append(this.date).toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("weatherCityCode",
				this.weatherCityCode).append("icon", this.icon).append(
				"issuetime", this.issuetime).append("low", this.low).append(
				"date", this.date).append("wind", this.wind).append("night",
				this.getNight()).append("weatherType", this.weatherType).append(
				"high", this.high).append("id", this.getId()).toString();
	}

}
