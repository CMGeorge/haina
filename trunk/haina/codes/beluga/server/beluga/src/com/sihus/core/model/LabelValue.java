package com.sihus.core.model;

import java.util.Comparator;

import org.apache.commons.lang.builder.CompareToBuilder;
import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;


/**
 * 标签-值对类�。<br/>
 * @author huangyongqiang
 * @version 1.0
 * @since 1.0
 * @date 2009-05-20
 */
public class LabelValue implements Comparable<Object>, IModel {

    /**
	 * 
	 */
	private static final long serialVersionUID = -3451700136910499609L;

	/*The property which supplies the option label visible to the end user.*/
    private String label = null;

    /* The property which supplies the value returned to the server.*/
    private String value = null;
    
    
    /**
     * Comparator that can be used for a case insensitive sort of
     * <code>LabelValue</code> objects.
     */
    public static final Comparator<Object> CASE_INSENSITIVE_ORDER = new Comparator<Object>() {
        public int compare(Object o1, Object o2) {
            String label1 = ((LabelValue) o1).getLabel();
            String label2 = ((LabelValue) o2).getLabel();
            return label1.compareToIgnoreCase(label2);
        }
    };


    // ----------------------------------------------------------- Constructors


    /**
     * Default constructor.
     */
    public LabelValue() {
        super();
    }

    /**
     * Construct an instance with the supplied property values.
     *
     * @param label The label to be displayed to the user.
     * @param value The value to be returned to the server.
     */
    public LabelValue(String label, String value) {
        this.label = label;
        this.value = value;
    }

    public String getLabel() {
        return this.label;
    }

    public void setLabel(String label) {
        this.label = label;
    }

    public String getValue() {
        return this.value;
    }

    public void setValue(String value) {
        this.value = value;
    }

    

    /**
	 * @see java.lang.Object#equals(Object)
	 */
	public boolean equals(Object object) {
		if (!(object instanceof LabelValue)) {
			return false;
		}
		LabelValue rhs = (LabelValue) object;
		return new EqualsBuilder().appendSuper(super.equals(object)).append(
				this.value, rhs.value).append(this.label, rhs.label).isEquals();
	}

	/**
	 * @see java.lang.Object#hashCode()
	 */
	public int hashCode() {
		return new HashCodeBuilder(-1792537989, -2016016415).appendSuper(
				super.hashCode()).append(this.value).append(this.label)
				.toHashCode();
	}

	/**
	 * @see java.lang.Object#toString()
	 */
	public String toString() {
		return new ToStringBuilder(this).append("value", this.value).append(
				"label", this.label).toString();
	}

	/**
	 * @see java.lang.Comparable#compareTo(Object)
	 */
	public int compareTo(Object object) {
		LabelValue myClass = (LabelValue) object;
		return new CompareToBuilder().append(this.value, myClass.value).append(
				this.label, myClass.label).toComparison();
	}
    
    
}
