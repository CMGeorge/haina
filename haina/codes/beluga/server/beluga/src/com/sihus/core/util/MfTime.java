package com.sihus.core.util;

import java.sql.Timestamp;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;

import org.springframework.util.Assert;

/**
 * @author X_FU
 * 时间类操作，不包括年月日
 */
public class MfTime {
    private final static int maxHour = 23;
    private final static int maxMinute = 59;
    /**
     * @see java.util.GregorianCalendar
     */
    private Calendar  base =  Calendar.getInstance();
    public MfTime() {
        this.init();
    }
    public MfTime(final Calendar arg) {
        Assert.notNull(arg, "arg must be not null");
        base = arg;
        this.init();
    }
    public MfTime(final Date arg) {
    	base.setTime(arg);
        Assert.notNull(arg, "arg must be not null");
        this.init();
    }
    private void init() {
//        base.set(Calendar.YEAR , c.get(Calendar.YEAR));
//        base.set(Calendar.MONTH , c.get(Calendar.MONTH));
//        base.set(Calendar.DAY_OF_MONTH , c.get(Calendar.DAY_OF_MONTH));
    }
    public MfTime(final int hour , final int minute) {
        Assert.notNull(hour);
        Assert.notNull(minute);
        Assert.isTrue(hour <= maxHour && hour >= 0, "0 <= hour <=23");
        Assert.isTrue(minute <= maxMinute && minute >= 0,
            "0<=minute <= 59");
        base.set(Calendar.HOUR_OF_DAY , hour);
        base.set(Calendar.MINUTE , minute);
        base.set(Calendar.SECOND , 00);
        base.set(Calendar.MILLISECOND , 00);
       this.init();
    }
    public MfTime(final Timestamp timestamp) {
       // java.util.Calendar result = Calendar.getInstance();
        base = Calendar.getInstance();
        base.setTime(timestamp);
        this.init();
    }
    /*
     * @param args
     * eg:args = 15:30
     */
    public MfTime(String argsNoSecond) {
        try {
       base.setTime(new MfDate(new MfDate(1900,1,1)
            .toString()
            + " " + argsNoSecond,
            "yyyy-MM-dd HH:mm").getTime());
        } catch (IllegalArgumentException e) {
            throw e;
        }
    }
    public MfTime(String timestampString , String patten) {
        try {
            MfDate mfDate = new MfDate(timestampString , patten);
            base.setTime(mfDate.getTime());
        } catch (IllegalArgumentException e) {
            e.printStackTrace();
            throw e;
        }
    }
    public int compareTo(MfTime mfTime) {
       // base.get
        if (this.isAllZero() && mfTime.isAllZero()) {
            return 0;
        }
        if (this.isAllZero()) {
            return 1;
        }
        if (mfTime.isAllZero()) {
            return -1;
        }
        return this.getTime().compareTo(mfTime.getTime());
        }
    /**
     * 分钟增运算.
     * @param arg 需增的分钟
     * @exception IllegalArgumentException
     */
    public void addMinutes(final int arg) {
        if (0 > arg) {
            throw new  IllegalArgumentException("arg must be >0");
        }
        base.add(GregorianCalendar.MINUTE, arg);
    }
    /**
     * 天增运算.
     * @param arg 需增的天
     * @exception IllegalArgumentException
     */
    public void addDay(final int arg) {
        if (0 > arg) {
            throw new  IllegalArgumentException("arg must be >0");
        }
        base.add(GregorianCalendar.DAY_OF_MONTH, arg);
    }
    /**
     * 分钟减运算.
     * @param arg
     *            需减的分钟
     * @exception IllegalArgumentException
     */
    public void minusMinutes(final int arg) {
        if (0 > arg) {
            throw new  IllegalArgumentException("arg must be >0");
        }
        base.add(GregorianCalendar.MINUTE, -arg);
    }
    public MfDate toMfDate() {
       return new MfDate(base.getTime());
    }
    public boolean equals(Object mftime) {
        if (!(mftime instanceof MfTime)) {
            return false;
        }
        return this.compareTo((MfTime)mftime) == 0;
    }
    public String toString() {
    	int hour = base.get(Calendar.HOUR_OF_DAY);
    	int minute =  base.get(Calendar.MINUTE);
    	String sHour = hour < 10 ? "0" + hour : String.valueOf(hour);
    	String sMinute = minute < 10 ? "0" + minute : String.valueOf(minute);
       return sHour + ":" + sMinute;
    }
    public static String toNow() {
    	Calendar c = Calendar.getInstance();
         c.setTimeInMillis(System.currentTimeMillis());
    	int hour = c.get(Calendar.HOUR_OF_DAY);
    	int minute =  c.get(Calendar.MINUTE);
    	String sHour = hour < 10 ? "0" + hour : String.valueOf(hour);
    	String sMinute = minute < 10 ? "0" + minute : String.valueOf(minute);
       return sHour + ":" + sMinute;
    }
    public Boolean isAllZero() {
        return (this.getHour() == 0 && this.getMinute() == 0 && this
            .getSecond() == 0);
    }
    public int getHour() {
        return base.get(Calendar.HOUR_OF_DAY);
    }
    public int getMinute() {
        return base.get(Calendar.MINUTE);
    }
    public int getSecond() {
        return base.get(Calendar.SECOND);
    }
    public Date getTime() {
        return base.getTime();
    }
    
    public static void main(String[] args){
    	MfTime base =  new MfTime(new Date());
    	 System.out.println(base.getTime().getTime());
		base.addDay(8);
		 System.out.println(base.getTime().getTime());
		 System.out.println(new Date());
    }
   
}
