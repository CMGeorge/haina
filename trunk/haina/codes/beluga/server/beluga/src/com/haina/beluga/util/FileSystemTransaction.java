package com.haina.beluga.util;

import java.io.File;
import java.io.IOException;
import java.util.Map;
import java.util.UUID;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.apache.commons.transaction.file.FileResourceManager;
import org.apache.commons.transaction.file.ResourceManager;
import org.apache.commons.transaction.file.ResourceManagerException;
import org.apache.commons.transaction.file.ResourceManagerSystemException;
import org.apache.commons.transaction.memory.HashMapFactory;
import org.apache.commons.transaction.memory.TransactionalMapWrapper;
import org.apache.commons.transaction.util.CommonsLoggingLogger;
import org.apache.commons.transaction.util.LoggerFacade;

import com.haina.beluga.Config;

/**
 * 文件系统事务类。<br/>
 * @author huangyongqiang
 * //@Version 1.0
 * @since 1.0
 * @date 2009-07-20
 */
public class FileSystemTransaction {

	private static final Log LOG=LogFactory.getLog(FileSystemTransaction.class);
	
	private static final LoggerFacade LOGGER_FACADE = new CommonsLoggingLogger(LOG);
	
	private static final HashMapFactory HASH_MAP_FACTORY=new HashMapFactory();
	
	private static final String ROOT_FLODER=Config.getGlobalConfig("com.haina.beluga.global.file.base.path");
	
	private static final String WORK_FLODER=ROOT_FLODER+"/";
	
	private static final String TEMP_CACHE_FLODER="/data/temp/fst";
	
	private FileResourceManager fileResourceManager;
	
	private String transactionId;
	
	private boolean activated;
	
	private boolean running;
	
	private String workPath;
	
	private String tempPath;
	
	static {
		File file=new File(ROOT_FLODER);
		if(!file.exists()) {
			file.mkdirs();
		}
	}
	
	public FileSystemTransaction(String workFloder, String tempFloder) throws ResourceManagerSystemException {
		workPath=ROOT_FLODER+workFloder;
		tempPath=ROOT_FLODER+tempFloder;
		fileResourceManager=new FileTxManager(workPath, tempPath, false, LOGGER_FACADE);
		transactionId=fileResourceManager.generatedUniqueTxId();
		fileResourceManager.start();
		activated=true;
		
		File file=new File(workPath);
		if(!file.exists()) {
			file.mkdirs();
		}
		
		file=new File(tempPath);
		if(!file.exists()) {
			file.mkdirs();
		}
	}

	public void begin() throws ResourceManagerException {
		fileResourceManager.startTransaction(this.transactionId);
		running=true;
	}
	
	public void commit() throws ResourceManagerException {
		fileResourceManager.commitTransaction(this.transactionId);
	}
	
	public void rollback() throws ResourceManagerException {
		//fileResourceManager.recover();
		fileResourceManager.rollbackTransaction(this.transactionId);
	}

	/**
	 * 删除文件。<br/>
	 * @param fileName 文件名
	 * @throws ResourceManagerException
	 */
	public void deleteFile(String fileName) throws ResourceManagerException {
		if(checkFile(fileName,true)) {
			fileResourceManager.deleteResource(this.transactionId, fileName);
		}
	}
	
	/**
	 * 生成文件。<br/>
	 * @param fileName 文件名
	 * @param fileContent 文件内容
	 * @param overwrite 如果存在是否覆盖
	 * @throws ResourceManagerException 
	 */
	public void createFile(String fileName,byte[] fileContent,boolean overwrite) throws ResourceManagerException {
		boolean existent=checkFile(fileName,true);
		if(existent) {
			if(overwrite) {
				fileResourceManager.deleteResource(this.transactionId, fileName);
			} else {
				return;
			}
		}
		fileResourceManager.createResource(this.transactionId, fileName);
	}
	
	public void moveFile(String fromFileAbsPath, String toFileAbsPath, boolean overwrite) {
		if(checkFile(fromFileAbsPath,true)) {
			
		}
	}
	
	public void copyFile(String fromFileAbsPath, String toFileAbsPath, boolean overwrite) {
		
	}

	/**
	 * 检查是否是文件。<br/>
	 * @param fileAbsPath 文件的绝对路径
	 * @return existent 是否要求存在的文件
	 */
	public static boolean checkFile(String fileAbsPath, boolean existent) {
		if(null!=fileAbsPath && fileAbsPath.trim().length()>0) {
			File file=new File(fileAbsPath);
			if(file.isAbsolute() && file.isFile()) {
				if(existent) {
					return file.exists();
				} else {
					return true;
				}
			}
		}
		return false;
	}
	
