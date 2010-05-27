package com.sihus.core.util;

import java.text.DateFormat;
import java.text.DateFormatSymbols;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.Locale;

/**
 * ����������
 */
public final class DateUtil {
	
	public static final String DEFAULT_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
	
	private DateUtil() {
		// util class, prevent from new instance
	}

	public static String toString(Date source, String format) {
		String dateStr = null;
		if (source != null) {
			DateFormat df = new SimpleDateFormat(StringUtils
					.isNull(format) ? DEFAULT_DATE_FORMAT : format.trim());
			dateStr = df.format(source);
		}
		return dateStr;
	}

	public static String toDefaultString(Date source) {
		return toString(source, null);
	}
	
    /**
     * ��ʽ����ǰʱ��.
     * @param pattern - ���ڸ�ʽ
     * @return the formatted date-time string
     * @see java.text.SimpleDateFormat
     */
	public static String formatDateTime(String pattern) {
		java.text.SimpleDateFormat sdf = new java.text.SimpleDateFormat(pattern);
		String now = sdf.format(new java.util.Date());
		return now;
	}

    /**
     * ��ʽ������ʱ��.
     * @param pattern - ���ڸ�ʽ
     * @param date - ��Ҫ��ʽ��������
     * @return the formatted date-time string
     * @see java.text.SimpleDateFormat
     */
    public static String formatDateTime(String pattern, java.util.Date date) {
        String strDate = null;
        String strFormat = pattern;
        SimpleDateFormat dateFormat = null;

        if (date == null) return "";

        dateFormat = new SimpleDateFormat(strFormat);
        strDate = dateFormat.format(date);

        return strDate;
    }

    /**
     * ��ʽ������ʱ��.
     * @param pattern - ���ڸ�ʽ
     * @param date - ��Ҫ��ʽ��������
     * @param locale - the locale whose date format symbols should be used
     * @return the formatted date-time string
     * @see java.text.SimpleDateFormat
     */
    public static String formatDateTime(String pattern, java.util.Date date, Locale locale) {
        String strDate = null;
        String strFormat = pattern;
        SimpleDateFormat dateFormat = null;

        if (date == null) return "";

        dateFormat = new SimpleDateFormat(strFormat, locale);
        strDate = dateFormat.format(date);

        return strDate;
    }

    /**
     * Parses a string to produce a Date.
     * @param pattern - the pattern of the string
     * @param strDateTime - the string to be parsed
     * @return A Date parsed from the string. In case of error, returns null.
     */
    public static java.util.Date parse(String pattern, String strDateTime) {
        java.util.Date date = null;
        if (strDateTime == null || pattern == null) return null;
        try {
            SimpleDateFormat formatter = new SimpleDateFormat(pattern);
            formatter.setLenient(false);
            date = formatter.parse(strDateTime);
        }
        catch (ParseException e) {
            e.printStackTrace();
        }
        return date;
    }

    /**
     * �����ں�ʱ��ƴ��һ��
     * @param date - the date
     * @param time - the time
     * @return the composed datetime
     */
    public static Date composeDate(Date date, Date time) {
        if (date == null || time == null) return null;
        Calendar c1 = Calendar.getInstance();
        c1.setTime(date);
        Calendar c2 = Calendar.getInstance();
        c2.setTime(time);
        Calendar c3 = Calendar.getInstance();
        c3.set(c1.get(Calendar.YEAR), c1.get(Calendar.MONTH), c1.get(Calendar.DATE), c2.get(Calendar.HOUR_OF_DAY), c2.get(Calendar.MINUTE), c2.get(Calendar.SECOND));
        return c3.getTime();
    }

