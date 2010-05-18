package com.haina.beluga.webservice;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.io.Serializable;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.springframework.beans.factory.InitializingBean;
import org.springframework.web.HttpRequestHandler;

import com.haina.beluga.webservice.data.hessian.HessianRemoteReturning;
import com.haina.beluga.webservice.flexjson.JSONSerializer;
import com.sihus.core.util.StringUtils;


public class OUServiceExporter implements HttpRequestHandler, InitializingBean {

	private static final long serialVersionUID = 6735390662011813417L;

	private String serverAPI;
	private Serializable serverImpl;
	private OUSkeleton uuSkeleton;
	private String method;
	
	private JSONSerializer jsonSerializer;
	private String passportName;

	private Map<String, List<String>> validatedMethod;

	public OUServiceExporter() {

	}

	@Override
	public void handleRequest(HttpServletRequest req, HttpServletResponse res)
			throws ServletException, IOException {
		req.setCharacterEncoding("UTF-8");
		res.setCharacterEncoding("UTF-8");
		res.setContentType("text/html;charset=UTF-8");
		if (!"POST".equals(req.getMethod())) {
			res.setStatus(500);
			PrintWriter out = res.getWriter();
			res.setContentType("text/html");
			out.println("<h1>OUCenter -OU中心 Requires POST</h1>");
			return;
		}
		InputStream is = req.getInputStream();
		OutputStream os = res.getOutputStream();
		String _method = req.getParameter(getMethod());
		MicroHessianInput hessianInput = new MicroHessianInput(is);
		// hessianInput.readString();
		MicroHessianOutput hessianOutput = new MicroHessianOutput(os);
		String urlPath=this.getRequestPath(req);
		boolean need=this.needLogin(_method, urlPath);
		if(need) {
			String passport=req.getParameter(passportName);
			if(StringUtils.isNull(passport)) {
				HessianRemoteReturning ret = new HessianRemoteReturning();
				ret.setStatusCode(IStatusCode.INVALID_LOGIN_PASSPORT);
				String rs=jsonSerializer.deepSerialize(ret);
				hessianOutput.writeString(rs);
				return ;
			}
		}
		uuSkeleton.invoke(_method, hessianInput, hessianOutput);

	}

	private Class<?> loadClass(String className) throws ClassNotFoundException {

		ClassLoader loader = Thread.currentThread().getContextClassLoader();
		if (loader != null)
			return Class.forName(className, false, loader);
		else
			return Class.forName(className);
	}

	public OUSkeleton getUuSkeleton() {
		return uuSkeleton;
	}

	public void setUuSkeleton(OUSkeleton uuSkeleton) {
		this.uuSkeleton = uuSkeleton;
	}

	public String getServerAPI() {
		return serverAPI;
	}

	public void setServerAPI(String serverAPI) {
		this.serverAPI = serverAPI;
	}

	public Serializable getServerImpl() {
		return serverImpl;
	}

	public void setServerImpl(Serializable serverImpl) {
		this.serverImpl = serverImpl;
	}

	public String getMethod() {
		return method;
	}

	public void setMethod(String method) {
		this.method = method;
	}

	public void setPassportName(String passportName) {
		this.passportName = passportName;
	}
	
	public void setValidatedMethod(Map<String, List<String>> validatedMethod) {
		this.validatedMethod = validatedMethod;
	}

	@Override
	public void afterPropertiesSet() throws Exception {
		// Class<?> homeClass = loadClass(getServerImpl());
		uuSkeleton = new OUSkeleton(getServerImpl(), loadClass(getServerAPI()));
		jsonSerializer=new JSONSerializer();
		if(null==validatedMethod) {
			validatedMethod=new HashMap<String, List<String>>(0);
		}
//		if(validatedMethod.size()>0) {
//			Iterator<String> keys=validatedMethod.keySet().iterator();
//			while(keys.hasNext()) {
//				String key=keys.next();
//				System.out.println("*************path: "+key+" ***************");
//				List<String> value=validatedMethod.get(key);
//				for(String m:value) {
//					System.out.println("*************method: "+m+" ***************");
//				}
//			}
//		}

	}

	/**
	 * 取得从contextpath开始的url路径。<br/>
	 * 
	 * @param request
	 * @return
	 */
	private String getRequestPath(HttpServletRequest request) {
		String ret = null;
		if (null != request) {
			String contextPath = request.getContextPath();
			String urlPath = request.getRequestURI();
			if (contextPath.length() > 0) {
				ret = urlPath.substring(contextPath.length(), urlPath.length());
			} else {
				ret = urlPath;
			}
		} else {
			ret = "";
		}
		return ret;
	}
	
	/**
	 * 检测是否调用的方法是否需要登录。<br/>
	 * @param hessianInput
	 * @param hessianOutput
	 * @param method
	 * @param urlPath
	 * @return
	 */
	private boolean needLogin(String method,String urlPath) {
		if(StringUtils.isNull(method) || StringUtils.isNull(urlPath)) {
			return false;
		}
		if(urlPath!=null && urlPath.length()>0) {
			if(validatedMethod!=null && validatedMethod.size()>0) {
				Iterator<String> paths=validatedMethod.keySet().iterator();
				while(paths.hasNext()) {
					String path=paths.next();
					if(path!=null && path.equals(urlPath)) {
						List<String> methods=validatedMethod.get(path);
						if(methods!=null && methods.size()>0) {
							for(int i=0;i<methods.size();i++) {
								String m=methods.get(i);
								if(m!=null && m.equals(method)) {
									return true;
								}
							}
						}
					}
				}
			}
		}
		return false;
	}
}