	/**
	 * 检查是否是文件夹。<br/>
	 * @param floderAbsPath 文件的绝对路径
	 * @return existent 是否要求存在的文件
	 */
	public static boolean checkFloder(String floderAbsPath, boolean existent) {
		if(null!=floderAbsPath && floderAbsPath.trim().length()>0) {
			File file=new File(floderAbsPath);
			if(file.isAbsolute() && file.isDirectory()) {
				if(existent) {
					return file.exists();
				} else {
					return true;
				}
			}
		}
		return false;
	}
	
	private static void init() throws IOException {
		File file=new File(TEMP_CACHE_FLODER);
		if(file.exists()) {
			clearFile(file);
		}
		file.mkdir();
	}
	
	public static void clearFile(File file) {
		if(file.exists()) {
			if(file.isDirectory()) {
				File[] files=file.listFiles();
				if(files!=null && files.length>0) {
					for(File f:files) {
						clearFile(f);
					}
				}
			}
			file.delete();
		}
	}
	
	public static void main(String[] args) {
		try {
			init();
		} catch (IOException e) {
			e.printStackTrace();
		}
		//memoryFileTransaction();
		commonFileTransaction();

	}
	
	@SuppressWarnings("unchecked")
	public static void memoryFileTransaction() {
		Map<Integer, String> map=(Map<Integer, String>)HASH_MAP_FACTORY.createMap();
		TransactionalMapWrapper mapWrapper=new TransactionalMapWrapper(map);
		try {
			mapWrapper.startTransaction();
			for(int i=0;i<100;i++) {
				mapWrapper.put(i, "第"+i+"个字符串");
				if(0==i%5) {
					mapWrapper.remove(i-3);
				}
				LOG.info("现在共有"+map.size()+"个字符串");
				LOG.info("时间"+System.currentTimeMillis());
			}
			mapWrapper.commitTransaction();
		} catch (Exception e) {
			e.printStackTrace();
			mapWrapper.rollbackTransaction();
		} finally {
			LOG.info("最后共有"+map.size()+"个字符串");
			LOG.info("时间"+System.currentTimeMillis());
		}
		
	}
	
	public static void commonFileTransaction() {
		//构造函数的第三个参数：false，标识是否encoding文档的url，这个一般不需要设置为true
		//FileResourceManager frm = new FileResourceManager("/var", TEMP_CACHE_FLODER, false, LOGGER_FACADE);
		FileResourceManager frm = new FileTxManager(WORK_FLODER, TEMP_CACHE_FLODER, false, LOGGER_FACADE);
		String firstTxId = null;
		//String secondTxId = null;
		try {
			//这标识frm的状态为start
			frm.start();
			
			//取得事务的唯一标识，需要frm的状态为start
			//firstTxId = frm.generatedUniqueTxId();
			firstTxId = UUID.randomUUID().toString();
			//secondTxId = frm.generatedUniqueTxId();
			
			System.out.println(">>>>>>>>>>>>>>>>>> "+firstTxId+ " >>>>>>>>>>>>>>>>>> ");
			frm.startTransaction(firstTxId);
			frm.deleteResource(firstTxId, "/fst/1.txt");
			frm.deleteResource(firstTxId, "/fst/2.txt");
			//frm.createResource(txId, resourceId);
			String n=String.valueOf(System.currentTimeMillis());
			frm.copyResource(firstTxId, "/fst/修改的文件.txt", "/test/fst/"+n+".id", true);
			frm.deleteResource(firstTxId, "/fst/3.txt");
			frm.createResource(firstTxId, "/fst/"+n+".bin");
			//提交事务
			frm.commitTransaction(firstTxId);
			//throw new ResourceManagerException(firstTxId+" 事务产生异常 ");
			//frm.rollbackTransaction(firstTxId);
		} catch (ResourceManagerSystemException e) {
			e.printStackTrace();
			try {
		        frm.rollbackTransaction(firstTxId);
		        //frm.stop(ResourceManager.SHUTDOWN_MODE_KILL);
		    } catch (Exception e1) {
		        e1.printStackTrace();
		    }
		} catch (ResourceManagerException e) {
			e.printStackTrace();
			try {
		        frm.rollbackTransaction(firstTxId);
		        //frm.stop(ResourceManager.SHUTDOWN_MODE_KILL);
		    } catch (Exception e1) {
		        e1.printStackTrace();
		    }
		} finally {
			try {
				frm.stop(ResourceManager.SHUTDOWN_MODE_KILL);
			} catch (ResourceManagerSystemException e) {
				e.printStackTrace();
			}
		}
	}

	public boolean isActivated() {
		return activated;
	}

	public boolean isRunning() {
		return running;
	}
}
