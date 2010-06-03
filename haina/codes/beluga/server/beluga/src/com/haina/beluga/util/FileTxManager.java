package com.haina.beluga.util;

import org.apache.commons.transaction.file.FileResourceManager;
import org.apache.commons.transaction.file.ResourceIdToPathMapper;
import org.apache.commons.transaction.file.TransactionIdToPathMapper;
import org.apache.commons.transaction.util.LoggerFacade;

public class FileTxManager extends FileResourceManager {

	public FileTxManager(String storeDir, String workDir,
			boolean urlEncodePath, LoggerFacade logger) {
		super(storeDir, workDir, urlEncodePath, logger);
	}

	public FileTxManager(String storeDir, String workDir,
			boolean urlEncodePath, LoggerFacade logger, boolean debug) {
		super(storeDir, workDir, urlEncodePath, logger, debug);
	}

	public FileTxManager(String storeDir, String workDir,
			ResourceIdToPathMapper idMapper, LoggerFacade logger, boolean debug) {
		super(storeDir, workDir, idMapper, logger, debug);
	}

	public FileTxManager(String storeDir, String workDir,
			ResourceIdToPathMapper idMapper,
			TransactionIdToPathMapper txIdMapper, LoggerFacade logger,
			boolean debug) {
		super(storeDir, workDir, idMapper, txIdMapper, logger, debug);
	}

//	@Override
//	protected String getDeletePath(Object txId, Object path) {
//		// String txBaseDir = getTransactionBaseDir(txId);
//		// StringBuffer buf = new StringBuffer(txBaseDir.length() +
//		// path.toString().length()
//		// + WORK_DELETE_DIR.length() + 5);
//		// buf.append(txBaseDir).append('/').append(WORK_DELETE_DIR).append(assureLeadingSlash(path));
//		// return buf.toString();
//		return path != null ? assureLeadingSlash(path.toString()) : "";
//	}
//
//	@Override
//	protected String getChangePath(Object txId, Object path) {
//		// String txBaseDir = getTransactionBaseDir(txId);
//		// StringBuffer buf = new StringBuffer(txBaseDir.length() +
//		// path.toString().length()
//		// + WORK_CHANGE_DIR.length() + 5);
//		// buf.append(txBaseDir).append('/').append(WORK_CHANGE_DIR).append(assureLeadingSlash(path));
//		// return buf.toString();
//		return path != null ? assureLeadingSlash(path.toString()) : "";
//	}
//
//	@Override
//	protected String getMainPath(Object path) {
//		// StringBuffer buf = new StringBuffer(storeDir.length() +
//		// path.toString().length() + 5);
//		// buf.append(storeDir).append(assureLeadingSlash(path));
//		return path != null ? assureLeadingSlash(path.toString()) : "";
//	}
//
//	@Override
//	protected String assureLeadingSlash(Object pathObject) {
//		String path = "";
//		if (pathObject != null) {
//			if (idMapper != null) {
//				path = idMapper.getPathForId(pathObject);
//			} else {
//				path = pathObject.toString();
//			}
//			// if (path.length() > 0 && path.charAt(0) != '/' && path.charAt(0)
//			// != '\\') {
//			// path = "/" + path;
//			// }
//		}
//		return path;
//	}
//
//	protected String getPathForRead(Object txId, Object resourceId)
//			throws ResourceManagerException {
//		String mainPath = getMainPath(resourceId);
////		String txChangePath = getChangePath(txId, resourceId);
////		String txDeletePath = getDeletePath(txId, resourceId);
////
////		// now, this gets a bit complicated:
////
////		boolean changeExists = FileHelper.fileExists(txChangePath);
////		boolean deleteExists = FileHelper.fileExists(txDeletePath);
//		boolean mainExists = FileHelper.fileExists(mainPath);
//		//boolean resourceIsDir = ((mainExists && new File(mainPath).isDirectory()) || (changeExists && new File(txChangePath).isDirectory()));
//		boolean resourceIsDir = ((mainExists && new File(mainPath).isDirectory()));
//		if (resourceIsDir) {
//			logger.logWarning("Resource at '" + resourceId + "' maps to directory");
//		}
//
//		// first do some sane checks
//
//		// this may never be, two cases are possible, both disallowing to have a
//		// delete together with a change
//		// 1. first there was a change, than a delete -> at least delete file
//		// exists (when there is a file in main store)
//		// 2. first there was a delete, than a change -> only change file exists
////		if (!resourceIsDir && changeExists && deleteExists) {
////			throw new ResourceManagerSystemException(
////					"Inconsistent delete and change combination for resource at '"
////							+ resourceId + "'", ERR_TX_INCONSISTENT, txId);
////		}
//
//		// you should not have been allowed to delete a file that does not exist
//		// at all
////		if (deleteExists && !mainExists) {
////			throw new ResourceManagerSystemException(
////					"Inconsistent delete for resource at '" + resourceId + "'",
////					ERR_TX_INCONSISTENT, txId);
////		}
////		if (changeExists) {
////			return txChangePath;
////		} else if (mainExists && !deleteExists) {
////			return mainPath;
////		} else {
////			return null;
////		}
//		if (mainExists) {
//			return mainPath;
//		} else {
//			return "";
//		}
//
//	}
}
