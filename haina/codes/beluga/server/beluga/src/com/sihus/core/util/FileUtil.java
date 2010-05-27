package com.sihus.core.util;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileFilter;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.URL;
import java.nio.CharBuffer;
import java.util.ArrayList;
import java.util.List;

public final class FileUtil {
	
	private static final CommonLog COMMLOG_LOG=CommonLog.getLog(FileUtil.class);
	
	public static final int DEFAULT_FILE_IO_BUFFER_SIZE=8192;

	/**
	 * 读取文件。<br/>
	 * 
	 * @param fileFullPathName
	 *            文件完整路径
	 * @return
	 */
	public static String readFileForString(String fileFullPathName) {
		if (StringUtils.isNull(fileFullPathName)) {
			return "";
		}
		FileInputStream fis = null;
		File file = null;
		try {
			file = new File(fileFullPathName);
			fis = new FileInputStream(file);
			byte[] buf = new byte[DEFAULT_FILE_IO_BUFFER_SIZE];
			int i = fis.read(buf);
			StringBuffer buffer=new StringBuffer();
			while(i>0) {
				buffer.append(new String(buf));
				fis.read(buf);
			}
			return buffer.toString();
		} catch (Exception ex) {
			COMMLOG_LOG.error("读取文件{0}出错", ex,
					new Object[] { fileFullPathName });
			return "";
		} finally {
			if (fis != null) {
				try {
					fis.close();
				} catch (IOException e) {
					COMMLOG_LOG.error("关闭文件{0}输入流流出错", e,
							new Object[] { fileFullPathName });
				}
				fis = null;
			}
			file = null;
		}
	}

	/**
	 * 读取文件。<br/>
	 * 
	 * @param fileFullPathName
	 *            文件完整路径
	 * @return
	 */
	public static byte[] readFileForByte(String fileFullPathName) {
		if (StringUtils.isNull(fileFullPathName)) {
			return null;
		}
		FileInputStream fis = null;
		File file = null;
		try {
			file = new File(fileFullPathName);
			fis = new FileInputStream(file);
			byte[] buf = new byte[DEFAULT_FILE_IO_BUFFER_SIZE];
			int i = fis.read(buf);
			byte[] ret=new byte[0];
			byte[] temp=null;
			while(i>0) {
				temp=ret;
				ret=new byte[temp.length+i];
				System.arraycopy(temp, 0, ret, 0, temp.length);
				System.arraycopy(buf, 0, ret, temp.length, temp.length);
			}
			return ret;
		} catch (Exception ex) {
			COMMLOG_LOG.error("读取文件{0}出错", ex,
					new Object[] { fileFullPathName });
			return null;
		} finally {
			if (fis != null) {
				try {
					fis.close();
				} catch (IOException e) {
					COMMLOG_LOG.error("关闭文件{0}输入流流出错", e,
							new Object[] { fileFullPathName });
				}
				fis = null;
			}
			file = null;
		}
	}
	
	/**
     * 向文件写内容。<br/>
     * @param fileFullPathName 文件完整路径
     * @param content 写入的内容
     * @param cover 是否覆盖存在的文件
     */
    public static void writeFile(String fileFullPathName, byte[] content,boolean cover,boolean trimCRLF) {
        /*Logger log = Logger.getLogger("FileUtils.writeFile");*/
    	if(StringUtils.isNull(fileFullPathName) || null==content 
    			|| 0==content.length) {
    		return;
    	}
    	writeFile(fileFullPathName, new String(content),cover,trimCRLF);
    }

    public static void writeFile(String fileFullPathName, String content,boolean cover,boolean trimCRLF) {
    	writeFile(fileFullPathName,content,Constants.DEFAULT_CHARSET,cover,trimCRLF);
    }

    public static synchronized void writeFile(String fileFullPathName, String content,String encoding,boolean cover,boolean trimCRLF) {
    	if(StringUtils.isNull(fileFullPathName)
    			|| StringUtils.isNull(content)
    			|| StringUtils.isNull(encoding)) {
    		return;
    	}
    	createPath(fileFullPathName,cover,trimCRLF);
    	PrintWriter pw=null;
        try {
        	pw = new PrintWriter(fileFullPathName,encoding.trim());
        	pw.write(content);
            pw.close();
            //fos = new FileOutputStream(fileFullPathName);
            //fos.write(content);
            //fos.flush();
            //fos.close();
        } catch (Exception ex) {
        	COMMLOG_LOG.error("写文件{0}出错", ex,new Object[]{fileFullPathName});
        } finally {
        	/*
        	if(fos!=null) {
                try {
                	fos.close();
                } catch (IOException e) {
                	
                }
                fos=null; 
            }
            */
        	if(pw!=null) {
        		pw.close();
                pw=null; 
            }
        }
    }
    