    /**
     * ȡ�����ڲ���
     * @param date
     * @return
     */
    public static Date getTheDate(Date date) {
        if (date == null) return null;
        Calendar c1 = Calendar.getInstance();
        c1.setTime(date);
        Calendar c2 = Calendar.getInstance();
        c2.set(c1.get(Calendar.YEAR), c1.get(Calendar.MONTH), c1.get(Calendar.DATE), 0, 0, 0);
        long millis = c2.getTimeInMillis();
        millis = millis - millis % 1000;
        c2.setTimeInMillis(millis);
        return c2.getTime();
    }

    /**
     * ����ƫ������(����գ�
     * @param d
     * @param skipDay ƫ������,�����ʾ���ƫ��,�����ʾ��ǰƫ��
     * @return
     */
    public static Date skipDateTime(Date d, int skipDay) {
        if (d == null) return null;
        Calendar calendar = Calendar.getInstance();
        calendar.setTime(d);
        calendar.add(Calendar.DATE, skipDay);
        return calendar.getTime();
    }
    
    /**
     * ĳһʱ���ƫ������(����գ�
     */
    public static String skipDateTime(String timeStr, int skipDay) {
        String pattern = "yyyy-MM-dd HH:mm:ss";
        Date d = parse(pattern, timeStr);
        Date date = skipDateTime(d, skipDay);
        return formatDateTime(pattern, date);
    }

    /**
     * ����ƫ������(����գ�
     */
    public static String skipDate(String dateStr, int skipDay) {
        String pattern = "yyyy-MM-dd";
        Date d = parse(pattern, dateStr);
        Date date = skipDateTime(d, skipDay);
        return formatDateTime(pattern, date);
    }

    /**
     * ĳһʱ���ƫ������(����ա���Сʱ�����֣�
     */
    public static String getTime(String timeStr, int skipDay, int skipHour,
            int skipMinute) {
        if (null == timeStr) {
            return null;
        }

        GregorianCalendar cal = new GregorianCalendar();
        cal.setTime(parse("yyyy-MM-dd HH:mm:ss", timeStr));

        cal.add(GregorianCalendar.DAY_OF_MONTH, skipDay);
        cal.add(GregorianCalendar.HOUR_OF_DAY, skipHour);
        cal.add(GregorianCalendar.MINUTE, skipMinute);
        cal.get(GregorianCalendar.DAY_OF_WEEK_IN_MONTH);

        SimpleDateFormat dateFormat = new SimpleDateFormat(
                "yyyy-MM-dd HH:mm:ss");

        return dateFormat.format(cal.getTime());
    }

	/**
	 * �Ƚϵ�ǰ���ں�ָ������ return boolean ���ǰ������ָ������֮�󷵻�true���򷵻�flase
	 */
	public static boolean dateCompare(String str) {
		boolean bea = false;
		SimpleDateFormat sdf_d = new SimpleDateFormat("yyyy-MM-dd");
		String isDate = sdf_d.format(new java.util.Date());
		java.util.Date date1;
		java.util.Date date0;
		try {
			date1 = sdf_d.parse(str);
			date0 = sdf_d.parse(isDate);
			if (date0.after(date1)) {
				bea = true;
			}
		}
		catch (ParseException e) {
			bea = false;
		}
		return bea;
	}

	/**
	 * �Ƚϵ�ǰ�·ݺ�ָ���·� ���ǰ�·���ָ���·�֮�󷵻�true���򷵻�flase
	 */
	public static boolean monthCompare(String str) {
		boolean bea = false;
		SimpleDateFormat sdf_m = new SimpleDateFormat("yyyy-MM");
		String isMonth = sdf_m.format(new java.util.Date());
		java.util.Date date1;
		java.util.Date date0;
		try {
			date1 = sdf_m.parse(str);
			date0 = sdf_m.parse(isMonth);
			if (date0.after(date1)) {
				bea = true;
			}
		}
		catch (ParseException e) {
			bea = false;
		}
		return bea;
	}

