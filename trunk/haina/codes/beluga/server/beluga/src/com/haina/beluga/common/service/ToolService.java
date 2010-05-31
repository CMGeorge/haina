package com.haina.beluga.common.service;

import org.springframework.stereotype.Service;

import com.sihus.core.util.FileUtil;

/**
 * 工具操作业务接口实现类。<br/>
 * @author huangyongqiang
 * @since 2010-05-31
 */
@Service(value="toolService")
public class ToolService implements IToolService {

	@Override
	public byte[] getLocalFileForByte(String filePath) {
		return FileUtil.readFileForByte(filePath);
	}

	@Override
	public String getLocalFileForString(String filePath) {
		return FileUtil.readFileForString(filePath);
	}

	@Override
	public byte[] geFtpFileForByte(String filePath) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public byte[] geFtpFileForString(String filePath) {
		// TODO Auto-generated method stub
		return null;
	}
}