    /**
     * 生成文件路径
     * @param fullFileName 文件路径
     * @param cover 是否覆盖
     */
    public static synchronized void createPath(String fullFileName,boolean cover,boolean trimCRLF) {
    	if(StringUtils.isNull(fullFileName)) {
    		return;
    	}
    	 String path = null;
         File filePath=null;
        try {
            int index = 0;
            index = fullFileName.lastIndexOf("/");
            if (index >= 0) {
                path = fullFileName.substring(0, index);
                filePath = new File(path);
                if(!filePath.exists()) {
                	filePath.mkdirs();
                }
                createFile(fullFileName,Constants.DEFAULT_CHARSET,cover,trimCRLF);
            }
        } catch (Exception e) {
        	COMMLOG_LOG.error("创建文件{0}出错", e,new Object[]{fullFileName});
        }  finally {
        	 path=null;
             filePath=null;
        }
    }

    public static void createFile(String fullFileName,String charset,boolean cover,boolean trimCRLF) {
    	if(StringUtils.isNull(fullFileName) || StringUtils.isNull(charset)) {
    		return;
    	}
    	createFile(fullFileName,"",charset,cover,trimCRLF);
    }
    
    public static synchronized boolean createFile(String fullFileName,String fileContent,String charset,boolean cover,boolean trimCRLF) {
    	if(StringUtils.isNull(fullFileName) 
    			|| StringUtils.isNull(charset)
    			|| StringUtils.isNull(fileContent)) {
    		return false;
    	}
    	File file =null;
        String path=null;
        PrintWriter pw = null;
        try {           
        	int index = 0;
            index = fullFileName.lastIndexOf("/");
            if (fullFileName.length()==index) {/*目录路径，不是文件路径。*/
               return false;
            } else {
            	 path = fullFileName.substring(0, index);
                 file = new File(path);
                 if(!file.exists()) {
                	 file.mkdirs();
                 }
                 file = new File(fullFileName);
                 if(file.exists() && cover) {
                	 file.delete();
                 }
                 pw = new PrintWriter(fullFileName,charset);
                 if(trimCRLF) {
                	 fileContent=fileContent.replace("\r", "").replace("\n", "");
                 }
                 pw.write(fileContent);
                 return true;
            }
        } catch (Exception e) {
        	COMMLOG_LOG.error("创建文件{0}出错", e,new Object[]{fullFileName});
        	return false;
        } finally {
        	if(pw!=null) {
        		pw.close();
        		pw=null;
        	}
        }
    }
    
    public static void createFile(String fullFileName,InputStream in,boolean trimCRLF) {
    	createFile(fullFileName,in,Constants.DEFAULT_CHARSET,trimCRLF);
    }
    
    public static synchronized void createFile(String fullFileName,InputStream in,String charset,boolean trimCRLF) {
    	if(StringUtils.isNull(fullFileName) 
    			|| StringUtils.isNull(charset) || null==in) {
    		return;
    	}
    	File file =null;
        String temp=null;
		//PrintWriter pw = null;
        OutputStream pw = null;
		BufferedReader breader = null;
		StringBuffer content = new StringBuffer();
		fullFileName.replace("\\", "/");
		try {
			int index = fullFileName.lastIndexOf("/");
            if (fullFileName.length()==index) {/*目录路径，不是文件路径。*/
               return;
            } else {
            	temp = fullFileName.substring(0, index);
            	file = new File(temp);
            	if(!file.exists()) {
            		file.mkdirs();
            	}
            	file = new File(fullFileName);
            	if(file.exists()) {
            		file.delete();
            	}
            	//pw = new PrintWriter(fullFileName,charset.trim());
            	pw = new FileOutputStream(fullFileName);
    			breader = new BufferedReader(new InputStreamReader(in,charset.trim())); 
    			while((temp=breader.readLine())!=null){
    				content.append(temp); 
    		    }
    			temp=content.toString();
    			if(trimCRLF) {
    				temp=temp.replace("\r", "").replace("\n", "");
    			}
    			byte[] outByte=temp.getBytes(charset);
    			COMMLOG_LOG.info("取得页面流{0} 的实际大小{1}", new Object[]{in,outByte.length});
    			pw.write(outByte);
    		    pw.close();
            }
		    /*
			reader = new InputStreamReader(in ,charset.trim());
			//out = new OutputStreamWriter(new FileOutputStream(fullFileName.trim()),charset.trim());
			int charsRead = 0;
			char[] buffer = new char[DEFAULT_FILE_IO_BUFFER_SIZE];
			while ((charsRead = reader.read(buffer, 0, DEFAULT_FILE_IO_BUFFER_SIZE)) != -1) {
				out.write(buffer, 0, charsRead);
			}
			out.flush();
			out.close();
			reader.close();
			*/
		} catch (FileNotFoundException fnfEx) {
			COMMLOG_LOG.error("文件{0}不存在",fnfEx,new Object[]{fullFileName});
		} catch (IOException ioEx) {
			COMMLOG_LOG.error("创建文件{0}出错",ioEx,new Object[]{fullFileName});
		}	
		pw = null;
		breader = null;
		temp=null;
		content = null;
		file =null;
    }
    
