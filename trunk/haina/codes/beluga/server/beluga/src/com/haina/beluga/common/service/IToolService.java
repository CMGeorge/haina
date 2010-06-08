package com.haina.beluga.common.service;

/**
 * 工具操作业务接口。<br/>
 * @author huangyongqiang
 * @since 2010-05-31
 */
public interface IToolService {

	/**
	 * 以字节形式取得本地文件
	 * @param filePath 本地文件路径
	 * @return
	 */
	byte[] getLocalFileForByte(String filePath);
	
	/**
	 * 以字符形式取得本地文件
	 * @param filePath 本地文件路径
	 * @return
	 */
	String getLocalFileForString(String filePath);
	
	/**
	 * 生成本度文件
	 * @param filePath 本地文件路径
	 * @param fileData 文件数据
	 */
	void createLocalFileString(String filePath, byte[] fileData);
	
	/**
	 * 以字节形式取得FTP服务器上的文件
	 * @param filePath ftp服务器上的文件路径
	 * @return
	 */
	byte[] geFtpFileForByte(String filePath);
	
	/**
	 * 以字符形式取得FTP服务器上的文件
	 * @param filePath ftp服务器上的文件路径
	 * @return
	 */
	byte[] geFtpFileForString(String filePath);
}
