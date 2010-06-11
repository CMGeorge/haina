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
	 * 生成本地文件
	 * @param filePath 本地文件路径
	 * @param fileData 文件数据
	 */
	void createLocalFile(String filePath, byte[] fileData);
	
	/**
	 * 生成本地缩略图文件
	 * @param sourceData 原始图片数据
	 * @param targetFilePath 目标文件存放路径
	 * @param targetWidth 目标宽度
	 * @param targetHeight 目标高度
	 * @throws Exception
	 */
	void createLocalThumbnailFile(byte[] sourceData, String targetFilePath, 
			int targetWidth, int targetHeight) throws Exception;
	
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