    public static synchronized boolean createFile(String fullFileName,URL urlPath,String charset,boolean trimCRLF) {
    	if(StringUtils.isNull(fullFileName) 
    			|| StringUtils.isNull(charset) || null==urlPath) {
    		return false;
    	}
//    	HttpURLConnection connection=null;
    	InputStream in = null;
    	BufferedReader reader=null;
		try {
//			connection = (HttpURLConnection)urlPath.openConnection();
//			connection.connect();
//			Map<String,List<String>> fields=connection.getHeaderFields();
//			Iterator<String> fieldsKeys=fields.keySet().iterator();
//			while(fieldsKeys.hasNext()) {
//				String key=fieldsKeys.next();
//				COMMLOG_LOG.info("取得页面{0} header字段{1}", new Object[]{urlPath.toString(),key});
//				List<String> list=fields.get(key);
//				if(list!=null && list.size()>0) {
//					for(int i=0;i<list.size();i++) {
//						COMMLOG_LOG.info("header字段{0}里第{1}个值 {2}", new Object[]{key,i+1,list.get(i)});
//					}
//				} else {
//					COMMLOG_LOG.info("header字段{0}没有值", new Object[]{key});
//				}
//			}
//			in = connection.getInputStream();
			
			in=urlPath.openStream();
			reader=new BufferedReader(new InputStreamReader(in, charset));
			CharBuffer bos = CharBuffer.allocate(20480);
		    //int read = 0; 
		    StringBuilder builder = new StringBuilder();
		    while (reader.read(bos) != -1) {   
                bos.flip();   
                builder.append(bos.toString());   
            }
		    return createFile(fullFileName,builder.toString(),charset,true,trimCRLF);
		} catch (IOException ioEx) {
			COMMLOG_LOG.error("使用地址{0}写文件{1}出错",ioEx,new Object[]{urlPath.getPath(),fullFileName});
			return false;
		} finally {
			if(reader!=null) {
				try {
					reader.close();
				} catch (IOException e) {
					COMMLOG_LOG.error("关闭输入流",e);
				}
			}
			if(in!=null) {
				try {
					in.close();
				} catch (IOException e) {
					COMMLOG_LOG.error("关闭输入流",e);
				}
			}
//			if(connection!=null) {
//				connection.disconnect();
//			}
		}
    }
    
	public static synchronized int moveFile(String fullFileName,String filePath1,String filePath2){
		if(StringUtils.isNull(fullFileName) 
				|| StringUtils.isNull(filePath1) || StringUtils.isNull(filePath2)) {
			return 0;
		}
		File file=new File(filePath1,fullFileName);
		File folder=null;
		File newFile=null;
		if(file.exists()){
			folder=new File(filePath2);
			if(!folder.exists()) {
				folder.mkdirs();
			}
			newFile=new File(filePath2,fullFileName);
			if(file.renameTo(newFile)) { 
				newFile=null;
				file=null;
				folder=null;
				return 1;
			} else {
				newFile=null;
				file=null;
				folder=null;
				return -1;
			}
		} else {
			file=null;
			return 0;
		}
		
	}
	
	public static synchronized int moveFile(String filePath1,String filePath2){
		if(StringUtils.isNull(filePath2) || StringUtils.isNull(filePath2)) {
			return 0;
		}
		File file=new File(filePath1);
		File newFile=null;
		File folder=null;
		if(file.exists()){
			newFile=new File(filePath2);
			folder=new File(newFile.getParent());
			if(!folder.exists()) {
				folder.mkdirs();	
			}
			if(file.renameTo(newFile)) {
				file=null;
				newFile=null;
				folder=null;
				return 1;
			} else {
				file=null;
				newFile=null;
				folder=null;
				return -1;
			}
		} else {
			file=null;
			return 0;
		}
	}	
	
