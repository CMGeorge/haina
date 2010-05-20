package com.sihus.core.util;

import java.util.Random;

public final class StringUtils {

	public static boolean isNullOrEmpty(String s) {
		if (null == s || s.trim().length()<1) {
			return true;
		}
		return false;
	}
	
	public static String htmlEncode(String s) {
		if (s == null)
			return "";
		s = s.replace("&", "&amp;");
		s = s.replace("\"", "&quot;");
		s = s.replace("<", "&lt;");
		s = s.replace(">", "&gt;");
		s = s.replace(" ", "&nbsp;");
		s = s.replace("\n", "<br>");
		s = s.replace("'", "&#039;");
		s = s.replace("\\", "\\\\");

		return s;
	}

	public static String getRandom(int length) {
		StringBuffer buffer = new StringBuffer(
				"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
		StringBuffer sb = new StringBuffer();
		Random r = new Random();
		int range = buffer.length();
		for (int i = 0; i < length; i++) {
			sb.append(buffer.charAt(r.nextInt(range)));
		}
		return sb.toString();
	}

	public static boolean isNull(String str) {
		return (str == null || str.equals("") || str.equals("null")) ? true
				: false;
	}

	/**
	 * 返回一个字符串中包含几个 spit
	 * 
	 * @param str
	 * @param spit
	 * @return
	 */
	public static int lenOf(String str, String spit) {
		if (isNull(str))
			return 0;
		int l = str.split(spit).length;
		if (l > 0)
			return l - 1;
		return l;
	}

	/**
	 *过滤XML.
	 * 
	 * @param str
	 * @return
	 */
	public static String filterXML(String str) {
		return "<![CDATA[" + str + "]]>";
	}

	/**
	 *验证不能半角，数字
	 * 
	 * @param name
	 * @return
	 */
	public static boolean validateDBC(String name) {
		boolean isValidate = true;
		// if (!name.matches(Constants.REGEX)) {
		// isValidate = false;
		// }
		return isValidate;
	}

	public static void main(String args[]) {
		System.out.println(lenOf(null, "1"));
		System.out.println(lenOf("", "1"));
		System.out.println(lenOf("1,", "1"));
		System.out.println(lenOf("1,1,", "1"));
	}

}