	/**
	 * �Ƚϵ�ǰ���ں�ָ������ return boolean ���ǰ������ָ������֮�󷵻�true���򷵻�flase
	 */
	public static boolean secondCompare(String str) {
		boolean bea = false;
		SimpleDateFormat sdf_d = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		String isDate = sdf_d.format(new java.util.Date());
		java.util.Date date1;
		java.util.Date date0;
		try {
			date1 = sdf_d.parse(str);
			date0 = sdf_d.parse(isDate);
			if (date0.after(date1)) {
				bea = true;
			}
		}
		catch (ParseException e) {
			bea = false;
		}
		return bea;
	}

	/**
	 * �Ƚ�ָ��}�������str1����str2��return true;
	 * 
	 * @param str1
	 * @param str2
	 * @return
	 */
	public static boolean secondCompare(String str1, String str2) {
		boolean bea = false;
		SimpleDateFormat sdf_d = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		java.util.Date date1;
		java.util.Date date0;
		try {
			date1 = sdf_d.parse(str1);
			date0 = sdf_d.parse(str2);
			if (date0.after(date1)) {
				bea = true;
			}
		}
		catch (ParseException e) {
			bea = false;
		}
		return bea;
	}

	/**
	 * ���ü����󷵻�ʱ��
	 * 
	 * @param type ������� �������
	 * @param ������� ����1�����һ��
	 * @return
	 */
	public static String dateAdd(String type, int i) {
		SimpleDateFormat df = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		String str = formatDateTime("yyyy-MM-dd HH:mm:ss");
		Calendar c = Calendar.getInstance(); // ��ʱ�����ں�ʱ��
		if (type.equals("s")) {
			int s = c.get(Calendar.SECOND);
			s = s + i;
			c.set(Calendar.SECOND, s);
			str = df.format(c.getTime());
		}
		else if (type.equals("d")) {
			int d = c.get(Calendar.DAY_OF_MONTH); // ȡ���ա���
			d = d + i;
			c.set(Calendar.DAY_OF_MONTH, d); // �����ա������û�ȥ
			str = df.format(c.getTime());
		}
		return str;
	}

	/**
	 * ʱ��ƫ������
	 */
	public static String getTime(int skipDay) {
		GregorianCalendar cal = new GregorianCalendar();
		cal.setTime(new Date());

		cal.add(GregorianCalendar.DAY_OF_MONTH, skipDay);
		SimpleDateFormat dateFormat = new SimpleDateFormat(
				"yyyy-MM-dd HH:mm:ss");

		return dateFormat.format(cal.getTime());
	}

	/**
	 * ������������
	 */
	public static long subtraction(Date minuend, Date subtrahend) {

		long daterange = minuend.getTime() - subtrahend.getTime();
		long time = 1000 * 3600 * 24;

		return (daterange % time == 0) ? (daterange / time)
				: (daterange / time + 1);
	}

	public static long getM(Date date) {
		GregorianCalendar cal = new GregorianCalendar();
		cal.setTime(date);
		return cal.get(GregorianCalendar.DAY_OF_WEEK);
	}

	public static String getLastDate(String temp) {
		// ��temp�ǿ�����ǰ������
		if (temp == null || temp.equals("")) {
			temp = "1";
		}
		int i = Integer.parseInt(temp);
		DateFormat dateFormat = DateFormat.getDateInstance(DateFormat.MEDIUM);
		Calendar grc = Calendar.getInstance();
		grc.add(GregorianCalendar.DATE, -i);
		return dateFormat.format(grc.getTime());
	}

	/**
	 * ��ȡ��һ������ڣ���4���ò�ѯ���������
	 */
	public static String getLastYearDate() {

		Calendar c = Calendar.getInstance();
		int y = c.get(Calendar.YEAR);
		String year = String.valueOf(y - 1);
		return year;
	}