	public static int copyFile(String fullFileName,String filePath1,String filePath2){
		if(StringUtils.isNull(fullFileName) 
				|| StringUtils.isNull(filePath1) || StringUtils.isNull(filePath2)) {
			return 0;
		}
		File file=new File(filePath1,fullFileName);
		File newFile=null;
		FileInputStream fin=null;
		FileOutputStream fout=null;
		try{
			if(file.exists()){
				newFile=new File(filePath2,fullFileName);
				if(!newFile.getParentFile().exists()) {
					newFile.getParentFile().mkdirs();
				}

				byte[] buffer = new byte[DEFAULT_FILE_IO_BUFFER_SIZE];
				int bytesRead = 0;
				fin = new FileInputStream(file);
				fout=new FileOutputStream(newFile);
				while ((bytesRead = fin.read(buffer)) != -1) {
					fout.write(buffer, 0, bytesRead);
				}				
				fin.close();
				fout.close();
				
				fin=null;
				fout=null;
				newFile=null;
				file=null;
				return 1;
			} else {
				file=null;
				return 0;
			}
		}catch(Exception e){
			COMMLOG_LOG.error("拷贝{0}出错",e,new Object[]{fullFileName});
			return -1;
		} finally {
			if(fout!=null) {
                try {
                	fout.close();
                } catch (IOException e) {
                	COMMLOG_LOG.error("关闭文件{0}输出流出错",e,new Object[]{fullFileName});
                }
                fout=null; 
            }
			if(fin!=null) {
                try {
                	fin.close();
                } catch (IOException e) {
                	COMMLOG_LOG.error("关闭文件{0}输入流出错",e,new Object[]{fullFileName});
                }
                fin=null; 
            }
			newFile=null;
			file=null;
		}
	}
	
	public static synchronized void deleteFile(String filePathName){
		if(StringUtils.isNull(filePathName)) {
			return;
		}
		File file=new File(filePathName);
		if(file.isFile() && file.exists()){
			file.delete();
		}
		file=null;
	}
	
	public static synchronized void deleteFile(File file){
		if(!file.exists()) {
			return;
		}
		if(file.isFile()){
			file.delete();
		} else{
			File[] files=file.listFiles();
			for(int i=0;i<files.length;i++){
				deleteFile(files[i]);
			}
			file.delete();
			files=null;
		}		
	}
	
	/**
	 * 删除目录，如果目录下还存在目录的话同样删除
	 * @param path	要删除的目录
	 * @param flag	true 如果目录中存在文件的话同样删除目录 false 如果目录中存在文件则不删除目录
	 */
	public static synchronized boolean deletePath(String path,boolean flag){
		if(StringUtils.isNull(path)) {
			return true;
		}
		File file = new File(path);
		if(file.exists()){
			File[] files = file.listFiles();
	
			for(int i=0;i<files.length;i++){
				if(files[i].isDirectory()) {
					if(!deletePath(files[i].toString(),flag)) {
						files=null;
						file=null;
						return false;
					}
				}
				if(files[i].isFile()) {
					if(flag) {
						files[i].delete();
					} else {
						files=null;
						file=null;
						return false;
					}
				}
			}
			files=null;
			file.delete();
		}
		file=null;
		return true;
	}	
    

	/**
	 * 删除目录，如果目录下还存在目录的话同样删除
	 * @param path	要删除的目录
	 */
	public static synchronized void deleteFolder(String path){
		File folder = new File(path);
		if(folder.exists()){
			File[] files = folder.listFiles();
			for(int i=0;i<files.length;i++){
				if(files[i].isFile())
					files[i].delete();
				if(files[i].isDirectory())
					deleteFolder(files[i].getPath());
			}
			files=null;
			folder.delete();
		}
		folder=null;
	}
	
	public static List<File> getListFile(String path) throws Exception {
    	if(StringUtils.isNull(path)) {
			return null;
		}
        List<File> fileList = new ArrayList<File>();

        listFile(path, fileList);

        return fileList;
    }

    private static void listFile(String path, List<File> fileList) throws Exception {
    	if(StringUtils.isNull(path)
    			|| fileList==null || fileList.size()<1) {
			return;
		}
        File file = new File(path);
	    File list[] = file.listFiles(new FileFilter() {
	            public boolean accept(File pathname) {
	                String tmp = pathname.getName().toLowerCase();
	                if ((pathname.isDirectory()) || (tmp.endsWith(".xml"))) {
	                    return true;
	                }
	                return false;
	            }
	        }
        );
        for (int i = 0; i < list.length; i++) {
            if (list[i].isDirectory()) {
                listFile(list[i].toString(), fileList);
            } else {
                fileList.add(list[i]);
            }
        }
    }
}