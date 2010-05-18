package com.sihus.core.util;


public class JmailUtil {
	
	public static boolean sendGetPasswordMail(String sendTo,String mailText){
//		String htmlText = "<html><a href='"+mailText+"'>修改密码地址:"+mailText+"</a></html>";
//		try {
//			JavaMailSenderImpl sender = new JavaMailSenderImpl();
//			sender.setHost("smtp.163.com");
//			sender.setPassword("665123");   
//			sender.setUsername("fuxianghao");  
//			MimeMessage message = sender.createMimeMessage();   
//			MimeMessageHelper messageHelp = new MimeMessageHelper(message,true,"GBK");
//			messageHelp.setFrom("fuxianghao@163.com");   
//		    messageHelp.setTo(sendTo);   
//		    messageHelp.setSubject("找回密码[邻里间]");
//		    messageHelp.setText(htmlText, true);  
//		    Properties prop = new Properties(); 
//		    prop.setProperty("mail.smtp.auth", "true"); 
//			sender.setJavaMailProperties(prop);
//			sender.send(message);
//		} catch (MessagingException e) {
//			e.printStackTrace();
//			return false;
//		}   
		return true;
	}
	public static void main(String[] args){
		sendGetPasswordMail("fuxianghao@163.com","日日");
		System.out.println("susess");
	}
}   