	/**
	 * ��ȡ�ϸ��µ����ڣ���4���ò�ѯ���������
	 */
	public static String getLastMonthDate() {

		Calendar c = Calendar.getInstance();
		int y = c.get(Calendar.YEAR);
		int m = c.get(Calendar.MONTH) + 1;
		String month = null;
		String year = String.valueOf(y);
		if (m > 1) {
			if (m > 10) {
				month = String.valueOf(m - 1);
			}
			else {
				month = "0" + String.valueOf(m - 1);
			}
		}
		else {
			year = String.valueOf(y - 1);
			month = String.valueOf(12);
		}

		return year + "-" + month;
	}

	/**
	 * ��ȡǰһ������ڣ���4���ò�ѯ���������
	 * 
	 * @return
	 */
	public static String getLastDayDate() {
		Calendar c = Calendar.getInstance();
		int y = c.get(Calendar.YEAR);
		int m = c.get(Calendar.MONTH) + 1;
		int d = c.get(Calendar.DAY_OF_MONTH);
		int days = 0;
		if (m > 1) {
			days = getMonthsDays(m - 1, y);
		}
		else {
			days = 31;
		}
		String day = null;
		String month = null;
		String year = String.valueOf(y);
		if (d > 1) { // ����1��
			day = String.valueOf(d - 1);
			if (m > 9) {
				month = String.valueOf(m);
			}
			else {
				month = "0" + String.valueOf(m);
			}
		}
		else if ((d < 2) && (m < 2)) { // һ��һ��
			day = String.valueOf(31);
			month = String.valueOf(12);
			year = String.valueOf(y - 1);
		}
		else if ((d < 2) && (m > 2)) {
			day = String.valueOf(days);
			if (m > 10) {
				month = String.valueOf(m - 1);
			}
			else {
				month = "0" + String.valueOf(m - 1);
			}
		}

		return year + "-" + month + "-" + day;
	}

	/**
	 * �ж��Ƿ�����
	 */
	public static boolean isLeapYear(int year) {
		if ((((year % 4) == 0) && ((year % 100) != 0)) || ((year % 4) == 0)
				&& ((year % 400) == 0)) {
			return true;
		}
		else {
			return false;
		}
	}

	/**
	 * ��ȡÿ���µ�����
	 * 
	 * @param month
	 * @param year
	 * @return
	 */
	public static int getMonthsDays(int month, int year) {
		if ((isLeapYear(year) == true) && (month == 2)) {
			return 29;
		}
		else if ((isLeapYear(year) == false) && (month == 2)) {
			return 28;
		}

		if ((month == 1) || (month == 3) || (month == 5) || (month == 7)
				|| (month == 8) || (month == 10) || (month == 12)) {
			return 31;
		}
		return 30;
	}

	public static String getWeekDay() {
		DateFormatSymbols symboles = new DateFormatSymbols(Locale.getDefault());
		symboles.setShortWeekdays(new String[] { "", "7", "1", "2", "3", "4",
				"5", "6" });
		SimpleDateFormat date = new SimpleDateFormat("E", symboles);
		return date.format(new Date());
	}

	/**
	 * ��ȡ��
	 */
	public static int getYear() {
		Calendar c = Calendar.getInstance();
		return c.get(Calendar.YEAR);
	}

	/**
	 * ��ȡ��
	 * 
	 * @return
	 */
	public static int getMonth() {
		Calendar c = Calendar.getInstance();
		return c.get(Calendar.MONTH);
	}

	/**
	 * ��ȡ��
	 * 
	 * @return
	 */
	public static int getDay() {
		Calendar c = Calendar.getInstance();
		return c.get(Calendar.DAY_OF_MONTH);
	}

	public static String getLastMonthDay(int lastmonths) {
		int month = getMonth() + 1;
		if (month - lastmonths > 0) {
			return String.valueOf(getYear()) + "-"
					+ String.valueOf(month - lastmonths) + "-1";
		}
		else {
			return String.valueOf(getYear() - 1) + "-"
					+ String.valueOf(12 + month - lastmonths) + "-1";
		}
	}

	public static void main(String args[]) {

	}
}
